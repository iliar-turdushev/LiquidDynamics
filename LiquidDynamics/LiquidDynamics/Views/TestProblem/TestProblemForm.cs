using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ControlLibrary.Graphs;
using ControlLibrary.Types;
using LiquidDynamics.MathModel;
using LiquidDynamics.MathModel.TestProblem;
using static LiquidDynamics.MathModel.DimensionConverter;
using static LiquidDynamics.MathModel.MeasureUnitConverter;
using static LiquidDynamics.Utils.StringConverter;

namespace LiquidDynamics.Views.TestProblem
{
   public partial class TestProblemForm : Form
   {
      private const string BtnBuild = "Построить";
      private const string BtnRun = "Запустить";
      private const string BtnStop = "Остановить";

      private static readonly string[] Graphs =
         {
            "Касательное напряжение трения ветра",
            "Скорость ветра",
            "Баротропная компонента"
         };

      private static readonly Pen VectorPen =
         new Pen(Color.Black, 1) {EndCap = LineCap.ArrowAnchor};

      private Grid _gx; // 1
      private Grid _gy; // 1

      private BarotropicCalc _calc;

      public TestProblemForm()
      {
         InitializeComponent();

         initCbGraphs();
      }

      private void cbGraph_SelectedIndexChanged(object sender, EventArgs e)
      {
         switch (_cbGraph.SelectedIndex)
         {
            case 0:
            case 1:
               prepareStaticUi();
               break;

            default:
               prepareDynamicUi();
               break;
         }

         _timer.Enabled = false;
      }

      private void btnBegin_Click(object sender, EventArgs e)
      {
         buildCalc();
         buildNext();

         _btnStep.Enabled = true;
         _btnRunStop.Enabled = true;
      }

      private void btnStep_Click(object sender, EventArgs e)
      {
         buildNext();
      }

      private void btnRunStop_Click(object sender, EventArgs e)
      {
         switch (_cbGraph.SelectedIndex)
         {
            case 0:
               buildTau();
               break;

            case 1:
               buildWind();
               break;

            default:
               updateDynamicUi();
               break;
         }
      }

      private void timer_Tick(object sender, EventArgs e)
      {
         buildNext();
      }

      private void initCbGraphs()
      {
         _cbGraph.BeginUpdate();
         _cbGraph.Items.AddRange(Graphs);
         _cbGraph.SelectedItem = Graphs[0];
         _cbGraph.EndUpdate();
      }

      private void prepareDynamicUi()
      {
         _btnBegin.Visible = true;
         _btnStep.Visible = true;

         _btnBegin.Enabled = true;
         _btnStep.Enabled = false;
         _btnRunStop.Enabled = false;

         _btnRunStop.Text = BtnRun;

         _gbTime.Visible = true;
         setTime(0);
      }

      private void prepareStaticUi()
      {
         _btnBegin.Visible = false;
         _btnStep.Visible = false;

         _btnBegin.Enabled = false;
         _btnStep.Enabled = false;
         _btnRunStop.Enabled = true;

         _btnRunStop.Text = BtnBuild;

         _gbTime.Visible = false;
      }

      private void updateDynamicUi()
      {
         bool e = !_timer.Enabled;

         _timer.Enabled = e;
         _btnBegin.Enabled = !e;
         _btnStep.Enabled = !e;
         _btnRunStop.Text = e ? BtnStop : BtnRun;
      }

      // [out] = 1
      private double getR()
      {
         double r = ToDouble(_txtR.Text, "r"); // км
         return dimlLen(r, MeasureUnit.Km);
      }

      // [out] = 1
      private double getQ()
      {
         double q = ToDouble(_txtQ.Text, "q"); // км
         return dimlLen(q, MeasureUnit.Km);
      }

      // [out] = 1
      private double getH()
      {
         double h = ToDouble(_txtH.Text, "H"); // м
         double cm = ToCm(h, MeasureUnit.M); // см
         return DimlH(cm);
      }

      // [out] = 1
      private double getF1() => ToDouble(_txtF1.Text, "F1");

      // [out] = 1
      private double getF2() => ToDouble(_txtF2.Text, "F2");

      // [out] = 1
      private double getBeta() => ToDouble(_txtBeta.Text, "beta");

      // [out] = 1
      private double getMu()
      {
         double mu = ToDouble(_txtMu.Text, "mu"); // 1/c
         return DimlMu(mu);
      }

      // [out] = 1
      private int getM() => ToInt(_txtM.Text, "m");

      // [out] = 1
      private int getK() => ToInt(_txtK.Text, "k");

      // [out] = 1
      private double getS1() => ToDouble(_txtS1.Text, "s1");

      // [out] = 1
      private double getS2() => ToDouble(_txtS2.Text, "s2");

      // [out] = 1
      private int getNx() => ToInt(_txtNx.Text, "Nx");

      // [out] = 1
      private int getNy() => ToInt(_txtNy.Text, "Ny");

      // [out] = 1
      private double getTau()
      {
         double tau = ToDouble(_txtTau.Text, "tau"); // с
         return DimlT(tau);
      }

      // [len] = mu
      // [out] = 1
      private double dimlLen(double len, MeasureUnit mu)
      {
         double cm = ToCm(len, mu); // см
         return DimlLen(cm);
      }

      private static void showErrorMessage(FormatException e)
      {
         MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      private void buildTau()
      {
         Grid gx; // 1
         Grid gy; // 1
         Tau tau; // 1

         if (calcTau(out tau, out gx, out gy))
         {
            Tau t = DimTau(tau); // г/(см*с^2)
            Grid x = DimLen(gx); // см
            Grid y = DimLen(gy); // см
            drawVectors(t.TauX, t.TauY, x, y, "г/(см*с^2)");
         }
      }

      private void buildWind()
      {
         Grid gx; // 1
         Grid gy; // 1
         Tau tau; // 1

         if (calcTau(out tau, out gx, out gy))
         {
            Wind wind = Wind.Create(tau); // 1
            Wind w = DimWind(wind); // см/с
            Grid x = DimLen(gx); // см
            Grid y = DimLen(gy); // см
            drawVectors(w.Wx, w.Wy, x, y, "см/с");
         }
      }

      // [gx] = [gy] = 1
      // [tau] = 1
      private bool calcTau(out Tau tau, out Grid gx, out Grid gy)
      {
         try
         {
            double r = getR(); // 1
            double q = getQ(); // 1
            double f1 = getF1(); // 1
            double f2 = getF2(); // 1
            int nx = getNx(); // 1
            int ny = getNy(); // 1

            gx = new Grid(r, nx); // 1
            gy = new Grid(q, ny); // 1
            tau = Tau.Calc(r, q, f1, f2, gx, gy); // 1

            return true;
         }
         catch (FormatException e)
         {
            showErrorMessage(e);

            gx = null;
            gy = null;
            tau = null;

            return false;
         }
      }

      private void buildCalc()
      {
         try
         {
            double r = getR(); // 1
            double q = getQ(); // 1
            double h = getH(); // 1
            double f1 = getF1(); // 1
            double f2 = getF2(); // 1
            double beta = getBeta(); // 1
            double mu = getMu(); // 1
            int m = getM(); // 1
            int k = getK(); // 1
            double s1 = getS1(); // 1
            double s2 = getS2(); // 1

            int nx = getNx(); // 1
            int ny = getNy(); // 1
            double tau = getTau(); // 1
            
            _gx = new Grid(r, nx); // 1
            _gy = new Grid(q, ny); // 1

            _calc = new BarotropicCalc(
               r, q, h, f1, f2, beta, mu, m, k, s1, s2, _gx, _gy, tau);
         }
         catch (FormatException e)
         {
            showErrorMessage(e);
         }
      }

      private void buildNext()
      {
         if (_calc == null)
            throw new InvalidOperationException($"{nameof(_calc)} is null");

         if (_gx == null)
            throw new InvalidOperationException($"{nameof(_gx)} is null");

         if (_gy == null)
            throw new InvalidOperationException($"{nameof(_gy)} is null");

         Barotropic barot = _calc.Next(); // 1
         Barotropic b = DimBarot(barot); // см/с
         Grid x = DimLen(_gx); // см
         Grid y = DimLen(_gy); // см
         drawVectors(b.U, b.V, x, y, "см/с");
         setTime(_calc.T);
      }

      // [vx] = [vy] = размерные величины
      // [gx] = [gy] = см
      private void drawVectors(double[,] vx, double[,] vy, Grid gx, Grid gy, string legendText)
      {
         SquareVelocityField vectors = buildVectorField(vx, vy, gx, gy);
         float x = (float) ToKm(gx[gx.N - 1], MeasureUnit.Cm); // км
         float y = (float) ToKm(gy[gy.N - 1], MeasureUnit.Cm); // км

         IPaletteDrawingTools colorMap = PaletteFactory.CreateParulaPalette();

         _pcColorMap.PaletteDrawingTools = colorMap;
         _pcColorMap.MinValue = vectors.GetMinVector().Length;
         _pcColorMap.MaxValue = vectors.GetMaxVector().Length;
         _pcColorMap.Invalidate();

         _gcGraph.Clear();
         _gcGraph.Caption = Graphs[_cbGraph.SelectedIndex];
         _gcGraph.XAxisName = "x, км";
         _gcGraph.YAxisName = "y, км";
         _gcGraph.AddLegend(legendText, VectorPen);
         _gcGraph.AxisBounds = new Bounds(0, x, 0, y);
         _gcGraph.DrawVectorField(vectors, VectorPen, colorMap);
         _gcGraph.Invalidate();
      }

      // [vx] = [vy] = размерные величины
      // [gx] = [gy] = см
      private static SquareVelocityField buildVectorField(double[,] vx, double[,] vy, Grid gx, Grid gy)
      {
         var vecs = new Vector[gx.N, gy.N];

         for (int i = 0; i < gx.N; i++)
         {
            var sx = (float) ToKm(gx[i], MeasureUnit.Cm); // км

            for (int j = 0; j < gy.N; j++)
            {
               var sy = (float) ToKm(gy[j], MeasureUnit.Cm); // км
               var ex = (float) vx[i, j];
               var ey = (float) vy[i, j];
               vecs[i, j] = new Vector(sx, sy, ex, ey);
            }
         }

         float w = (float) ToKm(gx.H, MeasureUnit.Cm); // км
         float h = (float) ToKm(gy.H, MeasureUnit.Cm); // км

         return new SquareVelocityField(vecs, w, h);
      }

      // [t] = 1
      private void setTime(double t)
      {
         double sec = DimT(t); // с
         _txtTime.Text = $"{TimeSpan.FromSeconds(sec):g}";
      }
   }
}
