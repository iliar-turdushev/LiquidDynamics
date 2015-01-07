using Common;

namespace ControlLibrary.Types
{
   public sealed class Curve3D
   {
      public Curve3D(Point3D[] points)
      {
         Check.NotNull(points, "points");
         Points = points;
      }

      public Point3D[] Points { get; private set; }
   }
}