using Common;

namespace BarotropicComponentProblem.Kriging
{
   public sealed class KrigingMethodParameters
   {
      public KrigingMethodParameters(int nodes, IVariogram variogram)
      {
         Check.NotNull(variogram, "variogram");

         Nodes = nodes;
         Variogram = variogram;
      }

      public int Nodes { get; private set; }
      public IVariogram Variogram { get; private set; }
   }
}