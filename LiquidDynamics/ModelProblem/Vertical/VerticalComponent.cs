using System;
using Common;

namespace ModelProblem.Vertical
{
   internal sealed class VerticalComponent : IVerticalComponent
   {
      private const double Pi = Math.PI;

      private static readonly Func<double, double> Sin = Math.Sin;
      private static readonly Func<double, double> Cos = Math.Cos;
      private static readonly Func<double, double> Exp = Math.Exp;
      private static readonly Func<double, double> Sqr = value => value * value; 
      private static readonly Func<double, double> Sqrt = Math.Sqrt; 
      private static readonly Func<double, double> Sinh = Math.Sinh; 
      private static readonly Func<double, double> Cosh = Math.Cosh; 
      
      private readonly Constants _constants;
      private readonly Parameters _parameters;

      private Func<double, double, double, double, double> _w;

      public VerticalComponent(Constants constants, Parameters parameters)
      {
         Check.NotNull(constants, "constants");
         Check.NotNull(parameters, "parameters");

         _constants = constants;
         _parameters = parameters;

         initialize();
      }

      public double W(double t, double x, double y, double z)
      {
         return _w(t, x, y, z);
      }

      private void initialize()
      {
         var q = _parameters.SmallQ;
         var rho0 = _parameters.Rho0;
         var c1 = _constants.C1;
         var c2 = _constants.C2;
         var c3 = _constants.C3;
         var a = _constants.A;
         var b = _constants.B;

         var aSqr = Sqr(a);
         var bSqr = Sqr(b);
         
         var t1 = Pi / q;
         var t1Sqr = Sqr(t1);

         Func<double, double> tauXDy =
            y => _parameters.F1 * rho0 * Sin(t1 * y);

         Func<double, double, double> reADx =
            (x, y) => t1 * (c1 * a * Exp(a * x) + c2 * b * Exp(b * x)) * Cos(t1 * y);

         Func<double, double, double> reADy =
            (x, y) => t1Sqr * (1.0 - c1 * Exp(a * x) - c2 * Exp(b * x) - c3) * Sin(t1 * y);

         Func<double, double, double> imADx =
            (x, y) => -(c1 * aSqr * Exp(a * x) + c2 * bSqr * Exp(b * x)) * Sin(t1 * y);

         Func<double, double, double> imADy =
            (x, y) => -t1 * (c1 * a * Exp(a * x) + c2 * b * Exp(b * x)) * Cos(t1 * y);

         var l = _parameters.SmallL0;
         var nu = _parameters.Nu;
         var t2 = Sqrt(l / 2.0 / nu);
         
         Func<double, double> smallF1 = z => Cos(t2 * z) * Sinh(t2 * z);
         Func<double, double> smallF2 = z => Sin(t2 * z) * Cosh(t2 * z);

         var mu = _parameters.Mu;
         var t3 = Sqr(mu) + Sqr(l);
         var d = Sqrt(t3);
         var t4 = Sqrt((mu + d) / 2.0 / nu);
         var t5 = l / Sqrt((mu + d) * 2.0 * nu);

         Func<double, double> smallG1 = z => Cos(t4 * z) * Sinh(t5 * z);
         Func<double, double> smallG2 = z => Sin(t4 * z) * Cosh(t5 * z);

         var t6 = nu / l;
         var t7 = nu / t3;

         Func<double, double> i1 = z => t6 * smallF2(z);
         Func<double, double> i2 = z => -t6 * smallF1(z);
         Func<double, double> i3 = z => -t7 * (mu * smallG1(z) - l * smallG2(z));
         Func<double, double> i4 = z => -t7 * (l * smallG1(z) + mu * smallG2(z));

         var s1 = _parameters.S1;
         var rm = _constants.SmallRm;
         var qk = _constants.SmallQk;
         var qkSqr = Sqr(qk);
         var rmSqr = Sqr(rm);
         var sqrt = Sqrt(rmSqr + qkSqr);

         var t8 = 2.0 * s1 * qk;

         Func<double, double, double> eta1Dx =
            (x, y) =>
               {
                  var e = t8 * Cos(qk * y);
                  var rmx = rm * x;
                  var sqrtx = sqrt * x;
                  return -e * (rm * Cos(rmx) * Cos(sqrtx) - sqrt * Sin(rmx) * Sin(sqrtx));
               };

         var t9 = 2.0 * s1 * qkSqr;

         Func<double, double, double> eta1Dy =
            (x, y) => t9 * Sin(rm * x) * Sin(qk * y) * Cos(sqrt * x);

         var t10 = 2.0 * s1 * (2.0 * rmSqr + qkSqr);
         var t11 = 4.0 * s1 * rm * sqrt;
         
         Func<double, double, double> eta2Dx =
            (x, y) =>
               {
                  var rmx = rm * x;
                  var sqrtx = sqrt * x;
                  var e = Sin(qk * y);
                  return -t10 * Sin(rmx) * e * Cos(sqrtx) - t11 * Cos(rmx) * e * Sin(sqrtx);
               };

         Func<double, double, double> eta2Dy =
            (x, y) =>
               {
                  var e = t8 * Cos(qk * y);
                  var rmx = rm * x;
                  var sqrtx = sqrt * x;
                  return rm * Cos(rmx) * e * Cos(sqrtx) - sqrt * Sin(rmx) * e * Sin(sqrtx);
               };

         Func<double, double, double, double> g1 =
            (x, y, z) => 2.0 * (eta1Dx(x, y) * i3(z) - eta2Dx(x, y) * i4(z));

         Func<double, double, double, double> g2 =
            (x, y, z) => 2.0 * (eta2Dx(x, y) * i3(z) + eta1Dx(x, y) * i4(z));

         Func<double, double, double, double> g3 =
            (x, y, z) => 2.0 * (eta1Dy(x, y) * i3(z) - eta2Dy(x, y) * i4(z));

         Func<double, double, double, double> g4 =
            (x, y, z) => 2.0 * (eta2Dy(x, y) * i3(z) + eta1Dy(x, y) * i4(z));

         var t12 = 2.0 * rho0 * mu;

         Func<double, double, double, double> f1 =
            (x, y, z) => t12 * (reADx(x, y) * i1(z) - imADx(x, y) * i2(z));

         Func<double, double, double, double> f2 =
            (x, y, z) => t12 * (imADx(x, y) * i1(z) + reADx(x, y) * i2(z));

         var h = _parameters.H;

         Func<double, double, double, double> f3 =
            (x, y, z) => 2.0 * tauXDy(y) * i1(h - z) +
                         t12 * (reADy(x, y) * i1(z) - imADy(x, y) * i2(z));

         Func<double, double, double, double> f4 =
            (x, y, z) => 2.0 * tauXDy(y) * i2(h - z) +
                         t12 * (imADy(x, y) * i1(z) + reADy(x, y) * i2(z));

         Func<double, double, double, double, double> qFunc =
            (t, x, y, z) =>
               {
                  var f1H = smallF1(h);
                  var f2H = smallF2(h);
                  var g1H = smallG1(h);
                  var g2H = smallG2(h);

                  var value =
                     ((f1(x, y, z) + f4(x, y, z)) * f1H + (f2(x, y, z) - f3(x, y, z)) * f2H) /
                     (2.0 * rho0 * (Sqr(f1H) + Sqr(f2H))) +
                     ((g1(x, y, z) + g4(x, y, z)) * g1H + (g2(x, y, z) - g3(x, y, z)) * g2H) *
                     mu * Exp(-mu * t) / (2.0 * (Sqr(g1H) + Sqr(g2H)));

                  return value / nu;
               };

         _w =
            (t, x, y, z) =>
               {
                  var c = qFunc(t, x, y, 0.0);
                  return qFunc(t, x, y, z) - (qFunc(t, x, y, h) - c) * z / h - c;
               };
      }
   }
}