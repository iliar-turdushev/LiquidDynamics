using BarotropicComponentProblem.IterationMethod;
using Common;
using ControlLibrary.Types;

namespace LiquidDynamics.Forms.BarotropicComponentNumerical
{
   internal sealed class BarotropicComponentResult
   {
      internal BarotropicComponentResult(
         SquareVelocityField exactSolution,
         SquareVelocityField calculatedSolution,
         double errorU, double errorV, double time,
         IterationStatus iterationStatus = IterationStatus.None)
      {
         Check.NotNull(exactSolution, "exactSolution");
         Check.NotNull(calculatedSolution, "calculatedSolution");

         ExactSolution = exactSolution;
         CalculatedSolution = calculatedSolution;
         ErrorU = errorU;
         ErrorV = errorV;
         Time = time;
         IterationStatus = iterationStatus;
      }

      internal SquareVelocityField ExactSolution { get; private set; }
      internal SquareVelocityField CalculatedSolution { get; private set; }
      internal double ErrorU { get; private set; }
      internal double ErrorV { get; private set; }
      internal double Time { get; private set; }
      internal IterationStatus IterationStatus { get; private set; }
   }
}