using static System.Math;

namespace LiquidDynamics.MathModel.TestProblem
{
   internal sealed class BarotropicCalc
   {
      private readonly double _h; // 1
      private readonly double _beta; // 1
      private readonly double _mu; // 1
      private readonly double _s1; // 1
      private readonly double _s2; // 1

      private readonly Grid _gx; // 1
      private readonly Grid _gy; // 1
      private readonly double _tau; // 1

      private readonly double _a; // 1
      private readonly double _b; // 1

      private readonly double _c1; // 1
      private readonly double _c2; // 1
      private readonly double _c3; // 1
      private readonly double _c4; // 1
      private readonly double _c5; // 1

      private readonly double _rm; // 1
      private readonly double _qk; // 1
      private readonly double _sqrt; // 1

      private readonly double _pir; // 1
      private readonly double _piq; // 1

      // [r] = [q] = [h] = 1
      // [f1] = [f2] = 1
      // [beta] = 1
      // [mu] = 1
      // [m] = [k] = 1
      // [s1] = [s2] = 1
      // [gx] = [gy] = 1
      // [tau] = 1
      public BarotropicCalc(
         double r, double q, double h,
         double f1, double f2,
         double beta,
         double mu,
         int m, int k,
         double s1, double s2,
         Grid gx, Grid gy,
         double tau
         )
      {
         _h = h;
         _beta = beta;
         _mu = mu;
         _s1 = s1;
         _s2 = s2;
         _gx = gx;
         _gy = gy;
         _tau = tau;

         _a = calcA(q, beta, mu);
         _b = calcB(q, beta, mu);

         _c3 = calcC3(q, f1, mu);
         _c4 = calcC4(r, q, f2, beta, mu);
         _c5 = calcC5(r, q, f2, beta, mu);
         _c1 = calcC1(r, _a, _b, _c3, _c4);
         _c2 = calcC2(_c1, _c3, _c4);

         _rm = calcRm(r, m);
         _qk = calcQk(q, k);
         _sqrt = Sqrt(sqr(_rm) + sqr(_qk));

         _pir = PI / r;
         _piq = PI / q;
      }

      // [out] = 1
      public double T { get; private set; }

      // [out] = 1
      public Barotropic Begin()
      {
         T = 0;
         return calcBarot(T);
      }

      // [out] = 1
      public Barotropic Step()
      {
         T += _tau;
         return calcBarot(T);
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

      // [out] = 1
      private Barotropic calcBarot(double t)
      {
         double btsqrt = _beta * t / (2 * _sqrt);
         double emut = Exp(-_mu * t);

         double[,] u = new double[_gx.N, _gy.N];
         double[,] v = new double[_gx.N, _gy.N];

         for (int i = 0; i < _gx.N; i++)
         {
            double x = _gx[i];

            double ea = Exp(_a * x);
            double eb = Exp(_b * x);

            double cosr = Cos(_pir * x);
            double sinr = Sin(_pir * x);

            double cosrm = Cos(_rm * x);
            double sinrm = Sin(_rm * x);

            double arg = x * _sqrt + btsqrt;
            double cosarg = Cos(arg);
            double sinarg = Sin(arg);

            for (int j = 0; j < _gy.N; j++)
            {
               double y = _gy[j];

               double cosq = Cos(_piq * y);
               double sinq = Sin(_piq * y);

               double cosqk = Cos(_qk * y);
               double sinqk = Sin(_qk * y);

               u[i, j] = (-_piq * cosq * (1 - _c1 * ea - _c2 * eb - _c3 - _c4 * cosr - _c5 * sinr) -
                          2 * _qk * sinrm * cosqk * (_s1 * cosarg - _s2 * sinarg) * emut) / _h;

               v[i, j] = (-sinq * (_c1 * _a * ea + _c2 * _b * eb - _c4 * _pir * sinr + _c5 * _pir * cosr) +
                          2 * sinqk * emut * (_rm * cosrm * (_s1 * cosarg - _s2 * sinarg) +
                                              _sqrt * sinrm * (-_s1 * sinarg - _s2 * cosarg))) / _h;
            }
         }

         return new Barotropic(u, v, _gx.N, _gy.N);
      }
   }
}