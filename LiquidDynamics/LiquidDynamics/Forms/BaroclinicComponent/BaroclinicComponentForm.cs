using System;
using System.Linq;
using System.Windows.Forms;
using BarotropicComponentProblem;
using Mathematics.MathTypes;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;

namespace LiquidDynamics.Forms.BaroclinicComponent
{
   public partial class BaroclinicComponentForm : Form
   {
      private readonly double _tau;
      private readonly int _n;
      private readonly double _dz;

      private readonly double _t;
      private readonly double _x;
      private readonly double _y;

      private readonly Parameters _parameters;

      public BaroclinicComponentForm(Parameters parameters)
      {
         _parameters = parameters;

         _tau = 0.01;
         _n = 100;
         _dz = _parameters.H / (_n - 1);

         _t = _tau;
         _x = _parameters.SmallR / 2;
         _y = _parameters.SmallQ / 2;

         InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         Solution solution = SolutionCreator.Create(_parameters);

         var solver =
            new BaroclinicProblemSolver(
               getParameters(),
               getExactTheta0(_t, solution),
               _tau, _dz, _n,
               _x, _y,
               getTauX(), getTauY(),
               getTauXb(solution), getTauYb(solution)
               );

         Complex[] theta = solver.Solve();
         theta = diff(theta);
         Complex[] thetaExact = getExactTheta(_t + _tau, solution.GetBaroclinicComponent());

         double re, im;
         Calculate(theta, thetaExact, out re, out im);

         Text = string.Format("{0}; {1}", re, im);
      }

      private Complex[] diff(Complex[] theta)
      {
         Complex avg = 0;

         for (int i = 0; i < theta.Length - 1; i++)
         {
            avg += theta[i + 1] + theta[i];
         }

         avg = avg * _dz / (2 * _parameters.H);

         return theta.Select(t => t - avg).ToArray();
      }

      private ProblemParameters getParameters()
      {
         return new ProblemParameters
                   {
                      Beta = _parameters.Beta,
                      F1 = _parameters.F1,
                      F2 = _parameters.F2,
                      H = _parameters.H,
                      Mu = _parameters.Mu,
                      Nu = _parameters.Nu,
                      Rho0 = _parameters.Rho0,
                      SmallL0 = _parameters.SmallL0,
                      SmallQ = _parameters.SmallQ,
                      SmallR = _parameters.SmallR
                   };
      }

      private Complex[] getExactTheta0(double t, Solution solution)
      {
         IBarotropicComponent barotropic = solution.GetBarotropicComponent();
         IBaroclinicComponent baroclinic = solution.GetBaroclinicComponent();

         var result = new Complex[_n];

         for (int i = 0; i < _n; i++)
         {
            double u = barotropic.U(t, _x, _y);
            double v = barotropic.V(t, _x, _y);
            Complex theta = baroclinic.Theta(t, _x, _y, _dz * i);
            result[i] = new Complex(u + theta.Re, v + theta.Im);
         }

         return result;
      }

      private Complex[] getExactTheta(double t, IBaroclinicComponent baroclinic)
      {
         var theta0 = new Complex[_n];

         for (int k = 0; k < _n; k++)
            theta0[k] = baroclinic.Theta(t, _x, _y, k * _dz);

         return theta0;
      }

      private double getTauX()
      {
         return -_parameters.F1 * _parameters.SmallQ * _parameters.Rho0 /
                Math.PI * Math.Cos(Math.PI * _y / _parameters.SmallQ);
      }

      private double getTauY()
      {
         return _parameters.F2 * _parameters.SmallR * _parameters.Rho0 /
                Math.PI * Math.Cos(Math.PI * _x / _parameters.SmallR) *
                Math.Sin(Math.PI * _y / _parameters.SmallQ);
      }

      private double getTauXb(Solution solution)
      {
         var barotropic = solution.GetBarotropicComponent();
         return _parameters.Mu * _parameters.Rho0 * barotropic.U(_t, _x, _y);
      }

      private double getTauYb(Solution solution)
      {
         var barotropic = solution.GetBarotropicComponent();
         return _parameters.Mu * _parameters.Rho0 * barotropic.V(_t, _x, _y);
      }

      public static void Calculate(Complex[] exact, Complex[] fact, out double errorRe, out double errorIm)
      {
         double maxRe = Math.Abs(exact[0].Re);
         double maxIm = Math.Abs(exact[0].Im);
         double maxDiffRe = Math.Abs(exact[0].Re - fact[0].Re);
         double maxDiffIm = Math.Abs(exact[0].Im - fact[0].Im);
         int n = exact.Length;
         for (int i = 1; i < n; i++)
         {
            maxRe = Math.Max(maxRe, Math.Abs(exact[i].Re));
            maxIm = Math.Max(maxIm, Math.Abs(exact[i].Im));
            maxDiffRe = Math.Max(maxDiffRe, Math.Abs(exact[i].Re - fact[i].Re));
            maxDiffIm = Math.Max(maxDiffIm, Math.Abs(exact[i].Im - fact[i].Im));
         }
         errorRe = maxDiffRe / maxRe * 100;
         errorIm = maxDiffIm / maxIm * 100;
      }
   }
}
