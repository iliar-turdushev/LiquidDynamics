namespace LiquidDynamics.Forms.IssykKul.Wind
{
   internal sealed class WindParameters
   {
      public WindParameters(
         double a1, double a2, double a3, double a4,
         double b1, double b2, double b3, double b4,
         double a1x, double a2x, double a3x, double a4x,
         double a1y, double a2y, double a3y, double a4y,
         double b1x, double b2x, double b3x, double b4x,
         double b1y, double b2y, double b3y, double b4y
         )
      {
         A1 = a1;
         A2 = a2;
         A3 = a3;
         A4 = a4;
         B1 = b1;
         B2 = b2;
         B3 = b3;
         B4 = b4;
         A1x = a1x;
         A2x = a2x;
         A3x = a3x;
         A4x = a4x;
         A1y = a1y;
         A2y = a2y;
         A3y = a3y;
         A4y = a4y;
         B1x = b1x;
         B2x = b2x;
         B3x = b3x;
         B4x = b4x;
         B1y = b1y;
         B2y = b2y;
         B3y = b3y;
         B4y = b4y;
      }

      public double A1 { get; private set; }
      public double A2 { get; private set; }
      public double A3 { get; private set; }
      public double A4 { get; private set; }

      public double B1 { get; private set; }
      public double B2 { get; private set; }
      public double B3 { get; private set; }
      public double B4 { get; private set; }

      public double A1x { get; private set; }
      public double A2x { get; private set; }
      public double A3x { get; private set; }
      public double A4x { get; private set; }

      public double A1y { get; private set; }
      public double A2y { get; private set; }
      public double A3y { get; private set; }
      public double A4y { get; private set; }

      public double B1x { get; private set; }
      public double B2x { get; private set; }
      public double B3x { get; private set; }
      public double B4x { get; private set; }

      public double B1y { get; private set; }
      public double B2y { get; private set; }
      public double B3y { get; private set; }
      public double B4y { get; private set; }
   }
}
