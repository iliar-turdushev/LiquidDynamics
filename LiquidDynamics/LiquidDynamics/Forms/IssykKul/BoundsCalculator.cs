using System;
using System.Collections.Generic;
using Mathematics.MathTypes;

namespace LiquidDynamics.Forms.IssykKul
{
   internal static class BoundsCalculator
   {
      internal static void Calculate(IEnumerable<Point3D> allPoints, out double xMin, out double xMax, out double yMin, out double yMax)
      {
         xMin = double.MaxValue;
         xMax = double.MinValue;
         yMin = double.MaxValue;
         yMax = double.MinValue;

         foreach (var point in allPoints)
         {
            xMin = Math.Min(xMin, point.X);
            xMax = Math.Max(xMax, point.X);
            yMin = Math.Min(yMin, point.Y);
            yMax = Math.Max(yMax, point.Y);
         }
      }
   }
}