using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ControlLibrary.Graphs;
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

      private const int XAxisNameOffset = 35;
      private const int YAxisNameOffset = 10;

      private const int ShiftFromLegend = 30;
      private const int LegendOffset = 20;
      private const int LegendElementLength = 25;
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

      private static readonly Font CaptionFont = new Font("Segoe UI", 14);
      private static readonly Brush CaptionBrush = Brushes.Black;

      private const string LabelFormat = "F3";
      private static readonly Font LabelFont = new Font("Segoe UI", 8);
      private static readonly Brush LabelBrush = Brushes.Blue;

      private static readonly Font LegendFont = new Font("Segoe UI", 10);
      private static readonly Brush LegendBrush = Brushes.Black;

      private static readonly Font AxisNameFont = new Font("Segoe UI", 10, FontStyle.Bold);
      private static readonly Brush AxisNameBrush = Brushes.Black;

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
      public string XAxisName { get; set; }
      public string YAxisName { get; set; }
      public Bounds AxisBounds { get; set; }

      public void Clear()
      {
         _drawers.Clear();
         _legendItems.Clear();
      }

      public void DrawVelocityField(SquareVelocityField velocityField, Pen vectorPen)
      {
         _drawers.Add(new SquareVelocityFieldDrawer(velocityField, vectorPen));
      }

      public void DrawVectorField(
         SquareVelocityField vectorField,
         Pen vectorPen,
         IPaletteDrawingTools colorMap)
      {
         _drawers.Add(new VectorFieldDrawer(vectorField, vectorPen, colorMap));
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
         Graphics g = args.Graphics;
         g.SmoothingMode = SmoothingMode.AntiAlias;

         var rectangle = new Rectangle(OffsetLeft, OffsetTop,
                                       DisplayRectangle.Width - (OffsetLeft + OffsetRight),
                                       DisplayRectangle.Height - (OffsetTop + getOffsetBottom()));

         drawCaption(g);
         drawAxisBars(g);
         drawAxisNames(g);
         drawLegend(g);
         drawGraphs(g, rectangle);
         drawAxisBox(g, rectangle);
      }

      protected override void OnResize(EventArgs e)
      {
         base.OnResize(e);
         Invalidate();
      }

      private void drawCaption(Graphics g)
      {
         if (!string.IsNullOrEmpty(Caption))
         {
            SizeF size = g.MeasureString(Caption, CaptionFont);
            float x = (DisplayRectangle.Width - size.Width) / 2;
            float y = (OffsetTop - size.Height) / 2;
            g.DrawString(Caption, CaptionFont, CaptionBrush, x, y);
         }
      }

      private void drawAxisBars(Graphics g)
      {
         const float xStart = OffsetLeft;
         const float yStart = OffsetTop;
         float xEnd = DisplayRectangle.Width - OffsetRight;
         float yEnd = DisplayRectangle.Height - getOffsetBottom();
         const float x = VerticalBarOffsetLeft;
         float y = DisplayRectangle.Height - getHorizontalBarOffsetBottom();

         g.DrawLine(AxisPen, xStart, y, xEnd, y);
         g.DrawLine(AxisPen, x, yStart, x, yEnd);
         
         var drawMajorXTick = getHorizontalTickDrawer(g, MajorTicksPen, y, MajorTicksLength);
         var drawMajorYTick = getVerticalTickDrawer(g, MajorTicksPen, x, MajorTicksLength);
         var drawMinorXTick = getHorizontalTickDrawer(g, MinorTicksPen, y, MinorTicksLength);
         var drawMinorYTick = getVerticalTickDrawer(g, MinorTicksPen, x, MinorTicksLength);

         var drawHorizontalLabel = getHorizontalLabelDrawer(g, y + MajorTicksLength + TickLabelOffset);
         var drawVerticalLabel = getVerticalLabelDrawer(g, x - MajorTicksLength - TickLabelOffset);

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

      private void drawAxisBox(Graphics drawingContext, Rectangle rectangle)
      {
         drawingContext.DrawRectangle(AxisBoxPen, rectangle);
      }

      private void drawAxisNames(Graphics g)
      {
         if (!string.IsNullOrEmpty(XAxisName))
         {
            SizeF sz = g.MeasureString(XAxisName, AxisNameFont);
            float x = (DisplayRectangle.Width - OffsetRight + OffsetLeft - sz.Width) / 2;
            float y = DisplayRectangle.Height - getHorizontalBarOffsetBottom() + XAxisNameOffset;
            g.DrawString(XAxisName, AxisNameFont, AxisNameBrush, x, y);
         }

         if (!string.IsNullOrEmpty(YAxisName))
         {
            var fmt = new StringFormat(StringFormatFlags.DirectionVertical);
            SizeF sz = g.MeasureString(YAxisName, AxisNameFont, Point.Empty, fmt);
            float y = (DisplayRectangle.Height + OffsetTop - getOffsetBottom() - sz.Height) / 2;
            g.DrawString(YAxisName, AxisNameFont, AxisNameBrush, YAxisNameOffset, y, fmt);
         }
      }

      private void drawLegend(Graphics g)
      {
         if (_legendItems.Count == 0)
            return;

         float x = OffsetLeft;
         float y = DisplayRectangle.Height - LegendOffset;

         foreach (LegendItem li in _legendItems)
         {
            float xEnd = x + LegendElementLength;
            g.DrawLine(li.Pen, x, y, xEnd, y);

            SizeF size = g.MeasureString(li.Text, LegendFont);
            g.DrawString(li.Text, LegendFont, LegendBrush, xEnd + LegendTextStep, y - size.Height / 2);

            x += LegendElementLength + LegendItemsStep + size.Width;
         }
      }

      private void drawGraphs(Graphics g, Rectangle rect)
      {
         if (_drawers.Count == 0)
            return;

         g.SetClip(rect);
         var c = new CoordinatesConverter(rect, AxisBounds);
         _drawers.ForEach(drawer => drawer.Draw(g, c));
         g.ResetClip();
      }

      private int getOffsetBottom()
      {
         return OffsetBottom + (_legendItems.Count == 0 ? 0 : ShiftFromLegend);
      }

      private int getHorizontalBarOffsetBottom()
      {
         return HorizontalBarOffsetBottom + (_legendItems.Count == 0 ? 0 : ShiftFromLegend);
      }

      private static Action<float> getHorizontalTickDrawer(Graphics g, Pen pen, float y, float tickLen)
      {
         return x => g.DrawLine(pen, x, y - tickLen, x, y + tickLen);
      }

      private static Action<float> getVerticalTickDrawer(Graphics g, Pen pen, float x, float tickLen)
      {
         return y => g.DrawLine(pen, x - tickLen, y, x + tickLen, y);
      }

      private Action<float, float> getHorizontalLabelDrawer(Graphics g, float top)
      {
         return (x, value) =>
                   {
                      string text = value.ToString(LabelFormat);
                      SizeF size = g.MeasureString(text, LabelFont);
                      g.DrawString(text, LabelFont, LabelBrush, x - size.Width / 2, top);
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
   }
}
