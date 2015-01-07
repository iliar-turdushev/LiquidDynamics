using System.Drawing;
using Common;
using ControlLibrary.Types;
using Tao.OpenGl;

namespace ControlLibrary.Graphs.Graphs3D
{
   internal sealed class Curve3DDrawer : IGraphDrawer3D
   {
      private readonly Curve3D _curve;
      private readonly Color _graphColor;

      public Curve3DDrawer(Curve3D curve, Color graphColor)
      {
         Check.NotNull(curve, "curve");

         _curve = curve;
         _graphColor = graphColor;
      }

      public void Draw()
      {
         Gl.glColor3ub(_graphColor.R, _graphColor.G, _graphColor.B);

         Gl.glBegin(Gl.GL_LINE_STRIP);
         foreach (var point in _curve.Points)
         {
            Gl.glVertex3f(point.X, point.Y, point.Z);
         }
         Gl.glEnd();
      }
   }
}