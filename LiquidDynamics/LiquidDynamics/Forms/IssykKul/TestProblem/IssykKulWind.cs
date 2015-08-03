using System;
using BarotropicComponentProblem;
using BarotropicComponentProblem.TestProblem;
using LiquidDynamics.Forms.IssykKul.Wind;

namespace LiquidDynamics.Forms.IssykKul.TestProblem
{
   internal sealed class IssykKulWind : IWind
   {
      private const double Pi = Math.PI;
      private const double Lx = 175.67;

      private static readonly Func<double, double> Sin = x => Math.Sin(x);

      private readonly WindParameters _parameters;

      public IssykKulWind(WindParameters parameters)
      {
         _parameters = parameters;
      }

      public double TauX(double x, double y)
      {
         double sx = stretch(x);
         double sy = stretch(y);
         return ulanTauX(sx, sy) + santashTauX(sx, sy);
      }

      public double TauY(double x, double y)
      {
         double sx = stretch(x);
         double sy = stretch(y);
         return ulanTauY(sx, sy) + santashTauY(sx, sy);
      }

      private double stretch(double value)
      {
         return StretchCoefficients.L0 * value;
      }

      private double ulanTauX(double x, double y)
      {
         double a1 = _parameters.A1;
         double a2 = _parameters.A2;
         double b1 = _parameters.B1;
         double b2 = _parameters.B2;
         double a1x = _parameters.A1x;
         double a2x = _parameters.A2x;
         double a3x = _parameters.A3x;
         double a4x = _parameters.A4x;

         double y1 = a1 + b1 * x;
         double y2 = a2 + b2 * x;

         double t = (y - y2) / (y1 - y2);

         return a1x * (1 - x / Lx) * (Sin(Pi * t) - a2x * Sin(2 * Pi * t) + a3x) + a4x;
      }

      private double ulanTauY(double x, double y)
      {
         double a1 = _parameters.A1;
         double a2 = _parameters.A2;
         double b1 = _parameters.B1;
         double b2 = _parameters.B2;
         double a1y = _parameters.A1y;
         double a2y = _parameters.A2y;
         double a3y = _parameters.A3y;
         double a4y = _parameters.A4y;

         double y1 = a1 + b1 * x;
         double y2 = a2 + b2 * x;

         double t = (y - y2) / (y1 - y2);

         return a1y * (1 - x / Lx) * (Sin(Pi * t) - a2y * Sin(2 * Pi * t) + a3y) + a4y;
      }

      private double santashTauX(double x, double y)
      {
         double a3 = _parameters.A3;
         double a4 = _parameters.A4;
         double b3 = _parameters.B3;
         double b4 = _parameters.B4;
         double b1x = _parameters.B1x;
         double b2x = _parameters.B2x;
         double b3x = _parameters.B3x;
         double b4x = _parameters.B4x;

         double y3 = a3 + b3 * x;
         double y4 = a4 + b4 * x;

         double t = (y - y4) / (y3 - y4);

         return b1x * x / Lx * (Sin(Pi * t) + b2x * Sin(2 * Pi * t) + b3x) + b4x;
      }

      private double santashTauY(double x, double y)
      {
         double a3 = _parameters.A3;
         double a4 = _parameters.A4;
         double b3 = _parameters.B3;
         double b4 = _parameters.B4;
         double b1y = _parameters.B1y;
         double b2y = _parameters.B2y;
         double b3y = _parameters.B3y;
         double b4y = _parameters.B4y;

         double y3 = a3 + b3 * x;
         double y4 = a4 + b4 * x;

         double t = (y - y4) / (y3 - y4);

         return b1y * x / Lx * (Sin(Pi * t) + b2y * Sin(2 * Pi * t) + b3y) + b4y;
      }
   }
}