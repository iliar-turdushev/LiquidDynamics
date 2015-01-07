using System.Drawing;
using LiquidDynamics.Forms.ParametersForms;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.VelocityField
{
   internal sealed class UWSquareVelocityField : SquareVelocityFieldBase
   {
      public UWSquareVelocityField(Parameters parameters, GraphParameters graphParameters)
         : base(parameters, graphParameters)
      {
      }

      protected override double XMax
      {
         get { return getParameters().SmallR; }
      }

      protected override double YMax
      {
         get { return getParameters().H; }
      }

      protected override PointF getVectorPoint(double arg1, double arg2)
      {
         var solution = getSolution();

         var barotropic = solution.GetBarotropicComponent();
         var u = barotropic.U(Time, arg1, Cut);

         var baroclinic = solution.GetBaroclinicComponent();
         var theta = baroclinic.Theta(Time, arg1, Cut, arg2);

         var vertical = solution.GetVerticalComponent();
         var w = vertical.W(Time, arg1, Cut, arg2);

         return new PointF((float) (u + theta.Re), (float) w);
      }

      protected override void checkCut(double cut)
      {
         if (cut < 0.0 || cut > getParameters().SmallQ)
         {
            var message = string.Format(Resources.InvalidCutValue, 0.0, getParameters().SmallQ);
            throw new InvalidFieldValueException(message);
         }
      }
   }
}