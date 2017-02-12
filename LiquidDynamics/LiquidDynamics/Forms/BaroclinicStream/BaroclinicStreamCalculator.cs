using System;
using BarotropicComponentProblem;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace LiquidDynamics.Forms.BaroclinicStream
{
   public class BaroclinicStreamCalculator
   {
      private static readonly Complex I = Complex.I;

      private readonly Grid _x;
      private readonly Grid _y;
      private readonly Grid _z;
      private readonly double _tau;

      private readonly double[,] _uBarotropic;
      private readonly double[,] _vBarotropic;

      private readonly Complex[,][] _theta0;
      private readonly Complex[,][] _theta;
      private readonly Complex[,][] _psi0;

      private readonly ProblemParameters _parameters;
      private readonly IWind _wind;
      private readonly Sigma _sigma;

      public BaroclinicStreamCalculator(
         Grid x, Grid y, Grid z, double tau,
         double[,] uBarotropic, double[,] vBarotropic,
         Complex[,][] theta0, Complex[,][] theta, Complex[,][] psi0,
         ProblemParameters parameters, IWind wind,
         Sigma sigma
         )
      {
         _x = x;
         _y = y;
         _z = z;
         _tau = tau;
         _uBarotropic = uBarotropic;
         _vBarotropic = vBarotropic;
         _theta0 = theta0;
         _theta = theta;
         _psi0 = psi0;
         _parameters = parameters;
         _wind = wind;
         _sigma = sigma;
      }

      public Complex[,][] Calculate()
      {
         int n = _x.Nodes;
         int m = _y.Nodes;

         var psi = new Complex[n, m][];

         for (int i = 0; i < n; i++)
         {
            double x = _x.Get(i);

            for (int j = 0; j < m; j++)
            {
               double y = _y.Get(j);

               double u = _uBarotropic[i, j];
               double v = _vBarotropic[i, j];
               Complex[] t0 = _theta0[i, j];
               Complex[] t = _theta[i, j];
               Complex[] p0 = _psi0[i, j];

               psi[i, j] = calculatePsi(x, y, u, v, t0, t, p0);
            }
         }

         return psi;
      }

      private Complex[] calculatePsi(
         double x, double y,
         double u, double v,
         Complex[] theta0, Complex[] theta, Complex[] psi0
         )
      {
         int n = _z.Nodes;
         double dz = _z.Step;

         double rho0 = _parameters.Rho0;
         double nu = _parameters.Nu;
         double mu = _parameters.Mu;

         Complex sigma = calculateSigma(y);
         Complex s = (1 - sigma) / sigma;
         
         double l = calculateL(y);
         Complex lamda = (1 + Complex.I) * Math.Sqrt(l / (2 * nu));
         Complex r = lamda * dz / 2;
         Complex d = nu / dz * r / Complex.Sinh(2 * r);

         var psi = new Complex[n];
         psi[0] = -(_wind.TauX(x, y) + I * _wind.TauY(x, y)) / rho0;

         for (int k = 1; k < n - 1; k++)
         {
            psi[k] = -s * psi0[k] +
                     d * (theta[k + 1] - theta[k]) +
                     d * (theta[k] - theta[k - 1]) +
                     s * d * (theta0[k + 1] - theta0[k]) +
                     s * d * (theta0[k] - theta0[k - 1]);
         }

         psi[n - 1] = -mu * (u + I * v) * _parameters.H;
         return psi;
      }

      private Complex calculateSigma(double y)
      {
         switch (_sigma)
         {
            case Sigma.Half:
               return 0.5;

            case Sigma.One:
               return 1.0;

            default:
               double mu = _parameters.Mu;
               double l = calculateL(y);

               double exp = Math.Exp(-mu * _tau);
               double alpha = 1.0 - Math.Cos(l * _tau) * exp;
               double beta = Math.Sin(l * _tau) * exp;

               double sqr = alpha * alpha + beta * beta;
               double a = (mu * alpha + l * beta) / sqr;
               double b = (l * alpha - mu * beta) / sqr;

               return (a + I * b - 1.0 / _tau) / (mu + I * l);
         }
      }

      private double calculateL(double y)
      {
         double l0 = _parameters.SmallL0;
         double beta = _parameters.Beta;
         return l0 + beta * y;
      }
   }
}
