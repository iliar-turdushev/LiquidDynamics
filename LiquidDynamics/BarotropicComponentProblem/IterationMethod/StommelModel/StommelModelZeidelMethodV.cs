using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod.StommelModel
{
   internal sealed class StommelModelZeidelMethodV : StommelModelZeidelMethodBase
   {
      internal StommelModelZeidelMethodV(IStommelModelProblem problem, Grid x, Grid y)
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
         var r = dy / 2.0 * (c1 * f(i + 1, j) + c2 * f(i, j));
         
         return (n - r) / d;
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
         var r = 0.5 / dx * (c1 * f(i + 1, j) + c2 * f(i, j) -
                             c5 * f(i, j) - c6 * f(i - 1, j));

         return (n - r) / d;
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
         var r = -dy / 2.0 * (c1 * f(i, j) + c2 * f(i - 1, j));

         return (n - r) / d;
      }
   }
}