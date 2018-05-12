using System.ComponentModel;

namespace LiquidDynamics.MathModel
{
   public static class MeasureUnitConverter
   {
      // [len] = mu
      // [out] = см
      public static double ToCm(double len, MeasureUnit mu)
      {
         switch (mu)
         {
            case MeasureUnit.M:
               return len * 100;

            case MeasureUnit.Km:
               return len * 100000;

            default:
               throw new InvalidEnumArgumentException(nameof (mu), (int) mu, typeof (MeasureUnit));
         }
      }

      // [len] = mu
      // [out] = км
      public static double ToKm(double len, MeasureUnit mu)
      {
         switch (mu)
         {
            case MeasureUnit.Cm:
               return len / 100000;

            default:
               throw new InvalidEnumArgumentException(nameof (mu), (int) mu, typeof (MeasureUnit));
         }
      }
   }
}
