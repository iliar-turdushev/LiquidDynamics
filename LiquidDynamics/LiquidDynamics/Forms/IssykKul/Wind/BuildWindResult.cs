using Common;
using ControlLibrary.Types;

namespace LiquidDynamics.Forms.IssykKul.Wind
{
   internal sealed class BuildWindResult
   {
      public BuildWindResult(SquareVelocityField windField, Bounds bounds)
      {
         Check.NotNull(windField, "windField");
         Check.NotNull(bounds, "bounds");

         WindField = windField;
         Bounds = bounds;
      }

      internal SquareVelocityField WindField { get; private set; }
      internal Bounds Bounds { get; private set; }
   }
}