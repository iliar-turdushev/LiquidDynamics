namespace ControlLibrary.Graphs
{
   public static class PaletteFactory
   {
      public static IPaletteDrawingTools CreateBluePalette()
      {
         var palette = new BluePalette();
         palette.Initialize();
         return palette;
      }

      public static IPaletteDrawingTools CreateBlueRedPalette()
      {
         var palette = new BlueRedPalette();
         palette.Initialize();
         return palette;
      }
   }
}