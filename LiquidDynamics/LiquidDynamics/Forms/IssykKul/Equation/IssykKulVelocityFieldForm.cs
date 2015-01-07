using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Common;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.IssykKul.Equation
{
   public partial class IssykKulVelocityFieldForm : Form
   {
      private readonly Parameters _problemParameters;
      private static readonly IPaletteDrawingTools PaletteDrawingTools = PaletteFactory.CreateBluePalette();

      private static readonly Pen SolutionPen = new Pen(Color.Red, 1) {EndCap = LineCap.ArrowAnchor};

      private readonly IssykKulVelocityFieldFormDataProvider _dataProvider;

      public IssykKulVelocityFieldForm(Parameters problemParameters)
      {
         Check.NotNull(problemParameters, "problemParameters");

         _problemParameters = problemParameters;
         _dataProvider = new IssykKulVelocityFieldFormDataProvider();

         InitializeComponent();
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         _timer.Enabled = false;

         try
         {
            Solution solution = _dataProvider.Reset(readN(), readM(), readTau(), readTheta(),
                                                    readSigma(), readDelta(), readK(),
                                                    _problemParameters);
            drawVelocityField(solution);
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void buttonStepClick(object sender, EventArgs e)
      {
         _timer.Enabled = true;
      }

      private void timerTick(object sender, EventArgs e)
      {
         Solution solution = _dataProvider.Step();
         drawVelocityField(solution);
      }

      private void drawVelocityField(Solution solution)
      {
         Bounds bounds = solution.Bounds;
         SizeF cellSize = solution.VelocityField.CellSize;

         _graphControl.AxisBounds = new Bounds(bounds.XMin - cellSize.Width / 2F,
                                               bounds.XMax + cellSize.Width / 2F,
                                               bounds.YMin - cellSize.Height / 2F,
                                               bounds.YMax + cellSize.Height / 2F);

         _graphControl.DrawVelocityField(solution.VelocityField, PaletteDrawingTools, SolutionPen,
                                         VectorScalingMode.ScaleEachVector);
         _graphControl.Invalidate();

         _paletteControl.MinValue = solution.VelocityField.GetMinVector().Length;
         _paletteControl.MaxValue = solution.VelocityField.GetMaxVector().Length;
         _paletteControl.Invalidate();
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

      private static double readDoubleValue(string textToRead, string parameterName)
      {
         double value;

         if (!double.TryParse(textToRead, out value))
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

      private int readN()
      {
         return readIntValue(_textBoxN.Text, "N");
      }

      private int readM()
      {
         return readIntValue(_textBoxM.Text, "M");
      }

      private double readTau()
      {
         return readDoubleValue(_textBoxTau.Text, "Tau");
      }

      private double readTheta()
      {
         return readDoubleValue(_textBoxTheta.Text, "Theta");
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
