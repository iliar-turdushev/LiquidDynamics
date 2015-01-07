using Mathematics.MathTypes;

namespace BarotropicComponentProblem.IssykKulGrid
{
   /*
    * Каждая ячейка грида представлена 9 узлами:
    * 
    * P31--P32--P33
    *  |    |    |
    * P21--P22--P23
    *  |    |    |
    * P11--P12--P13
    * 
    * Значения искомых функций (компоненты вектора скорости) вычисляются в
    * узлах P11, P13, P31, P33. Значения в остальных узлах нужны для реализации
    * разностной схемы.
    */
   public sealed class GridCell
   {
      public static readonly GridCell Empty = new GridCell();

      private const int Size = 3;

      private readonly double _x;
      private readonly double _y;
      private readonly double _hx;
      private readonly double _hy;
      private readonly double _z;

      private readonly Point3D[,] _points;

      public GridCell(double x, double y, double hx, double hy, double z)
      {
         _x = x;
         _y = y;
         _hx = hx;
         _hy = hy;
         _z = z;

         _points = new Point3D[Size, Size];
      }

      private GridCell()
      {
      }

      public Point3D P(int i, int j)
      {
         if (_points[i, j] == null)
            _points[i, j] = new Point3D(_x + _hx / 2 * i, _y + _hy / 2 * j, _z);
         
         return _points[i, j];
      }

      public void Stretch(double sx, double sy, double sz)
      {
         for (int i = 0; i < Size; i++)
         {
            for (int j = 0; j < Size; j++)
            {
               _points[i, j].X /= sx;
               _points[i, j].Y /= sy;
               _points[i, j].Z /= sz;
            }
         }
      }
   }
}