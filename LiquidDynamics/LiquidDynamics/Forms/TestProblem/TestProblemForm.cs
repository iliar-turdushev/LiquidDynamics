using System;
using System.Linq;
using System.Windows.Forms;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IssykKulGrid;
using BarotropicComponentProblem.IterationMethod;
using BarotropicComponentProblem.TestProblem;
using Common;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;
using GridParameters = BarotropicComponentProblem.GridParameters;

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
         InitialCondition initialCondition = createInitialCondition();

         var solver =
            new BarotropicComponentProblemSolver(
               createDynamicProblem(),
               createGridParameters(),
               initialCondition,
               new IterationProcessParameters(_sigma, _delta, _k, initialCondition.U),
               new IterationProcessParameters(_sigma, _delta, _k, initialCondition.V),
               getDesk(_n, _m)
               );

         IterationMethodResult result = solver.Begin();
         SquareGridFunction u = result.IterationResultU.Approximation;
         SquareGridFunction v = result.IterationResultV.Approximation;

         Grid xGrid = Grid.Create(0, _problemParameters.SmallR, _n);
         Grid yGrid = Grid.Create(0, _problemParameters.SmallQ, _m);
         Grid zGrid = Grid.Create(0, _problemParameters.H, _l);

         Complex[,][] B = new Complex[_n,_m][]; 

         for (int i = 0; i < _n; i++)
         {
            for (int j = 0; j < _m; j++)
            {
               BaroclinicProblemSolver solver2 =
                  createBaroclinicProblemSolver(xGrid.Get(i), yGrid.Get(j), u[i, j], v[i, j]);
               B[i, j] = calculateBaroclinic(solver2.Solve());
            }
         }

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

         Text = string.Format("{0}; {1}", diffU / maxU * 100, diffV / maxV * 100);
      }

      private Complex[] calculateBaroclinic(Complex[] theta)
      {
         Complex barotropic = calculateBarotropic(theta);
         var baroclinic = new Complex[theta.Length];

         for (int i = 0; i < baroclinic.Length; i++)
            baroclinic[i] = theta[i] - barotropic;

         return baroclinic.ToArray();
      }

      private Complex calculateBarotropic(Complex[] theta)
      {
         Grid zGrid = Grid.Create(0, _problemParameters.H, _l);
         Complex barotropic = 0;

         for (int i = 0; i < theta.Length - 1; i++)
            barotropic += theta[i + 1] + theta[i];

         return barotropic * zGrid.Step / (2.0 * _problemParameters.H);
      }

      private IDynamicProblem createDynamicProblem()
      {
         return new IntegroInterpolatingScheme(createProblemParameters(),
                                               createGrid(),
                                               createWind(),
                                               _tau);
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

      private GridParameters createGridParameters()
      {
         Grid xGrid = Grid.Create(0, _problemParameters.SmallR, _n);
         Grid yGrid = Grid.Create(0, _problemParameters.SmallQ, _m);
         return new GridParameters(_tau, xGrid, yGrid);
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

      private BaroclinicProblemSolver createBaroclinicProblemSolver(
         double x, double y, double u, double v
         )
      {
         return new BaroclinicProblemSolver(
            createProblemParameters(),
            createTheta0(x, y),
            _tau, _problemParameters.H / (_l - 1), _l,
            x, y, getTauX(x, y), getTauY(x, y),
            getTauXb(u), getTauYb(v)
            );
      }

      private Complex[] createTheta0(double x, double y)
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         Grid z = Grid.Create(0, _problemParameters.H, _l);

         double u = barotropic.U(_t, x, y);
         double v = barotropic.V(_t, x, y);

         var result = new Complex[z.Nodes];

         for (int i = 0; i < _l; i++)
         {
            Complex theta = baroclinic.Theta(_t, x, y, z.Get(i));
            result[i] = new Complex(u + theta.Re, v + theta.Im);
         }

         return result;
      }

      private double getTauX(double x, double y)
      {
         return -_problemParameters.F1 * _problemParameters.SmallQ * _problemParameters.Rho0 /
                Math.PI * Math.Cos(Math.PI * y / _problemParameters.SmallQ);
      }

      private double getTauY(double x, double y)
      {
         return _problemParameters.F2 * _problemParameters.SmallR * _problemParameters.Rho0 /
                Math.PI * Math.Cos(Math.PI * x / _problemParameters.SmallR) *
                Math.Sin(Math.PI * y / _problemParameters.SmallQ);
      }

      private double getTauXb(double u)
      {
         return _problemParameters.Mu * _problemParameters.Rho0 * u;
      }

      private double getTauYb(double v)
      {
         return _problemParameters.Mu * _problemParameters.Rho0 * v;
      }
   }
}
