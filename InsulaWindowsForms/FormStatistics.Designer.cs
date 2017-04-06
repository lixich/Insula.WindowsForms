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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.dataGridViewAll = new System.Windows.Forms.DataGridView();
            this.chart2D = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart3D = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBoxXE = new System.Windows.Forms.ComboBox();
            this.comboBoxGlucose = new System.Windows.Forms.ComboBox();
            this.trackBarGlucose = new System.Windows.Forms.TrackBar();
            this.labelGlucose = new System.Windows.Forms.Label();
            this.trackBarXE = new System.Windows.Forms.TrackBar();
            this.labelXE = new System.Windows.Forms.Label();
            this.numericUpDownCoefForGoodFacts = new System.Windows.Forms.NumericUpDown();
            this.labelCoefForGoodFacts = new System.Windows.Forms.Label();
            this.labelDifferenceIn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGlucose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarXE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoefForGoodFacts)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewAll
            // 
            this.dataGridViewAll.AllowUserToAddRows = false;
            this.dataGridViewAll.AllowUserToDeleteRows = false;
            this.dataGridViewAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAll.Location = new System.Drawing.Point(739, 39);
            this.dataGridViewAll.Name = "dataGridViewAll";
            this.dataGridViewAll.ReadOnly = true;
            this.dataGridViewAll.Size = new System.Drawing.Size(584, 625);
            this.dataGridViewAll.TabIndex = 0;
            // 
            // chart2D
            // 
            chartArea1.Name = "ChartArea1";
            this.chart2D.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart2D.Legends.Add(legend1);
            this.chart2D.Location = new System.Drawing.Point(12, 39);
            this.chart2D.Name = "chart2D";
            this.chart2D.Size = new System.Drawing.Size(622, 290);
            this.chart2D.TabIndex = 1;
            this.chart2D.Text = "chart2D";
            // 
            // chart3D
            // 
            chartArea2.Area3DStyle.Enable3D = true;
            chartArea2.Area3DStyle.Inclination = 60;
            chartArea2.Area3DStyle.IsRightAngleAxes = false;
            chartArea2.Name = "ChartArea1";
            this.chart3D.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart3D.Legends.Add(legend2);
            this.chart3D.Location = new System.Drawing.Point(12, 374);
            this.chart3D.Name = "chart3D";
            this.chart3D.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            this.chart3D.Size = new System.Drawing.Size(622, 290);
            this.chart3D.TabIndex = 2;
            this.chart3D.Text = "chart3D";
            // 
            // comboBoxXE
            // 
            this.comboBoxXE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXE.FormattingEnabled = true;
            this.comboBoxXE.Items.AddRange(new object[] {
            "Dose for 1 XE(Time)",
            "Dose to lower glucose by 1 mmol/L(Time)",
            "My dose(Time)",
            "My dose(XE)",
            "My dose(DifferenceInGlucose)",
            "My dose(GlucoseBeforeMeal)",
            "My glucose(Time)",
            "My XE(Time)"});
            this.comboBoxXE.Location = new System.Drawing.Point(12, 12);
            this.comboBoxXE.MinimumSize = new System.Drawing.Size(275, 0);
            this.comboBoxXE.Name = "comboBoxXE";
            this.comboBoxXE.Size = new System.Drawing.Size(275, 21);
            this.comboBoxXE.TabIndex = 3;
            this.comboBoxXE.SelectedIndexChanged += new System.EventHandler(this.comboBoxXE_SelectedIndexChanged);
            // 
            // comboBoxGlucose
            // 
            this.comboBoxGlucose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGlucose.FormattingEnabled = true;
            this.comboBoxGlucose.Items.AddRange(new object[] {
            "Dose for 1 XE(Time, XE)",
            "Dose for 1 XE(Time, BeforeMeal)",
            "Dose to lower glucose by 1 mmol/L(Time, DifferenceInGlucose)",
            "Dose to lower glucose by 1 mmol/L(Time, BeforeMeal)",
            "My dose(XE, Time)",
            "My dose(DifferenceInGlucose, XE)",
            "Dose(DifferenceInGlucose, XE)"});
            this.comboBoxGlucose.Location = new System.Drawing.Point(10, 347);
            this.comboBoxGlucose.Name = "comboBoxGlucose";
            this.comboBoxGlucose.Size = new System.Drawing.Size(277, 21);
            this.comboBoxGlucose.TabIndex = 4;
            this.comboBoxGlucose.SelectedIndexChanged += new System.EventHandler(this.comboBoxGlucose_SelectedIndexChanged);
            // 
            // trackBarGlucose
            // 
            this.trackBarGlucose.Location = new System.Drawing.Point(640, 120);
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
            this.labelGlucose.Location = new System.Drawing.Point(637, 96);
            this.labelGlucose.Name = "labelGlucose";
            this.labelGlucose.Size = new System.Drawing.Size(46, 13);
            this.labelGlucose.TabIndex = 6;
            this.labelGlucose.Text = "Glucose";
            // 
            // trackBarXE
            // 
            this.trackBarXE.Location = new System.Drawing.Point(640, 264);
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
            this.labelXE.Location = new System.Drawing.Point(637, 240);
            this.labelXE.Name = "labelXE";
            this.labelXE.Size = new System.Drawing.Size(21, 13);
            this.labelXE.TabIndex = 8;
            this.labelXE.Text = "XE";
            // 
            // numericUpDownCoefForGoodFacts
            // 
            this.numericUpDownCoefForGoodFacts.DecimalPlaces = 1;
            this.numericUpDownCoefForGoodFacts.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownCoefForGoodFacts.Location = new System.Drawing.Point(640, 53);
            this.numericUpDownCoefForGoodFacts.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownCoefForGoodFacts.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownCoefForGoodFacts.Name = "numericUpDownCoefForGoodFacts";
            this.numericUpDownCoefForGoodFacts.Size = new System.Drawing.Size(45, 20);
            this.numericUpDownCoefForGoodFacts.TabIndex = 9;
            this.numericUpDownCoefForGoodFacts.Tag = "";
            this.numericUpDownCoefForGoodFacts.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownCoefForGoodFacts.ValueChanged += new System.EventHandler(this.numericUpDownCoefForGoodFacts_ValueChanged);
            // 
            // labelCoefForGoodFacts
            // 
            this.labelCoefForGoodFacts.AutoSize = true;
            this.labelCoefForGoodFacts.Location = new System.Drawing.Point(637, 37);
            this.labelCoefForGoodFacts.Name = "labelCoefForGoodFacts";
            this.labelCoefForGoodFacts.Size = new System.Drawing.Size(96, 13);
            this.labelCoefForGoodFacts.TabIndex = 10;
            this.labelCoefForGoodFacts.Text = "CoefForGoodFacts";
            // 
            // labelDifferenceIn
            // 
            this.labelDifferenceIn.AutoSize = true;
            this.labelDifferenceIn.Location = new System.Drawing.Point(637, 83);
            this.labelDifferenceIn.Name = "labelDifferenceIn";
            this.labelDifferenceIn.Size = new System.Drawing.Size(68, 13);
            this.labelDifferenceIn.TabIndex = 11;
            this.labelDifferenceIn.Text = "Difference In";
            // 
            // FormStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1338, 684);
            this.Controls.Add(this.labelDifferenceIn);
            this.Controls.Add(this.labelCoefForGoodFacts);
            this.Controls.Add(this.numericUpDownCoefForGoodFacts);
            this.Controls.Add(this.labelXE);
            this.Controls.Add(this.trackBarXE);
            this.Controls.Add(this.labelGlucose);
            this.Controls.Add(this.trackBarGlucose);
            this.Controls.Add(this.comboBoxGlucose);
            this.Controls.Add(this.comboBoxXE);
            this.Controls.Add(this.chart3D);
            this.Controls.Add(this.chart2D);
            this.Controls.Add(this.dataGridViewAll);
            this.Name = "FormStatistics";
            this.Text = "Statistics";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGlucose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarXE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoefForGoodFacts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewAll;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2D;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3D;
        private System.Windows.Forms.ComboBox comboBoxXE;
        private System.Windows.Forms.ComboBox comboBoxGlucose;
        private System.Windows.Forms.TrackBar trackBarGlucose;
        private System.Windows.Forms.Label labelGlucose;
        private System.Windows.Forms.TrackBar trackBarXE;
        private System.Windows.Forms.Label labelXE;
        private System.Windows.Forms.NumericUpDown numericUpDownCoefForGoodFacts;
        private System.Windows.Forms.Label labelCoefForGoodFacts;
        private System.Windows.Forms.Label labelDifferenceIn;
    }
}