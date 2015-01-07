using System.Drawing;
using System.Linq;
using Common;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal sealed class CurveDrawer : IGraphDrawer
   {
      private readonly PointF[] _points;
      private readonly Pen _pen;

      public CurveDrawer(PointF[] points, Pen pen)
      {
         Check.NotNull(points, "points");
         Check.NotNull(pen, "pen");

         _points = points;
         _pen = pen;
      }

      public void Draw(Graphics drawingContext, CoordinatesConverter converter)
      {
         var pointsToDraw = _points.Select(converter.PointToScreen).ToArray();
         drawingContext.DrawCurve(_pen, pointsToDraw);
      }
   }
}