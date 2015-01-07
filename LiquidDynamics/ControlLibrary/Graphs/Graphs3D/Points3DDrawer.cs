using System.Drawing;
using Common;
using ControlLibrary.Types;
using Tao.OpenGl;

namespace ControlLibrary.Graphs.Graphs3D
{
   internal sealed class Points3DDrawer : IGraphDrawer3D
   {
      private const int PointSize = 2;

      private readonly Point3D[] _points;
      private readonly Color _color;

      public Points3DDrawer(Point3D[] points, Color color)
      {
         Check.NotNull(points, "points");

         _points = points;
         _color = color;
      }

      public void Draw()
      {
         Gl.glPointSize(PointSize);
         Gl.glColor3ub(_color.R, _color.G, _color.B);

         Gl.glBegin(Gl.GL_POINTS);
         foreach (var point in _points)
         {
            Gl.glVertex3f(point.X, point.Y, point.Z);
         }
         Gl.glEnd();
      }
   }
}