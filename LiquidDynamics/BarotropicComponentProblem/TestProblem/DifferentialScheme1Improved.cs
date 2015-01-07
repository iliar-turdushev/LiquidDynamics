using System;
using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.TestProblem
{
   public sealed class DifferentialScheme1Improved : IDynamicProblem
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
      private double[,] _g;

      public DifferentialScheme1Improved(
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
         return _g[i, j];
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

      private void initializeRightPart(InitialCondition initialCondition)
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

         _g = new double[n, m];
         _f = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = xGrid.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = yGrid.Get(j);
               var t1 = 1.0 / tau + _theta * mu - mu;
               var t2 = _theta * l(y) - l(y);

               _g[i, j] = tauX(y) / (rho0 * h) + t1 * u[i, j] / h - t2 * v[i, j] / h;
               _f[i, j] = tauY(x, y) / (rho0 * h) + t2 * u[i, j] / h + t1 * v[i, j] / h;
            }
         }
      }
   }
}