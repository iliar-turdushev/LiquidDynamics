namespace LiquidDynamics.Forms.BaroclinicComponent
{
   partial class BaroclinicComponentForm
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
         this._buttonBegin = new System.Windows.Forms.ToolStripButton();
         this._buttonStartStop = new System.Windows.Forms.ToolStripButton();
         this._buttonStep = new System.Windows.Forms.ToolStripButton();
         this._labelX = new System.Windows.Forms.ToolStripLabel();
         this._textBoxX = new System.Windows.Forms.ToolStripTextBox();
         this._labelY = new System.Windows.Forms.ToolStripLabel();
         this._textBoxY = new System.Windows.Forms.ToolStripTextBox();
         this._labelN = new System.Windows.Forms.ToolStripLabel();
         this._textBoxN = new System.Windows.Forms.ToolStripTextBox();
         this._labelTau = new System.Windows.Forms.ToolStripLabel();
         this._textBoxTau = new System.Windows.Forms.ToolStripTextBox();
         this._splitContainer = new System.Windows.Forms.SplitContainer();
         this._graphControlU = new ControlLibrary.Controls.GraphControl();
         this._graphControlV = new ControlLibrary.Controls.GraphControl();
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._buttonPlus = new System.Windows.Forms.ToolStripButton();
         this._buttonMinus = new System.Windows.Forms.ToolStripButton();
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
            this._buttonBegin,
            this._buttonStartStop,
            this._buttonStep,
            this._labelX,
            this._textBoxX,
            this._labelY,
            this._textBoxY,
            this._labelN,
            this._textBoxN,
            this._labelTau,
            this._textBoxTau,
            this._buttonPlus,
            this._buttonMinus});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(959, 25);
         this._toolStrip.TabIndex = 0;
         // 
         // _buttonBegin
         // 
         this._buttonBegin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonBegin.Image = global::LiquidDynamics.Properties.Resources.Stop;
         this._buttonBegin.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonBegin.Name = "_buttonBegin";
         this._buttonBegin.Size = new System.Drawing.Size(23, 22);
         this._buttonBegin.Click += new System.EventHandler(this.buttonBeginClick);
         // 
         // _buttonStartStop
         // 
         this._buttonStartStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonStartStop.Image = global::LiquidDynamics.Properties.Resources.Start;
         this._buttonStartStop.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStartStop.Name = "_buttonStartStop";
         this._buttonStartStop.Size = new System.Drawing.Size(23, 22);
         this._buttonStartStop.Click += new System.EventHandler(this.buttonStartStopClick);
         // 
         // _buttonStep
         // 
         this._buttonStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonStep.Image = global::LiquidDynamics.Properties.Resources.StepForward;
         this._buttonStep.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStep.Name = "_buttonStep";
         this._buttonStep.Size = new System.Drawing.Size(23, 22);
         this._buttonStep.Click += new System.EventHandler(this.buttonStepClick);
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
         this._textBoxY.Size = new System.Drawing.Size(50, 25);
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
         this._textBoxN.Text = "100";
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
         this._textBoxTau.Text = "0.01";
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
         this._splitContainer.Panel1.Controls.Add(this._graphControlU);
         // 
         // _splitContainer.Panel2
         // 
         this._splitContainer.Panel2.Controls.Add(this._graphControlV);
         this._splitContainer.Size = new System.Drawing.Size(959, 454);
         this._splitContainer.SplitterDistance = 485;
         this._splitContainer.TabIndex = 1;
         // 
         // _graphControlU
         // 
         this._graphControlU.Caption = "U";
         this._graphControlU.Dock = System.Windows.Forms.DockStyle.Fill;
         this._graphControlU.Location = new System.Drawing.Point(0, 0);
         this._graphControlU.Name = "_graphControlU";
         this._graphControlU.Size = new System.Drawing.Size(483, 452);
         this._graphControlU.TabIndex = 0;
         // 
         // _graphControlV
         // 
         this._graphControlV.Caption = "V";
         this._graphControlV.Dock = System.Windows.Forms.DockStyle.Fill;
         this._graphControlV.Location = new System.Drawing.Point(0, 0);
         this._graphControlV.Name = "_graphControlV";
         this._graphControlV.Size = new System.Drawing.Size(468, 452);
         this._graphControlV.TabIndex = 0;
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this.timerTick);
         // 
         // _buttonPlus
         // 
         this._buttonPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonPlus.Image = global::LiquidDynamics.Properties.Resources.Plus;
         this._buttonPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonPlus.Name = "_buttonPlus";
         this._buttonPlus.Size = new System.Drawing.Size(23, 22);
         this._buttonPlus.Click += new System.EventHandler(this.buttonPlusClick);
         // 
         // _buttonMinus
         // 
         this._buttonMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonMinus.Image = global::LiquidDynamics.Properties.Resources.Minus;
         this._buttonMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonMinus.Name = "_buttonMinus";
         this._buttonMinus.Size = new System.Drawing.Size(23, 22);
         this._buttonMinus.Click += new System.EventHandler(this.buttonMinusClick);
         // 
         // BaroclinicComponentForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(959, 479);
         this.Controls.Add(this._splitContainer);
         this.Controls.Add(this._toolStrip);
         this.Name = "BaroclinicComponentForm";
         this.Text = "BaroclinicComponentForm";
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
      private System.Windows.Forms.SplitContainer _splitContainer;
      private ControlLibrary.Controls.GraphControl _graphControlU;
      private ControlLibrary.Controls.GraphControl _graphControlV;
      private System.Windows.Forms.ToolStripButton _buttonBegin;
      private System.Windows.Forms.ToolStripButton _buttonStartStop;
      private System.Windows.Forms.ToolStripButton _buttonStep;
      private System.Windows.Forms.ToolStripLabel _labelX;
      private System.Windows.Forms.ToolStripTextBox _textBoxX;
      private System.Windows.Forms.ToolStripLabel _labelY;
      private System.Windows.Forms.ToolStripTextBox _textBoxY;
      private System.Windows.Forms.ToolStripLabel _labelN;
      private System.Windows.Forms.ToolStripTextBox _textBoxN;
      private System.Windows.Forms.ToolStripLabel _labelTau;
      private System.Windows.Forms.ToolStripTextBox _textBoxTau;
      private System.Windows.Forms.Timer _timer;
      private System.Windows.Forms.ToolStripButton _buttonPlus;
      private System.Windows.Forms.ToolStripButton _buttonMinus;

   }
}