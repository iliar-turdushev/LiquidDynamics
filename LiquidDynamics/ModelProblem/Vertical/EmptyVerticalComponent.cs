namespace ModelProblem.Vertical
{
   internal sealed class EmptyVerticalComponent : IVerticalComponent
   {
      public double W(double t, double x, double y, double z)
      {
         return 0.0;
      }
   }
}