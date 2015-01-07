using System.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Forms.ParametersForms;
using Mathematics.Numerical;
using ModelProblem;

namespace LiquidDynamics.Forms.BarotropicComponent
{
   internal sealed class BarotropicComponentDataProvider : DynamicDataProvider<SquareVelocityField>
   {
      public BarotropicComponentDataProvider(Parameters parameters, GraphParameters graphParameters)
         : base(parameters, graphParameters)
      {
      }

      public Bounds Bounds
      {
         get { return new Bounds(0, (float) getParameters().SmallR, 0, (float) getParameters().SmallQ); }
      }

      protected override SquareVelocityField getData()
      {
         var n = getGraphParameters().XCoordinatesCount;
         var m = getGraphParameters().YCoordinatesCount;
         var xGrid = Grid.Create(0, getParameters().SmallR, n);
         var yGrid = Grid.Create(0, getParameters().SmallQ, m);

         var vectors = new Vector[n, m];

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               var x = xGrid.Get(i);
               var y = yGrid.Get(j);
               vectors[i, j] = new Vector(new PointF((float) x, (float) y),
                                          getVectorPoint(x, y));
            }
         }

         var cellSize = new SizeF((float) xGrid.Step, (float) yGrid.Step);
         return new SquareVelocityField(vectors, cellSize);
      }

      private PointF getVectorPoint(double arg1, double arg2)
      {
         var solution = getSolution();

         var barotropic = solution.GetBarotropicComponent();
         var u = barotropic.U(Time, arg1, arg2);
         var v = barotropic.V(Time, arg1, arg2);

         return new PointF((float) u, (float) v);
      }
   }
}