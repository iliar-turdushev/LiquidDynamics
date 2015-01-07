using System;

namespace Mathematics.Helpers
{
   public static class DoubleHelper
   {
      public static bool IsZero(this double value)
      {
         return Math.Abs(value) < double.Epsilon;
      }
   }
}