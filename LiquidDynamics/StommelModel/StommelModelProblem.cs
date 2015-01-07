using System;
using BarotropicComponentProblem;

namespace StommelModel
{
   public sealed class StommelModelProblem : IProblem
   {
      private readonly double _epsilon;

      public StommelModelProblem(double epsilon)
      {
         _epsilon = epsilon;
      }

      public double Epsilon(double x, double y)
      {
         return _epsilon;
      }

      public double K(double x, double y)
      {
         return _epsilon;
      }

      public double A(double x, double y)
      {
         return 1.0;
      }

      public double B(double x, double y)
      {
         return 0.0;
      }

      public double F(double x, double y)
      {
         return -Math.PI * Math.Sin(Math.PI * y);
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
   }
}