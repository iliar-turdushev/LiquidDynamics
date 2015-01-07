namespace LiquidDynamics.Forms.EkmanSpiral
{
   internal partial class EkmanSpiralForm
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
         this._statusStrip = new System.Windows.Forms.StatusStrip();
         this._labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._buttonReset = new System.Windows.Forms.ToolStripButton();
         this._buttonStepBackward = new System.Windows.Forms.ToolStripButton();
         this._buttonStartPause = new System.Windows.Forms.ToolStripButton();
         this._buttonStepForward = new System.Windows.Forms.ToolStripButton();
         this._labelX = new System.Windows.Forms.ToolStripLabel();
         this._textBoxX = new System.Windows.Forms.ToolStripTextBox();
         this._labelY = new System.Windows.Forms.ToolStripLabel();
         this._textBoxY = new System.Windows.Forms.ToolStripTextBox();
         this._buttonMinus = new System.Windows.Forms.ToolStripButton();
         this._buttonPlus = new System.Windows.Forms.ToolStripButton();
         this._splitContainer = new System.Windows.Forms.SplitContainer();
         this._graphControl2D = new ControlLibrary.Controls.GraphControl();
         this._statusStrip.SuspendLayout();
         this._toolStrip.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
         this._splitContainer.Panel2.SuspendLayout();
         this._splitContainer.SuspendLayout();
         this.SuspendLayout();
         // 
         // _statusStrip
         // 
         this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._labelStatus});
         this._statusStrip.Location = new System.Drawing.Point(0, 438);
         this._statusStrip.Name = "_statusStrip";
         this._statusStrip.Size = new System.Drawing.Size(987, 22);
         this._statusStrip.TabIndex = 2;
         // 
         // _labelStatus
         // 
         this._labelStatus.Name = "_labelStatus";
         this._labelStatus.Size = new System.Drawing.Size(0, 17);
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this.timerTick);
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonStepBackward,
            this._buttonStartPause,
            this._buttonStepForward,
            this._labelX,
            this._textBoxX,
            this._labelY,
            this._textBoxY,
            this._buttonMinus,
            this._buttonPlus});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(987, 25);
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
         // _buttonStepBackward
         // 
         this._buttonStepBackward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonStepBackward.Image = global::LiquidDynamics.Properties.Resources.StepBackward;
         this._buttonStepBackward.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStepBackward.Name = "_buttonStepBackward";
         this._buttonStepBackward.Size = new System.Drawing.Size(23, 22);
         this._buttonStepBackward.Click += new System.EventHandler(this.buttonStepBackwardClick);
         // 
         // _buttonStartPause
         // 
         this._buttonStartPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonStartPause.Image = global::LiquidDynamics.Properties.Resources.Start;
         this._buttonStartPause.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStartPause.Name = "_buttonStartPause";
         this._buttonStartPause.Size = new System.Drawing.Size(23, 22);
         this._buttonStartPause.Click += new System.EventHandler(this.buttonStartPauseClick);
         // 
         // _buttonStepForward
         // 
         this._buttonStepForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonStepForward.Image = global::LiquidDynamics.Properties.Resources.StepForward;
         this._buttonStepForward.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStepForward.Name = "_buttonStepForward";
         this._buttonStepForward.Size = new System.Drawing.Size(23, 22);
         this._buttonStepForward.Click += new System.EventHandler(this.buttonStepForwardClick);
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
         this._textBoxX.Size = new System.Drawing.Size(100, 25);
         this._textBoxX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxXKeyPress);
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
         this._textBoxY.Size = new System.Drawing.Size(100, 25);
         this._textBoxY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxYKeyPress);
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
         // _buttonPlus
         // 
         this._buttonPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonPlus.Image = global::LiquidDynamics.Properties.Resources.Plus;
         this._buttonPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonPlus.Name = "_buttonPlus";
         this._buttonPlus.Size = new System.Drawing.Size(23, 22);
         this._buttonPlus.Click += new System.EventHandler(this.buttonPlusClick);
         // 
         // _splitContainer
         // 
         this._splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this._splitContainer.Location = new System.Drawing.Point(12, 28);
         this._splitContainer.Name = "_splitContainer";
         // 
         // _splitContainer.Panel2
         // 
         this._splitContainer.Panel2.Controls.Add(this._graphControl2D);
         this._splitContainer.Size = new System.Drawing.Size(963, 407);
         this._splitContainer.SplitterDistance = 480;
         this._splitContainer.TabIndex = 4;
         // 
         // _graphControl2D
         // 
         this._graphControl2D.Caption = "Спираль Экмана";
         this._graphControl2D.Dock = System.Windows.Forms.DockStyle.Fill;
         this._graphControl2D.Location = new System.Drawing.Point(0, 0);
         this._graphControl2D.Name = "_graphControl2D";
         this._graphControl2D.Size = new System.Drawing.Size(477, 405);
         this._graphControl2D.TabIndex = 0;
         // 
         // EkmanSpiralForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.White;
         this.ClientSize = new System.Drawing.Size(987, 460);
         this.Controls.Add(this._splitContainer);
         this.Controls.Add(this._toolStrip);
         this.Controls.Add(this._statusStrip);
         this.Name = "EkmanSpiralForm";
         this.Text = "Спираль Экмана";
         this._statusStrip.ResumeLayout(false);
         this._statusStrip.PerformLayout();
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this._splitContainer.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
         this._splitContainer.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.StatusStrip _statusStrip;
      private System.Windows.Forms.Timer _timer;
      private System.Windows.Forms.ToolStripStatusLabel _labelStatus;
      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripButton _buttonReset;
      private System.Windows.Forms.ToolStripButton _buttonStepBackward;
      private System.Windows.Forms.ToolStripButton _buttonStartPause;
      private System.Windows.Forms.ToolStripButton _buttonStepForward;
      private System.Windows.Forms.ToolStripLabel _labelX;
      private System.Windows.Forms.ToolStripTextBox _textBoxX;
      private System.Windows.Forms.ToolStripLabel _labelY;
      private System.Windows.Forms.ToolStripTextBox _textBoxY;
      private System.Windows.Forms.ToolStripButton _buttonMinus;
      private System.Windows.Forms.ToolStripButton _buttonPlus;
      private System.Windows.Forms.SplitContainer _splitContainer;
      private ControlLibrary.Controls.GraphControl _graphControl2D;
   }
}