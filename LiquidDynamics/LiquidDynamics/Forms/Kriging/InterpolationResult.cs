using Common;
using ControlLibrary.Types;

namespace LiquidDynamics.Forms.Kriging
{
   internal sealed class InterpolationResult
   {
      public InterpolationResult(Point3D[,] points, double error)
      {
         Check.NotNull(points, "points");

         Points = points;
         Error = error;
      }

      public Point3D[,] Points { get; private set; }
      public double Error { get; private set; }
   }
}