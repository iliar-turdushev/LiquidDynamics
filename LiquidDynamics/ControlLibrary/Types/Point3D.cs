namespace ControlLibrary.Types
{
   public sealed class Point3D
   {
      public float X { get; set; }
      public float Y { get; set; }
      public float Z { get; set; }

      public Point3D(float x, float y, float z)
      {
         X = x;
         Y = y;
         Z = z;
      }

      public Point3D Shift(Point3D point)
      {
         return new Point3D(X + point.X, Y + point.Y, Z + point.Z);
      }
   }
}