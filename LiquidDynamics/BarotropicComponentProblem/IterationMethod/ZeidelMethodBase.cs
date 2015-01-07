using System;
using System.Diagnostics;
using Common;
using Mathematics.Helpers;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod
{
   internal abstract class ZeidelMethodBase
   {
      private readonly IProblem _problem;

      protected ZeidelMethodBase(IProblem problem, Grid x, Grid y)
      {
         Check.NotNull(problem, "problem");
         Check.NotNull(x, "x");
         Check.NotNull(y, "y");

         _problem = problem;
         X = x;
         Y = y;
      }

      internal abstract SquareGridFunction Next(SquareGridFunction previous);

      protected Grid X { get; private set; }
      protected Grid Y { get; private set; }

      protected static double cMinus(double z)
      {
         if (z.IsZero())
            return 1.0;
         return z * (Math.Cosh(z) / Math.Sinh(z) - 1.0);
      }

      protected static double cPlus(double z)
      {
         if (z.IsZero())
            return 1.0;
         return z * (Math.Cosh(z) / Math.Sinh(z) + 1.0);
      }

      protected double rx(int i, int j)
      {
         double epsilon = epsilonRight(i, j);
         return 0.5 * X.Step * aRight(i, j) / epsilon;
      }

      protected double ry(int i, int j)
      {
         double k = kUp(i, j);
         return 0.5 * Y.Step * bUp(i, j) / k;
      }

      protected double aLeft(int i, int j)
      {
         return _problem.A(i, IndexOffset.HalfDecrease, j);
      }

      protected double aRight(int i, int j)
      {
         return _problem.A(i, IndexOffset.HalfIncrease, j);
      }

      protected double epsilonLeft(int i, int j)
      {
         return _problem.Epsilon(i, IndexOffset.HalfDecrease, j);
      }

      protected double epsilon(int i, int j)
      {
         return _problem.Epsilon(i, IndexOffset.None, j);
      }

      protected double epsilonRight(int i, int j)
      {
         return _problem.Epsilon(i, IndexOffset.HalfIncrease, j);
      }

      protected double bDown(int i, int j)
      {
         return _problem.B(i, j, IndexOffset.HalfDecrease);
      }

      protected double bUp(int i, int j)
      {
         return _problem.B(i, j, IndexOffset.HalfIncrease);
      }

      protected double kDown(int i, int j)
      {
         return _problem.K(i, j, IndexOffset.HalfDecrease);
      }

      protected double k(int i, int j)
      {
         return _problem.K(i, j, IndexOffset.None);
      }

      protected double kUp(int i, int j)
      {
         return _problem.K(i, j, IndexOffset.HalfIncrease);
      }
   }
}