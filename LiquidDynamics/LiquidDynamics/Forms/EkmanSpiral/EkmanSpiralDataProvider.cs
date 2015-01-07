using System.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Forms.ParametersForms;
using LiquidDynamics.Properties;
using Mathematics.Numerical;
using ModelProblem;

namespace LiquidDynamics.Forms.EkmanSpiral
{
   internal sealed class EkmanSpiralDataProvider : DynamicDataProvider<EkmanSpiralGraphs>
   {
      private double _x;
      private double _y;

      public EkmanSpiralDataProvider(Parameters parameters, GraphParameters graphParameters)
         : base(parameters, graphParameters)
      {
      }
      
      public double X
      {
         get { return _x; }
         set
         {
            if (value < 0.0 || value > getParameters().SmallR)
               invalidParameterValueError("X", 0.0, getParameters().SmallR);

            _x = value;
         }
      }

      public double Y
      {
         get { return _y; }
         set
         {
            if (value < 0.0 || value > getParameters().SmallQ)
               invalidParameterValueError("Y", 0.0, getParameters().SmallQ);

            _y = value;
         }
      }
      
      public double H
      {
         get { return getParameters().H; }
      }

      protected override EkmanSpiralGraphs getData()
      {
         var baroclinic = getSolution().GetBaroclinicComponent();
         var nodes = getGraphParameters().ZCoordinatesCount;
         var grid = Grid.Create(0.0, getParameters().H, nodes);
         var points = new Point3D[nodes];
         var u = new PointF[nodes];
         var v = new PointF[nodes];

         for (var i = 0; i < nodes; i++)
         {
            var z = grid.Get(i);
            var theta = baroclinic.Theta(Time, X, Y, z);
            u[i] = new PointF((float) z, (float) theta.Re);
            v[i] = new PointF((float) z, (float) theta.Im);
            points[i] = new Point3D((float) theta.Re, (float) theta.Im, (float) z);
         }

         return new EkmanSpiralGraphs(new Curve3D(points), u, v);
      }

      private void invalidParameterValueError(string parameterName, double from, double to)
      {
         var message = string.Format(Resources.InvalidParameterValueBoundsBroken, parameterName, from, to);
         throw new InvalidFieldValueException(message);
      }
   }
}