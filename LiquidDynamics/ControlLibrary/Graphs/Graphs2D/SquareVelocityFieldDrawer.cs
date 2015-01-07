using System;
using System.Drawing;
using Common;
using ControlLibrary.Types;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal sealed class SquareVelocityFieldDrawer : IGraphDrawer
   {
      private readonly SquareVelocityField _velocityField;
      private readonly Pen _vectorPen;

      public SquareVelocityFieldDrawer(SquareVelocityField velocityField, Pen vectorPen)
      {
         Check.NotNull(velocityField, "velocityField");
         Check.NotNull(vectorPen, "vectorPen");

         _velocityField = velocityField;
         _vectorPen = vectorPen;
      }

      public void Draw(Graphics drawingContext, CoordinatesConverter converter)
      {
         var n = _velocityField.Vectors.GetLength(0);
         var m = _velocityField.Vectors.GetLength(1);

         var width = _velocityField.CellSize.Width;
         var height = _velocityField.CellSize.Height;
         var scale = float.MinValue;

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               var vector = _velocityField.Vectors[i, j];

               if (vector != null)
                  scale = Math.Max(scale, Math.Max(Math.Abs(vector.EndPoint.X), Math.Abs(vector.EndPoint.Y)));
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