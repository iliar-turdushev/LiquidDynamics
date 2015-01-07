using ControlLibrary.Types;

namespace LiquidDynamics.Forms.StommelModel
{
   internal sealed class StommelModelSolutionInfo
   {
      internal StommelModelSolutionInfo(BarotropicComponentInfo u, BarotropicComponentInfo v, SquareVelocityField solution)
      {
         U = u;
         V = v;
         Solution = solution;
      }

      internal BarotropicComponentInfo U { get; private set; }
      internal BarotropicComponentInfo V { get; private set; }
      internal SquareVelocityField Solution { get; private set; }
   }
}