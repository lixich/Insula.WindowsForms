namespace InsulaWindowsForms
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelPatient = new System.Windows.Forms.Label();
            this.comboBoxPatient = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.patientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelGrowth = new System.Windows.Forms.LinkLabel();
            this.labelGrowth = new System.Windows.Forms.Label();
            this.linkLabelWeight = new System.Windows.Forms.LinkLabel();
            this.labelWeight = new System.Windows.Forms.Label();
            this.linkLabelInsulin = new System.Windows.Forms.LinkLabel();
            this.labelInsulin = new System.Windows.Forms.Label();
            this.linkLabelAge = new System.Windows.Forms.LinkLabel();
            this.labelAge = new System.Windows.Forms.Label();
            this.linkLabelName = new System.Windows.Forms.LinkLabel();
            this.labelName = new System.Windows.Forms.Label();
            this.dataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPatient
            // 
            this.labelPatient.AutoSize = true;
            this.labelPatient.Location = new System.Drawing.Point(13, 36);
            this.labelPatient.Name = "labelPatient";
            this.labelPatient.Size = new System.Drawing.Size(40, 13);
            this.labelPatient.TabIndex = 0;
            this.labelPatient.Text = "Patient";
            // 
            // comboBoxPatient
            // 
            this.comboBoxPatient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPatient.FormattingEnabled = true;
            this.comboBoxPatient.Location = new System.Drawing.Point(59, 33);
            this.comboBoxPatient.Name = "comboBoxPatient";
            this.comboBoxPatient.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPatient.TabIndex = 1;
            this.comboBoxPatient.SelectedIndexChanged += new System.EventHandler(this.comboBoxPatient_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.patientToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.dataToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // patientToolStripMenuItem
            // 
            this.patientToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.patientToolStripMenuItem.Name = "patientToolStripMenuItem";
            this.patientToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.patientToolStripMenuItem.Text = "Patient";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDoseToolStripMenuItem,
            this.statisticsToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.dataToolStripMenuItem.Text = "Statistics";
            // 
            // newDoseToolStripMenuItem
            // 
            this.newDoseToolStripMenuItem.Name = "newDoseToolStripMenuItem";
            this.newDoseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newDoseToolStripMenuItem.Text = "NewDose";
            this.newDoseToolStripMenuItem.Click += new System.EventHandler(this.newDoseToolStripMenuItem_Click);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.statisticsToolStripMenuItem.Text = "Details";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabelGrowth);
            this.panel1.Controls.Add(this.labelGrowth);
            this.panel1.Controls.Add(this.linkLabelWeight);
            this.panel1.Controls.Add(this.labelWeight);
            this.panel1.Controls.Add(this.linkLabelInsulin);
            this.panel1.Controls.Add(this.labelInsulin);
            this.panel1.Controls.Add(this.linkLabelAge);
            this.panel1.Controls.Add(this.labelAge);
            this.panel1.Controls.Add(this.linkLabelName);
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Location = new System.Drawing.Point(16, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 182);
            this.panel1.TabIndex = 3;
            // 
            // linkLabelGrowth
            // 
            this.linkLabelGrowth.AutoSize = true;
            this.linkLabelGrowth.Location = new System.Drawing.Point(4, 146);
            this.linkLabelGrowth.Name = "linkLabelGrowth";
            this.linkLabelGrowth.Size = new System.Drawing.Size(83, 13);
            this.linkLabelGrowth.TabIndex = 9;
            this.linkLabelGrowth.TabStop = true;
            this.linkLabelGrowth.Text = "linkLabelGrowth";
            // 
            // labelGrowth
            // 
            this.labelGrowth.AutoSize = true;
            this.labelGrowth.Location = new System.Drawing.Point(4, 133);
            this.labelGrowth.Name = "labelGrowth";
            this.labelGrowth.Size = new System.Drawing.Size(41, 13);
            this.labelGrowth.TabIndex = 8;
            this.labelGrowth.Text = "Growth";
            // 
            // linkLabelWeight
            // 
            this.linkLabelWeight.AutoSize = true;
            this.linkLabelWeight.Location = new System.Drawing.Point(4, 112);
            this.linkLabelWeight.Name = "linkLabelWeight";
            this.linkLabelWeight.Size = new System.Drawing.Size(83, 13);
            this.linkLabelWeight.TabIndex = 7;
            this.linkLabelWeight.TabStop = true;
            this.linkLabelWeight.Text = "linkLabelWeight";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(4, 99);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(41, 13);
            this.labelWeight.TabIndex = 6;
            this.labelWeight.Text = "Weight";
            // 
            // linkLabelInsulin
            // 
            this.linkLabelInsulin.AutoSize = true;
            this.linkLabelInsulin.Location = new System.Drawing.Point(4, 80);
            this.linkLabelInsulin.Name = "linkLabelInsulin";
            this.linkLabelInsulin.Size = new System.Drawing.Size(79, 13);
            this.linkLabelInsulin.TabIndex = 5;
            this.linkLabelInsulin.TabStop = true;
            this.linkLabelInsulin.Text = "linkLabelInsulin";
            // 
            // labelInsulin
            // 
            this.labelInsulin.AutoSize = true;
            this.labelInsulin.Location = new System.Drawing.Point(4, 67);
            this.labelInsulin.Name = "labelInsulin";
            this.labelInsulin.Size = new System.Drawing.Size(37, 13);
            this.labelInsulin.TabIndex = 4;
            this.labelInsulin.Text = "Insulin";
            // 
            // linkLabelAge
            // 
            this.linkLabelAge.AutoSize = true;
            this.linkLabelAge.Location = new System.Drawing.Point(4, 48);
            this.linkLabelAge.Name = "linkLabelAge";
            this.linkLabelAge.Size = new System.Drawing.Size(68, 13);
            this.linkLabelAge.TabIndex = 3;
            this.linkLabelAge.TabStop = true;
            this.linkLabelAge.Text = "linkLabelAge";
            // 
            // labelAge
            // 
            this.labelAge.AutoSize = true;
            this.labelAge.Location = new System.Drawing.Point(4, 35);
            this.labelAge.Name = "labelAge";
            this.labelAge.Size = new System.Drawing.Size(26, 13);
            this.labelAge.TabIndex = 2;
            this.labelAge.Text = "Age";
            // 
            // linkLabelName
            // 
            this.linkLabelName.AutoSize = true;
            this.linkLabelName.Location = new System.Drawing.Point(4, 17);
            this.linkLabelName.Name = "linkLabelName";
            this.linkLabelName.Size = new System.Drawing.Size(77, 13);
            this.linkLabelName.TabIndex = 1;
            this.linkLabelName.TabStop = true;
            this.linkLabelName.Text = "linkLabelName";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(4, 4);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // dataToolStripMenuItem1
            // 
            this.dataToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.dataToolStripMenuItem1.Name = "dataToolStripMenuItem1";
            this.dataToolStripMenuItem1.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem1.Text = "Data";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "SaveAs";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.comboBoxPatient);
            this.Controls.Add(this.labelPatient);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Insula";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPatient;
        private System.Windows.Forms.ComboBox comboBoxPatient;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem patientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDoseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabelName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.LinkLabel linkLabelGrowth;
        private System.Windows.Forms.Label labelGrowth;
        private System.Windows.Forms.LinkLabel linkLabelWeight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.LinkLabel linkLabelInsulin;
        private System.Windows.Forms.Label labelInsulin;
        private System.Windows.Forms.LinkLabel linkLabelAge;
        private System.Windows.Forms.Label labelAge;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    }
}

