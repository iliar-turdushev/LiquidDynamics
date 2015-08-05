using System.Drawing;
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
using Point = Mathematics.MathTypes.Point;
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
         TestProblemSolution solution = _solver.Begin();

         return new Solution(getVelocityField(solution, _grid.Grid2D, _x, _y), _bounds);
      }

      public Solution Step()
      {
         TestProblemSolution solution = _solver.Step();
         return new Solution(getVelocityField(solution, _grid.Grid2D, _x, _y), _bounds);
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
         int n = _grid.Grid2D.N;
         int m = _grid.Grid2D.M;

         var theta = new Complex[n, m][];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (_grid.Grid2D[i, j] == GridCell.Empty)
                  continue;

               Rectangle3D[] depthGrid = _grid.GetDepthGrid(i, j);
               theta[i, j] = new Complex[depthGrid.Length + 1];

               for (int k = 0; k < depthGrid.Length + 1; k++)
                  theta[i, j][k] = new Complex();
            }
         }

         return theta;
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
         TestProblemSolution solution, IssykKulGrid2D grid,
         Mathematics.Numerical.Grid x, Mathematics.Numerical.Grid y
         )
      {
         int n = grid.N;
         int m = grid.M;

         Mathematics.MathTypes.Vector[,] barotropic = solution.Barotropic;
         Complex[,][] baroclinic = solution.Baroclinic;
         var velocityField = new Vector[n, m];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (grid[i, j] == GridCell.Empty)
                  continue;

               var vector =
                  new Mathematics.MathTypes.Vector(
                     barotropic[i, j].Start,
                     new Point(barotropic[i, j].End.X + baroclinic[i, j][0].Re,
                               barotropic[i, j].End.Y + baroclinic[i, j][0].Im)
                     );

               velocityField[i, j] = getVector(vector);
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
