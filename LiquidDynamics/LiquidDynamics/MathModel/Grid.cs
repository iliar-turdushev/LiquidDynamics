using System;

namespace LiquidDynamics.MathModel
{
   public class Grid
   {
      private readonly double[] _vals;

      public Grid(double max, int n)
      {
         if (max <= 0)
            throw new ArgumentException("max <= 0", nameof (max));

         if (n <= 1)
            throw new ArgumentException("n <= 1", nameof (n));

         H = max / (n - 1);

         _vals = new double[n];
         _vals[0] = 0;
         _vals[n - 1] = max;

         double v = H;

         for (int i = 1; i < n - 1; i++)
         {
            _vals[i] = v;
            v += H;
         }
      }
      
      public double H { get; }

      public int N => _vals.Length;

      public double this[int i] => _vals[i];
   }
}
