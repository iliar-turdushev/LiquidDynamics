using LiquidDynamics.MathModel.TestProblem;

namespace LiquidDynamics.MathModel
{
   public static class DimensionConverter
   {
      public const double L0 = 1E+7;            // см
      public const double H0S = 3 * 1E+4;       // см
      public const double T0S = 1E+4;           // с
      public const double U0S = 5;              // см/с
      public const double L0S = 1E-4;           // 1/с
      public const double Rho0S = 1;            // г/см^3
      public const double W0 = 10;              // см/с
      public const double Gamma = 3.25 * 1E-6;  // г/см^3

      public const double TauCoef = Rho0S * U0S * H0S * L0S; // г/(см*с^2)
      
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

      // [h] = см
      // [out] = 1
      public static double DimlH(double h) => h / H0S;

      // [t] = с
      // [out] = 1
      public static double DimlT(double t) => t / T0S;

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
      
      // [wind] = 1
      // [out] = см/с
      public static Wind DimWind(Wind wind)
      {
         double[,] wx = new double[wind.Nx, wind.Ny];
         double[,] wy = new double[wind.Nx, wind.Ny];

         for (int i = 0; i < wind.Nx; i++)
         {
            for (int j = 0; j < wind.Ny; j++)
            {
               wx[i, j] = W0 * wind.Wx[i, j];
               wy[i, j] = W0 * wind.Wy[i, j];
            }
         }

         return new Wind(wx, wy, wind.Nx, wind.Ny);
      }

      // [mu] = 1/c
      // [out] = 1
      public static double DimlMu(double mu) => mu / L0S;

      // [b] = 1
      // [out] = см/с
      public static Barotropic DimBarot(Barotropic barot)
      {
         double[,] u = new double[barot.Nx, barot.Ny];
         double[,] v = new double[barot.Nx, barot.Ny];

         for (int i = 0; i < barot.Nx; i++)
         {
            for (int j = 0; j < barot.Ny; j++)
            {
               u[i, j] = U0S * barot.U[i, j];
               v[i, j] = U0S * barot.V[i, j];
            }
         }

         return new Barotropic(u, v, barot.Nx, barot.Ny);
      }
   }
}
