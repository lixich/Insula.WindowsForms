namespace InsulaWindowsForms
{
    partial class FormStatistics
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.dataGridViewAll = new System.Windows.Forms.DataGridView();
            this.chartXE = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartGlucose = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBoxXE = new System.Windows.Forms.ComboBox();
            this.comboBoxGlucose = new System.Windows.Forms.ComboBox();
            this.trackBarGlucose = new System.Windows.Forms.TrackBar();
            this.labelGlucose = new System.Windows.Forms.Label();
            this.trackBarXE = new System.Windows.Forms.TrackBar();
            this.labelXE = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartXE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartGlucose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGlucose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarXE)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewAll
            // 
            this.dataGridViewAll.AllowUserToAddRows = false;
            this.dataGridViewAll.AllowUserToDeleteRows = false;
            this.dataGridViewAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAll.Location = new System.Drawing.Point(728, 39);
            this.dataGridViewAll.Name = "dataGridViewAll";
            this.dataGridViewAll.ReadOnly = true;
            this.dataGridViewAll.Size = new System.Drawing.Size(595, 625);
            this.dataGridViewAll.TabIndex = 0;
            // 
            // chartXE
            // 
            chartArea5.Name = "ChartArea1";
            this.chartXE.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartXE.Legends.Add(legend5);
            this.chartXE.Location = new System.Drawing.Point(12, 39);
            this.chartXE.Name = "chartXE";
            this.chartXE.Size = new System.Drawing.Size(622, 290);
            this.chartXE.TabIndex = 1;
            this.chartXE.Text = "chartXE";
            // 
            // chartGlucose
            // 
            chartArea6.Name = "ChartArea1";
            this.chartGlucose.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chartGlucose.Legends.Add(legend6);
            this.chartGlucose.Location = new System.Drawing.Point(12, 374);
            this.chartGlucose.Name = "chartGlucose";
            this.chartGlucose.Size = new System.Drawing.Size(622, 290);
            this.chartGlucose.TabIndex = 2;
            this.chartGlucose.Text = "chartXE";
            // 
            // comboBoxXE
            // 
            this.comboBoxXE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXE.FormattingEnabled = true;
            this.comboBoxXE.Items.AddRange(new object[] {
            "Insulin dose for 1 XE",
            "XE"});
            this.comboBoxXE.Location = new System.Drawing.Point(12, 12);
            this.comboBoxXE.Name = "comboBoxXE";
            this.comboBoxXE.Size = new System.Drawing.Size(121, 21);
            this.comboBoxXE.TabIndex = 3;
            this.comboBoxXE.SelectedIndexChanged += new System.EventHandler(this.comboBoxXE_SelectedIndexChanged);
            // 
            // comboBoxGlucose
            // 
            this.comboBoxGlucose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGlucose.FormattingEnabled = true;
            this.comboBoxGlucose.Items.AddRange(new object[] {
            "Insulin dose to lower glucose by 1 mmol/L",
            "Glucose"});
            this.comboBoxGlucose.Location = new System.Drawing.Point(10, 347);
            this.comboBoxGlucose.Name = "comboBoxGlucose";
            this.comboBoxGlucose.Size = new System.Drawing.Size(121, 21);
            this.comboBoxGlucose.TabIndex = 4;
            this.comboBoxGlucose.SelectedIndexChanged += new System.EventHandler(this.comboBoxGlucose_SelectedIndexChanged);
            // 
            // trackBarGlucose
            // 
            this.trackBarGlucose.Location = new System.Drawing.Point(640, 374);
            this.trackBarGlucose.Maximum = 25;
            this.trackBarGlucose.Minimum = 6;
            this.trackBarGlucose.Name = "trackBarGlucose";
            this.trackBarGlucose.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarGlucose.Size = new System.Drawing.Size(45, 104);
            this.trackBarGlucose.TabIndex = 5;
            this.trackBarGlucose.Value = 25;
            this.trackBarGlucose.Scroll += new System.EventHandler(this.trackBarGlucose_Scroll);
            // 
            // labelGlucose
            // 
            this.labelGlucose.AutoSize = true;
            this.labelGlucose.Location = new System.Drawing.Point(637, 350);
            this.labelGlucose.Name = "labelGlucose";
            this.labelGlucose.Size = new System.Drawing.Size(46, 13);
            this.labelGlucose.TabIndex = 6;
            this.labelGlucose.Text = "Glucose";
            // 
            // trackBarXE
            // 
            this.trackBarXE.Location = new System.Drawing.Point(640, 518);
            this.trackBarXE.Maximum = 15;
            this.trackBarXE.Name = "trackBarXE";
            this.trackBarXE.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarXE.Size = new System.Drawing.Size(45, 104);
            this.trackBarXE.TabIndex = 7;
            this.trackBarXE.Value = 15;
            this.trackBarXE.Scroll += new System.EventHandler(this.trackBarXE_Scroll);
            // 
            // labelXE
            // 
            this.labelXE.AutoSize = true;
            this.labelXE.Location = new System.Drawing.Point(637, 494);
            this.labelXE.Name = "labelXE";
            this.labelXE.Size = new System.Drawing.Size(21, 13);
            this.labelXE.TabIndex = 8;
            this.labelXE.Text = "XE";
            // 
            // FormStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1338, 684);
            this.Controls.Add(this.labelXE);
            this.Controls.Add(this.trackBarXE);
            this.Controls.Add(this.labelGlucose);
            this.Controls.Add(this.trackBarGlucose);
            this.Controls.Add(this.comboBoxGlucose);
            this.Controls.Add(this.comboBoxXE);
            this.Controls.Add(this.chartGlucose);
            this.Controls.Add(this.chartXE);
            this.Controls.Add(this.dataGridViewAll);
            this.Name = "FormStatistics";
            this.Text = "Statistics";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartXE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartGlucose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGlucose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarXE)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewAll;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartXE;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGlucose;
        private System.Windows.Forms.ComboBox comboBoxXE;
        private System.Windows.Forms.ComboBox comboBoxGlucose;
        private System.Windows.Forms.TrackBar trackBarGlucose;
        private System.Windows.Forms.Label labelGlucose;
        private System.Windows.Forms.TrackBar trackBarXE;
        private System.Windows.Forms.Label labelXE;
    }
}