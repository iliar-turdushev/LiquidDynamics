namespace LiquidDynamics.Views.TestProblem
{
   partial class TestProblemForm
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
         this._gcGraph = new ControlLibrary.Controls.GraphControl();
         this._gbGraph = new System.Windows.Forms.GroupBox();
         this._cbGraph = new System.Windows.Forms.ComboBox();
         this._gbSizes = new System.Windows.Forms.GroupBox();
         this._txtH = new System.Windows.Forms.TextBox();
         this._lblH = new System.Windows.Forms.Label();
         this._txtQ = new System.Windows.Forms.TextBox();
         this._lblQ = new System.Windows.Forms.Label();
         this._txtR = new System.Windows.Forms.TextBox();
         this._lblR = new System.Windows.Forms.Label();
         this._gbWind = new System.Windows.Forms.GroupBox();
         this._txtF2 = new System.Windows.Forms.TextBox();
         this._lblF2 = new System.Windows.Forms.Label();
         this._txtF1 = new System.Windows.Forms.TextBox();
         this._lblF1 = new System.Windows.Forms.Label();
         this._btnRun = new System.Windows.Forms.Button();
         this._gbGrid = new System.Windows.Forms.GroupBox();
         this._txtNz = new System.Windows.Forms.TextBox();
         this._txtNy = new System.Windows.Forms.TextBox();
         this._txtNx = new System.Windows.Forms.TextBox();
         this._lblNz = new System.Windows.Forms.Label();
         this._lblNy = new System.Windows.Forms.Label();
         this._lblNx = new System.Windows.Forms.Label();
         this._gbGraph.SuspendLayout();
         this._gbSizes.SuspendLayout();
         this._gbWind.SuspendLayout();
         this._gbGrid.SuspendLayout();
         this.SuspendLayout();
         // 
         // _gcGraph
         // 
         this._gcGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._gcGraph.BackColor = System.Drawing.SystemColors.Control;
         this._gcGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this._gcGraph.Caption = "График";
         this._gcGraph.Location = new System.Drawing.Point(12, 12);
         this._gcGraph.Name = "_gcGraph";
         this._gcGraph.Size = new System.Drawing.Size(720, 470);
         this._gcGraph.TabIndex = 0;
         this._gcGraph.XAxisName = null;
         this._gcGraph.YAxisName = null;
         // 
         // _gbGraph
         // 
         this._gbGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this._gbGraph.Controls.Add(this._cbGraph);
         this._gbGraph.Location = new System.Drawing.Point(738, 12);
         this._gbGraph.Name = "_gbGraph";
         this._gbGraph.Size = new System.Drawing.Size(292, 47);
         this._gbGraph.TabIndex = 1;
         this._gbGraph.TabStop = false;
         this._gbGraph.Text = "График";
         // 
         // _cbGraph
         // 
         this._cbGraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._cbGraph.FormattingEnabled = true;
         this._cbGraph.Location = new System.Drawing.Point(6, 19);
         this._cbGraph.Name = "_cbGraph";
         this._cbGraph.Size = new System.Drawing.Size(280, 21);
         this._cbGraph.TabIndex = 0;
         // 
         // _gbSizes
         // 
         this._gbSizes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this._gbSizes.Controls.Add(this._txtH);
         this._gbSizes.Controls.Add(this._lblH);
         this._gbSizes.Controls.Add(this._txtQ);
         this._gbSizes.Controls.Add(this._lblQ);
         this._gbSizes.Controls.Add(this._txtR);
         this._gbSizes.Controls.Add(this._lblR);
         this._gbSizes.Location = new System.Drawing.Point(738, 65);
         this._gbSizes.Name = "_gbSizes";
         this._gbSizes.Size = new System.Drawing.Size(292, 47);
         this._gbSizes.TabIndex = 2;
         this._gbSizes.TabStop = false;
         this._gbSizes.Text = "Размеры бассейна";
         // 
         // _txtH
         // 
         this._txtH.Location = new System.Drawing.Point(233, 19);
         this._txtH.Name = "_txtH";
         this._txtH.Size = new System.Drawing.Size(48, 20);
         this._txtH.TabIndex = 5;
         this._txtH.Text = "700";
         // 
         // _lblH
         // 
         this._lblH.AutoSize = true;
         this._lblH.Location = new System.Drawing.Point(195, 22);
         this._lblH.Name = "_lblH";
         this._lblH.Size = new System.Drawing.Size(32, 13);
         this._lblH.TabIndex = 4;
         this._lblH.Text = "H, м:";
         // 
         // _txtQ
         // 
         this._txtQ.Location = new System.Drawing.Point(141, 19);
         this._txtQ.Name = "_txtQ";
         this._txtQ.Size = new System.Drawing.Size(48, 20);
         this._txtQ.TabIndex = 3;
         this._txtQ.Text = "58";
         // 
         // _lblQ
         // 
         this._lblQ.AutoSize = true;
         this._lblQ.Location = new System.Drawing.Point(99, 22);
         this._lblQ.Name = "_lblQ";
         this._lblQ.Size = new System.Drawing.Size(36, 13);
         this._lblQ.TabIndex = 2;
         this._lblQ.Text = "q, км:";
         // 
         // _txtR
         // 
         this._txtR.Location = new System.Drawing.Point(45, 19);
         this._txtR.Name = "_txtR";
         this._txtR.Size = new System.Drawing.Size(48, 20);
         this._txtR.TabIndex = 1;
         this._txtR.Text = "182";
         // 
         // _lblR
         // 
         this._lblR.AutoSize = true;
         this._lblR.Location = new System.Drawing.Point(6, 22);
         this._lblR.Name = "_lblR";
         this._lblR.Size = new System.Drawing.Size(33, 13);
         this._lblR.TabIndex = 0;
         this._lblR.Text = "r, км:";
         // 
         // _gbWind
         // 
         this._gbWind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this._gbWind.Controls.Add(this._txtF2);
         this._gbWind.Controls.Add(this._lblF2);
         this._gbWind.Controls.Add(this._txtF1);
         this._gbWind.Controls.Add(this._lblF1);
         this._gbWind.Location = new System.Drawing.Point(738, 118);
         this._gbWind.Name = "_gbWind";
         this._gbWind.Size = new System.Drawing.Size(292, 47);
         this._gbWind.TabIndex = 6;
         this._gbWind.TabStop = false;
         this._gbWind.Text = "Параметры, задающие силу ветра";
         // 
         // _txtF2
         // 
         this._txtF2.Location = new System.Drawing.Point(116, 19);
         this._txtF2.Name = "_txtF2";
         this._txtF2.Size = new System.Drawing.Size(48, 20);
         this._txtF2.TabIndex = 3;
         this._txtF2.Text = "0";
         // 
         // _lblF2
         // 
         this._lblF2.AutoSize = true;
         this._lblF2.Location = new System.Drawing.Point(88, 22);
         this._lblF2.Name = "_lblF2";
         this._lblF2.Size = new System.Drawing.Size(22, 13);
         this._lblF2.TabIndex = 2;
         this._lblF2.Text = "F2:";
         // 
         // _txtF1
         // 
         this._txtF1.Location = new System.Drawing.Point(34, 19);
         this._txtF1.Name = "_txtF1";
         this._txtF1.Size = new System.Drawing.Size(48, 20);
         this._txtF1.TabIndex = 1;
         this._txtF1.Text = "10";
         // 
         // _lblF1
         // 
         this._lblF1.AutoSize = true;
         this._lblF1.Location = new System.Drawing.Point(6, 22);
         this._lblF1.Name = "_lblF1";
         this._lblF1.Size = new System.Drawing.Size(22, 13);
         this._lblF1.TabIndex = 0;
         this._lblF1.Text = "F1:";
         // 
         // _btnRun
         // 
         this._btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this._btnRun.Location = new System.Drawing.Point(955, 459);
         this._btnRun.Name = "_btnRun";
         this._btnRun.Size = new System.Drawing.Size(75, 23);
         this._btnRun.TabIndex = 7;
         this._btnRun.Text = "Построить";
         this._btnRun.UseVisualStyleBackColor = true;
         this._btnRun.Click += new System.EventHandler(this.btnRun_Click);
         // 
         // _gbGrid
         // 
         this._gbGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this._gbGrid.Controls.Add(this._txtNz);
         this._gbGrid.Controls.Add(this._txtNy);
         this._gbGrid.Controls.Add(this._txtNx);
         this._gbGrid.Controls.Add(this._lblNz);
         this._gbGrid.Controls.Add(this._lblNy);
         this._gbGrid.Controls.Add(this._lblNx);
         this._gbGrid.Location = new System.Drawing.Point(738, 171);
         this._gbGrid.Name = "_gbGrid";
         this._gbGrid.Size = new System.Drawing.Size(292, 47);
         this._gbGrid.TabIndex = 8;
         this._gbGrid.TabStop = false;
         this._gbGrid.Text = "Узлы сетки";
         // 
         // _txtNz
         // 
         this._txtNz.Location = new System.Drawing.Point(198, 19);
         this._txtNz.Name = "_txtNz";
         this._txtNz.Size = new System.Drawing.Size(48, 20);
         this._txtNz.TabIndex = 5;
         this._txtNz.Text = "50";
         // 
         // _txtNy
         // 
         this._txtNy.Location = new System.Drawing.Point(117, 19);
         this._txtNy.Name = "_txtNy";
         this._txtNy.Size = new System.Drawing.Size(48, 20);
         this._txtNy.TabIndex = 4;
         this._txtNy.Text = "50";
         // 
         // _txtNx
         // 
         this._txtNx.Location = new System.Drawing.Point(34, 19);
         this._txtNx.Name = "_txtNx";
         this._txtNx.Size = new System.Drawing.Size(48, 20);
         this._txtNx.TabIndex = 3;
         this._txtNx.Text = "50";
         // 
         // _lblNz
         // 
         this._lblNz.AutoSize = true;
         this._lblNz.Location = new System.Drawing.Point(171, 22);
         this._lblNz.Name = "_lblNz";
         this._lblNz.Size = new System.Drawing.Size(23, 13);
         this._lblNz.TabIndex = 2;
         this._lblNz.Text = "Nz:";
         // 
         // _lblNy
         // 
         this._lblNy.AutoSize = true;
         this._lblNy.Location = new System.Drawing.Point(88, 22);
         this._lblNy.Name = "_lblNy";
         this._lblNy.Size = new System.Drawing.Size(23, 13);
         this._lblNy.TabIndex = 1;
         this._lblNy.Text = "Ny:";
         // 
         // _lblNx
         // 
         this._lblNx.AutoSize = true;
         this._lblNx.Location = new System.Drawing.Point(6, 22);
         this._lblNx.Name = "_lblNx";
         this._lblNx.Size = new System.Drawing.Size(23, 13);
         this._lblNx.TabIndex = 0;
         this._lblNx.Text = "Nx:";
         // 
         // TestProblemForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1042, 494);
         this.Controls.Add(this._gbGrid);
         this.Controls.Add(this._btnRun);
         this.Controls.Add(this._gbWind);
         this.Controls.Add(this._gbSizes);
         this.Controls.Add(this._gbGraph);
         this.Controls.Add(this._gcGraph);
         this.Name = "TestProblemForm";
         this.Text = "Тестовая задача";
         this._gbGraph.ResumeLayout(false);
         this._gbSizes.ResumeLayout(false);
         this._gbSizes.PerformLayout();
         this._gbWind.ResumeLayout(false);
         this._gbWind.PerformLayout();
         this._gbGrid.ResumeLayout(false);
         this._gbGrid.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private ControlLibrary.Controls.GraphControl _gcGraph;
      private System.Windows.Forms.GroupBox _gbGraph;
      private System.Windows.Forms.ComboBox _cbGraph;
      private System.Windows.Forms.GroupBox _gbSizes;
      private System.Windows.Forms.TextBox _txtH;
      private System.Windows.Forms.Label _lblH;
      private System.Windows.Forms.TextBox _txtQ;
      private System.Windows.Forms.Label _lblQ;
      private System.Windows.Forms.TextBox _txtR;
      private System.Windows.Forms.Label _lblR;
      private System.Windows.Forms.GroupBox _gbWind;
      private System.Windows.Forms.TextBox _txtF2;
      private System.Windows.Forms.Label _lblF2;
      private System.Windows.Forms.TextBox _txtF1;
      private System.Windows.Forms.Label _lblF1;
      private System.Windows.Forms.Button _btnRun;
      private System.Windows.Forms.GroupBox _gbGrid;
      private System.Windows.Forms.TextBox _txtNz;
      private System.Windows.Forms.TextBox _txtNy;
      private System.Windows.Forms.TextBox _txtNx;
      private System.Windows.Forms.Label _lblNz;
      private System.Windows.Forms.Label _lblNy;
      private System.Windows.Forms.Label _lblNx;
   }
}