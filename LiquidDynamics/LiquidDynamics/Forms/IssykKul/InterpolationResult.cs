using System;
using System.Drawing;
using BarotropicComponentProblem.IssykKulGrid;
using Common;
using Mathematics.MathTypes;
using GraphPoint3D = ControlLibrary.Types.Point3D;
using GraphRectangle3D = ControlLibrary.Types.Rectangle3D;

namespace LiquidDynamics.Forms.IssykKul
{
   internal sealed class InterpolationResult
   {
      public InterpolationResult(IssykKulGrid2D grid)
      {
         Check.NotNull(grid, "grid");

         N = grid.N;
         M = grid.M;

         Cells = getCells(grid);
         CellSize = new SizeF((float) grid.Hx, (float) grid.Hy);

         initializeMinMaxDepth(grid);
      }

      public InterpolationResult(IssykKulGrid3D grid) : this(grid.Grid2D)
      {
         Check.NotNull(grid, "grid");
         DepthRectangles = getDepthRectangles(grid);
      }

      public int N { get; private set; }
      public int M { get; private set; }

      public CellInfo[,] Cells { get; private set; }
      public SizeF CellSize { get; private set; }

      public float MinDepth { get; private set; }
      public float MaxDepth { get; private set; }

      public GraphRectangle3D[,][] DepthRectangles { get; private set; }

      private static CellInfo[,] getCells(IssykKulGrid2D grid)
      {
         var cells = new CellInfo[grid.N, grid.M];
         var hx = (float) grid.Hx;
         var hy = (float) grid.Hy;

         for (int i = 0; i < grid.N; i++)
         {
            for (int j = 0; j < grid.M; j++)
            {
               if (grid[i, j] == GridCell.Empty)
                  continue;

               Point3D point = grid[i, j].P(0, 2);
               var rectangle = new RectangleF((float) point.X, (float) point.Y, hx, hy);
               cells[i, j] = new CellInfo(rectangle, (float) getDepth(grid[i, j]));
            }
         }

         return cells;
      }

      private static double getDepth(GridCell cell)
      {
         return cell.P(1, 1).Z;
      }

      private void initializeMinMaxDepth(IssykKulGrid2D grid)
      {
         double minDepth = double.MaxValue;
         double maxDepth = double.MinValue;

         for (int i = 0; i < grid.N; i++)
         {
            for (int j = 0; j < grid.M; j++)
            {
               if (grid[i, j] != GridCell.Empty)
               {
                  minDepth = Math.Min(minDepth, getDepth(grid[i, j]));
                  maxDepth = Math.Max(maxDepth, getDepth(grid[i, j]));
               }
            }
         }

         MinDepth = (float) minDepth;
         MaxDepth = (float) maxDepth;
      }

      private GraphRectangle3D[,][] getDepthRectangles(IssykKulGrid3D grid)
      {
         IssykKulGrid2D surface = grid.Grid2D;
         var result = new GraphRectangle3D[surface.N, surface.M][];

         for (int i = 0; i < surface.N; i++)
         {
            for (int j = 0; j < surface.M; j++)
            {
               if (surface[i, j] == GridCell.Empty)
                  continue;

               Rectangle3D[] depthGrid = grid.GetDepthGrid(i, j);
               result[i, j] = new GraphRectangle3D[depthGrid.Length];

               for (int k = 0; k < depthGrid.Length; k++)
               {
                  Rectangle3D rectangle = depthGrid[k];
                  Point3D point = rectangle.Origin;
                  var origin = new GraphPoint3D((float) point.X, (float) point.Y, (float) point.Z);
                  result[i, j][k] = new GraphRectangle3D(origin, (float) rectangle.Hx, (float) rectangle.Hy, (float) rectangle.Hz);
               }
            }
         }

         return result;
      }
   }
}