using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod
{
   internal sealed class ZeidelIterationProcess
   {
      private readonly ZeidelMethodBase _zeidelMethod;
      private readonly IterationProcessParameters _parameters;

      private SquareGridFunction _current;
      private int _iterations;

      public ZeidelIterationProcess(
         ZeidelMethodBase zeidelMethod,
         IterationProcessParameters parameters)
      {
         Check.NotNull(zeidelMethod, "zeidelMethod");
         Check.NotNull(parameters, "parameters");

         _zeidelMethod = zeidelMethod;
         _parameters = parameters;

         _current = _parameters.InitialApproximation;
         _iterations = 0;
      }

      public IterationResult<SquareGridFunction> Solve()
      {
         var iterationResult = step();

         while (iterationResult.IterationStatus == IterationStatus.None)
         {
            iterationResult = step();
         }

         return iterationResult;
      }

      private IterationResult<SquareGridFunction> step()
      {
         var next = _zeidelMethod.Next(_current);
         _iterations++;

         var iterationResult = getResult(next);
         _current = next;

         return iterationResult;
      }

      private IterationResult<SquareGridFunction> getResult(SquareGridFunction next)
      {
         var norm = NormCalculator.Calculate(_current, next);

         if (norm > _parameters.Delta)
         {
            return IterationResult<SquareGridFunction>.Divergence(next, _iterations);
         }

         if (_iterations > _parameters.K)
         {
            return IterationResult<SquareGridFunction>.Circularity(next, _iterations);
         }

         if (norm < _parameters.Sigma)
         {
            return IterationResult<SquareGridFunction>.Convergence(next, _iterations);
         }

         return IterationResult<SquareGridFunction>.NoResult(next, _iterations);
      }
   }
}