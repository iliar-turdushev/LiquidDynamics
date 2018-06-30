using System;
using System.Drawing;

namespace ControlLibrary.Graphs
{
   internal sealed class BluePalette : PaletteBase
   {
      private const int MaxDensityValue = (Byte.MaxValue + 1) * 3 - 1;

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
            value => Color.FromArgb(0, 0, value));

         generatePalette(
            Byte.MaxValue + 1,
            2 * Byte.MaxValue + 2,
            value => Color.FromArgb(0, value - Byte.MaxValue - 1, 255));

         generatePalette(
            2 * Byte.MaxValue + 2,
            size,
            value => Color.FromArgb(value - 2 * Byte.MaxValue - 2, 255, 255));
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