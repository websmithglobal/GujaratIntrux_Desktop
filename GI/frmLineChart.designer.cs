namespace GI
{
    partial class frmLineChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.crtUnitTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.Chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.crtUnitTime)).BeginInit();
            this.SuspendLayout();
            // 
            // Chart1
            // 
            this.Chart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            chartArea1.Name = "ChartArea1";
            this.Chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.Chart1.Legends.Add(legend1);
            this.Chart1.Location = new System.Drawing.Point(0, 0);
            this.Chart1.Name = "Chart1";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.Legend = "Legend1";
            series1.MarkerBorderColor = System.Drawing.Color.Red;
            series1.MarkerColor = System.Drawing.Color.Red;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.ToolTip = "#VAL{D}";
            series1.YValuesPerPoint = 4;
            this.Chart1.Series.Add(series1);
            this.Chart1.Size = new System.Drawing.Size(1010, 561);
            this.Chart1.TabIndex = 0;
            // 
            // crtUnitTime
            // 
            this.crtUnitTime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.crtUnitTime.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.crtUnitTime.Legends.Add(legend2);
            this.crtUnitTime.Location = new System.Drawing.Point(0, 0);
            this.crtUnitTime.Name = "crtUnitTime";
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.DodgerBlue;
            series2.IsValueShownAsLabel = true;
            series2.LabelToolTip = "#VALX : #VAL";
            series2.Legend = "Legend1";
            series2.MarkerBorderColor = System.Drawing.Color.Red;
            series2.MarkerColor = System.Drawing.Color.Red;
            series2.MarkerSize = 8;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "unittime";
            this.crtUnitTime.Series.Add(series2);
            this.crtUnitTime.Size = new System.Drawing.Size(1010, 561);
            this.crtUnitTime.TabIndex = 1;
            // 
            // frmLineChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.crtUnitTime);
            this.Controls.Add(this.Chart1);
            this.KeyPreview = true;
            this.Name = "frmLineChart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Line Chart";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLineChart_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.crtUnitTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart crtUnitTime;
    }
}

