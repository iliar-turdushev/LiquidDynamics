using BarotropicComponentProblem.Kriging;

namespace LiquidDynamics.Forms.Kriging
{
   internal sealed class KrigingParameters
   {
      public int N { get; set; }
      public int M { get; set; }
      public int Nodes { get; set; }
      public IVariogram Variogram { get; set; }
   }
}