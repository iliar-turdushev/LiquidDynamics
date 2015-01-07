using System;

namespace BarotropicComponentProblem.Kriging
{
   internal sealed class GaussMethod
   {
      private readonly double[,] _a;
      private readonly double[] _f;

      internal GaussMethod(double[,] a, double[] f)
      {
         _a = a;
         _f = f;
      }

      internal double[] Solve()
      {
         var n = _f.Length;
       
         var rows = new int[n];
         var cols = new int[n];

         for (var i = 0; i < n; i++)
         {
            rows[i] = -1;
            cols[i] = -1;
         }

         for (var k = 0; k < n; k++)
         {
            var max = 0.0;
            var rowIndex = 0;
            var columnIndex = 0;

            for (var i = 0; i < n; i++)
            {
               if (rows[i] != -1)
                  continue;

               for (var j = 0; j < n; j++)
               {
                  if (cols[j] != -1)
                     continue;

                  var temp = Math.Abs(_a[i, j]);

                  if (temp > max)
                  {
                     max = temp;
                     rowIndex = i;
                     columnIndex = j;
                  }
               }
            }

            rows[rowIndex] = k;

            for (var i = 0; i < n; i++)
            {
               if (rows[i] != -1)
                  continue;

               var temp = _a[i, columnIndex] / _a[rowIndex, columnIndex];
               _f[i] -= temp * _f[rowIndex];

               for (var j = 0; j < n; j++)
               {
                  if (cols[j] != -1)
                     continue;

                  if (j == columnIndex)
                     _a[i, j] = 0;
                  else
                     _a[i, j] -= temp * _a[rowIndex, j];
               }
            }

            cols[columnIndex] = k;
         }

         var c = new double[n, n];
         var g = new double[n];

         for (var i = 0; i < n; i++)
         {
            g[rows[i]] = _f[i];

            for (var j = 0; j < n; j++)
            {
               c[rows[i], j] = _a[i, j];
            }
         }

         for (var j = 0; j < n; j++)
         {
            _f[j] = g[j];

            for (var i = 0; i < n; i++)
            {
               _a[i, cols[j]] = c[i, j];
            }
         }

         g[n - 1] = _f[n - 1] / _a[n - 1, n - 1];

         for (var i = n - 2; i >= 0; i--)
         {
            var temp = 0.0;

            for (var j = i + 1; j < n; j++)
            {
               temp += _a[i, j] * g[j];
            }

            g[i] = (_f[i] - temp) / _a[i, i];
         }

         var u = new double[n];

         for (var i = 0; i < n; i++)
         {
            u[i] = g[cols[i]];
         }

         return u;
      }
   }
}
