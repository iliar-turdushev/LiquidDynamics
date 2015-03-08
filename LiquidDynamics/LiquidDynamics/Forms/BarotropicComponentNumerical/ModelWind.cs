using System;
using BarotropicComponentProblem;

namespace LiquidDynamics.Forms.BarotropicComponentNumerical
{
   internal class ModelWind : IWind
   {
      private const double Pi = Math.PI;

      private readonly double _f1;
      private readonly double _f2;
      private readonly double _q;
      private readonly double _r;
      private readonly double _rho0;

      public ModelWind(
         double f1, double f2,
         double q, double r,
         double rho0
         )
      {
         _f1 = f1;
         _f2 = f2;
         _q = q;
         _r = r;
         _rho0 = rho0;
      }

      public double TauX(double x, double y)
      {
         return -_f1 * _q * _rho0 / Pi * Math.Cos(Pi * y / _q);
      }

      public double TauY(double x, double y)
      {
         return _f2 * _r * _rho0 / Pi * Math.Cos(Pi * x / _r) * Math.Sin(Pi * y / _q);
      }
   }
}