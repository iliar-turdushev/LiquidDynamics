using System.Windows.Controls;
using LiquidDynamics.Forms.ParametersForms;

namespace LiquidDynamics.Controls
{
   internal partial class GraphParametersControl : UserControl
   {
      public GraphParametersControl(GraphParameters parameters)
      {
         InitializeComponent();
         DataContext = parameters;
      }
   }
}
