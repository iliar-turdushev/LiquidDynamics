using Mathematics.Numerical;

namespace BarotropicComponentProblem
{
   public interface IDynamicProblem : IBarotropicComponentProblem
   {
      void SetInitialCondition(InitialCondition initialCondition);
      SquareGridFunction TransformU(SquareGridFunction u);
      SquareGridFunction TransformV(SquareGridFunction v);
   }
}