using System;
using Common;

namespace ModelProblem.Barotropic
{
   internal sealed class BarotropicComponent : IBarotropicComponent
   {
      private const double Pi = Math.PI;
      
      private static readonly Func<double, double> Exp = Math.Exp;
      private static readonly Func<double, double> Cos = Math.Cos;
      private static readonly Func<double, double> Sin = Math.Sin;
      private static readonly Func<double, double> Sqr = value => value * value;
      private static readonly Func<double, double> Sqrt = Math.Sqrt;

      private readonly Constants _constants;
      private readonly Parameters _parameters;

      private Func<double, double, double, double> _u;
      private Func<double, double, double, double> _v;

      public BarotropicComponent(Constants constants, Parameters parameters)
      {
         Check.NotNull(constants, "constants");
         Check.NotNull(parameters, "parameters");
         
         _constants = constants;
         _parameters = parameters;

         initialize();
      }

      public double U(double t, double x, double y)
      {
         return _u(t, x, y);
      }

      public double V(double t, double x, double y)
      {
         return _v(t, x, y);
      }

      private void initialize()
      {
         var h = _parameters.H;

         var q = _parameters.SmallQ;
         var r = _parameters.SmallR;
         var rm = _constants.SmallRm;
         var qk = _constants.SmallQk;

         var a = _constants.A;
         var b = _constants.B;

         var c1 = _constants.C1;
         var c2 = _constants.C2;
         var c3 = _constants.C3;
         var c4 = _constants.C4;
         var c5 = _constants.C5;

         var s1 = _parameters.S1;
         var s2 = _parameters.S2;

         var mu = _parameters.Mu;
         var beta = _parameters.Beta;

         var sqrt = Sqrt(Sqr(rm) + Sqr(qk));

         var t1 = Pi / q;
         var t2 = Pi / r;
         var t3 = beta / 2.0 / sqrt;

         _u =
            (t, x, y) =>
               {
                  var value = x * sqrt + t3 * t;
                  var integralU = 
                     -t1 * (1.0 - c1 * Exp(a * x) - c2 * Exp(b * x) - c3 - c4 * Cos(t2 * x) - c5 * Sin(t2 * x)) * Cos(t1 * y) -
                     2.0 * qk * Sin(rm * x) * Cos(qk * y) * (s1 * Cos(value) - s2 * Sin(value)) * Exp(-mu * t);
                  return integralU / h;
               };

         _v =
            (t, x, y) =>
               {
                  var value = x * sqrt + t3 * t;
                  var cos = Cos(value);
                  var sin = Sin(value);
                  var integralV = 
                     -(c1 * a * Exp(a * x) + c2 * b * Exp(b * x) - c4 * t2 * Sin(t2 * x) + c5 * t2 * Cos(t2 * x)) * Sin(t1 * y) +
                     2.0 * Sin(qk * y) * (rm * Cos(rm * x) * (s1 * cos - s2 * sin) + sqrt * Sin(rm * x) * (-s1 * sin - s2 * cos)) * Exp(-mu * t);
                  return integralV / h;
               };
      }
   }
}
