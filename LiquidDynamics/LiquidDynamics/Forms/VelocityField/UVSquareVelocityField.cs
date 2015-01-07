using System.Drawing;
using LiquidDynamics.Forms.ParametersForms;
using LiquidDynamics.Properties;
using ModelProblem;

namespace LiquidDynamics.Forms.VelocityField
{
   internal sealed class UVSquareVelocityField : SquareVelocityFieldBase
   {
      public UVSquareVelocityField(Parameters parameters, GraphParameters graphParameters)
         : base(parameters, graphParameters)
      {
      }

      protected override double XMax
      {
         get { return getParameters().SmallR; }
      }

      protected override double YMax
      {
         get { return getParameters().SmallQ; }
      }

      protected override PointF getVectorPoint(double arg1, double arg2)
      {
         var solution = getSolution();
         
         var barotropic = solution.GetBarotropicComponent();
         var u = barotropic.U(Time, arg1, arg2);
         var v = barotropic.V(Time, arg1, arg2);
         
         var baroclinic = solution.GetBaroclinicComponent();
         var theta = baroclinic.Theta(Time, arg1, arg2, Cut);

         return new PointF((float) (u + theta.Re), (float) (v + theta.Im));
      }

      protected override void checkCut(double cut)
      {
         if (cut < 0.0 || cut > getParameters().H)
         {
            var message = string.Format(Resources.InvalidCutValue, 0.0, getParameters().H);
            throw new InvalidFieldValueException(message);
         }
      }
   }
}