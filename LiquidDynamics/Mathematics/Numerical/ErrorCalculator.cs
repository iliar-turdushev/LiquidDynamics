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
         Complex[,][] exact, Complex[,][] fact,
         out double errorRe, out double errorIm
         )
      {
         int nx = exact.GetLength(0);
         int ny = exact.GetLength(1);

         double maxRe = 0;
         double maxIm = 0;
         double maxDiffRe = 0;
         double maxDiffIm = 0;

         for (int i = 0; i < nx; i++)
         {
            for (int j = 0; j < ny; j++)
            {
               int nz = exact[i, j].Length;

               for (int k = 0; k < nz; k++)
               {
                  maxRe = Math.Max(maxRe, Math.Abs(exact[i, j][k].Re));
                  maxIm = Math.Max(maxIm, Math.Abs(exact[i, j][k].Im));
                  maxDiffRe = Math.Max(maxDiffRe, Math.Abs(exact[i, j][k].Re - fact[i, j][k].Re));
                  maxDiffIm = Math.Max(maxDiffIm, Math.Abs(exact[i, j][k].Im - fact[i, j][k].Im));
               }
            }
         }

         errorRe = maxDiffRe / maxRe * 100;
         errorIm = maxDiffIm / maxIm * 100;
      }
   }
}