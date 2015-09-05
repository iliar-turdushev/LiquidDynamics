using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using Common;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using LiquidDynamics.Properties;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;
using ModelProblem.Vertical;
using Point = Mathematics.MathTypes.Point;
using Point3D = Mathematics.MathTypes.Point3D;
using Rectangle3D = Mathematics.MathTypes.Rectangle3D;
using Vector = ControlLibrary.Types.Vector;

namespace LiquidDynamics.Forms.TestProblem
{
   public partial class TestProblemForm : Form
   {
      private const double Sigma = 0.0001;
      private const double Delta = 100000;
      private const int K = 100000;

      private static readonly IPaletteDrawingTools PaletteDrawingTools = PaletteFactory.CreateBluePalette();
      private static readonly Pen SolutionPen = new Pen(Color.Red, 1) {EndCap = LineCap.ArrowAnchor};
     
      private readonly Parameters _problemParameters;
      private readonly Solution _solution;

      private TestProblemSolver _solver;

      private Grid _xGrid;
      private Grid _yGrid;
      private Grid _zGrid;

      private double _t;
      private double _tau;

      private int _zSlice;

      private bool _dynamicsAlive;
      
      public TestProblemForm(Parameters problemParameters)
      {
         Check.NotNull(problemParameters, "problemParameters");
         _problemParameters = problemParameters;
         
         _solution = SolutionCreator.Create(_problemParameters);

         InitializeComponent();

         _paletteControl.PaletteDrawingTools = PaletteDrawingTools;
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         _buttonStartStop.Enabled = true;
         _buttonStep.Enabled = true;

         int nx = int.Parse(_textBoxNx.Text);
         int ny = int.Parse(_textBoxNy.Text);
         int nz = int.Parse(_textBoxNz.Text);

         _tau = double.Parse(_textBoxTau.Text);
         _zSlice = int.Parse(_textBoxZSlice.Text);

         _t = 0;

         _xGrid = Grid.Create(0, _problemParameters.SmallR, nx + 1);
         _yGrid = Grid.Create(0, _problemParameters.SmallQ, ny + 1);
         _zGrid = Grid.Create(0, _problemParameters.H, nz + 1);

         _solver = new TestProblemSolver(
            createProblemParameters(),
            createIssykKulGrid3D(),
            createWind(),
            _tau,
            _xGrid,
            _yGrid,
            createInitialCondition(),
            createInitialTheta(),
            Sigma,
            Delta,
            K,
            getDesk()
            );

         TestProblemSolution solution = _solver.Begin();
         _t += _tau;

         double errorU, errorV;
         getError(solution, out errorU, out errorV);

         double errorW = calculateError(solution.W);

         printSolution(solution, errorU, errorV, errorW);
      }

      private void buttonStepClick(object sender, EventArgs e)
      {
         step();
      }

      private void buttonStartStopClick(object sender, EventArgs e)
      {
         _dynamicsAlive = !_dynamicsAlive;
         _timer.Enabled = _dynamicsAlive;

         if (_dynamicsAlive)
         {
            _buttonStartStop.Image = Resources.Pause;
            setButtonsAccessibility(false);
         }
         else
         {
            _buttonStartStop.Image = Resources.Start;
            setButtonsAccessibility(true);
         }
      }
      
      private void timerTick(object sender, EventArgs e)
      {
         step();
      }

      private ProblemParameters createProblemParameters()
      {
         return new ProblemParameters
                   {
                      Beta = _problemParameters.Beta,
                      F1 = _problemParameters.F1,
                      F2 = _problemParameters.F2,
                      H = _problemParameters.H,
                      Mu = _problemParameters.Mu,
                      Nu = _problemParameters.Nu,
                      Rho0 = _problemParameters.Rho0,
                      SmallL0 = _problemParameters.SmallL0,
                      SmallQ = _problemParameters.SmallQ,
                      SmallR = _problemParameters.SmallR
                   };
      }

      private IssykKulGrid3D createIssykKulGrid3D()
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
                  Point3D point3D = grid2D[i, j].P(0, 0);
                  point3D.Z = _zGrid.Get(k);
                  depthGrid[i, j][k] = new Rectangle3D(point3D, grid2D.Hx, grid2D.Hy, _zGrid.Step);
               }
            }
         }

         return new IssykKulGrid3D(grid2D, depthGrid);
      }

      private IssykKulGrid2D createGrid()
      {
         int n = _xGrid.Nodes;
         int m = _yGrid.Nodes;

         double hx = _xGrid.Step;
         double hy = _yGrid.Step;

         double z = _problemParameters.H;

         var cells = new GridCell[n - 1, m - 1];

         for (int i = 0; i < n - 1; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < m - 1; j++)
            {
               double y = _yGrid.Get(j);
               cells[i, j] = new GridCell(x, y, hx, hy, z);
            }
         }

         return new IssykKulGrid2D(cells, hx, hy);
      }

      private IWind createWind()
      {
         return new ModelWind(_problemParameters.F1,
                              _problemParameters.F2,
                              _problemParameters.SmallQ,
                              _problemParameters.SmallR,
                              _problemParameters.Rho0);
      }

      private InitialCondition createInitialCondition()
      {
         IBarotropicComponent barotropicComponent = _solution.GetBarotropicComponent();

         int n = _xGrid.Nodes;
         int m = _yGrid.Nodes;

         var u = new double[n, m];
         var v = new double[n, m];

         for (int i = 0; i < n; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < m; j++)
            {
               double y = _yGrid.Get(j);

               u[i, j] = barotropicComponent.U(_t, x, y) * _problemParameters.H;
               v[i, j] = barotropicComponent.V(_t, x, y) * _problemParameters.H;
            }
         }

         return new InitialCondition(new SquareGridFunction(_xGrid, _yGrid, u),
                                     new SquareGridFunction(_xGrid, _yGrid, v));
      }

      private Complex[,][] createInitialTheta()
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();
         var result = new Complex[_xGrid.Nodes - 1, _yGrid.Nodes - 1][];

         for (int i = 0; i < _xGrid.Nodes - 1; i++)
         {
            double x = _xGrid.Get(i) + 0.5 * _xGrid.Step;

            for (int j = 0; j < _yGrid.Nodes - 1; j++)
            {
               double y = _yGrid.Get(j) + 0.5 * _yGrid.Step;
               result[i, j] = new Complex[_zGrid.Nodes];

               for (int k = 0; k < _zGrid.Nodes; k++)
               {
                  result[i, j][k] = baroclinic.Theta(_t, x, y, _zGrid.Get(k));
               }
            }
         }

         return result;
      }

      private int[,] getDesk()
      {
         int n = _xGrid.Nodes;
         int m = _yGrid.Nodes;

         var desk = new int[n - 1, m - 1];

         for (int i = 0; i < desk.GetLength(0); i++)
         {
            for (int j = 0; j < desk.GetLength(1); j++)
               desk[i, j] = 1;
         }

         return desk;
      }

      private void getError(TestProblemSolution solution, out double errorU, out double errorV)
      {
         SquareGridFunction uBarotropic = solution.BarotropicU;
         SquareGridFunction vBarotropic = solution.BarotropicV;
         Complex[,][] baroclinic = solution.Baroclinic;

         int n = uBarotropic.N;
         int m = uBarotropic.M;

         IBarotropicComponent exactBarotropic = _solution.GetBarotropicComponent();
         IBaroclinicComponent exactBaroclinic = _solution.GetBaroclinicComponent();

         double h = _problemParameters.H;

         double maxU = 0;
         double maxV = 0;
         double diffU = 0;
         double diffV = 0;

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               Point point = uBarotropic.Grid(i, j);
               double x = point.X;
               double y = point.Y;

               double u = exactBarotropic.U(_t, x, y);
               double v = exactBarotropic.V(_t, x, y);

               for (int k = 0; k < _zGrid.Nodes; k++)
               {
                  double z = _zGrid.Get(k);

                  Complex theta = exactBaroclinic.Theta(_t, x, y, z);

                  maxU = Math.Max(maxU, Math.Abs(u + theta.Re));
                  maxV = Math.Max(maxV, Math.Abs(v + theta.Im));

                  diffU = Math.Max(diffU, Math.Abs(u + theta.Re - uBarotropic[i, j] / h - baroclinic[i, j][k].Re));
                  diffV = Math.Max(diffV, Math.Abs(v + theta.Im - vBarotropic[i, j] / h - baroclinic[i, j][k].Im));
               }
            }
         }

         errorU = diffU / maxU * 100;
         errorV = diffV / maxV * 100;
      }

      private void printSolution(TestProblemSolution solution, double errorU, double errorV, double errorW)
      {
         _graphControl.Caption = string.Format("Time = {0:F4}, Error U = {1:F4}%, Error V = {2:F4}%, Error W = {3:F4}%",
                                               _t, errorU, errorV, errorW);

         _graphControl.AxisBounds = new Bounds(0, (float) _problemParameters.SmallR,
                                               0, (float) _problemParameters.SmallQ);

         _graphControl.Clear();

//         SquareVelocityField squareVelocityField = getSquareVelocityField(solution);
//         _graphControl.DrawVelocityField(squareVelocityField, PaletteDrawingTools, SolutionPen);
         UpwellingData upwelling = buildUpwellingData(solution.W)[_zSlice];
         _graphControl.DrawUpwelling(upwelling, PaletteFactory.CreateBlueRedPalette());

         _graphControl.Invalidate();

//         _paletteControl.MinValue = squareVelocityField.GetMinVector().Length;
//         _paletteControl.MaxValue = squareVelocityField.GetMaxVector().Length;
         float value = Math.Max(Math.Abs(upwelling.GetMinIntensity()),
                                Math.Abs(upwelling.GetMaxIntensity()));
         _paletteControl.MinValue = -value;
         _paletteControl.MaxValue = value;
         _paletteControl.Invalidate();
      }

      private SquareVelocityField getSquareVelocityField(TestProblemSolution solution)
      {
         Mathematics.MathTypes.Vector[,] barotropicVectors = solution.Barotropic;

         int n = barotropicVectors.GetLength(0);
         int m = barotropicVectors.GetLength(1);

         double h = _problemParameters.H;

         var vectors = new Vector[n, m];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               var startPoint = new PointF((float) barotropicVectors[i, j].Start.X,
                                           (float) barotropicVectors[i, j].Start.Y);

               Complex theta = solution.Baroclinic[i, j][_zSlice];
               Point barotropic = barotropicVectors[i, j].End;

               var endPoint = new PointF((float) (barotropic.X / h + theta.Re),
                                         (float) (barotropic.Y / h + theta.Im));
               vectors[i, j] = new Vector(startPoint, endPoint);
            }
         }

         var cellSize = new SizeF((float) _xGrid.Step, (float) _yGrid.Step);
         return new SquareVelocityField(vectors, cellSize);
      }

      private void step()
      {
         TestProblemSolution solution = _solver.Step();
         _t += _tau;

         double errorU, errorV;
         getError(solution, out errorU, out errorV);

         double errorW = calculateError(solution.W);

         printSolution(solution, errorU, errorV, errorW);
      }

      private void setButtonsAccessibility(bool enabled)
      {
         _buttonReset.Enabled = enabled;
         _buttonStep.Enabled = enabled;
      }

      private double[,][] calculateExactW()
      {
         int nx = _xGrid.Nodes - 1;
         int ny = _yGrid.Nodes - 1;
         int nz = _zGrid.Nodes;

         IVerticalComponent vertical = _solution.GetVerticalComponent();

         var w = new double[nx, ny][];

         for (int i = 0; i < nx; i++)
         {
            double x = 0.5 * (_xGrid.Get(i) + _xGrid.Get(i + 1));

            for (int j = 0; j < ny; j++)
            {
               double y = 0.5 * (_yGrid.Get(j) + _yGrid.Get(j + 1));
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

      private double calculateError(double[,][] w)
      {
         double[,][] exactW = calculateExactW();

         int nx = _xGrid.Nodes - 1;
         int ny = _yGrid.Nodes - 1;
         int nz = _zGrid.Nodes;

         double max = 0.0;
         double maxDiff = 0.0;

         for (int i = 0; i < nx; i++)
            for (int j = 0; j < ny; j++)
               for (int k = 0; k < nz; k++)
               {
                  max = Math.Max(max, Math.Abs(exactW[i, j][k]));
                  maxDiff = Math.Max(maxDiff, Math.Abs(exactW[i, j][k] - w[i, j][k]));
               }

         return maxDiff / max * 100.0;
      }

      private UpwellingData[] buildUpwellingData(double[,][] w)
      {
         int nz = _zGrid.Nodes;
         var result = new UpwellingData[nz];

         for (int k = 0; k < nz; k++)
            result[k] = buildUpwellingData(k, w);

         return result;
      }

      private UpwellingData buildUpwellingData(int slice, double[,][] w)
      {
         int nx = _xGrid.Nodes;
         int ny = _yGrid.Nodes;

         var gridPoints = new PointF[nx - 1, ny - 1];
         var intensities = new float[nx - 1, ny - 1];

         for (var i = 0; i < nx - 1; i++)
         {
            double x = 0.5 * (_xGrid.Get(i) + _xGrid.Get(i + 1));

            for (var j = 0; j < ny - 1; j++)
            {
               double y = 0.5 * (_yGrid.Get(j) + _yGrid.Get(j + 1));
               gridPoints[i, j] = new PointF((float) x, (float) y);
               intensities[i, j] = (float) (_zGrid.Get(slice) - w[i, j][slice]);
            }
         }

         var cellSize = new SizeF((float) _xGrid.Step, (float) _yGrid.Step);
         return new UpwellingData(gridPoints, intensities, cellSize);
      }
   }
}
