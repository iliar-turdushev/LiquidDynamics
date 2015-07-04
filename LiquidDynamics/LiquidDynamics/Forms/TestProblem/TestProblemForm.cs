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

         _xGrid = Grid.Create(0, _problemParameters.SmallR, nx);
         _yGrid = Grid.Create(0, _problemParameters.SmallQ, ny);
         _zGrid = Grid.Create(0, _problemParameters.H, nz);

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

         printSolution(solution, errorU, errorV);
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
         var result = new Complex[_xGrid.Nodes, _yGrid.Nodes][];

         for (int i = 0; i < _xGrid.Nodes; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < _yGrid.Nodes; j++)
            {
               double y = _yGrid.Get(j);
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
         SquareGridFunction uBarotropic = solution.Barotropic.IterationResultU.Approximation;
         SquareGridFunction vBarotropic = solution.Barotropic.IterationResultV.Approximation;
         Complex[,][] baroclinic = solution.Baroclinic;

         IBarotropicComponent exactBarotropic = _solution.GetBarotropicComponent();
         IBaroclinicComponent exactBaroclinic = _solution.GetBaroclinicComponent();

         double maxU = 0;
         double maxV = 0;
         double diffU = 0;
         double diffV = 0;

         for (int i = 0; i < _xGrid.Nodes; i++)
         {
            double x = _xGrid.Get(i);

            for (int j = 0; j < _yGrid.Nodes; j++)
            {
               double y = _yGrid.Get(j);

               double u = exactBarotropic.U(_t, x, y);
               double v = exactBarotropic.V(_t, x, y);

               for (int k = 0; k < _zGrid.Nodes; k++)
               {
                  double z = _zGrid.Get(k);

                  Complex theta = exactBaroclinic.Theta(_t, x, y, z);

                  maxU = Math.Max(maxU, Math.Abs(u + theta.Re));
                  maxV = Math.Max(maxV, Math.Abs(v + theta.Im));

                  diffU = Math.Max(diffU, Math.Abs(u + theta.Re - uBarotropic[i, j] - baroclinic[i, j][k].Re));
                  diffV = Math.Max(diffV, Math.Abs(v + theta.Im - vBarotropic[i, j] - baroclinic[i, j][k].Im));
               }
            }
         }

         errorU = diffU / maxU * 100;
         errorV = diffV / maxV * 100;
      }

      private void printSolution(TestProblemSolution solution, double errorU, double errorV)
      {
         _graphControl.Caption = string.Format("Time = {0:F4}, Error U = {1:F4}%, Error V = {2:F4}%",
                                               _t, errorU, errorV);

         var hx = (float) _xGrid.Step / 2;
         var hy = (float) _yGrid.Step / 2;
         _graphControl.AxisBounds = new Bounds(-hx, (float) (_problemParameters.SmallR + hx),
                                               -hy, (float) (_problemParameters.SmallQ + hy));

         _graphControl.Clear();

         SquareVelocityField squareVelocityField = getSquareVelocityField(solution);
         _graphControl.DrawVelocityField(squareVelocityField, PaletteDrawingTools, SolutionPen);

         _graphControl.Invalidate();

         _paletteControl.MinValue = squareVelocityField.GetMinVector().Length;
         _paletteControl.MaxValue = squareVelocityField.GetMaxVector().Length;
         _paletteControl.Invalidate();
      }

      private SquareVelocityField getSquareVelocityField(TestProblemSolution solution)
      {
         Mathematics.MathTypes.Vector[,] barotropicVectors =
            solution.Barotropic.BarotropicComponent.Vectors;

         var vectors = new Vector[_xGrid.Nodes, _yGrid.Nodes];

         for (int i = 0; i < _xGrid.Nodes; i++)
         {
            var x = (float) _xGrid.Get(i);

            for (int j = 0; j < _yGrid.Nodes; j++)
            {
               var y = (float) _yGrid.Get(j);

               Complex theta = solution.Baroclinic[i, j][_zSlice];
               Point barotropic = barotropicVectors[i, j].End;

               var endPoint = new PointF((float) (barotropic.X + theta.Re),
                                         (float) (barotropic.Y + theta.Im));
               vectors[i, j] = new Vector(new PointF(x, y), endPoint);
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

         printSolution(solution, errorU, errorV);
      }

      private void setButtonsAccessibility(bool enabled)
      {
         _buttonReset.Enabled = enabled;
         _buttonStep.Enabled = enabled;
      }
   }
}
