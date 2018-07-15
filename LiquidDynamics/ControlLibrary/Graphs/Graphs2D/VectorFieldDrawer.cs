using System;
using System.Drawing;
using ControlLibrary.Types;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal sealed class VectorFieldDrawer : IGraphDrawer
   {
      private readonly SquareVelocityField _vectorField;
      private readonly IPaletteDrawingTools _colorMap;
      private readonly Pen _vectorPen;

      public VectorFieldDrawer(
         SquareVelocityField vectorField,
         Pen vectorPen,
         IPaletteDrawingTools colorMap)
      {
         if (vectorField == null)
            throw new ArgumentNullException(nameof(vectorField));

         if (vectorPen == null)
            throw new ArgumentNullException(nameof(vectorPen));

         if (colorMap == null)
            throw new ArgumentNullException(nameof(colorMap));

         _vectorField = vectorField;
         _vectorPen = (Pen) vectorPen.Clone();
         _colorMap = colorMap;
      }

      public void Draw(Graphics drawingContext, CoordinatesConverter converter)
      {
         int n = _vectorField.Vectors.GetLength(0);
         int m = _vectorField.Vectors.GetLength(1);

         float scale = float.MinValue;

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               Vector v = _vectorField.Vectors[i, j];

               if (v != null)
                  scale = Math.Max(scale, Math.Max(Math.Abs(v.EndPoint.X), Math.Abs(v.EndPoint.Y)));
            }
         }

         float minLen = _vectorField.GetMinVector().Length;
         float maxLen = _vectorField.GetMaxVector().Length;

         float width = _vectorField.CellSize.Width;
         float height = _vectorField.CellSize.Height;

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               Vector v = _vectorField.Vectors[i, j];
               
               if (v != null)
               {
                  int density = _colorMap.ToDensity(v.Length, minLen, maxLen);
                  _vectorPen.Color = _colorMap.GetColor(density);

                  PointF sp = converter.PointToScreen(v.StartPoint);
                  PointF ep = converter.PointToScreen(
                     v.StartPoint.X + v.EndPoint.X / scale * width,
                     v.StartPoint.Y + v.EndPoint.Y / scale * height);

                  drawingContext.DrawLine(_vectorPen, sp, ep);
               }
            }
         }
      }
   }
}