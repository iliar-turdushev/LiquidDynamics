namespace LiquidDynamics.Forms.IssykKul.Interpolation
{
   internal partial class IssykKulInterpolationForm
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
         this._panel = new System.Windows.Forms.Panel();
         this._graphControl = new ControlLibrary.Controls.GraphControl();
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._buttonReset = new System.Windows.Forms.ToolStripButton();
         this._buttonSolve = new System.Windows.Forms.ToolStripButton();
         this._labelDepthFactor = new System.Windows.Forms.ToolStripLabel();
         this._textBoxDepthFactor = new System.Windows.Forms.ToolStripTextBox();
         this._labelGraph = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxGraph = new System.Windows.Forms.ToolStripComboBox();
         this._labelN = new System.Windows.Forms.ToolStripLabel();
         this._textBoxN = new System.Windows.Forms.ToolStripTextBox();
         this._labelM = new System.Windows.Forms.ToolStripLabel();
         this._textBoxM = new System.Windows.Forms.ToolStripTextBox();
         this._labelTheta = new System.Windows.Forms.ToolStripLabel();
         this._textBoxTheta = new System.Windows.Forms.ToolStripTextBox();
         this._paletteControl = new ControlLibrary.Controls.PaletteControl();
         this._panel.SuspendLayout();
         this._toolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _panel
         // 
         this._panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._panel.Controls.Add(this._graphControl);
         this._panel.Location = new System.Drawing.Point(12, 41);
         this._panel.Name = "_panel";
         this._panel.Size = new System.Drawing.Size(857, 479);
         this._panel.TabIndex = 4;
         // 
         // _graphControl
         // 
         this._graphControl.Caption = "Иссык-Куль";
         this._graphControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this._graphControl.Location = new System.Drawing.Point(0, 0);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(857, 479);
         this._graphControl.TabIndex = 1;
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset,
            this._buttonSolve,
            this._labelDepthFactor,
            this._textBoxDepthFactor,
            this._labelGraph,
            this._comboBoxGraph,
            this._labelN,
            this._textBoxN,
            this._labelM,
            this._textBoxM,
            this._labelTheta,
            this._textBoxTheta});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(1000, 25);
         this._toolStrip.TabIndex = 5;
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
         // _labelDepthFactor
         // 
         this._labelDepthFactor.Name = "_labelDepthFactor";
         this._labelDepthFactor.Size = new System.Drawing.Size(126, 22);
         this._labelDepthFactor.Text = "Множитель глубины:";
         // 
         // _textBoxDepthFactor
         // 
         this._textBoxDepthFactor.Name = "_textBoxDepthFactor";
         this._textBoxDepthFactor.Size = new System.Drawing.Size(50, 25);
         this._textBoxDepthFactor.Text = "10";
         // 
         // _labelGraph
         // 
         this._labelGraph.Name = "_labelGraph";
         this._labelGraph.Size = new System.Drawing.Size(51, 22);
         this._labelGraph.Text = "График:";
         // 
         // _comboBoxGraph
         // 
         this._comboBoxGraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._comboBoxGraph.Items.AddRange(new object[] {
            "2D",
            "3D"});
         this._comboBoxGraph.Name = "_comboBoxGraph";
         this._comboBoxGraph.Size = new System.Drawing.Size(75, 25);
         this._comboBoxGraph.SelectedIndexChanged += new System.EventHandler(this.comboBoxGraphSelectedIndexChanged);
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
         this._textBoxN.Text = "50";
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
         this._textBoxM.Text = "25";
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
         // _paletteControl
         // 
         this._paletteControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._paletteControl.Location = new System.Drawing.Point(875, 41);
         this._paletteControl.MaxValue = 1F;
         this._paletteControl.MinValue = 0F;
         this._paletteControl.Name = "_paletteControl";
         this._paletteControl.Size = new System.Drawing.Size(113, 479);
         this._paletteControl.TabIndex = 6;
         // 
         // IssykKulInterpolationForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(1000, 532);
         this.Controls.Add(this._paletteControl);
         this.Controls.Add(this._toolStrip);
         this.Controls.Add(this._panel);
         this.Name = "IssykKulInterpolationForm";
         this.Text = "Восстановление дна оз. Иссык-Куль методом Кригинга";
         this._panel.ResumeLayout(false);
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Panel _panel;
      private ControlLibrary.Controls.GraphControl _graphControl;
      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripButton _buttonReset;
      private System.Windows.Forms.ToolStripButton _buttonSolve;
      private System.Windows.Forms.ToolStripLabel _labelDepthFactor;
      private System.Windows.Forms.ToolStripTextBox _textBoxDepthFactor;
      private System.Windows.Forms.ToolStripLabel _labelGraph;
      private System.Windows.Forms.ToolStripComboBox _comboBoxGraph;
      private System.Windows.Forms.ToolStripTextBox _textBoxN;
      private System.Windows.Forms.ToolStripLabel _labelN;
      private System.Windows.Forms.ToolStripLabel _labelM;
      private System.Windows.Forms.ToolStripTextBox _textBoxM;
      private ControlLibrary.Controls.PaletteControl _paletteControl;
      private System.Windows.Forms.ToolStripTextBox _textBoxTheta;
      private System.Windows.Forms.ToolStripLabel _labelTheta;
   }
}