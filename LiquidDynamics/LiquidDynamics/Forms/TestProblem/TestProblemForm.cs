using System;
using System.Windows.Forms;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using Common;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;

namespace LiquidDynamics.Forms.TestProblem
{
   public partial class TestProblemForm : Form
   {
      private readonly Parameters _problemParameters;

      private int _n = 20;
      private int _m = 20;
      private int _l = 20;

      private double _tau = 0.1;

      private double _t = 0;

      private double _sigma = 0.0001;
      private double _delta = 100000;
      private int _k = 100000;

      private Solution _solution;

      public TestProblemForm(Parameters problemParameters)
      {
         Check.NotNull(problemParameters, "problemParameters");
         _problemParameters = problemParameters;
         
         _solution = SolutionCreator.Create(_problemParameters);

         InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         Grid xGrid = Grid.Create(0, _problemParameters.SmallR, _n);
         Grid yGrid = Grid.Create(0, _problemParameters.SmallQ, _m);
         Grid zGrid = Grid.Create(0, _problemParameters.H, _l);

         var solver = new TestProblemSolver(
            createProblemParameters(),
            createIssykKulGrid3D(_l),
            createWind(),
            _tau,
            xGrid,
            yGrid,
            createInitialCondition(),
            createInitialTheta(xGrid, yGrid, zGrid),
            _sigma,
            _delta,
            _k,
            getDesk(_n, _m)
            );
         TestProblemSolution solution = solver.Begin();

         var u = solution.Barotropic.IterationResultU.Approximation;
         var v = solution.Barotropic.IterationResultV.Approximation;
         var B = solution.Baroclinic;

         double maxU = 0;
         double maxV = 0;
         double diffU = 0;
         double diffV = 0;

         IBarotropicComponent t1 = _solution.GetBarotropicComponent();
         IBaroclinicComponent t2 = _solution.GetBaroclinicComponent();

         for (int i = 0; i < _n; i++)
         {
            for (int j = 0; j < _m; j++)
            {
               double u1 = t1.U(_t + _tau, xGrid.Get(i), yGrid.Get(j));
               double v1 = t1.V(_t + _tau, xGrid.Get(i), yGrid.Get(j));

               for (int k = 0; k < _l; k++)
               {
                  Complex th2 = t2.Theta(_t + _tau, xGrid.Get(i), yGrid.Get(j), zGrid.Get(k));

                  maxU = Math.Max(maxU, Math.Abs(u1 + th2.Re));
                  maxV = Math.Max(maxV, Math.Abs(v1 + th2.Im));

                  diffU = Math.Max(diffU, Math.Abs(u1 + th2.Re - u[i, j] - B[i, j][k].Re));
                  diffV = Math.Max(diffV, Math.Abs(v1 + th2.Im - v[i, j] - B[i, j][k].Im));
               }
            }
         }

         textBox1.Text = string.Format("{0}; {1}", diffU / maxU * 100, diffV / maxV * 100);
      }

      private Complex[,][] createInitialTheta(Grid xGrid, Grid yGrid, Grid zGrid)
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();
         var result = new Complex[xGrid.Nodes, yGrid.Nodes][];

         for (int i = 0; i < xGrid.Nodes; i++)
         {
            for (int j = 0; j < yGrid.Nodes; j++)
            {
               result[i, j] = new Complex[zGrid.Nodes];

               for (int k = 0; k < zGrid.Nodes; k++)
               {
                  result[i, j][k] = baroclinic.Theta(_t, xGrid.Get(i), yGrid.Get(j), zGrid.Get(k));
               }
            }
         }

         return result;
      }

      private IssykKulGrid3D createIssykKulGrid3D(int nodes)
      {
         IssykKulGrid2D grid2D = createGrid();
         var depthGrid = new Rectangle3D[grid2D.N, grid2D.M][];
         double dz = _problemParameters.H / (nodes - 1);

         for (int i = 0; i < grid2D.N; i++)
         {
            for (int j = 0; j < grid2D.M; j++)
            {
               depthGrid[i, j] = new Rectangle3D[nodes - 1];
               
               for (int k = 0; k < nodes - 1; k++)
               {
                  Point3D point3D = grid2D[i, j].P(0, 0);
                  point3D.Z = dz * k;
                  depthGrid[i, j][k] = new Rectangle3D(point3D, grid2D.Hx, grid2D.Hy, dz);
               }
            }
         }

         return new IssykKulGrid3D(grid2D, depthGrid);
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

      private IssykKulGrid2D createGrid()
      {
         Grid xGrid = Grid.Create(0, _problemParameters.SmallR, _n);
         Grid yGrid = Grid.Create(0, _problemParameters.SmallQ, _m);

         int n = xGrid.Nodes;
         int m = yGrid.Nodes;

         double hx = xGrid.Step;
         double hy = yGrid.Step;

         double z = _problemParameters.H;

         var cells = new GridCell[n - 1,m - 1];

         for (int i = 0; i < n - 1; i++)
         {
            double x = xGrid.Get(i);

            for (int j = 0; j < m - 1; j++)
            {
               double y = yGrid.Get(j);
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

         Grid xGrid = Grid.Create(0, _problemParameters.SmallR, _n);
         Grid yGrid = Grid.Create(0, _problemParameters.SmallQ, _m);

         var n = xGrid.Nodes;
         var m = yGrid.Nodes;

         var u = new double[n, m];
         var v = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = xGrid.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = yGrid.Get(j);

               u[i, j] = barotropicComponent.U(_t, x, y) * _problemParameters.H;
               v[i, j] = barotropicComponent.V(_t, x, y) * _problemParameters.H;
            }
         }

         return new InitialCondition(new SquareGridFunction(xGrid, yGrid, u),
                                     new SquareGridFunction(xGrid, yGrid, v));
      }

      private static int[,] getDesk(int n, int m)
      {
         var desk = new int[n - 1, m - 1];

         for (int i = 0; i < desk.GetLength(0); i++)
         {
            for (int j = 0; j < desk.GetLength(1); j++)
               desk[i, j] = 1;
         }

         return desk;
      }
   }
}
