namespace BarotropicComponentProblem.Kriging
{
   public interface IVariogram
   {
      double GetValue(double rho);
   }
}