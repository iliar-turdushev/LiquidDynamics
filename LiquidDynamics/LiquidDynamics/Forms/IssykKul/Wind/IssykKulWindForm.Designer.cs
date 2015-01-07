namespace LiquidDynamics.Forms.IssykKul.Wind
{
   partial class IssykKulWindForm
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
         this._buttonBuildWind = new System.Windows.Forms.ToolStripButton();
         this._labelN = new System.Windows.Forms.ToolStripLabel();
         this._textBoxN = new System.Windows.Forms.ToolStripTextBox();
         this._labelM = new System.Windows.Forms.ToolStripLabel();
         this._textBoxM = new System.Windows.Forms.ToolStripTextBox();
         this._labelWind = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxWind = new System.Windows.Forms.ToolStripComboBox();
         this._graphControl = new ControlLibrary.Controls.GraphControl();
         this._paletteControl = new ControlLibrary.Controls.PaletteControl();
         this._textBoxB2 = new System.Windows.Forms.TextBox();
         this._textBoxB1 = new System.Windows.Forms.TextBox();
         this._labelB2 = new System.Windows.Forms.Label();
         this._labelB1 = new System.Windows.Forms.Label();
         this._textBoxA2 = new System.Windows.Forms.TextBox();
         this._textBoxA1 = new System.Windows.Forms.TextBox();
         this._labelA2 = new System.Windows.Forms.Label();
         this._labelA1 = new System.Windows.Forms.Label();
         this._textBoxA4y = new System.Windows.Forms.TextBox();
         this._textBoxA3y = new System.Windows.Forms.TextBox();
         this._textBoxA2y = new System.Windows.Forms.TextBox();
         this._textBoxA1y = new System.Windows.Forms.TextBox();
         this._labelA4y = new System.Windows.Forms.Label();
         this._labelA3y = new System.Windows.Forms.Label();
         this._labelA2y = new System.Windows.Forms.Label();
         this._labelA1y = new System.Windows.Forms.Label();
         this._textBoxA4x = new System.Windows.Forms.TextBox();
         this._textBoxA3x = new System.Windows.Forms.TextBox();
         this._textBoxA2x = new System.Windows.Forms.TextBox();
         this._textBoxA1x = new System.Windows.Forms.TextBox();
         this._labelA4x = new System.Windows.Forms.Label();
         this._labelA3x = new System.Windows.Forms.Label();
         this._labelA2x = new System.Windows.Forms.Label();
         this._labelA1x = new System.Windows.Forms.Label();
         this._groupBoxUlanParameters = new System.Windows.Forms.GroupBox();
         this._groupBoxSantashParameters = new System.Windows.Forms.GroupBox();
         this._textBoxB4 = new System.Windows.Forms.TextBox();
         this._textBoxB3 = new System.Windows.Forms.TextBox();
         this._labelB4 = new System.Windows.Forms.Label();
         this._labelB3 = new System.Windows.Forms.Label();
         this._textBoxA4 = new System.Windows.Forms.TextBox();
         this._textBoxA3 = new System.Windows.Forms.TextBox();
         this._labelA4 = new System.Windows.Forms.Label();
         this._labelA3 = new System.Windows.Forms.Label();
         this._textBoxB4y = new System.Windows.Forms.TextBox();
         this._textBoxB3y = new System.Windows.Forms.TextBox();
         this._textBoxB2y = new System.Windows.Forms.TextBox();
         this._textBoxB1y = new System.Windows.Forms.TextBox();
         this._labelB4y = new System.Windows.Forms.Label();
         this._labelB3y = new System.Windows.Forms.Label();
         this._labelB2y = new System.Windows.Forms.Label();
         this._labelB1y = new System.Windows.Forms.Label();
         this._textBoxB4x = new System.Windows.Forms.TextBox();
         this._textBoxB3x = new System.Windows.Forms.TextBox();
         this._textBoxB2x = new System.Windows.Forms.TextBox();
         this._textBoxB1x = new System.Windows.Forms.TextBox();
         this._labelB4x = new System.Windows.Forms.Label();
         this._labelB3x = new System.Windows.Forms.Label();
         this._labelB2x = new System.Windows.Forms.Label();
         this._labelB1x = new System.Windows.Forms.Label();
         this._toolStrip.SuspendLayout();
         this._groupBoxUlanParameters.SuspendLayout();
         this._groupBoxSantashParameters.SuspendLayout();
         this.SuspendLayout();
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonBuildWind,
            this._labelN,
            this._textBoxN,
            this._labelM,
            this._textBoxM,
            this._labelWind,
            this._comboBoxWind});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(763, 25);
         this._toolStrip.TabIndex = 0;
         this._toolStrip.Text = "toolStrip";
         // 
         // _buttonBuildWind
         // 
         this._buttonBuildWind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonBuildWind.Image = global::LiquidDynamics.Properties.Resources.Start;
         this._buttonBuildWind.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonBuildWind.Name = "_buttonBuildWind";
         this._buttonBuildWind.Size = new System.Drawing.Size(23, 22);
         this._buttonBuildWind.ToolTipText = "Построить поле ветра";
         this._buttonBuildWind.Click += new System.EventHandler(this.buttonBuildWindClick);
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
         this._textBoxM.Text = "50";
         // 
         // _labelWind
         // 
         this._labelWind.Name = "_labelWind";
         this._labelWind.Size = new System.Drawing.Size(41, 22);
         this._labelWind.Text = "Ветер:";
         // 
         // _comboBoxWind
         // 
         this._comboBoxWind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._comboBoxWind.Name = "_comboBoxWind";
         this._comboBoxWind.Size = new System.Drawing.Size(121, 25);
         // 
         // _graphControl
         // 
         this._graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._graphControl.Caption = "Ветровой режим";
         this._graphControl.Location = new System.Drawing.Point(12, 28);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(620, 387);
         this._graphControl.TabIndex = 1;
         // 
         // _paletteControl
         // 
         this._paletteControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._paletteControl.Location = new System.Drawing.Point(638, 28);
         this._paletteControl.MaxValue = 1F;
         this._paletteControl.MinValue = 0F;
         this._paletteControl.Name = "_paletteControl";
         this._paletteControl.Size = new System.Drawing.Size(113, 387);
         this._paletteControl.TabIndex = 2;
         // 
         // _textBoxB2
         // 
         this._textBoxB2.Location = new System.Drawing.Point(306, 19);
         this._textBoxB2.Name = "_textBoxB2";
         this._textBoxB2.Size = new System.Drawing.Size(50, 20);
         this._textBoxB2.TabIndex = 21;
         // 
         // _textBoxB1
         // 
         this._textBoxB1.Location = new System.Drawing.Point(217, 19);
         this._textBoxB1.Name = "_textBoxB1";
         this._textBoxB1.Size = new System.Drawing.Size(50, 20);
         this._textBoxB1.TabIndex = 20;
         // 
         // _labelB2
         // 
         this._labelB2.AutoSize = true;
         this._labelB2.Location = new System.Drawing.Point(277, 22);
         this._labelB2.Name = "_labelB2";
         this._labelB2.Size = new System.Drawing.Size(23, 13);
         this._labelB2.TabIndex = 19;
         this._labelB2.Text = "B2:";
         // 
         // _labelB1
         // 
         this._labelB1.AutoSize = true;
         this._labelB1.Location = new System.Drawing.Point(188, 22);
         this._labelB1.Name = "_labelB1";
         this._labelB1.Size = new System.Drawing.Size(23, 13);
         this._labelB1.TabIndex = 18;
         this._labelB1.Text = "B1:";
         // 
         // _textBoxA2
         // 
         this._textBoxA2.Location = new System.Drawing.Point(128, 19);
         this._textBoxA2.Name = "_textBoxA2";
         this._textBoxA2.Size = new System.Drawing.Size(50, 20);
         this._textBoxA2.TabIndex = 17;
         // 
         // _textBoxA1
         // 
         this._textBoxA1.Location = new System.Drawing.Point(39, 19);
         this._textBoxA1.Name = "_textBoxA1";
         this._textBoxA1.Size = new System.Drawing.Size(50, 20);
         this._textBoxA1.TabIndex = 16;
         // 
         // _labelA2
         // 
         this._labelA2.AutoSize = true;
         this._labelA2.Location = new System.Drawing.Point(99, 22);
         this._labelA2.Name = "_labelA2";
         this._labelA2.Size = new System.Drawing.Size(23, 13);
         this._labelA2.TabIndex = 15;
         this._labelA2.Text = "A2:";
         // 
         // _labelA1
         // 
         this._labelA1.AutoSize = true;
         this._labelA1.Location = new System.Drawing.Point(10, 22);
         this._labelA1.Name = "_labelA1";
         this._labelA1.Size = new System.Drawing.Size(23, 13);
         this._labelA1.TabIndex = 14;
         this._labelA1.Text = "A1:";
         // 
         // _textBoxA4y
         // 
         this._textBoxA4y.Location = new System.Drawing.Point(306, 71);
         this._textBoxA4y.Name = "_textBoxA4y";
         this._textBoxA4y.Size = new System.Drawing.Size(50, 20);
         this._textBoxA4y.TabIndex = 47;
         // 
         // _textBoxA3y
         // 
         this._textBoxA3y.Location = new System.Drawing.Point(217, 71);
         this._textBoxA3y.Name = "_textBoxA3y";
         this._textBoxA3y.Size = new System.Drawing.Size(50, 20);
         this._textBoxA3y.TabIndex = 46;
         // 
         // _textBoxA2y
         // 
         this._textBoxA2y.Location = new System.Drawing.Point(128, 71);
         this._textBoxA2y.Name = "_textBoxA2y";
         this._textBoxA2y.Size = new System.Drawing.Size(50, 20);
         this._textBoxA2y.TabIndex = 45;
         // 
         // _textBoxA1y
         // 
         this._textBoxA1y.Location = new System.Drawing.Point(39, 71);
         this._textBoxA1y.Name = "_textBoxA1y";
         this._textBoxA1y.Size = new System.Drawing.Size(50, 20);
         this._textBoxA1y.TabIndex = 44;
         // 
         // _labelA4y
         // 
         this._labelA4y.AutoSize = true;
         this._labelA4y.Location = new System.Drawing.Point(273, 74);
         this._labelA4y.Name = "_labelA4y";
         this._labelA4y.Size = new System.Drawing.Size(27, 13);
         this._labelA4y.TabIndex = 43;
         this._labelA4y.Text = "a4y:";
         // 
         // _labelA3y
         // 
         this._labelA3y.AutoSize = true;
         this._labelA3y.Location = new System.Drawing.Point(184, 74);
         this._labelA3y.Name = "_labelA3y";
         this._labelA3y.Size = new System.Drawing.Size(27, 13);
         this._labelA3y.TabIndex = 42;
         this._labelA3y.Text = "a3y:";
         // 
         // _labelA2y
         // 
         this._labelA2y.AutoSize = true;
         this._labelA2y.Location = new System.Drawing.Point(95, 74);
         this._labelA2y.Name = "_labelA2y";
         this._labelA2y.Size = new System.Drawing.Size(27, 13);
         this._labelA2y.TabIndex = 41;
         this._labelA2y.Text = "a2y:";
         // 
         // _labelA1y
         // 
         this._labelA1y.AutoSize = true;
         this._labelA1y.Location = new System.Drawing.Point(6, 74);
         this._labelA1y.Name = "_labelA1y";
         this._labelA1y.Size = new System.Drawing.Size(27, 13);
         this._labelA1y.TabIndex = 40;
         this._labelA1y.Text = "a1y:";
         // 
         // _textBoxA4x
         // 
         this._textBoxA4x.Location = new System.Drawing.Point(306, 45);
         this._textBoxA4x.Name = "_textBoxA4x";
         this._textBoxA4x.Size = new System.Drawing.Size(50, 20);
         this._textBoxA4x.TabIndex = 39;
         // 
         // _textBoxA3x
         // 
         this._textBoxA3x.Location = new System.Drawing.Point(217, 45);
         this._textBoxA3x.Name = "_textBoxA3x";
         this._textBoxA3x.Size = new System.Drawing.Size(50, 20);
         this._textBoxA3x.TabIndex = 38;
         // 
         // _textBoxA2x
         // 
         this._textBoxA2x.Location = new System.Drawing.Point(128, 45);
         this._textBoxA2x.Name = "_textBoxA2x";
         this._textBoxA2x.Size = new System.Drawing.Size(50, 20);
         this._textBoxA2x.TabIndex = 37;
         // 
         // _textBoxA1x
         // 
         this._textBoxA1x.Location = new System.Drawing.Point(39, 45);
         this._textBoxA1x.Name = "_textBoxA1x";
         this._textBoxA1x.Size = new System.Drawing.Size(50, 20);
         this._textBoxA1x.TabIndex = 36;
         // 
         // _labelA4x
         // 
         this._labelA4x.AutoSize = true;
         this._labelA4x.Location = new System.Drawing.Point(273, 48);
         this._labelA4x.Name = "_labelA4x";
         this._labelA4x.Size = new System.Drawing.Size(27, 13);
         this._labelA4x.TabIndex = 35;
         this._labelA4x.Text = "a4x:";
         // 
         // _labelA3x
         // 
         this._labelA3x.AutoSize = true;
         this._labelA3x.Location = new System.Drawing.Point(184, 48);
         this._labelA3x.Name = "_labelA3x";
         this._labelA3x.Size = new System.Drawing.Size(27, 13);
         this._labelA3x.TabIndex = 34;
         this._labelA3x.Text = "a3x:";
         // 
         // _labelA2x
         // 
         this._labelA2x.AutoSize = true;
         this._labelA2x.Location = new System.Drawing.Point(95, 48);
         this._labelA2x.Name = "_labelA2x";
         this._labelA2x.Size = new System.Drawing.Size(27, 13);
         this._labelA2x.TabIndex = 33;
         this._labelA2x.Text = "a2x:";
         // 
         // _labelA1x
         // 
         this._labelA1x.AutoSize = true;
         this._labelA1x.Location = new System.Drawing.Point(6, 48);
         this._labelA1x.Name = "_labelA1x";
         this._labelA1x.Size = new System.Drawing.Size(27, 13);
         this._labelA1x.TabIndex = 32;
         this._labelA1x.Text = "a1x:";
         // 
         // _groupBoxUlanParameters
         // 
         this._groupBoxUlanParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA1);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA4y);
         this._groupBoxUlanParameters.Controls.Add(this._labelA1);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA3y);
         this._groupBoxUlanParameters.Controls.Add(this._labelA2);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA2y);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA2);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA1y);
         this._groupBoxUlanParameters.Controls.Add(this._labelB1);
         this._groupBoxUlanParameters.Controls.Add(this._labelA4y);
         this._groupBoxUlanParameters.Controls.Add(this._labelB2);
         this._groupBoxUlanParameters.Controls.Add(this._labelA3y);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxB1);
         this._groupBoxUlanParameters.Controls.Add(this._labelA2y);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxB2);
         this._groupBoxUlanParameters.Controls.Add(this._labelA1y);
         this._groupBoxUlanParameters.Controls.Add(this._labelA1x);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA4x);
         this._groupBoxUlanParameters.Controls.Add(this._labelA2x);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA3x);
         this._groupBoxUlanParameters.Controls.Add(this._labelA3x);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA2x);
         this._groupBoxUlanParameters.Controls.Add(this._labelA4x);
         this._groupBoxUlanParameters.Controls.Add(this._textBoxA1x);
         this._groupBoxUlanParameters.Location = new System.Drawing.Point(12, 421);
         this._groupBoxUlanParameters.Name = "_groupBoxUlanParameters";
         this._groupBoxUlanParameters.Size = new System.Drawing.Size(366, 100);
         this._groupBoxUlanParameters.TabIndex = 48;
         this._groupBoxUlanParameters.TabStop = false;
         this._groupBoxUlanParameters.Text = "Улан";
         // 
         // _groupBoxSantashParameters
         // 
         this._groupBoxSantashParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB4y);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB3y);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB2y);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB1y);
         this._groupBoxSantashParameters.Controls.Add(this._labelB4y);
         this._groupBoxSantashParameters.Controls.Add(this._labelB3y);
         this._groupBoxSantashParameters.Controls.Add(this._labelB2y);
         this._groupBoxSantashParameters.Controls.Add(this._labelB1y);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB4x);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB3x);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB2x);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB1x);
         this._groupBoxSantashParameters.Controls.Add(this._labelB4x);
         this._groupBoxSantashParameters.Controls.Add(this._labelB3x);
         this._groupBoxSantashParameters.Controls.Add(this._labelB2x);
         this._groupBoxSantashParameters.Controls.Add(this._labelB1x);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB4);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxB3);
         this._groupBoxSantashParameters.Controls.Add(this._labelB4);
         this._groupBoxSantashParameters.Controls.Add(this._labelB3);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxA4);
         this._groupBoxSantashParameters.Controls.Add(this._textBoxA3);
         this._groupBoxSantashParameters.Controls.Add(this._labelA4);
         this._groupBoxSantashParameters.Controls.Add(this._labelA3);
         this._groupBoxSantashParameters.Location = new System.Drawing.Point(385, 421);
         this._groupBoxSantashParameters.Name = "_groupBoxSantashParameters";
         this._groupBoxSantashParameters.Size = new System.Drawing.Size(366, 100);
         this._groupBoxSantashParameters.TabIndex = 49;
         this._groupBoxSantashParameters.TabStop = false;
         this._groupBoxSantashParameters.Text = "Санташ";
         // 
         // _textBoxB4
         // 
         this._textBoxB4.Location = new System.Drawing.Point(306, 19);
         this._textBoxB4.Name = "_textBoxB4";
         this._textBoxB4.Size = new System.Drawing.Size(50, 20);
         this._textBoxB4.TabIndex = 23;
         // 
         // _textBoxB3
         // 
         this._textBoxB3.Location = new System.Drawing.Point(217, 19);
         this._textBoxB3.Name = "_textBoxB3";
         this._textBoxB3.Size = new System.Drawing.Size(50, 20);
         this._textBoxB3.TabIndex = 22;
         // 
         // _labelB4
         // 
         this._labelB4.AutoSize = true;
         this._labelB4.Location = new System.Drawing.Point(277, 22);
         this._labelB4.Name = "_labelB4";
         this._labelB4.Size = new System.Drawing.Size(23, 13);
         this._labelB4.TabIndex = 21;
         this._labelB4.Text = "B4:";
         // 
         // _labelB3
         // 
         this._labelB3.AutoSize = true;
         this._labelB3.Location = new System.Drawing.Point(188, 22);
         this._labelB3.Name = "_labelB3";
         this._labelB3.Size = new System.Drawing.Size(23, 13);
         this._labelB3.TabIndex = 20;
         this._labelB3.Text = "B3:";
         // 
         // _textBoxA4
         // 
         this._textBoxA4.Location = new System.Drawing.Point(128, 19);
         this._textBoxA4.Name = "_textBoxA4";
         this._textBoxA4.Size = new System.Drawing.Size(50, 20);
         this._textBoxA4.TabIndex = 19;
         // 
         // _textBoxA3
         // 
         this._textBoxA3.Location = new System.Drawing.Point(39, 19);
         this._textBoxA3.Name = "_textBoxA3";
         this._textBoxA3.Size = new System.Drawing.Size(50, 20);
         this._textBoxA3.TabIndex = 18;
         // 
         // _labelA4
         // 
         this._labelA4.AutoSize = true;
         this._labelA4.Location = new System.Drawing.Point(99, 22);
         this._labelA4.Name = "_labelA4";
         this._labelA4.Size = new System.Drawing.Size(23, 13);
         this._labelA4.TabIndex = 17;
         this._labelA4.Text = "A4:";
         // 
         // _labelA3
         // 
         this._labelA3.AutoSize = true;
         this._labelA3.Location = new System.Drawing.Point(10, 22);
         this._labelA3.Name = "_labelA3";
         this._labelA3.Size = new System.Drawing.Size(23, 13);
         this._labelA3.TabIndex = 16;
         this._labelA3.Text = "A3:";
         // 
         // _textBoxB4y
         // 
         this._textBoxB4y.Location = new System.Drawing.Point(306, 71);
         this._textBoxB4y.Name = "_textBoxB4y";
         this._textBoxB4y.Size = new System.Drawing.Size(50, 20);
         this._textBoxB4y.TabIndex = 63;
         // 
         // _textBoxB3y
         // 
         this._textBoxB3y.Location = new System.Drawing.Point(217, 71);
         this._textBoxB3y.Name = "_textBoxB3y";
         this._textBoxB3y.Size = new System.Drawing.Size(50, 20);
         this._textBoxB3y.TabIndex = 62;
         // 
         // _textBoxB2y
         // 
         this._textBoxB2y.Location = new System.Drawing.Point(128, 71);
         this._textBoxB2y.Name = "_textBoxB2y";
         this._textBoxB2y.Size = new System.Drawing.Size(50, 20);
         this._textBoxB2y.TabIndex = 61;
         // 
         // _textBoxB1y
         // 
         this._textBoxB1y.Location = new System.Drawing.Point(39, 71);
         this._textBoxB1y.Name = "_textBoxB1y";
         this._textBoxB1y.Size = new System.Drawing.Size(50, 20);
         this._textBoxB1y.TabIndex = 60;
         // 
         // _labelB4y
         // 
         this._labelB4y.AutoSize = true;
         this._labelB4y.Location = new System.Drawing.Point(273, 74);
         this._labelB4y.Name = "_labelB4y";
         this._labelB4y.Size = new System.Drawing.Size(27, 13);
         this._labelB4y.TabIndex = 59;
         this._labelB4y.Text = "b4y:";
         // 
         // _labelB3y
         // 
         this._labelB3y.AutoSize = true;
         this._labelB3y.Location = new System.Drawing.Point(184, 74);
         this._labelB3y.Name = "_labelB3y";
         this._labelB3y.Size = new System.Drawing.Size(27, 13);
         this._labelB3y.TabIndex = 58;
         this._labelB3y.Text = "b3y:";
         // 
         // _labelB2y
         // 
         this._labelB2y.AutoSize = true;
         this._labelB2y.Location = new System.Drawing.Point(95, 74);
         this._labelB2y.Name = "_labelB2y";
         this._labelB2y.Size = new System.Drawing.Size(27, 13);
         this._labelB2y.TabIndex = 57;
         this._labelB2y.Text = "b2y:";
         // 
         // _labelB1y
         // 
         this._labelB1y.AutoSize = true;
         this._labelB1y.Location = new System.Drawing.Point(6, 74);
         this._labelB1y.Name = "_labelB1y";
         this._labelB1y.Size = new System.Drawing.Size(27, 13);
         this._labelB1y.TabIndex = 56;
         this._labelB1y.Text = "b1y:";
         // 
         // _textBoxB4x
         // 
         this._textBoxB4x.Location = new System.Drawing.Point(306, 45);
         this._textBoxB4x.Name = "_textBoxB4x";
         this._textBoxB4x.Size = new System.Drawing.Size(50, 20);
         this._textBoxB4x.TabIndex = 55;
         // 
         // _textBoxB3x
         // 
         this._textBoxB3x.Location = new System.Drawing.Point(217, 45);
         this._textBoxB3x.Name = "_textBoxB3x";
         this._textBoxB3x.Size = new System.Drawing.Size(50, 20);
         this._textBoxB3x.TabIndex = 54;
         // 
         // _textBoxB2x
         // 
         this._textBoxB2x.Location = new System.Drawing.Point(128, 45);
         this._textBoxB2x.Name = "_textBoxB2x";
         this._textBoxB2x.Size = new System.Drawing.Size(50, 20);
         this._textBoxB2x.TabIndex = 53;
         // 
         // _textBoxB1x
         // 
         this._textBoxB1x.Location = new System.Drawing.Point(39, 45);
         this._textBoxB1x.Name = "_textBoxB1x";
         this._textBoxB1x.Size = new System.Drawing.Size(50, 20);
         this._textBoxB1x.TabIndex = 52;
         // 
         // _labelB4x
         // 
         this._labelB4x.AutoSize = true;
         this._labelB4x.Location = new System.Drawing.Point(273, 48);
         this._labelB4x.Name = "_labelB4x";
         this._labelB4x.Size = new System.Drawing.Size(27, 13);
         this._labelB4x.TabIndex = 51;
         this._labelB4x.Text = "b4x:";
         // 
         // _labelB3x
         // 
         this._labelB3x.AutoSize = true;
         this._labelB3x.Location = new System.Drawing.Point(184, 48);
         this._labelB3x.Name = "_labelB3x";
         this._labelB3x.Size = new System.Drawing.Size(27, 13);
         this._labelB3x.TabIndex = 50;
         this._labelB3x.Text = "b3x:";
         // 
         // _labelB2x
         // 
         this._labelB2x.AutoSize = true;
         this._labelB2x.Location = new System.Drawing.Point(95, 48);
         this._labelB2x.Name = "_labelB2x";
         this._labelB2x.Size = new System.Drawing.Size(27, 13);
         this._labelB2x.TabIndex = 49;
         this._labelB2x.Text = "b2x:";
         // 
         // _labelB1x
         // 
         this._labelB1x.AutoSize = true;
         this._labelB1x.Location = new System.Drawing.Point(6, 48);
         this._labelB1x.Name = "_labelB1x";
         this._labelB1x.Size = new System.Drawing.Size(27, 13);
         this._labelB1x.TabIndex = 48;
         this._labelB1x.Text = "b1x:";
         // 
         // IssykKulWindForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(763, 533);
         this.Controls.Add(this._groupBoxSantashParameters);
         this.Controls.Add(this._groupBoxUlanParameters);
         this.Controls.Add(this._paletteControl);
         this.Controls.Add(this._graphControl);
         this.Controls.Add(this._toolStrip);
         this.Name = "IssykKulWindForm";
         this.Text = "Моделирование ветрового режима для озера Иссык-Куль";
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this._groupBoxUlanParameters.ResumeLayout(false);
         this._groupBoxUlanParameters.PerformLayout();
         this._groupBoxSantashParameters.ResumeLayout(false);
         this._groupBoxSantashParameters.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ToolStrip _toolStrip;
      private ControlLibrary.Controls.GraphControl _graphControl;
      private System.Windows.Forms.ToolStripButton _buttonBuildWind;
      private System.Windows.Forms.ToolStripLabel _labelN;
      private System.Windows.Forms.ToolStripTextBox _textBoxN;
      private System.Windows.Forms.ToolStripLabel _labelM;
      private System.Windows.Forms.ToolStripTextBox _textBoxM;
      private ControlLibrary.Controls.PaletteControl _paletteControl;
      private System.Windows.Forms.ToolStripLabel _labelWind;
      private System.Windows.Forms.ToolStripComboBox _comboBoxWind;
      private System.Windows.Forms.TextBox _textBoxB2;
      private System.Windows.Forms.TextBox _textBoxB1;
      private System.Windows.Forms.Label _labelB2;
      private System.Windows.Forms.Label _labelB1;
      private System.Windows.Forms.TextBox _textBoxA2;
      private System.Windows.Forms.TextBox _textBoxA1;
      private System.Windows.Forms.Label _labelA2;
      private System.Windows.Forms.Label _labelA1;
      private System.Windows.Forms.TextBox _textBoxA4y;
      private System.Windows.Forms.TextBox _textBoxA3y;
      private System.Windows.Forms.TextBox _textBoxA2y;
      private System.Windows.Forms.TextBox _textBoxA1y;
      private System.Windows.Forms.Label _labelA4y;
      private System.Windows.Forms.Label _labelA3y;
      private System.Windows.Forms.Label _labelA2y;
      private System.Windows.Forms.Label _labelA1y;
      private System.Windows.Forms.TextBox _textBoxA4x;
      private System.Windows.Forms.TextBox _textBoxA3x;
      private System.Windows.Forms.TextBox _textBoxA2x;
      private System.Windows.Forms.TextBox _textBoxA1x;
      private System.Windows.Forms.Label _labelA4x;
      private System.Windows.Forms.Label _labelA3x;
      private System.Windows.Forms.Label _labelA2x;
      private System.Windows.Forms.Label _labelA1x;
      private System.Windows.Forms.GroupBox _groupBoxUlanParameters;
      private System.Windows.Forms.GroupBox _groupBoxSantashParameters;
      private System.Windows.Forms.TextBox _textBoxB4;
      private System.Windows.Forms.TextBox _textBoxB3;
      private System.Windows.Forms.Label _labelB4;
      private System.Windows.Forms.Label _labelB3;
      private System.Windows.Forms.TextBox _textBoxA4;
      private System.Windows.Forms.TextBox _textBoxA3;
      private System.Windows.Forms.Label _labelA4;
      private System.Windows.Forms.Label _labelA3;
      private System.Windows.Forms.TextBox _textBoxB4y;
      private System.Windows.Forms.TextBox _textBoxB3y;
      private System.Windows.Forms.TextBox _textBoxB2y;
      private System.Windows.Forms.TextBox _textBoxB1y;
      private System.Windows.Forms.Label _labelB4y;
      private System.Windows.Forms.Label _labelB3y;
      private System.Windows.Forms.Label _labelB2y;
      private System.Windows.Forms.Label _labelB1y;
      private System.Windows.Forms.TextBox _textBoxB4x;
      private System.Windows.Forms.TextBox _textBoxB3x;
      private System.Windows.Forms.TextBox _textBoxB2x;
      private System.Windows.Forms.TextBox _textBoxB1x;
      private System.Windows.Forms.Label _labelB4x;
      private System.Windows.Forms.Label _labelB3x;
      private System.Windows.Forms.Label _labelB2x;
      private System.Windows.Forms.Label _labelB1x;
   }
}