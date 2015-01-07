using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Point = System.Windows.Point;
using Size = System.Drawing.Size;

namespace LiquidDynamics.Forms.Kriging
{
   internal partial class KrigingForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private IContainer components = null;

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
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._buttonInterpolate = new System.Windows.Forms.ToolStripButton();
         this._labelN = new System.Windows.Forms.ToolStripLabel();
         this._textBoxN = new System.Windows.Forms.ToolStripTextBox();
         this._labelM = new System.Windows.Forms.ToolStripLabel();
         this._textBoxM = new System.Windows.Forms.ToolStripTextBox();
         this._labelNodes = new System.Windows.Forms.ToolStripLabel();
         this._textBoxNodes = new System.Windows.Forms.ToolStripTextBox();
         this._labelVariogram = new System.Windows.Forms.ToolStripLabel();
         this._comboBoxVariogram = new System.Windows.Forms.ToolStripComboBox();
         this._labelC = new System.Windows.Forms.ToolStripLabel();
         this._textBoxC = new System.Windows.Forms.ToolStripTextBox();
         this._labelC0 = new System.Windows.Forms.ToolStripLabel();
         this._textBoxC0 = new System.Windows.Forms.ToolStripTextBox();
         this._labelS = new System.Windows.Forms.ToolStripLabel();
         this._textBoxS = new System.Windows.Forms.ToolStripTextBox();
         this._labelA = new System.Windows.Forms.ToolStripLabel();
         this._textBoxA = new System.Windows.Forms.ToolStripTextBox();
         this._statusStrip = new System.Windows.Forms.StatusStrip();
         this._labelError = new System.Windows.Forms.ToolStripStatusLabel();
         this._labelResult = new System.Windows.Forms.ToolStripStatusLabel();
         this._toolStrip.SuspendLayout();
         this._statusStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _panel
         // 
         this._panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._panel.Location = new System.Drawing.Point(12, 28);
         this._panel.Name = "_panel";
         this._panel.Size = new System.Drawing.Size(789, 396);
         this._panel.TabIndex = 0;
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonInterpolate,
            this._labelN,
            this._textBoxN,
            this._labelM,
            this._textBoxM,
            this._labelNodes,
            this._textBoxNodes,
            this._labelVariogram,
            this._comboBoxVariogram,
            this._labelC,
            this._textBoxC,
            this._labelC0,
            this._textBoxC0,
            this._labelS,
            this._textBoxS,
            this._labelA,
            this._textBoxA});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(813, 25);
         this._toolStrip.TabIndex = 1;
         // 
         // _buttonInterpolate
         // 
         this._buttonInterpolate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._buttonInterpolate.Image = global::LiquidDynamics.Properties.Resources.Start;
         this._buttonInterpolate.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._buttonInterpolate.Name = "_buttonInterpolate";
         this._buttonInterpolate.Size = new System.Drawing.Size(23, 22);
         this._buttonInterpolate.Text = "toolStripButton1";
         this._buttonInterpolate.Click += new System.EventHandler(this.buttonInterpolateClick);
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
         this._textBoxN.Text = "20";
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
         this._textBoxM.Text = "20";
         // 
         // _labelNodes
         // 
         this._labelNodes.Name = "_labelNodes";
         this._labelNodes.Size = new System.Drawing.Size(44, 22);
         this._labelNodes.Text = "Nodes:";
         // 
         // _textBoxNodes
         // 
         this._textBoxNodes.Name = "_textBoxNodes";
         this._textBoxNodes.Size = new System.Drawing.Size(50, 25);
         this._textBoxNodes.Text = "4";
         // 
         // _labelVariogram
         // 
         this._labelVariogram.Name = "_labelVariogram";
         this._labelVariogram.Size = new System.Drawing.Size(65, 22);
         this._labelVariogram.Text = "Variogram:";
         // 
         // _comboBoxVariogram
         // 
         this._comboBoxVariogram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._comboBoxVariogram.Items.AddRange(new object[] {
            "Gamma1",
            "Gamma2",
            "Gamma3",
            "Gamma4",
            "Gamma5"});
         this._comboBoxVariogram.Name = "_comboBoxVariogram";
         this._comboBoxVariogram.Size = new System.Drawing.Size(75, 25);
         this._comboBoxVariogram.SelectedIndexChanged += new System.EventHandler(this.comboBoxVariogramSelectedIndexChanged);
         // 
         // _labelC
         // 
         this._labelC.Name = "_labelC";
         this._labelC.Size = new System.Drawing.Size(18, 22);
         this._labelC.Text = "C:";
         // 
         // _textBoxC
         // 
         this._textBoxC.Name = "_textBoxC";
         this._textBoxC.Size = new System.Drawing.Size(50, 25);
         this._textBoxC.Text = "1";
         // 
         // _labelC0
         // 
         this._labelC0.Name = "_labelC0";
         this._labelC0.Size = new System.Drawing.Size(24, 22);
         this._labelC0.Text = "C0:";
         // 
         // _textBoxC0
         // 
         this._textBoxC0.Name = "_textBoxC0";
         this._textBoxC0.Size = new System.Drawing.Size(50, 25);
         this._textBoxC0.Text = "0";
         // 
         // _labelS
         // 
         this._labelS.Name = "_labelS";
         this._labelS.Size = new System.Drawing.Size(16, 22);
         this._labelS.Text = "S:";
         // 
         // _textBoxS
         // 
         this._textBoxS.Name = "_textBoxS";
         this._textBoxS.Size = new System.Drawing.Size(50, 25);
         this._textBoxS.Text = "1";
         // 
         // _labelA
         // 
         this._labelA.Name = "_labelA";
         this._labelA.Size = new System.Drawing.Size(18, 22);
         this._labelA.Text = "A:";
         // 
         // _textBoxA
         // 
         this._textBoxA.Name = "_textBoxA";
         this._textBoxA.Size = new System.Drawing.Size(50, 25);
         this._textBoxA.Text = "1";
         // 
         // _statusStrip
         // 
         this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._labelError,
            this._labelResult});
         this._statusStrip.Location = new System.Drawing.Point(0, 427);
         this._statusStrip.Name = "_statusStrip";
         this._statusStrip.Size = new System.Drawing.Size(813, 22);
         this._statusStrip.TabIndex = 2;
         // 
         // _labelError
         // 
         this._labelError.Name = "_labelError";
         this._labelError.Size = new System.Drawing.Size(86, 17);
         this._labelError.Text = "Погрешность:";
         // 
         // _labelResult
         // 
         this._labelResult.Name = "_labelResult";
         this._labelResult.Size = new System.Drawing.Size(0, 17);
         this._labelResult.Click += new System.EventHandler(this.labelResultClick);
         // 
         // KrigingForm
         // 
         this.BackColor = System.Drawing.Color.White;
         this.ClientSize = new System.Drawing.Size(813, 449);
         this.Controls.Add(this._statusStrip);
         this.Controls.Add(this._toolStrip);
         this.Controls.Add(this._panel);
         this.Name = "KrigingForm";
         this.Text = "Кригинг";
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this._statusStrip.ResumeLayout(false);
         this._statusStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Panel _panel;
      private ToolStrip _toolStrip;
      private ToolStripLabel _labelN;
      private ToolStripTextBox _textBoxN;
      private ToolStripLabel _labelM;
      private ToolStripTextBox _textBoxM;
      private ToolStripLabel _labelNodes;
      private ToolStripTextBox _textBoxNodes;
      private ToolStripButton _buttonInterpolate;
      private ToolStripLabel _labelVariogram;
      private ToolStripComboBox _comboBoxVariogram;
      private ToolStripLabel _labelC;
      private ToolStripTextBox _textBoxC;
      private ToolStripLabel _labelC0;
      private ToolStripTextBox _textBoxC0;
      private ToolStripLabel _labelS;
      private ToolStripTextBox _textBoxS;
      private ToolStripLabel _labelA;
      private ToolStripTextBox _textBoxA;
      private StatusStrip _statusStrip;
      private ToolStripStatusLabel _labelResult;
      private ToolStripStatusLabel _labelError;
   }
}