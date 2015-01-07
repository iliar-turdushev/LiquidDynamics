using System;
using Common;
using Mathematics.MathTypes;

namespace Mathematics.Numerical
{
   public sealed class SquareGridFunction
   {
      private readonly double[,] _values;
      private readonly Point[,] _points;

      public SquareGridFunction(Grid x, Grid y, double[,] values)
      {
         Check.NotNull(values, "values");

         N = x.Nodes;
         M = y.Nodes;

         _values = new double[N, M];
         Array.Copy(values, 0, _values, 0, _values.Length);

         _points = new Point[N, M];

         for (var i = 0; i < N; i++)
         {
            var value = x.Get(i);

            for (var j = 0; j < M; j++)
            {
               _points[i, j] = new Point(value, y.Get(j));
            }
         }
      }

      public int N { get; private set; }
      public int M { get; private set; }

      public Point Grid(int i, int j)
      {
         return _points[i, j];
      }

      public double this[int i, int j]
      {
         get { return _values[i, j]; }
      }

      public static SquareGridFunction Zeros(Grid x, Grid y)
      {
         var values = new double[x.Nodes, y.Nodes];
         return new SquareGridFunction(x, y, values);
      }
   }
}