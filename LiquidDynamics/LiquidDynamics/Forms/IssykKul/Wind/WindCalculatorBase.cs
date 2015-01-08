using System;
using System.Drawing;
using BarotropicComponentProblem.IssykKulGrid;
using Common;
using ControlLibrary.Types;
using Point3D = Mathematics.MathTypes.Point3D;

namespace LiquidDynamics.Forms.IssykKul.Wind
{
   internal abstract class WindCalculatorBase
   {
      private const double Gamma = 3.25 * 10E-6; // г/см^3
      private const double W0 = 100; // см/с
      private const double Rho0 = 1; // г/см^3
      private const double U0 = 5; // см/с
      private const double H0 = 3 * 10E+4; // см
      private const double L0 = 10E-4; // 1/с

      private const double K = -Gamma * W0 * W0 / (Rho0 * U0 * H0 * L0);

      protected const double Pi = Math.PI;
      protected const double Lx = 175.67;

      private static readonly Func<double, double> Sqr = x => x * x;
      private static readonly Func<double, double, double> Pow = (x, y) => Math.Pow(x, y);
      
      protected static readonly Func<double, double> Sin = Math.Sin;

      private readonly IssykKulGrid2D _issykKulGrid;

      protected WindCalculatorBase(IssykKulGrid2D issykKulGrid, WindParameters windParameters)
      {
         Check.NotNull(issykKulGrid, "issykKulGrid");
         Check.NotNull(windParameters, "windParameters");

         _issykKulGrid = issykKulGrid;
         WindParameters = windParameters;
      }
      
      internal Vector[,] CalculateWindVectors()
      {
         int n = _issykKulGrid.N;
         int m = _issykKulGrid.M;

         var vectors = new Vector[n, m];

         for (int i = 0; i < n; i++)
         {
            for (int j = 0; j < m; j++)
            {
               GridCell gridCell = _issykKulGrid[i, j];

               if (gridCell == GridCell.Empty)
                  continue;

               Point3D point = gridCell.P(1, 1);

               double x = point.X;
               double y = point.Y;

               double tauX = TauX(x, y);
               double tauY = TauY(x, y);
               double windX = calculateWindX(tauX, tauY);
               double windY = calculateWindY(tauX, tauY);

               vectors[i, j] = new Vector(new PointF((float) x, (float) y),
                                          new PointF((float) windX, (float) windY));
            }
         }

         return vectors;
      }

      internal abstract double TauX(double x, double y);
      internal abstract double TauY(double x, double y);

      protected WindParameters WindParameters { get; private set; }

      private static double calculateWindX(double tauX, double tauY)
      {
         return tauX / (K * Pow((Sqr(tauX) + Sqr(tauY)) / Sqr(K), 0.25));
      }

      private static double calculateWindY(double tauX, double tauY)
      {
         return tauY / (K * Pow((Sqr(tauX) + Sqr(tauY)) / Sqr(K), 0.25));
      }
   }
}