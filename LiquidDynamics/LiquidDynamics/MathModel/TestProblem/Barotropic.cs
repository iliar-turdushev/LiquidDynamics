using System;
using static System.Math;

namespace LiquidDynamics.MathModel.TestProblem
{
   public sealed class Barotropic
   {
      public Barotropic(double[,] u, double[,] v, int nx, int ny)
      {
         if (u == null)
            throw new ArgumentNullException(nameof(u));

         if (v == null)
            throw new ArgumentNullException(nameof(v));

         if (nx <= 1)
            throw new ArgumentException("nx <= 1", nameof(nx));

         if (ny <= 1)
            throw new ArgumentException("ny <= 1", nameof(ny));

         U = u;
         V = v;
         Nx = nx;
         Ny = ny;
      }
      
      public double[,] U { get; }
      public double[,] V { get; }

      public int Nx { get; }
      public int Ny { get; }

      // [r] = [q] = [h] = 1
      // [f1] = [f2] = 1
      // [l0] = [beta] = 1
      // [mu] = 1
      // [m] = [k] = 1
      // [s1] = [s2] = 1
      // [gx] = [gy] = 1
      // [t] = 1
      // [out] = 1
      public static Barotropic Calc(
         double r, double q, double h,
         double f1, double f2,
         double beta,
         double mu,
         int m, int k,
         double s1, double s2,
         Grid gx, Grid gy,
         double t
         )
      {
         double a = calcA(q, beta, mu);
         double b = calcB(q, beta, mu);

         double c3 = calcC3(q, f1, mu);
         double c4 = calcC4(r, q, f2, beta, mu);
         double c5 = calcC5(r, q, f2, beta, mu);
         double c1 = calcC1(r, a, b, c3, c4);
         double c2 = calcC2(c1, c3, c4);

         double rm = calcRm(r, m);
         double qk = calcQk(q, k);
         double sqrt = Sqrt(sqr(rm) + sqr(qk));
         double btsqrt = beta * t / (2 * sqrt);

         double pir = PI / r;
         double piq = PI / q;

         double emut = Exp(-mu * t);

         double[,] u = new double[gx.N, gy.N];
         double[,] v = new double[gx.N, gy.N];

         for (int i = 0; i < gx.N; i++)
         {
            double x = gx[i];

            double ea = Exp(a * x);
            double eb = Exp(b * x);

            double cosr = Cos(pir * x);
            double sinr = Sin(pir * x);

            double cosrm = Cos(rm * x);
            double sinrm = Sin(rm * x);

            double arg = x * sqrt + btsqrt;
            double cosarg = Cos(arg);
            double sinarg = Sin(arg);

            for (int j = 0; j < gy.N; j++)
            {
               double y = gy[j];

               double cosq = Cos(piq * y);
               double sinq = Sin(piq * y);

               double cosqk = Cos(qk * y);
               double sinqk = Sin(qk * y);

               u[i, j] = (-piq * cosq * (1 - c1 * ea - c2 * eb - c3 - c4 * cosr - c5 * sinr) -
                          2 * qk * sinrm * cosqk * (s1 * cosarg - s2 * sinarg) * emut) / h;

               v[i, j] = (-sinq * (c1 * a * ea + c2 * b * eb - c4 * pir * sinr + c5 * pir * cosr) +
                          2 * sinqk * emut * (rm * cosrm * (s1 * cosarg - s2 * sinarg) +
                                              sqrt * sinrm * (-s1 * sinarg - s2 * cosarg))) / h;
            }
         }

         return new Barotropic(u, v, gx.N, gy.N);
      }

      // [q] = 1
      // [beta] = 1
      // [mu] = 1
      // [out] = 1
      private static double calcA(double q, double beta, double mu)
      {
         double t = beta / (2 * mu);
         return -t + Sqrt(sqr(t) + sqr(PI / q));
      }

      // [q] = 1
      // [beta] = 1
      // [mu] = 1
      // [out] = 1
      private static double calcB(double q, double beta, double mu)
      {
         double t = beta / (2 * mu);
         return -t - Sqrt(sqr(t) + sqr(PI / q));
      }

      // [q] = 1
      // [f1] = 1
      // [mu] = 1
      // [out] = 1
      private static double calcC3(double q, double f1, double mu)
      {
         return -f1 / mu * sqr(q / PI) + 1;
      }

      // [r] = [q] = 1
      // [f2] = 1
      // [beta] = 1
      // [mu] = 1
      // [out] = 1
      private static double calcC4(double r, double q, double f2, double beta, double mu)
      {
         return -f2 * beta * PI / r /
                (sqr(mu * (sqr(PI / r) + sqr(PI / q))) + sqr(beta * PI / r));
      }

      // [r] = [q] = 1
      // [f2] = 1
      // [beta] = 1
      // [mu] = 1
      // [out] = 1
      private static double calcC5(double r, double q, double f2, double beta, double mu)
      {
         double t = mu * (sqr(PI / r) + sqr(PI / q));
         return -f2 * t / (sqr(t) + sqr(beta * PI / r));
      }

      // [r] = 1
      // [a] = [b] = 1
      // [c3] = [c4] = 1
      // [out] = 1
      private static double calcC1(double r, double a, double b, double c3, double c4)
      {
         return (1 - c3 + c4 - (1 - c3 - c4) * Exp(b * r)) / (Exp(a * r) - Exp(b * r));
      }

      // [c1] = [c3] = [c4] = 1
      // [out] = 1
      private static double calcC2(double c1, double c3, double c4)
      {
         return 1 - c3 - c4 - c1;
      }

      // [x] = 1
      // [out] = 1
      private static double sqr(double x)
      {
         return Pow(x, 2);
      }

      // [r] = 1
      // [m] = 1
      // [out] = 1
      private static double calcRm(double r, int m)
      {
         return PI * m / r;
      }

      // [q] = 1
      // [k] = 1
      // [out] = 1
      private static double calcQk(double q, int k)
      {
         return PI * k / q;
      }
   }
}