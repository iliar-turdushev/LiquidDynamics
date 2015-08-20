using System;
using System.Windows.Forms;
using BarotropicComponentProblem;
using Common;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;
using ModelProblem.Vertical;

namespace LiquidDynamics.Forms.VerticalComponentNumerical
{
   public partial class VerticalComponentForm : Form
   {
      private static readonly Complex I = Complex.I;

      private readonly double _x0 = 0.5;
      private readonly double _x1 = 0.51;
      private readonly double _dx = 0.01;

      private readonly double _y0 = 0.5;
      private readonly double _y1 = 0.51;
      private readonly double _dy = 0.01;

      private readonly double _t = 0.0;
      private readonly double _tau = 0.1;
      
      private readonly Parameters _problemParameters;
      private readonly Solution _solution;

      private readonly int _n = 100;
      private readonly double _h = 1.0;
      private readonly Grid _zGrid;

      private IWind _wind;

      public VerticalComponentForm(Parameters problemParameters)
      {
         Check.NotNull(problemParameters, "problemParameters");

         _problemParameters = problemParameters;
         _solution = SolutionCreator.Create(_problemParameters);

         _zGrid = Grid.Create(0, _h, _n);

         _wind = new ModelWind(_problemParameters.F1,
                               _problemParameters.F2,
                               _problemParameters.SmallQ,
                               _problemParameters.SmallR,
                               _problemParameters.Rho0);

         InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         double[] dxPsi = calculateDx(calculatePsi(_t, _x0 - _dx, _y0),
                                      calculatePsi(_t, _x0 + _dx, _y0));
         double[] dyPsi = calculateDy(calculatePsi(_t, _x0, _y0 - _dy),
                                      calculatePsi(_t, _x0, _y0 + _dy));

         double[] w = calculateW(dxPsi, dyPsi);
         double[] exactW = calculateExactW();

         double error = calculateError(exactW, w);

         Text = error.ToString();
      }

      private double calculateError(double[] exact, double[] calculated)
      {
         double max = Math.Abs(exact[0]);
         double maxDiff = Math.Abs(exact[0] - calculated[0]);

         for (int i = 1; i < exact.Length; i++)
         {
            max = Math.Max(max, Math.Abs(exact[i]));
            maxDiff = Math.Max(maxDiff, Math.Abs(exact[i] - calculated[i]));
         }

         return maxDiff / max * 100;
      }

      private Complex[] calculatePsi(double t, double x, double y)
      {
         double nu = _problemParameters.Nu;
         double dz = _zGrid.Step;
         Complex sigma = calculateSigma(y);

         Complex[] theta0 = calculateTheta(t, x, y);
         Complex[] theta1 = calculateTheta(t + _tau, x, y);
         Complex[] psi0 = calculatePsi0(t, x, y);
         
         var psi = new Complex[_zGrid.Nodes];

         psi[0] = calculatePsi1(x, y);

         for (int k = 1; k < _zGrid.Nodes - 1; k++)
         {
            psi[k] = nu * (theta1[k + 1] - theta1[k]) / (2 * dz) +
                     nu * (theta1[k] - theta1[k - 1]) / (2 * dz) +
                     (1 / sigma - 1) * (nu * (theta0[k + 1] - theta0[k]) / (2 * dz) +
                                        nu * (theta0[k] - theta0[k - 1]) / (2 * dz)) -
                     (1 / sigma - 1) * psi0[k];

         }

         psi[_zGrid.Nodes - 1] = calculatePsiN(t + _tau, x, y);

         return psi;
      }

      private Complex calculateSigma(double y)
      {
         double mu = _problemParameters.Mu;
         double l = calculateL(y);

         double exp = Math.Exp(-mu * _tau);
         double alpha = 1.0 - Math.Cos(l * _tau) * exp;
         double beta = Math.Sin(l * _tau) * exp;

         double sqr = alpha * alpha + beta * beta;
         double a = (mu * alpha + l * beta) / sqr;
         double b = (l * alpha - mu * beta) / sqr;

         return (a + I * b - 1.0 / _tau) / (mu + I * l);
      }

      private double calculateL(double y)
      {
         double l0 = _problemParameters.SmallL0;
         double beta = _problemParameters.Beta;
         return l0 + beta * y;
      }

      private Complex[] calculateTheta(double t, double x, double y)
      {
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         double u = barotropic.U(t, x, y);
         double v = barotropic.V(t, x, y);

         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();

         var theta = new Complex[_zGrid.Nodes];

         for (int i = 0; i < _zGrid.Nodes; i++)
         {
            double z = _zGrid.Get(i);
            Complex c = baroclinic.Theta(t, x, y, z);
            theta[i] = new Complex(u + c.Re, v + c.Im);
         }

         return theta;
      }

      private Complex[] calculatePsi0(double t, double x, double y)
      {
         double dz = _zGrid.Step;
         int n = _zGrid.Nodes;

         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();

         var theta0 = new Complex[n];

         for (int i = 0; i < n; i++)
         {
            theta0[i] = baroclinic.Theta(t, x, y, _zGrid.Get(i));
         }

         var psi0 = new Complex[n];
         double nu = _problemParameters.Nu;

         psi0[0] = nu * new Complex((theta0[1].Re - theta0[0].Re) / dz,
                                    (theta0[1].Im - theta0[0].Im) / dz);

         for (int i = 1; i < n - 1; i++)
         {
            psi0[i] = nu * new Complex((theta0[i + 1].Re - theta0[i - 1].Re) / (2 * dz),
                                       (theta0[i + 1].Im - theta0[i - 1].Im) / (2 * dz));
         }

         psi0[n - 1] = nu * new Complex((theta0[n - 1].Re - theta0[n - 2].Re) / dz,
                                        (theta0[n - 1].Im - theta0[n - 2].Im) / dz);

         return psi0;
      }

      private Complex calculatePsi1(double x, double y)
      {
         double rho0 = _problemParameters.Rho0;
         return -(_wind.TauX(x, y) + I * _wind.TauY(x, y)) / rho0;
      }

      private Complex calculatePsiN(double t, double x, double y)
      {
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         double u = barotropic.U(t, x, y);
         double v = barotropic.V(t, x, y);

         double mu = _problemParameters.Mu;
         double rho0 = _problemParameters.Rho0;

         return -(mu * rho0 * u + I * mu * rho0 * v) / rho0;
      }

      private double[] calculateDx(Complex[] psi0, Complex[] psi1)
      {
         int n = _zGrid.Nodes;
         var result = new double[n];

         for (int i = 0; i < n; i++)
            result[i] = (psi1[i].Re - psi0[i].Re) / (2 * _dx);

         return result;
      }

      private double[] calculateDy(Complex[] psi0, Complex[] psi1)
      {
         int n = _zGrid.Nodes;
         var result = new double[n];

         for (int i = 0; i < n; i++)
            result[i] = (psi1[i].Im - psi0[i].Im) / (2 * _dy);

         return result;
      }

      private double[] calculateW(double[] dxPsi, double[] dyPsi)
      {
         int n = _zGrid.Nodes;
         double dz = _zGrid.Step;
         double nu = _problemParameters.Nu;

         var a = new double[n];
         var b = new double[n];
         var c = new double[n - 1];
         var f = new double[n];

         b[0] = 1;
         c[0] = 0;
         f[0] = 0;

         for (int k = 1; k < n - 1; k++)
         {
            a[k] = 1 / (dz * dz);
            b[k] = 2 / (dz * dz);
            c[k] = 1 / (dz * dz);
            f[k] = 1 / nu * (dxPsi[k] + dyPsi[k]);
         }

         a[n - 1] = 0;
         b[n - 1] = 1;
         f[n - 1] = 0;

         var w = new double[n];

         var alpha = new double[n - 1];
         var beta = new double[n - 1];

         alpha[0] = c[0] / b[0];
         beta[0] = f[0] / b[0];

         for (int i = 1; i < n - 1; i++)
         {
            double div = b[i] - alpha[i - 1] * a[i];
            alpha[i] = c[i] / div;
            beta[i] = (f[i] + beta[i - 1] * a[i]) / div;
         }

         w[n - 1] = (f[n - 1] + a[n - 1] * beta[n - 2]) /
                    (b[n - 1] - a[n - 1] * alpha[n - 2]);

         for (int i = n - 2; i >= 0; i--)
            w[i] = alpha[i] * w[i + 1] + beta[i];

         return w;
      }

      private double[] calculateExactW()
      {
         IVerticalComponent vertical = _solution.GetVerticalComponent();
         var w = new double[_zGrid.Nodes];

         for (int i = 0; i < _zGrid.Nodes; i++)
            w[i] = vertical.W(_t + _tau, _x0, _y0, _zGrid.Get(i));

         return w;
      }
   }
}
