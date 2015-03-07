using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Properties;

namespace LiquidDynamics.Forms.IssykKul.Wind
{
   public partial class IssykKulWindForm : Form
   {
      private const string Ulan = "Улан";
      private const string Santash = "Санташ";
      private const string UlanPlusSantash = "Улан + Санташ";

      private const string WindType1 = "Вариант 1";
      private const string WindType2 = "Вариант 2";
      private const string WindType3 = "Вариант 3";
      private const string WindType4 = "Вариант 4";
      private const string WindType5 = "Вариант 5";
      private const string WindType6 = "Вариант 6";
      private const string WindType7 = "Вариант 7";
      private const string WindType8 = "Вариант 8";
      private const string WindType9 = "Вариант 9";

      private static readonly Pen WindPen = new Pen(Color.Red, 1) {EndCap = LineCap.ArrowAnchor};
      private static readonly IPaletteDrawingTools PaletteDrawingTools = PaletteFactory.CreateBluePalette();

      private Dictionary<string, TextBox> _parameterNameToTextBox;

      private readonly IssykKulWindFormDataProvider _dataProvider;
      
      public IssykKulWindForm()
      {
         InitializeComponent();
         initializeComboBoxWind();
         initializeComboBoxWindTypes();

         fillWindParametersTextBoxes();
         initializeParameterNameToTextBoxDictionary();

         _dataProvider = new IssykKulWindFormDataProvider();
      }

      private void buttonBuildWindClick(object sender, EventArgs e)
      {
         try
         {
            WindParameters windParameters = createWindParameters();

            BuildWindResult result =
               _dataProvider.BuildWind(readN(), readM(), getWindType(), windParameters);

            drawWindField(result);
            drawPalette(result.WindField);
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void comboBoxWindTypesSelectedIndexChanged(object sender, EventArgs e)
      {
         fillWindParametersTextBoxes();
      }

      private void initializeComboBoxWind()
      {
         _comboBoxWind.Items.Add(Ulan);
         _comboBoxWind.Items.Add(Santash);
         _comboBoxWind.Items.Add(UlanPlusSantash);
         _comboBoxWind.SelectedItem = Ulan;
      }

      private void initializeComboBoxWindTypes()
      {
         _comboBoxWindTypes.Items.Add(WindType1);
         _comboBoxWindTypes.Items.Add(WindType2);
         _comboBoxWindTypes.Items.Add(WindType3);
         _comboBoxWindTypes.Items.Add(WindType4);
         _comboBoxWindTypes.Items.Add(WindType5);
         _comboBoxWindTypes.Items.Add(WindType6);
         _comboBoxWindTypes.Items.Add(WindType7);
         _comboBoxWindTypes.Items.Add(WindType8);
         _comboBoxWindTypes.Items.Add(WindType9);
         _comboBoxWindTypes.SelectedItem = WindType1;
      }

      private void fillWindParametersTextBoxes()
      {
         WindParameters windParameters = getWindParameters();

         _textBoxA1.Text = windParameters.A1.ToString();
         _textBoxA2.Text = windParameters.A2.ToString();
         _textBoxA3.Text = windParameters.A3.ToString();
         _textBoxA4.Text = windParameters.A4.ToString();

         _textBoxB1.Text = windParameters.B1.ToString();
         _textBoxB2.Text = windParameters.B2.ToString();
         _textBoxB3.Text = windParameters.B3.ToString();
         _textBoxB4.Text = windParameters.B4.ToString();

         _textBoxA1x.Text = windParameters.A1x.ToString();
         _textBoxA2x.Text = windParameters.A2x.ToString();
         _textBoxA3x.Text = windParameters.A3x.ToString();
         _textBoxA4x.Text = windParameters.A4x.ToString();

         _textBoxA1y.Text = windParameters.A1y.ToString();
         _textBoxA2y.Text = windParameters.A2y.ToString();
         _textBoxA3y.Text = windParameters.A3y.ToString();
         _textBoxA4y.Text = windParameters.A4y.ToString();

         _textBoxB1x.Text = windParameters.B1x.ToString();
         _textBoxB2x.Text = windParameters.B2x.ToString();
         _textBoxB3x.Text = windParameters.B3x.ToString();
         _textBoxB4x.Text = windParameters.B4x.ToString();

         _textBoxB1y.Text = windParameters.B1y.ToString();
         _textBoxB2y.Text = windParameters.B2y.ToString();
         _textBoxB3y.Text = windParameters.B3y.ToString();
         _textBoxB4y.Text = windParameters.B4y.ToString();
      }

      private WindParameters getWindParameters()
      {
         switch ((string) _comboBoxWindTypes.SelectedItem)
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
      
      private void initializeParameterNameToTextBoxDictionary()
      {
         _parameterNameToTextBox =
            new Dictionary<string, TextBox>
               {
                  {"A1", _textBoxA1},
                  {"A2", _textBoxA2},
                  {"A3", _textBoxA3},
                  {"A4", _textBoxA4},
                  {"B1", _textBoxB1},
                  {"B2", _textBoxB2},
                  {"B3", _textBoxB3},
                  {"B4", _textBoxB4},
                  {"a1x", _textBoxA1x},
                  {"a2x", _textBoxA2x},
                  {"a3x", _textBoxA3x},
                  {"a4x", _textBoxA4x},
                  {"a1y", _textBoxA1y},
                  {"a2y", _textBoxA2y},
                  {"a3y", _textBoxA3y},
                  {"a4y", _textBoxA4y},
                  {"b1x", _textBoxB1x},
                  {"b2x", _textBoxB2x},
                  {"b3x", _textBoxB3x},
                  {"b4x", _textBoxB4x},
                  {"b1y", _textBoxB1y},
                  {"b2y", _textBoxB2y},
                  {"b3y", _textBoxB3y},
                  {"b4y", _textBoxB4y}
               };
      }

      private WindParameters createWindParameters()
      {
         return
            new WindParameters(
               read("A1"), read("A2"), read("A3"), read("A4"),
               read("B1"), read("B2"), read("B3"), read("B4"),
               read("a1x"), read("a2x"), read("a3x"), read("a4x"),
               read("a1y"), read("a2y"), read("a3y"), read("a4y"),
               read("b1x"), read("b2x"), read("b3x"), read("b4x"),
               read("b1y"), read("b2y"), read("b3y"), read("b4y")
               );
      }

      private double read(string parameterName)
      {
         TextBox textBox = _parameterNameToTextBox[parameterName];
         return readDoubleValue(textBox.Text, parameterName);
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

      private static int readIntValue(string textToRead, string parameterName)
      {
         int value;

         if (!int.TryParse(textToRead, out value))
         {
            generateError(parameterName);
         }

         return value;
      }

      private WindType getWindType()
      {
         var selectedItem = (string) _comboBoxWind.SelectedItem;

         switch (selectedItem)
         {
            case Ulan:
               return WindType.Ulan;
            
            case Santash:
               return WindType.Santash;

            default: // UlanPlusSantash
               return WindType.UlanPlusSantash;
         }
      }

      private void drawWindField(BuildWindResult result)
      {
         _graphControl.Clear();
         _graphControl.AxisBounds = result.Bounds;
         _graphControl.DrawVelocityField(result.WindField, PaletteDrawingTools, WindPen, VectorScalingMode.ScaleEachVector);
         _graphControl.Invalidate();
      }

      private void drawPalette(SquareVelocityField windField)
      {
         _paletteControl.MinValue = windField.GetMinVector().Length;
         _paletteControl.MaxValue = windField.GetMaxVector().Length;
         _paletteControl.Invalidate();
      }
   }
}
