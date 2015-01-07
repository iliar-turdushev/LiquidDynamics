namespace BarotropicComponentProblem
{
   public sealed class ProblemParameters
   {
      // ���������.
      public double Rho0 { get; set; }

      // ������� ��������.
      public double SmallR { get; set; }
      public double SmallQ { get; set; }
      public double H { get; set; }

      // ��������, ��������������� ������ � ���� ��������.
      public double Mu { get; set; }

      // ���������, �������� ���� ���������.
      public double SmallL0 { get; set; }
      public double Beta { get; set; }

      // ���������, �������� ���� �����.
      public double F1 { get; set; }
      public double F2 { get; set; }
   }
}