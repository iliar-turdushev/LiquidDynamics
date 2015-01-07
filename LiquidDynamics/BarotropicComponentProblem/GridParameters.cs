using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem
{
   public sealed class GridParameters
   {
      public GridParameters(double tau, Grid x, Grid y)
      {
         Check.NotNull(x, "x");
         Check.NotNull(y, "y");

         Tau = tau;
         X = x;
         Y = y;
      }

      public double Tau { get; private set; }
      public Grid X { get; private set; }
      public Grid Y { get; private set; }
   }
}