using BarotropicComponentProblem.IssykKulGrid;

namespace LiquidDynamics.Forms.IssykKul.Wind
{
   internal class UlanWindCalculator : WindCalculatorBase
   {
      public UlanWindCalculator(IssykKulGrid2D issykKulGrid, WindParameters windParameters)
         : base(issykKulGrid, windParameters)
      {
      }

      internal override double TauX(double x, double y)
      {
         double a1 = WindParameters.A1;
         double a2 = WindParameters.A2;
         double b1 = WindParameters.B1;
         double b2 = WindParameters.B2;
         double a1x = WindParameters.A1x;
         double a2x = WindParameters.A2x;
         double a3x = WindParameters.A3x;
         double a4x = WindParameters.A4x;

         double y1 = a1 + b1 * x;
         double y2 = a2 + b2 * x;

         double t = (y - y2) / (y1 - y2);

         return a1x * (1 - x / Lx) * (Sin(Pi * t) - a2x * Sin(2 * Pi * t) + a3x) + a4x;
      }

      internal override double TauY(double x, double y)
      {
         double a1 = WindParameters.A1;
         double a2 = WindParameters.A2;
         double b1 = WindParameters.B1;
         double b2 = WindParameters.B2;
         double a1y = WindParameters.A1y;
         double a2y = WindParameters.A2y;
         double a3y = WindParameters.A3y;
         double a4y = WindParameters.A4y;

         double y1 = a1 + b1 * x;
         double y2 = a2 + b2 * x;

         double t = (y - y2) / (y1 - y2);

         return a1y * (1 - x / Lx) * (Sin(Pi * t) - a2y * Sin(2 * Pi * t) + a3y) + a4y;
      }
   }
}