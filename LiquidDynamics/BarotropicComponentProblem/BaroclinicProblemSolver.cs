using System;
using Common;
using Mathematics.MathTypes;

namespace BarotropicComponentProblem
{
   public sealed class BaroclinicProblemSolver
   {
      private static readonly Complex I = Complex.I;

      private readonly double _rho0;

      private readonly double _l0;
      private readonly double _beta;

      private readonly double _mu;
      private readonly double _nu;

      private readonly Complex[] _theta0;

      private readonly double _tau;
      private readonly double _dz;
      private readonly int _n;

      private readonly double _x;
      private readonly double _y;

      private readonly double _tauX;
      private readonly double _tauY;

      private readonly double _tauXb;
      private readonly double _tauYb;

      private readonly double _l;
      private readonly Complex _sigma;

      public BaroclinicProblemSolver(
         ProblemParameters problemParameters,
         Complex[] theta0,
         double tau,
         double dz, int n,
         double x, double y,
         double tauX, double tauY,
         double tauXb, double tauYb
         )
      {
         Check.NotNull(problemParameters, "problemParameters");
         
         _rho0 = problemParameters.Rho0;

         _l0 = problemParameters.SmallL0;
         _beta = problemParameters.Beta;

         _mu = problemParameters.Mu;
         _nu = problemParameters.Nu;

         _theta0 = theta0;

         _tau = tau;
         _dz = dz;
         _n = n;

         _x = x;
         _y = y;

         _tauX = tauX;
         _tauY = tauY;

         _tauXb = tauXb;
         _tauYb = tauYb;

         _l = calculateL();
         _sigma = calculateSigma();
      }

      public Complex[] Solve()
      {
         var a = new Complex[_n];
         var b = new Complex[_n];
         var c = new Complex[_n];
         var f = new Complex[_n];

         var a0 = new Complex[_n];
         var b0 = new Complex[_n];
         var c0 = new Complex[_n];

         c[0] = 2.0 * _nu * _sigma / (_dz * _dz);
         b[0] = 1.0 / _tau + I * _l * _sigma + c[0];
         f[0] = s(2.0 * (_tauX + I * _tauY) / (_dz * _rho0));

         c0[0] = 2.0 * _nu * (1.0 - _sigma) / (_dz * _dz);
         b0[0] = 1.0 / _tau - I * _l * (1.0 - _sigma) - c0[0];

         for (int k = 1; k < _n - 1; k++)
         {
            a[k] = _nu * _sigma / (_dz * _dz);
            c[k] = _nu * _sigma / (_dz * _dz);
            b[k] = 1.0 / _tau + I * _l * _sigma + a[k] + c[k];
            f[k] = 0.0;

            a0[k] = _nu * (1.0 - _sigma) / (_dz * _dz);
            c0[k] = _nu * (1.0 - _sigma) / (_dz * _dz);
            b0[k] = 1.0 / _tau - I * _l * (1.0 - _sigma) - a0[k] - c0[k];
         }

         a[_n - 1] = 2.0 * _nu * _sigma / (_dz * _dz);
         b[_n - 1] = 1.0 / _tau + I * _l * _sigma + a[_n - 1];
         f[_n - 1] = -s(2.0 * (_tauXb + I * _tauYb) / (_dz * _rho0));

         a0[_n - 1] = 2.0 * _nu * (1.0 - _sigma) / (_dz * _dz);
         b0[_n - 1] = 1.0 / _tau - I * _l * (1.0 - _sigma) - a0[_n - 1];

         var alpha = new Complex[_n - 1];
         var beta = new Complex[_n - 1];
         
         alpha[0] = c[0] / b[0];

         Complex f0 = (b0[0] * _theta0[0] + c0[0] * _theta0[1] + f[0]);
         beta[0] = f0 / b[0];

         for (int k = 1; k < _n - 1; k++)
         {
            Complex div = (b[k] - alpha[k - 1] * a[k]);
            alpha[k] = c[k] / div;

            Complex fk = a0[k] * _theta0[k - 1] +
                         b0[k] * _theta0[k] +
                         c0[k] * _theta0[k + 1] +
                         f[k];
            beta[k] = (fk + beta[k - 1] * a[k]) / div;
         }

         var theta = new Complex[_n];

         Complex fn = a0[_n - 1] * _theta0[_n - 2] +
                      b0[_n - 1] * _theta0[_n - 1] +
                      f[_n - 1];
         theta[_n - 1] = (fn + a[_n - 1] * beta[_n - 2]) /
                         (b[_n - 1] - a[_n - 1] * alpha[_n - 2]);

         for (int k = _n - 2; k >= 0; k--)
            theta[k] = alpha[k] * theta[k + 1] + beta[k];

         return theta;
      }

      private double calculateL()
      {
         return _l0 + _beta * _y;
      }

      private Complex calculateSigma()
      {
         double exp = Math.Exp(-_mu * _tau);
         double alpha = 1.0 - Math.Cos(_l * _tau) * exp;
         double beta = Math.Sin(_l * _tau) * exp;

         double sqr = alpha * alpha + beta * beta;
         double a = (_mu * alpha + _l * beta) / sqr;
         double b = (_l * alpha - _mu * beta) / sqr;

         return (a + I * b - 1.0 / _tau) / (_mu + I * _l);
      }

      private Complex s(Complex theta)
      {
         return _sigma * theta + (1.0 - _sigma) * theta;
      }
   }
}