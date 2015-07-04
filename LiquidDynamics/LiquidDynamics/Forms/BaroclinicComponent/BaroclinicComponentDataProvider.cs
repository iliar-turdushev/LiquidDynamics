using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BarotropicComponentProblem;
using Common;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;

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

      private Complex[] _calculatedSolution;

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
         _calculatedSolution = calculateExactBaroclinic(_time, _z);

         return
            new SolveBaroclinicProblemResult(
               _time,
               toPoints(_demoZ, exact.Select(c => c.Re)),
               toPoints(_demoZ, exact.Select(c => c.Im)),
               toPoints(_z, _calculatedSolution.Select(c => c.Re)),
               toPoints(_z, _calculatedSolution.Select(c => c.Im)),
               0.0,
               0.0
               );
      }

      public SolveBaroclinicProblemResult Step()
      {
         var solver =
            new BaroclinicProblemSolver(
               createParameters(),
               getExactTheta0(_time),
               _tau, _z.Step, _z.Nodes,
               _x, _y,
               getTauX(), getTauY(),
               getTauXb(_time + _tau), getTauYb(_time + _tau)
               );

         _calculatedSolution = calculateBaroclinic(solver.Solve());

         double errorRe, errorIm;
         Complex[] exact = calculateExactBaroclinic(_time + _tau, _z);
         ErrorCalculator.Calculate(exact, _calculatedSolution, out errorRe, out errorIm);

         Complex[] demo = calculateExactBaroclinic(_time + _tau, _demoZ);

         _time += _tau;

         return new SolveBaroclinicProblemResult(
            _time,
            toPoints(_demoZ, demo.Select(c => c.Re)),
            toPoints(_demoZ, demo.Select(c => c.Im)),
            toPoints(_z, _calculatedSolution.Select(c => c.Re)),
            toPoints(_z, _calculatedSolution.Select(c => c.Im)),
            errorRe,
            errorIm
            );
      }

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

      private Complex[] getExactTheta0(double t)
      {
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         double u = barotropic.U(t, _x, _y);
         double v = barotropic.V(t, _x, _y);

         var result = new Complex[_z.Nodes];

         for (int i = 0; i < _z.Nodes; i++)
         {
            Complex theta = _calculatedSolution[i];
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

      private double getTauXb(double t)
      {
         var barotropic = _solution.GetBarotropicComponent();
         return _parameters.Mu * _parameters.Rho0 * barotropic.U(t, _x, _y) * _parameters.H;
      }

      private double getTauYb(double t)
      {
         var barotropic = _solution.GetBarotropicComponent();
         return _parameters.Mu * _parameters.Rho0 * barotropic.V(t, _x, _y) * _parameters.H;
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