using Mathematics.MathTypes;

namespace ModelProblem.Baroclinic
{
   public interface IBaroclinicComponent
   {
      Complex Theta(double t, double x, double y, double z);
   }
}