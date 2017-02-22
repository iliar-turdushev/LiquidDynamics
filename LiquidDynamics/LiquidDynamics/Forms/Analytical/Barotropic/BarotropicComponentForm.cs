using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using Common;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.Analytical.Barotropic
{
   internal partial class BarotropicComponentForm : Form
   {
      private static readonly IPaletteDrawingTools Palette = PaletteFactory.CreateBluePalette();
      private static readonly Pen VectorPen = new Pen(Color.Crimson, 1) {EndCap = LineCap.ArrowAnchor};

      private readonly BarotropicComponentDataProvider _barotropicComponent;
      private bool _dynamicsActive;

      public BarotropicComponentForm(Parameters parameters)
      {
         InitializeComponent();

         Check.NotNull(parameters, nameof(parameters));
         _barotropicComponent = new BarotropicComponentDataProvider(parameters);
         _paletteControl.PaletteDrawingTools = Palette;
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         try
         {
            int nx = readNx();
            int ny = readNy();
            double dt = readDt();
            drawGraph(_barotropicComponent.Begin(nx, ny, dt));
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.Analytical_Barotropic_MessageBoxCaption,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void buttonStartStopClick(object sender, EventArgs e)
      {
         _dynamicsActive = !_dynamicsActive;
         _timer.Enabled = _dynamicsActive;

         if (_dynamicsActive)
         {
            _buttonStartStop.Image = Resources.Pause;
            setControlsAccessibillity(false);
         }
         else
         {
            _buttonStartStop.Image = Resources.Start;
            setControlsAccessibillity(true);
         }
      }

      private void buttonStepForwardClick(object sender, EventArgs e)
      {
         makeStep();
      }
      
      private void timerTick(object sender, EventArgs e)
      {
         makeStep();
      }

      private void makeStep()
      {
         drawGraph(_barotropicComponent.Step());
      }

      private void drawGraph(SquareVelocityField barotropic)
      {
         _graphControl.Caption = $"Время: {_barotropicComponent.Time:F4}";

         Bounds b = _barotropicComponent.Bounds;
         float w = barotropic.CellSize.Width / 2;
         float h = barotropic.CellSize.Height / 2;

         _graphControl.AxisBounds = new Bounds(b.XMin - w, b.XMax + w, b.YMin - h, b.YMax + h);

         _graphControl.Clear();
         _graphControl.DrawVelocityField(barotropic, Palette, VectorPen);
         _graphControl.Invalidate();

         _paletteControl.MinValue = barotropic.GetMinVector().Length;
         _paletteControl.MaxValue = barotropic.GetMaxVector().Length;
         _paletteControl.Invalidate();
      }

      private void setControlsAccessibillity(bool isEnabled)
      {
         _buttonStepForward.Enabled = isEnabled;
         _buttonReset.Enabled = isEnabled;
      }

      private int readNx()
      {
         return readN(_textBoxNx.Text, "Nx");
      }

      private int readNy()
      {
         return readN(_textBoxNy.Text, "Ny");
      }

      private static int readN(string value, string name)
      {
         int n;

         if (!int.TryParse(value, out n))
         {
            var msg = string.Format(Resources.Analytical_Barotropic_ParameterMustBeInteger, name);
            throw new InvalidFieldValueException(msg);
         }

         if (n <= 1)
         {
            var msg = string.Format(Resources.Analytical_Barotrpic_ParameterMustBeGreaterThanOne, name);
            throw new InvalidFieldValueException(msg);
         }

         return n;
      }

      private double readDt()
      {
         double dt;

         if (!tryParseDouble(out dt))
         {
            var msg = string.Format(Resources.Analytical_Barotropic_ParameterMustBeDouble, "Dt");
            throw new InvalidFieldValueException(msg);
         }

         if (dt <= 0)
         {
            var msg = string.Format(Resources.Analytical_Barotrpic_ParameterMustBeGreaterThanOne, "Dt");
            throw new InvalidFieldValueException(msg);
         }

         return dt;
      }

      private bool tryParseDouble(out double d)
      {
         return double.TryParse(_textBoxDt.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out d);
      }
   }
}
