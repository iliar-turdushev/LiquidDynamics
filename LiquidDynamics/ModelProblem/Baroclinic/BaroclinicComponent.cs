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
      private Func<double, double, double, double, Complex> _thetaStream;

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

      public Complex ThetaStream(double t, double x, double y, double z)
      {
         return _thetaStream(t, x, y, z);
      }

      private void initialize()
      {
         double h = _parameters.H;
         double rho0 = _parameters.Rho0;
         double phi = _parameters.Phi;
         
         double nu = _parameters.Nu;
         double mu = _parameters.Mu;

         Func<double, double> l = _functions.L;
         Func<double, double, Complex> tau =
            (x, y) => _functions.TauX(y) + I * _functions.TauY(x, y);

         double smallRm = _constants.SmallRm;
         double smallQk = _constants.SmallQk;
         double alpha = _parameters.Beta / (2.0 * Sqrt(Sqr(smallRm) + Sqr(smallQk)));

         _theta =
            (t, x, y, z) =>
               {
                  Complex l1 = _functions.Lamda1(y);
                  Complex l2 = _functions.Lamda2(y);
                  Complex l3 = _functions.Lamda3(y);

                  Complex t1 = l1 * (h - z);
                  Complex t2 = l1 * z;
                  Complex t3 = l1 * h;
                  Complex t4 = l2 * z;
                  Complex t5 = l2 * h;
                  Complex t6 = l3 * z;
                  Complex t7 = l3 * h;

                  Complex a = _functions.A(x, y);
                  Complex b = _functions.B(x, y);
                  Complex c = _functions.C(x, y);

                  double coriolis = l(y);

                  Complex exp1 = CExp((-mu + I * alpha) * t);
                  Complex exp2 = CExp((-mu - I * alpha) * t);
                  Complex tauValue = tau(x, y);

                  Complex value =
                     (tauValue * (CExp(t1) + CExp(-t1)) - rho0 * mu * a * (CExp(t2) + CExp(-t2))) / (l1 * rho0 * nu * (CExp(t3) - CExp(-t3))) -
                     mu * b * exp1 * (CExp(t4) + CExp(-t4)) / ((CExp(t5) - CExp(-t5)) * l2 * nu) -
                     mu * c * exp2 * (CExp(t6) + CExp(-t6)) / ((CExp(t7) - CExp(-t7)) * l3 * nu) +
                     phi * CExp(-I * coriolis * t) +
                     I * (tauValue / rho0 - mu * a) / (coriolis * h) +
                     mu * b * exp1 / (h * (-mu + I * (coriolis + alpha))) +
                     mu * c * exp2 / (h * (-mu + I * (coriolis - alpha)));

                  return value;
               };

         _thetaStream =
            (t, x, y, z) =>
               {
                  Complex l1 = _functions.Lamda1(y);
                  Complex l2 = _functions.Lamda2(y);
                  Complex l3 = _functions.Lamda3(y);

                  Complex a = _functions.A(x, y);
                  Complex b = _functions.B(x, y);
                  Complex c = _functions.C(x, y);

                  Complex v1 = -(tau(x, y) * (CExp(l1 * (h - z)) - CExp(-l1 * (h - z))) +
                                 rho0 * mu * a * (CExp(l1 * z) - CExp(-l1 * z))) /
                               (rho0 * (CExp(l1 * h) - CExp(-l1 * h)));

                  Complex v2 = -mu * b * (CExp(l2 * z) - CExp(-l2 * z)) /
                               (CExp(l2 * h) - CExp(-l2 * h)) *
                               CExp((-mu + I * alpha) * t);

                  Complex v3 = -mu * c * (CExp(l3 * z) - CExp(-l3 * z)) /
                               (CExp(l3 * h) - CExp(-l3 * h)) *
                               CExp((-mu - I * alpha) * t);

                  return v1 + v2 + v3;
               };
      }
   }
}