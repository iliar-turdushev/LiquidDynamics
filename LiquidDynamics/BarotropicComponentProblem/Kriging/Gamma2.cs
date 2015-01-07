using System;

namespace BarotropicComponentProblem.Kriging
{
   public sealed class Gamma2 : IVariogram
   {
      private readonly double _c;
      private readonly double _c0;
      private readonly double _a;

      public Gamma2(double c, double c0, double a)
      {
         _c = c;
         _c0 = c0;
         _a = a;
      }

      public double GetValue(double rho)
      {
         return _c0 + (_c - _c0) * (1 - Math.Exp(-rho * rho / (_a * _a)));
      }
   }
}