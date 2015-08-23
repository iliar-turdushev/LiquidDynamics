using Common;
using ControlLibrary.Types;

namespace LiquidDynamics.Forms.VerticalComponentNumerical
{
   public class VerticalComponentResult
   {
      public VerticalComponentResult(UpwellingData[] upwellingData, double error, double time)
      {
         Check.NotNull(upwellingData, "upwellingData");

         UpwellingData = upwellingData;
         Error = error;
         Time = time;
      }

      public UpwellingData[] UpwellingData { get; private set; }
      public double Error { get; private set; }
      public double Time { get; private set; }
   }
}