using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common;
using ControlLibrary.Controls;
using ControlLibrary.Types;
using LiquidDynamics.Properties;

namespace LiquidDynamics.Forms.EkmanSpiral
{
   internal partial class EkmanSpiralForm : Form
   {
      private const int InitialValue = 10;
      private const int ChangeValue = 10;

      private readonly Pen _uPen = new Pen(Color.Red, 1.0F);
      private readonly Pen _vPen = new Pen(Color.Green, 1.0F);

      private readonly EkmanSpiralDataProvider _dataProvider;
      private Graph3DControl _graphControl3D;
      private bool _dynamicsActive;

      private int _x;
      private int _y;

      public EkmanSpiralForm(EkmanSpiralDataProvider dataProvider)
      {
         Check.NotNull(dataProvider, "dataProvider");
         _dataProvider = dataProvider;

         _x = InitialValue;
         _y = InitialValue;

         InitializeComponent();
         initializeGraphControl();

         _textBoxX.Text = _dataProvider.X.ToString();
         _textBoxY.Text = _dataProvider.Y.ToString();

         _graphControl2D.AddLegend("U", _uPen);
         _graphControl2D.AddLegend("V", _vPen);
         drawEkmanSpiral();
      }

      private void initializeGraphControl()
      {
         _graphControl3D = new Graph3DControl(ZDirection.Down) {Dock = DockStyle.Fill};
         _graphControl3D.Initialize();

         _splitContainer.Panel1.Controls.Add(_graphControl3D);
      }

      private void drawEkmanSpiral()
      {
         var graphs = _dataProvider.GetData();

         // Graph3D.
         var curve = graphs.UV;
         _graphControl3D.GraphBounds = new Bounds3D(-_x, _x, -_y, _y, 0, (float) _dataProvider.H);
         _graphControl3D.Clear();
         _graphControl3D.DrawCurve(curve, Color.Cyan);

         var lines =
            curve
               .Points
               .Select(point => new Line3D(point, new Point3D(0, 0, point.Z)))
               .ToArray();
         _graphControl3D.DrawLines(lines, Color.DodgerBlue);

         _graphControl3D.Invalidate();

         // Graph2D.
         _graphControl2D.AxisBounds = new Bounds(0, (float) _dataProvider.H, -_y, _y);
         _graphControl2D.Clear();
         _graphControl2D.DrawCurve(graphs.U, _uPen);
         _graphControl2D.DrawCurve(graphs.V, _vPen);
         _graphControl2D.Invalidate();

         _labelStatus.Text = string.Format("X: {0}; Y: {1}; Время: {2}",
                                           _dataProvider.X, _dataProvider.Y, _dataProvider.Time);
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         _dataProvider.Reset();
         drawEkmanSpiral();
      }

      private void buttonStepBackwardClick(object sender, EventArgs e)
      {
         _dataProvider.StepBackward();
         drawEkmanSpiral();
      }

      private void buttonStartPauseClick(object sender, EventArgs e)
      {
         _dynamicsActive = !_dynamicsActive;
         _timer.Enabled = _dynamicsActive;

         if (_dynamicsActive)
         {
            _buttonStartPause.Image = Resources.Pause;
            setControlsAccessibillity(false);
         }
         else
         {
            _buttonStartPause.Image = Resources.Start;
            setControlsAccessibillity(true);
         }
      }

      private void buttonStepForwardClick(object sender, EventArgs e)
      {
         makeStepForward();
      }

      private void timerTick(object sender, EventArgs e)
      {
         makeStepForward();
      }
      
      private void textBoxXKeyPress(object sender, KeyPressEventArgs e)
      {
         processKeyPress(e, _textBoxX.Text, value => _dataProvider.X = value);
      }
      
      private void textBoxYKeyPress(object sender, KeyPressEventArgs e)
      {
         processKeyPress(e, _textBoxY.Text, value => _dataProvider.Y = value);
      }

      private void buttonMinusClick(object sender, EventArgs e)
      {
         var needRedraw = false;

         if (_x != InitialValue)
         {
            _x -= ChangeValue;
            needRedraw = true;
         }

         if (_y != InitialValue)
         {
            _y -= ChangeValue;
            needRedraw = true;
         }

         if (needRedraw)
            drawEkmanSpiral();
      }

      private void buttonPlusClick(object sender, EventArgs e)
      {
         _x += ChangeValue;
         _y += ChangeValue;
         drawEkmanSpiral();
      }

      private void makeStepForward()
      {
         _dataProvider.StepForward();
         drawEkmanSpiral();
      }

      private void setControlsAccessibillity(bool isEnabled)
      {
         _buttonStepForward.Enabled = isEnabled;
         _buttonStepBackward.Enabled = isEnabled;
         _buttonReset.Enabled = isEnabled;
         _textBoxX.Enabled = isEnabled;
         _textBoxY.Enabled = isEnabled;
      }

      private void processKeyPress(KeyPressEventArgs args, string text, Action<double> setter)
      {
         switch (args.KeyChar)
         {
            case (char) Keys.Return:
               double value;

               if (!double.TryParse(text, out value))
               {
                  MessageBox.Show(Resources.IncorrectValue, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }

               try
               {
                  setter(value);
                  drawEkmanSpiral();
               }
               catch (InvalidFieldValueException exception)
               {
                  MessageBox.Show(exception.Message, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
               
               break;
         }
      }
   }
}
