namespace Sunset.NewCourse
{
    partial class frmOpenCourse
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateCourseSection = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmbSchoolYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbSemester = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grdClassList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGradeYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProgramPlan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuProgramPlan = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClassList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.labelX3);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 586);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 37);
            this.panel1.TabIndex = 3;
            // 
            // labelX3
            // 
            this.labelX3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(12, 8);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(141, 21);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "請用右鍵指定課程規劃";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(408, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(327, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "開課";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.chkCreateCourseSection);
            this.panel2.Controls.Add(this.cmbSchoolYear);
            this.panel2.Controls.Add(this.labelX2);
            this.panel2.Controls.Add(this.cmbSemester);
            this.panel2.Controls.Add(this.labelX1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(492, 44);
            this.panel2.TabIndex = 9;
            // 
            // chkCreateCourseSection
            // 
            this.chkCreateCourseSection.AutoSize = true;
            this.chkCreateCourseSection.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkCreateCourseSection.BackgroundStyle.Class = "";
            this.chkCreateCourseSection.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCreateCourseSection.Location = new System.Drawing.Point(289, 14);
            this.chkCreateCourseSection.Name = "chkCreateCourseSection";
            this.chkCreateCourseSection.Size = new System.Drawing.Size(107, 21);
            this.chkCreateCourseSection.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCreateCourseSection.TabIndex = 29;
            this.chkCreateCourseSection.Text = "產生課程分段";
            // 
            // cmbSchoolYear
            // 
            this.cmbSchoolYear.DisplayMember = "Text";
            this.cmbSchoolYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSchoolYear.FormattingEnabled = true;
            this.cmbSchoolYear.ItemHeight = 19;
            this.cmbSchoolYear.Location = new System.Drawing.Point(73, 12);
            this.cmbSchoolYear.Name = "cmbSchoolYear";
            this.cmbSchoolYear.Size = new System.Drawing.Size(83, 25);
            this.cmbSchoolYear.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSchoolYear.TabIndex = 12;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(11, 14);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.TabIndex = 11;
            this.labelX2.Text = "學年度：";
            // 
            // cmbSemester
            // 
            this.cmbSemester.DisplayMember = "Text";
            this.cmbSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSemester.FormattingEnabled = true;
            this.cmbSemester.ItemHeight = 19;
            this.cmbSemester.Location = new System.Drawing.Point(215, 12);
            this.cmbSemester.Name = "cmbSemester";
            this.cmbSemester.Size = new System.Drawing.Size(68, 25);
            this.cmbSemester.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSemester.TabIndex = 10;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(166, 14);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(47, 21);
            this.labelX1.TabIndex = 9;
            this.labelX1.Text = "學期：";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.grdClassList);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 44);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(492, 542);
            this.panel3.TabIndex = 10;
            // 
            // grdClassList
            // 
            this.grdClassList.AllowUserToAddRows = false;
            this.grdClassList.AllowUserToDeleteRows = false;
            this.grdClassList.AllowUserToOrderColumns = true;
            this.grdClassList.AllowUserToResizeColumns = false;
            this.grdClassList.AllowUserToResizeRows = false;
            this.grdClassList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdClassList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdClassList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClassList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClassName,
            this.colGradeYear,
            this.colProgramPlan});
            this.grdClassList.ContextMenuStrip = this.menuProgramPlan;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdClassList.DefaultCellStyle = dataGridViewCellStyle5;
            this.grdClassList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdClassList.Location = new System.Drawing.Point(14, 13);
            this.grdClassList.Name = "grdClassList";
            this.grdClassList.RowHeadersVisible = false;
            this.grdClassList.RowTemplate.Height = 24;
            this.grdClassList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdClassList.Size = new System.Drawing.Size(465, 517);
            this.grdClassList.TabIndex = 5;
            // 
            // colClassName
            // 
            this.colClassName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colClassName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colClassName.HeaderText = "班級";
            this.colClassName.Name = "colClassName";
            this.colClassName.ReadOnly = true;
            this.colClassName.Width = 59;
            // 
            // colGradeYear
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colGradeYear.DefaultCellStyle = dataGridViewCellStyle3;
            this.colGradeYear.HeaderText = "年級";
            this.colGradeYear.MinimumWidth = 60;
            this.colGradeYear.Name = "colGradeYear";
            this.colGradeYear.ReadOnly = true;
            this.colGradeYear.Width = 60;
            // 
            // colProgramPlan
            // 
            this.colProgramPlan.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colProgramPlan.DefaultCellStyle = dataGridViewCellStyle4;
            this.colProgramPlan.HeaderText = "課程規劃";
            this.colProgramPlan.Name = "colProgramPlan";
            this.colProgramPlan.ReadOnly = true;
            this.colProgramPlan.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // menuProgramPlan
            // 
            this.menuProgramPlan.Name = "menuProgramPlan";
            this.menuProgramPlan.Size = new System.Drawing.Size(61, 4);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn1.HeaderText = "班級名稱";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn2.HeaderText = "班級年級";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn3.HeaderText = "課程規劃";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // frmOpenCourse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 623);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmOpenCourse";
            this.Text = "依課程規劃開課";
            this.Load += new System.EventHandler(this.frmOpenCourse_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdClassList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.LabelX labelX3;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSchoolYear;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSemester;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.Panel panel3;
        private DevComponents.DotNetBar.Controls.DataGridViewX grdClassList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ContextMenuStrip menuProgramPlan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGradeYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProgramPlan;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkCreateCourseSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

    }
}