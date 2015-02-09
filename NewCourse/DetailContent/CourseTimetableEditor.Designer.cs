namespace Sunset.NewCourse
{
    partial class CourseTimetableEditor
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel6 = new System.Windows.Forms.Panel();
            this.chkNoLimitNextDay = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkLimitNextDay = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chkNoAllowDup = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkAllowDup = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkCloseQuery = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkOpenQuery = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX27 = new DevComponents.DotNetBar.LabelX();
            this.lblGroupName = new DevComponents.DotNetBar.LabelX();
            this.txtCourseAliasName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.txtSplitSpec = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbTimeTable = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.chkNoLimitNextDay);
            this.panel6.Controls.Add(this.chkLimitNextDay);
            this.panel6.Location = new System.Drawing.Point(379, 76);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(142, 25);
            this.panel6.TabIndex = 67;
            // 
            // chkNoLimitNextDay
            // 
            this.chkNoLimitNextDay.AutoSize = true;
            // 
            // 
            // 
            this.chkNoLimitNextDay.BackgroundStyle.Class = "";
            this.chkNoLimitNextDay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoLimitNextDay.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkNoLimitNextDay.Location = new System.Drawing.Point(68, 2);
            this.chkNoLimitNextDay.Name = "chkNoLimitNextDay";
            this.chkNoLimitNextDay.Size = new System.Drawing.Size(40, 21);
            this.chkNoLimitNextDay.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoLimitNextDay.TabIndex = 7;
            this.chkNoLimitNextDay.Text = "否";
            // 
            // chkLimitNextDay
            // 
            this.chkLimitNextDay.AutoSize = true;
            // 
            // 
            // 
            this.chkLimitNextDay.BackgroundStyle.Class = "";
            this.chkLimitNextDay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkLimitNextDay.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkLimitNextDay.Location = new System.Drawing.Point(8, 2);
            this.chkLimitNextDay.Name = "chkLimitNextDay";
            this.chkLimitNextDay.Size = new System.Drawing.Size(40, 21);
            this.chkLimitNextDay.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkLimitNextDay.TabIndex = 6;
            this.chkLimitNextDay.Text = "是";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            // 
            // 
            // 
            this.labelX18.BackgroundStyle.Class = "";
            this.labelX18.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX18.Location = new System.Drawing.Point(272, 78);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(101, 21);
            this.labelX18.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX18.TabIndex = 66;
            this.labelX18.Text = "分段不同日排課";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.chkNoAllowDup);
            this.panel5.Controls.Add(this.chkAllowDup);
            this.panel5.Location = new System.Drawing.Point(379, 42);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(142, 25);
            this.panel5.TabIndex = 65;
            // 
            // chkNoAllowDup
            // 
            this.chkNoAllowDup.AutoSize = true;
            // 
            // 
            // 
            this.chkNoAllowDup.BackgroundStyle.Class = "";
            this.chkNoAllowDup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoAllowDup.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkNoAllowDup.Location = new System.Drawing.Point(68, 2);
            this.chkNoAllowDup.Name = "chkNoAllowDup";
            this.chkNoAllowDup.Size = new System.Drawing.Size(40, 21);
            this.chkNoAllowDup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoAllowDup.TabIndex = 7;
            this.chkNoAllowDup.Text = "否";
            // 
            // chkAllowDup
            // 
            this.chkAllowDup.AutoSize = true;
            // 
            // 
            // 
            this.chkAllowDup.BackgroundStyle.Class = "";
            this.chkAllowDup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkAllowDup.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkAllowDup.Location = new System.Drawing.Point(8, 2);
            this.chkAllowDup.Name = "chkAllowDup";
            this.chkAllowDup.Size = new System.Drawing.Size(40, 21);
            this.chkAllowDup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkAllowDup.TabIndex = 6;
            this.chkAllowDup.Text = "是";
            // 
            // labelX16
            // 
            this.labelX16.AutoSize = true;
            // 
            // 
            // 
            this.labelX16.BackgroundStyle.Class = "";
            this.labelX16.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX16.Location = new System.Drawing.Point(272, 44);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(101, 21);
            this.labelX16.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX16.TabIndex = 64;
            this.labelX16.Text = "分段可同天排課";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chkCloseQuery);
            this.panel4.Controls.Add(this.chkOpenQuery);
            this.panel4.Location = new System.Drawing.Point(379, 8);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(142, 25);
            this.panel4.TabIndex = 63;
            // 
            // chkCloseQuery
            // 
            this.chkCloseQuery.AutoSize = true;
            // 
            // 
            // 
            this.chkCloseQuery.BackgroundStyle.Class = "";
            this.chkCloseQuery.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCloseQuery.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkCloseQuery.Location = new System.Drawing.Point(68, 2);
            this.chkCloseQuery.Name = "chkCloseQuery";
            this.chkCloseQuery.Size = new System.Drawing.Size(67, 21);
            this.chkCloseQuery.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCloseQuery.TabIndex = 7;
            this.chkCloseQuery.Text = "不開放";
            // 
            // chkOpenQuery
            // 
            this.chkOpenQuery.AutoSize = true;
            // 
            // 
            // 
            this.chkOpenQuery.BackgroundStyle.Class = "";
            this.chkOpenQuery.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkOpenQuery.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkOpenQuery.Location = new System.Drawing.Point(8, 2);
            this.chkOpenQuery.Name = "chkOpenQuery";
            this.chkOpenQuery.Size = new System.Drawing.Size(54, 21);
            this.chkOpenQuery.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkOpenQuery.TabIndex = 6;
            this.chkOpenQuery.Text = "開放";
            // 
            // labelX27
            // 
            this.labelX27.AutoSize = true;
            // 
            // 
            // 
            this.labelX27.BackgroundStyle.Class = "";
            this.labelX27.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX27.Location = new System.Drawing.Point(292, 10);
            this.labelX27.Name = "labelX27";
            this.labelX27.Size = new System.Drawing.Size(60, 21);
            this.labelX27.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX27.TabIndex = 62;
            this.labelX27.Text = "開放查詢";
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            // 
            // 
            // 
            this.lblGroupName.BackgroundStyle.Class = "";
            this.lblGroupName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblGroupName.Location = new System.Drawing.Point(270, 50);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(0, 0);
            this.lblGroupName.TabIndex = 23;
            // 
            // txtCourseAliasName
            // 
            // 
            // 
            // 
            this.txtCourseAliasName.Border.Class = "TextBoxBorder";
            this.txtCourseAliasName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCourseAliasName.Location = new System.Drawing.Point(117, 42);
            this.txtCourseAliasName.Name = "txtCourseAliasName";
            this.txtCourseAliasName.Size = new System.Drawing.Size(142, 25);
            this.txtCourseAliasName.TabIndex = 22;
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(44, 44);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(60, 21);
            this.labelX7.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX7.TabIndex = 21;
            this.labelX7.Text = "科目簡稱";
            // 
            // txtSplitSpec
            // 
            // 
            // 
            // 
            this.txtSplitSpec.Border.Class = "TextBoxBorder";
            this.txtSplitSpec.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSplitSpec.Location = new System.Drawing.Point(117, 76);
            this.txtSplitSpec.Name = "txtSplitSpec";
            this.txtSplitSpec.Size = new System.Drawing.Size(142, 25);
            this.txtSplitSpec.TabIndex = 20;
            this.txtSplitSpec.WatermarkText = "範例：1,2,2";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(44, 78);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX2.TabIndex = 19;
            this.labelX2.Text = "分割設定";
            // 
            // cmbTimeTable
            // 
            this.cmbTimeTable.DisplayMember = "Text";
            this.cmbTimeTable.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTimeTable.FormattingEnabled = true;
            this.cmbTimeTable.ItemHeight = 19;
            this.cmbTimeTable.Location = new System.Drawing.Point(117, 8);
            this.cmbTimeTable.Name = "cmbTimeTable";
            this.cmbTimeTable.Size = new System.Drawing.Size(142, 25);
            this.cmbTimeTable.TabIndex = 18;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(37, 10);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.TabIndex = 17;
            this.labelX1.Text = "上課時間表";
            // 
            // TimetableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.labelX18);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelX16);
            this.Controls.Add(this.cmbTimeTable);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.txtSplitSpec);
            this.Controls.Add(this.labelX27);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.lblGroupName);
            this.Controls.Add(this.txtCourseAliasName);
            this.Name = "TimetableEditor";
            this.Size = new System.Drawing.Size(550, 115);
            this.Load += new System.EventHandler(this.TimetableEditor_Load);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkNoLimitNextDay;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkLimitNextDay;
        private DevComponents.DotNetBar.LabelX labelX18;
        private System.Windows.Forms.Panel panel5;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkNoAllowDup;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkAllowDup;
        private DevComponents.DotNetBar.LabelX labelX16;
        private System.Windows.Forms.Panel panel4;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkCloseQuery;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkOpenQuery;
        private DevComponents.DotNetBar.LabelX labelX27;
        private DevComponents.DotNetBar.LabelX lblGroupName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCourseAliasName;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSplitSpec;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTimeTable;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}
