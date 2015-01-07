using System.Drawing;
using Common;
using ControlLibrary.Types;
using Tao.OpenGl;

namespace ControlLibrary.Graphs.Graphs3D
{
   internal sealed class Lines3DDrawer : IGraphDrawer3D
   {
      private readonly Line3D[] _lines;
      private readonly Color _color;

      public Lines3DDrawer(Line3D[] lines, Color color)
      {
         Check.NotNull(lines, "lines");

         _lines = lines;
         _color = color;
      }

      public void Draw()
      {
         Gl.glColor3ub(_color.R, _color.G, _color.B);

         Gl.glBegin(Gl.GL_LINES);

         foreach (var line in _lines)
         {
            Gl.glVertex3f(line.Start.X, line.Start.Y, line.Start.Z);
            Gl.glVertex3f(line.End.X, line.End.Y, line.End.Z);
         }

         Gl.glEnd();
      }
   }
}