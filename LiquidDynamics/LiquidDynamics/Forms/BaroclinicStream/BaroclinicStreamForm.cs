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

namespace LiquidDynamics.Forms.BaroclinicStream
{
   public partial class BaroclinicStreamForm : Form
   {
      private readonly ProblemParameters _problemParameters;
      private readonly ModelWind _wind;
      private readonly Solution _solution;

      private double _t;
      private double _tau;

      private int _nx;
      private int _ny;
      private int _nz;

      private Grid _x;
      private Grid _y;
      private Grid _z;

      public BaroclinicStreamForm(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");

         _problemParameters = createProblemParameters(parameters);
         _wind = new ModelWind(parameters.F1, parameters.F2,
                               parameters.SmallQ, parameters.SmallR,
                               parameters.Rho0);
         _solution = SolutionCreator.Create(parameters);

         _t = 0;
         _tau = 0.1;

         _nx = 20;
         _ny = 20;
         _nz = 20;

         _x = Grid.Create(0, parameters.SmallR, _nx);
         _y = Grid.Create(0, parameters.SmallQ, _ny);
         _z = Grid.Create(0, parameters.H, _nz);

         InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         double[,] u = calculateU(_t + _tau);
         double[,] v = calculateV(_t + _tau);

         Complex[,][] theta0 = calculateTheta(_t);
         Complex[,][] theta = calculateTheta(_t + _tau);
         Complex[,][] psi0 = calculatePsi(_t);

         Complex[,][] psi = calculatePsi(_t + _tau);
         Complex[,][] calculatedPsi = calculateBaroclinicStream(u, v, theta0, theta, psi0);

         double errorRe;
         double errorIm;
         ErrorCalculator.Calculate(psi, calculatedPsi, out errorRe, out errorIm);

         Text = string.Format("{0}; {1}", errorRe, errorIm);
      }

      private ProblemParameters createProblemParameters(Parameters parameters)
      {
         return new ProblemParameters
                   {
                      Beta = parameters.Beta,
                      F1 = parameters.F1,
                      F2 = parameters.F2,
                      H = parameters.H,
                      Mu = parameters.Mu,
                      Nu = parameters.Nu,
                      Rho0 = parameters.Rho0,
                      SmallL0 = parameters.SmallL0,
                      SmallQ = parameters.SmallQ,
                      SmallR = parameters.SmallR
                   };
      }

      private double[,] calculateU(double t)
      {
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         return g(t, barotropic.U);
      }

      private double[,] calculateV(double t)
      {
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         return g(t, barotropic.V);
      }

      private double[,] g(double t, Func<double, double, double, double> func)
      {
         var result = new double[_x.Nodes, _y.Nodes];

         for (int i = 0; i < _x.Nodes; i++)
         {
            double x = _x.Get(i);

            for (int j = 0; j < _y.Nodes; j++)
            {
               double y = _y.Get(j);
               result[i, j] = func(t, x, y);
            }
         }

         return result;
      }

      private Complex[,][] calculateTheta(double t)
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();
         return f(t, baroclinic.Theta);
      }

      private Complex[,][] calculatePsi(double t)
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();
         return f(t, baroclinic.ThetaStream);
      }

      private Complex[,][] f(double t, Func<double, double, double, double, Complex> func)
      {
         var result = new Complex[_x.Nodes,_y.Nodes][];

         for (int i = 0; i < _x.Nodes; i++)
         {
            double x = _x.Get(i);

            for (int j = 0; j < _y.Nodes; j++)
            {
               double y = _y.Get(j);
               result[i, j] = new Complex[_z.Nodes];

               for (int k = 0; k < _z.Nodes; k++)
               {
                  double z = _z.Get(k);
                  result[i, j][k] = func(t, x, y, z);
               }
            }
         }

         return result;
      }

      private Complex[,][] calculateBaroclinicStream(
         double[,] u, double[,] v,
         Complex[,][] theta0, Complex[,][] theta, Complex[,][] psi0
         )
      {
         var c = new BaroclinicStreamCalculator(_x, _y, _z, _tau,
                                                u, v,
                                                theta0, theta, psi0,
                                                _problemParameters, _wind);
         return c.Calculate();
      }
   }
}
