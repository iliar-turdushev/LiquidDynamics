using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Common;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.VerticalComponentNumerical
{
   public partial class VerticalComponentForm : Form
   {
      private const int DrawUpwelling = 0;

      private static readonly Pen ErrorPen = new Pen(Color.Red, 1)
                                                {StartCap = LineCap.RoundAnchor, EndCap = LineCap.RoundAnchor};
      private readonly IPaletteDrawingTools _paletteDrawingTools;

      private readonly Parameters _parameters;
      private readonly VerticalComponentProblemSolver _solver;

      private VerticalComponentResult _result;

      private bool _dynamicsActive;

      public VerticalComponentForm(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");
         
         _parameters = parameters;
         _solver = new VerticalComponentProblemSolver();

         InitializeComponent();

         _paletteDrawingTools = PaletteFactory.CreateBlueRedPalette();
         _paletteControl.PaletteDrawingTools = _paletteDrawingTools;

         _comboBoxGraphType.SelectedIndex = 0;
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         try
         {
            _result = _solver.Begin(readNx(), readNy(), readNz(), readTau(), _parameters);
            drawGraph();

            _buttonStep.Enabled = true;
            _buttonStartPause.Enabled = true;
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void buttonStepClick(object sender, EventArgs e)
      {
         _result = _solver.Step();
         drawGraph();
      }

      private void buttonStartPauseClick(object sender, EventArgs e)
      {
         _dynamicsActive = !_dynamicsActive;
         _timer.Enabled = _dynamicsActive;

         if (_dynamicsActive)
         {
            _buttonStartPause.Image = Resources.Pause;
            setButtonsAccessibility(false);
         }
         else
         {
            _buttonStartPause.Image = Resources.Start;
            setButtonsAccessibility(true);
         }
      }

      private void timerTick(object sender, EventArgs e)
      {
         _result = _solver.Step();
         drawGraph();
      }

      private void setButtonsAccessibility(bool isEnabled)
      {
         _buttonStep.Enabled = isEnabled;
         _buttonReset.Enabled = isEnabled;
      }

      private void drawGraph()
      {
         if (_comboBoxGraphType.SelectedIndex == DrawUpwelling)
            drawUpwelling();
         else
            drawError();

         drawPallete();
      }

      private void drawUpwelling()
      {
         UpwellingData upwellingData = _result.UpwellingData[readSlice()];

         _graphControl.AxisBounds = new Bounds(0, (float) _parameters.SmallR,
                                               0, (float) _parameters.SmallQ);

         _graphControl.Clear();
         _graphControl.Caption = string.Format("Time = {0:F4}, Error = {1:F4}%", _result.Time, _result.Error);
         _graphControl.DrawUpwelling(upwellingData, _paletteDrawingTools);
         _graphControl.Invalidate();
      }

      private void drawError()
      {
         _graphControl.AxisBounds = new Bounds(0, _solver.Errors.Time, 0, _solver.Errors.MaxError);

         _graphControl.Clear();
         _graphControl.Caption = string.Format("Time = {0:F4}, Error = {1:F4}%",
                                               _solver.Errors.Time, _result.Error);
         _graphControl.DrawLines(_solver.Errors.Errors, ErrorPen);
         _graphControl.Invalidate();
      }

      private void drawPallete()
      {
         UpwellingData upwellingData = _result.UpwellingData[readSlice()];
         
         float value = Math.Max(Math.Abs(upwellingData.GetMinIntensity()),
                                Math.Abs(upwellingData.GetMaxIntensity()));

         _paletteControl.MinValue = -value;
         _paletteControl.MaxValue = value;
         _paletteControl.Invalidate();
      }

      private int readNx()
      {
         return readIntValue(_textBoxNx.Text, "Nx");
      }

      private int readNy()
      {
         return readIntValue(_textBoxNy.Text, "Ny");
      }

      private int readNz()
      {
         return readIntValue(_textBoxNz.Text, "Nz");
      }

      private double readTau()
      {
         return readDoubleValue(_textBoxTau.Text, "Tau");
      }

      private int readSlice()
      {
         return readIntValue(_textBoxSlice.Text, "Срез");
      }
      
      private static double readDoubleValue(string textToRead, string parameterName)
      {
         double value;

         if (!double.TryParse(textToRead, out value))
         {
            generateError(parameterName);
         }

         return value;
      }

      private static int readIntValue(string textToRead, string parameterName)
      {
         int value;

         if (!int.TryParse(textToRead, out value))
         {
            generateError(parameterName);
         }

         return value;
      }

      private static void generateError(string parameterName)
      {
         var message = string.Format(Resources.InvalidParameterValue, parameterName);
         throw new InvalidFieldValueException(message);
      }
   }
}
