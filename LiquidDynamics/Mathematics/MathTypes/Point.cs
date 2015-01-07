using System;

namespace Mathematics.MathTypes
{
   public sealed class Point
   {
      public Point()
      {
      }

      public Point(double x, double y)
      {
         X = x;
         Y = y;
      }

      public double X { get; private set; }
      public double Y { get; private set; }

      public static double Distance(Point start, Point end)
      {
         return Math.Sqrt(Math.Pow(start.X - end.X, 2.0) + Math.Pow(start.Y - end.Y, 2.0));
      }
   }
}