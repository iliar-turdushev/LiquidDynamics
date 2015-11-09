using System.Collections.Generic;
using System.Drawing;

namespace LiquidDynamics.Forms.BarotropicComponentNumerical
{
   internal sealed class ErrorContainer
   {
      private readonly List<PointF> _errorsU;
      private readonly List<PointF> _errorsV;

      internal ErrorContainer()
      {
         _errorsU = new List<PointF>();
         _errorsV = new List<PointF>();
         MaxError = 0;
         Time = 0;
      }

      internal PointF[] ErrorsU
      {
         get { return _errorsU.ToArray(); }
      }

      internal PointF[] ErrorsV
      {
         get { return _errorsV.ToArray(); }
      }

      internal float MaxError { get; private set; }
      internal float MaxErrorU { get; private set; }
      internal float MaxErrorV { get; private set; }

      internal float Time { get; private set; }

      internal void AddError(double time, double errorU, double errorV)
      {
         if (errorU > MaxErrorU)
            MaxErrorU = (float) errorU;

         if (errorV > MaxErrorV)
            MaxErrorV = (float) errorV;

         if (errorU > MaxError)
            MaxError = (float) errorU;

         if (errorV > MaxError)
            MaxError = (float) errorV;

         _errorsU.Add(new PointF((float) time, (float) errorU));
         _errorsV.Add(new PointF((float) time, (float) errorV));
         Time = (float) time;
      }
   }
}