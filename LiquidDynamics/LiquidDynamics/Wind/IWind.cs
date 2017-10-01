using System;

namespace LiquidDynamics.Wind
{
   public class Wind
   {
      public double[,] Wx { get; private set; }
      public double[,] Wy { get; private set; }
   }

   public class Tau
   {
      public double[,] TauX { get; private set; }
      public double[,] TauY { get; private set; }
   }

   public class WindCalculator
   {
      private readonly Tau _tau;

      public WindCalculator(Tau tau)
      {
         if (tau == null) throw new ArgumentNullException(nameof(tau));
         _tau = tau;
      }

      public Wind Calculate()
      {


         return new Wind();
      }
   }
}
