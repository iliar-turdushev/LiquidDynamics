namespace BarotropicComponentProblem
{
   public interface IStommelModelProblem : IProblem
   {
      double F(int i, int j);
   }
}