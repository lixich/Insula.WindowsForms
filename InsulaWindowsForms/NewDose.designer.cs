namespace InsulaWindowsForms
{
    partial class NewDose
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
            this.btnComplete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numCurrentGlucose = new System.Windows.Forms.NumericUpDown();
            this.numPlanGlucose = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numXE = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbNumberNeighbors = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDoseKNN = new System.Windows.Forms.TextBox();
            this.tbAccuracy = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tbTime = new System.Windows.Forms.MaskedTextBox();
            this.tbDose = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentGlucose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPlanGlucose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(15, 215);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(62, 23);
            this.btnComplete.TabIndex = 0;
            this.btnComplete.Text = "Complete";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current level of glucose";
            // 
            // numCurrentGlucose
            // 
            this.numCurrentGlucose.DecimalPlaces = 1;
            this.numCurrentGlucose.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numCurrentGlucose.Location = new System.Drawing.Point(15, 26);
            this.numCurrentGlucose.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numCurrentGlucose.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numCurrentGlucose.Name = "numCurrentGlucose";
            this.numCurrentGlucose.Size = new System.Drawing.Size(48, 20);
            this.numCurrentGlucose.TabIndex = 2;
            this.numCurrentGlucose.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // numPlanGlucose
            // 
            this.numPlanGlucose.DecimalPlaces = 1;
            this.numPlanGlucose.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numPlanGlucose.Location = new System.Drawing.Point(15, 66);
            this.numPlanGlucose.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPlanGlucose.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numPlanGlucose.Name = "numPlanGlucose";
            this.numPlanGlucose.Size = new System.Drawing.Size(48, 20);
            this.numPlanGlucose.TabIndex = 5;
            this.numPlanGlucose.Tag = "";
            this.numPlanGlucose.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Planned level of glucose";
            // 
            // numXE
            // 
            this.numXE.DecimalPlaces = 1;
            this.numXE.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numXE.Location = new System.Drawing.Point(15, 106);
            this.numXE.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numXE.Name = "numXE";
            this.numXE.Size = new System.Drawing.Size(48, 20);
            this.numXE.TabIndex = 7;
            this.numXE.Tag = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "XE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Number of neighbors";
            // 
            // cmbNumberNeighbors
            // 
            this.cmbNumberNeighbors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNumberNeighbors.FormattingEnabled = true;
            this.cmbNumberNeighbors.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbNumberNeighbors.Location = new System.Drawing.Point(15, 186);
            this.cmbNumberNeighbors.Name = "cmbNumberNeighbors";
            this.cmbNumberNeighbors.Size = new System.Drawing.Size(48, 21);
            this.cmbNumberNeighbors.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Dose by KNN";
            // 
            // tbDoseKNN
            // 
            this.tbDoseKNN.BackColor = System.Drawing.SystemColors.Control;
            this.tbDoseKNN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDoseKNN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDoseKNN.Location = new System.Drawing.Point(15, 267);
            this.tbDoseKNN.Name = "tbDoseKNN";
            this.tbDoseKNN.ReadOnly = true;
            this.tbDoseKNN.Size = new System.Drawing.Size(48, 21);
            this.tbDoseKNN.TabIndex = 14;
            this.tbDoseKNN.Text = "0";
            // 
            // tbAccuracy
            // 
            this.tbAccuracy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAccuracy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbAccuracy.Location = new System.Drawing.Point(89, 267);
            this.tbAccuracy.Name = "tbAccuracy";
            this.tbAccuracy.ReadOnly = true;
            this.tbAccuracy.Size = new System.Drawing.Size(48, 21);
            this.tbAccuracy.TabIndex = 16;
            this.tbAccuracy.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(84, 251);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Euclidean dist.";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(166, 9);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(274, 320);
            this.dataGridView1.TabIndex = 17;
            // 
            // tbTime
            // 
            this.tbTime.Location = new System.Drawing.Point(15, 145);
            this.tbTime.Mask = "00:00:00";
            this.tbTime.Name = "tbTime";
            this.tbTime.Size = new System.Drawing.Size(48, 20);
            this.tbTime.TabIndex = 18;
            // 
            // tbDose
            // 
            this.tbDose.BackColor = System.Drawing.SystemColors.Control;
            this.tbDose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDose.Location = new System.Drawing.Point(15, 307);
            this.tbDose.Name = "tbDose";
            this.tbDose.ReadOnly = true;
            this.tbDose.Size = new System.Drawing.Size(48, 21);
            this.tbDose.TabIndex = 20;
            this.tbDose.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 291);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Dose by Harmonic";
            // 
            // NewDose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 341);
            this.Controls.Add(this.tbDose);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbTime);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tbAccuracy);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbDoseKNN);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbNumberNeighbors);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numXE);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numPlanGlucose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numCurrentGlucose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnComplete);
            this.MinimumSize = new System.Drawing.Size(468, 325);
            this.Name = "NewDose";
            this.Text = "NewDose";
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentGlucose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPlanGlucose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numCurrentGlucose;
        private System.Windows.Forms.NumericUpDown numPlanGlucose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numXE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbNumberNeighbors;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbDoseKNN;
        private System.Windows.Forms.TextBox tbAccuracy;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MaskedTextBox tbTime;
        private System.Windows.Forms.TextBox tbDose;
        private System.Windows.Forms.Label label8;
    }
}