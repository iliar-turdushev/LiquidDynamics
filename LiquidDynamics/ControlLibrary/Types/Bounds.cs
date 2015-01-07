namespace ControlLibrary.Types
{
   public sealed class Bounds
   {
      public float XMin { get; set; }
      public float XMax { get; set; }
      public float YMin { get; set; }
      public float YMax { get; set; }

      public Bounds(float xMin, float xMax, float yMin, float yMax)
      {
         XMin = xMin;
         XMax = xMax;
         YMin = yMin;
         YMax = yMax;
      }
   }
}