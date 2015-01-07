namespace BarotropicComponentProblem
{
   public interface IProblem
   {
      double Epsilon(int i, IndexOffset offset, int j);
      double K(int i, int j, IndexOffset offset);
      double A(int i, IndexOffset offset, int j);
      double B(int i, int j, IndexOffset offset);
   }
}