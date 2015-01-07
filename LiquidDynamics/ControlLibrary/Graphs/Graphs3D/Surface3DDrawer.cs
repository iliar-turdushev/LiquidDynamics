using System.Drawing;
using Common;
using ControlLibrary.Types;
using Tao.OpenGl;

namespace ControlLibrary.Graphs.Graphs3D
{
   internal sealed class Surface3DDrawer : IGraphDrawer3D
   {
      private readonly Surface3D _surface;
      private readonly Color _color;

      public Surface3DDrawer(Surface3D surface, Color color)
      {
         Check.NotNull(surface, "surface");

         _surface = surface;
         _color = color;
      }

      public void Draw()
      {
         Gl.glColor3ub(_color.R, _color.G, _color.B);
         Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);

         var points = _surface.Points;
         var n = points.GetLength(0);
         var m = points.GetLength(1);

         for (var i = 0; i < n - 1; i++)
         {
            Gl.glBegin(Gl.GL_QUAD_STRIP);

            for (var j = 0; j < m; j++)
            {
               Gl.glVertex3f(points[i, j].X, points[i, j].Y, points[i, j].Z);
               Gl.glVertex3f(points[i + 1, j].X, points[i + 1, j].Y, points[i + 1, j].Z);
            }

            Gl.glEnd();
         }
      }
   }
}