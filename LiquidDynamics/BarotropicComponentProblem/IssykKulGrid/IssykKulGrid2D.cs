using Common;

namespace BarotropicComponentProblem.IssykKulGrid
{
   public sealed class IssykKulGrid2D
   {
      private readonly GridCell[,] _grid;

      public IssykKulGrid2D(GridCell[,] grid, double hx, double hy)
      {
         Check.NotNull(grid, "grid");
         _grid = grid;

         Hx = hx;
         Hy = hy;
      }

      public GridCell this[int i, int j]
      {
         get { return _grid[i, j]; }
      }

      public int N
      {
         get { return _grid.GetLength(0); }
      }

      public int M
      {
         get { return _grid.GetLength(1); }
      }

      public double Hx { get; private set; }

      public double Hy { get; private set; }

      public void Stretch(double sx, double sy, double sz)
      {
         Hx /= sx;
         Hy /= sy;

         for (int i = 0; i < N; i++)
         {
            for (int j = 0; j < M; j++)
            {
               if (_grid[i, j] == GridCell.Empty)
                  continue;

               _grid[i, j].Stretch(sx, sy, sz);
            }
         }
      }
   }
}