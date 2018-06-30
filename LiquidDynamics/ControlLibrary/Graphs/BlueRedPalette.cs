using System;
using System.Drawing;

namespace ControlLibrary.Graphs
{
   internal sealed class BlueRedPalette : PaletteBase
   {
      private const int MaxDensityValue = (Byte.MaxValue + 1) * 2 - 1;

      public override int MinDensity
      {
         get { return 0; }
      }

      public override int MaxDensity
      {
         get { return MaxDensityValue; }
      }

      protected override void generatePalette()
      {
         var size = MaxDensity + 1;
         Colors = new Color[size];
         Pens = new Pen[size];
         Brushes = new Brush[size];

         generatePalette(
            0,
            Byte.MaxValue + 1,
            value => Color.FromArgb(value, value, 255));

         generatePalette(
            Byte.MaxValue + 1,
            size,
            value => Color.FromArgb(255, 2 * Byte.MaxValue - value + 1, 2 * Byte.MaxValue - value + 1));
      }

      private void generatePalette(int minDensity, int maxDensity, Func<int, Color> colorFactory)
      {
         for (var i = minDensity; i < maxDensity; i++)
         {
            Colors[i] = colorFactory(i);
            Pens[i] = new Pen(Colors[i]);
            Brushes[i] = new SolidBrush(Colors[i]);
         }
      }
   }
}