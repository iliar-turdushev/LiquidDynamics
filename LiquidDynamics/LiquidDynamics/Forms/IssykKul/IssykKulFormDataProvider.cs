using System.Collections.Generic;
using System.Linq;
using BarotropicComponentProblem.IssykKulGrid;
using ControlLibrary.Types;
using LiquidDynamics.Properties;
using Point3D = Mathematics.MathTypes.Point3D;
using GraphPoint3D = ControlLibrary.Types.Point3D;

namespace LiquidDynamics.Forms.IssykKul
{
   internal sealed class IssykKulFormDataProvider
   {
      private IssykKulGridBuilder _gridBuilder;

      internal IssykKulDisplayData Initialize(double depthFactor)
      {
         Point3D[] data = IssykKulDataReader.ReadData(Resources.IssykKulData);
         Point3D[] allPoints = convertDepth(data, depthFactor);

         double xMin, xMax, yMin, yMax;
         BoundsCalculator.Calculate(allPoints, out xMin, out xMax, out yMin, out yMax);

         _gridBuilder = new IssykKulGridBuilder(allPoints, xMin, xMax, yMin, yMax);

         IssykKulGraphData graphData = IssykKulDataReader.ReadGraphData(Resources.IssykKulGraphData);
         GraphPoint3D[] points = graphData.Points;
         GraphPoint3D[][] borders = graphData.Borders;

         convertDepth(points, (float) depthFactor);
         convertDepth(borders, (float) depthFactor);

         var bounds = new Bounds((float) xMin, (float) xMax, (float) yMin, (float) yMax);

         return new IssykKulDisplayData(borders, points, bounds);
      }

      internal InterpolationResult Interpolate(int n, int m, double theta)
      {
         return new InterpolationResult(_gridBuilder.BuildGrid2D(n, m, theta));
      }

      internal InterpolationResult Interpolate(int n, int m, double dz, double theta)
      {
         return new InterpolationResult(_gridBuilder.BuildGrid3D(n, m, dz, theta));
      }

      private static Point3D[] convertDepth(IList<Point3D> data, double depthFactor)
      {
         var result = new Point3D[data.Count];

         for (var i = 0; i < result.Length; i++)
         {
            result[i] = new Point3D(data[i].X, data[i].Y, data[i].Z * depthFactor);
         }

         return result;
      }

      private static void convertDepth(IEnumerable<GraphPoint3D> data, float depthFactor)
      {
         foreach (var point in data)
         {
            point.Z *= depthFactor;
         }
      }

      private static void convertDepth(IEnumerable<GraphPoint3D[]> data, float depthFactor)
      {
         foreach (var point in data.SelectMany(points => points))
         {
            point.Z *= depthFactor;
         }
      }
   }
}