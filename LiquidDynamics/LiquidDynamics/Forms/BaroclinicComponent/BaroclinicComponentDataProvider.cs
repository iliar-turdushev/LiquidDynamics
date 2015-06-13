using Common;
using ModelProblem;

namespace LiquidDynamics.Forms.BaroclinicComponent
{
   internal class BaroclinicComponentDataProvider
   {
      private readonly Parameters _parameters;

      public BaroclinicComponentDataProvider(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");
         _parameters = parameters;
      }


   }
}