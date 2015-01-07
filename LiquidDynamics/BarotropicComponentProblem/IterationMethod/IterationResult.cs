using Common;

namespace BarotropicComponentProblem.IterationMethod
{
   public sealed class IterationResult<T> where T : class
   {
      public IterationResult(T approximation, int iterationNumber, IterationStatus iterationStatus)
      {
         Check.NotNull(approximation, "approximation");

         Approximation = approximation;
         IterationNumber = iterationNumber;
         IterationStatus = iterationStatus;
      }

      public T Approximation { get; private set; }
      public int IterationNumber { get; private set; }
      public IterationStatus IterationStatus { get; private set; }

      public static IterationResult<T> Convergence(T approximation, int iterationNumber)
      {
         return new IterationResult<T>(approximation, iterationNumber, IterationStatus.Convergence);
      }

      public static IterationResult<T> Divergence(T approximation, int iterationNumber)
      {
         return new IterationResult<T>(approximation, iterationNumber, IterationStatus.Divergence);
      }

      public static IterationResult<T> Circularity(T approximation, int iterationNumber)
      {
         return new IterationResult<T>(approximation, iterationNumber, IterationStatus.Circularity);
      }

      public static IterationResult<T> NoResult(T approximation, int iterationNumber)
      {
         return new IterationResult<T>(approximation, iterationNumber, IterationStatus.None);
      }
   }
}