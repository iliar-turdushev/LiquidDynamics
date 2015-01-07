using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ControlLibrary.Drawing;

namespace ControlLibrary.Controls
{
   public sealed class PaletteControl : UserControl
   {
      #region Constants

      private const int OffsetLeft = 10;
      private const int OffsetTop = 10;
      private const int OffsetRight = 80;
      private const int OffsetBottom = 10;

      private const int AxisBarOffsetLeft = 70;

      private const int MajorTicksLength = 6;
      private const int MinorTicksLength = 3;

      private const int MajorTicksCount = 11;
      private const int MinorTicksCount = 3;

      private const int TickLabelOffset = 5;

      private static readonly Pen AxisPen = new Pen(Color.Black, 1);
      private static readonly Pen PaletteBoxPen = new Pen(Color.Black, 1);
      private static readonly Pen MajorTicksPen = new Pen(Color.Black, 1);
      private static readonly Pen MinorTicksPen = new Pen(Color.Black, 1);

      private const string LabelFormat = "F3";
      private static readonly Font LabelFont = new Font("Courier New", 8);
      private static readonly Brush LabelBrush = Brushes.Blue;

      #endregion

      public PaletteControl()
      {
         SetStyle(
            ControlStyles.ResizeRedraw |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true);

         PaletteDrawingTools = PaletteFactory.CreateBluePalette();

         MinValue = 0;
         MaxValue = 1;
      }

      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public IPaletteDrawingTools PaletteDrawingTools { get; set; }

      public float MinValue { get; set; }
      public float MaxValue { get; set; }

      protected override void OnPaint(PaintEventArgs args)
      {
         var drawingContext = args.Graphics;
         drawingContext.SmoothingMode = SmoothingMode.AntiAlias;

         var rectangle = new Rectangle(OffsetLeft, OffsetTop,
                                       DisplayRectangle.Width - OffsetLeft - OffsetRight,
                                       DisplayRectangle.Height - OffsetTop - OffsetBottom);
         drawPaletteBar(drawingContext, rectangle);
         drawingContext.DrawRectangle(PaletteBoxPen, rectangle);
         drawAxisBar(drawingContext);
      }

      protected override void OnResize(EventArgs e)
      {
         base.OnResize(e);
         Invalidate();
      }

      private void drawPaletteBar(Graphics drawingContext, Rectangle rectangle)
      {
         var step = (float) rectangle.Height / (PaletteDrawingTools.MaxDensity - PaletteDrawingTools.MinDensity);
         float x = rectangle.Left;
         var y = rectangle.Bottom - step;

         for (var i = PaletteDrawingTools.MinDensity; i < PaletteDrawingTools.MaxDensity; i++)
         {
            var pen = PaletteDrawingTools.GetPen(i);
            var brush = PaletteDrawingTools.GetBrush(i);
            drawingContext.DrawRectangle(pen, x, y, rectangle.Width, step);
            drawingContext.FillRectangle(brush, x, y, rectangle.Width, step);
            y -= step;
         }
      }

      private void drawAxisBar(Graphics drawingContext)
      {
         float x = DisplayRectangle.Width - AxisBarOffsetLeft;
         const float yStart = OffsetTop;
         float yEnd = DisplayRectangle.Height - OffsetBottom;
         
         drawingContext.DrawLine(AxisPen, x, yStart, x, yEnd);

         var drawMajorYTick = getVerticalTickDrawer(drawingContext, MajorTicksPen, x, MajorTicksLength);
         var drawMinorYTick = getVerticalTickDrawer(drawingContext, MinorTicksPen, x, MinorTicksLength);
         var drawVerticalLabel = getVerticalLabelDrawer(drawingContext, x + MajorTicksLength + TickLabelOffset);

         var yCurrent = yStart;
         var yStep = (yEnd - yStart) / (MajorTicksCount - 1);
         var minorTickYStep = yStep / (MinorTicksCount + 1);
         var yValue = MaxValue;
         var yValueStep = (yValue - MinValue) / (MajorTicksCount - 1);

         for (var i = 0; i < MajorTicksCount; i++)
         {
            drawMajorYTick(yCurrent);
            drawVerticalLabel(yCurrent, yValue);

            if (i == MajorTicksCount - 1)
               break;

            var minorTickYCurrent = yCurrent + minorTickYStep;

            for (var j = 0; j < MinorTicksCount; j++)
            {
               drawMinorYTick(minorTickYCurrent);
               minorTickYCurrent += minorTickYStep;
            }

            yCurrent += yStep;
            yValue -= yValueStep;
         }
      }

      private Action<float> getVerticalTickDrawer(Graphics drawingContext, Pen pen, float x, float tickLength)
      {
         var tickYStart = x - tickLength;
         var tickYEnd = x + tickLength;

         return y => drawingContext.DrawLine(pen, tickYStart, y, tickYEnd, y);
      }

      private Action<float, float> getVerticalLabelDrawer(Graphics drawingContext, float right)
      {
         return (y, value) =>
                   {
                      var text = value.ToString(LabelFormat);
                      var size = drawingContext.MeasureString(text, LabelFont);
                      drawingContext.DrawString(text, LabelFont, LabelBrush, right, y - size.Height / 2);
                   };
      }
   }
}