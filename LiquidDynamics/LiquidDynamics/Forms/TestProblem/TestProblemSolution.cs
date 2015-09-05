using Common;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace LiquidDynamics.Forms.TestProblem
{
   internal class TestProblemSolution
   {
      public TestProblemSolution(
         Vector[,] barotropic,
         SquareGridFunction barotropicU,
         SquareGridFunction barotropicV,
         Complex[,][] baroclinic
         )
      {
         Check.NotNull(barotropic, "barotropic");
         Check.NotNull(barotropicU, "barotropicU");
         Check.NotNull(barotropicV, "barotropicV");
         Check.NotNull(baroclinic, "baroclinic");

         Barotropic = barotropic;
         BarotropicU = barotropicU;
         BarotropicV = barotropicV;
         Baroclinic = baroclinic;
      }

      public Vector[,] Barotropic { get; private set; }
      public SquareGridFunction BarotropicU { get; private set; }
      public SquareGridFunction BarotropicV { get; private set; }

      public Complex[,][] Baroclinic { get; private set; }

      public double[,][] W { get; set; }
   }
}