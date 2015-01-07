using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod.StommelModel
{
   internal abstract class StommelModelZeidelMethodBase : ZeidelMethodBase
   {
      private readonly IStommelModelProblem _problem;

      protected StommelModelZeidelMethodBase(IStommelModelProblem problem, Grid x, Grid y)
         : base(problem, x, y)
      {
         _problem = problem;
      }

      protected double f(int i, int j)
      {
         return _problem.F(i, j);
      }
   }
}