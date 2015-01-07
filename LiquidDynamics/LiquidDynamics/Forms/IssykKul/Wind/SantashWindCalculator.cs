using BarotropicComponentProblem.IssykKulGrid;

namespace LiquidDynamics.Forms.IssykKul.Wind
{
   internal class SantashWindCalculator : WindCalculatorBase
   {
      public SantashWindCalculator(IssykKulGrid2D issykKulGrid, WindParameters windParameters)
         : base(issykKulGrid, windParameters)
      {
      }

      internal override double TauX(double x, double y)
      {
         double a3 = WindParameters.A3;
         double a4 = WindParameters.A4;
         double b3 = WindParameters.B3;
         double b4 = WindParameters.B4;
         double b1x = WindParameters.B1x;
         double b2x = WindParameters.B2x;
         double b3x = WindParameters.B3x;
         double b4x = WindParameters.B4x;

         double y3 = a3 + b3 * x;
         double y4 = a4 + b4 * x;

         double t = (y - y4) / (y3 - y4);

         return b1x * x / Lx * (Sin(Pi * t) + b2x * Sin(2 * Pi * t) + b3x) + b4x;
      }

      internal override double TauY(double x, double y)
      {
         double a3 = WindParameters.A3;
         double a4 = WindParameters.A4;
         double b3 = WindParameters.B3;
         double b4 = WindParameters.B4;
         double b1y = WindParameters.B1y;
         double b2y = WindParameters.B2y;
         double b3y = WindParameters.B3y;
         double b4y = WindParameters.B4y;

         double y3 = a3 + b3 * x;
         double y4 = a4 + b4 * x;

         double t = (y - y4) / (y3 - y4);

         return b1y * x / Lx * (Sin(Pi * t) + b2y * Sin(2 * Pi * t) + b3y) + b4y;
      }
   }
}