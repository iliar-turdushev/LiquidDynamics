using Common;

namespace ControlLibrary.Types
{
   public sealed class Rectangle3D
   {
      public Rectangle3D(Point3D origin, float hx, float hy, float hz)
      {
         Check.NotNull(origin, "origin");

         Origin = origin;
         Hx = hx;
         Hy = hy;
         Hz = hz;
      }

      public Point3D Origin { get; private set; }
      public float Hx { get; private set; }
      public float Hy { get; private set; }
      public float Hz { get; private set; }

      public Rectangle3D Shift(Point3D point)
      {
         var x = Origin.X + point.X;
         var y = Origin.Y + point.Y;
         var z = Origin.Z + point.Z;

         return new Rectangle3D(new Point3D(x, y, z), Hx, Hy, Hz);
      }
   }
}