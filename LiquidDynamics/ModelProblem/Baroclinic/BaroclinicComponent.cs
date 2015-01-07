using System;
using Common;
using Mathematics.MathTypes;

namespace ModelProblem.Baroclinic
{
   internal sealed class BaroclinicComponent : IBaroclinicComponent
   {
      private static readonly Complex I = Complex.I;

      private static readonly Func<Complex, Complex> CExp = Complex.Exp;
      private static readonly Func<double, double> Sqrt = Math.Sqrt;
      private static readonly Func<double, double> Sqr = value => value * value;

      private readonly Parameters _parameters;
      private readonly Constants _constants;
      private readonly Functions _functions;

      private Func<double, double, double, double, Complex> _theta;

      public BaroclinicComponent(Constants constants, Parameters parameters, Functions functions)
      {
         Check.NotNull(constants, "constants");
         Check.NotNull(parameters, "parameters");
         Check.NotNull(functions, "functions");

         _constants = constants;
         _parameters = parameters;
         _functions = functions;

         initialize();
      }

      public Complex Theta(double t, double x, double y, double z)
      {
         return _theta(t, x, y, z);
      } 

      private void initialize()
      {
         var h = _parameters.H;
         var rho0 = _parameters.Rho0;
         var phi = _parameters.Phi;
         
         var nu = _parameters.Nu;
         var mu = _parameters.Mu;

         Func<double, double> l = _functions.L;
         Func<double, double, Complex> tau =
            (x, y) => _functions.TauX(y) + I * _functions.TauY(x, y);

         var smallRm = _constants.SmallRm;
         var smallQk = _constants.SmallQk;
         var alpha = _parameters.Beta / (2.0 * Sqrt(Sqr(smallRm) + Sqr(smallQk)));

         _theta =
            (t, x, y, z) =>
               {
                  var l1 = _functions.Lamda1(y);
                  var l2 = _functions.Lamda2(y);
                  var l3 = _functions.Lamda3(y);

                  var t1 = l1 * (h - z);
                  var t2 = l1 * z;
                  var t3 = l1 * h;
                  var t4 = l2 * z;
                  var t5 = l2 * h;
                  var t6 = l3 * z;
                  var t7 = l3 * h;

                  var a = _functions.A(x, y);
                  var b = _functions.B(x, y);
                  var c = _functions.C(x, y);

                  var coriolis = l(y);

                  var exp1 = CExp((-mu + I * alpha) * t);
                  var exp2 = CExp((-mu - I * alpha) * t);
                  var tauValue = tau(x, y);

                  var value =
                     (tauValue * (CExp(t1) + CExp(-t1)) - rho0 * mu * a * (CExp(t2) + CExp(-t2))) / (l1 * rho0 * nu * (CExp(t3) - CExp(-t3))) -
                     mu * b * exp1 * (CExp(t4) + CExp(-t4)) / ((CExp(t5) - CExp(-t5)) * l2 * nu) -
                     mu * c * exp2 * (CExp(t6) + CExp(-t6)) / ((CExp(t7) - CExp(-t7)) * l3 * nu) +
                     phi * CExp(-I * coriolis * t) +
                     I * (tauValue / rho0 - mu * a) / (coriolis * h) +
                     mu * b * exp1 / (h * (-mu + I * (coriolis + alpha))) +
                     mu * c * exp2 / (h * (-mu + I * (coriolis - alpha)));

                  return value;
               };
      }
   }
}