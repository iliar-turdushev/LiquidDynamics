using Common;

namespace Mathematics.MathTypes
{
   public sealed class Rectangle3D
   {
      public Rectangle3D(Point3D origin, double hx, double hy, double hz)
      {
         Check.NotNull(origin, "origin");

         Origin = origin;
         Hx = hx;
         Hy = hy;
         Hz = hz;
      }

      public Point3D Origin { get; private set; }
      public double Hx { get; set; }
      public double Hy { get; set; }
      public double Hz { get; set; }
   }
}