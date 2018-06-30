using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BarotropicComponentProblem.IterationMethod;
using Common;
using ControlLibrary.Controls;
using ControlLibrary.Graphs;
using ControlLibrary.Types;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.BarotropicComponentNumerical
{
   public partial class BarotropicComponentNumericalForm : Form
   {
      private const string Separately = "Отдельно";
      private const string Together = "Вместе";
      private const string Error = "Погрешность";

      private const string DifferentialScheme1 = "Разностная схема 1";
      private const string DifferentialScheme2 = "Разностная схема 2";
      private const string IntegroInterpolatingScheme = "ПВИИМ";

      private static readonly IPaletteDrawingTools PaletteDrawingTools = PaletteFactory.CreateBluePalette();

      private static readonly Pen SolutionPen = new Pen(Color.Red, 1) {EndCap = LineCap.ArrowAnchor};
      private static readonly Pen ExactSolutionPen = new Pen(Color.Green, 1) {EndCap = LineCap.ArrowAnchor};
      private static readonly Pen CalculatedSolutionPen = new Pen(Color.Red, 1) {EndCap = LineCap.ArrowAnchor};

      private static readonly Pen ErrorUPen = new Pen(Color.Red, 1)
                                                 {StartCap = LineCap.RoundAnchor, EndCap = LineCap.RoundAnchor};
      private static readonly Pen ErrorVPen = new Pen(Color.Green, 1)
                                                 {StartCap = LineCap.RoundAnchor, EndCap = LineCap.RoundAnchor};

      private readonly BarotropicComponentProblem _problem;
      private readonly Parameters _problemParameters;
      private BarotropicComponentResult _current;

      private bool _dynamicsActive;
      private GraphType _graphType;

      public BarotropicComponentNumericalForm(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");

         InitializeComponent();

         _comboBoxGraphs.Items.Add(Separately);
         _comboBoxGraphs.Items.Add(Together);
         _comboBoxGraphs.Items.Add(Error);
         _comboBoxGraphs.SelectedItem = Separately;

         _comboBoxScheme.Items.Add(DifferentialScheme1);
         _comboBoxScheme.Items.Add(DifferentialScheme2);
         _comboBoxScheme.Items.Add(IntegroInterpolatingScheme);
         _comboBoxScheme.SelectedItem = DifferentialScheme1;
         
         _exactSolutionPaletteControl.PaletteDrawingTools = PaletteDrawingTools;
         _calculatedSolutionPaletteControl.PaletteDrawingTools = PaletteDrawingTools;

         _problem = new BarotropicComponentProblem();
         _problemParameters = parameters;

         _graphType = GraphType.Separately;
      }

      private void comboBoxGraphsSelectedIndexChanged(object sender, EventArgs e)
      {
         var item = (string) _comboBoxGraphs.SelectedItem;

         switch (item)
         {
            case Separately:
               _splitContainer.Visible = true;
               _graphControl.Visible = false;

               _graphType = GraphType.Separately;
               break;

            case Together:
            case Error:
               _splitContainer.Visible = false;
               _graphControl.Visible = true;

               _graphType = item == Together ? GraphType.Together : GraphType.Error;
               break;
         }

         if (_current != null)
            drawGraphs(_current);
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         try
         {
            var gridParameters = new GridParameters(readTau(), readN(), readM());
            _current = _problem.Reset(_problemParameters, gridParameters,
                                      readSigma(), readDelta(), readK(),
                                      readTheta(), readChi(), getSchemeType());
            displayResetResult(_current);

            _buttonStep.Enabled = true;
            _buttonStartStop.Enabled = true;
         }
         catch (InvalidFieldValueException exception)
         {
            MessageBox.Show(exception.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void buttonStepClick(object sender, EventArgs e)
      {
         doStep();
      }

      private void buttonStartStopClick(object sender, EventArgs e)
      {
         _dynamicsActive = !_dynamicsActive;
         _timer.Enabled = _dynamicsActive;

         if (_dynamicsActive)
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

      private void timerTick(object sender, EventArgs e)
      {
         doStep();
      }

      private void labelResultClick(object sender, EventArgs e)
      {
         if (!string.IsNullOrEmpty(_labelResult.Text))
         {
            Clipboard.SetText(_labelResult.Text);
         }
      }

      private void doStep()
      {
         _current = _problem.Step();
         displayStepResult(_current);
      }

      private void drawGraphs(BarotropicComponentResult result)
      {
         var width = result.ExactSolution.CellSize.Width;
         var height = result.ExactSolution.CellSize.Height;
         var axisBounds = new Bounds(-width / 2, (float) _problemParameters.SmallR + width / 2,
                                     -height / 2, (float) _problemParameters.SmallQ + height / 2);

         switch (_graphType)
         {
            case GraphType.Separately:
               _exactSolutionGraphControl.AxisBounds = axisBounds;
               displayVelocityField(result.ExactSolution, SolutionPen,
                                    _exactSolutionGraphControl, _exactSolutionPaletteControl);

               _calculatedSolutionGraphControl.AxisBounds = axisBounds;
               displayVelocityField(result.CalculatedSolution, SolutionPen,
                                    _calculatedSolutionGraphControl, _calculatedSolutionPaletteControl);
               break;

            case GraphType.Together:
               _graphControl.Caption = "Точное и приближенное решения";
               _graphControl.AxisBounds = axisBounds;
               displayVelocityField(result);
               break;

            default: // GraphType.Error;
               _graphControl.Caption = "Погрешность";
               displayError(_problem.ErrorContainer);
               break;
         }
      }

      private void displayResetResult(BarotropicComponentResult result)
      {
         _labelResult.Text = string.Format("Время: {0}", result.Time);
         drawGraphs(result);
      }

      private void displayStepResult(BarotropicComponentResult result)
      {
         _labelResult.Text =
            string.Format("Время = {0:F3}; Погрешность U: {1:F5}%; Погрешность V = {2:F5}%; Статус: {3}",
                          result.Time, result.ErrorU, result.ErrorV, getStatusName(result.IterationStatus));
         drawGraphs(result);
      }

      private void displayError(ErrorContainer errors)
      {
         _graphControl.Clear();

         if (errors != null && errors.ErrorsU.Length > 1)
         {
            _graphControl.AxisBounds = new Bounds(0.0F, errors.Time, 0.0F, errors.MaxError);
            _graphControl.DrawLines(errors.ErrorsU, ErrorUPen);
            _graphControl.DrawLines(errors.ErrorsV, ErrorVPen);
         }

         _graphControl.Invalidate();
      }

      private void displayVelocityField(BarotropicComponentResult result)
      {
         _graphControl.Clear();
         _graphControl.DrawVelocityField(result.ExactSolution, ExactSolutionPen);
         _graphControl.DrawVelocityField(result.CalculatedSolution, CalculatedSolutionPen);
         _graphControl.Invalidate();
      }

      private static void displayVelocityField(SquareVelocityField velocityField, Pen pen,
                                               GraphControl graphControl, PaletteControl paletteControl)
      {
         graphControl.Clear();
         graphControl.DrawVelocityField(velocityField, PaletteDrawingTools, pen);
         graphControl.Invalidate();

         paletteControl.MinValue = velocityField.GetMinVector().Length;
         paletteControl.MaxValue = velocityField.GetMaxVector().Length;
         paletteControl.Invalidate();
      }

      private static string getStatusName(IterationStatus status)
      {
         switch (status)
         {
            case IterationStatus.Circularity:
               return "Метод зацикливается";

            case IterationStatus.Convergence:
               return "Метод сходится";

            case IterationStatus.Divergence:
               return "Метод расходится";

            default:
               return "Нужно увеличить число итераций";
         }
      }

      private void setButtonsAccessibility(bool isEnabled)
      {
         _buttonStep.Enabled = isEnabled;
         _buttonReset.Enabled = isEnabled;
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

      private SchemeType getSchemeType()
      {
         switch ((string) _comboBoxScheme.SelectedItem)
         {
            case DifferentialScheme1:
               return SchemeType.DifferentialScheme1;

            case DifferentialScheme2:
               return SchemeType.DifferentialScheme2;

            default: // IntegroInterpolatingScheme.
               return SchemeType.IntegroInterpolatingScheme;
         }
      }

      private double readTau()
      {
         return readDoubleValue(_textBoxTau.Text, "Tau");
      }

      private int readN()
      {
         return readIntValue(_textBoxN.Text, "N");
      }

      private int readM()
      {
         return readIntValue(_textBoxM.Text, "M");
      }

      private double readSigma()
      {
         return readDoubleValue(_textBoxSigma.Text, "Sigma");
      }

      private double readDelta()
      {
         return readDoubleValue(_textBoxDelta.Text, "Delta");
      }

      private int readK()
      {
         return readIntValue(_textBoxK.Text, "K");
      }

      private double readTheta()
      {
         const double max = 1.0;
         var min = (string) _comboBoxScheme.SelectedItem == DifferentialScheme1 ? 0.0 : -1.0;
         var value = readDoubleValue(_textBoxTheta.Text, "Theta");

         if (value >= min && value <= max)
            return value;

         var message = string.Format(Resources.InvalidParameterValueBoundsBroken, "Theta", min, max);
         throw new InvalidFieldValueException(message);
      }

      private double readChi()
      {
         const double min = -1.0;
         const double max = 1.0;
         var value = readDoubleValue(_textBoxChi.Text, "Chi");

         if (value >= min && value <= max)
            return value;

         var message = string.Format(Resources.InvalidParameterValueBoundsBroken, "Chi", min, max);
         throw new InvalidFieldValueException(message);
      }
   }
}
