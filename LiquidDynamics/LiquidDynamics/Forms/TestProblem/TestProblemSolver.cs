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

      private Grid _xResult;
      private Grid _yResult;
      private Grid _zResult;
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

         _xResult = getMiddleNodesGrid(_x);
         _yResult = getMiddleNodesGrid(_y);
         _zResult = Grid.Create(0, _parameters.H, _issykKulGrid3D.GetDepthGrid(0, 0).Length + 1);
      }

      public TestProblemSolution Begin()
      {
         TestProblemSolution solution = step(_barotropicSolver.Begin());

         _verticalProblemSolver = new VerticalProblemSolver(_xResult, _yResult, _zResult, _tau, _wind, _parameters, calcTheta(solution));
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
         int n = _xResult.Nodes;
         int m = _yResult.Nodes;

         var u = new double[n,m];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               u[i, j] = solution.BarotropicU[i, j]/*/_parameters.H*/;
            }
         }

         return u;
      }

      private double[,] getV(TestProblemSolution solution)
      {
         int n = _xResult.Nodes;
         int m = _yResult.Nodes;

         var v = new double[n, m];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               v[i, j] = solution.BarotropicV[i, j]/*/_parameters.H*/;
            }
         }

         return v;
      }

      private Complex[,][] calcTheta(TestProblemSolution solution)
      {
         int n = _xResult.Nodes;
         int m = _yResult.Nodes;
         int l = _zResult.Nodes;

         SquareGridFunction u = solution.BarotropicU;
         SquareGridFunction v = solution.BarotropicV;
         Complex[,][] baroclinic = solution.Baroclinic;

         var theta = new Complex[n, m][];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               double u0 = u[i, j];
               double v0 = v[i, j];

               theta[i, j] = new Complex[l];

               for (int k = 0; k < l; k++)
               {
                  Complex c = baroclinic[i, j][k];
                  theta[i, j][k] = new Complex(c.Re + u0 /*/ _parameters.H*/, c.Im + v0 /*/ _parameters.H*/);
               }
            }
         }

         return theta;
      }

      private TestProblemSolution step(IterationMethodResult barotropicResult)
      {
         SquareGridFunction u = barotropicResult.IterationResultU.Approximation;
         SquareGridFunction v = barotropicResult.IterationResultV.Approximation;

         int n = _issykKulGrid3D.Grid2D.N;
         int m = _issykKulGrid3D.Grid2D.M;

         var baroclinicResult = new Complex[n, m][];
         var barotropicU = new double[n, m];
         var barotropicV = new double[n, m];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (_issykKulGrid3D.Grid2D[i, j] == GridCell.Empty)
                  continue;

               Rectangle3D[] grid = _issykKulGrid3D.GetDepthGrid(i, j);
               double dz = grid[0].Hz;
               double h = grid.Length * dz;

               double uAvg = 0.25 * (u[i, j] + u[i, j + 1] + u[i + 1, j] + u[i + 1, j + 1]);
               double vAvg = 0.25 * (v[i, j] + v[i, j + 1] + v[i + 1, j] + v[i + 1, j + 1]);

               barotropicU[i, j] = uAvg;
               barotropicV[i, j] = vAvg;

               BaroclinicProblemSolver solver =
                  createBaroclinicProblemSolver(grid, i, j, uAvg, vAvg, _baroclinic[i, j]);

               baroclinicResult[i, j] = calculateBaroclinic(solver.Solve(), dz, h);
            }
         }

         _baroclinic = baroclinicResult;

         var uBarotropic = new SquareGridFunction(_xResult, _yResult, barotropicU);
         var vBarotropic = new SquareGridFunction(_xResult, _yResult, barotropicV);

         return
            new TestProblemSolution(
               getBarotropic(uBarotropic, vBarotropic),
               uBarotropic,
               vBarotropic, 
               baroclinicResult
               );
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

      private BaroclinicProblemSolver createBaroclinicProblemSolver(
         Rectangle3D[] grid, int i, int j, double u, double v, Complex[] theta
         )
      {
         double h = grid[0].Hz * grid.Length;

         double x = _xResult.Get(i);
         double y = _yResult.Get(j);

         return 
            new BaroclinicProblemSolver(
               _parameters,
               createTheta0(u, v, theta, h),
               _tau,
               grid[0].Hz,
               grid.Length + 1,
               x,
               y,
               _wind.TauX(x, y),
               _wind.TauY(x, y),
               getTauXb(u),
               getTauYb(v)
               );
      }
      
      private Complex[] createTheta0(double u, double v, Complex[] theta, double h)
      {
         var result = new Complex[theta.Length];

         for (int i = 0; i < result.Length; i++)
            result[i] = new Complex(u / h + theta[i].Re, v / h + theta[i].Im);

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

         return baroclinic.ToArray();
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
   }
}