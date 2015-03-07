namespace LiquidDynamics.Forms.IssykKul.Wind
{
   internal static class TypicalWindParameters
   {
      internal static readonly WindParameters WindType1 =
         new WindParameters(0, -3000, -12, 40,
                            -1, 11, -1, 1,
                            0.1551, -3.5, 1, 0,
                            0, 0, 0, 0,
                            -1E-11, 0.61, 0.001, 0,
                            0, 0, 0, 0);

      internal static readonly WindParameters WindType2 =
         new WindParameters(0, -3000, -12, 40,
                            -1, 11, -1, 1,
                            0.141, 0, 1, 0.001,
                            0, 0, 0, 0,
                            -1E-11, 0.61, 0.001, 0,
                            0, 0, 0, 0);

      internal static readonly WindParameters WindType3 =
         new WindParameters(-150, -3000, -12, 40,
                            -3, 9, -1, 1,
                            0.11, 0, 1, 0.001,
                            0, 0, 0, 0,
                            -1E-11, 0.61, 0.001, 0,
                            0, 0, 0, 0);

      internal static readonly WindParameters WindType4 =
         new WindParameters(-150, -3000, -50, 100,
                            -3, 9, -1, 1,
                            0.11, 0, 1, 0.001,
                            0, 0, 0, 0,
                            -0.1, 1, 0, 0,
                            0, 0, 0, 0);

      internal static readonly WindParameters WindType5 =
         new WindParameters(-300, -3000, -500, -5000,
                            -3, 9, 0.1, -1,
                            0.318, 0, 1, 1E-9,
                            0, 0, 0, 0,
                            0.115, 0.61, 0.1, -0.055,
                            0, 0, 0, 0);

      internal static readonly WindParameters WindType6 =
         new WindParameters(80, -3000, -60, 40,
                            -0.5, -10, -1, 1,
                            1.27, 0, 0.05, 0,
                            0, 0, 0, 0,
                            -0.1, 0.61, 0.001, 0,
                            0, 0, 0, 0);

      internal static readonly WindParameters WindType7 =
         new WindParameters(100, -3000, -20, 100,
                            -0.1, 5, -0.9, 1,
                            1.654, 0, 0, 0,
                            0, 0, 0, 0,
                            -0.1, 1, 0, 0,
                            0, 0, 0, 0);

      internal static readonly WindParameters WindType8 =
         new WindParameters(100, -3000, -60, 40,
                            -0.1, 5, -1, 1,
                            1E-10, 0, 0.1, 0,
                            0, 0, 0, 0,
                            -0.065, 0.61, 0.001, 0,
                            0, 0, 0, 0);

      internal static readonly WindParameters WindType9 =
         new WindParameters(100, -3000, -1000, 28,
                            -0.1, 5, -0.5, 1,
                            1E-10, 0, 0.1, 0,
                            0, 0, 0, 0,
                            -0.115, 0.61, 0, 0,
                            0, 0, 0, 0);
   }
}