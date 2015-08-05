using BarotropicComponentProblem;
using Common;
using Mathematics.MathTypes;
using Mathematics.Numerical;

namespace LiquidDynamics.Forms.TestProblem
{
   internal class TestProblemSolution
   {
      public TestProblemSolution(
         IterationMethodResult barotropic,
         Vector[,] _barotropic,
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
         _Barotropic = _barotropic;
         BarotropicU = barotropicU;
         BarotropicV = barotropicV;
         Baroclinic = baroclinic;
      }

      public IterationMethodResult Barotropic { get; private set; }

      public SquareGridFunction BarotropicU { get; private set; }
      public SquareGridFunction BarotropicV { get; private set; }
      public Vector[,] _Barotropic { get; private set; }

      public Complex[,][] Baroclinic { get; private set; }
   }
}