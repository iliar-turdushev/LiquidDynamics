namespace BarotropicComponentProblem.Kriging
{
   public sealed class Gamma4 : IVariogram
   {
      private readonly double _c0;
      private readonly double _s;
      private readonly double _a;

      public Gamma4(double c0, double s, double a)
      {
         _c0 = c0;
         _s = s;
         _a = a;
      }

      public double GetValue(double rho)
      {
         return _c0 + (_s * rho) / (1 + _a * rho);
      }
   }
}