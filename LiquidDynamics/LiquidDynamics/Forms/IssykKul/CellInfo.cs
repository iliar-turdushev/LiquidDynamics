using System.Drawing;

namespace LiquidDynamics.Forms.IssykKul
{
   internal sealed class CellInfo
   {
      public CellInfo(RectangleF rectangle, float depth)
      {
         Rectangle = rectangle;
         Depth = depth;
      }

      public RectangleF Rectangle { get; private set; }
      public float Depth { get; private set; }
   }
}