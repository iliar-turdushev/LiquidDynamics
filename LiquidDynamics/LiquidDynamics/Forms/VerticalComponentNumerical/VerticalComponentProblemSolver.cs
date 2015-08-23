using System;
using System.Drawing;
using BarotropicComponentProblem;
using ControlLibrary.Types;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;
using ModelProblem.Vertical;

namespace LiquidDynamics.Forms.VerticalComponentNumerical
{
   public class VerticalComponentProblemSolver
   {
      private Grid _xGrid;
      private Grid _yGrid;
      private Grid _zGrid;
      
      private double _t;
      private double _tau;

      private Parameters _parameters;

      private IWind _wind;
      private Solution _solution;

      private Complex[,][] _psi0;
      private Complex[,][] _psi;

      private double[,][] _dxPsi;
      private double[,][] _dyPsi;

      private double[,][] _w;
      private double[,][] _exactW;

      public VerticalComponentResult Begin(int nx, int ny, int nz, double tau, Parameters parameters)
      {
         _xGrid = createGrid(nx, parameters.SmallR);
         _yGrid = createGrid(ny, parameters.SmallQ);
         _zGrid = createZGrid(nz, parameters.H);

         _t = 0.0;
         _tau = tau;

         _parameters = parameters;

         _wind = new ModelWind(_parameters.F1, _parameters.F2,
                               _parameters.SmallQ, _parameters.SmallR,
                               _parameters.Rho0);
         _solution = SolutionCreator.Create(_parameters);

         _psi0 = calculateInitialPsi();
         _psi = _psi0;

         _dxPsi = calculateDxRePsi();
         _dyPsi = calculateDyImPsi();

         _w = calculateW();
         _exactW = calculateExactW();

         return new VerticalComponentResult(buildUpwellingData(), calculateError(), _t);
      }
      
      public VerticalComponentResult Step()
      {
         _t += _tau;

         _psi = calculatePsi();
         _psi0 = _psi;

         _dxPsi = calculateDxRePsi();
         _dyPsi = calculateDyImPsi();

         _w = calculateW();

         _exactW = calculateExactW();

         return new VerticalComponentResult(buildUpwellingData(), calculateError(), _t);
      }

      private Grid createGrid(int cells, double length)
      {
         double halfCellLength = length / (2.0 * cells);
         return Grid.Create(halfCellLength, length - halfCellLength, cells);
      }

      private static Grid createZGrid(int cells, double depth)
      {
         return Grid.Create(0.0, depth, cells + 1);
      }

      private Complex[,][] calculateInitialPsi()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         var psi = new Complex[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < ny; j++)
            {
               double y = _yGrid.Get(j);
               psi[i, j] = calculateInitialPsi(x, y);
            }
         }

         return psi;
      }

      private Complex[] calculateInitialPsi(double x, double y)
      {
         const double t = 0.0;

         int nz = _zGrid.Nodes;
         double dz = _zGrid.Step;
         double nu = _parameters.Nu;

         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();

         var re = new double[nz];
         var im = new double[nz];

         for (int k = 0; k < nz; k++)
         {
            double z = _zGrid.Get(k);
            Complex c = baroclinic.Theta(t, x, y, z);
            
            re[k] = c.Re;
            im[k] = c.Im;
         }

         var psi = new Complex[nz];

         psi[0] = nu * new Complex((re[1] - re[0]) / dz,
                                   (im[1] - im[0]) / dz);

         for (int k = 1; k < nz - 1; k++)
         {
            psi[k] = nu * new Complex((re[k + 1] - re[k - 1]) / (2.0 * dz),
                                      (im[k + 1] - im[k - 1]) / (2.0 * dz));
         }

         psi[nz - 1] = nu * new Complex((re[nz - 1] - re[nz - 2]) / dz,
                                        (im[nz - 1] - im[nz - 2]) / dz);

         return psi;
      }

      private double[,][] calculateDxRePsi()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         double dx = _xGrid.Step;

         var dxRePsi = new double[nx, ny][];

         for (int j = 0; j < ny; j++)
            dxRePsi[0, j] = calculateDxRePsi(dx, _psi[0, j], _psi[1, j]);

         for (int i = 1; i < nx - 1; i++)
            for (int j = 0; j < ny; j++)
               dxRePsi[i, j] = calculateDxRePsi(2.0 * dx, _psi[i - 1, j], _psi[i + 1, j]);

         for (int j = 0; j < ny; j++)
            dxRePsi[nx - 1, j] = calculateDxRePsi(dx, _psi[nx - 2, j], _psi[nx - 1, j]);

         return dxRePsi;
      }

      private double[] calculateDxRePsi(double dx, Complex[] psi0, Complex[] psi1)
      {
         int nz = _zGrid.Nodes;
         var dxRe = new double[nz];

         for (int k = 0; k < nz; k++)
            dxRe[k] = (psi1[k].Re - psi0[k].Re) / dx;

         return dxRe;
      }

      private double[,][] calculateDyImPsi()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         double dy = _yGrid.Step;

         var dyRePsi = new double[nx, ny][];

         for (int i = 0; i < nx; i++)
            dyRePsi[i, 0] = calculateDyImPsi(dy, _psi[i, 0], _psi[i, 1]);

         for (int j = 1; j < ny - 1; j++)
            for (int i = 0; i < nx; i++)
               dyRePsi[i, j] = calculateDyImPsi(2.0 * dy, _psi[i, j - 1], _psi[i, j + 1]);

         for (int i = 0; i < nx; i++)
            dyRePsi[i, ny - 1] = calculateDyImPsi(dy, _psi[i, ny - 2], _psi[i, ny - 1]);

         return dyRePsi;
      }

      private double[] calculateDyImPsi(double dy, Complex[] psi0, Complex[] psi1)
      {
         int nz = _zGrid.Nodes;
         var dyIm = new double[nz];

         for (int k = 0; k < nz; k++)
            dyIm[k] = (psi1[k].Im - psi0[k].Im) / dy;

         return dyIm;
      }

      private double[,][] calculateW()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         var w = new double[nx, ny][];

         for (int i = 0; i < nx; i++)
            for (int j = 0; j < ny; j++)
               w[i, j] = calculateW(_dxPsi[i, j], _dyPsi[i, j]);

         return w;
      }

      private double[] calculateW(double[] dxPsi, double[] dyPsi)
      {
         int nz = _zGrid.Nodes;
         double dz = _zGrid.Step;
         double nu = _parameters.Nu;

         var a = new double[nz];
         var b = new double[nz];
         var c = new double[nz - 1];
         var f = new double[nz];

         b[0] = 1.0;
         c[0] = 0.0;
         f[0] = 0.0;

         for (int k = 1; k < nz - 1; k++)
         {
            a[k] = 1.0 / (dz * dz);
            b[k] = 2.0 / (dz * dz);
            c[k] = 1.0 / (dz * dz);
            f[k] = (dxPsi[k] + dyPsi[k]) / nu;
         }

         a[nz - 1] = 0.0;
         b[nz - 1] = 1.0;
         f[nz - 1] = 0.0;

         var w = new double[nz];

         var alpha = new double[nz - 1];
         var beta = new double[nz - 1];

         alpha[0] = c[0] / b[0];
         beta[0] = f[0] / b[0];

         for (int k = 1; k < nz - 1; k++)
         {
            double div = b[k] - alpha[k - 1] * a[k];
            alpha[k] = c[k] / div;
            beta[k] = (f[k] + beta[k - 1] * a[k]) / div;
         }

         w[nz - 1] = (f[nz - 1] + a[nz - 1] * beta[nz - 2]) /
                     (b[nz - 1] - a[nz - 1] * alpha[nz - 2]);

         for (int k = nz - 2; k >= 0; k--)
            w[k] = alpha[k] * w[k + 1] + beta[k];

         return w;
      }

      private double[,][] calculateExactW()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;
         int nz = _zGrid.Nodes;

         IVerticalComponent vertical = _solution.GetVerticalComponent();

         var w = new double[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < ny; j++)
            {
               double y = _yGrid.Get(j);
               w[i, j] = new double[nz];

               for (int k = 0; k < nz; k++)
               {
                  double z = _zGrid.Get(k);
                  w[i, j][k] = vertical.W(_t, x, y, z);
               }
            }
         }

         return w;
      }

      private double calculateError()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;
         int nz = _zGrid.Nodes;
         
         double max = 0.0;
         double maxDiff = 0.0;

         for (int i = 0; i < nx; i++)
            for (int j = 0; j < ny; j++)
               for (int k = 0; k < nz; k++)
               {
                  max = Math.Max(max, Math.Abs(_exactW[i, j][k]));
                  maxDiff = Math.Max(maxDiff, Math.Abs(_exactW[i, j][k] - _w[i, j][k]));
               }

         return maxDiff / max * 100.0;
      }

      private UpwellingData[] buildUpwellingData()
      {
         int nz = _zGrid.Nodes;
         var result = new UpwellingData[nz];

         for (int k = 0; k < nz; k++)
            result[k] = buildUpwellingData(k);

         return result;
      }

      private UpwellingData buildUpwellingData(int slice)
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         var gridPoints = new PointF[nx, ny];
         var intensities = new float[nx, ny];

         for (var i = 0; i < nx; i++)
         {
            double x = _xGrid.Get(i);

            for (var j = 0; j < ny; j++)
            {
               double y = _yGrid.Get(j);
               gridPoints[i, j] = new PointF((float) x, (float) y);
               intensities[i, j] = (float) _w[i, j][slice];
            }
         }

         var cellSize = new SizeF((float) _xGrid.Step, (float) _yGrid.Step);
         return new UpwellingData(gridPoints, intensities, cellSize);
      }

      private Complex[,][] calculatePsi()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         var psi = new Complex[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < ny; j++)
            {
               double y = _yGrid.Get(j);
               psi[i, j] = calculatePsi(x, y, _psi0[i, j]);
            }
         }

         return psi;
      }

      private Complex[] calculatePsi(double x, double y, Complex[] psi0)
      {
         int nz = _zGrid.Nodes;
         double dz = _zGrid.Step;
         double nu = _parameters.Nu;

         Complex sigma = calculateSigma(y);

         Complex[] theta0 = calculateTheta(_t - _tau, x, y);
         Complex[] theta1 = calculateTheta(_t, x, y);

         var psi = new Complex[nz];
         psi[0] = calculatePsi1(x, y);

         for (int k = 1; k < nz - 1; k++)
         {
            psi[k] = nu * (theta1[k + 1] - theta1[k]) / (2.0 * dz) +
                     nu * (theta1[k] - theta1[k - 1]) / (2.0 * dz) +
                     (1.0 / sigma - 1.0) * (nu * (theta0[k + 1] - theta0[k]) / (2.0 * dz) +
                                            nu * (theta0[k] - theta0[k - 1]) / (2.0 * dz) -
                                            psi0[k]);
         }

         psi[nz - 1] = calculatePsiN(x, y);

         return psi;
      }

      private Complex calculateSigma(double y)
      {
         double mu = _parameters.Mu;
         double l = calculateL(y);

         double exp = Math.Exp(-mu * _tau);
         double alpha = 1.0 - Math.Cos(l * _tau) * exp;
         double beta = Math.Sin(l * _tau) * exp;

         double sqr = alpha * alpha + beta * beta;
         double a = (mu * alpha + l * beta) / sqr;
         double b = (l * alpha - mu * beta) / sqr;

         return (a + Complex.I * b - 1.0 / _tau) / (mu + Complex.I * l);
      }

      private double calculateL(double y)
      {
         double l0 = _parameters.SmallL0;
         double beta = _parameters.Beta;
         return l0 + beta * y;
      }

      private Complex[] calculateTheta(double t, double x, double y)
      {
         int nz = _zGrid.Nodes;

         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         double u = barotropic.U(t, x, y);
         double v = barotropic.V(t, x, y);

         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();
         var theta = new Complex[nz];

         for (int k = 0; k < nz; k++)
         {
            double z = _zGrid.Get(k);
            Complex c = baroclinic.Theta(t, x, y, z);
            theta[k] = new Complex(u + c.Re, v + c.Im);
         }

         return theta;
      }

      private Complex calculatePsi1(double x, double y)
      {
         double rho0 = _parameters.Rho0;
         return -(_wind.TauX(x, y) + Complex.I * _wind.TauY(x, y)) / rho0;
      }

      private Complex calculatePsiN(double x, double y)
      {
         double mu = _parameters.Mu;
         double rho0 = _parameters.Rho0;

         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         double u = barotropic.U(_t, x, y);
         double v = barotropic.V(_t, x, y);

         return -(mu * rho0 * u + Complex.I * mu * rho0 * v) / rho0;
      }
   }
}