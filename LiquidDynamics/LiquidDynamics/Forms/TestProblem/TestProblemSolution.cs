using BarotropicComponentProblem;
using Common;
using Mathematics.MathTypes;

namespace LiquidDynamics.Forms.TestProblem
{
   internal class TestProblemSolution
   {
      public TestProblemSolution(IterationMethodResult barotropic, Complex[,][] baroclinic)
      {
         Check.NotNull(barotropic, "barotropic");
         Check.NotNull(baroclinic, "baroclinic");

         Barotropic = barotropic;
         Baroclinic = baroclinic;
      }

      public IterationMethodResult Barotropic { get; private set; }
      public Complex[,][] Baroclinic { get; private set; }
   }
}