using System;
using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.TestProblem
{
   public sealed class IntegroInterpolatingScheme : IDynamicProblem
   {
      private const double Pi = Math.PI;

      private static readonly Func<double, double> Cos = Math.Cos;
      private static readonly Func<double, double> Sin = Math.Sin;
      private static readonly Func<double, double> Exp = Math.Exp;
      private static readonly Func<double, double> Sqr = x => x * x;

      private double[] _l;
      private double[] _alpha;
      private double[] _beta;
      private double[] _fa;
      private double[] _fb;
      private double[] _tauX;
      private double[,] _tauY;

      private double[] _epsilon;
      private double[] _a;
      private double[,] _f;
      private double[,] _g;

      private readonly ProblemParameters _problemParameters;
      private readonly GridParameters _gridParameters;

      public IntegroInterpolatingScheme(ProblemParameters problemParameters, GridParameters gridParameters)
      {
         Check.NotNull(problemParameters, "problemParameters");
         Check.NotNull(gridParameters, "gridParameters");

         _problemParameters = problemParameters;
         _gridParameters = gridParameters;

         initializeFunctions();
         initializeTau();
      }

      public void SetInitialCondition(InitialCondition initialCondition)
      {
         Check.NotNull(initialCondition, "initialCondition");
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
         return getEpsilon(j, IndexOffset.None);
      }

      public double K(int i, int j, IndexOffset offset)
      {
         return getEpsilon(j, offset);
      }

      public double A(int i, IndexOffset offset, int j)
      {
         return _a[j];
      }

      public double B(int i, int j, IndexOffset offset)
      {
         return 0.0;
      }

      public double F(int i, int j)
      {
         return _f[i, j];
      }

      public double G(int i, int j)
      {
         return _g[i, j];
      }

      private double getEpsilon(int j, IndexOffset offset)
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

         return _epsilon[2 * j + indexOffset];
      }
      
      private void initializeFunctions()
      {
         var h = _problemParameters.H;
         var l0 = _problemParameters.SmallL0;
         var beta = _problemParameters.Beta;
         var mu = _problemParameters.Mu;
         var tau = _gridParameters.Tau;

         var grid = _gridParameters.Y;
         var extendedGrid = getExtendedGrid(grid);
         var nodes = extendedGrid.Nodes;

         _l = new double[nodes];
         _alpha = new double[nodes];
         _beta = new double[nodes];

         _fa = new double[nodes];
         _epsilon = new double[nodes];

         for (var i = 0; i < nodes; i++)
         {
            var y = extendedGrid.Get(i);

            _l[i] = l0 + beta * y;
            _alpha[i] = 1.0 - Cos(_l[i] * tau) * Exp(-mu * tau);
            _beta[i] = Sin(_l[i] * tau) * Exp(-mu * tau);

            _fa[i] = (mu * _alpha[i] + _l[i] * _beta[i]) / (Sqr(_alpha[i]) + Sqr(_beta[i]));
            _epsilon[i] = _fa[i] / h;
         }

         var n = grid.Nodes;
         _fb = new double[n];

         for (var i = 0; i < n; i++)
         {
            var j = 2 * i;
            _fb[i] = (_l[j] * _alpha[j] - mu * _beta[j]) / (Sqr(_alpha[j]) + Sqr(_beta[j]));
         }

         var hy = grid.Step;
         _a = new double[n];

         _a[0] = (_fb[1] - _fb[0]) / (h * hy);

         for (var i = 1; i < n - 1; i++)
         {
            // TODO: подумать о производной для случая, когда рельеф зависит от (x, y), т.е. H(x, y).
            _a[i] = (_fb[i + 1] - _fb[i - 1]) / (2.0 * h * hy);
         }

         _a[n - 1] = (_fb[n - 1] - _fb[n - 2]) / (h * hy);
      }

      private void initializeTau()
      {
         var xGrid = _gridParameters.X;
         var yGrid = _gridParameters.Y;
         var n = xGrid.Nodes;
         var m = yGrid.Nodes;

         var rho0 = _problemParameters.Rho0;
         var smallR = _problemParameters.SmallR;
         var smallQ = _problemParameters.SmallQ;
         var f1 = _problemParameters.F1;
         var f2 = _problemParameters.F2;

         _tauX = new double[m];
         _tauY = new double[n, m];

         for (var j = 0; j < m; j++)
         {
            var y = yGrid.Get(j);
            _tauX[j] = -f1 * smallQ * rho0 / Pi * Cos(Pi * y / smallQ);

            for (var i = 0; i < n; i++)
            {
               var x = xGrid.Get(i);
               _tauY[i, j] = f2 * smallR * rho0 / Pi * Cos(Pi * x / smallR) * Sin(Pi * y / smallQ);
            }
         }
      }

      private static Grid getExtendedGrid(Grid grid)
      {
         return Grid.Create(grid.Get(0), grid.Get(grid.Nodes - 1), 2 * grid.Nodes - 1);
      }

      private void initializeRightPart(InitialCondition initialCondition)
      {
         var u = initialCondition.U;
         var v = initialCondition.V;

         var rho0 = _problemParameters.Rho0;
         var h = _problemParameters.H;
         var mu = _problemParameters.Mu;

         var xGrid = _gridParameters.X;
         var yGrid = _gridParameters.Y;
         var n = xGrid.Nodes;
         var m = yGrid.Nodes;

         _g = new double[n, m];
         _f = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               var t1 = _fa[2 * j] - mu;
               var t2 = _fb[j] - _l[2 * j];

               _g[i, j] = _tauX[j] / (rho0 * h) + t1 * u[i, j] / h - t2 * v[i, j] / h;
               _f[i, j] = _tauY[i, j] / (rho0 * h) + t2 * u[i, j] / h + t1 * v[i, j] / h;
            }
         }
      }
   }
}
