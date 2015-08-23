using System;
using System.Windows.Forms;
using Common;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using ModelProblem;

namespace LiquidDynamics.Forms.VerticalComponentNumerical
{
   public partial class VerticalComponentForm : Form
   {
      private readonly IPaletteDrawingTools _paletteDrawingTools;

      private readonly Parameters _parameters;
      private readonly VerticalComponentProblemSolver _solver;
      
      public VerticalComponentForm(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");
         
         _parameters = parameters;
         _solver = new VerticalComponentProblemSolver();

         _paletteDrawingTools = PaletteFactory.CreateBlueRedPalette();

         InitializeComponent();
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         UpwellingData upwellingData = _solver.Begin(100, 100, 50, 0.1, _parameters);

         _graphControl.AxisBounds = new Bounds(0, 1, 0, 1);
         _graphControl.Clear();
         _graphControl.DrawUpwelling(upwellingData, _paletteDrawingTools);
         _graphControl.Invalidate();
      }
   }
}
