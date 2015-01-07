using System;
using BarotropicComponentProblem.Kriging;
using Common;
using ControlLibrary.Types;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using Point3D = ControlLibrary.Types.Point3D;

namespace LiquidDynamics.Forms.Kriging
{
   internal sealed class Kriging
   {
      private const double R = 10;

      private readonly KrigingParameters _parameters;
      private GridFunction _interpolationData;

      public Kriging(KrigingParameters parameters)
      {
         Check.NotNull(parameters, "parameters");

         _parameters = parameters;

         calculateSurfaceCoordinates();
         calculateInterpolationData();
      }

      internal Surface3D Surface { get; private set; }

      internal InterpolationResult Interpolate()
      {
         var thetaStep = getThetaStep();
         var phiStep = getPhiStep();

         var approximation = new Point3D[_parameters.N - 1, _parameters.M - 1];
         var error = 0.0;

         var parameters = new KrigingMethodParameters(_parameters.Nodes, _parameters.Variogram);
         var kriging = new KrigingMethodInterpolation(_interpolationData, parameters);
         
         for (var i = 0; i < _parameters.N - 1; i++)
         {
            var theta = thetaStep * i + thetaStep / 2.0;
            var zExact = getZ(theta);

            for (var j = 0; j < _parameters.M - 1; j++)
            {
               var phi = phiStep * j + phiStep / 2.0;
               var x = getX(theta, phi);
               var y = getY(theta, phi);
               var zCalculated = kriging.Interpolate(new Point(x, y));

               error = Math.Max(error, Math.Abs(zCalculated - zExact));
               approximation[i, j] = new Point3D((float) x, (float) y, (float) zCalculated);
            }
         }

         return new InterpolationResult(approximation, error);
      }

      private void calculateSurfaceCoordinates()
      {
         var thetaStep = getThetaStep();
         var phiStep = getPhiStep();
         var points = new Point3D[_parameters.N, _parameters.M];

         for (var i = 0; i < _parameters.N; i++)
         {
            var theta = thetaStep * i;
            var z = (float) getZ(theta);

            for (var j = 0; j < _parameters.M; j++)
            {
               var phi = phiStep * j;
               points[i, j] = new Point3D((float) getX(theta, phi), (float) getY(theta, phi), z);
            }
         }

         Surface = new Surface3D(points);
      }

      private void calculateInterpolationData()
      {
         var n = _parameters.N - 1;
         var m = _parameters.M - 1;

         var thetaStep = getThetaStep();
         var phiStep = getPhiStep();
         
         var points = new Point[n * m + 1];
         var values = new double[n * m + 1];

         points[0] = new Point(0.0, 0.0);
         values[0] = R;

         for (var i = 0; i < n; i++)
         {
            var theta = thetaStep * (i + 1);
            var z = getZ(theta);

            for (var j = 0; j < m; j++)
            {
               var phi = phiStep * j;
               var index = i * m + j + 1;

               points[index] = new Point(getX(theta, phi), getY(theta, phi));
               values[index] = z;
            }
         }

         _interpolationData = new GridFunction(points, values);
      }

      private double getThetaStep()
      {
         return Math.PI / 2.0 / (_parameters.N - 1);
      }

      private double getPhiStep()
      {
         return 2.0 * Math.PI / (_parameters.M - 1);
      }

      private static double getX(double theta, double phi)
      {
         return R * Math.Cos(phi) * Math.Sin(theta);
      }

      private static double getY(double theta, double phi)
      {
         return R * Math.Sin(phi) * Math.Sin(theta);
      }

      private static double getZ(double theta)
      {
         return R * Math.Cos(theta);
      }
   }
}