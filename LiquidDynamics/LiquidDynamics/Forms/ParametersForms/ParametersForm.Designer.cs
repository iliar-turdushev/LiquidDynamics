﻿namespace LiquidDynamics.Forms.ParametersForms
{
   internal partial class ParametersForm
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
         this._elementHost = new System.Windows.Forms.Integration.ElementHost();
         this.SuspendLayout();
         // 
         // _elementHost
         // 
         this._elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
         this._elementHost.Location = new System.Drawing.Point(0, 0);
         this._elementHost.Name = "_elementHost";
         this._elementHost.Size = new System.Drawing.Size(617, 469);
         this._elementHost.TabIndex = 0;
         this._elementHost.Child = null;
         // 
         // ParametersForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(617, 469);
         this.Controls.Add(this._elementHost);
         this.Name = "ParametersForm";
         this.Text = "Параметры";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Integration.ElementHost _elementHost;
   }
}