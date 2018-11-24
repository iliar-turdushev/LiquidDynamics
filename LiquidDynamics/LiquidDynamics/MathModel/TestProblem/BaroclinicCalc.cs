using System.Numerics;
using static System.Math;
using static System.Numerics.Complex;

namespace LiquidDynamics.MathModel.TestProblem
{
   internal sealed class BaroclinicCalc
   {
      private static readonly Complex I = ImaginaryOne;

      private readonly double _r; // 1
      private readonly double _q; // 1
      private readonly double _h; // 1

      private readonly double _f1; // 1
      private readonly double _f2; // 1

      private readonly double _l0; // 1
      private readonly double _beta; // 1

      private readonly double _mu; // 1

      private readonly double _nu; // 1

      private readonly int _m; // 1
      private readonly int _k; // 1

      private readonly double _s1; // 1
      private readonly double _s2; // 1

      private readonly Grid _gx; // 1
      private readonly Grid _gy; // 1
      private readonly Grid _gz; // 1

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
      private readonly double _alpha; // 1

      // [r] = [q] = [h] = 1
      // [f1] = [f2] = 1
      // [l0] = 1
      // [beta] = 1
      // [mu] = 1
      // [nu] = 1
      // [m] = [k] = 1
      // [s1] = [s2] = 1
      // [gx] = [gy] = [gz] = 1
      // [tau] = 1
      public BaroclinicCalc(
         double r, double q, double h,
         double f1, double f2,
         double l0, double beta,
         double mu,
         double nu,
         int m, int k,
         double s1, double s2,
         Grid gx, Grid gy, Grid gz,
         double tau
         )
      {
         _r = r;
         _q = q;
         _h = h;
         _f1 = f1;
         _f2 = f2;
         _l0 = l0;
         _beta = beta;
         _mu = mu;
         _nu = nu;
         _m = m;
         _k = k;
         _s1 = s1;
         _s2 = s2;
         _gx = gx;
         _gy = gy;
         _gz = gz;
         _tau = tau;

         _a = calcA(q, beta, mu);
         _b = calcB(q, beta, mu);

         _c3 = calcC3(q, f1, mu);
         _c4 = calcC4(r, q, f2, beta, mu);
         _c5 = calcC5(r, q, f2, beta, mu);
         _c1 = calcC1(r, _a, _b, _c3, _c4);
         _c2 = calcC2(_c1, _c3, _c4);

         _rm = PI * m / r;
         _qk = PI * k / q;
         _sqrt = Sqrt(sqr(_rm) + sqr(_qk));
         _alpha = beta / (2 * _sqrt);

         T = -_tau;
      }

      // [out] = 1
      public double T { get; private set; }

      // [out] = 1
      public Baroclinic Next()
      {
         T += _tau;
         return calcBaroc(T);
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

      // [out] = 1
      private Baroclinic calcBaroc(double t)
      {
         .
      }

      // [y] = 1
      // [out] = 1
      private double calcL(double y)
      {
         return _l0 + _beta * y;
      }

      // [y] = 1
      // [out] = 1
      private Complex calcLamda1(double l)
      {
         return (1 + I) * Sqrt(l / (2 * _nu));
      }

      // [y] = 1
      // [out] = 1
      private Complex calcLamda2(double l)
      {
         double d = Sqrt(sqr(_mu) + sqr(l + _alpha));
         return (l + _alpha) / Sqrt(2 * _nu * (_mu + d)) + I * Sqrt((_mu + d) / (2 * _nu));
      }

      // [y] = 1
      // [out] = 1
      private Complex calcLamda3(double l)
      {
         double e = Sqrt(sqr(_mu) + sqr(l - _alpha));
         return (l - _alpha) / Sqrt(2 * _nu * (_mu + e)) + I * Sqrt((_mu + e) / (2 * _nu));
      }

      // [x] = [y] = 1
      // [out] = 1
      private Complex calcAf(double x, double y)
      {
         double ea = Exp(_a * x);
         double eb = Exp(_b * x);

         double cosr = Cos(PI * x / _r);
         double sinr = Sin(PI * x / _r);
         double cosq = Cos(PI * y / _q);
         double sinq = Sin(PI * y / _q);

         return -PI / _q * (1 - _c1 * ea - _c2 * eb - _c3 - _c4 * cosr - _c5 * sinr) * cosq -
                I * (_c1 * _a * ea + _c2 * _b * eb - _c4 * PI / _r * sinr + _c5 * PI / _r * cosr) * sinq;
      }

      // [x] = [y] = 1
      // [out] = 1
      private Complex calcBf(double x, double y)
      {
         .
      }

      // [x] = [y] = 1
      // [out] = 1
      private Complex calcCf(double x, double y)
      {
         .
      }
   }
}