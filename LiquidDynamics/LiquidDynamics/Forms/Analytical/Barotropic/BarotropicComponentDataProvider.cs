using System.Drawing;
using Common;
using ControlLibrary.Types;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Barotropic;

namespace LiquidDynamics.Forms.Analytical.Barotropic
{
   internal sealed class BarotropicComponentDataProvider
   {
      private readonly Parameters _parameters;
      private readonly IBarotropicComponent _barotropic;

      private int _nx;
      private int _ny;
      private double _dt;
      
      public BarotropicComponentDataProvider(Parameters parameters)
      {
         Check.NotNull(parameters, nameof(parameters));

         _parameters = parameters;
         _barotropic = SolutionCreator.Create(_parameters).GetBarotropicComponent();
      }

      public Bounds Bounds => new Bounds(0, (float) _parameters.SmallR, 0, (float) _parameters.SmallQ);

      public double Time { get; private set; }

      public SquareVelocityField Begin(int nx, int ny, double dt)
      {
         _nx = nx;
         _ny = ny;
         _dt = dt;

         Time = 0;

         return calculateBarotropicComponent();
      }

      public SquareVelocityField Step()
      {
         Time += _dt;
         return calculateBarotropicComponent();
      }

      private SquareVelocityField calculateBarotropicComponent()
      {
         var x = Grid.Create(0, _parameters.SmallR, _nx);
         var y = Grid.Create(0, _parameters.SmallQ, _ny);
         var vectors = new Vector[x.Nodes, y.Nodes];

         for (int i = 0; i < x.Nodes; i++)
         {
            for (int j = 0; j < y.Nodes; j++)
            {
               double u = _barotropic.U(Time, x[i], y[j]);
               double v = _barotropic.V(Time, x[i], y[j]);
               var start = new PointF((float) x[i], (float) y[j]);
               var end = new PointF((float) u, (float) v);
               vectors[i, j] = new Vector(start, end);
            }
         }

         var cellSize = new SizeF((float) x.Step, (float) y.Step);
         return new SquareVelocityField(vectors, cellSize);
      }
   }
}