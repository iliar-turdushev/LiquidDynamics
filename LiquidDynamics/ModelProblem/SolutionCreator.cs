using Common;
using Mathematics.Helpers;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;
using ModelProblem.Vertical;

namespace ModelProblem
{
   public static class SolutionCreator
   {
      public static Solution Create(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");

         var constants = new Constants(parameters);
         var barotropicComponent = new BarotropicComponent(constants, parameters);

         var functions = new Functions(constants, parameters);
         var baroclinicComponent = new BaroclinicComponent(constants, parameters, functions);

         var verticalComponent = createVerticalComponent(parameters, constants);

         return new Solution(barotropicComponent, baroclinicComponent, verticalComponent);
      }

      private static IVerticalComponent createVerticalComponent(Parameters parameters, Constants constants)
      {
         IVerticalComponent verticalComponent;

         if (parameters.Beta.IsZero() && parameters.F2.IsZero() && parameters.S2.IsZero())
            verticalComponent = new VerticalComponent(constants, parameters);
         else
            verticalComponent = new EmptyVerticalComponent();

         return verticalComponent;
      }
   }
}