using System.Drawing;
using Common;

namespace LiquidDynamics.Forms.BaroclinicComponent
{
   internal class SolveBaroclinicProblemResult
   {
      public SolveBaroclinicProblemResult(
         double time, 
         PointF[,][] exactU,
         PointF[,][] exactV,
         PointF[,][] calculatedU,
         PointF[,][] calculatedV, 
         double errorU, 
         double errorV
         )
      {
         Check.NotNull(exactU, "exactU");
         Check.NotNull(exactV, "exactV");
         Check.NotNull(calculatedU, "calculatedU");
         Check.NotNull(calculatedV, "calculatedV");

         Time = time;
         ExactU = exactU;
         ExactV = exactV;
         CalculatedU = calculatedU;
         CalculatedV = calculatedV;
         ErrorU = errorU;
         ErrorV = errorV;
      }

      public double Time { get; private set; }

      public PointF[,][] ExactU { get; private set; }
      public PointF[,][] ExactV { get; private set; }

      public PointF[,][] CalculatedU { get; private set; }
      public PointF[,][] CalculatedV { get; private set; }

      public double ErrorU { get; private set; }
      public double ErrorV { get; private set; }
   }
}