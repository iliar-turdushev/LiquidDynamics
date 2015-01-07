using System;

namespace Common
{
   public static class Check
   {
      public static void NotNull(object parameter, string name)
      {
         if (parameter == null)
            throw new ArgumentException(name);
      }
   }
}
