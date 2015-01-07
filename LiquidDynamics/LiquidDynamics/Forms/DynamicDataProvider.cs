using Common;
using LiquidDynamics.Forms.ParametersForms;
using ModelProblem;

namespace LiquidDynamics.Forms
{
   internal abstract class DynamicDataProvider<T>
   {
      private readonly Parameters _parameters;
      private readonly GraphParameters _graphParameters;
      private readonly Solution _solution;

      protected DynamicDataProvider(Parameters parameters, GraphParameters graphParameters)
      {
         Check.NotNull(parameters, "parameters");
         Check.NotNull(graphParameters, "graphParameters");

         _parameters = parameters;
         _graphParameters = graphParameters;
         _solution = SolutionCreator.Create(_parameters);
      }

      public double Time { get; private set; }

      public T GetData()
      {
         return getData();
      }

      public void Reset()
      {
         Time = 0;
      }

      public void StepForward()
      {
         Time += _graphParameters.TimeStep;
      }

      public void StepBackward()
      {
         Time -= _graphParameters.TimeStep;
      }

      protected Parameters getParameters()
      {
         return _parameters;
      }

      protected GraphParameters getGraphParameters()
      {
         return _graphParameters;
      }

      protected Solution getSolution()
      {
         return _solution;
      }

      protected abstract T getData();
   }
}