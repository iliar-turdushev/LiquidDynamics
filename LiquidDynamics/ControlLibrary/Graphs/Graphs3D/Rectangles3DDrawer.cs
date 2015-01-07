using System.Drawing;
using Common;
using ControlLibrary.Types;
using Tao.OpenGl;

namespace ControlLibrary.Graphs.Graphs3D
{
   internal sealed class Rectangles3DDrawer : IGraphDrawer3D
   {
      private readonly Color _color;
      private readonly Rectangle3D[] _rectangles;

      public Rectangles3DDrawer(Rectangle3D[] rectangles, Color color)
      {
         Check.NotNull(rectangles, "rectangles");

         _rectangles = rectangles;
         _color = color;
      }

      public void Draw()
      {
         Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
         Gl.glColor3ub(_color.R, _color.G, _color.B);

         foreach (var rectangle in _rectangles)
         {
            Gl.glBegin(Gl.GL_QUADS);

            var x = rectangle.Origin.X;
            var y = rectangle.Origin.Y;
            var z = rectangle.Origin.Z;
            var hx = rectangle.Hx;
            var hy = rectangle.Hy;
            var hz = rectangle.Hz;

            Gl.glVertex3f(x, y, z);
            Gl.glVertex3f(x + hx, y, z);
            Gl.glVertex3f(x + hx, y + hy, z);
            Gl.glVertex3f(x, y + hy, z);

            Gl.glVertex3f(x, y, z + hz);
            Gl.glVertex3f(x + hx, y, z + hz);
            Gl.glVertex3f(x + hx, y + hy, z + hz);
            Gl.glVertex3f(x, y + hy, z + hz);

            Gl.glEnd();

            Gl.glBegin(Gl.GL_LINES);

            Gl.glVertex3f(x, y, z);
            Gl.glVertex3f(x, y, z + hz);

            Gl.glVertex3f(x + hx, y, z);
            Gl.glVertex3f(x + hx, y, z + hz);

            Gl.glVertex3f(x + hx, y + hy, z);
            Gl.glVertex3f(x + hx, y + hy, z + hz);

            Gl.glVertex3f(x, y + hy, z);
            Gl.glVertex3f(x, y + hy, z + hz);

            Gl.glEnd();
         }
      }
   }
}