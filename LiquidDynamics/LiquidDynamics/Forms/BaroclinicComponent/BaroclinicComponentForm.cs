using System.Drawing;
using System.Windows.Forms;
using Common;
using ControlLibrary.Controls;
using ControlLibrary.Types;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.BaroclinicComponent
{
   public partial class BaroclinicComponentForm : Form
   {
      private const int YBoundStep = 10;
      private const int YBoundInitialValue = 20;

      private static readonly Pen ExactSolutionPen = new Pen(Color.Green, 1);
      private static readonly Pen CalculatedSolutionPen = new Pen(Color.Red, 1);

      private readonly Parameters _parameters;
      private readonly BaroclinicComponentDataProvider _provider;

      private bool _dynamicsAlive;

      private int _yBound;

      public BaroclinicComponentForm(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");
         
         _parameters = parameters;
         _provider = new BaroclinicComponentDataProvider(parameters);

         InitializeComponent();

         addLegend(_graphControlU);
         addLegend(_graphControlV);

         _textBoxX.Text = string.Format("{0}", _parameters.SmallR / 2.0);
         _textBoxY.Text = string.Format("{0}", _parameters.SmallQ / 2.0);

         _yBound = YBoundInitialValue;
      }

      private void buttonBeginClick(object sender, System.EventArgs e)
      {
         try
         {
            SolveBaroclinicProblemResult result =
               _provider.Begin(readX(), readY(), readN(), readTau());

            drawU(result);
            drawV(result);
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void buttonStartStopClick(object sender, System.EventArgs e)
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

      private void buttonStepClick(object sender, System.EventArgs e)
      {
         step();
      }

      private void timerTick(object sender, System.EventArgs e)
      {
         step();
      }

      private void buttonPlusClick(object sender, System.EventArgs e)
      {
         _yBound += YBoundStep;
         changeAxisBounds();
      }

      private void buttonMinusClick(object sender, System.EventArgs e)
      {
         if (_yBound > YBoundInitialValue)
         {
            _yBound -= YBoundStep;
            changeAxisBounds();
         }
      }

      private void step()
      {
         SolveBaroclinicProblemResult result = _provider.Step();
         drawU(result);
         drawV(result);
      }

      private void addLegend(GraphControl graphControl)
      {
         graphControl.AddLegend("Exact", ExactSolutionPen);
         graphControl.AddLegend("Calculated", CalculatedSolutionPen);
      }

      private void drawU(SolveBaroclinicProblemResult result)
      {
         string caption = string.Format("U: Time = {0:F4}, Error = {1:F4}%", result.Time, result.ErrorU);
         drawSolutions(_graphControlU, result.ExactU, result.CalculatedU, caption);
      }

      private void drawV(SolveBaroclinicProblemResult result)
      {
         string caption = string.Format("V: Time = {0:F4}, Error = {1:F4}%", result.Time, result.ErrorV);
         drawSolutions(_graphControlV, result.ExactV, result.CalculatedV, caption);
      }

      private void drawSolutions(GraphControl graphControl, PointF[] exact, PointF[] calculated, string caption)
      {
         graphControl.AxisBounds = new Bounds(0, (float) _parameters.H, -_yBound, _yBound);

         graphControl.Clear();
         graphControl.Caption = caption;
         graphControl.DrawCurve(exact, ExactSolutionPen);
         graphControl.DrawCurve(calculated, CalculatedSolutionPen);
         graphControl.Invalidate();
      }

      private void setButtonsAccessibility(bool enabled)
      {
         _buttonBegin.Enabled = enabled;
         _buttonStep.Enabled = enabled;
      }

      private void changeAxisBounds()
      {
         _graphControlU.AxisBounds = new Bounds(0, (float) _parameters.H, -_yBound, _yBound);
         _graphControlV.AxisBounds = new Bounds(0, (float) _parameters.H, -_yBound, _yBound);

         _graphControlU.Invalidate();
         _graphControlV.Invalidate();
      }

      private double readX()
      {
         return readDoubleValue(_textBoxX.Text, "X");
      }

      private double readY()
      {
         return readDoubleValue(_textBoxY.Text, "Y");
      }

      private int readN()
      {
         return readIntValue(_textBoxN.Text, "N");
      }

      private double readTau()
      {
         return readDoubleValue(_textBoxTau.Text, "Tau");
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
