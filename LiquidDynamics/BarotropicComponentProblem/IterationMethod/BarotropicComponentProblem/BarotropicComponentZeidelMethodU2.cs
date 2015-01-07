using System.Diagnostics;
using Mathematics.Helpers;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod.BarotropicComponentProblem
{
   internal sealed class BarotropicComponentZeidelMethodU2 : BarotropicComponentZeidelMethodBase
   {
      private readonly int[,] _surface;

      internal BarotropicComponentZeidelMethodU2(IBarotropicComponentProblem problem, Grid x, Grid y, int[,] surface)
         : base(problem, x, y)
      {
         _surface = surface;
      }

      internal override SquareGridFunction Next(SquareGridFunction previous)
      {
         var n = X.Nodes;
         var m = Y.Nodes;
         var current = new double[n, m];

         for (var i = 1; i < n - 1; i++)
         {
            if (_surface[i - 1, 0] == 1 && _surface[i, 0] == 1)
               current[i, 0] = getBottom(i, 0, previous, current);

            for (var j = 1; j < m - 1; j++)
            {
               int count =
                  (_surface[i - 1, j - 1] == 1 ? 1 : 0) +
                  (_surface[i - 1, j] == 1     ? 1 : 0) +
                  (_surface[i, j - 1] == 1     ? 1 : 0) +
                  (_surface[i, j] == 1         ? 1 : 0);

               if (count == 4)
                  current[i, j] = getCentral(i, j, previous, current);
               else if (count == 3)
                  current[i, j] = 0;
               else if (count == 2)
               {
                  if (_surface[i - 1, j] == 1 && _surface[i, j] == 1)
                     current[i, j] = getBottom(i, j, previous, current);
                  else if (_surface[i - 1, j - 1] == 1 && _surface[i, j - 1] == 1)
                     current[i, j] = getTop(i, j, previous, current);
                  else
                     current[i, j] = 0;
               }
               else if (count <= 1)
                  current[i, j] = 0;
            }

            if (_surface[i - 1, m - 2] == 1 && _surface[i, m - 2] == 1)
               current[i, m - 1] = getTop(i, m - 1, previous, current);
         }

         return new SquareGridFunction(X, Y, current);
      }

      private double getBottom(int i, int j, SquareGridFunction previous, double[,] current)
      {
         var dx = X.Step;
         var dy = Y.Step;

         var c1 = cMinus(ry(i, j));
         var c2 = cPlus(ry(i, j));

         var k1 = epsilonRight(i, j) * cPlus(rx(i, j));
         var k2 = epsilonLeft(i, j) * cMinus(rx(i - 1, j));

         var n = 0.5 * k1 * dy / dx * previous[i + 1, j] +
                 0.5 * k2 * dy / dx * current[i - 1, j] +
                 c1 * dx / dy * k(i, j + 1) * previous[i, j + 1];
         var d = 0.5 * k1 * dy / dx +
                 0.5 * k2 * dy / dx +
                 c2 * k(i, j) * dx / dy;
         var r = getBottomR(i, j);

         return (n - r) / d;
      }

      private double getBottomR(int i, int j)
      {
         var dx = X.Step;
         var dy = Y.Step;

         double c1 = cMinus(ry(i, j));
         double c2 = cPlus(rx(i, j));
         double c3 = cPlus(ry(i, j));
         double c4 = cPlus(rx(i - 1, j));
         double c5 = cMinus(rx(i, j));
         double c6 = cMinus(rx(i - 1, j));
         double b = bUp(i, j);
         double k = kUp(i, j);

         Debug.Assert(!k.IsZero());

         return
            -f(i + 1, j + 1) * 0.25 * c1 * c2 -
            f(i + 1, j) * 0.25 * c3 * c2 +
            f(i, j + 1) * 0.25 * (c4 - c5) * c1 +
            f(i, j) * 0.25 * (c4 - c5) * c3 +
            f(i - 1, j + 1) * 0.25 * c1 * c6 +
            f(i - 1, j) * 0.25 * c3 * c6 +
            g(i, j + 1) * (dx / dy + dx / 2.0 * b / k) * c1 +
            g(i, j) * (dx / 2.0 * b / k - dx / dy) * c3;
      }

      private double getCentral(int i, int j, SquareGridFunction previous, double[,] current)
      {
         var dx = X.Step;
         var dy = Y.Step;

         var c1 = cPlus(rx(i, j));
         var c2 = cMinus(rx(i - 1, j));
         var c3 = cMinus(ry(i, j));
         var c4 = cPlus(ry(i, j));
         var c5 = cMinus(ry(i, j - 1));
         var c6 = cPlus(ry(i, j - 1));

         var k1 = epsilonRight(i, j) * c1;
         var k2 = epsilonLeft(i, j) * c2;

         var n = k1 / (dx * dx) * previous[i + 1, j] +
                 k2 / (dx * dx) * current[i - 1, j] +
                 c3 / (dy * dy) * k(i, j + 1) * previous[i, j + 1] +
                 c6 / (dy * dy) * k(i, j - 1) * current[i, j - 1];
         var d = k1 / (dx * dx) + k2 / (dx * dx) +
                 c4 / (dy * dy) * k(i, j) +
                 c5 / (dy * dy) * k(i, j);
         var r = getCentralR(i, j);

         return (n - r) / d;
      }

      private double getCentralR(int i, int j)
      {
         var dx = X.Step;
         var dy = Y.Step;

         double c1 = cMinus(ry(i, j));
         double c2 = cPlus(rx(i, j));
         double c3 = cPlus(ry(i, j));
         double c4 = cMinus(ry(i, j - 1));
         double c5 = cPlus(ry(i, j - 1));
         double c6 = cPlus(rx(i - 1, j));
         double c7 = cMinus(rx(i, j));
         double c8 = cMinus(rx(i - 1, j));
         double b1 = bUp(i, j);
         double k1 = kUp(i, j);
         double b2 = bDown(i, j);
         double k2 = kDown(i, j);

         Debug.Assert(!k1.IsZero());
         Debug.Assert(!k2.IsZero());

         double result =
            -f(i + 1, j + 1) * 0.25 * c1 * c2 -
            f(i + 1, j) * 0.25 * (c3 - c4) * c2 +
            f(i + 1, j - 1) * 0.25 * c5 * c2 +
            f(i, j + 1) * 0.25 * (c6 - c7) * c1 +
            f(i, j) * 0.25 * (c6 - c7) * (c3 - c4) +
            f(i, j - 1) * 0.25 * (c7 - c6) * c5 +
            f(i - 1, j + 1) * 0.25 * c1 * c8 +
            f(i - 1, j) * 0.25 * (c3 - c4) * c8 -
            f(i - 1, j - 1) * 0.25 * c5 * c8 +
            g(i, j + 1) * (dx / dy + dx / 2.0 * b1 / k1) * c1 +
            g(i, j) * ((dx / 2.0 * b1 / k1 - dx / dy) * c3 -
                       (dx / 2.0 * b2 / k2 + dx / dy) * c4) +
            g(i, j - 1) * (dx / dy - dx / 2.0 * b2 / k2) * c5;

         return result / (dx * dy);
      }

      private double getTop(int i, int j, SquareGridFunction previous, double[,] current)
      {
         var dx = X.Step;
         var dy = Y.Step;

         var c1 = cMinus(ry(i, j - 1));
         var c2 = cPlus(ry(i, j - 1));

         var k1 = epsilonRight(i, j) * cPlus(rx(i, j));
         var k2 = epsilonLeft(i, j) * cMinus(rx(i - 1, j));

         var n = 0.5 * k1 * dy / dx * previous[i + 1, j] +
                 0.5 * k2 * dy / dx * current[i - 1, j] +
                 c2 * k(i, j - 1) * current[i, j - 1] * dx / dy;
         var d = 0.5 * k1 * dy / dx +
                 0.5 * k2 * dy / dx +
                 c1 * k(i, j) * dx / dy;
         var r = getTopR(i, j);

         return (n - r) / d;
      }

      private double getTopR(int i, int j)
      {
         var dx = X.Step;
         var dy = Y.Step;

         double c1 = cMinus(ry(i, j - 1));
         double c2 = cPlus(rx(i, j));
         double c3 = cPlus(ry(i, j - 1));
         double c4 = cMinus(rx(i, j));
         double c5 = cPlus(rx(i - 1, j));
         double c6 = cMinus(rx(i - 1, j));
         double b = bDown(i, j);
         double k = kDown(i, j);

         Debug.Assert(!k.IsZero());

         return
            f(i + 1, j) * 0.25 * c1 * c2 +
            f(i + 1, j - 1) * 0.25 * c3 * c2 +
            f(i, j) * 0.25 * (c4 - c5) * c1 +
            f(i, j - 1) * 0.25 * (c4 - c5) * c3 -
            f(i - 1, j) * 0.25 * c1 * c6 -
            f(i - 1, j - 1) * 0.25 * c3 * c6 -
            g(i, j) * (dx / dy + dx / 2.0 * b / k) * c1 +
            g(i, j - 1) * (dx / dy - dx / 2.0 * b / k) * c3;
      }
   }
}