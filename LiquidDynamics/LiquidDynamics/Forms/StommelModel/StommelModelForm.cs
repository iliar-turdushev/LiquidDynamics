using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using BarotropicComponentProblem.IterationMethod;
using ControlLibrary.Types;
using LiquidDynamics.Properties;

namespace LiquidDynamics.Forms.StommelModel
{
   internal partial class StommelModelForm : Form
   {
      private const string EpsilonDefaultValue = "1/(40*pi)";

      private readonly Dictionary<string, double> _epsilonConstans =
         new Dictionary<string, double>
            {
               {"1/(20*pi)", 1/(20*Math.PI)},
               {"1/(30*pi)", 1/(30*Math.PI)},
               {"1/(40*pi)", 1/(40*Math.PI)},
               {"1/(50*pi)", 1/(50*Math.PI)},
               {"1/(60*pi)", 1/(60*Math.PI)},
               {"1/(70*pi)", 1/(70*Math.PI)},
               {"1/(80*pi)", 1/(80*Math.PI)}
            };

      private StommelModel _stommelModel;

      private readonly Pen _exactSolutionPen;
      private readonly Pen _approximatedSolutionPen;

      internal StommelModelForm()
      {
         InitializeComponent();

         var items = _epsilonConstans.Select(item => item.Key).ToArray();
         _comboBoxEpsilon.Items.AddRange(items);
         _comboBoxEpsilon.SelectedItem = EpsilonDefaultValue;

         _exactSolutionPen = new Pen(Color.Green, 1F) {EndCap = LineCap.ArrowAnchor};
         _approximatedSolutionPen = new Pen(Color.Red, 1F) {EndCap = LineCap.ArrowAnchor};
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         try
         {
            var parameters = new StommelModelParameters
                                {
                                   Epsilon = readEpsilon(),
                                   N = readN(),
                                   M = readM(),
                                   Sigma = readSigma(),
                                   Delta = readDelta(),
                                   K = readK()
                                };

            _stommelModel = new StommelModel(parameters);
            var solution = _stommelModel.InitialApproximation();
            
            var bounds = _stommelModel.Bounds;
            var width = solution.Solution.CellSize.Width;
            var height = solution.Solution.CellSize.Height;
            _graphControl.AxisBounds = new Bounds(bounds.XMin - width / 2, bounds.XMax + width / 2,
                                                  bounds.YMin - height / 2, bounds.YMax + height / 2);

            displayResult(solution.Solution, getInitialText(solution));

            _buttonSolve.Enabled = true;
         }
         catch (InvalidFieldValueException exception)
         {
            MessageBox.Show(exception.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      
      private void buttonSolveClick(object sender, EventArgs e)
      {
         var solution = _stommelModel.Solve();
         displayResult(solution.Solution, getSolveText(solution));
      }

      private void displayResult(SquareVelocityField result, string text)
      {
         _graphControl.Clear();
         _graphControl.Caption = text;
         _graphControl.DrawVelocityField(_stommelModel.Solution, _exactSolutionPen);
         _graphControl.DrawVelocityField(result, _approximatedSolutionPen);
         _graphControl.Invalidate();
      }

      private static string getInitialText(StommelModelSolutionInfo solution)
      {
         var u = string.Format("U: Относительная погрешность = {0}%", solution.U.Error);
         var v = string.Format("V: Относительная погрешность = {0}%", solution.V.Error);
         
         return u + Environment.NewLine + v;
      }

      private static string getSolveText(StommelModelSolutionInfo solution)
      {
         var u = solution.U;
         var uText = string.Format("U: Относительная погрешность = {0}%; Число итераций = {1}; Статус = {2}",
                                   u.Error, u.IterationNumber, getStatusName(u.IterationStatus));

         var v = solution.V;
         var vText = string.Format("V: Относительная погрешность = {0}%; Число итераций = {1}; Статус = {2}",
                                   v.Error, v.IterationNumber, getStatusName(v.IterationStatus));

         return uText + Environment.NewLine + vText;
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

      private static void generateError(string parameterName)
      {
         var message = string.Format(Resources.InvalidParameterValue, parameterName);
         throw new InvalidFieldValueException(message);
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

      private double readEpsilon()
      {
         var text = _comboBoxEpsilon.Text;
         double value;

         if (_epsilonConstans.TryGetValue(text, out value))
            return value;

         return readDoubleValue(text, "Epsilon");
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
   }
}
