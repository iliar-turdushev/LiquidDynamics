using System;
using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.TestProblem
{
   public sealed class DifferentialScheme2 : IDynamicProblem
   {
      private const double Pi = Math.PI;

      private static readonly Func<double, double> Cos = Math.Cos;
      private static readonly Func<double, double> Sin = Math.Sin;

      private readonly ProblemParameters _problemParameters;
      private readonly GridParameters _gridParameters;
      private readonly double _theta;
      private readonly double _chi;

      private readonly double _epsilon;
      private readonly double _a;
      private double[,] _f;

      private InitialCondition _initialCondition;

      public DifferentialScheme2(
         ProblemParameters problemParameters,
         GridParameters gridParameters,
         double theta, double chi)
      {
         Check.NotNull(problemParameters, "problemParameters");
         Check.NotNull(gridParameters, "gridParameters");

         _problemParameters = problemParameters;
         _gridParameters = gridParameters;
         _theta = theta;
         _chi = chi;

         _epsilon = getEpsilon();
         _a = getA();
      }

      public void SetInitialCondition(InitialCondition initialCondition)
      {
         Check.NotNull(initialCondition, "initialCondition");

         _initialCondition = initialCondition;
         _f = initializeF(initialCondition);
      }

      public SquareGridFunction TransformU(SquareGridFunction u)
      {
         return transform(u, _initialCondition.U);
      }

      public SquareGridFunction TransformV(SquareGridFunction v)
      {
         return transform(v, _initialCondition.V);
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
         var h = _problemParameters.H;
         var mu = _problemParameters.Mu;
         var tau = _gridParameters.Tau;
         return 2.0 / (h * tau * (1.0 + _theta)) * (1.0 + tau * mu * (1.0 + _chi) / 2.0);
      }

      private double getA()
      {
         return _problemParameters.Beta / _problemParameters.H;
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

         var theta = 2.0 / (h * tau * (1.0 + _theta)) * (1.0 + tau * mu * (_chi - _theta) / 2.0);

         // Функции phi1 и phi2.
         var phi1 = new double[n, m];
         var phi2 = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = xGrid.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = yGrid.Get(j);
               phi1[i, j] = tauX(y) / (rho0 * h) + theta * u[i, j];
               phi2[i, j] = tauY(x, y) / (rho0 * h) + theta * v[i, j];
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

      private SquareGridFunction transform(SquareGridFunction solution, SquareGridFunction initialCondition)
      {
         var result = new double[solution.N, solution.M];

         for (var i = 0; i < result.GetLength(0); i++)
         {
            for (var j = 0; j < result.GetLength(1); j++)
            {
               result[i, j] = 1.0 / (1.0 + _theta) * (2.0 * solution[i, j] + (_theta - 1) * initialCondition[i, j]);
            }
         }

         return new SquareGridFunction(_gridParameters.X, _gridParameters.Y, result);
      }
   }
}