using System.Linq;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using BarotropicComponentProblem.IterationMethod;
using BarotropicComponentProblem.TestProblem;
using Common;
using LiquidDynamics.Forms.VerticalComponentNumerical;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace LiquidDynamics.Forms.TestProblem
{
   internal class TestProblemSolver
   {
      private readonly ProblemParameters _parameters;
      private readonly IssykKulGrid3D _issykKulGrid3D;
      private readonly IWind _wind;
      private readonly double _tau;
      private readonly Grid _x;
      private readonly Grid _y;
      private readonly InitialCondition _initialCondition;
      private readonly double _sigma;
      private readonly double _delta;
      private readonly int _k;
      private readonly int[,] _surface;

      private readonly BarotropicComponentProblemSolver _barotropicSolver;

      private Complex[,][] _baroclinic;

      private VerticalProblemSolver _verticalProblemSolver;

      public TestProblemSolver(
         ProblemParameters parameters,
         IssykKulGrid3D issykKulGrid3D,
         IWind wind,
         double tau,
         Grid x,
         Grid y,
         InitialCondition initialCondition,
         Complex[,][] initialTheta,
         double sigma,
         double delta,
         int k,
         int[,] surface
         )
      {
         Check.NotNull(parameters, "parameters");
         Check.NotNull(issykKulGrid3D, "issykKulGrid3D");
         Check.NotNull(wind, "wind");
         Check.NotNull(x, "x");
         Check.NotNull(y, "y");
         Check.NotNull(initialCondition, "initialCondition");
         Check.NotNull(initialTheta, "initialTheta");
         Check.NotNull(surface, "surface");

         _parameters = parameters;
         _issykKulGrid3D = issykKulGrid3D;
         _wind = wind;
         _tau = tau;
         _x = x;
         _y = y;
         _initialCondition = initialCondition;
         _sigma = sigma;
         _delta = delta;
         _k = k;
         _surface = surface;

         _barotropicSolver = createBarotropicSolver();
         _baroclinic = initialTheta;
      }

      public TestProblemSolution Begin()
      {
         TestProblemSolution solution = step(_barotropicSolver.Begin());

         _verticalProblemSolver = new VerticalProblemSolver(_x, _y, _tau, _wind, _parameters, calcTheta(solution), _issykKulGrid3D);
         solution.W = _verticalProblemSolver.Begin();

         return solution;
      }

      public TestProblemSolution Step()
      {
         TestProblemSolution solution = step(_barotropicSolver.Step());
         solution.W = _verticalProblemSolver.Step(calcTheta(solution), getU(solution), getV(solution));
         return solution;
      }

      private double[,] getU(TestProblemSolution solution)
      {
         var u = new double[_x.Nodes,_y.Nodes];

         for (int i = 0; i < _x.Nodes; i++)
            for (int j = 0; j < _y.Nodes; j++)
               u[i, j] = solution.BarotropicU[i, j];

         return u;
      }

      private double[,] getV(TestProblemSolution solution)
      {
         var v = new double[_x.Nodes, _y.Nodes];

         for (int i = 0; i < _x.Nodes; i++)
            for (int j = 0; j < _y.Nodes; j++)
               v[i, j] = solution.BarotropicV[i, j];

         return v;
      }

      private Complex[,][] calcTheta(TestProblemSolution solution)
      {
         int n = solution.BarotropicU.N;
         int m = solution.BarotropicU.M;

         SquareGridFunction u = solution.BarotropicU;
         SquareGridFunction v = solution.BarotropicV;
         Complex[,][] baroclinic = solution.Baroclinic;

         var theta = new Complex[n, m][];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (baroclinic[i, j] == null)
                  continue;

               int nz = baroclinic[i, j].Length;
               theta[i, j] = new Complex[nz];

               for (int k = 0; k < nz; k++)
               {
                  Complex c = baroclinic[i, j][k];
                  theta[i, j][k] = new Complex(c.Re + u[i, j], c.Im + v[i, j]);
               }
            }
         }

         return theta;
      }

      private TestProblemSolution step(IterationMethodResult barotropicResult)
      {
         IssykKulGrid2D grid2D = _issykKulGrid3D.Grid2D;
         int n = grid2D.N;
         int m = grid2D.M;

         SquareGridFunction u = barotropicResult.IterationResultU.Approximation;
         SquareGridFunction v = barotropicResult.IterationResultV.Approximation;

         var baroclinicResult = new Complex[n + 1, m + 1][];
         var barotropicU = new double[n + 1, m + 1];
         var barotropicV = new double[n + 1, m + 1];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               GridCell cell = grid2D[i, j];

               if (cell == GridCell.Empty)
                  continue;

               barotropicU[i, j] = u[i, j] / cell.P(0, 0).Z;
               barotropicV[i, j] = v[i, j] / cell.P(0, 0).Z;

               baroclinicResult[i, j] =
                  calculateBaroclinic(getDepthGrid(i, j), cell.P(0, 0).Z, i, j,
                                      barotropicU[i, j], barotropicV[i, j]);

               if (i == n - 1 || grid2D[i + 1, j] == GridCell.Empty)
               {
                  barotropicU[i + 1, j] = u[i + 1, j] / cell.P(2, 0).Z;
                  barotropicV[i + 1, j] = v[i + 1, j] / cell.P(2, 0).Z;

                  baroclinicResult[i + 1, j] =
                     calculateBaroclinic(getDepthGrid(i + 1, j), cell.P(2, 0).Z, i + 1, j,
                                         barotropicU[i + 1, j], barotropicV[i + 1, j]);
               }

               if (j == m - 1 || grid2D[i, j + 1] == GridCell.Empty)
               {
                  barotropicU[i, j + 1] = u[i, j + 1] / cell.P(0, 2).Z;
                  barotropicV[i, j + 1] = v[i, j + 1] / cell.P(0, 2).Z;

                  baroclinicResult[i, j + 1] =
                     calculateBaroclinic(getDepthGrid(i, j + 1), cell.P(0, 2).Z, i, j + 1,
                                         barotropicU[i, j + 1], barotropicV[i, j + 1]);
               }

               if ((i == n - 1 && j == m - 1) ||
                   (i == n - 1 && j < m - 1 && grid2D[i, j + 1] == GridCell.Empty) ||
                   (j == m - 1 && i < n - 1 && grid2D[i + 1, j] == GridCell.Empty) ||
                   (i < n - 1 && j < m - 1 && grid2D[i + 1, j + 1] == GridCell.Empty))
               {
                  barotropicU[i + 1, j + 1] = u[i + 1, j + 1] / cell.P(2, 2).Z;
                  barotropicV[i + 1, j + 1] = v[i + 1, j + 1] / cell.P(2, 2).Z;

                  baroclinicResult[i + 1, j + 1] =
                     calculateBaroclinic(getDepthGrid(i + 1, j + 1), cell.P(2, 2).Z, i + 1, j + 1,
                                         barotropicU[i + 1, j + 1], barotropicV[i + 1, j + 1]);
               }
            }
         }

         _baroclinic = baroclinicResult;

         var uResult = new SquareGridFunction(_x, _y, barotropicU);
         var vResult = new SquareGridFunction(_x, _y, barotropicV);

         return new TestProblemSolution(getBarotropic(uResult, vResult),
                                        uResult, vResult, baroclinicResult);
      }

      private BarotropicComponentProblemSolver createBarotropicSolver()
      {
         return
            new BarotropicComponentProblemSolver(
               createDynamicProblem(),
               createGridParameters(),
               _initialCondition,
               createInterationProcessParameters(_initialCondition.U),
               createInterationProcessParameters(_initialCondition.V),
               _surface
               );
      }

      private IDynamicProblem createDynamicProblem()
      {
         return new IntegroInterpolatingScheme(_parameters, _issykKulGrid3D.Grid2D, _wind, _tau);
      }

      private GridParameters createGridParameters()
      {
         return new GridParameters(_tau, _x, _y);
      }

      private IterationProcessParameters createInterationProcessParameters(
         SquareGridFunction initialApproximation
         )
      {
         return new IterationProcessParameters(_sigma, _delta, _k, initialApproximation);
      }

      private Grid getMiddleNodesGrid(Grid grid)
      {
         return Grid.Create(grid.Get(0) + grid.Step * 0.5,
                            grid.Get(grid.Nodes - 1) - grid.Step * 0.5,
                            grid.Nodes - 1);
      }

      private Complex[] calculateBaroclinic(
         DepthGridParams gridParams, double h,
         int i, int j, double u, double v
         )
      {
         BaroclinicProblemSolver solver =
            createBaroclinicProblemSolver(
               gridParams.Grid[0].Hz, gridParams.Grid.Length + 1, h,
               i, j, u, v, _baroclinic[i, j]
               );

         return calculateBaroclinic(solver.Solve(), gridParams.Grid[0].Hz, h);
      }

      private DepthGridParams getDepthGrid(int i, int j)
      {
         DepthGridParams[] grids =
            {
               getDepthGridSafe(i - 1, j - 1),
               getDepthGridSafe(i - 1, j),
               getDepthGridSafe(i, j - 1),
               getDepthGridSafe(i, j)
            };

         return grids.Where(g => g != null)
                     .OrderBy(g => g.Grid.Length)
                     .Last();
      }

      private DepthGridParams getDepthGridSafe(int i, int j)
      {
         int n = _issykKulGrid3D.Grid2D.N;
         int m = _issykKulGrid3D.Grid2D.M;

         if (i < 0 || i > n - 1)
            return null;

         if (j < 0 || j > m - 1)
            return null;

         if (_issykKulGrid3D.Grid2D[i, j] == GridCell.Empty)
            return null;

         return new DepthGridParams
                   {
                      Grid = _issykKulGrid3D.GetDepthGrid(i, j),
                      Cell = _issykKulGrid3D.Grid2D[i, j]
                   };
      }

      private BaroclinicProblemSolver createBaroclinicProblemSolver(
         double dz, int nz, double h,
         int i, int j,
         double u, double v,
         Complex[] theta
         )
      {
         double x = _x.Get(i);
         double y = _y.Get(j);

         return 
            new BaroclinicProblemSolver(
               _parameters,
               createTheta0(u, v, theta),
               _tau,
               dz,
               nz,
               x,
               y,
               _wind.TauX(x, y),
               _wind.TauY(x, y),
               getTauXb(u * h),
               getTauYb(v * h)
               );
      }
      
      private Complex[] createTheta0(double u, double v, Complex[] theta)
      {
         var result = new Complex[theta.Length];

         for (int i = 0; i < result.Length; i++)
            result[i] = new Complex(u + theta[i].Re, v + theta[i].Im);

         return result;
      }

      private double getTauXb(double u)
      {
         return _parameters.Mu * _parameters.Rho0 * u;
      }

      private double getTauYb(double v)
      {
         return _parameters.Mu * _parameters.Rho0 * v;
      }

      private Complex[] calculateBaroclinic(Complex[] theta, double dz, double h)
      {
         Complex barotropic = calculateBarotropic(theta, dz, h);
         var baroclinic = new Complex[theta.Length];

         for (int i = 0; i < baroclinic.Length; i++)
            baroclinic[i] = theta[i] - barotropic;

         return baroclinic;
      }

      private Complex calculateBarotropic(Complex[] theta, double dz, double h)
      {
         Complex barotropic = 0;

         for (int i = 0; i < theta.Length - 1; i++)
            barotropic += theta[i + 1] + theta[i];

         return barotropic * dz / (2.0 * h);
      }

      private Vector[,] getBarotropic(SquareGridFunction u, SquareGridFunction v)
      {
         var vectors = new Vector[u.N, u.M];

         for (var i = 0; i < u.N; i++)
            for (var j = 0; j < u.M; j++)
               vectors[i, j] = new Vector(u.Grid(i, j), new Point(u[i, j], v[i, j]));

         return vectors;
      }

      private class DepthGridParams
      {
         public Rectangle3D[] Grid { get; set; }
         public GridCell Cell { get; set; }
      }
   }
}