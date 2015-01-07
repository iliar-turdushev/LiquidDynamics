namespace ModelProblem.Barotropic
{
   public interface IBarotropicComponent
   {
      double U(double t, double x, double y);
      double V(double t, double x, double y);
   }
}