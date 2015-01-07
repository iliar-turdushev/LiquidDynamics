using System.Drawing;
using ControlLibrary.Types;

namespace LiquidDynamics.Forms.EkmanSpiral
{
   internal sealed class EkmanSpiralGraphs
   {
      internal EkmanSpiralGraphs(Curve3D uv, PointF[] u, PointF[] v)
      {
         UV = uv;
         U = u;
         V = v;
      }

      internal Curve3D UV { get; private set; }
      internal PointF[] U { get; private set; }
      internal PointF[] V { get; private set; }
   }
}