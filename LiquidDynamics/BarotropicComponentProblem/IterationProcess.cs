using BarotropicComponentProblem.IterationMethod;
using BarotropicComponentProblem.IterationMethod.BarotropicComponentProblem;
using BarotropicComponentProblem.IterationMethod.StommelModel;
using Common;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace BarotropicComponentProblem
{
   public sealed class IterationProcess
   {
      private readonly ZeidelIterationProcess _uIterationProcess;
      private readonly ZeidelIterationProcess _vIterationProcess;

      public static IterationProcess NewStommelModelProblemSolver(
         IStommelModelProblem problem,
         IterationProcessParameters parametersU,
         IterationProcessParameters parametersV,
         Grid x, Grid y)
      {
         checkParams(problem, parametersU, parametersV, x, y);

         return new IterationProcess(parametersU, parametersV,
                                     new StommelModelZeidelMethodU(problem, x, y),
                                     new StommelModelZeidelMethodV(problem, x, y));
      }

      public static IterationProcess NewBarotropicComponentProblemSolver(
         IBarotropicComponentProblem problem,
         IterationProcessParameters parametersU,
         IterationProcessParameters parametersV,
         Grid x, Grid y, int[,] surface)
      {
         checkParams(problem, parametersU, parametersV, x, y);

         return new IterationProcess(parametersU, parametersV,
                                     new BarotropicComponentZeidelMethodU2(problem, x, y, surface),
                                     new BarotropicComponentZeidelMethodV2(problem, x, y, surface));
      }

      public IterationMethodResult Solve()
      {
         var u = _uIterationProcess.Solve();
         var v = _vIterationProcess.Solve();
         return new IterationMethodResult(getBarotropicComponent(u, v), u, v);
      }

      private IterationProcess(
         IterationProcessParameters parametersU,
         IterationProcessParameters parametersV,
         ZeidelMethodBase zeidelMethodU,
         ZeidelMethodBase zeidelMethodV)
      {
         _uIterationProcess = new ZeidelIterationProcess(zeidelMethodU, parametersU);
         _vIterationProcess = new ZeidelIterationProcess(zeidelMethodV, parametersV);
      }

      private static void checkParams(
         IProblem problem,
         IterationProcessParameters parametersU,
         IterationProcessParameters parametersV,
         Grid x, Grid y)
      {
         Check.NotNull(problem, "problem");
         Check.NotNull(parametersU, "parametersU");
         Check.NotNull(parametersV, "parametersV");
         Check.NotNull(x, "x");
         Check.NotNull(y, "y");
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