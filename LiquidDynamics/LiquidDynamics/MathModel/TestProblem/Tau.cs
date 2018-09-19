using System;
using static System.Math;

namespace LiquidDynamics.MathModel.TestProblem
{
   public class Tau
   {
      public Tau(double[,] tauX, double[,] tauY, int nx, int ny)
      {
         if (tauX == null)
            throw new ArgumentNullException(nameof (tauX));

         if (tauY == null)
            throw new ArgumentNullException(nameof (tauY));

         if (nx <= 1)
            throw new ArgumentException("nx <= 1", nameof (nx));

         if (ny <= 1)
            throw new ArgumentException("ny <= 1", nameof (ny));

         TauX = tauX;
         TauY = tauY;
         Nx = nx;
         Ny = ny;
      }

      // [TauX] = 1
      public double[,] TauX { get; }

      // [TauY] = 1
      public double[,] TauY { get; }

      public int Nx { get; }
      public int Ny { get; }

      // [r] = [q] = 1
      // [f1] = [f2] = 1
      // [gx] = [gy] = 1
      // [out] = 1
      public static Tau Calc(
         double r, double q,
         double f1, double f2,
         Grid gx, Grid gy
         )
      {
         if (r <= 0)
            throw new ArgumentException("r <= 0", nameof (r));

         if (q <= 0)
            throw new ArgumentException("q <= 0", nameof (q));

         double[,] tauX = new double[gx.N, gy.N];
         double[,] tauY = new double[gx.N, gy.N];

         for (int i = 0; i < gx.N; i++)
         {
            double x = gx[i];

            for (int j = 0; j < gy.N; j++)
            {
               double y = gy[j];

               tauX[i, j] = -f1 * q / PI * Cos(PI * y / q);
               tauY[i, j] = f2 * r / PI * Cos(PI * x / r) * Sin(PI * y / q);
            }
         }

         return new Tau(tauX, tauY, gx.N, gy.N);
      }
   }
}
