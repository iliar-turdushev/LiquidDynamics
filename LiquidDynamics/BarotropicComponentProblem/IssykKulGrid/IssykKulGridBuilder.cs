using System;
using System.Collections.Generic;
using System.Linq;
using BarotropicComponentProblem.Kriging;
using Common;
using Mathematics.Helpers;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.IssykKulGrid
{
   public sealed class IssykKulGridBuilder
   {
      private readonly Point3D[] _allPoints;
      private readonly double _xMin;
      private readonly double _xMax;
      private readonly double _yMin;
      private readonly double _yMax;

      public IssykKulGridBuilder(Point3D[] allPoints, double xMin, double xMax, double yMin, double yMax)
      {
         Check.NotNull(allPoints, "allPoints");

         _allPoints = allPoints;
         _xMin = xMin;
         _xMax = xMax;
         _yMin = yMin;
         _yMax = yMax;
      }

      // n - Число ячеек в направлении Восток-Запад.
      // m - Число ячеек в направлении Юг-Север.
      // theta - Параметр сглаживания данных по глубине.
      public IssykKulGrid2D BuildGrid2D(int n, int m, double theta)
      {
         double hx = (_xMax - _xMin) / n;
         double hy = (_yMax - _yMin) / m;

         CellInfo[,] surface = getReservoirSurface(n, m, hx, hy);
         GridCell[,] grid = getGrid(surface, n, m, hx, hy);

         smoothDepthData(grid, n, m, theta);
         filterZeroCells(grid, n, m);
         filterAloneCells(grid, n, m);
         calculateDepths(grid, n, m);

         return new IssykKulGrid2D(grid, hx, hy);
      }

      // n - Число ячеек в направлении Восток-Запад.
      // m - Число ячеек в направлении Юг-Север.
      // dz - Шаг сетки в глубину.
      // theta - Параметр сглаживания данных по глубине.
      public IssykKulGrid3D BuildGrid3D(int n, int m, double dz, double theta)
      {
         IssykKulGrid2D grid = BuildGrid2D(n, m, theta);
         Rectangle3D[,][] depthGrid = getDepthGrid(grid, dz);
         return new IssykKulGrid3D(grid, depthGrid);
      }

      #region Построение зеркала озера Иссык-Куль

      private CellInfo[,] getReservoirSurface(int n, int m, double hx, double hy)
      {
         var surface = new CellInfo[n, m];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               surface[i, j] = new CellInfo
                                  {
                                     Data = new List<Point3D>(),
                                     Status = CellStatus.Inner
                                  };
            }
         }

         foreach (Point3D point in _allPoints)
         {
            var column = (int) (point.X / hx);
            var row = (int) (point.Y / hy);

            if (column == n)
               column--;

            if (row == m)
               row--;

            if (point.Z.IsZero())
               surface[column, row].Status = CellStatus.Border;

            surface[column, row].Data.Add(point);
         }

         return fillOuterArea(surface, n, m);
      }

      private static CellInfo[,] fillOuterArea(CellInfo[,] surface, int n, int m)
      {
         for (int column = 0; column < n; column++)
         {
            fillFlood(surface, column, 0);
            fillFlood(surface, column, m - 1);
         }

         for (int row = 0; row < m; row++)
         {
            fillFlood(surface, 0, row);
            fillFlood(surface, n - 1, row);
         }

         return surface;
      }

      private static void fillFlood(CellInfo[,] surface, int column, int row)
      {
         if (surface[column, row].Status == CellStatus.Border ||
             surface[column, row].Status == CellStatus.Outer)
            return;

         int columns = surface.GetLength(0);
         int rows = surface.GetLength(1);

         var queue = new Queue<Pair>();
         queue.Enqueue(new Pair(column, row));

         while (queue.Count > 0)
         {
            Pair node = queue.Dequeue();
            int columnIndex = node.Column;
            int rowIndex = node.Row;

            if (surface[columnIndex, rowIndex].Status == CellStatus.Inner)
            {
               surface[columnIndex, rowIndex].Status = CellStatus.Outer;

               int west = columnIndex - 1;
               int east = columnIndex + 1;

               while (west >= 0 && surface[west, rowIndex].Status == CellStatus.Inner)
               {
                  surface[west--, rowIndex].Status = CellStatus.Outer;
               }

               while (east < columns && surface[east, rowIndex].Status == CellStatus.Inner)
               {
                  surface[east++, rowIndex].Status = CellStatus.Outer;
               }

               for (var i = west + 1; i < east; i++)
               {
                  if (rowIndex - 1 >= 0 && surface[i, rowIndex - 1].Status == CellStatus.Inner)
                     queue.Enqueue(new Pair(i, rowIndex - 1));

                  if (rowIndex + 1 < rows && surface[i, rowIndex + 1].Status == CellStatus.Inner)
                     queue.Enqueue(new Pair(i, rowIndex + 1));
               }
            }
         }
      }

      #endregion

      #region Построение сеточной области для зеркала озера Иссык-Куль

      private const int NodesCount = 4;

      private GridCell[,] getGrid(CellInfo[,] surface, int n, int m, double hx, double hy)
      {
         var grid = new GridCell[n, m];
         var variogram = new Gamma1(0, 1);

         for (int i = 0; i < n; i++)
         {
            double x = hx * i;
            
            for (int j = 0; j < m; j++)
            {
               double y = hy * j;

               if (surface[i, j].Status == CellStatus.Inner ||
                   surface[i, j].Status == CellStatus.Border)
               {
                  var point = new Point(x + hx / 2, y + hy / 2);
                  GridFunction gridFunction = getGridFunction(point, i, j, surface);
                  var kriging = new KrigingMethod(gridFunction, variogram);

                  double value = kriging.GetValue(point);
                  double z = value < 0 ? 0 : value;

                  grid[i, j] = new GridCell(x, y, hx, hy, z);
               }
               else
               {
                  grid[i, j] = GridCell.Empty;
               }
            }
         }

         return grid;
      }

      private GridFunction getGridFunction(Point point, int column, int row, CellInfo[,] surface)
      {
         CellInfo cellInfo = surface[column, row];
         var result = new DistanceInfo[NodesCount];

         for (int i = 0; i < NodesCount; i++)
         {
            result[i] = new DistanceInfo {Distance = double.MaxValue};
         }

         int left = column;
         int right = column;
         int bottom = row;
         int top = row;

         List<Point3D> data = cellInfo != null ? cellInfo.Data : new List<Point3D>();

         while (data.Count == 0)
         {
            data = getPoints(--left, ++right, --bottom, ++top, surface);
         }

         while (getNearestPoints(point, data, result))
         {
            var hasNotDefinedPoints = result.Any(info => info.Point == null);

            do
            {
               data = getPoints(--left, ++right, --bottom, ++top, surface);
            } while (data.Count == 0 && hasNotDefinedPoints);

            if (data.Count == 0 && !hasNotDefinedPoints)
               break;
         }

         return
            new GridFunction(
               result.Select(info => new Point(info.Point.X, info.Point.Y)).ToArray(),
               result.Select(info => info.Point.Z).ToArray());
      }

      private List<Point3D> getPoints(int left, int right, int bottom, int top, CellInfo[,] surface)
      {
         var list = new List<Point3D>();
         int leftBorder = left >= 0 ? left : 0;
         int rightBorder = right < surface.GetLength(0) ? right : surface.GetLength(0) - 1;
         int bottomBorder = bottom >= 0 ? bottom : 0;
         int topBorder = top < surface.GetLength(1) ? top : surface.GetLength(1) - 1;

         if (left >= 0)
         {
            for (int i = bottomBorder; i <= topBorder; i++)
            {
               if (surface[left, i] != null)
                  list.AddRange(surface[left, i].Data);
            }
         }

         if (right < surface.GetLength(0))
         {
            for (int i = bottomBorder; i <= topBorder; i++)
            {
               if (surface[right, i] != null)
                  list.AddRange(surface[right, i].Data);
            }
         }

         if (bottom >= 0)
         {
            for (int i = leftBorder + 1; i <= rightBorder - 1; i++)
            {
               if (surface[i, bottom] != null)
                  list.AddRange(surface[i, bottom].Data);
            }
         }

         if (top < surface.GetLength(1))
         {
            for (int i = leftBorder + 1; i <= rightBorder - 1; i++)
            {
               if (surface[i, top] != null)
                  list.AddRange(surface[i, top].Data);
            }
         }

         return list;
      }

      private static bool getNearestPoints(Point point, IList<Point3D> points, IList<DistanceInfo> distanceInfos)
      {
         bool hasNearestPoints = false;

         for (int i = 0; i < points.Count; i++)
         {
            var distanceInfo = new DistanceInfo
                                  {
                                     Point = points[i],
                                     Distance = DistanceInfo.CalculateDistance(points[i], point)
                                  };

            for (int j = 0; j < NodesCount; j++)
            {
               if (distanceInfo.Distance < distanceInfos[j].Distance)
               {
                  hasNearestPoints = true;

                  DistanceInfo temp = distanceInfo;

                  for (int k = j; k < NodesCount; k++)
                  {
                     DistanceInfo current = distanceInfos[k];
                     distanceInfos[k] = temp;
                     temp = current;
                  }

                  break;
               }
            }
         }

         return hasNearestPoints;
      }

      #endregion

      #region Отбрасывание "одиноких" ячеек. В таких ячейках нет узлов, в которых может быть расчитана скорость

      private static void filterAloneCells(GridCell[,] grid, int n, int m)
      {
         GridCell empty = GridCell.Empty;

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (grid[i, j] == empty)
                  continue;

               if ((i == 0 || i == n - 1) && j == 0)
               {
                  if (grid[i, j + 1] == empty)
                     grid[i, j] = empty;
               }
               else if ((i == 0 || i == n - 1) && j == m - 1)
               {
                  if (grid[i, j - 1] == empty)
                     grid[i, j] = empty;
               }
               else if (i == 0 || i == n - 1)
               {
                  int emptyCells =
                     (grid[i, j - 1] == GridCell.Empty ? 1 : 0) +
                     (grid[i, j + 1] == GridCell.Empty ? 1 : 0);

                  if (emptyCells == 2)
                     grid[i, j] = empty;
               }
               else if (j == 0 || j == m - 1)
               {
                  int emptyCells =
                     (grid[i - 1, j] == GridCell.Empty ? 1 : 0) +
                     (grid[i + 1, j] == GridCell.Empty ? 1 : 0);

                  if (emptyCells == 2)
                     grid[i, j] = empty;
               }
               else
               {
                  int emptyCells =
                     (grid[i - 1, j] == GridCell.Empty ? 1 : 0) +
                     (grid[i + 1, j] == GridCell.Empty ? 1 : 0) +
                     (grid[i, j - 1] == GridCell.Empty ? 1 : 0) +
                     (grid[i, j + 1] == GridCell.Empty ? 1 : 0);

                  if (emptyCells >= 3)
                     grid[i, j] = empty;
               }
            }
         }
      }

      #endregion

      #region Отбрасывание ячеек, в которых глубина равна 0

      private void filterZeroCells(GridCell[,] grid, int n, int m)
      {
         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (grid[i, j] == GridCell.Empty)
                  continue;

               if (getDepth(grid[i, j]).IsZero())
                  grid[i, j] = GridCell.Empty;
            }
         }
      }

      #endregion

      #region Сглаживание

      private void smoothDepthData(GridCell[,] grid, int n, int m, double theta)
      {
         const int smoothIterations = 3;
         GridCell empty = GridCell.Empty;

         var depths = new List<double>();
         var columns = new List<int>();
         var rows = new List<int>();

         for (int iteration = 0; iteration < smoothIterations; iteration++)
         {
            for (int i = 1; i < n - 1; i++)
            {
               for (int j = 1; j < m - 1; j++)
               {
                  if (grid[i, j] != empty &&
                      grid[i, j - 1] != empty && grid[i, j + 1] != empty &&
                      grid[i - 1, j] != empty && grid[i + 1, j] != empty)
                  {
                     double depth =
                        (1 - theta) * getDepth(grid[i, j]) +
                        0.25 * theta * (getDepth(grid[i, j - 1]) + getDepth(grid[i, j + 1]) +
                                        getDepth(grid[i + 1, j]) + getDepth(grid[i - 1, j]));
                     depths.Add(depth);
                     columns.Add(i);
                     rows.Add(j);
                  }
               }

               for (int k = 0; k < depths.Count; k++)
                  setDepth(grid[columns[k], rows[k]], depths[k]);
            }

            depths.Clear();
            columns.Clear();
            rows.Clear();
         }
      }

      private double getDepth(GridCell cell)
      {
         return cell.P(1, 1).Z;
      }

      private void setDepth(GridCell cell, double depth)
      {
         cell.P(1, 1).Z = depth;
      }

      #endregion

      #region Вычисление глубины в узлах и на ребрах сеточных ячеек

      private void calculateDepths(GridCell[,] grid, int n, int m)
      {
         Func<int, int, double> depthGetter =
            (column, row) =>
               {
                  if (column < 0 || column >= n || row < 0 || row >= m)
                     return 0;

                  GridCell cell = grid[column, row];
                  return cell == GridCell.Empty ? 0 : cell.P(1, 1).Z;
               };

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               GridCell currentCell = grid[i, j];

               if (currentCell == GridCell.Empty)
                  continue;

               // P11.
               {
                  double depth = 0.25 * (depthGetter(i - 1, j - 1) + depthGetter(i, j - 1) +
                                         depthGetter(i - 1, j) + depthGetter(i, j));
                  setDepth(currentCell, 0, 0, depth);
               }

               // P12.
               {
                  double depth = 0.5 * (depthGetter(i, j - 1) + depthGetter(i, j));
                  setDepth(currentCell, 1, 0, depth);
               }

               // P13.
               {
                  double depth = 0.25 * (depthGetter(i, j - 1) + depthGetter(i + 1, j - 1) +
                                         depthGetter(i, j) + depthGetter(i + 1, j));
                  setDepth(currentCell, 2, 0, depth);
               }

               // P21.
               {
                  double depth = 0.5 * (depthGetter(i - 1, j) + depthGetter(i, j));
                  setDepth(currentCell, 0, 1, depth);
               }

               // P23.
               {
                  double depth = 0.5 * (depthGetter(i, j) + depthGetter(i + 1, j));
                  setDepth(currentCell, 2, 1, depth);
               }

               // P31.
               {
                  double depth = 0.25 * (depthGetter(i - 1, j) + depthGetter(i, j) +
                                         depthGetter(i - 1, j + 1) + depthGetter(i, j + 1));
                  setDepth(currentCell, 0, 2, depth);
               }

               // P32.
               {
                  double depth = 0.5 * (depthGetter(i, j) + depthGetter(i, j + 1));
                  setDepth(currentCell, 1, 2, depth);
               }

               // P33.
               {
                  double depth = 0.25 * (depthGetter(i, j) + depthGetter(i + 1, j) +
                                         depthGetter(i, j + 1) + depthGetter(i + 1, j + 1));
                  setDepth(currentCell, 2, 2, depth);
               }
            }
         }
      }

      private void setDepth(GridCell cell, int i, int j, double depth)
      {
         cell.P(i, j).Z = depth;
      }

      #endregion

      #region Построение сеточной области в глубину

      private Rectangle3D[,][] getDepthGrid(IssykKulGrid2D grid, double dz)
      {
         var depthRectangles = new Rectangle3D[grid.N, grid.M][];

         for (int i = 0; i < grid.N; i++)
         {
            for (int j = 0; j < grid.M; j++)
            {
               if (grid[i, j] == GridCell.Empty)
                  continue;

               double depth = getDepth(grid[i, j]);
               var size = (int) (depth / dz);
               double temp = depth - dz * size;

               if (!temp.IsZero())
                  size++;

               if (size == 0)
                  size = 1;

               Point3D point = getOrigin(grid[i, j]);
               depthRectangles[i, j] = new Rectangle3D[size];

               for (int k = 0; k < size; k++)
               {
                  double z = dz * k;
                  var origin = new Point3D(point.X, point.Y, z);
                  depthRectangles[i, j][k] = new Rectangle3D(origin, grid.Hx, grid.Hx, dz);
               }
            }
         }

         return depthRectangles;
      }

      private static Point3D getOrigin(GridCell cell)
      {
         return cell.P(0, 0);
      }

      #endregion
      
      #region Вложенные классы

      private enum CellStatus
      {
         Inner,
         Border,
         Outer
      }

      private sealed class CellInfo
      {
         internal CellStatus Status { get; set; }
         internal List<Point3D> Data { get; set; }
      }

      private sealed class Pair
      {
         internal Pair(int column, int row)
         {
            Column = column;
            Row = row;
         }

         internal int Column { get; private set; }
         internal int Row { get; private set; }
      }

      private sealed class DistanceInfo
      {
         internal Point3D Point { get; set; }
         internal double Distance { get; set; }

         internal static double CalculateDistance(Point3D start, Point end)
         {
            return Math.Sqrt(Math.Pow(start.X - end.X, 2.0) + Math.Pow(start.Y - end.Y, 2.0));
         }
      }

      #endregion
   }
}
