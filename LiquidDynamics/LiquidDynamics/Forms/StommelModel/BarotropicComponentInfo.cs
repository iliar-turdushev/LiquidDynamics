using BarotropicComponentProblem.IterationMethod;

namespace LiquidDynamics.Forms.StommelModel
{
   internal sealed class BarotropicComponentInfo
   {
      internal BarotropicComponentInfo(IterationStatus iterationStatus, int iterationNumber, double error)
      {
         IterationStatus = iterationStatus;
         IterationNumber = iterationNumber;
         Error = error;
      }

      internal BarotropicComponentInfo(double error)
         : this(IterationStatus.None, 0, error)
      {
      }

      internal IterationStatus IterationStatus { get; private set; }
      internal int IterationNumber { get; private set; }
      internal double Error { get; private set; }
   }
}