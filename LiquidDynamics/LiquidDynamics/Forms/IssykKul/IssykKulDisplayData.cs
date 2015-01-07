using ControlLibrary.Types;

namespace LiquidDynamics.Forms.IssykKul
{
   internal sealed class IssykKulDisplayData
   {
      internal Point3D[][] Borders { get; private set; }
      internal Point3D[] Points { get; private set; }
      internal Bounds Bounds { get; private set; }

      internal IssykKulDisplayData(Point3D[][] borders, Point3D[] points, Bounds bounds)
      {
         Borders = borders;
         Points = points;
         Bounds = bounds;
      }
   }
}