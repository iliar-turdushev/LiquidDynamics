using System.Drawing;
using System.Linq;
using Common;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal sealed class PointsDrawer : IGraphDrawer
   {
      private readonly PointF[] _points;
      private readonly Brush _brush;

      public PointsDrawer(PointF[] points, Brush brush)
      {
         Check.NotNull(points, "points");
         Check.NotNull(brush, "brush");

         _points = points;
         _brush = brush;
      }

      public void Draw(Graphics drawingContext, CoordinatesConverter converter)
      {
         var size = new SizeF(0.3F, 0.3F);
         var rectangles =
            _points
               .Select(point => converter.RectangleToScreen(new RectangleF(point, size)))
               .ToArray();

         drawingContext.FillRectangles(_brush, rectangles);
      }
   }
}