namespace LiquidDynamics.Forms.BarotropicComponentNumerical
{
   partial class BarotropicComponentNumericalForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._buttonReset = new System.Windows.Forms.ToolStripButton();
         this._buttonStartStop = new System.Windows.Forms.ToolStripButton();
         this._buttonStep = new System.Windows.Forms.ToolStripButton();
         this._labelScheme = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxScheme = new System.Windows.Forms.ToolStripComboBox();
         this._labelTheta = new System.Windows.Forms.ToolStripLabel();
         this._textBoxTheta = new System.Windows.Forms.ToolStripTextBox();
         this._labelChi = new System.Windows.Forms.ToolStripLabel();
         this._textBoxChi = new System.Windows.Forms.ToolStripTextBox();
         this._labelTau = new System.Windows.Forms.ToolStripLabel();
         this._textBoxTau = new System.Windows.Forms.ToolStripTextBox();
         this._labelN = new System.Windows.Forms.ToolStripLabel();
         this._textBoxN = new System.Windows.Forms.ToolStripTextBox();
         this._labelM = new System.Windows.Forms.ToolStripLabel();
         this._textBoxM = new System.Windows.Forms.ToolStripTextBox();
         this._labelSigma = new System.Windows.Forms.ToolStripLabel();
         this._textBoxSigma = new System.Windows.Forms.ToolStripTextBox();
         this._labelDelta = new System.Windows.Forms.ToolStripLabel();
         this._textBoxDelta = new System.Windows.Forms.ToolStripTextBox();
         this._labelK = new System.Windows.Forms.ToolStripLabel();
         this._textBoxK = new System.Windows.Forms.ToolStripTextBox();
         this._labelGraphs = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxGraphs = new System.Windows.Forms.ToolStripComboBox();
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._splitContainer = new System.Windows.Forms.SplitContainer();
         this._exactSolutionPaletteControl = new ControlLibrary.Controls.PaletteControl();
         this._exactSolutionGraphControl = new ControlLibrary.Controls.GraphControl();
         this._calculatedSolutionPaletteControl = new ControlLibrary.Controls.PaletteControl();
         this._calculatedSolutionGraphControl = new ControlLibrary.Controls.GraphControl();
         this._statusStrip = new System.Windows.Forms.StatusStrip();
         this._labelResult = new System.Windows.Forms.ToolStripStatusLabel();
         this._panel = new System.Windows.Forms.Panel();
         this._graphControl = new ControlLibrary.Controls.GraphControl();
         this._toolStrip.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
         this._splitContainer.Panel1.SuspendLayout();
         this._splitContainer.Panel2.SuspendLayout();
         this._splitContainer.SuspendLayout();
         this._statusStrip.SuspendLayout();
         this._panel.SuspendLayout();
         this.SuspendLayout();
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonStartStop,
            this._buttonStep,
            this._labelScheme,
            this._comboBoxScheme,
            this._labelTheta,
            this._textBoxTheta,
            this._labelChi,
            this._textBoxChi,
            this._labelTau,
            this._textBoxTau,
            this._labelN,
            this._textBoxN,
            this._labelM,
            this._textBoxM,
            this._labelSigma,
            this._textBoxSigma,
            this._labelDelta,
            this._textBoxDelta,
            this._labelK,
            this._textBoxK,
            this._labelGraphs,
            this._comboBoxGraphs});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(1211, 25);
         this._toolStrip.TabIndex = 3;
         // 
         // _buttonReset
         // 
         this._buttonReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonReset.Image = global::LiquidDynamics.Properties.Resources.Stop;
         this._buttonReset.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonReset.Name = "_buttonReset";
         this._buttonReset.Size = new System.Drawing.Size(23, 22);
         this._buttonReset.Click += new System.EventHandler(this.buttonResetClick);
         // 
         // _buttonStartStop
         // 
         this._buttonStartStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonStartStop.Enabled = false;
         this._buttonStartStop.Image = global::LiquidDynamics.Properties.Resources.Start;
         this._buttonStartStop.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStartStop.Name = "_buttonStartStop";
         this._buttonStartStop.Size = new System.Drawing.Size(23, 22);
         this._buttonStartStop.Click += new System.EventHandler(this.buttonStartStopClick);
         // 
         // _buttonStep
         // 
         this._buttonStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonStep.Enabled = false;
         this._buttonStep.Image = global::LiquidDynamics.Properties.Resources.StepForward;
         this._buttonStep.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStep.Name = "_buttonStep";
         this._buttonStep.Size = new System.Drawing.Size(23, 22);
         this._buttonStep.Click += new System.EventHandler(this.buttonStepClick);
         // 
         // _labelScheme
         // 
         this._labelScheme.Name = "_labelScheme";
         this._labelScheme.Size = new System.Drawing.Size(44, 22);
         this._labelScheme.Text = "Схема:";
         // 
         // _comboBoxScheme
         // 
         this._comboBoxScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._comboBoxScheme.Name = "_comboBoxScheme";
         this._comboBoxScheme.Size = new System.Drawing.Size(121, 25);
         // 
         // _labelTheta
         // 
         this._labelTheta.Name = "_labelTheta";
         this._labelTheta.Size = new System.Drawing.Size(40, 22);
         this._labelTheta.Text = "Theta:";
         // 
         // _textBoxTheta
         // 
         this._textBoxTheta.Name = "_textBoxTheta";
         this._textBoxTheta.Size = new System.Drawing.Size(50, 25);
         this._textBoxTheta.Text = "0.5";
         // 
         // _labelChi
         // 
         this._labelChi.Name = "_labelChi";
         this._labelChi.Size = new System.Drawing.Size(28, 22);
         this._labelChi.Text = "Chi:";
         // 
         // _textBoxChi
         // 
         this._textBoxChi.Name = "_textBoxChi";
         this._textBoxChi.Size = new System.Drawing.Size(50, 25);
         this._textBoxChi.Text = "0.0";
         // 
         // _labelTau
         // 
         this._labelTau.Name = "_labelTau";
         this._labelTau.Size = new System.Drawing.Size(30, 22);
         this._labelTau.Text = "Tau:";
         // 
         // _textBoxTau
         // 
         this._textBoxTau.Name = "_textBoxTau";
         this._textBoxTau.Size = new System.Drawing.Size(50, 25);
         this._textBoxTau.Text = "0.1";
         // 
         // _labelN
         // 
         this._labelN.Name = "_labelN";
         this._labelN.Size = new System.Drawing.Size(19, 22);
         this._labelN.Text = "N:";
         // 
         // _textBoxN
         // 
         this._textBoxN.Name = "_textBoxN";
         this._textBoxN.Size = new System.Drawing.Size(50, 25);
         this._textBoxN.Text = "20";
         // 
         // _labelM
         // 
         this._labelM.Name = "_labelM";
         this._labelM.Size = new System.Drawing.Size(21, 22);
         this._labelM.Text = "M:";
         // 
         // _textBoxM
         // 
         this._textBoxM.Name = "_textBoxM";
         this._textBoxM.Size = new System.Drawing.Size(50, 25);
         this._textBoxM.Text = "20";
         // 
         // _labelSigma
         // 
         this._labelSigma.Name = "_labelSigma";
         this._labelSigma.Size = new System.Drawing.Size(43, 22);
         this._labelSigma.Text = "Sigma:";
         // 
         // _textBoxSigma
         // 
         this._textBoxSigma.Name = "_textBoxSigma";
         this._textBoxSigma.Size = new System.Drawing.Size(50, 25);
         this._textBoxSigma.Text = "0.0001";
         // 
         // _labelDelta
         // 
         this._labelDelta.Name = "_labelDelta";
         this._labelDelta.Size = new System.Drawing.Size(37, 22);
         this._labelDelta.Text = "Delta:";
         // 
         // _textBoxDelta
         // 
         this._textBoxDelta.Name = "_textBoxDelta";
         this._textBoxDelta.Size = new System.Drawing.Size(50, 25);
         this._textBoxDelta.Text = "100000";
         // 
         // _labelK
         // 
         this._labelK.Name = "_labelK";
         this._labelK.Size = new System.Drawing.Size(17, 22);
         this._labelK.Text = "K:";
         // 
         // _textBoxK
         // 
         this._textBoxK.Name = "_textBoxK";
         this._textBoxK.Size = new System.Drawing.Size(50, 25);
         this._textBoxK.Text = "100000";
         // 
         // _labelGraphs
         // 
         this._labelGraphs.Name = "_labelGraphs";
         this._labelGraphs.Size = new System.Drawing.Size(58, 22);
         this._labelGraphs.Text = "Графики:";
         // 
         // _comboBoxGraphs
         // 
         this._comboBoxGraphs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._comboBoxGraphs.Name = "_comboBoxGraphs";
         this._comboBoxGraphs.Size = new System.Drawing.Size(121, 25);
         this._comboBoxGraphs.SelectedIndexChanged += new System.EventHandler(this.comboBoxGraphsSelectedIndexChanged);
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this.timerTick);
         // 
         // _splitContainer
         // 
         this._splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
         this._splitContainer.Location = new System.Drawing.Point(0, 0);
         this._splitContainer.Name = "_splitContainer";
         // 
         // _splitContainer.Panel1
         // 
         this._splitContainer.Panel1.Controls.Add(this._exactSolutionPaletteControl);
         this._splitContainer.Panel1.Controls.Add(this._exactSolutionGraphControl);
         // 
         // _splitContainer.Panel2
         // 
         this._splitContainer.Panel2.Controls.Add(this._calculatedSolutionPaletteControl);
         this._splitContainer.Panel2.Controls.Add(this._calculatedSolutionGraphControl);
         this._splitContainer.Size = new System.Drawing.Size(1187, 472);
         this._splitContainer.SplitterDistance = 594;
         this._splitContainer.TabIndex = 4;
         // 
         // _exactSolutionPaletteControl
         // 
         this._exactSolutionPaletteControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._exactSolutionPaletteControl.Location = new System.Drawing.Point(480, 3);
         this._exactSolutionPaletteControl.MaxValue = 1F;
         this._exactSolutionPaletteControl.MinValue = 0F;
         this._exactSolutionPaletteControl.Name = "_exactSolutionPaletteControl";
         this._exactSolutionPaletteControl.Size = new System.Drawing.Size(109, 464);
         this._exactSolutionPaletteControl.TabIndex = 2;
         // 
         // _exactSolutionGraphControl
         // 
         this._exactSolutionGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._exactSolutionGraphControl.Caption = "Точное решение";
         this._exactSolutionGraphControl.Location = new System.Drawing.Point(3, 2);
         this._exactSolutionGraphControl.Name = "_exactSolutionGraphControl";
         this._exactSolutionGraphControl.Size = new System.Drawing.Size(471, 465);
         this._exactSolutionGraphControl.TabIndex = 1;
         // 
         // _calculatedSolutionPaletteControl
         // 
         this._calculatedSolutionPaletteControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._calculatedSolutionPaletteControl.Location = new System.Drawing.Point(475, 3);
         this._calculatedSolutionPaletteControl.MaxValue = 1F;
         this._calculatedSolutionPaletteControl.MinValue = 0F;
         this._calculatedSolutionPaletteControl.Name = "_calculatedSolutionPaletteControl";
         this._calculatedSolutionPaletteControl.Size = new System.Drawing.Size(109, 464);
         this._calculatedSolutionPaletteControl.TabIndex = 1;
         // 
         // _calculatedSolutionGraphControl
         // 
         this._calculatedSolutionGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._calculatedSolutionGraphControl.Caption = "Приближенное решение";
         this._calculatedSolutionGraphControl.Location = new System.Drawing.Point(3, 3);
         this._calculatedSolutionGraphControl.Name = "_calculatedSolutionGraphControl";
         this._calculatedSolutionGraphControl.Size = new System.Drawing.Size(466, 464);
         this._calculatedSolutionGraphControl.TabIndex = 0;
         // 
         // _statusStrip
         // 
         this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._labelResult});
         this._statusStrip.Location = new System.Drawing.Point(0, 503);
         this._statusStrip.Name = "_statusStrip";
         this._statusStrip.Size = new System.Drawing.Size(1211, 22);
         this._statusStrip.TabIndex = 5;
         // 
         // _labelResult
         // 
         this._labelResult.Name = "_labelResult";
         this._labelResult.Size = new System.Drawing.Size(0, 17);
         this._labelResult.Click += new System.EventHandler(this.labelResultClick);
         // 
         // _panel
         // 
         this._panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._panel.Controls.Add(this._graphControl);
         this._panel.Controls.Add(this._splitContainer);
         this._panel.Location = new System.Drawing.Point(12, 28);
         this._panel.Name = "_panel";
         this._panel.Size = new System.Drawing.Size(1187, 472);
         this._panel.TabIndex = 6;
         // 
         // _graphControl
         // 
         this._graphControl.Caption = "";
         this._graphControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this._graphControl.Location = new System.Drawing.Point(0, 0);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(1187, 472);
         this._graphControl.TabIndex = 7;
         // 
         // BarotropicComponentNumericalForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.White;
         this.ClientSize = new System.Drawing.Size(1211, 525);
         this.Controls.Add(this._panel);
         this.Controls.Add(this._statusStrip);
         this.Controls.Add(this._toolStrip);
         this.Name = "BarotropicComponentNumericalForm";
         this.Text = "Баротропная компонента";
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this._splitContainer.Panel1.ResumeLayout(false);
         this._splitContainer.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
         this._splitContainer.ResumeLayout(false);
         this._statusStrip.ResumeLayout(false);
         this._statusStrip.PerformLayout();
         this._panel.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private ControlLibrary.Controls.GraphControl _exactSolutionGraphControl;
      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripButton _buttonReset;
      private System.Windows.Forms.ToolStripButton _buttonStep;
      private System.Windows.Forms.ToolStripLabel _labelTau;
      private System.Windows.Forms.ToolStripTextBox _textBoxTau;
      private System.Windows.Forms.ToolStripLabel _labelN;
      private System.Windows.Forms.ToolStripTextBox _textBoxN;
      private System.Windows.Forms.ToolStripLabel _labelM;
      private System.Windows.Forms.ToolStripTextBox _textBoxM;
      private System.Windows.Forms.ToolStripLabel _labelSigma;
      private System.Windows.Forms.ToolStripTextBox _textBoxSigma;
      private System.Windows.Forms.ToolStripLabel _labelDelta;
      private System.Windows.Forms.ToolStripTextBox _textBoxDelta;
      private System.Windows.Forms.ToolStripLabel _labelK;
      private System.Windows.Forms.ToolStripTextBox _textBoxK;
      private System.Windows.Forms.ToolStripButton _buttonStartStop;
      private System.Windows.Forms.Timer _timer;
      private System.Windows.Forms.SplitContainer _splitContainer;
      private ControlLibrary.Controls.PaletteControl _exactSolutionPaletteControl;
      private System.Windows.Forms.StatusStrip _statusStrip;
      private System.Windows.Forms.ToolStripStatusLabel _labelResult;
      private ControlLibrary.Controls.PaletteControl _calculatedSolutionPaletteControl;
      private ControlLibrary.Controls.GraphControl _calculatedSolutionGraphControl;
      private System.Windows.Forms.ToolStripLabel _labelGraphs;
      private System.Windows.Forms.ToolStripComboBox _comboBoxGraphs;
      private System.Windows.Forms.Panel _panel;
      private ControlLibrary.Controls.GraphControl _graphControl;
      private System.Windows.Forms.ToolStripLabel _labelScheme;
      private System.Windows.Forms.ToolStripComboBox _comboBoxScheme;
      private System.Windows.Forms.ToolStripLabel _labelTheta;
      private System.Windows.Forms.ToolStripTextBox _textBoxTheta;
      private System.Windows.Forms.ToolStripLabel _labelChi;
      private System.Windows.Forms.ToolStripTextBox _textBoxChi;
   }
}