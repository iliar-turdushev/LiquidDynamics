using System;
using Common;

namespace ModelProblem
{
   internal sealed class Constants
   {
      private const double Pi = Math.PI;

      private static readonly Func<double, double> Exp = Math.Exp;
      private static readonly Func<double, double> Sqr = value => value * value;
      private static readonly Func<double, double> Sqrt = Math.Sqrt;

      private readonly Parameters _parameters;

      public Constants(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");

         _parameters = parameters;
         initialize();
      }
      
      public double A { get; private set; }
      public double B { get; private set; }

      public double C1 { get; private set; }
      public double C2 { get; private set; }
      public double C3 { get; private set; }
      public double C4 { get; private set; }
      public double C5 { get; private set; }

      public double SmallQk { get; private set; }
      public double SmallRm { get; private set; }

      private void initialize()
      {
         var q = _parameters.SmallQ;
         var r = _parameters.SmallR;

         var beta = _parameters.Beta;
         var mu = _parameters.Mu;

         var f1 = _parameters.F1;
         var f2 = _parameters.F2;

         var t1 = beta / 2.0 / mu;
         var t2 = Pi / q;
         var t2Sqr = Sqr(t2);
         var t3 = Sqrt(Sqr(t1) + t2Sqr);
         var t4 = Pi / r;
         var t4Sqr = Sqr(t4);
         var t5 = t2Sqr + t4Sqr;
         var t6 = Sqr(beta) * t4Sqr;
         var t7 = Sqr(mu) * Sqr(t5) + t6;
         
         A = -t1 + t3;
         B = -t1 - t3;

         var expA = Exp(A * r);
         var expB = Exp(B * r);

         C3 = -f1 / mu / t2Sqr + 1.0;
         C4 = -f2 * beta * t4 / t7;
         C5 = -f2 * mu * t5 / t7;
         C1 = (1.0 - C3 + C4 - (1.0 - C3 - C4) * expB) / (expA - expB);
         C2 = 1.0 - C3 - C4 - C1;

         var k = _parameters.SmallK;
         var m = _parameters.SmallM;

         SmallQk = Pi * k / q;
         SmallRm = Pi * m / r;
      }
   }
}