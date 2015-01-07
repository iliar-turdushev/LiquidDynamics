namespace LiquidDynamics.Forms.StommelModel
{
   internal partial class StommelModelForm
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
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._buttonReset = new System.Windows.Forms.ToolStripButton();
         this._buttonSolve = new System.Windows.Forms.ToolStripButton();
         this._labelEpsilon = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxEpsilon = new System.Windows.Forms.ToolStripComboBox();
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
         this._graphControl = new ControlLibrary.Controls.GraphControl();
         this._toolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonSolve,
            this._labelEpsilon,
            this._comboBoxEpsilon,
            this._labelN,
            this._textBoxN,
            this._labelM,
            this._textBoxM,
            this._labelSigma,
            this._textBoxSigma,
            this._labelDelta,
            this._textBoxDelta,
            this._labelK,
            this._textBoxK});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(751, 25);
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
         // _buttonSolve
         // 
         this._buttonSolve.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonSolve.Enabled = false;
         this._buttonSolve.Image = global::LiquidDynamics.Properties.Resources.Start;
         this._buttonSolve.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonSolve.Name = "_buttonSolve";
         this._buttonSolve.Size = new System.Drawing.Size(23, 22);
         this._buttonSolve.Click += new System.EventHandler(this.buttonSolveClick);
         // 
         // _labelEpsilon
         // 
         this._labelEpsilon.Name = "_labelEpsilon";
         this._labelEpsilon.Size = new System.Drawing.Size(48, 22);
         this._labelEpsilon.Text = "Epsilon:";
         this._labelEpsilon.ToolTipText = "Параметр модели";
         // 
         // _comboBoxEpsilon
         // 
         this._comboBoxEpsilon.Name = "_comboBoxEpsilon";
         this._comboBoxEpsilon.Size = new System.Drawing.Size(100, 25);
         // 
         // _labelN
         // 
         this._labelN.Name = "_labelN";
         this._labelN.Size = new System.Drawing.Size(19, 22);
         this._labelN.Text = "N:";
         this._labelN.ToolTipText = "Число узлов по оси X";
         // 
         // _textBoxN
         // 
         this._textBoxN.Name = "_textBoxN";
         this._textBoxN.Size = new System.Drawing.Size(75, 25);
         this._textBoxN.Text = "20";
         // 
         // _labelM
         // 
         this._labelM.Name = "_labelM";
         this._labelM.Size = new System.Drawing.Size(21, 22);
         this._labelM.Text = "M:";
         this._labelM.ToolTipText = "Число узлов по оси Y";
         // 
         // _textBoxM
         // 
         this._textBoxM.Name = "_textBoxM";
         this._textBoxM.Size = new System.Drawing.Size(75, 25);
         this._textBoxM.Text = "20";
         // 
         // _labelSigma
         // 
         this._labelSigma.Name = "_labelSigma";
         this._labelSigma.Size = new System.Drawing.Size(43, 22);
         this._labelSigma.Text = "Sigma:";
         this._labelSigma.ToolTipText = "Параметр критерия сходимости";
         // 
         // _textBoxSigma
         // 
         this._textBoxSigma.Name = "_textBoxSigma";
         this._textBoxSigma.Size = new System.Drawing.Size(75, 25);
         this._textBoxSigma.Text = "0.0001";
         // 
         // _labelDelta
         // 
         this._labelDelta.Name = "_labelDelta";
         this._labelDelta.Size = new System.Drawing.Size(37, 22);
         this._labelDelta.Text = "Delta:";
         this._labelDelta.ToolTipText = "Параметр критерия расходимости";
         // 
         // _textBoxDelta
         // 
         this._textBoxDelta.Name = "_textBoxDelta";
         this._textBoxDelta.Size = new System.Drawing.Size(75, 25);
         this._textBoxDelta.Text = "1000";
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
         this._textBoxK.Size = new System.Drawing.Size(75, 25);
         this._textBoxK.Text = "1000";
         // 
         // _graphControl
         // 
         this._graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._graphControl.Caption = null;
         this._graphControl.Location = new System.Drawing.Point(12, 28);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(727, 408);
         this._graphControl.TabIndex = 2;
         // 
         // StommelModelForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(751, 448);
         this.Controls.Add(this._graphControl);
         this.Controls.Add(this._toolStrip);
         this.Name = "StommelModelForm";
         this.Text = "Модель Стоммела";
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripLabel _labelEpsilon;
      private System.Windows.Forms.ToolStripLabel _labelSigma;
      private System.Windows.Forms.ToolStripTextBox _textBoxSigma;
      private System.Windows.Forms.ToolStripLabel _labelDelta;
      private System.Windows.Forms.ToolStripTextBox _textBoxDelta;
      private System.Windows.Forms.ToolStripLabel _labelN;
      private System.Windows.Forms.ToolStripTextBox _textBoxN;
      private System.Windows.Forms.ToolStripLabel _labelM;
      private System.Windows.Forms.ToolStripTextBox _textBoxM;
      private System.Windows.Forms.ToolStripLabel _labelK;
      private System.Windows.Forms.ToolStripTextBox _textBoxK;
      private System.Windows.Forms.ToolStripButton _buttonReset;
      private System.Windows.Forms.ToolStripButton _buttonSolve;
      private System.Windows.Forms.ToolStripComboBox _comboBoxEpsilon;
      private ControlLibrary.Controls.GraphControl _graphControl;

   }
}