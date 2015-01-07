using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Common;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Properties;

namespace LiquidDynamics.Forms.BarotropicComponent
{
   internal partial class BarotropicComponentForm : Form
   {
      private readonly BarotropicComponentDataProvider _barotropicComponent;
      private readonly IPaletteDrawingTools _paletteDrawingTools;
      private readonly Pen _vectorPen;

      private bool _dynamicsActive;

      public BarotropicComponentForm(BarotropicComponentDataProvider barotropicComponent)
      {
         Check.NotNull(barotropicComponent, "barotropicComponent");

         _barotropicComponent = barotropicComponent;
         _paletteDrawingTools = PaletteFactory.CreateBluePalette();
         _vectorPen = new Pen(Color.Crimson, 1F) {EndCap = LineCap.ArrowAnchor};

         InitializeComponent();

         _paletteControl.PaletteDrawingTools = _paletteDrawingTools;
         
         drawGraph();
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         _barotropicComponent.Reset();
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
         _barotropicComponent.StepBackward();
         drawGraph();
      }

      private void timerTick(object sender, EventArgs e)
      {
         makeStepForward();
      }

      private void makeStepForward()
      {
         _barotropicComponent.StepForward();
         drawGraph();
      }

      private void drawGraph()
      {
         updateCaption();

         var squareVelocityField = _barotropicComponent.GetData();

         var bounds = _barotropicComponent.Bounds;
         var width = squareVelocityField.CellSize.Width;
         var height = squareVelocityField.CellSize.Height;
         _graphControl.AxisBounds = new Bounds(bounds.XMin - width / 2, bounds.XMax + width / 2,
                                               bounds.YMin - height / 2, bounds.YMax + height / 2);

         _graphControl.Clear();
         _graphControl.DrawVelocityField(squareVelocityField, _paletteDrawingTools, _vectorPen);
         _graphControl.Invalidate();

         _paletteControl.MinValue = squareVelocityField.GetMinVector().Length;
         _paletteControl.MaxValue = squareVelocityField.GetMaxVector().Length;
         _paletteControl.Invalidate();
      }

      private void updateCaption()
      {
         _graphControl.Caption = string.Format("Время: {0}", _barotropicComponent.Time);
      }

      private void setControlsAccessibillity(bool isEnabled)
      {
         _buttonStepForward.Enabled = isEnabled;
         _buttonStepBackward.Enabled = isEnabled;
         _buttonReset.Enabled = isEnabled;
      }
   }
}
