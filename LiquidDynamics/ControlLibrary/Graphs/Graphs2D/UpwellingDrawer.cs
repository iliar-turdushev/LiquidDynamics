using System.Drawing;
using Common;
using ControlLibrary.Types;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal sealed class UpwellingDrawer : IGraphDrawer
   {
      private readonly UpwellingData _upwellingData;
      private readonly IPaletteDrawingTools _paletteDrawingTools;

      public UpwellingDrawer(UpwellingData upwellingData, IPaletteDrawingTools paletteDrawingTools)
      {
         Check.NotNull(upwellingData, "upwellingData");
         Check.NotNull(paletteDrawingTools, "paletteDrawingTools");

         _upwellingData = upwellingData;
         _paletteDrawingTools = paletteDrawingTools;
      }

      public void Draw(Graphics drawingContext, CoordinatesConverter converter)
      {
         var n = _upwellingData.GridPoints.GetLength(0);
         var m = _upwellingData.GridPoints.GetLength(1);

         var width = _upwellingData.CellSize.Width;
         var halfWidth = width / 2;
         var height = _upwellingData.CellSize.Height;
         var halfHeight = height / 2;

         var minLength = _upwellingData.GetMinIntensity();
         var maxLength = _upwellingData.GetMaxIntensity();

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               var gridPoint = _upwellingData.GridPoints[i, j];
               var intensity = _upwellingData.Intensities[i, j];

               var value = _paletteDrawingTools.ToDensity(intensity, minLength, maxLength);
               var brush = _paletteDrawingTools.GetBrush(value);
               var pen = _paletteDrawingTools.GetPen(value);

               var rectangle = converter.RectangleToScreen(gridPoint.X - halfWidth,
                                                           gridPoint.Y + halfHeight,
                                                           width, height);
               drawingContext.DrawRectangle(pen,
                                            rectangle.Left, rectangle.Top,
                                            rectangle.Width, rectangle.Height);
               drawingContext.FillRectangle(brush, rectangle);
            }
         }
      }
   }
}