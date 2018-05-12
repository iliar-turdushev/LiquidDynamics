using LiquidDynamics.MathModel.TestProblem;

namespace LiquidDynamics.MathModel
{
   public static class DimensionConverter
   {
      private const double L0 = 1E+7;        // см
      private const double H0S = 3 * 1E+4;   // см
      private const double U0S = 5;          // см/с
      private const double L0S = 1E-4;       // 1/с
      private const double Rho0S = 1;        // г/см^3

      private const double TauCoef = Rho0S * U0S * H0S * L0S; // г/(см*с^2)

      // [len] = 1
      // [out] = см
      public static double DimLen(double len) => len * L0;

      // [g] = 1
      // [out] = см
      public static Grid DimLen(Grid g)
      {
         double max = DimLen(g[g.N - 1]);
         return new Grid(max, g.N);
      }
      
      // [len] = см
      // [out] = 1
      public static double DimlLen(double len) => len / L0;

      // [tau] = 1
      // [out] = г/(см*с^2)
      public static double DimTau(double tau) => tau * TauCoef;

      // [tau] = 1
      // [out] = г/(см*с^2)
      public static Tau DimTau(Tau tau)
      {
         double[,] tauX = new double[tau.Nx, tau.Ny];
         double[,] tauY = new double[tau.Nx, tau.Ny];

         for (int i = 0; i < tau.Nx; i++)
         {
            for (int j = 0; j < tau.Ny; j++)
            {
               tauX[i, j] = DimTau(tau.TauX[i, j]);
               tauY[i, j] = DimTau(tau.TauY[i, j]);
            }
         }

         return new Tau(tauX, tauY, tau.Nx, tau.Ny);
      }

      // [tau] = г/(см*с^2)
      // [out] = 1
      public static double DimlTau(double tau) => tau / TauCoef;
   }
}
