namespace ModelProblem
{
   public sealed class Parameters
   {
      // Плотность.
      public double Rho0 { get; set; }

      // Параметры, задающие силу Кориолиса.
      public double SmallL0 { get; set; }
      public double Beta { get; set; }

      // Размеры бассейна.
      public double SmallR { get; set; }
      public double SmallQ { get; set; }
      public double H { get; set; }
      
      // Коэффициент вертикальной турбулентной вязкости.
      public double Nu { get; set; }

      // Параметр, характеризующий трение о дно.
      public double Mu { get; set; }

      // Параметры силы ветра.
      public double F1 { get; set; }
      public double F2 { get; set; }
      
      // Параметры, возникшие при решении задачи.
      public int SmallK { get; set; }
      public int SmallM { get; set; }
      public double S1 { get; set; }
      public double S2 { get; set; }
      public double Phi { get; set; }
   }
}