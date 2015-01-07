using System.Drawing;

namespace ControlLibrary.Drawing
{
   public interface IPaletteDrawingTools
   {
      Color GetColor(int density);
      Brush GetBrush(int density);
      Pen GetPen(int density);
      int ToDensity(float value, float minValue, float maxValue);
      int MinDensity { get; }
      int MaxDensity { get; }
   }
}