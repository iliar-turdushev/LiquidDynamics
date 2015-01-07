namespace ControlLibrary.Types
{
   public sealed class Bounds3D
   {
      public float XMin { get; set; }
      public float XMax { get; set; }
      public float YMin { get; set; }
      public float YMax { get; set; }
      public float ZMin { get; set; }
      public float ZMax { get; set; }

      public Bounds3D(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
      {
         XMin = xMin;
         XMax = xMax;
         YMin = yMin;
         YMax = yMax;
         ZMin = zMin;
         ZMax = zMax;
      }
   }
}