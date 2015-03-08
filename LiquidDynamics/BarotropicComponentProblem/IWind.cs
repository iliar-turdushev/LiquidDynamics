namespace BarotropicComponentProblem
{
   public interface IWind
   {
      double TauX(double x, double y);
      double TauY(double x, double y);
   }
}