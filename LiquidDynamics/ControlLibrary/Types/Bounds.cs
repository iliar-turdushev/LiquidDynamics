namespace ControlLibrary.Types
{
   public sealed class Bounds
   {
      public Bounds(float xMin, float xMax, float yMin, float yMax)
      {
         XMin = xMin;
         XMax = xMax;
         YMin = yMin;
         YMax = yMax;
      }

      public float XMin { get; }
      public float XMax { get; }
      public float YMin { get; }
      public float YMax { get; }
   }
}