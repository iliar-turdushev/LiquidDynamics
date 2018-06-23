using System.Drawing;

namespace ControlLibrary
{
   internal sealed class LegendItem
   {
      internal LegendItem(string text, Pen pen)
      {
         Text = text;
         Pen = pen;
      }

      internal string Text { get; private set; }
      internal Pen Pen { get; private set; }
   }
}