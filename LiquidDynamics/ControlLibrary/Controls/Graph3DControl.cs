using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ControlLibrary.Graphs.Graphs3D;
using ControlLibrary.Types;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace ControlLibrary.Controls
{
   public sealed class Graph3DControl : SimpleOpenGlControl
   {
      // Коэффициент переноса.
      private const float TranslateFactor = 0.025F;
      // Коэффициент переноса по оси Z.
      private const float TranslateZFactor = 0.0025F;
      // Коэффициент поворота.
      private const float RotateFactor = 0.15F;
      // Коэффициент масшатибирования.
      private const float ScaleFactor = 1;

      // Положение курсора на контроле.
      private PointF _cursorPosition;

      // Константа переноса по оси X.
      private float _translateX;
      // Константа переноса по оси Y.
      private float _translateY;
      // Константа переноса по оси Z.
      private float _translateZ;

      // Угол поворота вокруг оси X.
      private float _angleX;
      // Угол поворота вокруг оси Y.
      private const float AngleY = 0;
      // Угол поворота вокруг оси Z.
      private float _angleZ;

      // Направление оси OZ.
      private readonly ZDirection _zDirection;

      // Цвет осей координат.
      private static readonly Color AxisColor = Color.GreenYellow;
      // Цвет стрелок осей координат.
      private static readonly Color AxisArrowColor = Color.Green;

      private readonly List<IGraphDrawer3D> _graphDrawers;
      private static bool _isGlutInitialized;

      public Graph3DControl(ZDirection zDirection)
      {
         SetStyle(ControlStyles.ResizeRedraw, true);
         
         _cursorPosition = new PointF();

         _translateX = 0;
         _translateY = 0;
         _translateZ = -35;

         _angleX = -65;
         _angleZ = -135;
         
         _zDirection = zDirection;

         DrawAxis = true;
         GraphBounds = new Bounds3D(-10, 10, -10, 10, -10, 10);

         _graphDrawers = new List<IGraphDrawer3D>();
      }

      public bool DrawAxis { get; set; }

      public Bounds3D GraphBounds { get; set; }

      public void Initialize()
      {
         initializeGlut();

         InitializeContexts();
         initializeScene();

         setViewport();
         setProjection();
      }

      public void Clear()
      {
         _graphDrawers.Clear();
      }

      public void Translate(float dx, float dy, float dz)
      {
         _translateX += dx;
         _translateY += dy;
         _translateZ += dz;
      }

      public void Rotate(float rx, float rz)
      {
         _angleX += rx;
         _angleZ += rz;
      }

      public void DrawCurve(Curve3D curve, Color graphColor)
      {
         _graphDrawers.Add(new Curve3DDrawer(curve, graphColor));
      }

      public void DrawSurface(Surface3D surface, Color color)
      {
         _graphDrawers.Add(new Surface3DDrawer(surface, color));
      }

      public void DrawPoints(Point3D[] points, Color color)
      {
         _graphDrawers.Add(new Points3DDrawer(points, color));
      }

      public void DrawLines(Line3D[] lines, Color color)
      {
         _graphDrawers.Add(new Lines3DDrawer(lines, color));
      }

      public void DrawRectangles(Rectangle3D[] rectangles, Color color)
      {
         _graphDrawers.Add(new Rectangles3DDrawer(rectangles, color));
      }

      public void DrawCustom(Action action)
      {
         _graphDrawers.Add(new CustomDrawer(action));
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         base.OnPaint(e);

         Gl.glClearColor(
                BackColor.R / 255.0F, BackColor.G / 255.0F,
                BackColor.B / 255.0F, BackColor.A / 255.0F);
         Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
         Gl.glMatrixMode(Gl.GL_MODELVIEW);
         Gl.glLoadIdentity();

         Gl.glTranslatef(_translateX, _translateY, _translateZ);
         Gl.glRotatef(_angleX, 1, 0, 0);
         Gl.glRotatef(AngleY, 0, 1, 0);
         Gl.glRotatef(_angleZ, 0, 0, 1);

         var x = -500 * (float) Math.Sin((_angleZ * Math.PI) / 180);
         var y = -500 * (float) Math.Cos((_angleZ * Math.PI) / 180);
         Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new[] {x, y, -300, 0});

         var coefficient = _zDirection == ZDirection.Up ? 1 : -1;
         Gl.glScalef(ScaleFactor, ScaleFactor, coefficient * ScaleFactor);

         if (DrawAxis)
         {
            drawAxis();
         }

         drawGraphs();
         
         SwapBuffers();
      }

      protected override void OnSizeChanged(EventArgs e)
      {
         base.OnSizeChanged(e);
         setViewport();
         setProjection();
         Invalidate();
      }

      protected override void OnMouseDown(MouseEventArgs e)
      {
         base.OnMouseDown(e);
         _cursorPosition.X = e.X;
         _cursorPosition.Y = e.Y;
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
         base.OnMouseMove(e);

         if (e.Button == MouseButtons.Left)
         {
            _angleX += (e.Y - _cursorPosition.Y) * RotateFactor;
            _angleZ += (e.X - _cursorPosition.X) * RotateFactor;
            _cursorPosition.X = e.X;
            _cursorPosition.Y = e.Y;
            Invalidate();
         }
         else if (e.Button == MouseButtons.Right)
         {
            _translateX += (e.X - _cursorPosition.X) * TranslateFactor;
            _translateY -= (e.Y - _cursorPosition.Y) * TranslateFactor;
            _cursorPosition.X = e.X;
            _cursorPosition.Y = e.Y;
            Invalidate();
         }
      }

      protected override void OnMouseWheel(MouseEventArgs e)
      {
         base.OnMouseWheel(e);
         _translateZ += TranslateZFactor * e.Delta;
         Invalidate();
      }

      private static void initializeGlut()
      {
         if (!_isGlutInitialized)
         {
            Glut.glutInit();
            _isGlutInitialized = true;
         }

         Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);
      }

      private void initializeScene()
      {
         Gl.glShadeModel(Gl.GL_SMOOTH);
         Gl.glClearDepth(1);
         Gl.glEnable(Gl.GL_DEPTH_TEST);
         Gl.glDepthFunc(Gl.GL_LESS);
         Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
      }

      private void setProjection()
      {
         Gl.glMatrixMode(Gl.GL_PROJECTION);
         Gl.glLoadIdentity();
         Glu.gluPerspective(45, (double) Width / Height, 0.1, 500);
         Gl.glMatrixMode(Gl.GL_MODELVIEW);
         Gl.glLoadIdentity();
      }

      private void setViewport()
      {
         Gl.glViewport(0, 0, Width, Height);
      }

      private void transformDraw(Action trasform, Action draw)
      {
         Gl.glPushMatrix();
         trasform();
         draw();
         Gl.glPopMatrix();
      }

      private void drawAxis()
      {
         Gl.glColor3ub(AxisColor.R, AxisColor.G, AxisColor.B);

         Gl.glBegin(Gl.GL_LINES);
         Gl.glVertex3f(GraphBounds.XMin, 0, 0);
         Gl.glVertex3f(GraphBounds.XMax, 0, 0);
         Gl.glVertex3f(0, GraphBounds.YMin, 0);
         Gl.glVertex3f(0, GraphBounds.YMax, 0);
         Gl.glVertex3f(0, 0, GraphBounds.ZMin);
         Gl.glVertex3f(0, 0, GraphBounds.ZMax);
         Gl.glEnd();

         Gl.glColor3ub(AxisArrowColor.R, AxisArrowColor.G, AxisArrowColor.B);

         transformDraw(
            () =>
               {
                  Gl.glTranslatef(GraphBounds.XMax, 0, 0);
                  Gl.glRotatef(90, 0, 1, 0);
               },
            () => drawAxisArrow(GraphBounds.XMax - GraphBounds.XMin));

         transformDraw(
            () =>
               {
                  Gl.glTranslatef(0, GraphBounds.YMax, 0);
                  Gl.glRotatef(-90, 1, 0, 0);
               },
            () => drawAxisArrow(GraphBounds.YMax - GraphBounds.YMin));

         transformDraw(
            () => Gl.glTranslatef(0, 0, GraphBounds.ZMax),
            () => drawAxisArrow(GraphBounds.ZMax - GraphBounds.ZMin));
      }

      private void drawAxisArrow(float axisLength)
      {
         const int slices = 16;
         const int stacks = 16;
         const float radiusMultiplier = 0.01f;
         const float heightMultiplier = 0.05f;

         var baseRadius = axisLength * radiusMultiplier;
         var height = axisLength * heightMultiplier;

         Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
         Glut.glutSolidCone(baseRadius, height, slices, stacks);
      }

      private void drawGraphs()
      {
         foreach (var graphDrawer in _graphDrawers)
         {
            graphDrawer.Draw();
         }
      }
   }
}