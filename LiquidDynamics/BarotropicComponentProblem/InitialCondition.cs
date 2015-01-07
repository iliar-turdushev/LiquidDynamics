using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem
{
   public sealed class InitialCondition
   {
      public InitialCondition(SquareGridFunction u, SquareGridFunction v)
      {
         Check.NotNull(u, "u");
         Check.NotNull(v, "v");

         U = u;
         V = v;
      }

      public SquareGridFunction U { get; private set; }
      public SquareGridFunction V { get; private set; }
   }
}