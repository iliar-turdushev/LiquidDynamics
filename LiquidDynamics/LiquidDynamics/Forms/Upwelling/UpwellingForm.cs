using System;
using System.Windows.Forms;
using Common;
using ControlLibrary.Graphs;
using LiquidDynamics.Properties;

namespace LiquidDynamics.Forms.Upwelling
{
   internal partial class UpwellingForm : Form
   {
      private readonly UpwellingDataProvider _upwellingDataProvider;
      private readonly IPaletteDrawingTools _paletteDrawingTools;

      private bool _dynamicsActive;

      public UpwellingForm(UpwellingDataProvider upwellingDataProvider)
      {
         Check.NotNull(upwellingDataProvider, "upwellingDataProvider");
         _upwellingDataProvider = upwellingDataProvider;
         _paletteDrawingTools = PaletteFactory.CreateBlueRedPalette();
         
         InitializeComponent();

         _paletteControl.PaletteDrawingTools = _paletteDrawingTools;
         _textBoxZ.Text = _upwellingDataProvider.Z.ToString();
         
         drawGraph();
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         _upwellingDataProvider.Reset();
         drawGraph();
      }

      private void buttonStepBackwardClick(object sender, EventArgs e)
      {
         _upwellingDataProvider.StepBackward();
         drawGraph();
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

      private void textBoxZKeyPress(object sender, KeyPressEventArgs e)
      {
         switch (e.KeyChar)
         {
            case (char)Keys.Return:
               double z;

               if (!double.TryParse(_textBoxZ.Text, out z))
               {
                  MessageBox.Show(Resources.IncorrectValue, Resources.ApplicationName,
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }
               
               try
               {
                  _upwellingDataProvider.Z = z;
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

      private void timerTick(object sender, EventArgs e)
      {
         makeStepForward();
      }

      private void makeStepForward()
      {
         _upwellingDataProvider.StepForward();
         drawGraph();
      }

      private void drawGraph()
      {
         _graphControl.Caption = string.Format("Z: {0}, Время: {1}",
                                               _upwellingDataProvider.Z,
                                               _upwellingDataProvider.Time);

         var upwellingData = _upwellingDataProvider.GetData();
         _graphControl.AxisBounds = _upwellingDataProvider.Bounds;
         _graphControl.Clear();
         _graphControl.DrawUpwelling(upwellingData, _paletteDrawingTools);
         _graphControl.Invalidate();

         var value = Math.Max(Math.Abs(upwellingData.GetMinIntensity()),
                              Math.Abs(upwellingData.GetMaxIntensity()));
         _paletteControl.MinValue = -value;
         _paletteControl.MaxValue = value;
         _paletteControl.Invalidate();
      }

      private void setControlsAccessibillity(bool isEnabled)
      {
         _buttonStepForward.Enabled = isEnabled;
         _buttonStepBackward.Enabled = isEnabled;
         _buttonReset.Enabled = isEnabled;
         _textBoxZ.Enabled = isEnabled;
      }
   }
}
