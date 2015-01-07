using System;
using Common;

namespace ControlLibrary.Graphs.Graphs3D
{
   internal sealed class CustomDrawer : IGraphDrawer3D
   {
      private readonly Action _action;

      public CustomDrawer(Action action)
      {
         Check.NotNull(action, "action");
         _action = action;
      }

      public void Draw()
      {
         _action();
      }
   }
}