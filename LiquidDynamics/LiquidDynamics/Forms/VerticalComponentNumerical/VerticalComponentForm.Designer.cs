namespace LiquidDynamics.Forms.VerticalComponentNumerical
{
   partial class VerticalComponentForm
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
         this._buttonStep = new System.Windows.Forms.ToolStripButton();
         this._labelNx = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNx = new System.Windows.Forms.ToolStripTextBox();
         this._labelNy = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNy = new System.Windows.Forms.ToolStripTextBox();
         this._labelNz = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNz = new System.Windows.Forms.ToolStripTextBox();
         this._labelTau = new System.Windows.Forms.ToolStripLabel();
         this._textBoxTau = new System.Windows.Forms.ToolStripTextBox();
         this._labelSlice = new System.Windows.Forms.ToolStripLabel();
         this._textBoxSlice = new System.Windows.Forms.ToolStripTextBox();
         this._graphControl = new ControlLibrary.Controls.GraphControl();
         this._paletteControl = new ControlLibrary.Controls.PaletteControl();
         this._buttonStartPause = new System.Windows.Forms.ToolStripButton();
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._toolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonStartPause,
            this._buttonStep,
            this._labelNx,
            this._textBoxNx,
            this._labelNy,
            this._textBoxNy,
            this._labelNz,
            this._textBoxNz,
            this._labelTau,
            this._textBoxTau,
            this._labelSlice,
            this._textBoxSlice});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(780, 25);
         this._toolStrip.TabIndex = 0;
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
         this._textBoxNx.Text = "50";
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
         this._textBoxNy.Text = "50";
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
         this._textBoxNz.Text = "50";
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
         // _labelSlice
         // 
         this._labelSlice.Name = "_labelSlice";
         this._labelSlice.Size = new System.Drawing.Size(36, 22);
         this._labelSlice.Text = "Срез:";
         // 
         // _textBoxSlice
         // 
         this._textBoxSlice.Name = "_textBoxSlice";
         this._textBoxSlice.Size = new System.Drawing.Size(50, 25);
         this._textBoxSlice.Text = "25";
         // 
         // _graphControl
         // 
         this._graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._graphControl.Caption = "Вертикальная компонента";
         this._graphControl.Location = new System.Drawing.Point(12, 28);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(639, 423);
         this._graphControl.TabIndex = 1;
         // 
         // _paletteControl
         // 
         this._paletteControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._paletteControl.Location = new System.Drawing.Point(657, 28);
         this._paletteControl.MaxValue = 1F;
         this._paletteControl.MinValue = 0F;
         this._paletteControl.Name = "_paletteControl";
         this._paletteControl.Size = new System.Drawing.Size(111, 423);
         this._paletteControl.TabIndex = 2;
         // 
         // _buttonStartPause
         // 
         this._buttonStartPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonStartPause.Enabled = false;
         this._buttonStartPause.Image = global::LiquidDynamics.Properties.Resources.Start;
         this._buttonStartPause.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStartPause.Name = "_buttonStartPause";
         this._buttonStartPause.Size = new System.Drawing.Size(23, 22);
         this._buttonStartPause.Click += new System.EventHandler(this.buttonStartPauseClick);
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this.timerTick);
         // 
         // VerticalComponentForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(780, 463);
         this.Controls.Add(this._paletteControl);
         this.Controls.Add(this._graphControl);
         this.Controls.Add(this._toolStrip);
         this.Name = "VerticalComponentForm";
         this.Text = "VerticalComponentForm";
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripButton _buttonReset;
      private ControlLibrary.Controls.GraphControl _graphControl;
      private System.Windows.Forms.ToolStripButton _buttonStep;
      private System.Windows.Forms.ToolStripLabel _labelNx;
      private System.Windows.Forms.ToolStripTextBox _textBoxNx;
      private System.Windows.Forms.ToolStripLabel _labelNy;
      private System.Windows.Forms.ToolStripTextBox _textBoxNy;
      private System.Windows.Forms.ToolStripLabel _labelNz;
      private System.Windows.Forms.ToolStripTextBox _textBoxNz;
      private System.Windows.Forms.ToolStripLabel _labelTau;
      private System.Windows.Forms.ToolStripTextBox _textBoxTau;
      private System.Windows.Forms.ToolStripLabel _labelSlice;
      private System.Windows.Forms.ToolStripTextBox _textBoxSlice;
      private ControlLibrary.Controls.PaletteControl _paletteControl;
      private System.Windows.Forms.ToolStripButton _buttonStartPause;
      private System.Windows.Forms.Timer _timer;

   }
}