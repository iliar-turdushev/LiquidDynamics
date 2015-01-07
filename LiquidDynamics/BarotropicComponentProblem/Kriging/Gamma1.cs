namespace BarotropicComponentProblem.Kriging
{
   public sealed class Gamma1 : IVariogram
   {
      private readonly double _c0;
      private readonly double _s;

      public Gamma1(double c0, double s)
      {
         _c0 = c0;
         _s = s;
      }

      public double GetValue(double rho)
      {
         return _c0 + _s * rho;
      }
   }
}