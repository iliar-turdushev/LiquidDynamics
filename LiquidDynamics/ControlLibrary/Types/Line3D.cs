using Common;

namespace ControlLibrary.Types
{
   public sealed class Line3D
   {
      public Line3D(Point3D start, Point3D end)
      {
         Check.NotNull(start, "start");
         Check.NotNull(end, "end");

         Start = start;
         End = end;
      }

      public Point3D Start { get; private set; }
      public Point3D End { get; private set; }
   }
}