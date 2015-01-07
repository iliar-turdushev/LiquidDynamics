using System;
using Common;
using Mathematics.MathTypes;

namespace Mathematics.Numerical
{
   public sealed class GridFunction
   {
      private readonly Point[] _points;
      private readonly double[] _values;

      public GridFunction(Point[] points, double[] values)
      {
         Check.NotNull(points, "points");
         Check.NotNull(values, "values");

         N = points.Length;

         _points = new Point[N];
         Array.Copy(points, 0, _points, 0, N);

         _values = new double[N];
         Array.Copy(values, 0, _values, 0, N);
      }

      public int N { get; private set; }

      public Point Grid(int index)
      {
         return _points[index];
      }

      public double this[int index]
      {
         get { return _values[index]; }
      }
   }
}