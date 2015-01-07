using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ControlLibrary.Controls;
using ControlLibrary.Drawing;
using ControlLibrary.Types;
using LiquidDynamics.Properties;
using Tao.OpenGl;

namespace LiquidDynamics.Forms.IssykKul.Grid
{
   internal partial class IssykKulGridForm : Form
   {
      private const int N = 300;
      private const int M = 300;
      private const double Theta = 1;
      
      private const int Graph2D = 0;
      private const int Graph3D = 1;

      private const int NoCut = 0;
      private const int WestEastCut = 1;
      private const int NorthSouthCut = 2;

      private const float PenWidth = 0.01F;
      
      private static readonly Color AxisBackColor = Color.DarkSlateGray;

      private static readonly Pen BordersPen = new Pen(Color.Red, PenWidth);
      private static readonly Brush PointsBrush = Brushes.Green;

      private static readonly Color BordersColor3D = Color.Red;
      private static readonly Color PointsColor3D = Color.Green;

      private static readonly Color RectanglesColor = Color.DarkOrchid;
      private static readonly Pen RectanglesPen = new Pen(RectanglesColor, 0.1F);

      private readonly IssykKulFormDataProvider _dataProvider;

      private Graph3DControl _graphControl3D;
      private IssykKulDisplayData _issykKulDisplayData;
      private InterpolationResult _krigingResult;
      private InterpolationResult _interpolationResult;

      internal IssykKulGridForm()
      {
         InitializeComponent();
         initializeGraphControl();

         _dataProvider = new IssykKulFormDataProvider();
         _comboBoxGraph.SelectedIndex = Graph2D;
         _comboBoxCut.SelectedIndex = NoCut;
      }

      private void initializeGraphControl()
      {
         _graphControl3D = new Graph3DControl(ZDirection.Down)
                              {
                                 Dock = DockStyle.Fill,
                                 DrawAxis = false,
                                 BackColor = AxisBackColor
                              };
         _graphControl3D.Initialize();
         _graphControl3D.Translate(0, 0, -90);
         _graphControl3D.Rotate(0, 130);
         _panel.Controls.Add(_graphControl3D);
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         if (!_buttonSolve.Enabled)
            _buttonSolve.Enabled = true;

         _comboBoxCut.Enabled = false;
         _textBoxCut.Enabled = false;

         try
         {
            var depthFactor = readDepthFactor();

            _issykKulDisplayData = _dataProvider.Initialize(depthFactor);
            _interpolationResult = _dataProvider.Interpolate(N, M, Theta);

            _krigingResult = null;

            clearGraphs();
            drawIssykKul();
            invalidateGraphs();
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      
      private void buttonSolveClick(object sender, EventArgs e)
      {
         _comboBoxCut.Enabled = _comboBoxGraph.SelectedIndex == Graph3D;
         _textBoxCut.Enabled = _comboBoxCut.Enabled && _comboBoxCut.SelectedIndex != NoCut;

         try
         {
            var n = readN();
            var m = readM();
            var dz = readDz();
            
            _krigingResult = _dataProvider.Interpolate(n, m, dz, Theta);

            drawKringingAndIssykKul();
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void comboBoxGraphSelectedIndexChanged(object sender, EventArgs e)
      {
         switch (_comboBoxGraph.SelectedIndex)
         {
            case Graph2D:
               _graphControl.Visible = true;
               _graphControl3D.Visible = false;
               _comboBoxCut.Enabled = false;
               _textBoxCut.Enabled = false;
               break;

            case Graph3D:
               _graphControl.Visible = false;
               _graphControl3D.Visible = true;
               _comboBoxCut.Enabled = _krigingResult != null;
               _textBoxCut.Enabled = _comboBoxCut.Enabled && _comboBoxCut.SelectedIndex != NoCut;
               break;
         }

         invalidateGraphs();
      }

      private void comboBoxCutSelectedIndexChanged(object sender, EventArgs e)
      {
         switch (_comboBoxCut.SelectedIndex)
         {
            case NoCut:
               _textBoxCut.Enabled = false;
               drawKringingAndIssykKul();
               break;

            case WestEastCut:
            case NorthSouthCut:
               _textBoxCut.Enabled = true;
               break;
         }
      }

      private void textBoxCutKeyPress(object sender, KeyPressEventArgs e)
      {
         switch (e.KeyChar)
         {
            case (char) Keys.Return:

               if (tryReadCut())
               {
                  drawKringingAndIssykKul();
               }

               break;
         }
      }

      private void textBoxCutValidating(object sender, CancelEventArgs e)
      {
         e.Cancel = !tryReadCut();
      }
      
      private void buttonBathymetryCheckedChanged(object sender, EventArgs e)
      {
         drawKringingAndIssykKul();
      }

      private void buttonReliefCheckedChanged(object sender, EventArgs e)
      {
         drawKringingAndIssykKul();
      }

      private bool tryReadCut()
      {
         try
         {
            readCut();
            return true;
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
      }

      private void clearGraphs()
      {
         _graphControl.Clear();
         _graphControl3D.Clear();
      }

      private void invalidateGraphs()
      {
         switch (_comboBoxGraph.SelectedIndex)
         {
            case Graph2D:
               _graphControl.Invalidate();
               break;

            case Graph3D:
               _graphControl3D.Invalidate();
               break;
         }
      }

      private void drawIssykKul()
      {
         if (_buttonBathymetry.Checked && _issykKulDisplayData != null)
         {
            drawIssykKul3D();
         }

         if (_buttonRelief.Checked && _interpolationResult != null)
         {
            drawIssykKulInterpolation2D();
            drawIssykKulInterpolation3D();
         }

         if (_buttonBathymetry.Checked && _issykKulDisplayData != null)
         {
            drawIssykKul2D();
         }
      }

      private void drawIssykKul2D()
      {
         var borders =
            _issykKulDisplayData
               .Borders
               .Select(border => border.Select(point => new PointF(point.X, point.Y)).ToArray())
               .ToArray();

         var points =
            _issykKulDisplayData
               .Points
               .Select(point => new PointF(point.X, point.Y))
               .ToArray();

         _graphControl.AxisBounds = _issykKulDisplayData.Bounds;
         
         foreach (var border in borders)
         {
            _graphControl.DrawCurve(border, BordersPen);
         }

         _graphControl.DrawPoints(points, PointsBrush);
      }

      private void drawIssykKul3D()
      {
         var shiftPoint = getShiftPoint(_issykKulDisplayData.Bounds);

         foreach (var border in _issykKulDisplayData.Borders)
         {
            var pointsToDraw = border.Select(point => point.Shift(shiftPoint)).ToArray();
            _graphControl3D.DrawCurve(new Curve3D(pointsToDraw), BordersColor3D);
         }

         var points = _issykKulDisplayData.Points.Select(point => point.Shift(shiftPoint)).ToArray();
         _graphControl3D.DrawPoints(points, PointsColor3D);
      }

      private void drawIssykKulInterpolation2D()
      {
         CellInfo[,] grid = _interpolationResult.Cells;
         float min = _interpolationResult.MinDepth;
         float max = _interpolationResult.MaxDepth;

         IPaletteDrawingTools palette = PaletteFactory.CreateBluePalette();
         var pens = new Dictionary<int, Pen>();
         var brushes = new Dictionary<int, SolidBrush>();

         foreach (CellInfo cell in grid)
         {
            if (cell == null)
               continue;

            int density = palette.ToDensity(cell.Depth, min, max);
            Color color = palette.GetColor(density);

            Pen pen;
            if (!pens.TryGetValue(density, out pen))
               pens.Add(density, pen = new Pen(color, PenWidth));

            SolidBrush brush;
            if (!brushes.TryGetValue(density, out brush))
               brushes.Add(density, brush = new SolidBrush(color));

            _graphControl.DrawFilledRectangle(cell.Rectangle, pen, brush);
         }
      }

      private void drawIssykKulInterpolation3D()
      {
         _graphControl3D.DrawCustom(drawIssykKulRelief3D);
      }

      private void drawIssykKulRelief3D()
      {
         int n = _interpolationResult.N;
         int m = _interpolationResult.M;

         Point3D shiftPoint = getShiftPoint(_issykKulDisplayData.Bounds);
         float hx = _interpolationResult.CellSize.Width;
         float hy = _interpolationResult.CellSize.Height;

         IPaletteDrawingTools palette = PaletteFactory.CreateBluePalette();
         float max = _interpolationResult.MaxDepth;
         float min = _interpolationResult.MinDepth;

         Gl.glEnable(Gl.GL_COLOR_MATERIAL);
         Gl.glEnable(Gl.GL_LIGHTING);
         Gl.glEnable(Gl.GL_LIGHT0);
         Gl.glEnable(Gl.GL_NORMALIZE);
         Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);

         CellInfo[,] cells = _interpolationResult.Cells;

         for (int i = 0; i < n - 1; i++)
         {
            for (int j = 0; j < m - 1; j++)
            {
               if (cells[i, j] == null)
                  continue;

               RectangleF r1 = cells[i, j].Rectangle;
               var r2 = new RectangleF(r1.X + hx, r1.Y, hx, hy);
               var r3 = new RectangleF(r1.X, r1.Y + hy, hx, hy);
               var r4 = new RectangleF(r1.X + hx, r1.Y + hy, hx, hy);

               float d1 = cells[i, j].Depth;
               float d2 = cells[i + 1, j] == null ? 0 : cells[i + 1, j].Depth;
               float d3 = cells[i, j + 1] == null ? 0 : cells[i, j + 1].Depth;
               float d4 = cells[i + 1, j + 1] == null ? 0 : cells[i + 1, j + 1].Depth;

               Gl.glBegin(Gl.GL_TRIANGLE_STRIP);

               Color c1 = palette.GetColor(palette.ToDensity(d1, min, max));
               Gl.glColor3ub(c1.R, c1.G, c1.B);
               Gl.glVertex3f(r1.X + hx / 2 + shiftPoint.X, r1.Y - hy / 2 + shiftPoint.Y, d1);

               Color c2 = palette.GetColor(palette.ToDensity(d2, min, max));
               Gl.glColor3ub(c2.R, c2.G, c2.B);
               Gl.glVertex3f(r2.X + hx / 2 + shiftPoint.X, r2.Y - hy / 2 + shiftPoint.Y, d2);

               Color c3 = palette.GetColor(palette.ToDensity(d3, min, max));
               Gl.glColor3ub(c3.R, c3.G, c3.B);
               Gl.glVertex3f(r3.X + hx / 2 + shiftPoint.X, r3.Y - hy / 2 + shiftPoint.Y, d3);

               Color c4 = palette.GetColor(palette.ToDensity(d4, min, max));
               Gl.glColor3ub(c4.R, c4.G, c4.B);
               Gl.glVertex3f(r4.X + hx / 2 + shiftPoint.X, r4.Y - hy / 2 + shiftPoint.Y, d4);

               Gl.glEnd();
            }
         }

         Gl.glDisable(Gl.GL_NORMALIZE);
         Gl.glDisable(Gl.GL_LIGHT0);
         Gl.glDisable(Gl.GL_LIGHTING);
         Gl.glDisable(Gl.GL_COLOR_MATERIAL);
      }

      private void drawKringingAndIssykKul()
      {
         clearGraphs();

         if (_buttonBathymetry.Checked && _issykKulDisplayData != null)
         {
            drawIssykKul3D();
         }
         
         if (_buttonRelief.Checked && _interpolationResult != null)
         {
            drawIssykKulInterpolation2D();
            drawIssykKulInterpolation3D();
         }

         if (_krigingResult != null)
         {
            float max = _krigingResult.MaxDepth;
            float min = _krigingResult.MinDepth;

            drawKrigingResult2D();
            drawKrigingResult3D();

            _paletteControl.MinValue = min;
            _paletteControl.MaxValue = max;
            _paletteControl.Invalidate();
         }

         if (_buttonBathymetry.Checked && _issykKulDisplayData != null)
         {
            drawIssykKul2D();
         }

         invalidateGraphs();
      }

      private void drawKrigingResult2D()
      {
         foreach (CellInfo cell in _krigingResult.Cells)
         {
            if (cell == null)
               continue;

            _graphControl.DrawRectangle(cell.Rectangle, RectanglesPen);
         }
      }

      private void drawKrigingResult3D()
      {
         Func<int, int, bool> filter;

         if (_comboBoxCut.SelectedIndex == NoCut)
         {
            filter = (column, row) => true;
         }
         else
         {
            int cut = readCut();

            if (_comboBoxCut.SelectedIndex == NorthSouthCut)
               filter = (column, row) => column == cut;
            else
               filter = (column, row) => row == cut;
         }

         Point3D shiftPoint = getShiftPoint(_issykKulDisplayData.Bounds);
         Rectangle3D[,][] depthRectangles = _krigingResult.DepthRectangles;

         for (int i = 0; i < _krigingResult.N; i++)
         {
            for (int j = 0; j < _krigingResult.M; j++)
            {
               if (!filter(i, j))
                  continue;

               if (depthRectangles[i, j] == null)
                  continue;

               Rectangle3D[] rectanglesTranslated =
                  depthRectangles[i, j]
                     .Select(rectangle => rectangle.Shift(shiftPoint))
                     .ToArray();
               _graphControl3D.DrawRectangles(rectanglesTranslated, RectanglesColor);
            }
         }
      }

      private static double readDoubleValue(string textToRead, string parameterName)
      {
         double value;

         if (!double.TryParse(textToRead, out value))
         {
            generateError(parameterName);
         }

         return value;
      }

      private static int readIntValue(string textToRead, string parameterName)
      {
         int value;

         if (!int.TryParse(textToRead, out value))
         {
            generateError(parameterName);
         }

         return value;
      }

      private static void generateError(string parameterName)
      {
         var message = string.Format(Resources.InvalidParameterValue, parameterName);
         throw new InvalidFieldValueException(message);
      }

      private double readDepthFactor()
      {
         return readDoubleValue(_textBoxDepthFactor.Text, "Множитель глубины");
      }

      private int readN()
      {
         return readIntValue(_textBoxN.Text, "N");
      }

      private int readM()
      {
         return readIntValue(_textBoxM.Text, "M");
      }

      private double readDz()
      {
         return readDoubleValue(_textBoxDz.Text, "Dz");
      }
      
      private int readCut()
      {
         return readIntValue(_textBoxCut.Text, "Срез");
      }

      private static Point3D getShiftPoint(Bounds bounds)
      {
         var hx = -(bounds.XMax - bounds.XMin) / 2;
         var hy = -(bounds.YMax - bounds.YMin) / 2;
         return new Point3D(hx, hy, 0);
      }
   }
}
