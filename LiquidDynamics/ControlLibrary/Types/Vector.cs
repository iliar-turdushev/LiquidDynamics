using System;
using System.Drawing;

namespace ControlLibrary.Types
{
   public sealed class Vector
   {
      private readonly float _length;

      public Vector(PointF startPoint, PointF endPoint)
      {
         StartPoint = startPoint;
         EndPoint = endPoint;
         _length = getVectorLength(endPoint);
      }

      public PointF StartPoint { get; private set; }
      public PointF EndPoint { get; private set; }

      public float Length
      {
         get { return _length; }
      }

      private static float getVectorLength(PointF end)
      {
         return (float) Math.Sqrt(Math.Pow(end.X, 2) + Math.Pow(end.Y, 2));
      }
   }
}