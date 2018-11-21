using System;

namespace LiquidDynamics.MathModel.TestProblem
{
   public sealed class Barotropic
   {
      public Barotropic(double[,] u, double[,] v, int nx, int ny)
      {
         if (u == null)
            throw new ArgumentNullException(nameof(u));

         if (v == null)
            throw new ArgumentNullException(nameof(v));

         if (nx <= 1)
            throw new ArgumentException("nx <= 1", nameof(nx));

         if (ny <= 1)
            throw new ArgumentException("ny <= 1", nameof(ny));

         U = u;
         V = v;
         Nx = nx;
         Ny = ny;
      }
      
      public double[,] U { get; }
      public double[,] V { get; }

      public int Nx { get; }
      public int Ny { get; }
   }
}