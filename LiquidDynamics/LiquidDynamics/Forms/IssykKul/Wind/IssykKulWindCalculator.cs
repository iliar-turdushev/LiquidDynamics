using BarotropicComponentProblem.IssykKulGrid;

namespace LiquidDynamics.Forms.IssykKul.Wind
{
   internal class IssykKulWindCalculator : WindCalculatorBase
   {
      private readonly UlanWindCalculator _ulan;
      private readonly SantashWindCalculator _santash;

      public IssykKulWindCalculator(
         UlanWindCalculator ulan, SantashWindCalculator santash,
         IssykKulGrid2D issykKulGrid, WindParameters windParameters
         )
         : base(issykKulGrid, windParameters)
      {
         _ulan = ulan;
         _santash = santash;
      }

      internal override double TauX(double x, double y)
      {
         return _ulan.TauX(x, y) + _santash.TauX(x, y);
      }

      internal override double TauY(double x, double y)
      {
         return _ulan.TauY(x, y) + _santash.TauY(x, y);
      }
   }
}