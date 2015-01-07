using System;
using System.Drawing;
using BarotropicComponentProblem;
using BarotropicComponentProblem.IterationMethod;
using BarotropicComponentProblem.StommelModelProblem;
using Common;
using ControlLibrary.Types;
using Mathematics.Numerical;

namespace LiquidDynamics.Forms.StommelModel
{
   internal sealed class StommelModel
   {
      private readonly IterationProcessParameters _parameters;
      private readonly IterationProcess _problemSolver;

      private readonly Grid _x; 
      private readonly Grid _y;
      
      private SquareGridFunction _u;
      private SquareGridFunction _v;

      internal StommelModel(StommelModelParameters parameters)
      {
         Check.NotNull(parameters, "parameters");

         _x = Grid.Create(StommelModelProblem.XMin, StommelModelProblem.XMax, parameters.N);
         _y = Grid.Create(StommelModelProblem.YMin, StommelModelProblem.YMax, parameters.M);

         _parameters = new IterationProcessParameters(parameters.Sigma, parameters.Delta, parameters.K,
                                                      SquareGridFunction.Zeros(_x, _y));

         _problemSolver =
            IterationProcess.NewStommelModelProblemSolver(new StommelModelProblem(parameters.Epsilon, _y),
                                                          _parameters, _parameters, _x, _y);

         Bounds = new Bounds((float) StommelModelProblem.XMin, (float) StommelModelProblem.XMax,
                             (float) StommelModelProblem.YMin, (float) StommelModelProblem.YMax);

         initializeSolution(parameters.N, parameters.M, parameters.Epsilon);
      }

      internal Bounds Bounds { get; private set; }
      internal SquareVelocityField Solution { get; private set; }

      internal StommelModelSolutionInfo InitialApproximation()
      {
         var approximation = _parameters.InitialApproximation;

         var u = new BarotropicComponentInfo(ErrorCalculator.Calculate(_u, approximation));
         var v = new BarotropicComponentInfo(ErrorCalculator.Calculate(_v, approximation));

         var solution = getSquareVelocityField((i, j) => approximation[i, j],
                                               (i, j) => approximation[i, j]);

         return new StommelModelSolutionInfo(u, v, solution);
      }

      internal StommelModelSolutionInfo Solve()
      {
         var result = _problemSolver.Solve();
         var approximation = result.BarotropicComponent;
         
         var u = result.IterationResultU;
         var uComponentInfo = new BarotropicComponentInfo(u.IterationStatus, u.IterationNumber,
                                                          ErrorCalculator.Calculate(_u, u.Approximation));

         var v = result.IterationResultV;
         var vComponentInfo = new BarotropicComponentInfo(v.IterationStatus, v.IterationNumber,
                                                          ErrorCalculator.Calculate(_v, v.Approximation));
         
         var solution = getSquareVelocityField((i, j) => approximation.Vectors[i, j].End.X,
                                               (i, j) => approximation.Vectors[i, j].End.Y);

         return new StommelModelSolutionInfo(uComponentInfo, vComponentInfo, solution);
      }

      private void initializeSolution(int n, int m, double epsilon)
      {
         var solution = new StommelModelSolution(epsilon);

         var u = new double[n, m];
         var v = new double[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = _x.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = _y.Get(j);

               u[i, j] = solution.U(x, y);
               v[i, j] = solution.V(x, y);
            }
         }

         _u = new SquareGridFunction(_x, _y, u);
         _v = new SquareGridFunction(_x, _y, v);

         Solution = getSquareVelocityField((i, j) => u[i, j], (i, j) => v[i, j]);
      }

      private SquareVelocityField getSquareVelocityField(Func<int, int, double> u, Func<int, int, double> v)
      {
         var n = _x.Nodes;
         var m = _y.Nodes;
         var vectors = new Vector[n, m];

         for (var i = 0; i < n; i++)
         {
            var x = (float) _x.Get(i);

            for (var j = 0; j < m; j++)
            {
               var y = (float) _y.Get(j);
               vectors[i, j] = new Vector(new PointF(x, y),
                                          new PointF((float) u(i, j), (float) v(i, j)));
            }
         }

         var cellSize = new SizeF((float) _x.Step, (float) _y.Step);
         return new SquareVelocityField(vectors, cellSize);
      }
   }
}