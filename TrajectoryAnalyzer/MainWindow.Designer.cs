namespace TrajectoryAnalyzer
{
    partial class MainWindow
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.firstTrajectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.distributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autocorrelationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondTrajectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.secondDistributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondAutocorrelationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondFftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.correlationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOnSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.secondChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.firstChart)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secondChart)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firstTrajectoryToolStripMenuItem,
            this.secondTrajectoryToolStripMenuItem,
            this.correlationToolStripMenuItem,
            this.fftToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1685, 28);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip";
            // 
            // firstTrajectoryToolStripMenuItem
            // 
            this.firstTrajectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.secondOpenToolStripMenuItem,
            this.toolStripSeparator,
            this.distributionToolStripMenuItem,
            this.autocorrelationToolStripMenuItem,
            this.fftToolStripMenuItem,
            this.toolStripSeparator,
            this.closeToolStripMenuItem});
            this.firstTrajectoryToolStripMenuItem.Name = "firstTrajectoryToolStripMenuItem";
            this.firstTrajectoryToolStripMenuItem.Size = new System.Drawing.Size(109, 24);
            this.firstTrajectoryToolStripMenuItem.Text = "1st trajectory";
            // 
            // openToolStripMenuItem1
            // 
            this.secondOpenToolStripMenuItem.Name = "openToolStripMenuItem1";
            this.secondOpenToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.secondOpenToolStripMenuItem.Text = "Open";
            this.secondOpenToolStripMenuItem.Click += new System.EventHandler(this.FirstOpenToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator.Name = "toolStripSeparator1";
            this.toolStripSeparator.Size = new System.Drawing.Size(193, 6);
            // 
            // distributionToolStripMenuItem1
            // 
            this.distributionToolStripMenuItem.Enabled = false;
            this.distributionToolStripMenuItem.Name = "distributionToolStripMenuItem1";
            this.distributionToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.distributionToolStripMenuItem.Text = "Distribution";
            this.distributionToolStripMenuItem.Click += new System.EventHandler(this.FirstDistributionToolStripMenuItem_Click);
            // 
            // autocorrelationToolStripMenuItem1
            // 
            this.autocorrelationToolStripMenuItem.Enabled = false;
            this.autocorrelationToolStripMenuItem.Name = "autocorrelationToolStripMenuItem1";
            this.autocorrelationToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.autocorrelationToolStripMenuItem.Text = "Autocorrelation";
            this.autocorrelationToolStripMenuItem.Click += new System.EventHandler(this.FirstAurocorrelationToolStripMenuItem_Click);
            // 
            // fftToolStripMenuItem1
            // 
            this.fftToolStripMenuItem.Enabled = false;
            this.fftToolStripMenuItem.Name = "fftToolStripMenuItem1";
            this.fftToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.fftToolStripMenuItem.Text = "FFT";
            this.fftToolStripMenuItem.Click += new System.EventHandler(this.FFTToolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator.Name = "toolStripSeparator2";
            this.toolStripSeparator.Size = new System.Drawing.Size(193, 6);
            // 
            // closeToolStripMenuItem1
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem1";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // secondTrajectoryToolStripMenuItem
            // 
            this.secondTrajectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.secondOpenToolStripMenuItem,
            this.toolStripSeparator3,
            this.secondDistributionToolStripMenuItem,
            this.secondAutocorrelationToolStripMenuItem,
            this.secondFftToolStripMenuItem,
            this.toolStripSeparator4,
            this.closeToolStripMenuItem2});
            this.secondTrajectoryToolStripMenuItem.Enabled = false;
            this.secondTrajectoryToolStripMenuItem.Name = "secondTrajectoryToolStripMenuItem";
            this.secondTrajectoryToolStripMenuItem.Size = new System.Drawing.Size(115, 24);
            this.secondTrajectoryToolStripMenuItem.Text = "2nd trajectory";
            // 
            // openToolStripMenuItem2
            // 
            this.secondOpenToolStripMenuItem.Name = "openToolStripMenuItem2";
            this.secondOpenToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.secondOpenToolStripMenuItem.Text = "Open";
            this.secondOpenToolStripMenuItem.Click += new System.EventHandler(this.SecondOpenToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(193, 6);
            // 
            // distributionToolStripMenuItem2
            // 
            this.secondDistributionToolStripMenuItem.Enabled = false;
            this.secondDistributionToolStripMenuItem.Name = "distributionToolStripMenuItem2";
            this.secondDistributionToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.secondDistributionToolStripMenuItem.Text = "Distribution";
            this.secondDistributionToolStripMenuItem.Click += new System.EventHandler(this.SecondDistributionToolStripMenuItem_Click);
            // 
            // autocorrelationToolStripMenuItem2
            // 
            this.secondAutocorrelationToolStripMenuItem.Enabled = false;
            this.secondAutocorrelationToolStripMenuItem.Name = "autocorrelationToolStripMenuItem2";
            this.secondAutocorrelationToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.secondAutocorrelationToolStripMenuItem.Text = "Autocorrelation";
            this.secondAutocorrelationToolStripMenuItem.Click += new System.EventHandler(this.SecondAutocorrelationToolStripMenuItem_Click);
            // 
            // fftToolStripMenuItem2
            // 
            this.secondFftToolStripMenuItem.Enabled = false;
            this.secondFftToolStripMenuItem.Name = "fftToolStripMenuItem2";
            this.secondFftToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.secondFftToolStripMenuItem.Text = "FFT";
            this.secondFftToolStripMenuItem.Click += new System.EventHandler(this.FFTToolStripMenuItem2_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(193, 6);
            // 
            // closeToolStripMenuItem2
            // 
            this.closeToolStripMenuItem2.Enabled = false;
            this.closeToolStripMenuItem2.Name = "closeToolStripMenuItem2";
            this.closeToolStripMenuItem2.Size = new System.Drawing.Size(196, 26);
            this.closeToolStripMenuItem2.Text = "Close";
            // 
            // correlationToolStripMenuItem
            // 
            this.correlationToolStripMenuItem.Enabled = false;
            this.correlationToolStripMenuItem.Name = "correlationToolStripMenuItem";
            this.correlationToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.correlationToolStripMenuItem.Text = "Correlation";
            this.correlationToolStripMenuItem.Click += new System.EventHandler(this.CorrelationToolStripMenuItem_Click);
            // 
            // fftToolStripMenuItem
            // 
            this.fftToolStripMenuItem.Enabled = false;
            this.fftToolStripMenuItem.Name = "fftToolStripMenuItem";
            this.fftToolStripMenuItem.Size = new System.Drawing.Size(45, 24);
            this.fftToolStripMenuItem.Text = "FFT";
            this.fftToolStripMenuItem.Click += new System.EventHandler(this.FFTToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomOnSelectToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // zoomOnSelectToolStripMenuItem
            // 
            this.zoomOnSelectToolStripMenuItem.CheckOnClick = true;
            this.zoomOnSelectToolStripMenuItem.Name = "zoomOnSelectToolStripMenuItem";
            this.zoomOnSelectToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
            this.zoomOnSelectToolStripMenuItem.Text = "Zoom on select";
            this.zoomOnSelectToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ZoomOnSelectToolStripMenuItem_CheckedChanged);
            // 
            // mainChart
            // 
            this.firstChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            this.firstChart.ChartAreas.Add(chartArea1);
            this.firstChart.Cursor = System.Windows.Forms.Cursors.Arrow;
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Name = "Legend1";
            this.firstChart.Legends.Add(legend1);
            this.firstChart.Location = new System.Drawing.Point(3, 2);
            this.firstChart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.firstChart.Name = "mainChart";
            this.firstChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Black;
            series2.IsVisibleInLegend = false;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Red;
            series3.IsVisibleInLegend = false;
            series3.Legend = "Legend1";
            series3.Name = "Series3";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.Black;
            series4.IsVisibleInLegend = false;
            series4.Legend = "Legend1";
            series4.Name = "Series4";
            this.firstChart.Series.Add(series1);
            this.firstChart.Series.Add(series2);
            this.firstChart.Series.Add(series3);
            this.firstChart.Series.Add(series4);
            this.firstChart.Size = new System.Drawing.Size(1679, 848);
            this.firstChart.TabIndex = 3;
            this.firstChart.Text = "Main Chart";
            this.firstChart.SelectionRangeChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.Chart1_SelectionRangeChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.secondChart, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.firstChart, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel.Name = "tableLayoutPanel1";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1685, 852);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // chart2
            // 
            this.secondChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.CursorX.IsUserSelectionEnabled = true;
            chartArea2.Name = "ChartArea1";
            this.secondChart.ChartAreas.Add(chartArea2);
            this.secondChart.Cursor = System.Windows.Forms.Cursors.Arrow;
            legend2.DockedToChartArea = "ChartArea1";
            legend2.Name = "Legend1";
            this.secondChart.Legends.Add(legend2);
            this.secondChart.Location = new System.Drawing.Point(3, 854);
            this.secondChart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.secondChart.Name = "chart2";
            this.secondChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.Black;
            series6.IsVisibleInLegend = false;
            series6.Legend = "Legend1";
            series6.Name = "Series2";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.Red;
            series7.IsVisibleInLegend = false;
            series7.Legend = "Legend1";
            series7.Name = "Series3";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Color = System.Drawing.Color.Black;
            series8.IsVisibleInLegend = false;
            series8.Legend = "Legend1";
            series8.Name = "Series4";
            this.secondChart.Series.Add(series5);
            this.secondChart.Series.Add(series6);
            this.secondChart.Series.Add(series7);
            this.secondChart.Series.Add(series8);
            this.secondChart.Size = new System.Drawing.Size(1679, 1);
            this.secondChart.TabIndex = 4;
            this.secondChart.Text = "chart2";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1685, 880);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.firstChart)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.secondChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem firstTrajectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem distributionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autocorrelationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondTrajectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem secondDistributionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondAutocorrelationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondFftToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem correlationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOnSelectToolStripMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart firstChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart secondChart;
    }
}

