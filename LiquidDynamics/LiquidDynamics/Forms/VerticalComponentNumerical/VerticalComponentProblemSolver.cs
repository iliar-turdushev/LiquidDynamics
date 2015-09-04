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
      
      private double[,][] _w;
      private double[,][] _exactW;

      private VerticalProblemSolver _solver;

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

         _solver = new VerticalProblemSolver(_xGrid, _yGrid, _zGrid, _tau, _wind, _parameters, calculateTheta(_t));

         _w = _solver.Begin();
         _exactW = calculateExactW();

         return new VerticalComponentResult(buildUpwellingData(), calculateError(), _t);
      }

      public VerticalComponentResult Step()
      {
         _t += _tau;
         
         _w = _solver.Step(calculateTheta(_t), u(_t), v(_t));
         _exactW = calculateExactW();

         return new VerticalComponentResult(buildUpwellingData(), calculateError(), _t);
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

      private Grid createGrid(int cells, double length)
      {
         double halfCellLength = length / (2.0 * cells);
         return Grid.Create(halfCellLength, length - halfCellLength, cells);
      }

      private static Grid createZGrid(int cells, double depth)
      {
         return Grid.Create(0.0, depth, cells + 1);
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
   }
}