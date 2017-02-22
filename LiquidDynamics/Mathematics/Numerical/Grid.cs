using System;

namespace Mathematics.Numerical
{
   public sealed class Grid
   {
      public static Grid Create(double min, double max, int nodes)
      {
         var grid = new double[nodes];
         var value = min;
         var step = (max - min) / (nodes - 1);

         for (var i = 0; i < nodes; i++)
         {
            grid[i] = value;
            value += step;
         }

         return new Grid(grid, step, nodes);
      }

      private readonly double[] _grid;

      private Grid(double[] grid, double step, int nodes)
      {
         _grid = grid;
         Step = step;
         Nodes = nodes;
      }

      public double Step { get; private set; }

      public int Nodes { get; private set; }

      [Obsolete("Use indexer this[int index] instead of this method.")]
      public double Get(int index)
      {
         return _grid[index];
      }

      public double this[int index] => _grid[index];
   }
}
