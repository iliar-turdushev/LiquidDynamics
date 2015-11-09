using System.Drawing;
using System.Drawing.Drawing2D;
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

      private static readonly Pen ErrorUPen = new Pen(Color.Red, 1)
                                                 {StartCap = LineCap.RoundAnchor, EndCap = LineCap.RoundAnchor};

      private static readonly Pen ErrorVPen = new Pen(Color.Green, 1)
                                                 {StartCap = LineCap.RoundAnchor, EndCap = LineCap.RoundAnchor};

      private readonly Parameters _parameters;
      private readonly BaroclinicComponentDataProvider _provider;

      private bool _dynamicsAlive;

      private int _yBound;

      private SolveBaroclinicProblemResult _result;

      private int _x;
      private int _y;

      public BaroclinicComponentForm(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");
         
         _parameters = parameters;
         _provider = new BaroclinicComponentDataProvider(parameters);

         InitializeComponent();

         addLegend(_graphControlU);
         addLegend(_graphControlV);

         _yBound = YBoundInitialValue;

         _x = int.Parse(_textBoxX.Text);
         _y = int.Parse(_textBoxY.Text);
      }

      private void buttonBeginClick(object sender, System.EventArgs e)
      {
         try
         {
            _result = _provider.Begin(readNx(), readNy(), readNz(), readTau());

            drawU(_result);
            drawV(_result);
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

      private void buttonDrawModeCheckedChanged(object sender, System.EventArgs e)
      {
         if (_buttonDrawMode.Checked)
            _buttonDrawMode.Text = "Графики решений";
         else
            _buttonDrawMode.Text = "Графики погрешностей";
      }

      private void textBoxXKeyPress(object sender, KeyPressEventArgs e)
      {
         if (_buttonDrawMode.Checked)
            return;

         switch (e.KeyChar)
         {
            case (char) Keys.Return:
               int cut;

               if (!int.TryParse(_textBoxX.Text, out cut))
               {
                  MessageBox.Show(Resources.IncorrectValue, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }

               try
               {
                  _x = cut;
                  drawU(_result);
                  drawV(_result);
               }
               catch (InvalidFieldValueException exception)
               {
                  MessageBox.Show(exception.Message, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
               }

               break;
         }
      }

      private void textBoxYKeyPress(object sender, KeyPressEventArgs e)
      {
         if (_buttonDrawMode.Checked)
            return;

         switch (e.KeyChar)
         {
            case (char) Keys.Return:
               int cut;

               if (!int.TryParse(_textBoxY.Text, out cut))
               {
                  MessageBox.Show(Resources.IncorrectValue, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }

               try
               {
                  _y = cut;
                  drawU(_result);
                  drawV(_result);
               }
               catch (InvalidFieldValueException exception)
               {
                  MessageBox.Show(exception.Message, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
               }

               break;
         }
      }

      private void step()
      {
         _result = _provider.Step();
         drawU(_result);
         drawV(_result);
      }

      private void addLegend(GraphControl graphControl)
      {
         graphControl.AddLegend("Exact", ExactSolutionPen);
         graphControl.AddLegend("Calculated", CalculatedSolutionPen);
      }

      private void drawU(SolveBaroclinicProblemResult result)
      {
         string caption = string.Format("U: Time = {0:F4}, Error = {1:F4}%", result.Time, result.ErrorU);

         if (_buttonDrawMode.Checked)
         {
            drawErrors(_provider.Errors.ErrorsU, _provider.Errors.Time, _provider.Errors.MaxErrorU,
                       _graphControlU, ErrorUPen, caption);
         }
         else
         {
            drawSolutions(_graphControlU, result.ExactU[_x, _y], result.CalculatedU[_x, _y], caption);
         }
      }

      private void drawV(SolveBaroclinicProblemResult result)
      {
         string caption = string.Format("V: Time = {0:F4}, Error = {1:F4}%", result.Time, result.ErrorV);

         if (_buttonDrawMode.Checked)
         {
            drawErrors(_provider.Errors.ErrorsV, _provider.Errors.Time, _provider.Errors.MaxErrorV,
                       _graphControlV, ErrorVPen, caption);
         }
         else
         {
            drawSolutions(_graphControlV, result.ExactV[_x, _y], result.CalculatedV[_x, _y], caption);
         }
      }

      private void drawErrors(
         PointF[] errors, float time, float maxError,
         GraphControl graphControl, Pen pen, string caption
         )
      {
         graphControl.Clear();

         if (errors.Length > 1)
         {
            graphControl.Caption = caption;
            graphControl.AxisBounds = new Bounds(0.0F, time, 0.0F, maxError);
            graphControl.DrawLines(errors, pen);
         }

         graphControl.Invalidate();
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
