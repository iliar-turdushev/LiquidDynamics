using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod.StommelModel
{
   internal sealed class StommelModelZeidelMethodU : StommelModelZeidelMethodBase
   {
      internal StommelModelZeidelMethodU(IStommelModelProblem problem, Grid x, Grid y)
         : base(problem, x, y)
      {
      }

      internal override SquareGridFunction Next(SquareGridFunction previous)
      {
         var n = X.Nodes;
         var m = Y.Nodes;
         var current = new double[n, m];

         for (var i = 1; i < n - 1; i++)
         {
            current[i, 0] = getBottom(i, previous, current);

            for (var j = 1; j < m - 1; j++)
            {
               current[i, j] = getCentral(i, j, previous, current);
            }

            current[i, m - 1] = getTop(i, previous, current);
         }

         return new SquareGridFunction(X, Y, current);
      }

      private double getBottom(int i, SquareGridFunction previous, double[,] current)
      {
         const int j = 0;
         
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
         var r = -0.5 * dx * (c1 * f(i, j + 1) + c2 * f(i, j));

         return (n - r) / d;
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
         var r = 0.5 / dy * (-c3 * f(i, j + 1) - c4 * f(i, j) +
                             c5 * f(i, j) + c6 * f(i, j - 1));

         return (n - r) / d;
      }

      private double getTop(int i, SquareGridFunction previous, double[,] current)
      {
         var j = Y.Nodes - 1;

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
         var r = 0.5 * dx * (c1 * f(i, j) + c2 * f(i, j - 1));

         return (n - r) / d;
      }
   }
}