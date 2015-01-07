namespace LiquidDynamics.Forms.Upwelling
{
   internal partial class UpwellingForm
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
         this._graphControl = new ControlLibrary.Controls.GraphControl();
         this._paletteControl = new ControlLibrary.Controls.PaletteControl();
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._buttonReset = new System.Windows.Forms.ToolStripButton();
         this._buttonStepBackward = new System.Windows.Forms.ToolStripButton();
         this._buttonStartPause = new System.Windows.Forms.ToolStripButton();
         this._buttonStepForward = new System.Windows.Forms.ToolStripButton();
         this._labelZ = new System.Windows.Forms.ToolStripLabel();
         this._textBoxZ = new System.Windows.Forms.ToolStripTextBox();
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._toolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _graphControl
         // 
         this._graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._graphControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this._graphControl.Caption = null;
         this._graphControl.Location = new System.Drawing.Point(12, 28);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(644, 442);
         this._graphControl.TabIndex = 0;
         // 
         // _paletteControl
         // 
         this._paletteControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._paletteControl.Location = new System.Drawing.Point(662, 28);
         this._paletteControl.MaxValue = 1F;
         this._paletteControl.MinValue = 0F;
         this._paletteControl.Name = "_paletteControl";
         this._paletteControl.Size = new System.Drawing.Size(115, 442);
         this._paletteControl.TabIndex = 1;
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonStepBackward,
            this._buttonStartPause,
            this._buttonStepForward,
            this._labelZ,
            this._textBoxZ});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(785, 25);
         this._toolStrip.TabIndex = 2;
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
         // _labelZ
         // 
         this._labelZ.Name = "_labelZ";
         this._labelZ.Size = new System.Drawing.Size(17, 22);
         this._labelZ.Text = "Z:";
         // 
         // _textBoxZ
         // 
         this._textBoxZ.Name = "_textBoxZ";
         this._textBoxZ.Size = new System.Drawing.Size(100, 25);
         this._textBoxZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxZKeyPress);
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this.timerTick);
         // 
         // UpwellingForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.White;
         this.ClientSize = new System.Drawing.Size(785, 482);
         this.Controls.Add(this._toolStrip);
         this.Controls.Add(this._paletteControl);
         this.Controls.Add(this._graphControl);
         this.Name = "UpwellingForm";
         this.Text = "Апвеллинг";
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private ControlLibrary.Controls.GraphControl _graphControl;
      private ControlLibrary.Controls.PaletteControl _paletteControl;
      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripButton _buttonReset;
      private System.Windows.Forms.ToolStripButton _buttonStepBackward;
      private System.Windows.Forms.ToolStripButton _buttonStartPause;
      private System.Windows.Forms.ToolStripButton _buttonStepForward;
      private System.Windows.Forms.ToolStripLabel _labelZ;
      private System.Windows.Forms.ToolStripTextBox _textBoxZ;
      private System.Windows.Forms.Timer _timer;
   }
}