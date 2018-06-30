using System;
using System.Drawing;

namespace ControlLibrary.Graphs
{
   internal abstract class PaletteBase : IPaletteDrawingTools
   {
      protected Color[] Colors;
      protected Pen[] Pens;
      protected Brush[] Brushes;

      public abstract int MinDensity { get; }
      public abstract int MaxDensity { get; }

      public void Initialize()
      {
         generatePalette();
      }

      public Color GetColor(int density)
      {
         return Colors[density];
      }

      public Brush GetBrush(int density)
      {
         return Brushes[density];
      }

      public Pen GetPen(int density)
      {
         return Pens[density];
      }

      public int ToDensity(float value, float minValue, float maxValue)
      {
         float temp = maxValue - minValue;

         if (Math.Abs(temp) < float.Epsilon)
            return MinDensity;

         return (int) (MaxDensity * (value - minValue) / temp);
      }

      protected abstract void generatePalette();
   }
}