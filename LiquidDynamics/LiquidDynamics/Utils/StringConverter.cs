using System;
using System.Globalization;

namespace LiquidDynamics.Utils
{
   public static class StringConverter
   {
      private static readonly CultureInfo InvCult = CultureInfo.InvariantCulture;

      private const NumberStyles IntFmt = NumberStyles.AllowLeadingSign;

      private const NumberStyles DoubleFmt = NumberStyles.AllowDecimalPoint |
                                             NumberStyles.AllowLeadingSign;

      public static int ToInt(string str, string name)
      {
         int value;

         if (int.TryParse(str, IntFmt, InvCult, out value))
            return value;

         throw new FormatException($"Параметр \'{name}\' имеет неверный формат.");
      }

      public static double ToDouble(string str, string name)
      {
         double value;

         if (double.TryParse(str, DoubleFmt, InvCult, out value))
            return value;

         throw new FormatException($"Параметр \'{name}\' имеет неверный формат.");
      }
   }
}
