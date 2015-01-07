namespace BarotropicComponentProblem
{
   public interface IBarotropicComponentProblem : IProblem
   {
      double F(int i, int j);
      double G(int i, int j);
   }
}