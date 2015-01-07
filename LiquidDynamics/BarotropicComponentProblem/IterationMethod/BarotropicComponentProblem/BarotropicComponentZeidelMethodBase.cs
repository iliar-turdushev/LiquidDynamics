using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod.BarotropicComponentProblem
{
   internal abstract class BarotropicComponentZeidelMethodBase : ZeidelMethodBase
   {
      private readonly IBarotropicComponentProblem _problem;

      protected BarotropicComponentZeidelMethodBase(IBarotropicComponentProblem problem, Grid x, Grid y)
         : base(problem, x, y)
      {
         _problem = problem;
      }

      protected double f(int i, int j)
      {
         return _problem.F(i, j);
      }

      protected double g(int i, int j)
      {
         return _problem.G(i, j);
      }
   }
}