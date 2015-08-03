using Common;
using ControlLibrary.Types;

namespace LiquidDynamics.Forms.IssykKul.TestProblem
{
   internal sealed class Solution
   {
      internal Solution(SquareVelocityField velocityField, Bounds bounds)
      {
         Check.NotNull(velocityField, "velocityField");
         Check.NotNull(bounds, "bounds");

         VelocityField = velocityField;
         Bounds = bounds;
      }

      internal SquareVelocityField VelocityField { get; private set; }
      internal Bounds Bounds { get; private set; }
   }
}