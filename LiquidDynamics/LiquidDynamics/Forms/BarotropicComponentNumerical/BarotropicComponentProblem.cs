using System;
using System.Drawing;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using BarotropicComponentProblem.IterationMethod;
using BarotropicComponentProblem.TestProblem;
using Common;
using ControlLibrary.Types;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Barotropic;

namespace LiquidDynamics.Forms.BarotropicComponentNumerical
{
   internal sealed class BarotropicComponentProblem
   {
      private const double MinX = 0;
      private const double MinY = 0;
      private const double BeginTime = 0;

      private IBarotropicComponent _barotropicComponent;
      private BarotropicComponentProblemSolver _problemSolver;

      private Grid _x;
      private Grid _y;
      private double _h;

      private double _tau;
      private double _time;
      private SizeF _cellSize;

      private bool _isFirstStep;

      internal ErrorContainer ErrorContainer { get; private set; }

      internal BarotropicComponentResult Reset(
         Parameters modelProblemParameters,
         GridParameters gridParameters,
         double sigma, double delta, int k,
         double theta, double chi, SchemeType schemeType)
      {
         Check.NotNull(modelProblemParameters, "modelProblemParameters");
         Check.NotNull(gridParameters, "gridParameters");

         _barotropicComponent = SolutionCreator.Create(modelProblemParameters).GetBarotropicComponent();

         _x = Grid.Create(MinX, modelProblemParameters.SmallR, gridParameters.N);
         _y = Grid.Create(MinY, modelProblemParameters.SmallQ, gridParameters.M);
         _h = modelProblemParameters.H;

         _tau = gridParameters.Tau;
         _time = BeginTime;

         _cellSize = new SizeF((float) _x.Step, (float) _y.Step);
         _isFirstStep = true;

         // Exact solution.
         var initialCondition = getExactSolution(_barotropicComponent, _time, _x, _y, _h);
         var squareVelocityField = new SquareVelocityField(
            toVectors(_x, _y, initialCondition.U, initialCondition.V),
            _cellSize);

         // Barotropic problem solver initialization.
         var u = new SquareGridFunction(_x, _y, initialCondition.U);
         var v = new SquareGridFunction(_x, _y, initialCondition.V);
         var problemParameters = getTestProblemParameters(modelProblemParameters);
         var solverGridParameters = new global::BarotropicComponentProblem.GridParameters(_tau, _x, _y);

         var scheme = getScheme(schemeType, problemParameters, solverGridParameters, theta, chi);

         _problemSolver = new BarotropicComponentProblemSolver(
            scheme, solverGridParameters,
            new InitialCondition(u, v),
            new IterationProcessParameters(sigma, delta, k, u),
            new IterationProcessParameters(sigma, delta, k, v),
            getDesk(_x.Nodes, _y.Nodes),
            getZeidelMethodType(schemeType)
            );

         ErrorContainer = new ErrorContainer();
         ErrorContainer.AddError(0.0, 0.0, 0.0);

         return new BarotropicComponentResult(squareVelocityField, squareVelocityField, 0, 0, _time);
      }
      
      internal BarotropicComponentResult Step()
      {
         _time += _tau;

         // Exact solution.
         var exactSolution = getExactSolution(_barotropicComponent, _time, _x, _y, _h);
         var exactSolutionField = new SquareVelocityField(
            toVectors(_x, _y, exactSolution.U, exactSolution.V),
            _cellSize);

         // Calculated solution.
         var iterationResult = _isFirstStep ? _problemSolver.Begin() : _problemSolver.Step();
         var calculatedSolutionField =
            getSquareVelocityField((i, j) => iterationResult.BarotropicComponent.Vectors[i, j].End.X,
                                   (i, j) => iterationResult.BarotropicComponent.Vectors[i, j].End.Y);
         _isFirstStep = false;

         // Result.
         var errorU = ErrorCalculator.Calculate(new SquareGridFunction(_x, _y, exactSolution.U),
                                                iterationResult.IterationResultU.Approximation);
         var errorV = ErrorCalculator.Calculate(new SquareGridFunction(_x, _y, exactSolution.V),
                                                iterationResult.IterationResultV.Approximation);
         var iterationStatus = getIterationStatus(iterationResult.IterationResultU.IterationStatus,
                                                  iterationResult.IterationResultV.IterationStatus);

         ErrorContainer.AddError(_time, errorU, errorV);

         return new BarotropicComponentResult(exactSolutionField, calculatedSolutionField,
                                              errorU, errorV, _time, iterationStatus);
      }

      private static IDynamicProblem getScheme(
         SchemeType schemeType,
         ProblemParameters problemParameters,
         global::BarotropicComponentProblem.GridParameters solverGridParameters,
         double theta, double chi)
      {
         switch (schemeType)
         {
            case SchemeType.DifferentialScheme1:
               return new DifferentialScheme1(problemParameters, solverGridParameters, theta);

            case SchemeType.DifferentialScheme2:
               return new DifferentialScheme2(problemParameters, solverGridParameters, theta, chi);

            case SchemeType.IntegroInterpolatingScheme:
               return new IntegroInterpolatingScheme(problemParameters, solverGridParameters);

            case SchemeType.DifferentialScheme1Improved:
               return new DifferentialScheme1Improved(problemParameters, solverGridParameters, theta);

            case SchemeType.DifferentialScheme2Improved:
               return new DifferentialScheme2Improved(problemParameters, solverGridParameters, theta, chi);

            default: // IntegroInterpolatingScheme.
               return getCommonScheme(solverGridParameters, problemParameters);
         }
      }

      private static ZeidelMethodType getZeidelMethodType(SchemeType schemeType)
      {
         switch (schemeType)
         {
            case SchemeType.DifferentialScheme1:
            case SchemeType.DifferentialScheme2:
            case SchemeType.IntegroInterpolatingScheme:
               return ZeidelMethodType.Simple;

            default:
               return ZeidelMethodType.Improved;
         }
      }

      private static IDynamicProblem getCommonScheme(
         global::BarotropicComponentProblem.GridParameters gridParameters,
         ProblemParameters problemParameters)
      {
         Grid xGrid = gridParameters.X;
         Grid yGrid = gridParameters.Y;

         int n = xGrid.Nodes;
         int m = yGrid.Nodes;

         double hx = xGrid.Step;
         double hy = yGrid.Step;

         double z = problemParameters.H;

         var cells = new GridCell[n - 1, m - 1];

         for (int i = 0; i < n - 1; i++)
         {
            double x = xGrid.Get(i);

            for (int j = 0; j < m - 1; j++)
            {
               double y = yGrid.Get(j);
               cells[i, j] = new GridCell(x, y, hx, hy, z);
            }
         }

         var grid = new IssykKulGrid2D(cells, hx, hy);
         return new IntegroInterpolatingSchemeImproved(problemParameters, grid, gridParameters.Tau);
      }

      private static int[,] getDesk(int n, int m)
      {
         var desk = new int[n - 1, m - 1];

         for (int i = 0; i < desk.GetLength(0); i++)
         {
            for (int j = 0; j < desk.GetLength(1); j++)
               desk[i, j] = 1;
         }

         return desk;
      }

      private SquareVelocityField getSquareVelocityField(Func<int, int, double> u, Func<int, int, double> v)
      {
         var n = _x.Nodes;
         var m = _y.Nodes;
         var vectors = new Vector[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = (float) _x.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = (float) _y.Get(j);
               vectors[i, j] = new Vector(new PointF(x, y),
                                          new PointF((float) u(i, j), (float) v(i, j)));
            }
         }

         return new SquareVelocityField(vectors, _cellSize);
      }

      private static BarotropicComponent getExactSolution(
         IBarotropicComponent barotropicComponent, double time, Grid xGrid, Grid yGrid, double h)
      {
         var n = xGrid.Nodes;
         var m = yGrid.Nodes;
         var u = new double[n, m];
         var v = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = xGrid.Get(i);
            
            for (var j = 0; j < m; j++)
            {
               var y = yGrid.Get(j);
               // Результаты функций U и V умножаются на h, чтобы получить интегральную скорость. 
               u[i, j] = barotropicComponent.U(time, x, y) * h;
               v[i, j] = barotropicComponent.V(time, x, y) * h;
            }
         }

         return new BarotropicComponent(u, v);
      }

      private static Vector[,] toVectors(Grid xGrid, Grid yGrid, double[,] u, double[,] v)
      {
         var n = xGrid.Nodes;
         var m = yGrid.Nodes;
         var vectors = new Vector[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = (float) xGrid.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = (float) yGrid.Get(j);

               vectors[i, j] = new Vector(new PointF(x, y),
                                          new PointF((float) u[i, j], (float) v[i, j]));
            }
         }

         return vectors;
      }

      private static ProblemParameters getTestProblemParameters(
         Parameters modelProblemParameters)
      {
         return new ProblemParameters
                   {
                      Beta = modelProblemParameters.Beta,
                      F1 = modelProblemParameters.F1,
                      F2 = modelProblemParameters.F2,
                      H = modelProblemParameters.H,
                      Mu = modelProblemParameters.Mu,
                      Rho0 = modelProblemParameters.Rho0,
                      SmallL0 = modelProblemParameters.SmallL0,
                      SmallQ = modelProblemParameters.SmallQ,
                      SmallR = modelProblemParameters.SmallR
                   };
      }

      private IterationStatus getIterationStatus(IterationStatus uStatus, IterationStatus vStatus)
      {
         if (uStatus == IterationStatus.Convergence && vStatus == IterationStatus.Convergence)
         {
            return IterationStatus.Convergence;
         }

         if (uStatus == IterationStatus.Circularity || vStatus == IterationStatus.Circularity)
         {
            return IterationStatus.Circularity;
         }

         if (uStatus == IterationStatus.Divergence || vStatus == IterationStatus.Divergence)
         {
            return IterationStatus.Divergence;
         }

         return IterationStatus.None;
      }

      private sealed class BarotropicComponent
      {
         public BarotropicComponent(double[,] u, double[,] v)
         {
            U = u;
            V = v;
         }

         public double[,] U { get; private set; }
         public double[,] V { get; private set; }
      }
   }
}
