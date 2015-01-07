using System;
using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.TestProblem
{
   public sealed class DifferentialScheme1 : IDynamicProblem
   {
      private const double Pi = Math.PI;

      private static readonly Func<double, double> Cos = Math.Cos;
      private static readonly Func<double, double> Sin = Math.Sin;

      private readonly GridParameters _gridParameters;
      private readonly double _theta;
      private readonly ProblemParameters _problemParameters;

      private readonly double _epsilon;
      private readonly double _a;
      private double[,] _f;

      public DifferentialScheme1(
         ProblemParameters problemParameters,
         GridParameters gridParameters,
         double theta)
      {
         Check.NotNull(problemParameters, "problemParameters");
         Check.NotNull(gridParameters, "gridParameters");

         _problemParameters = problemParameters;
         _gridParameters = gridParameters;
         _theta = theta;

         _epsilon = getEpsilon();
         _a = getA();
      }

      public void SetInitialCondition(InitialCondition initialCondition)
      {
         Check.NotNull(initialCondition, "initialCondition");
         _f = initializeF(initialCondition);
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
         return _epsilon;
      }

      public double K(int i, int j, IndexOffset offset)
      {
         return _epsilon;
      }

      public double A(int i, IndexOffset offset, int j)
      {
         return _a;
      }

      public double B(int i, int j, IndexOffset offset)
      {
         return 0;
      }

      public double F(int i, int j)
      {
         return _f[i, j];
      }

      public double G(int i, int j)
      {
         throw new NotSupportedException();
      }

      private double getEpsilon()
      {
         return (1.0 / _gridParameters.Tau + _theta * _problemParameters.Mu) / _problemParameters.H;
      }

      private double getA()
      {
         return _theta * _problemParameters.Beta / _problemParameters.H;
      }

      private double tauX(double y)
      {
         var f1 = _problemParameters.F1;
         var q = _problemParameters.SmallQ;
         var rho0 = _problemParameters.Rho0;
         return -f1 * q * rho0 / Pi * Cos(Pi * y / q);
      }

      private double tauY(double x, double y)
      {
         var f2 = _problemParameters.F2;
         var q = _problemParameters.SmallQ;
         var r = _problemParameters.SmallR;
         var rho0 = _problemParameters.Rho0;
         return f2 * r * rho0 / Pi * Cos(Pi * x / r) * Sin(Pi * y / q);
      }

      private double l(double y)
      {
         return _problemParameters.SmallL0 + _problemParameters.Beta * y;
      }

      private double[,] initializeF(InitialCondition initialCondition)
      {
         var u = initialCondition.U;
         var v = initialCondition.V;

         var rho0 = _problemParameters.Rho0;
         var h = _problemParameters.H;
         var mu = _problemParameters.Mu;
         var tau = _gridParameters.Tau;

         var xGrid = _gridParameters.X;
         var yGrid = _gridParameters.Y;
         var n = xGrid.Nodes;
         var m = yGrid.Nodes;

         // Функции phi1 и phi2.
         var phi1 = new double[n, m];
         var phi2 = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = xGrid.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = yGrid.Get(j);
               var t1 = 1.0 / tau + _theta * mu - mu;
               var t2 = _theta * l(y) - l(y);
               phi1[i, j] = tauX(y) / (rho0 * h) + t1 * u[i, j] / h - t2 * v[i, j] / h;
               phi2[i, j] = tauY(x, y) / (rho0 * h) + t2 * u[i, j] / h + t1 * v[i, j] / h;
            }
         }

         // Производная функции phi1 по переменной y.
         var hy = yGrid.Step;
         var phi1Dy = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            phi1Dy[i, 0] = (phi1[i, 1] - phi1[i, 0]) / hy;
         }

         for (var i = 0; i < n; i++)
         {
            for (var j = 1; j < m - 1; j++)
            {
               phi1Dy[i, j] = (phi1[i, j + 1] - phi1[i, j - 1]) / (2 * hy);
            }
         }

         for (var i = 0; i < n; i++)
         {
            phi1Dy[i, m - 1] = (phi1[i, m - 1] - phi1[i, m - 2]) / hy;
         }

         // Производная функции phi2 по переменной x.
         var hx = xGrid.Step;
         var phi2Dx = new double[n, m];

         for (var j = 0; j < m; j++)
         {
            phi2Dx[0, j] = (phi2[1, j] - phi2[0, j]) / hx;
         }

         for (var j = 0; j < m; j++)
         {
            for (var i = 1; i < n - 1; i++)
            {
               phi2Dx[i, j] = (phi2[i + 1, j] - phi2[i - 1, j]) / (2 * hx);
            }
         }

         for (var j = 0; j < m; j++)
         {
            phi2Dx[n - 1, j] = (phi2[n - 1, j] - phi2[n - 2, j]) / hx;
         }

         // Функция f.
         var f = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               f[i, j] = phi2Dx[i, j] - phi1Dy[i, j];
            }
         }

         return f;
      }
   }
}