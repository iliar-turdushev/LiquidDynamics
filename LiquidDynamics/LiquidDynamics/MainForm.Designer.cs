namespace LiquidDynamics
{
   public partial class MainForm
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
         this._menuStrip = new System.Windows.Forms.MenuStrip();
         this._miTestProblem = new System.Windows.Forms.ToolStripMenuItem();
         this._problemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._problemParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._testProblemMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._graphParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._analyticalSolutionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._barotropicComponentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._ekmanSpiralMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._upwellingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._velocityFieldMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._uvMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._uwMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._vwMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._barotropicComponentSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._stommelModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._krigingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._krigingTestProblemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._issykKulInterpolationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.issykKulGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._barotropicComponentTestProblemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.issykKulVelocityFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.issykKulWindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.baroclinicComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.testProblemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.issykKulToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.verticalComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.streamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._menuStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _menuStrip
         // 
         this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miTestProblem,
            this._problemToolStripMenuItem,
            this._testProblemMenuItem,
            this._barotropicComponentSolutionToolStripMenuItem,
            this.issykKulVelocityFieldToolStripMenuItem,
            this.issykKulWindToolStripMenuItem,
            this.baroclinicComponentToolStripMenuItem,
            this.testProblemToolStripMenuItem,
            this.issykKulToolStripMenuItem,
            this.verticalComponentToolStripMenuItem,
            this.streamToolStripMenuItem});
         this._menuStrip.Location = new System.Drawing.Point(0, 0);
         this._menuStrip.Name = "_menuStrip";
         this._menuStrip.Size = new System.Drawing.Size(1370, 24);
         this._menuStrip.TabIndex = 0;
         this._menuStrip.Text = "MenuStrip";
         // 
         // _miTestProblem
         // 
         this._miTestProblem.Name = "_miTestProblem";
         this._miTestProblem.Size = new System.Drawing.Size(107, 20);
         this._miTestProblem.Text = "Тестовая задача";
         this._miTestProblem.Click += new System.EventHandler(this.miTestProblem_Click);
         // 
         // _problemToolStripMenuItem
         // 
         this._problemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._problemParametersToolStripMenuItem});
         this._problemToolStripMenuItem.Name = "_problemToolStripMenuItem";
         this._problemToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
         this._problemToolStripMenuItem.Text = "Задача";
         // 
         // _problemParametersToolStripMenuItem
         // 
         this._problemParametersToolStripMenuItem.Name = "_problemParametersToolStripMenuItem";
         this._problemParametersToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
         this._problemParametersToolStripMenuItem.Text = "Параметры";
         this._problemParametersToolStripMenuItem.Click += new System.EventHandler(this.problemParametersToolStripMenuItemClick);
         // 
         // _testProblemMenuItem
         // 
         this._testProblemMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._graphParametersToolStripMenuItem,
            this._analyticalSolutionsMenuItem});
         this._testProblemMenuItem.Name = "_testProblemMenuItem";
         this._testProblemMenuItem.Size = new System.Drawing.Size(107, 20);
         this._testProblemMenuItem.Text = "Тестовая задача";
         // 
         // _graphParametersToolStripMenuItem
         // 
         this._graphParametersToolStripMenuItem.Name = "_graphParametersToolStripMenuItem";
         this._graphParametersToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
         this._graphParametersToolStripMenuItem.Text = "Параметры";
         this._graphParametersToolStripMenuItem.Click += new System.EventHandler(this.graphParametersToolStripMenuItemClick);
         // 
         // _analyticalSolutionsMenuItem
         // 
         this._analyticalSolutionsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._barotropicComponentMenuItem,
            this._ekmanSpiralMenuItem,
            this._upwellingMenuItem,
            this._velocityFieldMenuItem});
         this._analyticalSolutionsMenuItem.Name = "_analyticalSolutionsMenuItem";
         this._analyticalSolutionsMenuItem.Size = new System.Drawing.Size(206, 22);
         this._analyticalSolutionsMenuItem.Text = "Аналитичекие решения";
         // 
         // _barotropicComponentMenuItem
         // 
         this._barotropicComponentMenuItem.Name = "_barotropicComponentMenuItem";
         this._barotropicComponentMenuItem.Size = new System.Drawing.Size(216, 22);
         this._barotropicComponentMenuItem.Text = "Баротропная компонента";
         this._barotropicComponentMenuItem.Click += new System.EventHandler(this.barotropicComponentMenuItemClick);
         // 
         // _ekmanSpiralMenuItem
         // 
         this._ekmanSpiralMenuItem.Name = "_ekmanSpiralMenuItem";
         this._ekmanSpiralMenuItem.Size = new System.Drawing.Size(216, 22);
         this._ekmanSpiralMenuItem.Text = "Спираль Экмана";
         this._ekmanSpiralMenuItem.Click += new System.EventHandler(this.ekmanSpiralMenuItemClick);
         // 
         // _upwellingMenuItem
         // 
         this._upwellingMenuItem.Name = "_upwellingMenuItem";
         this._upwellingMenuItem.Size = new System.Drawing.Size(216, 22);
         this._upwellingMenuItem.Text = "Апвеллинг";
         this._upwellingMenuItem.Click += new System.EventHandler(this.upwellingMenuItemClick);
         // 
         // _velocityFieldMenuItem
         // 
         this._velocityFieldMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._uvMenuItem,
            this._uwMenuItem,
            this._vwMenuItem});
         this._velocityFieldMenuItem.Name = "_velocityFieldMenuItem";
         this._velocityFieldMenuItem.Size = new System.Drawing.Size(216, 22);
         this._velocityFieldMenuItem.Text = "Поле скоростей";
         // 
         // _uvMenuItem
         // 
         this._uvMenuItem.Name = "_uvMenuItem";
         this._uvMenuItem.Size = new System.Drawing.Size(104, 22);
         this._uvMenuItem.Text = "(u, v)";
         this._uvMenuItem.Click += new System.EventHandler(this.uvMenuItemClick);
         // 
         // _uwMenuItem
         // 
         this._uwMenuItem.Name = "_uwMenuItem";
         this._uwMenuItem.Size = new System.Drawing.Size(104, 22);
         this._uwMenuItem.Text = "(u, w)";
         this._uwMenuItem.Click += new System.EventHandler(this.uwMenuItemClick);
         // 
         // _vwMenuItem
         // 
         this._vwMenuItem.Name = "_vwMenuItem";
         this._vwMenuItem.Size = new System.Drawing.Size(104, 22);
         this._vwMenuItem.Text = "(v, w)";
         this._vwMenuItem.Click += new System.EventHandler(this.vwMenuItemClick);
         // 
         // _barotropicComponentSolutionToolStripMenuItem
         // 
         this._barotropicComponentSolutionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._stommelModelToolStripMenuItem,
            this._krigingToolStripMenuItem,
            this.issykKulGridToolStripMenuItem,
            this._barotropicComponentTestProblemToolStripMenuItem});
         this._barotropicComponentSolutionToolStripMenuItem.Name = "_barotropicComponentSolutionToolStripMenuItem";
         this._barotropicComponentSolutionToolStripMenuItem.Size = new System.Drawing.Size(161, 20);
         this._barotropicComponentSolutionToolStripMenuItem.Text = "Баротропная компонента";
         // 
         // _stommelModelToolStripMenuItem
         // 
         this._stommelModelToolStripMenuItem.Name = "_stommelModelToolStripMenuItem";
         this._stommelModelToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
         this._stommelModelToolStripMenuItem.Text = "Модель Стоммела";
         this._stommelModelToolStripMenuItem.Click += new System.EventHandler(this.stommelModelToolStripMenuItemClick);
         // 
         // _krigingToolStripMenuItem
         // 
         this._krigingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._krigingTestProblemToolStripMenuItem,
            this._issykKulInterpolationToolStripMenuItem});
         this._krigingToolStripMenuItem.Name = "_krigingToolStripMenuItem";
         this._krigingToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
         this._krigingToolStripMenuItem.Text = "Кригинг";
         // 
         // _krigingTestProblemToolStripMenuItem
         // 
         this._krigingTestProblemToolStripMenuItem.Name = "_krigingTestProblemToolStripMenuItem";
         this._krigingTestProblemToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
         this._krigingTestProblemToolStripMenuItem.Text = "Тестовая задача";
         this._krigingTestProblemToolStripMenuItem.Click += new System.EventHandler(this.krigingTestProblemToolStripMenuItemClick);
         // 
         // _issykKulInterpolationToolStripMenuItem
         // 
         this._issykKulInterpolationToolStripMenuItem.Name = "_issykKulInterpolationToolStripMenuItem";
         this._issykKulInterpolationToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
         this._issykKulInterpolationToolStripMenuItem.Text = "Восстановление дна оз. Иссык-Куль";
         this._issykKulInterpolationToolStripMenuItem.Click += new System.EventHandler(this.issykKulInterpolationToolStripMenuItemClick);
         // 
         // issykKulGridToolStripMenuItem
         // 
         this.issykKulGridToolStripMenuItem.Name = "issykKulGridToolStripMenuItem";
         this.issykKulGridToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
         this.issykKulGridToolStripMenuItem.Text = "Построение сеточной области оз. Иссык-Куль";
         this.issykKulGridToolStripMenuItem.Click += new System.EventHandler(this.issykKulGridToolStripMenuItemClick);
         // 
         // _barotropicComponentTestProblemToolStripMenuItem
         // 
         this._barotropicComponentTestProblemToolStripMenuItem.Name = "_barotropicComponentTestProblemToolStripMenuItem";
         this._barotropicComponentTestProblemToolStripMenuItem.Size = new System.Drawing.Size(332, 22);
         this._barotropicComponentTestProblemToolStripMenuItem.Text = "Тестовая задача";
         this._barotropicComponentTestProblemToolStripMenuItem.Click += new System.EventHandler(this.barotropicComponentTestProblemToolStripMenuItemClick);
         // 
         // issykKulVelocityFieldToolStripMenuItem
         // 
         this.issykKulVelocityFieldToolStripMenuItem.Name = "issykKulVelocityFieldToolStripMenuItem";
         this.issykKulVelocityFieldToolStripMenuItem.Size = new System.Drawing.Size(234, 20);
         this.issykKulVelocityFieldToolStripMenuItem.Text = "Поле скоростей для озера Иссык-Куль";
         this.issykKulVelocityFieldToolStripMenuItem.Click += new System.EventHandler(this.issykKulVelocityFieldToolStripMenuItemClick);
         // 
         // issykKulWindToolStripMenuItem
         // 
         this.issykKulWindToolStripMenuItem.Name = "issykKulWindToolStripMenuItem";
         this.issykKulWindToolStripMenuItem.Size = new System.Drawing.Size(215, 20);
         this.issykKulWindToolStripMenuItem.Text = "Моделирование ветрового режима";
         this.issykKulWindToolStripMenuItem.Click += new System.EventHandler(this.issykKulWindToolStripMenuItemClick);
         // 
         // baroclinicComponentToolStripMenuItem
         // 
         this.baroclinicComponentToolStripMenuItem.Name = "baroclinicComponentToolStripMenuItem";
         this.baroclinicComponentToolStripMenuItem.Size = new System.Drawing.Size(162, 20);
         this.baroclinicComponentToolStripMenuItem.Text = "Бароклинная компонента";
         this.baroclinicComponentToolStripMenuItem.Click += new System.EventHandler(this.baroclinicComponentToolStripMenuItemClick);
         // 
         // testProblemToolStripMenuItem
         // 
         this.testProblemToolStripMenuItem.Name = "testProblemToolStripMenuItem";
         this.testProblemToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
         this.testProblemToolStripMenuItem.Text = "Тестовая задача";
         this.testProblemToolStripMenuItem.Click += new System.EventHandler(this.testProblemToolStripMenuItemClick);
         // 
         // issykKulToolStripMenuItem
         // 
         this.issykKulToolStripMenuItem.Name = "issykKulToolStripMenuItem";
         this.issykKulToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
         this.issykKulToolStripMenuItem.Text = "Иссык-Куль";
         this.issykKulToolStripMenuItem.Click += new System.EventHandler(this.issykKulToolStripMenuItemClick);
         // 
         // verticalComponentToolStripMenuItem
         // 
         this.verticalComponentToolStripMenuItem.Name = "verticalComponentToolStripMenuItem";
         this.verticalComponentToolStripMenuItem.Size = new System.Drawing.Size(165, 20);
         this.verticalComponentToolStripMenuItem.Text = "Вертикальная компонента";
         this.verticalComponentToolStripMenuItem.Click += new System.EventHandler(this.verticalComponentToolStripMenuItemClick);
         // 
         // streamToolStripMenuItem
         // 
         this.streamToolStripMenuItem.Name = "streamToolStripMenuItem";
         this.streamToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
         this.streamToolStripMenuItem.Text = "Поток";
         this.streamToolStripMenuItem.Click += new System.EventHandler(this.streamToolStripMenuItem_Click);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.ClientSize = new System.Drawing.Size(1370, 453);
         this.Controls.Add(this._menuStrip);
         this.IsMdiContainer = true;
         this.MainMenuStrip = this._menuStrip;
         this.Name = "MainForm";
         this.Text = "Гидродинамика";
         this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
         this._menuStrip.ResumeLayout(false);
         this._menuStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion


      private System.Windows.Forms.MenuStrip _menuStrip;
      private System.Windows.Forms.ToolStripMenuItem _problemToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _testProblemMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _problemParametersToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _graphParametersToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _barotropicComponentSolutionToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _stommelModelToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _krigingToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _issykKulInterpolationToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _krigingTestProblemToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem issykKulGridToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _barotropicComponentTestProblemToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem issykKulVelocityFieldToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem issykKulWindToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem baroclinicComponentToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem testProblemToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem issykKulToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem verticalComponentToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem streamToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _analyticalSolutionsMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _barotropicComponentMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _ekmanSpiralMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _upwellingMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _velocityFieldMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _uvMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _uwMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _vwMenuItem;
      private System.Windows.Forms.ToolStripMenuItem _miTestProblem;
   }
}



