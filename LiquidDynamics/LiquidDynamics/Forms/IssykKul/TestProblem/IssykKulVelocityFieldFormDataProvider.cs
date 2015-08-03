﻿using System.Drawing;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using BarotropicComponentProblem.TestProblem;
using Common;
using ControlLibrary.Types;
using LiquidDynamics.Forms.IssykKul.Wind;
using LiquidDynamics.Forms.TestProblem;
using LiquidDynamics.Properties;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using Point3D = Mathematics.MathTypes.Point3D;
using Rectangle3D = Mathematics.MathTypes.Rectangle3D;
using Vector = ControlLibrary.Types.Vector;

namespace LiquidDynamics.Forms.IssykKul.TestProblem
{
   internal sealed class IssykKulVelocityFieldFormDataProvider
   {
      private const double Dz = 0.01;

      private ProblemParameters _problemParameters;

      private Mathematics.Numerical.Grid _x;
      private Mathematics.Numerical.Grid _y;
      private IssykKulGrid3D _grid;
      private Bounds _bounds;
      
      private TestProblemSolver _solver;

      public Solution Reset(int n, int m, double tau, double theta, double sigma, double delta, int k, Parameters parameters, WindParameters windParameters)
      {
         Check.NotNull(parameters, "parameters");
         Check.NotNull(windParameters, "windParameters");

         Point3D[] data = IssykKulDataReader.ReadData(Resources.IssykKulData);

         double xMin, xMax, yMin, yMax;
         BoundsCalculator.Calculate(data, out xMin, out xMax, out yMin, out yMax);
         
         var gridBuilder = new IssykKulGridBuilder(data, xMin, xMax, yMin, yMax);
         _grid = gridBuilder.BuildGrid3D(n, m, Dz, theta);
         _grid.Stretch(StretchCoefficients.L0, StretchCoefficients.L0, StretchCoefficients.SmallH0);

         _bounds = new Bounds((float) xMin, (float) xMax, (float) yMin, (float) yMax);

         _problemParameters = getParameters(parameters, xMax, yMax);
         _x = Mathematics.Numerical.Grid.Create(0.0, xMax / StretchCoefficients.L0, n + 1);
         _y = Mathematics.Numerical.Grid.Create(0.0, yMax / StretchCoefficients.L0, m + 1);
         
         _solver = createProblemSolver(windParameters, tau, sigma, delta, k);
         IterationMethodResult result = _solver.Begin().Barotropic;

         return new Solution(getVelocityField(result, _grid.Grid2D, _x, _y), _bounds);
      }

      public Solution Step()
      {
         IterationMethodResult result = _solver.Step().Barotropic;
         return new Solution(getVelocityField(result, _grid.Grid2D, _x, _y), _bounds);
      }

      private static ProblemParameters getParameters(Parameters parameters, double xMax, double yMax)
      {
         return new ProblemParameters
                   {
                      SmallL0 = parameters.SmallL0,
                      Beta = parameters.Beta,
                      Mu = parameters.Mu,
                      Nu = parameters.Nu,
                      Rho0 = StretchCoefficients.Rho0,
                      SmallR = xMax / StretchCoefficients.L0,
                      SmallQ = yMax / StretchCoefficients.L0
                   };
      }

      private TestProblemSolver createProblemSolver(
         WindParameters windParameters,
         double tau,
         double sigma,
         double delta,
         int k
         )
      {
         return
            new TestProblemSolver(
               _problemParameters,
               _grid,
               getWind(windParameters),
               tau,
               _x,
               _y,
               new InitialCondition(SquareGridFunction.Zeros(_x, _y),
                                    SquareGridFunction.Zeros(_x, _y)),
               getInitialTheta(),
               sigma,
               delta,
               k,
               getSurface(_grid.Grid2D)
               );
      }

      private IWind getWind(WindParameters windParameters)
      {
         return new IssykKulWind(windParameters);
      }

      private Complex[,][] getInitialTheta()
      {
         var theta = new Complex[_grid.Grid2D.N + 1, _grid.Grid2D.M + 1][];

         for (int i = 0; i < _grid.Grid2D.N; i++)
         {
            for (int j = 0; j < _grid.Grid2D.M; j++)
            {
               if (_grid.Grid2D[i, j] == GridCell.Empty)
                  continue;

               Rectangle3D[] depthGrid = _grid.GetDepthGrid(i, j);
               setTheta(theta, i, j, depthGrid.Length);
               setTheta(theta, i, j + 1, depthGrid.Length);
               setTheta(theta, i + 1, j, depthGrid.Length);
               setTheta(theta, i + 1, j + 1, depthGrid.Length);
            }
         }

         return theta;
      }

      private void setTheta(Complex[,][] theta, int i, int j, int n)
      {
         if (theta[i, j] != null)
            return;

         theta[i, j] = new Complex[n + 1];

         for (int k = 0; k < theta[i, j].Length; k++)
            theta[i, j][k] = new Complex();
      }

      private int[,] getSurface(IssykKulGrid2D grid)
      {
         var surface = new int[grid.N, grid.M];

         for (int i = 0; i < grid.N; i++)
         {
            for (int j = 0; j < grid.M; j++)
            {
               surface[i, j] = grid[i, j] == GridCell.Empty ? 0 : 1;
            }
         }

         return surface;
      }

      private SquareVelocityField getVelocityField(
         IterationMethodResult result, IssykKulGrid2D grid,
         Mathematics.Numerical.Grid x, Mathematics.Numerical.Grid y)
      {
         int n = grid.N;
         int m = grid.M;

         var vectors = result.BarotropicComponent.Vectors;
         var velocityField = new Vector[n + 1, m + 1];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (grid[i, j] == GridCell.Empty)
                  continue;

               velocityField[i, j] = getVector(vectors[i, j]);

               if (i == n - 1 || grid[i + 1, j] == GridCell.Empty)
               {
                  velocityField[i + 1, j] = getVector(vectors[i + 1, j]);
               }

               if (j == m - 1 || grid[i, j + 1] == GridCell.Empty)
               {
                  velocityField[i, j + 1] = getVector(vectors[i, j + 1]);
               }

               if ((i == n - 1 && j == m - 1) ||
                   (i == n - 1 && j < m - 1 && grid[i, j + 1] == GridCell.Empty) ||
                   (j == m - 1 && i < n - 1 && grid[i + 1, j] == GridCell.Empty) ||
                   (i < n - 1 && j < m - 1 && grid[i + 1, j + 1] == GridCell.Empty))
               {
                  velocityField[i + 1, j + 1] = getVector(vectors[i + 1, j + 1]);
               }
            }
         }

         var xStep = (float) (x.Step * StretchCoefficients.L0);
         var yStep = (float) (y.Step * StretchCoefficients.L0);

         return new SquareVelocityField(velocityField, new SizeF(xStep, yStep));
      }

      private static Vector getVector(Mathematics.MathTypes.Vector vector)
      {
         const float u0 = (float) StretchCoefficients.SmallU0;
         const float l0 = (float) StretchCoefficients.L0;

         return new Vector(
            new PointF((float) vector.Start.X * l0, (float) vector.Start.Y * l0),
            new PointF((float) vector.End.X * u0, (float) vector.End.Y * u0)
            );
      }
   }
}
