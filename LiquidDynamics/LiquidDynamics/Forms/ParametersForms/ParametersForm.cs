using System.Windows;
using System.Windows.Forms;

namespace LiquidDynamics.Forms.ParametersForms
{
   internal partial class ParametersForm : Form
   {
      public ParametersForm(UIElement child)
      {
         InitializeComponent();
         _elementHost.Child = child;
      }
   }
}
