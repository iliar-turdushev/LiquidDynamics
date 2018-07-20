using System;
using static System.Math;
using static LiquidDynamics.MathModel.DimensionConverter;

namespace LiquidDynamics.MathModel.TestProblem
{
   public class Wind
   {
      private static readonly double Coef = Sqrt(Rho0S * U0S * H0S * L0S / (Gamma * W0 * W0));

      public Wind(double[,] wx, double[,] wy, int nx, int ny)
      {
         if (wx == null)
            throw new ArgumentNullException(nameof (wx));

         if (wy == null)
            throw new ArgumentNullException(nameof (wy));

         if (nx <= 1)
            throw new ArgumentException("nx <= 1", nameof (nx));

         if (ny <= 1)
            throw new ArgumentException("ny <= 1", nameof (ny));

         Wx = wx;
         Wy = wy;
         Nx = nx;
         Ny = ny;
      }

      // [Wx] = 1
      public double[,] Wx { get; }

      // [Wy] = 1
      public double[,] Wy { get; }

      public int Nx { get; }
      public int Ny { get; }

      // [tau] = 1
      // [out] = 1
      public static Wind Create(Tau tau)
      {
         double[,] wx = new double[tau.Nx, tau.Ny];
         double[,] wy = new double[tau.Nx, tau.Ny];

         for (int i = 0; i < tau.Nx; i++)
         {
            for (int j = 0; j < tau.Ny; j++)
            {
               double tx = tau.TauX[i, j];
               double ty = tau.TauY[i, j];
               double c = Coef / Pow(tx * tx + ty * ty, 0.25);

               wx[i, j] = c * tx;
               wy[i, j] = c * ty;
            }
         }

         return new Wind(wx, wy, tau.Nx, tau.Ny);
      }
   }
}