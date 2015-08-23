using System;
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
      private readonly IPaletteDrawingTools _paletteDrawingTools;

      private readonly Parameters _parameters;
      private readonly VerticalComponentProblemSolver _solver;

      private VerticalComponentResult _result;

      public VerticalComponentForm(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");
         
         _parameters = parameters;
         _solver = new VerticalComponentProblemSolver();

         InitializeComponent();

         _paletteDrawingTools = PaletteFactory.CreateBlueRedPalette();
         _paletteControl.PaletteDrawingTools = _paletteDrawingTools;
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         try
         {
            _result = _solver.Begin(readNx(), readNy(), readNz(), readTau(), _parameters);
            drawUpwelling(_result.UpwellingData[readSlice()]);
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void buttonStepClick(object sender, EventArgs e)
      {

      }

      private void drawUpwelling(UpwellingData upwellingData)
      {
         _graphControl.AxisBounds = new Bounds(0, (float) _parameters.SmallR,
                                               0, (float) _parameters.SmallQ);

         _graphControl.Clear();
         _graphControl.Caption = getCaption();
         _graphControl.DrawUpwelling(upwellingData, _paletteDrawingTools);
         _graphControl.Invalidate();

         float value = Math.Max(Math.Abs(upwellingData.GetMinIntensity()),
                                Math.Abs(upwellingData.GetMaxIntensity()));

         _paletteControl.MinValue = -value;
         _paletteControl.MaxValue = value;
         _paletteControl.Invalidate();
      }

      private string getCaption()
      {
         return string.Format("Time = {0:F4}, Error = {1:F4}%",
                              _result.Time, _result.Error);
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
