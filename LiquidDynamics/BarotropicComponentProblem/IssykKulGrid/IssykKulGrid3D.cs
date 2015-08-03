using Common;
using Mathematics.MathTypes;

namespace BarotropicComponentProblem.IssykKulGrid
{
   public sealed class IssykKulGrid3D
   {
      private readonly IssykKulGrid2D _grid2D;
      private readonly Rectangle3D[,][] _depthGrid;

      public IssykKulGrid3D(IssykKulGrid2D grid2D, Rectangle3D[,][] depthGrid)
      {
         Check.NotNull(grid2D, "grid2D");
         Check.NotNull(depthGrid, "depthGrid");

         _grid2D = grid2D;
         _depthGrid = depthGrid;
      }

      public IssykKulGrid2D Grid2D
      {
         get { return _grid2D; }
      }

      public Rectangle3D[] GetDepthGrid(int i, int j)
      {
         return _depthGrid[i, j];
      }

      public void Stretch(double sx, double sy, double sz)
      {
         _grid2D.Stretch(sx, sy, sz);

         for (int i = 0; i < _grid2D.N; i++)
         {
            for (int j = 0; j < _grid2D.M; j++)
            {
               if (_grid2D[i, j] == GridCell.Empty)
                  continue;
               
               foreach (Rectangle3D cell in _depthGrid[i, j])
               {
                  cell.Hx /= sx;
                  cell.Hy /= sy;
                  cell.Hz /= sz;

                  cell.Origin.X /= sx;
                  cell.Origin.Y /= sy;
                  cell.Origin.Z /= sz;
               }
            }
         }
      }
   }
}