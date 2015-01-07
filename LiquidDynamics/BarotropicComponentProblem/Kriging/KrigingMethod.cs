using Common;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.Kriging
{
   public sealed class KrigingMethod
   {
      private readonly GridFunction _function;
      private readonly IVariogram _variogram;

      public KrigingMethod(GridFunction function, IVariogram variogram)
      {
         Check.NotNull(function, "function");
         Check.NotNull(variogram, "variogram");

         _function = function;
         _variogram = variogram;
      }

      public double GetValue(Point point)
      {
         var n = _function.N + 1;
         var a = new double[n, n];
         var f = new double[n];

         for (var i = 0; i < n - 1; i++)
         {
            f[i] = 2.0 * _variogram.GetValue(Point.Distance(point, _function.Grid(i)));

            for (var j = 0; j < n - 1; j++)
            {
               a[i, j] = _variogram.GetValue(Point.Distance(_function.Grid(i), _function.Grid(j)));
            }

            a[i, n - 1] = 1.0;
            a[n - 1, i] = 1.0;
         }

         f[n - 1] = 1.0;
         a[n - 1, n - 1] = 0.0;

         var gaussMethod = new GaussMethod(a, f);
         var lamda = gaussMethod.Solve();
         var result = 0.0;

         for (var i = 0; i < lamda.Length - 1; i++)
         {
            result += lamda[i] * _function[i];
         }

         return result;
      }
   }
}