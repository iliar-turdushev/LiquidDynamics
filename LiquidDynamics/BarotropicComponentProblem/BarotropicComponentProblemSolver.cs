using BarotropicComponentProblem.IterationMethod;
using Common;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace BarotropicComponentProblem
{
   public sealed class BarotropicComponentProblemSolver
   {
      private const double BeginTime = 0;

      private readonly GridParameters _gridParameters;
      private readonly IDynamicProblem _problem;

      private readonly IterationProcessParameters _iterationProcessParametersU;
      private readonly IterationProcessParameters _iterationProcessParametersV;
      private readonly int[,] _surface;
      private readonly ZeidelMethodType _zeidelMethodType;

      private IterationMethodResult _previousResult;

      public BarotropicComponentProblemSolver(
         IDynamicProblem problem,
         GridParameters gridParameters,
         InitialCondition initialCondition,
         IterationProcessParameters iterationProcessParametersU,
         IterationProcessParameters iterationProcessParametersV,
         int[,] surface,
         ZeidelMethodType zeidelMethodType)
      {
         Check.NotNull(problem, "problem");
         Check.NotNull(gridParameters, "gridParameters");
         Check.NotNull(initialCondition, "initialCondition");
         Check.NotNull(iterationProcessParametersU, "iterationProcessParametersU");
         Check.NotNull(iterationProcessParametersV, "iterationProcessParametersV");

         _problem = problem;
         _problem.SetInitialCondition(initialCondition);

         _gridParameters = gridParameters;
         _iterationProcessParametersU = iterationProcessParametersU;
         _iterationProcessParametersV = iterationProcessParametersV;
         _surface = surface;
         _zeidelMethodType = zeidelMethodType;

         Time = BeginTime;
      }

      private double Time { get; set; }

      public IterationMethodResult Begin()
      {
         Time += _gridParameters.Tau;

         var iterationProcess =
            IterationProcess.NewBarotropicComponentProblemSolver(_problem,
                                                                 _iterationProcessParametersU, _iterationProcessParametersV,
                                                                 _gridParameters.X, _gridParameters.Y, _surface,
                                                                 _zeidelMethodType);

         _previousResult = getResult(iterationProcess.Solve());

         return _previousResult;
      }

      public IterationMethodResult Step()
      {
         Time += _gridParameters.Tau;

         var initialCondition = new InitialCondition(_previousResult.IterationResultU.Approximation,
                                                     _previousResult.IterationResultV.Approximation);
         _problem.SetInitialCondition(initialCondition);

         var parametersU = copyParameters(_iterationProcessParametersU,
                                          _previousResult.IterationResultU.Approximation);
         var parametersV = copyParameters(_iterationProcessParametersV,
                                          _previousResult.IterationResultV.Approximation);
         var iterationProcess =
            IterationProcess.NewBarotropicComponentProblemSolver(_problem, parametersU, parametersV,
                                                                 _gridParameters.X, _gridParameters.Y, _surface,
                                                                 _zeidelMethodType);

         _previousResult = getResult(iterationProcess.Solve());

         return _previousResult;
      }

      private static IterationProcessParameters copyParameters(
         IterationProcessParameters parameters, SquareGridFunction approximation)
      {
         return new IterationProcessParameters(parameters.Sigma, parameters.Delta, parameters.K, approximation);
      }
      
      private IterationMethodResult getResult(IterationMethodResult solution)
      {
         var u = new IterationResult<SquareGridFunction>(_problem.TransformU(solution.IterationResultU.Approximation),
                                                         solution.IterationResultU.IterationNumber,
                                                         solution.IterationResultU.IterationStatus);
         var v = new IterationResult<SquareGridFunction>(_problem.TransformV(solution.IterationResultV.Approximation),
                                                         solution.IterationResultV.IterationNumber,
                                                         solution.IterationResultV.IterationStatus);
         return new IterationMethodResult(getBarotropicComponent(u, v), u, v);
      }

      private static BarotropicComponent getBarotropicComponent(
        IterationResult<SquareGridFunction> uIterationResult,
        IterationResult<SquareGridFunction> vIterationResult)
      {
         var u = uIterationResult.Approximation;
         var v = vIterationResult.Approximation;

         var n = u.N;
         var m = u.M;

         var vectors = new Vector[n, m];

         for (var i = 0; i < n; i++)
         {
            for (var j = 0; j < m; j++)
            {
               vectors[i, j] = new Vector(u.Grid(i, j), new Point(u[i, j], v[i, j]));
            }
         }

         return new BarotropicComponent(vectors);
      }
   }
}