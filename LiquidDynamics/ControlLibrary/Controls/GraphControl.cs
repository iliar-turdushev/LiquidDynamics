using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ControlLibrary.Drawing;
using ControlLibrary.Graphs.Graphs2D;
using ControlLibrary.Types;

namespace ControlLibrary.Controls
{
   public sealed class GraphControl : UserControl
   {
      #region Constans

      private const int OffsetLeft = 90;
      private const int OffsetRight = 40;
      private const int OffsetTop = 70;
      private const int OffsetBottom = 70;

      private const int HorizontalBarOffsetBottom = 60;
      private const int VerticalBarOffsetLeft = 80;

      private const int ShiftFromLegend = 30;
      private const int LegendOffset = 35;
      private const int LegendElementLength = 40;
      private const int LegendItemsStep = 20;
      private const int LegendTextStep = 5;

      private const int MajorTicksLength = 6;
      private const int MinorTicksLength = 3;

      private const int MajorTicksCount = 11;
      private const int MinorTicksCount = 3;

      private const int TickLabelOffset = 5;

      private static readonly Pen AxisBoxPen = new Pen(Color.Black, 1);
      private static readonly Pen AxisPen = new Pen(Color.Black, 1);
      private static readonly Pen MajorTicksPen = new Pen(Color.Black, 1);
      private static readonly Pen MinorTicksPen = new Pen(Color.Black, 1);

      private static readonly Font CaptionFont = new Font("Courier New", 10);
      private static readonly Brush CaptionBrush = Brushes.Blue;

      private const string LabelFormat = "F3";
      private static readonly Font LabelFont = new Font("Courier New", 8);
      private static readonly Brush LabelBrush = Brushes.Blue;

      private static readonly Font LegendFont = new Font("Courier New", 10);
      private static readonly Brush LegendBrush = Brushes.Blue;

      #endregion

      private readonly List<IGraphDrawer> _drawers;
      private readonly List<LegendItem> _legendItems; 

      public GraphControl()
      {
         SetStyle(
            ControlStyles.ResizeRedraw |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true);

         _drawers = new List<IGraphDrawer>();
         _legendItems = new List<LegendItem>();
         AxisBounds = new Bounds(0, 1, 0, 1);
      }

      public string Caption { get; set; }
      public Bounds AxisBounds { get; set; }

      public void Clear()
      {
         _drawers.Clear();
      }

      public void DrawVelocityField(SquareVelocityField velocityField, Pen vectorPen)
      {
         _drawers.Add(new SquareVelocityFieldDrawer(velocityField, vectorPen));
      }

      public void DrawVelocityField(
         SquareVelocityField velocityField, IPaletteDrawingTools paletteDrawingTools, Pen vectorPen,
         VectorScalingMode vectorScalingMode = VectorScalingMode.ScaleAllVectors)
      {
         _drawers.Add(new PalettedSquareVelocityFieldDrawer(velocityField, paletteDrawingTools, vectorPen, vectorScalingMode));
      }

      public void DrawUpwelling(UpwellingData upwellingData, IPaletteDrawingTools paletteDrawingTools)
      {
         _drawers.Add(new UpwellingDrawer(upwellingData, paletteDrawingTools));
      }

      public void DrawCurve(PointF[] points, Pen pen)
      {
         _drawers.Add(new CurveDrawer(points, pen));
      }

      public void AddLegend(string text, Pen pen)
      {
         _legendItems.Add(new LegendItem(text, pen));
      }

      public void DrawRectangle(RectangleF rectangle, Pen pen)
      {
         _drawers.Add(new RectangleDrawer(rectangle, pen));
      }

      public void DrawFilledRectangle(RectangleF rectangle, Pen borderPen, Brush brush)
      {
         _drawers.Add(new FilledRectangleDrawer(rectangle, borderPen, brush));
      }

      public void DrawPoints(PointF[] points, Brush brush)
      {
         _drawers.Add(new PointsDrawer(points, brush));
      }

      public void DrawLines(PointF[] points, Pen pen)
      {
         _drawers.Add(new LinesDrawer(points, pen));
      }

      protected override void OnPaint(PaintEventArgs args)
      {
         var drawingContext = args.Graphics;
         drawingContext.SmoothingMode = SmoothingMode.AntiAlias;

         var rectangle = new Rectangle(OffsetLeft, OffsetTop,
                                       DisplayRectangle.Width - (OffsetLeft + OffsetRight),
                                       DisplayRectangle.Height - (OffsetTop + getOffsetBottom()));

         if (!string.IsNullOrEmpty(Caption))
            drawCaption(drawingContext);

         if (AxisBounds != null)
            drawAxisBars(drawingContext);
         
         if (_drawers.Count > 0)
            drawGraphs(drawingContext, rectangle);

         drawAxisBox(drawingContext, rectangle);

         if (_legendItems.Count > 0)
            drawLegend(drawingContext);
      }

      protected override void OnResize(EventArgs e)
      {
         base.OnResize(e);
         Invalidate();
      }

      private void drawCaption(Graphics drawingContext)
      {
         var size = drawingContext.MeasureString(Caption, CaptionFont);
         var x = (DisplayRectangle.Width - size.Width) / 2;
         var y = (OffsetTop - size.Height) / 2;
         drawingContext.DrawString(Caption, CaptionFont, CaptionBrush, x, y);
      }

      private void drawAxisBox(Graphics drawingContext, Rectangle rectangle)
      {
         drawingContext.DrawRectangle(AxisBoxPen, rectangle);
      }

      private void drawAxisBars(Graphics drawingContext)
      {
         const float xStart = OffsetLeft;
         const float yStart = OffsetTop;
         float xEnd = DisplayRectangle.Width - OffsetRight;
         float yEnd = DisplayRectangle.Height - getOffsetBottom();
         const float x = VerticalBarOffsetLeft;
         float y = DisplayRectangle.Height - getHorizontalBarOffsetBottom();

         drawingContext.DrawLine(AxisPen, xStart, y, xEnd, y);
         drawingContext.DrawLine(AxisPen, x, yStart, x, yEnd);
         
         var drawMajorXTick = getHorizontalTickDrawer(drawingContext, MajorTicksPen, y, MajorTicksLength);
         var drawMajorYTick = getVerticalTickDrawer(drawingContext, MajorTicksPen, x, MajorTicksLength);
         var drawMinorXTick = getHorizontalTickDrawer(drawingContext, MinorTicksPen, y, MinorTicksLength);
         var drawMinorYTick = getVerticalTickDrawer(drawingContext, MinorTicksPen, x, MinorTicksLength);

         var drawHorizontalLabel = getHorizontalLabelDrawer(drawingContext, y + MajorTicksLength + TickLabelOffset);
         var drawVerticalLabel = getVerticalLabelDrawer(drawingContext, x - MajorTicksLength - TickLabelOffset);

         var xCurrent = xStart;
         var xStep = (xEnd - xStart) / (MajorTicksCount - 1);
         var yCurrent = yStart;
         var yStep = (yEnd - yStart) / (MajorTicksCount - 1);

         var minorTickXStep = xStep / (MinorTicksCount + 1);
         var minorTickYStep = yStep / (MinorTicksCount + 1);

         var xValue = AxisBounds.XMin;
         var yValue = AxisBounds.YMax;
         var xValueStep = (AxisBounds.XMax - xValue) / (MajorTicksCount - 1);
         var yValueStep = (yValue - AxisBounds.YMin) / (MajorTicksCount - 1);

         for (var i = 0; i < MajorTicksCount; i++)
         {
            drawMajorXTick(xCurrent);
            drawMajorYTick(yCurrent);

            drawHorizontalLabel(xCurrent, xValue);
            drawVerticalLabel(yCurrent, yValue);

            if (i == MajorTicksCount - 1)
               break;

            var minorTickXCurrent = xCurrent + minorTickXStep;
            var minorTickYCurrent = yCurrent + minorTickYStep;

            for (var j = 0; j < MinorTicksCount; j++)
            {
               drawMinorXTick(minorTickXCurrent);
               drawMinorYTick(minorTickYCurrent);

               minorTickXCurrent += minorTickXStep;
               minorTickYCurrent += minorTickYStep;
            }

            xCurrent += xStep;
            yCurrent += yStep;
            xValue += xValueStep;
            yValue -= yValueStep;
         }
      }

      private void drawGraphs(Graphics drawingContext, Rectangle clipRectangle)
      {
         drawingContext.SetClip(clipRectangle);
         _drawers.ForEach(drawer => drawer.Draw(drawingContext, new CoordinatesConverter(clipRectangle, AxisBounds)));
         drawingContext.ResetClip();
      }

      private Action<float> getHorizontalTickDrawer(Graphics drawingContext, Pen pen, float y, float tickLength)
      {
         var tickXStart = y - tickLength;
         var tickXEnd = y + tickLength;

         return x => drawingContext.DrawLine(pen, x, tickXStart, x, tickXEnd);
      }

      private Action<float> getVerticalTickDrawer(Graphics drawingContext, Pen pen, float x, float tickLength)
      {
         var tickYStart = x - tickLength;
         var tickYEnd = x + tickLength;

         return y => drawingContext.DrawLine(pen, tickYStart, y, tickYEnd, y);
      }

      private Action<float, float> getHorizontalLabelDrawer(Graphics drawingContext, float top)
      {
         return (x, value) =>
                   {
                      var text = value.ToString(LabelFormat);
                      var size = drawingContext.MeasureString(text, LabelFont);
                      drawingContext.DrawString(text, LabelFont, LabelBrush, x - size.Width / 2, top);
                   };
      }

      private Action<float, float> getVerticalLabelDrawer(Graphics drawingContext, float right)
      {
         return (y, value) =>
                   {
                      var text = value.ToString(LabelFormat);
                      var size = drawingContext.MeasureString(text, LabelFont);
                      drawingContext.DrawString(text, LabelFont, LabelBrush, right - size.Width, y - size.Height / 2);
                   };
      }

      private void drawLegend(Graphics drawingContext)
      {
         float x = OffsetLeft;
         float y = DisplayRectangle.Height - LegendOffset;

         foreach (var legendItem in _legendItems)
         {
            var xEnd = x + LegendElementLength;
            drawingContext.DrawLine(legendItem.Pen, x, y, xEnd, y);

            var size = drawingContext.MeasureString(legendItem.Text, LegendFont);
            drawingContext.DrawString(legendItem.Text, LegendFont, LegendBrush,
                                      xEnd + LegendTextStep, y - size.Height / 2);
            
            x += LegendElementLength + LegendItemsStep + size.Width;
         }
      }

      private int getOffsetBottom()
      {
         return OffsetBottom + (_legendItems.Count == 0 ? 0 : ShiftFromLegend);
      }

      private int getHorizontalBarOffsetBottom()
      {
         return HorizontalBarOffsetBottom + (_legendItems.Count == 0 ? 0 : ShiftFromLegend);
      }
   }
}
