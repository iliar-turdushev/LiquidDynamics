using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
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
      private static readonly string[] Graphs =
         {
            "Касательное напряжение трения ветра",
            "Скорость ветра"
         };

      private static readonly Pen VectorPen = new Pen(Color.Red, 1) {EndCap = LineCap.ArrowAnchor};

      public TestProblemForm()
      {
         InitializeComponent();

         initCbGraphs();
      }

      private void btnRun_Click(object sender, EventArgs e)
      {
         switch (_cbGraph.SelectedIndex)
         {
            case 0:
               buildTau();
               break;

            case 1:
               buildWind();
               break;
         }
      }

      private void initCbGraphs()
      {
         _cbGraph.BeginUpdate();
         _cbGraph.Items.AddRange(Graphs);
         _cbGraph.SelectedItem = Graphs[0];
         _cbGraph.EndUpdate();
      }

      private int getNx()
      {
         return ToInt(_txtNx.Text, "Nx");
      }

      private int getNy()
      {
         return ToInt(_txtNy.Text, "Ny");
      }

      // [out] = 1
      private double getF1()
      {
         return ToDouble(_txtF1.Text, "F1");
      }

      // [out] = 1
      private double getF2()
      {
         return ToDouble(_txtF2.Text, "F2");
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

      // [len] = mu
      // [out] = 1
      private double dimlLen(double len, MeasureUnit mu)
      {
         double cm = ToCm(len, mu);
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
            drawVectors(t.TauX, t.TauY, x, y, "г / (см * с^2)");
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
            drawVectors(w.Wx, w.Wy, x, y, "см / с");
         }
      }

      // [gx] = [gy] = 1
      // [tau] = 1
      private bool calcTau(out Tau tau, out Grid gx, out Grid gy)
      {
         try
         {
            double f1 = getF1(); // 1
            double f2 = getF2(); // 1
            double r = getR(); // 1
            double q = getQ(); // 1
            int nx = getNx();
            int ny = getNy();

            gx = new Grid(r, nx); // 1
            gy = new Grid(q, ny); // 1
            tau = Tau.Calc(f1, f2, r, q, gx, gy); // 1

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

      // [vx] = [vy] = размерные величины
      // [gx] = [gy] = см
      private void drawVectors(double[,] vx, double[,] vy, Grid gx, Grid gy, string legendText)
      {
         SquareVelocityField vectors = buildVectorField(vx, vy, gx, gy);
         float x = (float) ToKm(gx[gx.N - 1], MeasureUnit.Cm); // км
         float y = (float) ToKm(gy[gy.N - 1], MeasureUnit.Cm); // км

         _gcGraph.Clear();
         _gcGraph.Caption = Graphs[_cbGraph.SelectedIndex];
         _gcGraph.XAxisName = "x, км";
         _gcGraph.YAxisName = "y, км";
         _gcGraph.AddLegend(legendText, VectorPen);
         _gcGraph.AxisBounds = new Bounds(0, x, 0, y);
         _gcGraph.DrawVectorField(vectors, VectorPen);
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
   }
}
