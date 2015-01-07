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
      protected const double Pi = Math.PI;
      protected const double Lx = 175.67;

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

               vectors[i, j] = new Vector(new PointF((float) x, (float) y),
                                          new PointF((float) TauX(x, y), (float) TauY(x, y)));
            }
         }

         return vectors;
      }

      internal abstract double TauX(double x, double y);
      internal abstract double TauY(double x, double y);

      protected WindParameters WindParameters { get; private set; }
   }
}