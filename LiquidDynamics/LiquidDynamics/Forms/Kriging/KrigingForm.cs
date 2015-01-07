using System;
using System.Drawing;
using System.Windows.Forms;
using BarotropicComponentProblem.Kriging;
using ControlLibrary.Controls;
using ControlLibrary.Types;
using LiquidDynamics.Properties;

namespace LiquidDynamics.Forms.Kriging
{
   internal partial class KrigingForm : Form
   {
      private Graph3DControl _graphControl;

      public KrigingForm()
      {
         InitializeComponent();
         initializeGraphControl();

         _comboBoxVariogram.SelectedItem = _comboBoxVariogram.Items[0];
      }

      private void initializeGraphControl()
      {
         _graphControl = new Graph3DControl(ZDirection.Up)
                            {
                               Dock = DockStyle.Fill,
                               GraphBounds = new Bounds3D(-15, 15, -15, 15, -1, 15)
                            };
         _graphControl.Initialize();
         _panel.Controls.Add(_graphControl);
      }

      private void buttonInterpolateClick(object sender, EventArgs e)
      {
         try
         {
            var parameters = new KrigingParameters
                                {
                                   N = readN(),
                                   M = readM(),
                                   Nodes = readNodes(),
                                   Variogram = readVariogram()
                                };
            var kriging = new Kriging(parameters);
            var result = kriging.Interpolate();

            displayError(result.Error);
            displayResult(kriging.Surface, result.Points);
         }
         catch (InvalidFieldValueException exception)
         {
            MessageBox.Show(exception.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void labelResultClick(object sender, EventArgs e)
      {
         if (!string.IsNullOrEmpty(_labelResult.Text))
         {
            Clipboard.SetText(_labelResult.Text);
         }
      }

      private void displayError(double error)
      {
         _labelResult.Text = error.ToString();
      }

      private void displayResult(Surface3D surface, Point3D[,] points)
      {
         _graphControl.Clear();
         _graphControl.DrawSurface(surface, Color.DeepPink);

         var drawPoints = new Point3D[points.Length];
         var index = 0;

         foreach (var point in points)
         {
            drawPoints[index++] = point;
         }

         _graphControl.DrawPoints(drawPoints, Color.Cyan);
         _graphControl.Invalidate();
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

      private int readN()
      {
         return readIntValue(_textBoxN.Text, "N");
      }

      private int readM()
      {
         return readIntValue(_textBoxM.Text, "M");
      }

      private int readNodes()
      {
         return readIntValue(_textBoxNodes.Text, "Nodes");
      }

      private double readC()
      {
         return readDoubleValue(_textBoxC.Text, "C");
      }

      private double readC0()
      {
         return readDoubleValue(_textBoxC0.Text, "C0");
      }

      private double readS()
      {
         return readDoubleValue(_textBoxS.Text, "S");
      }

      private double readA()
      {
         return readDoubleValue(_textBoxA.Text, "A");
      }

      private IVariogram readVariogram()
      {
         var variogram = (string) _comboBoxVariogram.SelectedItem;

         switch (variogram)
         {
            case "Gamma1":
               return new Gamma1(readC0(), readS());

            case "Gamma2":
               return new Gamma2(readC(), readC0(), readA());

            case "Gamma3":
               return new Gamma3(readC(), readC0(), readA());

            case "Gamma4":
               return new Gamma4(readC0(), readS(), readA());

            default:
               return new Gamma5(readC0(), readS(), readA());
         }
      }

      private void comboBoxVariogramSelectedIndexChanged(object sender, EventArgs e)
      {
         var variogram = (string) _comboBoxVariogram.SelectedItem;

         switch (variogram)
         {
            case "Gamma1":
               cVisibility(false);
               c0Visibility(true);
               sVisibility(true);
               aVisibility(false);
               break;

            case "Gamma2":
            case "Gamma3":
               cVisibility(true);
               c0Visibility(true);
               sVisibility(false);
               aVisibility(true);
               break;

            case "Gamma4":
            case "Gamma5":
               cVisibility(false);
               c0Visibility(true);
               sVisibility(true);
               aVisibility(true);
               break;
         }
      }

      private void cVisibility(bool visible)
      {
         _labelC.Visible = visible;
         _textBoxC.Visible = visible;
      }

      private void c0Visibility(bool visible)
      {
         _labelC0.Visible = visible;
         _textBoxC0.Visible = visible;
      }

      private void sVisibility(bool visible)
      {
         _labelS.Visible = visible;
         _textBoxS.Visible = visible;
      }

      private void aVisibility(bool visible)
      {
         _labelA.Visible = visible;
         _textBoxA.Visible = visible;
      }
   }
}