using Mathematics.Numerical;

namespace BarotropicComponentProblem
{
   public interface IDynamicProblem : IBarotropicComponentProblem, IStommelModelProblem
   {
      void SetInitialCondition(InitialCondition initialCondition);
      SquareGridFunction TransformU(SquareGridFunction u);
      SquareGridFunction TransformV(SquareGridFunction v);
   }
}