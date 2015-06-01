namespace BarotropicComponentProblem
{
   public sealed class ProblemParameters
   {
      // Плотность.
      public double Rho0 { get; set; }

      // Размеры бассейна.
      public double SmallR { get; set; }
      public double SmallQ { get; set; }
      public double H { get; set; }

      // Параметр, характеризующий трение о одно бассейна.
      public double Mu { get; set; }

      // Коэффициент вертикальной турбулентной вязкости.
      public double Nu { get; set; }

      // Параметры, задающие силу Кориолиса.
      public double SmallL0 { get; set; }
      public double Beta { get; set; }

      // Параметры, задающие силу ветра.
      public double F1 { get; set; }
      public double F2 { get; set; }
   }
}