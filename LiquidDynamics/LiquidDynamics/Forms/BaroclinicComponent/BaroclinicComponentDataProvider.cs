using System;
using System.Drawing;
using System.Linq;
using BarotropicComponentProblem;
using Common;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;

namespace LiquidDynamics.Forms.BaroclinicComponent
{
   internal class BaroclinicComponentDataProvider
   {
      private const int ExactSolutionNodes = 100;

      private readonly Parameters _parameters;

      private Grid _x;
      private Grid _y;
      private Grid _z;
      private double _tau;
      private SchemeType _schemeType;

      private Grid _demoZ;

      private double _time;
      private Solution _solution;

      private Complex[,][] _calculatedSolution;

      public BaroclinicComponentDataProvider(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");
         _parameters = parameters;
      }

      public ErrorContainer Errors { get; private set; }
      
      public SolveBaroclinicProblemResult Begin(int nx, int ny, int nz, double tau, SchemeType schemeType)
      {
         _x = Grid.Create(0, _parameters.SmallR, nx);
         _y = Grid.Create(0, _parameters.SmallQ, ny);
         _z = Grid.Create(0.0, _parameters.H, nz);
         _tau = tau;
         _schemeType = schemeType;

         _demoZ = Grid.Create(0.0, _parameters.H, ExactSolutionNodes);

         _time = 0;
         _solution = SolutionCreator.Create(_parameters);
         
         Complex[,][] exact = calculateExactBaroclinic(_time, _demoZ);
         _calculatedSolution = calculateExactBaroclinic(_time, _z);

         Errors = new ErrorContainer();
         Errors.AddError(_time, 0, 0);

         return
            new SolveBaroclinicProblemResult(
               _time,
               toPoints(_demoZ, exact, c => c.Re),
               toPoints(_demoZ, exact, c => c.Im),
               toPoints(_z, _calculatedSolution, c => c.Re),
               toPoints(_z, _calculatedSolution, c => c.Im),
               0.0,
               0.0
               );
      }

      public SolveBaroclinicProblemResult Step()
      {
         calculateBaroclinic();

         double errorRe, errorIm;
         Complex[,][] exact = calculateExactBaroclinic(_time + _tau, _z);
         ErrorCalculator.Calculate(exact, _calculatedSolution, out errorRe, out errorIm);

         Complex[,][] demo = calculateExactBaroclinic(_time + _tau, _demoZ);

         _time += _tau;

         Errors.AddError(_time, errorRe, errorIm);

         return new SolveBaroclinicProblemResult(
            _time,
            toPoints(_demoZ, demo, c => c.Re),
            toPoints(_demoZ, demo, c => c.Im),
            toPoints(_z, _calculatedSolution, c => c.Re),
            toPoints(_z, _calculatedSolution, c => c.Im),
            errorRe,
            errorIm
            );
      }

      private Complex[,][] calculateExactBaroclinic(double t, Grid z)
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();

         var result = new Complex[_x.Nodes, _y.Nodes][];

         for (int i = 0; i < _x.Nodes; i++)
         {
            double x = _x.Get(i);

            for (int j = 0; j < _y.Nodes; j++)
            {
               double y = _y.Get(j);
               result[i, j] = new Complex[z.Nodes];

               for (int k = 0; k < z.Nodes; k++)
               {
                  result[i, j][k] = baroclinic.Theta(t, x, y, z.Get(k));
               }
            }
         }

         return result;
      }

      private PointF[,][] toPoints(Grid z, Complex[,][] values, Func<Complex, double> selector)
      {
         int nx = values.GetLength(0);
         int ny = values.GetLength(1);

         var points = new PointF[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            for (int j = 0; j < ny; j++)
            {
               int nz = values[i, j].Length;
               points[i, j] = new PointF[nz];

               for (int k = 0; k < nz; k++)
               {
                  var x = (float) z.Get(k);
                  var y = (float) selector(values[i, j][k]);

                  points[i, j][k] = new PointF(x, y);
               }
            }
         }

         return points;
      }

      private void calculateBaroclinic()
      {
         for (int i = 0; i < _x.Nodes; i++)
         {
            double x = _x.Get(i);

            for (int j = 0; j < _y.Nodes; j++)
            {
               double y = _y.Get(j);
               IBaroclinicProblemSolver solver = createSolver(x, y, i, j);
               _calculatedSolution[i, j] = calculateBaroclinic(solver.Solve());
            }
         }
      }

      private IBaroclinicProblemSolver createSolver(double x, double y, int i, int j)
      {
         if (_schemeType == SchemeType.Linear)
         {
            return new BaroclinicProblemSolverLinear(
               createParameters(),
               getExactTheta0(_time, i, j),
               _tau, _z.Step, _z.Nodes,
               x, y,
               getTauX(y), getTauY(x, y),
               getTauXb(_time + _tau, x, y), getTauYb(_time + _tau, x, y)
               );
         }

         return new BaroclinicProblemSolverHyperbolic(
            createParameters(),
            getExactTheta0(_time, i, j),
            _tau, _z.Step, _z.Nodes,
            x, y,
            getTauX(y), getTauY(x, y),
            getTauXb(_time + _tau, x, y), getTauYb(_time + _tau, x, y)
            );
      }

      private ProblemParameters createParameters()
      {
         return new ProblemParameters
                   {
                      Beta = _parameters.Beta,
                      F1 = _parameters.F1,
                      F2 = _parameters.F2,
                      H = _parameters.H,
                      Mu = _parameters.Mu,
                      Nu = _parameters.Nu,
                      Rho0 = _parameters.Rho0,
                      SmallL0 = _parameters.SmallL0,
                      SmallQ = _parameters.SmallQ,
                      SmallR = _parameters.SmallR
                   };
      }

      private Complex[] getExactTheta0(double t, int i, int j)
      {
         double x = _x.Get(i);
         double y = _y.Get(j);

         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         double u = barotropic.U(t, x, y);
         double v = barotropic.V(t, x, y);

         var result = new Complex[_z.Nodes];

         for (int k = 0; k < _z.Nodes; k++)
         {
            Complex theta = _calculatedSolution[i, j][k];
            result[k] = new Complex(u + theta.Re, v + theta.Im);
         }

         return result;
      }

      private double getTauX(double y)
      {
         return -_parameters.F1 * _parameters.SmallQ * _parameters.Rho0 /
                Math.PI * Math.Cos(Math.PI * y / _parameters.SmallQ);
      }

      private double getTauY(double x, double y)
      {
         return _parameters.F2 * _parameters.SmallR * _parameters.Rho0 /
                Math.PI * Math.Cos(Math.PI * x / _parameters.SmallR) *
                Math.Sin(Math.PI * y / _parameters.SmallQ);
      }

      private double getTauXb(double t, double x, double y)
      {
         var barotropic = _solution.GetBarotropicComponent();
         return _parameters.Mu * _parameters.Rho0 * barotropic.U(t, x, y) * _parameters.H;
      }

      private double getTauYb(double t, double x, double y)
      {
         var barotropic = _solution.GetBarotropicComponent();
         return _parameters.Mu * _parameters.Rho0 * barotropic.V(t, x, y) * _parameters.H;
      }

      private Complex[] calculateBaroclinic(Complex[] theta)
      {
         Complex barotropic = calculateBarotropic(theta);
         var baroclinic = new Complex[theta.Length];

         for (int i = 0; i < baroclinic.Length; i++)
            baroclinic[i] = theta[i] - barotropic;

         return baroclinic.ToArray();
      }

      private Complex calculateBarotropic(Complex[] theta)
      {
         Complex barotropic = 0;

         for (int i = 0; i < theta.Length - 1; i++)
            barotropic += theta[i + 1] + theta[i];

         return barotropic * _z.Step / (2.0 * _parameters.H);
      }
   }
}