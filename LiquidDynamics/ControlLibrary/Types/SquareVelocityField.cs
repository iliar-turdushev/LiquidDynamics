using System;
using System.Drawing;
using Common;

namespace ControlLibrary.Types
{
   public sealed class SquareVelocityField
   {
      private Vector _minVector;
      private Vector _maxVector;

      public SquareVelocityField(Vector[,] vectors, SizeF cellSize)
      {
         Check.NotNull(vectors, "vectors");

         Vectors = vectors;
         CellSize = cellSize;
      }

      public SquareVelocityField(Vector[,] vectors, float width, float height)
      {
         if (vectors == null)
            throw new ArgumentNullException(nameof (vectors));

         Vectors = vectors;
         CellSize = new SizeF(width, height);
      }

      public Vector[,] Vectors { get; private set; }
      public SizeF CellSize { get; private set; }

      public Vector GetMinVector()
      {
         return _minVector ?? (_minVector = getMinVector());
      }

      public Vector GetMaxVector()
      {
         return _maxVector ?? (_maxVector = getMaxVector());
      }

      private Vector getMinVector()
      {
         return getExtremumVector((lhs, rhs) => lhs.Length > rhs.Length);
      }
      
      private Vector getMaxVector()
      {
         return getExtremumVector((lhs, rhs) => lhs.Length < rhs.Length);
      }

      private Vector getExtremumVector(Func<Vector, Vector, bool> predicate)
      {
         int n = Vectors.GetLength(0);
         int m = Vectors.GetLength(1);
         Vector result = null;

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               if (Vectors[i, j] == null)
                  continue;

               if (result == null)
               {
                  result = Vectors[i, j];
                  continue;
               }

               if (predicate(result, Vectors[i, j]))
                  result = Vectors[i, j];
            }
         }

         return result;
      }
   }
}