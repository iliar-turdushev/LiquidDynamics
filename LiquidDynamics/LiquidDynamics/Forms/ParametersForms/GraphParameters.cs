namespace LiquidDynamics.Forms.ParametersForms
{
   internal sealed class GraphParameters
   {
      // Ўаг по времени.
      public double TimeStep { get; set; }
      
      // „исло узлов по ос€м X и Y (используютс€ при построении полей скоростей).
      public int XCoordinatesCount { get; set; }
      public int YCoordinatesCount { get; set; }

      // „исло узлов по оси Z (используетс€ при построении спирали Ёкмана).
      public int ZCoordinatesCount { get; set; }
   }
}