using System;
using System.Drawing;
using ControlLibrary.Types;

namespace ControlLibrary
{
   internal sealed class CoordinatesConverter
   {
      private readonly float _sx;
      private readonly float _sy;
      private readonly float _dx;
      private readonly float _dy;

      public CoordinatesConverter(RectangleF screen, Bounds bounds)
      {
         _sx = screen.Width / (bounds.XMax - bounds.XMin);
         _sy = -screen.Height / (bounds.YMax - bounds.YMin);
         _dx = screen.Left - bounds.XMin * _sx;
         _dy = screen.Top - bounds.YMax * _sy;
      }

      public PointF PointToScreen(float x, float y)
      {
         return new PointF(x * _sx + _dx, y * _sy + _dy);
      }

      public PointF PointToScreen(PointF point)
      {
         return PointToScreen(point.X, point.Y);
      }

      public RectangleF RectangleToScreen(float x, float y, float width, float height)
      {
         return new RectangleF(x * _sx + _dx, y * _sy + _dy, width * Math.Abs(_sx), height * Math.Abs(_sy));
      }

      public RectangleF RectangleToScreen(RectangleF rectangle)
      {
         return RectangleToScreen(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
      }
   }
}