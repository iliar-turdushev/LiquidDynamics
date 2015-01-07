using System;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.StommelModelProblem
{
   public sealed class StommelModelProblem : IStommelModelProblem
   {
      private readonly double _epsilon;
      private readonly double[] _f;

      public StommelModelProblem(double epsilon, Grid y)
      {
         _epsilon = epsilon;
         _f = getF(y);
      }

      public double Epsilon(int i, IndexOffset offset, int j)
      {
         return _epsilon;
      }

      public double K(int i, int j, IndexOffset offset)
      {
         return _epsilon;
      }

      public double A(int i, IndexOffset offset, int j)
      {
         return 1.0;
      }

      public double B(int i, int j, IndexOffset offset)
      {
         return 0.0;
      }

      public double F(int i, int j)
      {
         return _f[j];
      }

      public static double XMin
      {
         get { return 0.0; }
      }

      public static double XMax
      {
         get { return 5.0 / Math.PI; }
      }

      public static double YMin
      {
         get { return 0.0; }
      }

      public static double YMax
      {
         get { return 1.0; }
      }

      private static double[] getF(Grid y)
      {
         var result = new double[y.Nodes];

         for (var i = 0; i < y.Nodes; i++)
         {
            result[i] = -Math.PI * Math.Sin(Math.PI * y.Get(i));
         }

         return result;
      }
   }
}