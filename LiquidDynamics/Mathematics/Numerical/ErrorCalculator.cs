using System;
using Common;
using Mathematics.MathTypes;

namespace Mathematics.Numerical
{
   public static class ErrorCalculator
   {
      public static double Calculate(SquareGridFunction exact, SquareGridFunction fact)
      {
         Check.NotNull(exact, "exact");
         Check.NotNull(fact, "fact");

         var max = 0.0;
         var maxDiff = 0.0;

         for (var i = 0; i < exact.N; i++)
         {
            for (var j = 0; j < exact.M; j++)
            {
               max = Math.Max(max, Math.Abs(exact[i, j]));
               maxDiff = Math.Max(maxDiff, Math.Abs(exact[i, j] - fact[i, j]));
            }
         }

         return maxDiff / max * 100;
      }

      public static void Calculate(
         Complex[] exact, Complex[] fact,
         out double errorRe, out double errorIm
         )
      {
         double maxRe = Math.Abs(exact[0].Re);
         double maxIm = Math.Abs(exact[0].Im);
         double maxDiffRe = Math.Abs(exact[0].Re - fact[0].Re);
         double maxDiffIm = Math.Abs(exact[0].Im - fact[0].Im);

         for (int i = 1; i < exact.Length; i++)
         {
            maxRe = Math.Max(maxRe, Math.Abs(exact[i].Re));
            maxIm = Math.Max(maxIm, Math.Abs(exact[i].Im));
            maxDiffRe = Math.Max(maxDiffRe, Math.Abs(exact[i].Re - fact[i].Re));
            maxDiffIm = Math.Max(maxDiffIm, Math.Abs(exact[i].Im - fact[i].Im));
         }

         errorRe = maxDiffRe / maxRe * 100;
         errorIm = maxDiffIm / maxIm * 100;
      }
   }
}