using System.Drawing;
using Common;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal sealed class RectangleDrawer : IGraphDrawer
   {
      private readonly RectangleF _rectangle;
      private readonly Pen _pen;

      public RectangleDrawer(RectangleF rectangle, Pen pen)
      {
         Check.NotNull(rectangle, "rectangle");
         Check.NotNull(pen, "pen");

         _rectangle = rectangle;
         _pen = pen;
      }

      public void Draw(Graphics drawingContext, CoordinatesConverter converter)
      {
         var rectangleToDraw = converter.RectangleToScreen(_rectangle);
         drawingContext.DrawRectangle(_pen, rectangleToDraw.X, rectangleToDraw.Y,
                                      rectangleToDraw.Width, rectangleToDraw.Height);
      }
   }
}