using ControlLibrary.Types;

namespace LiquidDynamics.Forms.IssykKul
{
   internal sealed class IssykKulGraphData
   {
      internal IssykKulGraphData(Point3D[][] borders, Point3D[] points)
      {
         Borders = borders;
         Points = points;
      }

      internal Point3D[][] Borders { get; private set; }
      internal Point3D[] Points { get; private set; }
   }
}