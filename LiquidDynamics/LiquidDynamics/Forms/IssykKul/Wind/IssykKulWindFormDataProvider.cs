using System.Drawing;
using BarotropicComponentProblem.IssykKulGrid;
using ControlLibrary.Types;
using LiquidDynamics.Properties;
using Point3D = Mathematics.MathTypes.Point3D;

namespace LiquidDynamics.Forms.IssykKul.Wind
{
   internal sealed class IssykKulWindFormDataProvider
   {
      private const double Theta = 0.5;

      private readonly Point3D[] _issykKulData;
      private readonly Bounds _bounds;

      private readonly double _xMin;
      private readonly double _xMax;
      private readonly double _yMin;
      private readonly double _yMax;

      public IssykKulWindFormDataProvider()
      {
         _issykKulData = IssykKulDataReader.ReadData(Resources.IssykKulData);

         BoundsCalculator.Calculate(_issykKulData, out _xMin, out _xMax, out _yMin, out _yMax);
         _bounds = new Bounds((float) _xMin, (float) _xMax, (float) _yMin, (float) _yMax);
      }

      internal BuildWindResult BuildWind(int n, int m, WindType windType, WindParameters windParameters)
      {
         IssykKulGrid2D issykKulGrid = buildIssykKulGrid(n, m);
         SquareVelocityField velocityField = calculateWindVectorField(issykKulGrid, windType, windParameters);
         return new BuildWindResult(velocityField, _bounds);
      }

      private IssykKulGrid2D buildIssykKulGrid(int n, int m)
      {
         var gridBuilder = new IssykKulGridBuilder(_issykKulData, _xMin, _xMax, _yMin, _yMax);
         return gridBuilder.BuildGrid2D(n, m, Theta);
      }

      private static SquareVelocityField calculateWindVectorField(IssykKulGrid2D issykKulGrid, WindType windType, WindParameters windParameters)
      {
         Vector[,] vectors = calculateWindVectors(issykKulGrid, windType, windParameters);
         var cellSize = new SizeF((float) issykKulGrid.Hx, (float) issykKulGrid.Hy);
         return new SquareVelocityField(vectors, cellSize);
      }

      private static Vector[,] calculateWindVectors(IssykKulGrid2D issykKulGrid, WindType windType, WindParameters windParameters)
      {
         WindCalculatorBase windCalculator = createWindCalculator(issykKulGrid, windType, windParameters);
         return windCalculator.CalculateWindVectors();
      }

      private static WindCalculatorBase createWindCalculator(IssykKulGrid2D issykKulGrid, WindType windType, WindParameters windParameters)
      {
         switch (windType)
         {
            case WindType.Ulan:
               return new UlanWindCalculator(issykKulGrid, windParameters);

            case WindType.Santash:
               return new SantashWindCalculator(issykKulGrid, windParameters);

            default: // WindType.UlanPlusSantash
               var ulan = new UlanWindCalculator(issykKulGrid, windParameters);
               var santash = new SantashWindCalculator(issykKulGrid, windParameters);
               return new IssykKulWindCalculator(ulan, santash, issykKulGrid, windParameters);
         }
      }
   }
}