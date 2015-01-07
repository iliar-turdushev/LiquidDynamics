using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod
{
   public sealed class IterationProcessParameters
   {
      public IterationProcessParameters(double sigma, double delta, int k, SquareGridFunction initialApproximation)
      {
         Check.NotNull(initialApproximation, "initialApproximation");

         Sigma = sigma;
         Delta = delta;
         K = k;
         InitialApproximation = initialApproximation;
      }
      
      // Параметр критерия сходимости.
      public double Sigma { get; private set; }

      // Параметр критерия расходимости.
      public double Delta { get; private set; }

      // Максимальное число итераций.
      public int K { get; private set; }

      // Начальное приближение.
      public SquareGridFunction InitialApproximation { get; private set; }
   }
}