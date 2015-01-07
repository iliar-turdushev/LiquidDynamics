using System;

namespace StommelModel
{
   public sealed class StommelModelSolution
   {
      private const double Pi = Math.PI;
      private const double R = 5.0 / Pi;

      private static readonly Func<double, double> Sqrt = Math.Sqrt;
      private static readonly Func<double, double> Sqr = value => value * value;
      private static readonly Func<double, double> Exp = Math.Exp;
      private static readonly Func<double, double> Cos = Math.Cos;
      private static readonly Func<double, double> Sin = Math.Sin;

      private Func<double, double, double> _u;
      private Func<double, double, double> _v;
      
      private readonly double _epsilon;
      
      public StommelModelSolution(double epsilon)
      {
         _epsilon = epsilon;
         initialize();
      }

      public double U(double x, double y)
      {
         return _u(x, y);
      }

      public double V(double x, double y)
      {
         return _v(x, y);
      }

      private void initialize()
      {
         var mu1 = (-1.0 + Sqrt(1.0 + 4.0 * Sqr(_epsilon * Pi))) / (2.0 * _epsilon);
         var mu2 = (-1.0 - Sqrt(1.0 + 4.0 * Sqr(_epsilon * Pi))) / (2.0 * _epsilon);

         var p = (1.0 - Exp(mu2 * R)) / (Exp(mu1 * R) - Exp(mu2 * R));
         var q = 1.0 - p;

         _u = (x, y) => Cos(Pi * y) / _epsilon * (p * Exp(mu1 * x) + q * Exp(mu2 * x) - 1.0);
         _v = (x, y) => -Sin(Pi * y) / _epsilon / Pi * (mu1 * p * Exp(mu1 * x) + mu2 * q * Exp(mu2 * x));
      }
   }
}
