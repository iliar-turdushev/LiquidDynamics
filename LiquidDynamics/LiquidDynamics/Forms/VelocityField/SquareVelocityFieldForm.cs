using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Common;
using ControlLibrary.Graphs;
using LiquidDynamics.Properties;

namespace LiquidDynamics.Forms.VelocityField
{
   internal partial class SquareVelocityFieldForm : Form
   {
      private readonly SquareVelocityFieldBase _squareVelocityField;
      private readonly IPaletteDrawingTools _paletteDrawingTools;
      private readonly Pen _vectorPen;

      private bool _dynamicsActive;

      public SquareVelocityFieldForm(SquareVelocityFieldBase squareVelocityField)
      {
         Check.NotNull(squareVelocityField, "squareVelocityField");

         _squareVelocityField = squareVelocityField;
         _paletteDrawingTools = PaletteFactory.CreateBluePalette();
         _vectorPen = new Pen(Color.Crimson, 1F) {EndCap = LineCap.ArrowAnchor};

         InitializeComponent();

         _textBoxCut.Text = _squareVelocityField.Cut.ToString();
         _paletteControl.PaletteDrawingTools = _paletteDrawingTools;
         
         drawGraph();
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         _squareVelocityField.Reset();
         drawGraph();
      }

      private void buttonStartStopClick(object sender, EventArgs e)
      {
         _dynamicsActive = !_dynamicsActive;
         _timer.Enabled = _dynamicsActive;

         if (_dynamicsActive)
         {
            _buttonStartStop.Image = Resources.Pause;
            setControlsAccessibillity(false);
         }
         else
         {
            _buttonStartStop.Image = Resources.Start;
            setControlsAccessibillity(true);
         }
      }

      private void buttonStepForwardClick(object sender, EventArgs e)
      {
         makeStepForward();
      }

      private void buttonStepBackwardClick(object sender, EventArgs e)
      {
         _squareVelocityField.StepBackward();
         drawGraph();
      }

      private void timerTick(object sender, EventArgs e)
      {
         makeStepForward();
      }

      private void textBoxCutKeyPress(object sender, KeyPressEventArgs e)
      {
         switch (e.KeyChar)
         {
            case (char) Keys.Return:
               double cut;

               if (!double.TryParse(_textBoxCut.Text, out cut))
               {
                  MessageBox.Show(Resources.IncorrectValue, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }
               
               try
               {
                  _squareVelocityField.Cut = cut;
                  drawGraph();
               }
               catch (InvalidFieldValueException exception)
               {
                  MessageBox.Show(exception.Message, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
               }

               break;
         }
      }

      private void makeStepForward()
      {
         _squareVelocityField.StepForward();
         drawGraph();
      }

      private void drawGraph()
      {
         updateCaption();
         _graphControl.AxisBounds = _squareVelocityField.Bounds;

         var squareVelocityField = _squareVelocityField.GetData();
         _graphControl.Clear();
         _graphControl.DrawVelocityField(squareVelocityField, _paletteDrawingTools, _vectorPen);
         _graphControl.Invalidate();

         _paletteControl.MinValue = squareVelocityField.GetMinVector().Length;
         _paletteControl.MaxValue = squareVelocityField.GetMaxVector().Length;
         _paletteControl.Invalidate();
      }

      private void updateCaption()
      {
         _graphControl.Caption = string.Format("Срез: {0}, Время: {1}",
                                               _squareVelocityField.Cut,
                                               _squareVelocityField.Time);
      }

      private void setControlsAccessibillity(bool isEnabled)
      {
         _buttonStepForward.Enabled = isEnabled;
         _buttonStepBackward.Enabled = isEnabled;
         _buttonReset.Enabled = isEnabled;
         _textBoxCut.Enabled = isEnabled;
      }
   }
}
