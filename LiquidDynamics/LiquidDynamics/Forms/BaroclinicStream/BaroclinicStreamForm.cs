using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BarotropicComponentProblem;
using Common;
using ControlLibrary.Controls;
using ControlLibrary.Types;
using LiquidDynamics.Forms.BarotropicComponentNumerical;
using LiquidDynamics.Properties;
using Mathematics.MathTypes;
using Mathematics.Numerical;
using ModelProblem;
using ModelProblem.Baroclinic;
using ModelProblem.Barotropic;

namespace LiquidDynamics.Forms.BaroclinicStream
{
   public partial class BaroclinicStreamForm : Form
   {
      private static readonly Pen ExactSolutionPen = new Pen(Color.Green, 1);
      private static readonly Pen CalculatedSolutionPen = new Pen(Color.Red, 1);

      private static readonly Pen ErrorUPen = new Pen(Color.Red, 1)
                                                 {StartCap = LineCap.RoundAnchor, EndCap = LineCap.RoundAnchor};

      private static readonly Pen ErrorVPen = new Pen(Color.Green, 1)
                                                 {StartCap = LineCap.RoundAnchor, EndCap = LineCap.RoundAnchor};

      private readonly ProblemParameters _problemParameters;
      private readonly ModelWind _wind;
      private readonly Solution _solution;

      private double _t;
      private double _tau;

      private int _nx;
      private int _ny;
      private int _nz;

      private int _xCut;
      private int _yCut;

      private Grid _x;
      private Grid _y;
      private Grid _z;
      
      private Complex[,][] _psi0;

      private ErrorContainer _errorContainer;

      private bool _dynamicsAlive;

      public BaroclinicStreamForm(Parameters parameters)
      {
         Check.NotNull(parameters, "parameters");

         _problemParameters = createProblemParameters(parameters);
         _wind = new ModelWind(parameters.F1, parameters.F2,
                               parameters.SmallQ, parameters.SmallR,
                               parameters.Rho0);
         _solution = SolutionCreator.Create(parameters);

         InitializeComponent();
         _comboBoxGraphType.SelectedIndex = 0;
      }

      private void buttonResetClick(object sender, EventArgs e)
      {
         _buttonStartStop.Enabled = true;
         _buttonStep.Enabled = true;

         try
         {
            _tau = readTau();
            _t = _tau;

            _nx = readNx();
            _ny = readNy();
            _nz = readNz();

            _xCut = readXCut();
            _yCut = readYCut();

            _x = Grid.Create(0, _problemParameters.SmallR, _nx);
            _y = Grid.Create(0, _problemParameters.SmallQ, _ny);
            _z = Grid.Create(0, _problemParameters.H, _nz);
            
            _errorContainer = new ErrorContainer();
            _errorContainer.AddError(time: 0, errorU: 0, errorV: 0);

            _psi0 = calculateThetaStream(_t - _tau);

            step();
         }
         catch (InvalidFieldValueException error)
         {
            MessageBox.Show(error.Message, Resources.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void buttonStartStopClick(object sender, EventArgs e)
      {
         _dynamicsAlive = !_dynamicsAlive;
         _timer.Enabled = _dynamicsAlive;

         if (_dynamicsAlive)
         {
            _buttonStartStop.Image = Resources.Pause;
            setButtonsAccessibility(false);
         }
         else
         {
            _buttonStartStop.Image = Resources.Start;
            setButtonsAccessibility(true);
         }
      }

      private void buttonStepClick(object sender, EventArgs e)
      {
         _t += _tau;
         step();
      }

      private void timerTick(object sender, EventArgs e)
      {
         _t += _tau;
         step();
      }

      private ProblemParameters createProblemParameters(Parameters parameters)
      {
         return new ProblemParameters
                   {
                      Beta = parameters.Beta,
                      F1 = parameters.F1,
                      F2 = parameters.F2,
                      H = parameters.H,
                      Mu = parameters.Mu,
                      Nu = parameters.Nu,
                      Rho0 = parameters.Rho0,
                      SmallL0 = parameters.SmallL0,
                      SmallQ = parameters.SmallQ,
                      SmallR = parameters.SmallR
                   };
      }

      private void step()
      {
         Complex[,][] exactPsi = calculateThetaStream(_t);

         double[,] u = calculateU(_t);
         double[,] v = calculateV(_t);
         Complex[,][] theta0 = calculateTheta(_t - _tau);
         Complex[,][] theta = calculateTheta(_t);
         Complex[,][] calculatedPsi = calculateBaroclinicStream(u, v, theta0, theta, _psi0);

         double errorRe;
         double errorIm;
         ErrorCalculator.Calculate(exactPsi, calculatedPsi, out errorRe, out errorIm);

         _errorContainer.AddError(_t, errorRe, errorIm);

         if (_comboBoxGraphType.SelectedIndex == 0)
            drawPsi(exactPsi, calculatedPsi, errorRe, errorIm);
         else if (_comboBoxGraphType.SelectedIndex == 1)
            drawErrors();

         _psi0 = calculatedPsi;
      }

      private double[,] calculateU(double t)
      {
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         return barotropicCalculator(t, barotropic.U);
      }

      private double[,] calculateV(double t)
      {
         IBarotropicComponent barotropic = _solution.GetBarotropicComponent();
         return barotropicCalculator(t, barotropic.V);
      }

      private double[,] barotropicCalculator(double t, Func<double, double, double, double> func)
      {
         var result = new double[_x.Nodes, _y.Nodes];

         for (int i = 0; i < _x.Nodes; i++)
         {
            double x = _x.Get(i);

            for (int j = 0; j < _y.Nodes; j++)
            {
               double y = _y.Get(j);
               result[i, j] = func(t, x, y);
            }
         }

         return result;
      }

      private Complex[,][] calculateTheta(double t)
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();
         return baroclinicCalculator(t, baroclinic.Theta);
      }

      private Complex[,][] calculateThetaStream(double t)
      {
         IBaroclinicComponent baroclinic = _solution.GetBaroclinicComponent();
         return baroclinicCalculator(t, baroclinic.ThetaStream);
      }

      private Complex[,][] baroclinicCalculator(
         double t, Func<double, double, double, double, Complex> func
         )
      {
         var result = new Complex[_x.Nodes,_y.Nodes][];

         for (int i = 0; i < _x.Nodes; i++)
         {
            double x = _x.Get(i);

            for (int j = 0; j < _y.Nodes; j++)
            {
               double y = _y.Get(j);
               result[i, j] = new Complex[_z.Nodes];

               for (int k = 0; k < _z.Nodes; k++)
               {
                  double z = _z.Get(k);
                  result[i, j][k] = func(t, x, y, z);
               }
            }
         }

         return result;
      }

      private Complex[,][] calculateBaroclinicStream(
         double[,] u, double[,] v,
         Complex[,][] theta0, Complex[,][] theta, Complex[,][] psi0
         )
      {
         var c = new BaroclinicStreamCalculator(_x, _y, _z, _tau,
                                                u, v,
                                                theta0, theta, psi0,
                                                _problemParameters, _wind);
         return c.Calculate();
      }

      private void drawPsi(Complex[,][] exactPsi, Complex[,][] calculatedPsi,
                           double errorRe, double errorIm)
      {
         Complex[] ep = exactPsi[_xCut, _yCut];
         Complex[] cp = calculatedPsi[_xCut, _yCut];

         var eu = new PointF[_z.Nodes];
         var ev = new PointF[_z.Nodes];
         var cu = new PointF[_z.Nodes];
         var cv = new PointF[_z.Nodes];

         for (int i = 0; i < _z.Nodes; i++)
         {
            var z = (float) _z.Get(i);

            eu[i] = new PointF(z, (float) ep[i].Re);
            ev[i] = new PointF(z, (float) ep[i].Im);

            cu[i] = new PointF(z, (float) cp[i].Re);
            cv[i] = new PointF(z, (float) cp[i].Im);
         }

         _uGraphControl.AxisBounds = new Bounds(0, (float) _problemParameters.H, -10, 10);
         _uGraphControl.Caption = string.Format("Time = {0:F4}, Error = {1:F4}%", _t, errorRe);
         _uGraphControl.Clear();
         _uGraphControl.DrawCurve(eu, ExactSolutionPen);
         _uGraphControl.DrawCurve(cu, CalculatedSolutionPen);
         _uGraphControl.Invalidate();

         _vGraphControl.AxisBounds = new Bounds(0, (float) _problemParameters.H, -10, 10);
         _vGraphControl.Caption = string.Format("Time = {0:F4}, Error = {1:F4}%", _t, errorIm);
         _vGraphControl.Clear();
         _vGraphControl.DrawCurve(ev, ExactSolutionPen);
         _vGraphControl.DrawCurve(cv, CalculatedSolutionPen);
         _vGraphControl.Invalidate();
      }

      private void drawErrors()
      {
         drawErrors(_uGraphControl, ErrorUPen, _errorContainer.ErrorsU, _errorContainer.MaxErrorU);
         drawErrors(_vGraphControl, ErrorVPen, _errorContainer.ErrorsV, _errorContainer.MaxErrorV);
      }

      private void drawErrors(GraphControl graphControl, Pen pen, PointF[] errorCurve, float maxError)
      {
         graphControl.Clear();

         if (errorCurve.Length > 1)
         {
            graphControl.Caption = string.Format("Time = {0:F4}", _t);
            graphControl.AxisBounds = new Bounds(0.0F, (float) _t, 0.0F, maxError);
            graphControl.DrawLines(errorCurve, pen);
         }

         graphControl.Invalidate();
      }

      private double readTau()
      {
         return readDoubleValue(_textBoxTau.Text, "Tau");
      }

      private int readNx()
      {
         return readIntValue(_textBoxNx.Text, "Nx");
      }

      private int readNy()
      {
         return readIntValue(_textBoxNy.Text, "Ny");
      }

      private int readNz()
      {
         return readIntValue(_textBoxNz.Text, "Nz");
      }

      private int readXCut()
      {
         return readIntValue(_textBoxX.Text, "X");
      }

      private int readYCut()
      {
         return readIntValue(_textBoxY.Text, "Y");
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

      private void setButtonsAccessibility(bool enabled)
      {
         _buttonReset.Enabled = enabled;
         _buttonStep.Enabled = enabled;
      }
   }
}
