using System.Drawing;
using LiquidDynamics.Forms.ParametersForms;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.VelocityField
{
   internal sealed class VWSquareVelocityField : SquareVelocityFieldBase
   {
      public VWSquareVelocityField(Parameters parameters, GraphParameters graphParameters)
         : base(parameters, graphParameters)
      {
      }

      protected override double XMax
      {
         get { return getParameters().SmallQ; }
      }

      protected override double YMax
      {
         get { return getParameters().H; }
      }

      protected override PointF getVectorPoint(double arg1, double arg2)
      {
         var solution = getSolution();

         var barotropic = solution.GetBarotropicComponent();
         var v = barotropic.V(Time, Cut, arg1);

         var baroclinic = solution.GetBaroclinicComponent();
         var theta = baroclinic.Theta(Time, Cut, arg1, arg2);

         var vertical = solution.GetVerticalComponent();
         var w = vertical.W(Time, Cut, arg1, arg2);

         return new PointF((float) (v + theta.Im), (float) w);
      }

      protected override void checkCut(double cut)
      {
         if (cut < 0.0 || cut > getParameters().SmallR)
         {
            var message = string.Format(Resources.InvalidCutValue, 0.0, getParameters().SmallR);
            throw new InvalidFieldValueException(message);
         }
      }
   }
}