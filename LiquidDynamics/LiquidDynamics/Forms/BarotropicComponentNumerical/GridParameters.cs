namespace LiquidDynamics.Forms.BarotropicComponentNumerical
{
   internal sealed class GridParameters
   {
      public GridParameters(double tau, int n, int m)
      {
         Tau = tau;
         N = n;
         M = m;
      }

      internal double Tau { get; private set; }
      internal int N { get; private set; }
      internal int M { get; private set; }
   }
}