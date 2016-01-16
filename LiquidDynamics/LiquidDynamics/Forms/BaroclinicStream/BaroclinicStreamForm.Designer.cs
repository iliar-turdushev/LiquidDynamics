namespace LiquidDynamics.Forms.BaroclinicStream
{
   partial class BaroclinicStreamForm
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
         this._labelNx = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNx = new System.Windows.Forms.ToolStripTextBox();
         this._labelNy = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNy = new System.Windows.Forms.ToolStripTextBox();
         this._labelNz = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNz = new System.Windows.Forms.ToolStripTextBox();
         this._labelTau = new System.Windows.Forms.ToolStripLabel();
         this._textBoxTau = new System.Windows.Forms.ToolStripTextBox();
         this._labelSigma = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxSigma = new System.Windows.Forms.ToolStripComboBox();
         this._labelGraphType = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxGraphType = new System.Windows.Forms.ToolStripComboBox();
         this._labelX = new System.Windows.Forms.ToolStripLabel();
         this._textBoxX = new System.Windows.Forms.ToolStripTextBox();
         this._labelY = new System.Windows.Forms.ToolStripLabel();
         this._textBoxY = new System.Windows.Forms.ToolStripTextBox();
         this._splitContainer = new System.Windows.Forms.SplitContainer();
         this._uGraphControl = new ControlLibrary.Controls.GraphControl();
         this._vGraphControl = new ControlLibrary.Controls.GraphControl();
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._toolStrip.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
         this._splitContainer.Panel1.SuspendLayout();
         this._splitContainer.Panel2.SuspendLayout();
         this._splitContainer.SuspendLayout();
         this.SuspendLayout();
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonStartStop,
            this._buttonStep,
            this._labelNx,
            this._textBoxNx,
            this._labelNy,
            this._textBoxNy,
            this._labelNz,
            this._textBoxNz,
            this._labelTau,
            this._textBoxTau,
            this._labelSigma,
            this._comboBoxSigma,
            this._labelGraphType,
            this._comboBoxGraphType,
            this._labelX,
            this._textBoxX,
            this._labelY,
            this._textBoxY});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(890, 25);
         this._toolStrip.TabIndex = 1;
         this._toolStrip.Text = "toolStrip1";
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
         // _labelNx
         // 
         this._labelNx.Name = "_labelNx";
         this._labelNx.Size = new System.Drawing.Size(24, 22);
         this._labelNx.Text = "Nx:";
         // 
         // _textBoxNx
         // 
         this._textBoxNx.Name = "_textBoxNx";
         this._textBoxNx.Size = new System.Drawing.Size(50, 25);
         this._textBoxNx.Text = "20";
         // 
         // _labelNy
         // 
         this._labelNy.Name = "_labelNy";
         this._labelNy.Size = new System.Drawing.Size(25, 22);
         this._labelNy.Text = "Ny:";
         // 
         // _textBoxNy
         // 
         this._textBoxNy.Name = "_textBoxNy";
         this._textBoxNy.Size = new System.Drawing.Size(50, 25);
         this._textBoxNy.Text = "20";
         // 
         // _labelNz
         // 
         this._labelNz.Name = "_labelNz";
         this._labelNz.Size = new System.Drawing.Size(24, 22);
         this._labelNz.Text = "Nz:";
         // 
         // _textBoxNz
         // 
         this._textBoxNz.Name = "_textBoxNz";
         this._textBoxNz.Size = new System.Drawing.Size(50, 25);
         this._textBoxNz.Text = "20";
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
         // _labelSigma
         // 
         this._labelSigma.Name = "_labelSigma";
         this._labelSigma.Size = new System.Drawing.Size(43, 22);
         this._labelSigma.Text = "Sigma:";
         // 
         // _comboBoxSigma
         // 
         this._comboBoxSigma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._comboBoxSigma.Items.AddRange(new object[] {
            "0.5",
            "1.0",
            "Специальный вид"});
         this._comboBoxSigma.Name = "_comboBoxSigma";
         this._comboBoxSigma.Size = new System.Drawing.Size(121, 25);
         // 
         // _labelGraphType
         // 
         this._labelGraphType.Name = "_labelGraphType";
         this._labelGraphType.Size = new System.Drawing.Size(58, 22);
         this._labelGraphType.Text = "Графики:";
         // 
         // _comboBoxGraphType
         // 
         this._comboBoxGraphType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._comboBoxGraphType.Items.AddRange(new object[] {
            "Потоков",
            "Погрешностей"});
         this._comboBoxGraphType.Name = "_comboBoxGraphType";
         this._comboBoxGraphType.Size = new System.Drawing.Size(121, 25);
         // 
         // _labelX
         // 
         this._labelX.Name = "_labelX";
         this._labelX.Size = new System.Drawing.Size(17, 22);
         this._labelX.Text = "X:";
         // 
         // _textBoxX
         // 
         this._textBoxX.Name = "_textBoxX";
         this._textBoxX.Size = new System.Drawing.Size(50, 25);
         this._textBoxX.Text = "10";
         // 
         // _labelY
         // 
         this._labelY.Name = "_labelY";
         this._labelY.Size = new System.Drawing.Size(17, 22);
         this._labelY.Text = "Y:";
         // 
         // _textBoxY
         // 
         this._textBoxY.Name = "_textBoxY";
         this._textBoxY.Size = new System.Drawing.Size(50, 23);
         this._textBoxY.Text = "10";
         // 
         // _splitContainer
         // 
         this._splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
         this._splitContainer.Location = new System.Drawing.Point(0, 25);
         this._splitContainer.Name = "_splitContainer";
         // 
         // _splitContainer.Panel1
         // 
         this._splitContainer.Panel1.Controls.Add(this._uGraphControl);
         // 
         // _splitContainer.Panel2
         // 
         this._splitContainer.Panel2.Controls.Add(this._vGraphControl);
         this._splitContainer.Size = new System.Drawing.Size(890, 432);
         this._splitContainer.SplitterDistance = 443;
         this._splitContainer.TabIndex = 2;
         // 
         // _uGraphControl
         // 
         this._uGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._uGraphControl.Caption = "U";
         this._uGraphControl.Location = new System.Drawing.Point(3, 3);
         this._uGraphControl.Name = "_uGraphControl";
         this._uGraphControl.Size = new System.Drawing.Size(435, 424);
         this._uGraphControl.TabIndex = 0;
         // 
         // _vGraphControl
         // 
         this._vGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._vGraphControl.Caption = "V";
         this._vGraphControl.Location = new System.Drawing.Point(3, 3);
         this._vGraphControl.Name = "_vGraphControl";
         this._vGraphControl.Size = new System.Drawing.Size(435, 424);
         this._vGraphControl.TabIndex = 0;
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this.timerTick);
         // 
         // BaroclinicStreamForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(890, 457);
         this.Controls.Add(this._splitContainer);
         this.Controls.Add(this._toolStrip);
         this.Name = "BaroclinicStreamForm";
         this.Text = "BaroclinicStreamForm";
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this._splitContainer.Panel1.ResumeLayout(false);
         this._splitContainer.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
         this._splitContainer.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripButton _buttonReset;
      private System.Windows.Forms.ToolStripButton _buttonStartStop;
      private System.Windows.Forms.ToolStripButton _buttonStep;
      private System.Windows.Forms.SplitContainer _splitContainer;
      private ControlLibrary.Controls.GraphControl _uGraphControl;
      private ControlLibrary.Controls.GraphControl _vGraphControl;
      private System.Windows.Forms.ToolStripLabel _labelNz;
      private System.Windows.Forms.ToolStripTextBox _textBoxNz;
      private System.Windows.Forms.ToolStripLabel _labelNx;
      private System.Windows.Forms.ToolStripTextBox _textBoxNx;
      private System.Windows.Forms.ToolStripLabel _labelNy;
      private System.Windows.Forms.ToolStripTextBox _textBoxNy;
      private System.Windows.Forms.ToolStripLabel _labelTau;
      private System.Windows.Forms.ToolStripTextBox _textBoxTau;
      private System.Windows.Forms.ToolStripLabel _labelX;
      private System.Windows.Forms.ToolStripTextBox _textBoxX;
      private System.Windows.Forms.ToolStripLabel _labelY;
      private System.Windows.Forms.ToolStripTextBox _textBoxY;
      private System.Windows.Forms.Timer _timer;
      private System.Windows.Forms.ToolStripLabel _labelGraphType;
      private System.Windows.Forms.ToolStripComboBox _comboBoxGraphType;
      private System.Windows.Forms.ToolStripLabel _labelSigma;
      private System.Windows.Forms.ToolStripComboBox _comboBoxSigma;
   }
}