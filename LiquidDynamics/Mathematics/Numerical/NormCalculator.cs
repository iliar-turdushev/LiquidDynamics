using System;

namespace Mathematics.Numerical
{
   public static class NormCalculator
   {
      public static double Calculate(SquareGridFunction u, SquareGridFunction v)
      {
         var max = 0.0;
         
         for (var i = 0; i < u.N; i++)
         {
            for (var j = 0; j < u.M; j++)
            {
               max = Math.Max(max, Math.Abs(u[i, j] - v[i, j]));
            }
         }

         return max;
      }
   }
}