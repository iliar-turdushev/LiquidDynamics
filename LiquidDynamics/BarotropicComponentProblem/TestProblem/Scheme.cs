using System;
using BarotropicComponentProblem.IssykKulGrid;
using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.TestProblem
{
   public sealed class Scheme : IDynamicProblem
   {
      private const double Pi = Math.PI;

      private static readonly Func<double, double> Cos = Math.Cos;
      private static readonly Func<double, double> Sin = Math.Sin;
      private static readonly Func<double, double> Exp = Math.Exp;
      private static readonly Func<double, double> Sqr = x => x * x;

      private readonly ProblemParameters _problemParameters;
      private readonly IssykKulGrid2D _grid;

      private readonly double _tau;

      private double[] _l;
      private double[] _alpha;
      private double[] _beta;
      private double[] _fa;
      private double[] _fb;

      private double[] _tauX;
      private double[,] _tauY;
      
      private readonly Double[,] _epsilon;
      private readonly Double[,] _a;
      private readonly Double[,] _b;

      private Double[,] _f;
      private Double[,] _g;

      public Scheme(ProblemParameters problemParameters, IssykKulGrid2D grid, double tau)
      {
         Check.NotNull(problemParameters, "problemParameters");
         Check.NotNull(grid, "grid");

         _problemParameters = problemParameters;
         _grid = grid;
         
         _tau = tau;

         initializeCommonFunctions();
         initializeTau();
         
         _epsilon = initializeEpsilon();
         _a = initializeA();
         _b = initializeB();
      }

      public void SetInitialCondition(InitialCondition initialCondition)
      {
         initializeRightPart(initialCondition);
      }

      public SquareGridFunction TransformU(SquareGridFunction u)
      {
         return u;
      }

      public SquareGridFunction TransformV(SquareGridFunction v)
      {
         return v;
      }

      public double Epsilon(int i, IndexOffset offset, int j)
      {
         return getEpsilon(i, offset, j, IndexOffset.None);
      }

      public double K(int i, int j, IndexOffset offset)
      {
         return getEpsilon(i, IndexOffset.None, j, offset);
      }

      public double A(int i, IndexOffset offset, int j)
      {
         int column = i + (offset == IndexOffset.HalfDecrease ? -1 : 0);
         return _a[column, j].Value;
      }

      public double B(int i, int j, IndexOffset offset)
      {
         int row = j + (offset == IndexOffset.HalfDecrease ? -1 : 0);
         return _b[i, row].Value;
      }

      public double F(int i, int j)
      {
         return _f[i, j].Value;
      }

      public double G(int i, int j)
      {
         return _g[i, j].Value;
      }

      private void initializeCommonFunctions()
      {
         int m = _grid.M;

         double l0 = _problemParameters.SmallL0;
         double beta = _problemParameters.Beta;
         double mu = _problemParameters.Mu;

         Grid yGrid = Grid.Create(0.0, _problemParameters.SmallQ, 2 * m + 1);
         
         _l = new double[yGrid.Nodes];
         _alpha = new double[yGrid.Nodes];
         _beta = new double[yGrid.Nodes];
         _fa = new double[yGrid.Nodes];
         _fb = new double[yGrid.Nodes];

         for (int i = 0; i < yGrid.Nodes; i++)
         {
            double y = yGrid.Get(i);

            _l[i] = l0 + beta * y;
            _alpha[i] = 1 - Cos(_l[i] * _tau) * Exp(-mu * _tau);
            _beta[i] = Sin(_l[i] * _tau) * Exp(-mu * _tau);

            double sqr = Sqr(_alpha[i]) + Sqr(_beta[i]);

            _fa[i] = (mu * _alpha[i] + _l[i] * _beta[i]) / sqr;
            _fb[i] = (_l[i] * _alpha[i] - mu * _beta[i]) / sqr;
         }
      }

      private void initializeTau()
      {
         Grid xGrid = Grid.Create(0.0, _problemParameters.SmallR, _grid.N + 1);
         Grid yGrid = Grid.Create(0.0, _problemParameters.SmallQ, _grid.M + 1);

         double rho0 = _problemParameters.Rho0;
         double smallR = _problemParameters.SmallR;
         double smallQ = _problemParameters.SmallQ;
         double f1 = _problemParameters.F1;
         double f2 = _problemParameters.F2;

         _tauX = new double[yGrid.Nodes];
         _tauY = new double[xGrid.Nodes, yGrid.Nodes];

         for (var j = 0; j < yGrid.Nodes; j++)
         {
            var y = yGrid.Get(j);
            _tauX[j] = -f1 * smallQ * rho0 / Pi * Cos(Pi * y / smallQ);

            for (var i = 0; i < xGrid.Nodes; i++)
            {
               var x = xGrid.Get(i);
               _tauY[i, j] = f2 * smallR * rho0 / Pi * Cos(Pi * x / smallR) * Sin(Pi * y / smallQ);
            }
         }
      }

      private Double[,] initializeEpsilon()
      {
         int n = _grid.N;
         int m = _grid.M;

         var epsilon = new Double[2 * n + 1, 2 * m + 1];

         for (int i = 0; i < n; i++)
         {
            int column = 2 * i;

            for (int j = 0; j < m; j++)
            {
               GridCell cell = _grid[i, j];

               if (cell == GridCell.Empty)
                  continue;

               int row = 2 * j;

               epsilon[column, row] = new Double {Value = _fa[row] / cell.P(0, 0).Z};
               epsilon[column + 1, row] = new Double {Value = _fa[row] / cell.P(1, 0).Z};
               epsilon[column, row + 1] = new Double {Value = _fa[row + 1] / cell.P(0, 1).Z};

               if (i == n - 1 || _grid[i + 1, j] == GridCell.Empty)
               {
                  epsilon[column + 2, row] = new Double {Value = _fa[row] / cell.P(2, 0).Z};
                  epsilon[column + 2, row + 1] = new Double {Value = _fa[row + 1] / cell.P(2, 1).Z};
               }

               if (j == m - 1 || _grid[i, j + 1] == GridCell.Empty)
               {
                  epsilon[column, row + 2] = new Double {Value = _fa[row + 2] / cell.P(0, 2).Z};
                  epsilon[column + 1, row + 2] = new Double {Value = _fa[row + 2] / cell.P(1, 2).Z};
               }

               if ((i == n - 1 && j == m - 1) ||
                   (i == n - 1 && j < m - 1 && _grid[i, j + 1] == GridCell.Empty) ||
                   (j == m - 1 && i < n - 1 && _grid[i + 1, j] == GridCell.Empty) ||
                   (i < n - 1 && j < m - 1 && _grid[i + 1, j + 1] == GridCell.Empty))
               {
                  epsilon[column + 2, row + 2] = new Double {Value = _fa[row + 2] / cell.P(2, 2).Z};
               }
            }
         }

         return epsilon;
      }

      private Double[,] initializeA()
      {
         int n = _grid.N;
         int m = _grid.M;

         var a = new Double[n, m + 1];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (_grid[i, j] == GridCell.Empty)
                  continue;

               int row = 2 * j;

               a[i, j] = new Double {Value = _fb[row] / _grid[i, j].P(1, 0).Z};

               if (j == m - 1 || _grid[i, j + 1] == GridCell.Empty)
               {
                  a[i, j + 1] = new Double {Value = _fb[row + 2] / _grid[i, j].P(1, 2).Z};
               }
            }
         }

         var diff = new Double[n, m + 1];
         double hy = _grid.Hy;

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               if (_grid[i, j] == GridCell.Empty)
                  continue;

               if (j == 0 || _grid[i, j - 1] == GridCell.Empty)
               {
                  // Производная, направленная вперед.
                  diff[i, j] = new Double {Value = (a[i, j + 1].Value - a[i, j].Value) / hy};
               }
               else
               {
                  // Центральная разность.
                  diff[i, j] = new Double {Value = (a[i, j + 1].Value - a[i, j - 1].Value) / (2 * hy)};
               }

               if (j == m - 1 || _grid[i, j + 1] == GridCell.Empty)
               {
                  // Производная, направленная назад.
                  diff[i, j + 1] = new Double {Value = (a[i, j + 1].Value - a[i, j].Value) / hy};
               }
            }
         }

         return diff;
      }

      private Double[,] initializeB()
      {
         int n = _grid.N;
         int m = _grid.M;

         var b = new Double[n + 1, m];
         
         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               GridCell cell = _grid[i, j];

               if (cell == GridCell.Empty)
                  continue;

               int row = 2 * j;
               
               b[i, j] = new Double {Value = _fb[row + 1] / cell.P(0, 1).Z};

               if (i == n - 1 || _grid[i + 1, j] == GridCell.Empty)
               {
                  b[i + 1, j] = new Double {Value = _fb[row + 1] / cell.P(2, 1).Z};
               }
            }
         }

         var diff = new Double[n + 1, m];
         double hx = _grid.Hx;

         for (int j = 0; j < m; j++)
         {
            for (int i = 0; i < n; i++)
            {
               if (_grid[i, j] == GridCell.Empty)
                  continue;
               
               if (i == 0 || _grid[i - 1, j] == GridCell.Empty)
               {
                  // Производная, направленная вперед.
                  diff[i, j] = new Double {Value = (b[i + 1, j].Value - b[i, j].Value) / hx};
               }
               else
               {
                  // Центральная разность.
                  diff[i, j] = new Double {Value = (b[i + 1, j].Value - b[i - 1, j].Value) / (2 * hx)};
               }
               
               if (i == n - 1 || _grid[i + 1, j] == GridCell.Empty)
               {
                  // Производная, направленная назад.
                  diff[i + 1, j] = new Double {Value = (b[i + 1, j].Value - b[i, j].Value) / hx};
               }
            }
         }

         return diff;
      }

      private void initializeRightPart(InitialCondition initialCondition)
      {
         var u = initialCondition.U;
         var v = initialCondition.V;
         
         var rho0 = _problemParameters.Rho0;
         var mu = _problemParameters.Mu;

         int n = _grid.N;
         int m = _grid.M;

         _g = new Double[n + 1, m + 1];
         _f = new Double[n + 1, m + 1];

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               GridCell cell = _grid[i, j];

               if (cell == GridCell.Empty)
                  continue;

               int row = 2 * j;

               {
                  double h = cell.P(0, 0).Z;
                  double t1 = _fb[row] - _l[row];
                  double t2 = _fa[row] - mu;

                  _f[i, j] = new Double {Value = _tauY[i, j] / (rho0 * h) + t1 * u[i, j] / h + t2 * v[i, j] / h};
                  _g[i, j] = new Double {Value = _tauX[j] / (rho0 * h) + t2 * u[i, j] / h - t1 * v[i, j] / h};
               }

               if (i == n - 1 || _grid[i + 1, j] == GridCell.Empty)
               {
                  double h = cell.P(2, 0).Z;
                  double t1 = _fb[row] - _l[row];
                  double t2 = _fa[row] - mu;

                  _f[i + 1, j] = new Double {Value = _tauY[i + 1, j] / (rho0 * h) + t1 * u[i + 1, j] / h + t2 * v[i + 1, j] / h};
                  _g[i + 1, j] = new Double {Value = _tauX[j] / (rho0 * h) + t2 * u[i + 1, j] / h - t1 * v[i + 1, j] / h};
               }

               if (j == m - 1 || _grid[i, j + 1] == GridCell.Empty)
               {
                  double h = cell.P(0, 2).Z;
                  double t1 = _fb[row + 2] - _l[row + 2];
                  double t2 = _fa[row + 2] - mu;

                  _f[i, j + 1] = new Double {Value = _tauY[i, j + 1] / (rho0 * h) + t1 * u[i, j + 1] / h + t2 * v[i, j + 1] / h};
                  _g[i, j + 1] = new Double {Value = _tauX[j + 1] / (rho0 * h) + t2 * u[i, j + 1] / h - t1 * v[i, j + 1] / h};
               }

               if ((i == n - 1 && j == m - 1) ||
                   (i == n - 1 && j < m - 1 && _grid[i, j + 1] == GridCell.Empty) ||
                   (j == m - 1 && i < n - 1 && _grid[i + 1, j] == GridCell.Empty) ||
                   (i < n - 1 && j < m - 1 && _grid[i + 1, j + 1] == GridCell.Empty))
               {
                  double h = cell.P(2, 2).Z;
                  double t1 = _fb[row + 2] - _l[row + 2];
                  double t2 = _fa[row + 2] - mu;

                  _f[i + 1, j + 1] = new Double
                                        {
                                           Value =
                                              _tauY[i + 1, j + 1] / (rho0 * h) + t1 * u[i + 1, j + 1] / h +
                                              t2 * v[i + 1, j + 1] / h
                                        };
                  _g[i + 1, j + 1] = new Double
                                        {
                                           Value =
                                              _tauX[j + 1] / (rho0 * h) + t2 * u[i + 1, j + 1] / h -
                                              t1 * v[i + 1, j + 1] / h
                                        };
               }
            }
         }
      }

      private double getEpsilon(int i, IndexOffset iOffset, int j, IndexOffset jOffset)
      {
         int column = 2 * i + getIndexOffset(iOffset);
         int row = 2 * j + getIndexOffset(jOffset);
         return _epsilon[column, row].Value;
      }

      private static int getIndexOffset(IndexOffset offset)
      {
         int indexOffset;

         switch (offset)
         {
            case IndexOffset.HalfDecrease:
               indexOffset = -1;
               break;

            case IndexOffset.HalfIncrease:
               indexOffset = 1;
               break;

            default:
               indexOffset = 0;
               break;
         }

         return indexOffset;
      }

      private sealed class Double
      {
         public double Value;

         public override string ToString()
         {
            return Value.ToString();
         }
      }
   }
}