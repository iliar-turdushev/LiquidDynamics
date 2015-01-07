using Common;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;
using ModelProblem.Vertical;

namespace ModelProblem
{
   public sealed class Solution
   {
      private readonly IBarotropicComponent _barotropicComponent;
      private readonly IBaroclinicComponent _baroclinicComponent;
      private readonly IVerticalComponent _verticalComponent;

      internal Solution(IBarotropicComponent barotropicComponent, IBaroclinicComponent baroclinicComponent, IVerticalComponent verticalComponent)
      {
         Check.NotNull(barotropicComponent, "barotropicComponent");
         Check.NotNull(baroclinicComponent, "baroclinicComponent");
         Check.NotNull(verticalComponent, "verticalComponent");

         _barotropicComponent = barotropicComponent;
         _baroclinicComponent = baroclinicComponent;
         _verticalComponent = verticalComponent;
      }

      public IBarotropicComponent GetBarotropicComponent()
      {
         return _barotropicComponent;
      }

      public IBaroclinicComponent GetBaroclinicComponent()
      {
         return _baroclinicComponent;
      }

      public IVerticalComponent GetVerticalComponent()
      {
         return _verticalComponent;
      }
   }
}
