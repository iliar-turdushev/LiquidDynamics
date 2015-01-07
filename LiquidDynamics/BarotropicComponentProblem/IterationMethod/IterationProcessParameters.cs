using Common;
using Mathematics.Numerical;

namespace BarotropicComponentProblem.IterationMethod
{
   public sealed class IterationProcessParameters
   {
      public IterationProcessParameters(double sigma, double delta, int k, SquareGridFunction initialApproximation)
      {
         Check.NotNull(initialApproximation, "initialApproximation");

         Sigma = sigma;
         Delta = delta;
         K = k;
         InitialApproximation = initialApproximation;
      }
      
      // �������� �������� ����������.
      public double Sigma { get; private set; }

      // �������� �������� ������������.
      public double Delta { get; private set; }

      // ������������ ����� ��������.
      public int K { get; private set; }

      // ��������� �����������.
      public SquareGridFunction InitialApproximation { get; private set; }
   }
}