using System;
using System.Drawing;

namespace ControlLibrary.Types
{
   public sealed class Vector
   {
      public Vector(PointF startPoint, PointF endPoint)
      {
         StartPoint = startPoint;
         EndPoint = endPoint;
         Length = getVectorLength(endPoint);
      }
      
      public PointF StartPoint { get; private set; }

      public PointF EndPoint { get; private set; }

      public float Length { get; private set; }

      private static float getVectorLength(PointF end)
      {
         return (float) Math.Sqrt(Math.Pow(end.X, 2) + Math.Pow(end.Y, 2));
      }
   }
}