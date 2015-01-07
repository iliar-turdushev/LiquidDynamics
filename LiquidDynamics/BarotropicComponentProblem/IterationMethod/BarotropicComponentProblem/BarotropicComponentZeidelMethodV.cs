using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod.BarotropicComponentProblem
{
   internal sealed class BarotropicComponentZeidelMethodV : BarotropicComponentZeidelMethodBase
   {
      internal BarotropicComponentZeidelMethodV(IBarotropicComponentProblem problem, Grid x, Grid y)
         : base(problem, x, y)
      {
      }

      internal override SquareGridFunction Next(SquareGridFunction previous)
      {
         var n = X.Nodes;
         var m = Y.Nodes;
         var current = new double[n, m];

         for (var j = 1; j < m - 1; j++)
         {
            current[0, j] = getLeft(j, previous, current);

            for (var i = 1; i < n - 1; i++)
            {
               current[i, j] = getCentral(i, j, previous, current);
            }

            current[n - 1, j] = getRight(j, previous, current);
         }

         return new SquareGridFunction(X, Y, current);
      }

      private double getLeft(int j, SquareGridFunction previous, double[,] current)
      {
         const int i = 0;

         var dx = X.Step;
         var dy = Y.Step;

         var c1 = cPlus(rx(i, j));
         var c2 = cMinus(rx(i, j));
         var c3 = cMinus(ry(i, j));
         var c4 = cPlus(ry(i, j - 1));

         var k1 = kUp(i, j) * c3;
         var k2 = kDown(i, j) * c4;

         var n = dx / dy * k1 / 2.0 * previous[i, j + 1] +
                 dx / dy * k2 / 2.0 * current[i, j - 1] +
                 dy / dx * c1 * epsilon(i + 1, j) * previous[i + 1, j];
         var d = dx / dy * k1 / 2.0 + dx / dy * k2 / 2.0 + dy / dx * c2 * epsilon(i, j);
         var r = getLeftR(j);

         return (n - r) / d;
      }

      private double getLeftR(int j)
      {
         const int i = 0;

         var dx = X.Step;
         var dy = Y.Step;

         double a = aRight(i, j);
         double e = epsilonRight(i, j);

         double c1 = cPlus(rx(i, j));
         double c2 = cMinus(rx(i, j));
         double c3 = cMinus(ry(i, j));
         double c4 = cPlus(ry(i, j));
         double c5 = cMinus(ry(i, j - 1));
         double c6 = cPlus(ry(i, j - 1));

         return
            -f(i + 1, j) * (dy / 2.0 * a / e - dy / dx) * c1 -
            f(i, j) * (dy / 2.0 * a / e + dy / dx) * c2 -
            g(i + 1, j + 1) * 0.25 * c1 * c3 -
            g(i + 1, j) * 0.25 * (c4 - c5) * c1 +
            g(i + 1, j - 1) * 0.25 * c1 * c6 -
            g(i, j + 1) * 0.25 * c2 * c3 -
            g(i, j) * 0.25 * (c4 - c5) * c2 +
            g(i, j - 1) * 0.25 * c2 * c6;
      }

      private double getCentral(int i, int j, SquareGridFunction previous, double[,] current)
      {
         var dx = X.Step;
         var dy = Y.Step;

         var c1 = cPlus(rx(i, j));
         var c2 = cMinus(rx(i, j));
         var c3 = cMinus(ry(i, j));
         var c4 = cPlus(ry(i, j - 1));
         var c5 = cPlus(rx(i - 1, j));
         var c6 = cMinus(rx(i - 1, j));

         var k1 = kUp(i, j) * c3;
         var k2 = kDown(i, j) * c4;

         var n = k1 / (dy * dy) * previous[i, j + 1] +
                 k2 / (dy * dy) * current[i, j - 1] +
                 c1 / (dx * dx) * epsilon(i + 1, j) * previous[i + 1, j] +
                 c6 / (dx * dx) * epsilon(i - 1, j) * current[i - 1, j];
         var d = k1 / (dy * dy) +
                 k2 / (dy * dy) +
                 c2 * epsilon(i, j) / (dx * dx) +
                 c5 * epsilon(i, j) / (dx * dx);
         var r = getCentralR(i, j);

         return (n - r) / d;
      }

      private double getCentralR(int i, int j)
      {
         var dx = X.Step;
         var dy = Y.Step;

         double a1 = aRight(i, j);
         double e1 = epsilonRight(i, j);
         double a2 = aLeft(i, j);
         double e2 = epsilonLeft(i, j);

         double c1 = cPlus(rx(i, j));
         double c2 = cMinus(rx(i, j));
         double c3 = cPlus(rx(i - 1, j));
         double c4 = cMinus(rx(i - 1, j));
         double c5 = cMinus(ry(i, j));
         double c6 = cPlus(ry(i, j));
         double c7 = cMinus(ry(i, j - 1));
         double c8 = cPlus(ry(i, j - 1));

         double result =
            -f(i + 1, j) * (dy / 2.0 * a1 / e1 - dy / dx) * c1 -
            f(i, j) * ((dy / 2.0 * a1 / e1 + dy / dx) * c2 +
                       (dy / dx - dy / 2.0 * a2 / e2) * c3) +
            f(i - 1, j) * (dy / dx + dy / 2.0 * a2 / e2) * c4 -
            g(i + 1, j + 1) * 0.25 * c1 * c5 -
            g(i + 1, j) * 0.25 * (c6 - c7) * c1 +
            g(i + 1, j - 1) * 0.25 * c1 * c8 -
            g(i, j + 1) * 0.25 * (c2 - c3) * c5 -
            g(i, j) * 0.25 * (c6 - c7) * (c2 - c3) +
            g(i, j - 1) * 0.25 * (c2 - c3) * c8 +
            g(i - 1, j + 1) * 0.25 * c4 * c5 -
            g(i - 1, j) * 0.25 * (c7 - c6) * c4 -
            g(i - 1, j - 1) * 0.25 * c4 * c8;

         return result / (dx * dy);
      }

      private double getRight(int j, SquareGridFunction previous, double[,] current)
      {
         var i = X.Nodes - 1;

         var dx = X.Step;
         var dy = Y.Step;

         var c1 = cPlus(rx(i - 1, j));
         var c2 = cMinus(rx(i - 1, j));
         var c3 = cMinus(ry(i, j));
         var c4 = cPlus(ry(i, j - 1));

         var k1 = kUp(i, j) * c3;
         var k2 = kDown(i, j) * c4;

         var n = dx / dy * k1 / 2.0 * previous[i, j + 1] +
                 dx / dy * k2 / 2.0 * current[i, j - 1] +
                 dy / dx * c2 * epsilon(i - 1, j) * current[i - 1, j];
         var d = dx / dy * k1 / 2.0 + dx / dy * k2 / 2.0 + dy / dx * c1 * epsilon(i, j);
         var r = getRightR(j);

         return (n - r) / d;
      }

      private double getRightR(int j)
      {
         var i = X.Nodes - 1;

         var dx = X.Step;
         var dy = Y.Step;

         double a = aLeft(i, j);
         double e = epsilonLeft(i, j);
         double c1 = cPlus(rx(i - 1, j));
         double c2 = cMinus(rx(i - 1, j));
         double c3 = cMinus(ry(i, j));
         double c4 = cMinus(ry(i, j - 1));
         double c5 = cPlus(ry(i, j));
         double c6 = cPlus(ry(i, j - 1));

         return
            -f(i, j) * (dy / dx - dy / 2.0 * a / e) * c1 +
            f(i - 1, j) * (dy / dx + dy / 2.0 * a / e) * c2 +
            g(i, j + 1) * 0.25 * c1 * c3 -
            g(i, j) * 0.25 * (c4 - c5) * c1 -
            g(i, j - 1) * 0.25 * c1 * c6 +
            g(i - 1, j + 1) * 0.25 * c2 * c3 -
            g(i - 1, j) * 0.25 * (c4 - c5) * c2 -
            g(i - 1, j - 1) * 0.25 * c2 * c6;
      }
   }
}