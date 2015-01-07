using Mathematics.MathTypes;

namespace BarotropicComponentProblem
{
   public sealed class BarotropicComponent
   {
      public BarotropicComponent(Vector[,] vectors)
      {
         Vectors = vectors;
      }

      public Vector[,] Vectors { get; private set; }
   }
}