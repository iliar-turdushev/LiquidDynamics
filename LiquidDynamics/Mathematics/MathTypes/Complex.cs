using System;
using Mathematics.Helpers;

namespace Mathematics.MathTypes
{
   public sealed class Complex
   {
      public static readonly Complex I = new Complex(0, 1);

      public double Re { get; private set; }
      public double Im { get; private set; }

      public Complex() : this(0.0, 0.0)
      {
      }

      public Complex(double re, double im)
      {
         Re = re;
         Im = im;
      }

      public Complex(Complex complex) : this(complex.Re, complex.Im)
      {
      }

      private void add(Complex right)
      {
         Re += right.Re;
         Im += right.Im;
      }

      private void sub(Complex right)
      {
         Re -= right.Re;
         Im -= right.Im;
      }

      private void mul(Complex right)
      {
         var tempRe = Re * right.Re - Im * right.Im;
         var tempIm = Re * right.Im + Im * right.Re;
         Re = tempRe;
         Im = tempIm;
      }

      private void div(Complex right)
      {
         var tempRe = (Re * right.Re + Im * right.Im) /
                      (right.Re * right.Re + right.Im * right.Im);
         var tempIm = (-Re * right.Im + Im * right.Re) /
                      (right.Re * right.Re + right.Im * right.Im);
         Re = tempRe;
         Im = tempIm;
      }

      public static implicit operator Complex(double real)
      {
         return new Complex(real, 0);
      }

      public static Complex operator -(Complex complex)
      {
         return new Complex(-complex.Re, -complex.Im);
      }

      public static Complex operator +(Complex left, Complex right)
      {
         var result = new Complex(left);
         result.add(right);
         return result;
      }

      public static Complex operator -(Complex left, Complex right)
      {
         var result = new Complex(left);
         result.sub(right);
         return result;
      }

      public static Complex operator *(Complex left, Complex right)
      {
         var result = new Complex(left);
         result.mul(right);
         return result;
      }

      public static Complex operator /(Complex left, Complex right)
      {
         var result = new Complex(left);
         result.div(right);
         return result;
      }

      public static double Mod(Complex complex)
      {
         return Math.Sqrt(complex.Re * complex.Re + complex.Im * complex.Im);
      }

      public static double Arg(Complex complex)
      {
         double arg;
         if (complex.Re > 0)
         {
            arg = Math.Atan(complex.Im / complex.Re);
         }
         else if (complex.Re < 0 && complex.Im >= 0)
         {
            arg = Math.PI + Math.Atan(complex.Im / complex.Re);
         }
         else if (complex.Re < 0 && complex.Im < 0)
         {
            arg = -Math.PI + Math.Atan(complex.Im / complex.Re);
         }
         else if (complex.Re.IsZero() && complex.Im > 0)
         {
            arg = Math.PI / 2.0;
         }
         else
         {
            arg = -Math.PI / 2.0;
         }
         return arg;
      }

      public static Complex Exp(Complex complex)
      {
         return new Complex(
            Math.Exp(complex.Re) * Math.Cos(complex.Im),
            Math.Exp(complex.Re) * Math.Sin(complex.Im));
      }

      public static Complex Pow(Complex complex, int n)
      {
         var mod = Mod(complex);
         var arg = Arg(complex);
         return new Complex(
            Math.Pow(mod, n) * Math.Cos(n * arg),
            Math.Pow(mod, n) * Math.Sin(n * arg));
      }

      public static Complex Sin(Complex complex)
      {
         return (Exp(I * complex) - Exp(-I * complex)) / (2.0 * I);
      }

      public static Complex Cos(Complex complex)
      {
         return (Exp(I * complex) + Exp(-I * complex)) / 2.0;
      }

      public static Complex Tan(Complex complex)
      {
         return Sin(complex) / Cos(complex);
      }

      public static Complex Sinh(Complex complex)
      {
         return (Exp(complex) - Exp(-complex)) / 2.0;
      }

      public static Complex Cosh(Complex complex)
      {
         return (Exp(complex) + Exp(-complex)) / 2.0;
      }

      public static Complex Tanh(Complex complex)
      {
         return Sinh(complex) / Cosh(complex);
      }
   }
}