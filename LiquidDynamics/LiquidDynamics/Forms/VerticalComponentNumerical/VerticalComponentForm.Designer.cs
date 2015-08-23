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
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._graphControl = new ControlLibrary.Controls.GraphControl();
         this._buttonReset = new System.Windows.Forms.ToolStripButton();
         this._toolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _toolStrip
         // 
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonReset});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(665, 25);
         this._toolStrip.TabIndex = 0;
         // 
         // _graphControl
         // 
         this._graphControl.Caption = "Вертикальная компонента";
         this._graphControl.Location = new System.Drawing.Point(12, 28);
         this._graphControl.Name = "_graphControl";
         this._graphControl.Size = new System.Drawing.Size(641, 423);
         this._graphControl.TabIndex = 1;
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
         // VerticalComponentForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(665, 463);
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

   }
}