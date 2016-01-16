using System.Collections.Generic;
using System.Drawing;

namespace LiquidDynamics.Forms.VerticalComponentNumerical
{
   internal class VerticalComponentErrorContainer
   {
      private readonly List<PointF> _errors;

      public VerticalComponentErrorContainer()
      {
         _errors = new List<PointF>();
      }

      public float Time { get; private set; }

      public float MaxError { get; private set; }

      public PointF[] Errors
      {
         get { return _errors.ToArray(); }
      }

      public void AddError(double time, double error)
      {
         Time = (float) time;

         if (error > MaxError)
            MaxError = (float) error;

         _errors.Add(new PointF(Time, (float) error));
      }
   }
}