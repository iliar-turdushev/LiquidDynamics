using System.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Forms.ParametersForms;
using LiquidDynamics.Properties;
using Mathematics.Numerical;
using ModelProblem;

namespace LiquidDynamics.Forms.Upwelling
{
   internal sealed class UpwellingDataProvider : DynamicDataProvider<UpwellingData>
   {
      private double _z;

      public UpwellingDataProvider(Parameters parameters, GraphParameters graphParameters)
         : base(parameters, graphParameters)
      {
         Z = getParameters().H / 2.0;
      }

      public Bounds Bounds
      {
         get { return new Bounds(0, (float) getParameters().SmallR, 0, (float) getParameters().SmallQ); }
      }

      public double Z
      {
         get { return _z; }
         set
         {
            if (value <= 0.0 || value >= getParameters().H)
               invalidParameterValueError(0.0, getParameters().H);

            _z = value;
         }
      }

      protected override UpwellingData getData()
      {
         var n = getGraphParameters().XCoordinatesCount;
         var m = getGraphParameters().YCoordinatesCount;
         var xGrid = Grid.Create(0, getParameters().SmallR, n);
         var yGrid = Grid.Create(0, getParameters().SmallQ, m);
         
         var verticalComponent = getSolution().GetVerticalComponent();
         var gridPoints = new PointF[n, m];
         var intensities = new float[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = xGrid.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = yGrid.Get(j);
               var value = verticalComponent.W(Time, x, y, Z);
               gridPoints[i, j] = new PointF((float) x, (float) y);
               intensities[i, j] = (float) (Z - value);
            }
         }

         var cellSize = new SizeF((float) xGrid.Step, (float) yGrid.Step);
         return new UpwellingData(gridPoints, intensities, cellSize);
      }

      private void invalidParameterValueError(double from, double to)
      {
         var message = string.Format(Resources.InvalidUpwellingParameterValue, from, to);
         throw new InvalidFieldValueException(message);
      }
   }
}