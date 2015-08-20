using System;
using System.Windows;
using System.Windows.Forms;
using LiquidDynamics.Controls;
using LiquidDynamics.Forms.BaroclinicComponent;
using LiquidDynamics.Forms.BarotropicComponent;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using LiquidDynamics.Forms.EkmanSpiral;
using LiquidDynamics.Forms.IssykKul.Equation;
using LiquidDynamics.Forms.IssykKul.Grid;
using LiquidDynamics.Forms.IssykKul.Interpolation;
using LiquidDynamics.Forms.IssykKul.TestProblem;
using LiquidDynamics.Forms.IssykKul.Wind;
using LiquidDynamics.Forms.Kriging;
using LiquidDynamics.Forms.ParametersForms;
using LiquidDynamics.Forms.StommelModel;
using LiquidDynamics.Forms.TestProblem;
using LiquidDynamics.Forms.Upwelling;
using LiquidDynamics.Forms.VelocityField;
using LiquidDynamics.Forms.VerticalComponentNumerical;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics
{
   internal partial class MainForm : Form
   {
      private readonly Parameters _problemParameters;
      private readonly GraphParameters _graphParameters;

      public MainForm()
      {
         InitializeComponent();

         _problemParameters = new Parameters
                                 {
                                    Rho0 = 1000.0,
                                    SmallL0 = 1.0,
                                    Beta = 0.0,
                                    H = 1.0,
                                    SmallQ = 1.0,
                                    SmallR = 1.0,
                                    Nu = 0.05,
                                    Mu = 0.5,
                                    F1 = 10.0,
                                    F2 = 0.0,
                                    S1 = 1.0,
                                    S2 = 0.0,
                                    SmallK = 1,
                                    SmallM = 2,
                                    Phi = 0.0
                                 };

         _graphParameters = new GraphParameters
                               {
                                  XCoordinatesCount = 20,
                                  YCoordinatesCount = 20,
                                  ZCoordinatesCount = 100,
                                  TimeStep = 0.25
                               };
      }

      private void showForm(Form form)
      {
         form.MdiParent = this;
         form.Show();
      }

      private void showParametersForm(UIElement child, string caption)
      {
         var form = new ParametersForm(child) {Text = caption};
         form.ShowDialog(this);
      }

      private void problemParametersToolStripMenuItemClick(object sender, EventArgs e)
      {
         showParametersForm(new ProblemParametersControl(_problemParameters),
                            Resources.ProblemParameters);
      }

      private void graphParametersToolStripMenuItemClick(object sender, EventArgs e)
      {
         showParametersForm(new GraphParametersControl(_graphParameters),
                            Resources.GraphParameters);
      }

      private void uvToolStripMenuItemClick(object sender, EventArgs e)
      {
         var field = new UVSquareVelocityField(_problemParameters, _graphParameters);
         showForm(new SquareVelocityFieldForm(field) {Text = Resources.UVSquareVelocityField});
      }

      private void uwToolStripMenuItemClick(object sender, EventArgs e)
      {
         var field = new UWSquareVelocityField(_problemParameters, _graphParameters);
         showForm(new SquareVelocityFieldForm(field) {Text = Resources.UWSquareVelocityField});
      }

      private void vwToolStripMenuItemClick(object sender, EventArgs e)
      {
         var field = new VWSquareVelocityField(_problemParameters, _graphParameters);
         showForm(new SquareVelocityFieldForm(field) {Text = Resources.VWSquareVelocityField});
      }
      
      private void barotropicComponentToolStripMenuItemClick(object sender, EventArgs e)
      {
         var barotropicComponent = new BarotropicComponentDataProvider(_problemParameters, _graphParameters);
         showForm(new BarotropicComponentForm(barotropicComponent));
      }

      private void ekmanSpiralToolStripMenuItemClick(object sender, EventArgs e)
      {
         var ekmanSpiral = new EkmanSpiralDataProvider(_problemParameters, _graphParameters);
         showForm(new EkmanSpiralForm(ekmanSpiral));
      }

      private void upwellingToolStripMenuItemClick(object sender, EventArgs e)
      {
         var upwelling = new UpwellingDataProvider(_problemParameters, _graphParameters);
         showForm(new UpwellingForm(upwelling));
      }

      private void stommelModelToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new StommelModelForm());
      }

      private void krigingTestProblemToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new KrigingForm());
      }

      private void issykKulInterpolationToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new IssykKulInterpolationForm());
      }

      private void issykKulGridToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new IssykKulGridForm());
      }

      private void barotropicComponentTestProblemToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new BarotropicComponentNumericalForm(_problemParameters) {WindowState = FormWindowState.Maximized});
      }

      private void issykKulVelocityFieldToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new IssykKulVelocityFieldForm(_problemParameters));
      }

      private void issykKulWindToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new IssykKulWindForm());
      }

      private void baroclinicComponentToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new BaroclinicComponentForm(_problemParameters));
      }

      private void testProblemToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new TestProblemForm(_problemParameters));
      }

      private void issykKulToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new IssykKulTestProblemForm(_problemParameters));
      }

      private void verticalComponentToolStripMenuItemClick(object sender, EventArgs e)
      {
         showForm(new VerticalComponentForm(_problemParameters));
      }
   }
}
