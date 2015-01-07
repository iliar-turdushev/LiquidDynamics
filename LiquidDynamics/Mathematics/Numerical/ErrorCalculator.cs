using System;
using Common;

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
   }
}