using System.Windows.Controls;
using ModelProblem;

namespace LiquidDynamics.Controls
{
   internal partial class ProblemParametersControl : UserControl
   {
      public ProblemParametersControl(Parameters parameters)
      {
         InitializeComponent();
         DataContext = parameters;
      }
   }
}
