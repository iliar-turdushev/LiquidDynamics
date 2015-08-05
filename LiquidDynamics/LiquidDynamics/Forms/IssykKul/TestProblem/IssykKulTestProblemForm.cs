using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Common;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Forms.IssykKul.Wind;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.IssykKul.TestProblem
{
   public partial class IssykKulTestProblemForm : Form
   {
      private const string WindType1 = "Вариант 1";
      private const string WindType2 = "Вариант 2";
      private const string WindType3 = "Вариант 3";
      private const string WindType4 = "Вариант 4";
      private const string WindType5 = "Вариант 5";
      private const string WindType6 = "Вариант 6";
      private const string WindType7 = "Вариант 7";
      private const string WindType8 = "Вариант 8";
      private const string WindType9 = "Вариант 9";

      private readonly Parameters _problemParameters;
      private static readonly IPaletteDrawingTools PaletteDrawingTools = PaletteFactory.CreateBluePalette();

      private static readonly Pen SolutionPen = new Pen(Color.Red, 1) {EndCap = LineCap.ArrowAnchor};

      private readonly IssykKulVelocityFieldFormDataProvider _dataProvider;

      private bool _dynamicsAlive;

      public IssykKulTestProblemForm(Parameters problemParameters)
      {
         Check.NotNull(problemParameters, "problemParameters");

         _problemParameters = problemParameters;
         _dataProvider = new IssykKulVelocityFieldFormDataProvider();

         InitializeComponent();
         initializeComboBoxWindType();
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         _buttonStartStop.Enabled = true;
         _buttonStep.Enabled = true;

         try
         {
            Solution solution = _dataProvider.Reset(readN(), readM(), readDz(), readTau(), readTheta(),
                                                    readSigma(), readDelta(), readK(),
                                                    _problemParameters, getWindParameters());
            drawVelocityField(solution);
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void buttonStartStopClick(object sender, EventArgs e)
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

      private void buttonStepClick(object sender, EventArgs e)
      {
         step();
      }

      private void timerTick(object sender, EventArgs e)
      {
         step();
      }

      private void step()
      {
         Solution solution = _dataProvider.Step();
         drawVelocityField(solution);
      }

      private void initializeComboBoxWindType()
      {
         _comboBoxWindType.Items.Add(WindType1);
         _comboBoxWindType.Items.Add(WindType2);
         _comboBoxWindType.Items.Add(WindType3);
         _comboBoxWindType.Items.Add(WindType4);
         _comboBoxWindType.Items.Add(WindType5);
         _comboBoxWindType.Items.Add(WindType6);
         _comboBoxWindType.Items.Add(WindType7);
         _comboBoxWindType.Items.Add(WindType8);
         _comboBoxWindType.Items.Add(WindType9);
         _comboBoxWindType.SelectedItem = WindType6;
      }

      private WindParameters getWindParameters()
      {
         switch ((string) _comboBoxWindType.SelectedItem)
         {
            case WindType1:
               return TypicalWindParameters.WindType1;

            case WindType2:
               return TypicalWindParameters.WindType2;

            case WindType3:
               return TypicalWindParameters.WindType3;

            case WindType4:
               return TypicalWindParameters.WindType4;

            case WindType5:
               return TypicalWindParameters.WindType5;

            case WindType6:
               return TypicalWindParameters.WindType6;

            case WindType7:
               return TypicalWindParameters.WindType7;

            case WindType8:
               return TypicalWindParameters.WindType8;

            default:
               return TypicalWindParameters.WindType9;
         }
      }

      private void drawVelocityField(Solution solution)
      {
         Bounds bounds = solution.Bounds;
         SizeF cellSize = solution.VelocityField.CellSize;

         _graphControl.Caption = string.Format("Time = {0:F4}", _dataProvider.Time);

         _graphControl.AxisBounds = new Bounds(bounds.XMin - cellSize.Width / 2F,
                                               bounds.XMax + cellSize.Width / 2F,
                                               bounds.YMin - cellSize.Height / 2F,
                                               bounds.YMax + cellSize.Height / 2F);

         _graphControl.Clear();
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

      private double readDz()
      {
         return readDoubleValue(_textBoxDz.Text, "Dz");
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

      private void setButtonsAccessibility(bool enabled)
      {
         _buttonReset.Enabled = enabled;
         _buttonStep.Enabled = enabled;
      }
   }
}
