using System;
using System.Diagnostics;
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
      private readonly Grid _zGrid;

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
         int nx = _issykKulGrid3D.Grid2D.N;
         int ny = _issykKulGrid3D.Grid2D.M;

         var psi = new Complex[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            for (int j = 0; j < ny; j++)
            {
               if (_issykKulGrid3D.Grid2D[i, j] == GridCell.Empty)
                  continue;

               psi[i, j] = calculateInitialPsi(i, j);
            }
         }

         return psi;
      }

      private int getNz(int i, int j)
      {
         Rectangle3D[] depthGrid = _issykKulGrid3D.GetDepthGrid(i, j);
         return depthGrid.Length + 1;
      }

      private double getDz(int i, int j)
      {
         Rectangle3D[] depthGrid = _issykKulGrid3D.GetDepthGrid(i, j);
         return depthGrid[0].Hz;
      }

      private Complex[] calculateInitialPsi(int i, int j)
      {
         int nz = getNz(i, j);
         double dz = getDz(i, j);
         double nu = _parameters.Nu;

         Complex[] theta = _theta0[i, j];
         Debug.Assert(nz == _theta0[i, j].Length, "1");

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

               if (i != 0 &&
                   i != nx - 1 &&
                   grid[i - 1, j] != GridCell.Empty &&
                   grid[i + 1, j] != GridCell.Empty)
               {
                  dxRePsi[i, j] = calculateDxRePsi(2.0 * dx, nz, _psi[i - 1, j], _psi[i + 1, j]);
               }
               else if (i != 0 &&
                        i != nx - 1 &&
                        grid[i - 1, j] == GridCell.Empty &&
                        grid[i + 1, j] == GridCell.Empty)
               {
                  dxRePsi[i, j] = new double[nz];
               }
               else if (i == 0 || grid[i - 1, j] == GridCell.Empty)
               {
                  Debug.Assert(grid[i + 1, j] != GridCell.Empty, "2");
                  dxRePsi[i, j] = calculateDxRePsi(dx, nz, _psi[i, j], _psi[i + 1, j]);
               }
               else if (i == nx - 1 || grid[i + 1, j] == GridCell.Empty)
               {
                  Debug.Assert(grid[i - 1, j] != GridCell.Empty, "3");
                  dxRePsi[i, j] = calculateDxRePsi(dx, nz, _psi[i - 1, j], _psi[i, j]);
               }
               else
                  Debug.Fail("dx");
            }
         }

         return dxRePsi;
      }

      private double[] calculateDxRePsi(double dx, int nz, Complex[] psi0, Complex[] psi1)
      {
         var dxRe = new double[nz];

         for (int k = 0; k < nz; k++)
         {
            double re0 = psi0.Length > k ? psi0[k].Re : 0;
            double re1 = psi1.Length > k ? psi1[k].Re : 0;
            
            dxRe[k] = (re1 - re0) / dx;
         }

         return dxRe;
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

               if (j != 0 &&
                   j != ny - 1 &&
                   grid[i, j - 1] != GridCell.Empty &&
                   grid[i, j + 1] != GridCell.Empty)
               {
                  dyRePsi[i, j] = calculateDyImPsi(2.0 * dy, nz, _psi[i, j - 1], _psi[i, j + 1]);
               }
               else if (j != 0 &&
                        j != ny - 1 &&
                        grid[i, j - 1] == GridCell.Empty &&
                        grid[i, j + 1] == GridCell.Empty)
               {
                  dyRePsi[i, j] = new double[nz];
               }
               else if (j == 0 || grid[i, j - 1] == GridCell.Empty)
               {
                  Debug.Assert(grid[i, j + 1] != GridCell.Empty, "4");
                  dyRePsi[i, j] = calculateDyImPsi(dy, nz, _psi[i, j], _psi[i, j + 1]);
               }
               else if (j == ny - 1 || grid[i, j + 1] == GridCell.Empty)
               {
                  Debug.Assert(grid[i, j - 1] != GridCell.Empty, "5");
                  dyRePsi[i, j] = calculateDyImPsi(dy, nz, _psi[i, j - 1], _psi[i, j]);
               }
               else
                  Debug.Fail("dy");
            }
         }

         return dyRePsi;
      }

      private double[] calculateDyImPsi(double dy, int nz, Complex[] psi0, Complex[] psi1)
      {
         var dyIm = new double[nz];

         for (int k = 0; k < nz; k++)
         {
            double im0 = psi0.Length > k ? psi0[k].Im : 0;
            double im1 = psi1.Length > k ? psi1[k].Im : 0;

            dyIm[k] = (im1 - im0) / dy;
         }

         return dyIm;
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

         int nx = grid.N;
         int ny = grid.M;
         int nz = getNz(i, j);

         double dx = grid.Hx;
         double dy = grid.Hy;
         double dz = getDz(i, j);

         double dxDh = 0;
         double dyDh = 0;

         if (i != 0 &&
             i != nx - 1 &&
             grid[i - 1, j] != GridCell.Empty &&
             grid[i + 1, j] != GridCell.Empty)
         {
            double h0 = getNz(i - 1, j) * dz;
            double h1 = getNz(i + 1, j) * dz;
            dxDh = (h1 - h0) / (2 * dx);
         }
         else if (i != 0 &&
                  i != nx - 1 &&
                  grid[i - 1, j] == GridCell.Empty &&
                  grid[i + 1, j] == GridCell.Empty)
         {
            dxDh = 0;
         }
         else if (i == 0 || grid[i - 1, j] == GridCell.Empty)
         {
            double h0 = getNz(i, j) * dz;
            double h1 = getNz(i + 1, j) * dz;
            dxDh = (h1 - h0) / dx;
         }
         else if (i == nx - 1 || grid[i + 1, j] == GridCell.Empty)
         {
            double h0 = getNz(i - 1, j) * dz;
            double h1 = getNz(i, j) * dz;
            dxDh = (h1 - h0) / dx;
         }
         else
            Debug.Fail("dxDh");

         if (j != 0 &&
             j != ny - 1 &&
             grid[i, j - 1] != GridCell.Empty &&
             grid[i, j + 1] != GridCell.Empty)
         {
            double h0 = getNz(i, j - 1) * dz;
            double h1 = getNz(i, j + 1) * dz;
            dyDh = (h1 - h0) / (2 * dy);
         }
         else if (j != 0 &&
                  j != ny - 1 &&
                  grid[i, j - 1] == GridCell.Empty &&
                  grid[i, j + 1] == GridCell.Empty)
         {
            dyDh = 0;
         }
         else if (j == 0 || grid[i, j - 1] == GridCell.Empty)
         {
            double h0 = getNz(i, j) * dz;
            double h1 = getNz(i, j + 1) * dz;
            dyDh = (h1 - h0) / dy;
         }
         else if (j == ny - 1 || grid[i, j + 1] == GridCell.Empty)
         {
            double h0 = getNz(i, j - 1) * dz;
            double h1 = getNz(i, j) * dz;
            dyDh = (h1 - h0) / dy;
         }
         else
            Debug.Fail("dyDh");

         Complex c = _theta1[i, j][nz - 1];
         return dxDh * c.Re + dyDh * c.Im;
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
               psi[i, j] = calculatePsi(i, j, _psi0[i, j]);
            }
         }

         return psi;
      }

      private Complex[] calculatePsi(int i, int j, Complex[] psi0)
      {
         int nz = _zGrid.Nodes;
         double dz = _zGrid.Step;
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