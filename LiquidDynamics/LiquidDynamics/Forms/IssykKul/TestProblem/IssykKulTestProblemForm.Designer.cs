﻿namespace LiquidDynamics.Forms.IssykKul.TestProblem
{
   partial class IssykKulTestProblemForm
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
         this._buttonStartStop = new System.Windows.Forms.ToolStripButton();
         this._buttonStep = new System.Windows.Forms.ToolStripButton();
         this._labelN = new System.Windows.Forms.ToolStripLabel();
         this._textBoxN = new System.Windows.Forms.ToolStripTextBox();
         this._labelM = new System.Windows.Forms.ToolStripLabel();
         this._textBoxM = new System.Windows.Forms.ToolStripTextBox();
         this.labelDz = new System.Windows.Forms.ToolStripLabel();
         this._textBoxDz = new System.Windows.Forms.ToolStripTextBox();
         this._labelTau = new System.Windows.Forms.ToolStripLabel();
         this._textBoxTau = new System.Windows.Forms.ToolStripTextBox();
         this._labelTheta = new System.Windows.Forms.ToolStripLabel();
         this._textBoxTheta = new System.Windows.Forms.ToolStripTextBox();
         this._labelSigma = new System.Windows.Forms.ToolStripLabel();
         this._textBoxSigma = new System.Windows.Forms.ToolStripTextBox();
         this._labelDelta = new System.Windows.Forms.ToolStripLabel();
         this._textBoxDelta = new System.Windows.Forms.ToolStripTextBox();
         this._labelK = new System.Windows.Forms.ToolStripLabel();
         this._textBoxK = new System.Windows.Forms.ToolStripTextBox();
         this._labelWindType = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxWindType = new System.Windows.Forms.ToolStripComboBox();
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._labelSlice = new System.Windows.Forms.ToolStripLabel();
         this._textBoxSlice = new System.Windows.Forms.ToolStripTextBox();
         this._toolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _graphControl
         // 
         this._graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._graphControl.Caption = "Иссык-Куль";
         this._graphControl.Location = new System.Drawing.Point(12, 28);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(890, 499);
         this._graphControl.TabIndex = 0;
         // 
         // _paletteControl
         // 
         this._paletteControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._paletteControl.Location = new System.Drawing.Point(908, 28);
         this._paletteControl.MaxValue = 1F;
         this._paletteControl.MinValue = 0F;
         this._paletteControl.Name = "_paletteControl";
         this._paletteControl.Size = new System.Drawing.Size(113, 499);
         this._paletteControl.TabIndex = 1;
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonStartStop,
            this._buttonStep,
            this._labelN,
            this._textBoxN,
            this._labelM,
            this._textBoxM,
            this.labelDz,
            this._textBoxDz,
            this._labelTau,
            this._textBoxTau,
            this._labelTheta,
            this._textBoxTheta,
            this._labelSigma,
            this._textBoxSigma,
            this._labelDelta,
            this._textBoxDelta,
            this._labelK,
            this._textBoxK,
            this._labelWindType,
            this._comboBoxWindType,
            this._labelSlice,
            this._textBoxSlice});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(1033, 25);
         this._toolStrip.TabIndex = 2;
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
         // _labelN
         // 
         this._labelN.Name = "_labelN";
         this._labelN.Size = new System.Drawing.Size(19, 22);
         this._labelN.Text = "N:";
         this._labelN.ToolTipText = "Число узлов по оси OX";
         // 
         // _textBoxN
         // 
         this._textBoxN.Name = "_textBoxN";
         this._textBoxN.Size = new System.Drawing.Size(50, 25);
         this._textBoxN.Text = "100";
         // 
         // _labelM
         // 
         this._labelM.Name = "_labelM";
         this._labelM.Size = new System.Drawing.Size(21, 22);
         this._labelM.Text = "M:";
         this._labelM.ToolTipText = "Число узлов по оси OY";
         // 
         // _textBoxM
         // 
         this._textBoxM.Name = "_textBoxM";
         this._textBoxM.Size = new System.Drawing.Size(50, 25);
         this._textBoxM.Text = "50";
         // 
         // labelDz
         // 
         this.labelDz.Name = "labelDz";
         this.labelDz.Size = new System.Drawing.Size(23, 22);
         this.labelDz.Text = "Dz:";
         // 
         // _textBoxDz
         // 
         this._textBoxDz.Name = "_textBoxDz";
         this._textBoxDz.Size = new System.Drawing.Size(50, 25);
         this._textBoxDz.Text = "0.01";
         // 
         // _labelTau
         // 
         this._labelTau.Name = "_labelTau";
         this._labelTau.Size = new System.Drawing.Size(30, 22);
         this._labelTau.Text = "Tau:";
         this._labelTau.ToolTipText = "Шаг по времени";
         // 
         // _textBoxTau
         // 
         this._textBoxTau.Name = "_textBoxTau";
         this._textBoxTau.Size = new System.Drawing.Size(50, 25);
         this._textBoxTau.Text = "0.1";
         // 
         // _labelTheta
         // 
         this._labelTheta.Name = "_labelTheta";
         this._labelTheta.Size = new System.Drawing.Size(40, 22);
         this._labelTheta.Text = "Theta:";
         this._labelTheta.ToolTipText = "Параметр сглаживания";
         // 
         // _textBoxTheta
         // 
         this._textBoxTheta.Name = "_textBoxTheta";
         this._textBoxTheta.Size = new System.Drawing.Size(50, 25);
         this._textBoxTheta.Text = "0.5";
         // 
         // _labelSigma
         // 
         this._labelSigma.Name = "_labelSigma";
         this._labelSigma.Size = new System.Drawing.Size(43, 22);
         this._labelSigma.Text = "Sigma:";
         this._labelSigma.ToolTipText = "Параметр, задающий условие сходимости";
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
         this._labelDelta.ToolTipText = "Параметр, задающий условие расходимости";
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
         this._labelK.ToolTipText = "Максимальное число итераций";
         // 
         // _textBoxK
         // 
         this._textBoxK.Name = "_textBoxK";
         this._textBoxK.Size = new System.Drawing.Size(50, 25);
         this._textBoxK.Text = "1000";
         // 
         // _labelWindType
         // 
         this._labelWindType.Name = "_labelWindType";
         this._labelWindType.Size = new System.Drawing.Size(41, 22);
         this._labelWindType.Text = "Ветер:";
         // 
         // _comboBoxWindType
         // 
         this._comboBoxWindType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._comboBoxWindType.Name = "_comboBoxWindType";
         this._comboBoxWindType.Size = new System.Drawing.Size(121, 25);
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this.timerTick);
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
         this._textBoxSlice.Text = "0";
         // 
         // IssykKulTestProblemForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(1033, 539);
         this.Controls.Add(this._toolStrip);
         this.Controls.Add(this._paletteControl);
         this.Controls.Add(this._graphControl);
         this.Name = "IssykKulTestProblemForm";
         this.Text = "Поле скоростей для озера Иссык-Куль";
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
      private System.Windows.Forms.ToolStripLabel _labelN;
      private System.Windows.Forms.ToolStripTextBox _textBoxN;
      private System.Windows.Forms.ToolStripLabel _labelM;
      private System.Windows.Forms.ToolStripTextBox _textBoxM;
      private System.Windows.Forms.ToolStripLabel _labelTheta;
      private System.Windows.Forms.ToolStripTextBox _textBoxTheta;
      private System.Windows.Forms.ToolStripLabel _labelTau;
      private System.Windows.Forms.ToolStripTextBox _textBoxTau;
      private System.Windows.Forms.ToolStripLabel _labelSigma;
      private System.Windows.Forms.ToolStripTextBox _textBoxSigma;
      private System.Windows.Forms.ToolStripLabel _labelDelta;
      private System.Windows.Forms.ToolStripTextBox _textBoxDelta;
      private System.Windows.Forms.ToolStripLabel _labelK;
      private System.Windows.Forms.ToolStripTextBox _textBoxK;
      private System.Windows.Forms.ToolStripButton _buttonStep;
      private System.Windows.Forms.ToolStripLabel _labelWindType;
      private System.Windows.Forms.ToolStripComboBox _comboBoxWindType;
      private System.Windows.Forms.ToolStripLabel labelDz;
      private System.Windows.Forms.ToolStripTextBox _textBoxDz;
      private System.Windows.Forms.Timer _timer;
      private System.Windows.Forms.ToolStripButton _buttonStartStop;
      private System.Windows.Forms.ToolStripLabel _labelSlice;
      private System.Windows.Forms.ToolStripTextBox _textBoxSlice;
   }
}