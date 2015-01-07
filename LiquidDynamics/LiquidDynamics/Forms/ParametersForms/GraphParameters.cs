namespace LiquidDynamics.Forms.ParametersForms
{
   internal sealed class GraphParameters
   {
      // ��� �� �������.
      public double TimeStep { get; set; }
      
      // ����� ����� �� ���� X � Y (������������ ��� ���������� ����� ���������).
      public int XCoordinatesCount { get; set; }
      public int YCoordinatesCount { get; set; }

      // ����� ����� �� ��� Z (������������ ��� ���������� ������� ������).
      public int ZCoordinatesCount { get; set; }
   }
}