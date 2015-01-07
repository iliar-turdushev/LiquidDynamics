using System.Drawing;
using Common;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal sealed class FilledRectangleDrawer : IGraphDrawer
   {
      private readonly RectangleF _rectangle;
      private readonly Pen _pen;
      private readonly Brush _brush;

      public FilledRectangleDrawer(RectangleF rectangle, Pen pen, Brush brush)
      {
         Check.NotNull(rectangle, "rectangle");
         Check.NotNull(pen, "pen");
         Check.NotNull(brush, "brush");

         _rectangle = rectangle;
         _pen = pen;
         _brush = brush;
      }

      public void Draw(Graphics drawingContext, CoordinatesConverter converter)
      {
         var rectangleToDraw = converter.RectangleToScreen(_rectangle);
         drawingContext.FillRectangle(_brush, rectangleToDraw);
         drawingContext.DrawRectangle(_pen, rectangleToDraw.X, rectangleToDraw.Y,
                                      rectangleToDraw.Width, rectangleToDraw.Height);
      }
   }
}