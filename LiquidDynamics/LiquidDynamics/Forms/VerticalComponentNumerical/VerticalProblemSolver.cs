using System;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace LiquidDynamics.Forms.VerticalComponentNumerical
{
   public class VerticalProblemSolver
   {
      private readonly Grid _xGrid;
      private readonly Grid _yGrid;

      private readonly IWind _wind;
      private readonly ProblemParameters _parameters;
      private Complex[,][] _theta0;
      private readonly IssykKulGrid3D _issykKulGrid3D;
      private Complex[,][] _theta1;

      private Complex[,][] _psi0;
      private Complex[,][] _psi;

      private double[,][] _dxPsi;
      private double[,][] _dyPsi;

      private double[,] _u;
      private double[,] _v;

      private double _t;
      private readonly double _tau;

      public VerticalProblemSolver(Grid xGrid, Grid yGrid, double tau, IWind wind, ProblemParameters parameters, Complex[,][] theta0, IssykKulGrid3D issykKulGrid3D)
      {
         _xGrid = xGrid;
         _yGrid = yGrid;
         _tau = tau;
         _wind = wind;
         _parameters = parameters;
         _theta0 = theta0;
         _theta1 = theta0;
         _issykKulGrid3D = issykKulGrid3D;
      }

      public double[,][] Begin()
      {
         _t = 0.0;

         _psi0 = calculateInitialPsi();
         _psi = _psi0;

         _dxPsi = calculateDxRePsi();
         _dyPsi = calculateDyImPsi();

         return calculateW();
      }

      public double[,][] Step(Complex[,][] theta1, double[,] u, double[,] v)
      {
         _theta1 = theta1;

         _u = u;
         _v = v;

         _t += _tau;

         _psi = calculatePsi();
         _psi0 = _psi;

         _dxPsi = calculateDxRePsi();
         _dyPsi = calculateDyImPsi();

         _theta0 = _theta1;

         return calculateW();
      }

      private Complex[,][] calculateInitialPsi()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         var psi = new Complex[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            for (int j = 0; j < ny; j++)
            {
               if (_theta0[i, j] == null)
                  continue;

               psi[i, j] = calculateInitialPsi(i, j);
            }
         }

         return psi;
      }

      private int getNz(int i, int j)
      {
         i = i == _xGrid.Nodes - 1 ? _xGrid.Nodes - 2 : i;
         j = j == _yGrid.Nodes - 1 ? _yGrid.Nodes - 2 : j;
         Rectangle3D[] depthGrid = _issykKulGrid3D.GetDepthGrid(i, j);
         return depthGrid.Length + 1;
      }

      private double getDz(int i, int j)
      {
         i = i == _xGrid.Nodes - 1 ? _xGrid.Nodes - 2 : i;
         j = j == _yGrid.Nodes - 1 ? _yGrid.Nodes - 2 : j;
         Rectangle3D[] depthGrid = getDepthGrid(i, j);
         return depthGrid[0].Hz;
      }

      private Rectangle3D[] getDepthGrid(int i, int j)
      {
         return _issykKulGrid3D.GetDepthGrid(i, j) ??
                _issykKulGrid3D.GetDepthGrid(i - 1, j) ??
                _issykKulGrid3D.GetDepthGrid(i, j - 1) ??
                _issykKulGrid3D.GetDepthGrid(i - 1, j - 1);
      }

      private Complex[] calculateInitialPsi(int i, int j)
      {
         int nz = _theta0[i, j].Length;
         double dz = getDz(i, j);
         double nu = _parameters.Nu;

         Complex[] theta = _theta0[i, j];

         var re = new double[nz];
         var im = new double[nz];

         for (int k = 0; k < nz; k++)
         {
            re[k] = theta[k].Re;
            im[k] = theta[k].Im;
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
         IssykKulGrid2D grid = _issykKulGrid3D.Grid2D;

         int nx = grid.N;
         int ny = grid.M;

         double dx = grid.Hx;

         var dxRePsi = new double[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            for (int j = 0; j < ny; j++)
            {
               if (grid[i, j] == GridCell.Empty)
                  continue;

               int nz = getNz(i, j);
               var dxRe = new double[nz];

               for (int k = 0; k < nz; k++)
               {
                  double re1 = _psi[i + 1, j + 1].Length > k ? _psi[i + 1, j + 1][k].Re : 0;
                  double re2 = _psi[i, j + 1].Length > k ? _psi[i, j + 1][k].Re : 0;
                  double re3 = _psi[i + 1, j].Length > k ? _psi[i + 1, j][k].Re : 0;
                  double re4 = _psi[i, j].Length > k ? _psi[i, j][k].Re : 0;

                  dxRe[k] = (re1 - re2 + re3 - re4) / (2 * dx);
               }

               dxRePsi[i, j] = dxRe;
            }
         }

         return dxRePsi;
      }

      private double[,][] calculateDyImPsi()
      {
         IssykKulGrid2D grid = _issykKulGrid3D.Grid2D;

         int nx = grid.N;
         int ny = grid.M;

         double dy = grid.Hy;

         var dyRePsi = new double[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            for (int j = 0; j < ny; j++)
            {
               if (grid[i, j] == GridCell.Empty)
                  continue;

               int nz = getNz(i, j);
               var dyIm = new double[nz];

               for (int k = 0; k < nz; k++)
               {
                  double im1 = _psi[i + 1, j + 1].Length > k ? _psi[i + 1, j + 1][k].Im : 0;
                  double im2 = _psi[i + 1, j].Length > k ? _psi[i + 1, j][k].Im : 0;
                  double im3 = _psi[i, j + 1].Length > k ? _psi[i, j + 1][k].Im : 0;
                  double im4 = _psi[i, j].Length > k ? _psi[i, j][k].Im : 0;

                  dyIm[k] = (im1 - im2 + im3 - im4) / (2 * dy);
               }

               dyRePsi[i, j] = dyIm;
            }
         }

         return dyRePsi;
      }

      private double[,][] calculateW()
      {
         IssykKulGrid2D grid = _issykKulGrid3D.Grid2D;
         int nx = grid.N;
         int ny = grid.M;

         var w = new double[nx, ny][];

         for (int i = 0; i < nx; i++)
            for (int j = 0; j < ny; j++)
            {
               if (grid[i, j] == GridCell.Empty)
                  continue;

               w[i, j] = calculateW(i, j, _dxPsi[i, j], _dyPsi[i, j]);
            }

         return w;
      }

      private double[] calculateW(int i, int j, double[] dxPsi, double[] dyPsi)
      {
         int nz = getNz(i, j);
         double dz = getDz(i, j);

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
         f[nz - 1] = getFn(i, j);

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

      private double getFn(int i, int j)
      {
         IssykKulGrid2D grid = _issykKulGrid3D.Grid2D;
         GridCell cell = grid[i, j];

         double dh = cell.P(2, 2).Z - cell.P(0, 2).Z +
                     cell.P(2, 0).Z - cell.P(0, 0).Z;

         double dxDh = dh / (2 * grid.Hx);
         double dyDh = dh / (2 * grid.Hy);

         Complex t1 = _theta1[i, j][_theta0[i, j].Length - 1];
         Complex t2 = _theta1[i + 1, j][_theta0[i + 1, j].Length - 1];
         Complex t3 = _theta1[i, j + 1][_theta0[i, j + 1].Length - 1];
         Complex t4 = _theta1[i + 1, j + 1][_theta0[i + 1, j + 1].Length - 1];

         double re = (t1.Re + t2.Re + t3.Re + t4.Re) / 4;
         double im = (t1.Im + t2.Im + t3.Im + t4.Im) / 4;

         return dxDh * re + dyDh * im;
      }

      private Complex[,][] calculatePsi()
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         var psi = new Complex[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            for (int j = 0; j < ny; j++)
            {
               if (_theta0[i, j] == null)
                  continue;

               psi[i, j] = calculatePsi(i, j, _psi0[i, j]);
            }
         }

         return psi;
      }

      private Complex[] calculatePsi(int i, int j, Complex[] psi0)
      {
         int nz = psi0.Length;
         double dz = getDz(i, j);
         double nu = _parameters.Nu;

         Complex sigma = calculateSigma(j);

         Complex[] theta0 = _theta0[i, j];
         Complex[] theta1 = _theta1[i, j];

         var psi = new Complex[nz];
         psi[0] = calculatePsi1(i, j);

         for (int k = 1; k < nz - 1; k++)
         {
            psi[k] = nu * (theta1[k + 1] - theta1[k]) / (2.0 * dz) +
                     nu * (theta1[k] - theta1[k - 1]) / (2.0 * dz) +
                     (1.0 / sigma - 1.0) * (nu * (theta0[k + 1] - theta0[k]) / (2.0 * dz) +
                                            nu * (theta0[k] - theta0[k - 1]) / (2.0 * dz) -
                                            psi0[k]);
         }

         psi[nz - 1] = calculatePsiN(i, j);

         return psi;
      }

      private Complex calculateSigma(int j)
      {
         double mu = _parameters.Mu;
         double l = calculateL(j);

         double exp = Math.Exp(-mu * _tau);
         double alpha = 1.0 - Math.Cos(l * _tau) * exp;
         double beta = Math.Sin(l * _tau) * exp;

         double sqr = alpha * alpha + beta * beta;
         double a = (mu * alpha + l * beta) / sqr;
         double b = (l * alpha - mu * beta) / sqr;

         return (a + Complex.I * b - 1.0 / _tau) / (mu + Complex.I * l);
      }

      private double calculateL(int j)
      {
         double l0 = _parameters.SmallL0;
         double beta = _parameters.Beta;
         return l0 + beta * _yGrid.Get(j);
      }

      private Complex calculatePsi1(int i, int j)
      {
         double x = _xGrid.Get(i);
         double y = _yGrid.Get(j);
         double rho0 = _parameters.Rho0;
         return -(_wind.TauX(x, y) + Complex.I * _wind.TauY(x, y)) / rho0;
      }

      private Complex calculatePsiN(int i, int j)
      {
         double mu = _parameters.Mu;
         double rho0 = _parameters.Rho0;

         double u = _u[i, j];
         double v = _v[i, j];

         return -(mu * rho0 * u + Complex.I * mu * rho0 * v) / rho0;
      }
   }
}