using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Common;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;

namespace LiquidDynamics.Forms.BaroclinicComponent
{
   internal class BaroclinicComponentDataProvider
   {
      private const int ExactSolutionNodes = 500;

      private readonly Parameters _parameters;

      private double _x;
      private double _y;
      private Grid _z;
      private double _tau;

      private double _time;
      private Solution _solution;

      private Grid _demoZ;

      public BaroclinicComponentDataProvider(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");
         _parameters = parameters;
      }
      
      public SolveBaroclinicProblemResult Begin(double x, double y, int n, double tau)
      {
         _x = x;
         _y = y;
         _z = createZGrid(n);
         _tau = tau;

         _time = 0;
         _solution = SolutionCreator.Create(_parameters);

         _demoZ = createZGrid(ExactSolutionNodes);

         Complex[] exact = calculateExactBaroclinic(_time, _demoZ);
         Complex[] calculated = calculateExactBaroclinic(_time, _z);

         return
            new SolveBaroclinicProblemResult(
               _time,
               toPoints(_demoZ, exact.Select(c => c.Re)),
               toPoints(_demoZ, exact.Select(c => c.Im)),
               toPoints(_z, calculated.Select(c => c.Re)),
               toPoints(_z, calculated.Select(c => c.Im)),
               0.0,
               0.0
               );
      }

      /*
      public SolveBaroclinicProblemResult Step()
      {
         var solver =
            new BaroclinicProblemSolver(
               getParameters(),
               getExactTheta0(),
               _tau, _dz, _n,
               _x, _y,
               getTauX(), getTauY(),
               getTauXb(), getTauYb()
               );

         Complex[] theta = baroclinicTheta(solver.Solve());
         Complex[] thetaExact = getExactTheta();

         double re, im;
         ErrorCalculator.Calculate(theta, thetaExact, out re, out im);

         _time += _tau;

         return new SolveBaroclinicProblemResult(
            _time,
            toPoints(i => thetaExact[i].Re),
            toPoints(i => thetaExact[i].Im),
            toPoints(i => thetaExact[i].Re),
            toPoints(i => thetaExact[i].Im),
            );
      }

      private ProblemParameters getParameters()
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

      private Complex[] getExactTheta0()
      {
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();

         var result = new Complex[_n];

         for (int i = 0; i < _n; i++)
         {
            double u = barotropic.U(_time, _x, _y);
            double v = barotropic.V(_time, _x, _y);
            Complex theta = baroclinic.Theta(_time, _x, _y, _dz * i);
            result[i] = new Complex(u + theta.Re, v + theta.Im);
         }

         return result;
      }

      private double getTauX()
      {
         return -_parameters.F1 * _parameters.SmallQ * _parameters.Rho0 /
                Math.PI * Math.Cos(Math.PI * _y / _parameters.SmallQ);
      }

      private double getTauY()
      {
         return _parameters.F2 * _parameters.SmallR * _parameters.Rho0 /
                Math.PI * Math.Cos(Math.PI * _x / _parameters.SmallR) *
                Math.Sin(Math.PI * _y / _parameters.SmallQ);
      }

      private double getTauXb()
      {
         var barotropic = _solution.GetBarotropicComponent();
         return _parameters.Mu * _parameters.Rho0 * barotropic.U(_time, _x, _y);
      }

      private double getTauYb()
      {
         var barotropic = _solution.GetBarotropicComponent();
         return _parameters.Mu * _parameters.Rho0 * barotropic.V(_time, _x, _y);
      }

      private Complex[] baroclinicTheta(Complex[] theta)
      {
         Complex barotropic = barotropicTheta(theta);
         var baroclinic = new Complex[theta.Length];

         for (int i = 0; i < baroclinic.Length; i++)
            baroclinic[i] = theta[i] - barotropic;

         return baroclinic.ToArray();
      }

      private Complex barotropicTheta(Complex[] theta)
      {
         Complex barotropic = 0;

         for (int i = 0; i < theta.Length - 1; i++)
            barotropic += theta[i + 1] + theta[i];

         return barotropic * _dz / (2.0 * _parameters.H);
      }
      */

      private Grid createZGrid(int n)
      {
         return Grid.Create(0.0, _parameters.H, n);
      }

      private Complex[] calculateExactBaroclinic(double t, Grid z)
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();

         var result = new Complex[z.Nodes];

         for (int i = 0; i < result.Length; i++)
            result[i] = baroclinic.Theta(t, _x, _y, z.Get(i));

         return result;
      }

      private PointF[] toPoints(Grid z, IEnumerable<double> values)
      {
         return values.Select((v, i) => new PointF((float) z.Get(i), (float) v)).ToArray();
      }
   }
}