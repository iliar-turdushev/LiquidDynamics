using System.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Forms.ParametersForms;
using Mathematics.Numerical;
using ModelProblem;

namespace LiquidDynamics.Forms.VelocityField
{
   internal abstract class SquareVelocityFieldBase : DynamicDataProvider<SquareVelocityField>
   {
      private double _cut;

      protected SquareVelocityFieldBase(Parameters parameters, GraphParameters graphParameters)
         : base(parameters, graphParameters)
      {
      }

      public Bounds Bounds
      {
         get { return new Bounds(0, (float) XMax, 0, (float) YMax); }
      }

      public double Cut
      {
         get { return _cut; }
         set
         {
            checkCut(value);
            _cut = value;
         }
      }

      protected override SquareVelocityField getData()
      {
         var n = getGraphParameters().XCoordinatesCount;
         var m = getGraphParameters().YCoordinatesCount;
         var xGrid = Grid.Create(0, XMax, n);
         var yGrid = Grid.Create(0, YMax, m);

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

      protected abstract double XMax { get; }
      protected abstract double YMax { get; }

      protected abstract PointF getVectorPoint(double arg1, double arg2);
      protected abstract void checkCut(double cut);
   }
}