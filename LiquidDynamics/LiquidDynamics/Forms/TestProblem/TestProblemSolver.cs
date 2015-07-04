using System.Linq;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using BarotropicComponentProblem.IterationMethod;
using BarotropicComponentProblem.TestProblem;
using Common;
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
         return step(_barotropicSolver.Begin());
      }

      public TestProblemSolution Step()
      {
         return step(_barotropicSolver.Step());
      }

      private TestProblemSolution step(IterationMethodResult barotropicResult)
      {
         SquareGridFunction u = barotropicResult.IterationResultU.Approximation;
         SquareGridFunction v = barotropicResult.IterationResultV.Approximation;

         var baroclinicResult = new Complex[_x.Nodes,_y.Nodes][];

         for (int i = 0; i < _x.Nodes; i++)
         {
            for (int j = 0; j < _y.Nodes; j++)
            {
               BaroclinicProblemSolver solver =
                  createBaroclinicProblemSolver(i, j, u[i, j], v[i, j], _baroclinic[i, j]);

               Rectangle3D[] grid = _issykKulGrid3D.GetDepthGrid(i == _x.Nodes - 1 ? i - 1 : i,
                                                                 j == _y.Nodes - 1 ? j - 1 : j);
               double dz = grid[0].Hz;

               baroclinicResult[i, j] = calculateBaroclinic(solver.Solve(), dz, grid.Length * dz);
            }
         }

         _baroclinic = baroclinicResult;

         return new TestProblemSolution(barotropicResult, baroclinicResult);
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

      private BaroclinicProblemSolver createBaroclinicProblemSolver(
         int i, int j, double u, double v, Complex[] theta
         )
      {
         Rectangle3D[] grid = _issykKulGrid3D.GetDepthGrid(i == _x.Nodes - 1 ? i - 1 : i,
                                                           j == _y.Nodes - 1 ? j - 1 : j);

         double x = _x.Get(i);
         double y = _y.Get(j);

         return 
            new BaroclinicProblemSolver(
               _parameters,
               createTheta0(u, v, theta),
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

         return baroclinic.ToArray();
      }

      private Complex calculateBarotropic(Complex[] theta, double dz, double h)
      {
         Complex barotropic = 0;

         for (int i = 0; i < theta.Length - 1; i++)
            barotropic += theta[i + 1] + theta[i];

         return barotropic * dz / (2.0 * h);
      }
   }
}