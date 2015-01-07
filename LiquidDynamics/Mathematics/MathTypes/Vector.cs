namespace Mathematics.MathTypes
{
   public sealed class Vector
   {
      public Vector()
      {
      }

      public Vector(Point start, Point end)
      {
         Start = start;
         End = end;
      }

      public Point Start { get; private set; }
      public Point End { get; private set; }
   }
}