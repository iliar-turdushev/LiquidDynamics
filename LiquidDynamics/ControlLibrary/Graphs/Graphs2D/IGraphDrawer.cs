using System.Drawing;

namespace ControlLibrary.Graphs.Graphs2D
{
   internal interface IGraphDrawer
   {
      void Draw(Graphics drawingContext, CoordinatesConverter converter);
   }
}