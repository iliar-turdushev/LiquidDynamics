using System;
using System.Drawing;
using Common;
using ControlLibrary.Types;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal sealed class PalettedSquareVelocityFieldDrawer : IGraphDrawer
   {
      private readonly SquareVelocityField _velocityField;
      private readonly IPaletteDrawingTools _paletteDrawingTools;
      private readonly Pen _vectorPen;
      private readonly VectorScalingMode _vectorScalingMode;

      public PalettedSquareVelocityFieldDrawer(
         SquareVelocityField velocityField,
         IPaletteDrawingTools paletteDrawingTools,
         Pen vectorPen,
         VectorScalingMode vectorScalingMode)
      {
         Check.NotNull(vectorPen, "vectorPen");
         Check.NotNull(velocityField, "velocityField");
         Check.NotNull(paletteDrawingTools, "paletteDrawingTools");

         _velocityField = velocityField;
         _paletteDrawingTools = paletteDrawingTools;
         _vectorPen = vectorPen;
         _vectorScalingMode = vectorScalingMode;
      }

      public void Draw(Graphics drawingContext, CoordinatesConverter converter)
      {
         var n = _velocityField.Vectors.GetLength(0);
         var m = _velocityField.Vectors.GetLength(1);

         var width = _velocityField.CellSize.Width;
         var halfWidth = width / 2;
         var height = _velocityField.CellSize.Height;
         var halfHeight = height / 2;

         var minLength = _velocityField.GetMinVector().Length;
         var maxLength = _velocityField.GetMaxVector().Length;
         var scale = float.MinValue;

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               var vector = _velocityField.Vectors[i, j];

               if (vector == null)
                  continue;

               var startPoint = vector.StartPoint;
               var endPoint = vector.EndPoint;

               var value = _paletteDrawingTools.ToDensity(vector.Length, minLength, maxLength);
               var brush = _paletteDrawingTools.GetBrush(value);
               var pen = _paletteDrawingTools.GetPen(value);

               var rectangle = converter.RectangleToScreen(startPoint.X - halfWidth,
                                                           startPoint.Y + halfHeight,
                                                           width, height);
               drawingContext.DrawRectangle(pen,
                                            rectangle.Left, rectangle.Top,
                                            rectangle.Width, rectangle.Height);
               drawingContext.FillRectangle(brush, rectangle);

               if (_vectorScalingMode == VectorScalingMode.ScaleAllVectors)
                  scale = Math.Max(scale, Math.Max(Math.Abs(endPoint.X), Math.Abs(endPoint.Y)));
            }
         }

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               var vector = _velocityField.Vectors[i, j];

               if (vector != null)
               {
                  var startPoint = vector.StartPoint;
                  var endPoint = vector.EndPoint;

                  if (_vectorScalingMode == VectorScalingMode.ScaleEachVector)
                     scale = Math.Max(Math.Abs(endPoint.X), Math.Abs(endPoint.Y));

                  var start = converter.PointToScreen(startPoint);
                  var end = converter.PointToScreen(startPoint.X + endPoint.X / scale * width,
                                                    startPoint.Y + endPoint.Y / scale * height);
                  drawingContext.DrawLine(_vectorPen, start, end);
               }
            }
         }
      }
   }
}