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
            "Касательное напряжение трения ветра"
         };

      private static readonly Pen VectorPen = new Pen(Color.Red, 1) {EndCap = LineCap.ArrowAnchor};

      public TestProblemForm()
      {
         InitializeComponent();

         initCbGraphs();
      }

      private void btnRun_Click(object sender, System.EventArgs e)
      {
         switch (_cbGraph.SelectedIndex)
         {
            case 0:
               buildTau();
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
         double f1;
         double f2;
         double r;
         double q;
         int nx;
         int ny;

         try
         {
            f1 = getF1(); // 1
            f2 = getF2(); // 1
            r = getR(); // 1
            q = getQ(); // 1
            nx = getNx();
            ny = getNy();
         }
         catch (FormatException e)
         {
            showErrorMessage(e);
            return;
         }

         var gx = new Grid(r, nx); // 1
         var gy = new Grid(q, ny); // 1
         Tau tau = Tau.Calc(f1, f2, r, q, gx, gy); // 1
         
         drawTau(DimTau(tau), DimLen(gx), DimLen(gy));
      }
      
      // [tau] = г/(см*с^2)
      // [gx] = [gy] = см
      private void drawTau(Tau tau, Grid gx, Grid gy)
      {
         SquareVelocityField vectors = buildVectorField(tau, gx, gy);

         _gcGraph.Clear();
         _gcGraph.Caption = Graphs[_cbGraph.SelectedIndex];

         float x = (float) ToKm(gx[gx.N - 1], MeasureUnit.Cm); // км
         float y = (float) ToKm(gy[gy.N - 1], MeasureUnit.Cm); // км
         _gcGraph.AxisBounds = new Bounds(0, x, 0, y);

         _gcGraph.DrawVelocityField(vectors, VectorPen);
         _gcGraph.Invalidate();
      }

      // [tau] = г/(см*с^2)
      // [gx] = [gy] = см
      private static SquareVelocityField buildVectorField(Tau tau, Grid gx, Grid gy)
      {
         var vecs = new Vector[tau.Ny, tau.Ny];

         for (int i = 0; i < gx.N; i++)
         {
            var sx = (float) ToKm(gx[i], MeasureUnit.Cm); // км

            for (int j = 0; j < gy.N; j++)
            {
               var sy = (float) ToKm(gy[j], MeasureUnit.Cm); // км
               var ex = (float) tau.TauX[i, j];
               var ey = (float) tau.TauY[i, j];
               vecs[i, j] = new Vector(sx, sy, ex, ey);
            }
         }

         float w = (float) ToKm(gx.H, MeasureUnit.Cm); // км
         float h = (float) ToKm(gy.H, MeasureUnit.Cm); // км

         return new SquareVelocityField(vecs, w, h);
      }
   }
}
