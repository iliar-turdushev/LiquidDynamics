using System;
using System.Drawing;
using Common;

namespace ControlLibrary.Types
{
   public sealed class UpwellingData
   {
      private float? _minIntensity;
      private float? _maxIntensity;

      public UpwellingData(PointF[,] gridPoints, float[,] intensities, SizeF cellSize)
      {
         Check.NotNull(gridPoints, "gridPoints");
         Check.NotNull(intensities, "intensities");
         Check.NotNull(cellSize, "cellSize");

         GridPoints = gridPoints;
         Intensities = intensities;
         CellSize = cellSize;
      }

      public PointF[,] GridPoints { get; private set; }
      public float[,] Intensities { get; private set; }
      public SizeF CellSize { get; private set; }

      public float GetMinIntensity()
      {
         if (!_minIntensity.HasValue)
            _minIntensity = getExtremumIntensity((lhs, rhs) => lhs > rhs);
         return _minIntensity.Value;
      }

      public float GetMaxIntensity()
      {
         if (!_maxIntensity.HasValue)
            _maxIntensity = getExtremumIntensity((lhs, rhs) => lhs < rhs);
         return _maxIntensity.Value;
      }

      private float getExtremumIntensity(Func<float, float, bool> predicate)
      {
         var n = Intensities.GetLength(0);
         var m = Intensities.GetLength(1);
         var result = Intensities[0, 0];

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               if (predicate(result, Intensities[i, j]))
                  result = Intensities[i, j];
            }
         }

         return result;
      }
   }
}