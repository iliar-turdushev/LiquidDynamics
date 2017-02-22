﻿using ControlLibrary.Controls;

namespace LiquidDynamics.Forms.Analytical.Barotropic
{
   internal partial class BarotropicComponentForm
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
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._paletteControl = new ControlLibrary.Controls.PaletteControl();
         this._graphControl = new ControlLibrary.Controls.GraphControl();
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._buttonReset = new System.Windows.Forms.ToolStripButton();
         this._buttonStartStop = new System.Windows.Forms.ToolStripButton();
         this._buttonStepForward = new System.Windows.Forms.ToolStripButton();
         this._labelNx = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNx = new System.Windows.Forms.ToolStripTextBox();
         this._labelNy = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNy = new System.Windows.Forms.ToolStripTextBox();
         this._labelDt = new System.Windows.Forms.ToolStripLabel();
         this._textBoxDt = new System.Windows.Forms.ToolStripTextBox();
         this._toolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this.timerTick);
         // 
         // _paletteControl
         // 
         this._paletteControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._paletteControl.Location = new System.Drawing.Point(664, 28);
         this._paletteControl.MaxValue = 1F;
         this._paletteControl.MinValue = 0F;
         this._paletteControl.Name = "_paletteControl";
         this._paletteControl.Size = new System.Drawing.Size(115, 433);
         this._paletteControl.TabIndex = 2;
         // 
         // _graphControl
         // 
         this._graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._graphControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this._graphControl.Caption = "Баротропная компонента";
         this._graphControl.Location = new System.Drawing.Point(12, 28);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(646, 433);
         this._graphControl.TabIndex = 0;
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonStartStop,
            this._buttonStepForward,
            this._labelNx,
            this._textBoxNx,
            this._labelNy,
            this._textBoxNy,
            this._labelDt,
            this._textBoxDt});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(791, 25);
         this._toolStrip.TabIndex = 3;
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
         this._buttonStartStop.Image = global::LiquidDynamics.Properties.Resources.Start;
         this._buttonStartStop.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonStartStop.Name = "_buttonStartStop";
         this._buttonStartStop.Size = new System.Drawing.Size(23, 22);
         this._buttonStartStop.Click += new System.EventHandler(this.buttonStartStopClick);
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
         // _labelDt
         // 
         this._labelDt.Name = "_labelDt";
         this._labelDt.Size = new System.Drawing.Size(22, 22);
         this._labelDt.Text = "Dt:";
         // 
         // _textBoxDt
         // 
         this._textBoxDt.Name = "_textBoxDt";
         this._textBoxDt.Size = new System.Drawing.Size(50, 25);
         this._textBoxDt.Text = "0.25";
         // 
         // BarotropicComponentForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(791, 473);
         this.Controls.Add(this._toolStrip);
         this.Controls.Add(this._paletteControl);
         this.Controls.Add(this._graphControl);
         this.Name = "BarotropicComponentForm";
         this.Text = "Аналитическое вычисление баротропной компоненты";
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private GraphControl _graphControl;
      private System.Windows.Forms.Timer _timer;
      private PaletteControl _paletteControl;
      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripButton _buttonReset;
      private System.Windows.Forms.ToolStripButton _buttonStartStop;
      private System.Windows.Forms.ToolStripButton _buttonStepForward;
      private System.Windows.Forms.ToolStripLabel _labelNx;
      private System.Windows.Forms.ToolStripTextBox _textBoxNx;
      private System.Windows.Forms.ToolStripLabel _labelNy;
      private System.Windows.Forms.ToolStripTextBox _textBoxNy;
      private System.Windows.Forms.ToolStripLabel _labelDt;
      private System.Windows.Forms.ToolStripTextBox _textBoxDt;
   }
}