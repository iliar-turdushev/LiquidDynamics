using System;
using Common;
using Mathematics.MathTypes;

namespace ModelProblem
{
   internal sealed class Functions
   {
      private const double Pi = Math.PI;
      private static readonly Complex I = Complex.I;

      private static readonly Func<double, double> Sqrt = Math.Sqrt;
      private static readonly Func<double, double> Sqr = value => value * value;
      private static readonly Func<double, double> Cos = Math.Cos;
      private static readonly Func<double, double> Sin = Math.Sin;
      private static readonly Func<double, double> Exp = Math.Exp;
      private static readonly Func<Complex, Complex> CExp = Complex.Exp; 

      private Func<double, Complex> _lamda1;
      private Func<double, Complex> _lamda2;
      private Func<double, Complex> _lamda3; 

      private Func<double, double> _tauX;
      private Func<double, double, double> _tauY;

      private Func<double, double, Complex> _a;
      private Func<double, double, Complex> _b;
      private Func<double, double, Complex> _c;

      private Func<double, double> _l; 

      private readonly Constants _constants;
      private readonly Parameters _parameters;

      public Functions(Constants constants, Parameters parameters)
      {
         Check.NotNull(constants, "constants");
         Check.NotNull(parameters, "parameters");

         _constants = constants;
         _parameters = parameters;
         initialize();
      }

      public Complex Lamda1(double y)
      {
         return _lamda1(y);
      }

      public Complex Lamda2(double y)
      {
         return _lamda2(y);
      }

      public Complex Lamda3(double y)
      {
         return _lamda3(y);
      }

      public double TauX(double y)
      {
         return _tauX(y);
      }

      public double TauY(double x, double y)
      {
         return _tauY(x, y);
      }

      public Complex A(double x, double y)
      {
         return _a(x, y);
      }

      public Complex B(double x, double y)
      {
         return _b(x, y);
      }

      public Complex C(double x, double y)
      {
         return _c(x, y);
      }

      public double L(double y)
      {
         return _l(y);
      }

      private void initialize()
      {
         var nu = _parameters.Nu;
         var mu = _parameters.Mu;
         var smallL0 = _parameters.SmallL0;
         var beta = _parameters.Beta;
         var rho0 = _parameters.Rho0;

         var f1 = _parameters.F1;
         var f2 = _parameters.F2;
         var smallQ = _parameters.SmallQ;
         var smallR = _parameters.SmallR;

         var smallQk = _constants.SmallQk;
         var smallRm = _constants.SmallRm;

         var s1 = _parameters.S1;
         var s2 = _parameters.S2;

         _l = y => smallL0 + beta * y;

         var sqrt = Sqrt(Sqr(smallRm) + Sqr(smallQk));
         var alpha = beta / (2.0 * sqrt);

         var t1 = (1.0 + I) / Sqrt(2.0 * nu);
         _lamda1 = y => t1 * Sqrt(_l(y));

         _lamda2 =
            y =>
               {
                  var v = _l(y);
                  var d = Sqrt(Sqr(mu) + Sqr(v + alpha));
                  return (v + alpha) / Sqrt(2.0 * nu * (mu + d)) + I * Sqrt((mu + d) / (2.0 * nu));
               };

         _lamda3 =
            y =>
               {
                  var v = _l(y);
                  var e = Sqrt(Sqr(mu) + Sqr(v - alpha));
                  return (v - alpha) / Sqrt(2.0 * nu * (mu + e)) + I * Sqrt((mu + e) / (2.0 * nu));
               };

         var t2 = Pi / smallQ;
         var t3 = Pi / smallR;

         var t4 = -f1 * rho0 / t2;
         _tauX = y => t4 * Cos(t2 * y);

         var t5 = f2 * rho0 / t3;
         _tauY = (x, y) => t5 * Cos(t3 * x) * Sin(t2 * y);

         var c1 = _constants.C1;
         var c2 = _constants.C2;
         var c3 = _constants.C3;
         var c4 = _constants.C4;
         var c5 = _constants.C5;
         var a = _constants.A;
         var b = _constants.B;
         
         _a =
            (x, y) =>
               {
                  var eA = Exp(a * x);
                  var eB = Exp(b * x);
                  var value = 
                     -t2 * (1.0 - c1 * eA - c2 * eB - c3 - c4 * Cos(t3 * x) - c5 * Sin(t3 * x)) * Cos(t2 * y) -
                     I * (c1 * a * eA + c2 * b * eB - c4 * t3 * Sin(t3 * x) + c5 * t3 * Cos(t3 * x)) * Sin(t2 * y);
                  return value;
               };

         var s = s1 + I * s2;
         var conjS = s1 - I * s2;

         _b =
            (x, y) =>
               {
                  var rmx = smallRm * x;
                  var qky = smallQk * y;
                  var e1 = Sin(rmx);
                  var e2 = Cos(qky);
                  var e3 = Sin(qky);
                  var e4 = Cos(rmx);
                  var e5 = CExp(I * x * sqrt);
                  
                  return 
                     (-s * smallQk * e1 * e2 + I * s * smallRm * e3 * e4 - s * sqrt * e1 * e3) * e5;
               };

         _c =
            (x, y) =>
               {
                  var rmx = smallRm * x;
                  var qky = smallQk * y;
                  var e1 = Sin(rmx);
                  var e2 = Cos(qky);
                  var e3 = Sin(qky);
                  var e4 = Cos(rmx);
                  var e5 = CExp(-I * x * sqrt);

                  return
                     (-conjS * smallQk * e1 * e2 + I * conjS * smallRm * e3 * e4 + conjS * sqrt * e1 * e3) * e5;
               };
      }
   }
}