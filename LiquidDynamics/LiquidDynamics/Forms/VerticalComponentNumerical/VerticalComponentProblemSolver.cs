using System;
using System.Drawing;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using ControlLibrary.Types;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;
using ModelProblem.Vertical;
using Point3D = Mathematics.MathTypes.Point3D;
using Rectangle3D = Mathematics.MathTypes.Rectangle3D;

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
      
      private double[,][] _w;
      private double[,][] _exactW;

      private VerticalProblemSolver _solver;

      internal VerticalComponentErrorContainer Errors { get; private set; }

      public VerticalComponentResult Begin(int nx, int ny, int nz, double tau, Parameters parameters)
      {
         Errors = new VerticalComponentErrorContainer();
         Errors.AddError(0, 0);

         _xGrid = Grid.Create(0, parameters.SmallR, nx + 1);
         _yGrid = Grid.Create(0, parameters.SmallQ, ny + 1);
         _zGrid = Grid.Create(0, parameters.H, nz + 1);

         _t = tau;
         _tau = tau;

         _parameters = parameters;

         _wind = new ModelWind(_parameters.F1, _parameters.F2,
                               _parameters.SmallQ, _parameters.SmallR,
                               _parameters.Rho0);
         _solution = SolutionCreator.Create(_parameters);

         _solver = new VerticalProblemSolver(_xGrid, _yGrid, _tau, _wind, createProblemParameters(),
                                             calculateTheta(_t), createGrid3D());

         _w = _solver.Begin();
         _exactW = calculateExactW();

         double error = calculateError();
         Errors.AddError(_t, error);

         return new VerticalComponentResult(buildUpwellingData(), error, _t);
      }

      private IssykKulGrid3D createGrid3D()
      {
         IssykKulGrid2D grid2D = createGrid();
         var depthGrid = new Rectangle3D[grid2D.N, grid2D.M][];

         for (int i = 0; i < grid2D.N; i++)
         {
            for (int j = 0; j < grid2D.M; j++)
            {
               depthGrid[i, j] = new Rectangle3D[_zGrid.Nodes - 1];

               for (int k = 0; k < _zGrid.Nodes - 1; k++)
               {
                  Point3D p = grid2D[i, j].P(0, 0);
                  var point3D = new Point3D(p.X, p.Y, _zGrid.Get(k));
                  depthGrid[i, j][k] = new Rectangle3D(point3D, grid2D.Hx, grid2D.Hy, _zGrid.Step);
               }
            }
         }

         return new IssykKulGrid3D(grid2D, depthGrid);
      }

      private IssykKulGrid2D createGrid()
      {
         int n = _xGrid.Nodes - 1;
         int m = _yGrid.Nodes - 1;

         double hx = _xGrid.Step;
         double hy = _yGrid.Step;

         double z = _parameters.H;

         var cells = new GridCell[n, m];

         for (int i = 0; i < n; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < m; j++)
            {
               double y = _yGrid.Get(j);
               cells[i, j] = new GridCell(x, y, hx, hy, z);
            }
         }

         return new IssykKulGrid2D(cells, hx, hy);
      }

      public VerticalComponentResult Step()
      {
         _t += _tau;
         
         _w = _solver.Step(calculateTheta(_t), u(_t), v(_t));
         _exactW = calculateExactW();

         double error = calculateError();
         Errors.AddError(_t, error);

         return new VerticalComponentResult(buildUpwellingData(), error, _t);
      }

      private ProblemParameters createProblemParameters()
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

      private double[,] u(double t)
      {
         int n = _xGrid.Nodes;
         int m = _yGrid.Nodes;

         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();

         var result = new double[n, m];

         for (int i = 0; i < n; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < m; j++)
            {
               result[i, j] = barotropic.U(t, x, _yGrid.Get(j));
            }
         }

         return result;
      }

      private double[,] v(double t)
      {
         int n = _xGrid.Nodes;
         int m = _yGrid.Nodes;

         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();

         var result = new double[n, m];

         for (int i = 0; i < n; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < m; j++)
            {
               result[i, j] = barotropic.V(t, x, _yGrid.Get(j));
            }
         }

         return result;
      }

      private Complex[,][] calculateTheta(double t)
      {
         int n = _xGrid.Nodes;
         int m = _yGrid.Nodes;
         int l = _zGrid.Nodes; 

         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();

         var result = new Complex[n, m][];

         for (int i = 0; i < n; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < m; j++)
            {
               double y = _yGrid.Get(j);
               double u = barotropic.U(t, x, y);
               double v = barotropic.V(t, x, y);

               result[i, j] = new Complex[l];

               for (int k = 0; k < l; k++)
               {
                  double z = _zGrid.Get(k);
                  Complex c = baroclinic.Theta(t, x, y, z);
                  result[i, j][k] = new Complex(c.Re + u, c.Im + v);
               }
            }
         }

         return result;
      }

      private double[,][] calculateExactW()
      {
         int nx = _xGrid.Nodes - 1;
         int ny = _yGrid.Nodes - 1;
         int nz = _zGrid.Nodes;

         double halfHx = _xGrid.Step / 2;
         double halfHy = _yGrid.Step / 2;

         IVerticalComponent vertical = _solution.GetVerticalComponent();

         var w = new double[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            double x = _xGrid.Get(i) + halfHx;

            for (int j = 0; j < ny; j++)
            {
               double y = _yGrid.Get(j) + halfHy;
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
         int nx = _xGrid.Nodes - 1;
         int ny = _yGrid.Nodes - 1;
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
         int nx = _xGrid.Nodes - 1;
         int ny = _yGrid.Nodes - 1;

         var gridPoints = new PointF[nx, ny];
         var intensities = new float[nx, ny];

         for (var i = 0; i < nx; i++)
         {
            double x = 0.5 * (_xGrid.Get(i) + _xGrid.Get(i + 1));

            for (var j = 0; j < ny; j++)
            {
               double y = 0.5 * (_yGrid.Get(j) + _yGrid.Get(j + 1));
               gridPoints[i, j] = new PointF((float) x, (float) y);
               intensities[i, j] = (float) _w[i, j][slice];
            }
         }

         var cellSize = new SizeF((float) _xGrid.Step, (float) _yGrid.Step);
         return new UpwellingData(gridPoints, intensities, cellSize);
      }
   }
}