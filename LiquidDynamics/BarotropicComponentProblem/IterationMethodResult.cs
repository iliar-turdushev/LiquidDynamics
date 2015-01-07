using BarotropicComponentProblem.IterationMethod;
using Mathematics.Numerical;

namespace BarotropicComponentProblem
{
   public sealed class IterationMethodResult
   {
      public IterationMethodResult(BarotropicComponent barotropicComponent, IterationResult<SquareGridFunction> iterationResultU, IterationResult<SquareGridFunction> iterationResultV)
      {
         BarotropicComponent = barotropicComponent;
         IterationResultU = iterationResultU;
         IterationResultV = iterationResultV;
      }

      public BarotropicComponent BarotropicComponent { get; private set; }
      public IterationResult<SquareGridFunction> IterationResultU { get; private set; } 
      public IterationResult<SquareGridFunction> IterationResultV { get; private set; } 
   }
}