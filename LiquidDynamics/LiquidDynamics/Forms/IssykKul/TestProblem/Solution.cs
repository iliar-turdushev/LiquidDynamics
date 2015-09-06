using Common;
using ControlLibrary.Types;

namespace LiquidDynamics.Forms.IssykKul.TestProblem
{
   internal sealed class Solution
   {
      internal Solution(SquareVelocityField velocityField, UpwellingData upwellingData, Bounds bounds)
      {
         Check.NotNull(velocityField, "velocityField");
         Check.NotNull(upwellingData, "upwellingData");
         Check.NotNull(bounds, "bounds");

         VelocityField = velocityField;
         UpwellingData = upwellingData;
         Bounds = bounds;
      }

      internal SquareVelocityField VelocityField { get; private set; }
      internal UpwellingData UpwellingData { get; private set; }
      internal Bounds Bounds { get; private set; }
   }
}