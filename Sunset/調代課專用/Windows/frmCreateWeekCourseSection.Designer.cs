//namespace ischedulePlus
//{
//    partial class frmCreateWeekCourseSection
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.lblHelp = new DevComponents.DotNetBar.LabelX();
//            this.lstSchoolYearSemesterDate = new System.Windows.Forms.ListBox();
//            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
//            this.btnClose = new DevComponents.DotNetBar.ButtonX();
//            this.btnExport = new DevComponents.DotNetBar.ButtonX();
//            this.SuspendLayout();
//            // 
//            // lblHelp
//            // 
//            this.lblHelp.BackColor = System.Drawing.Color.Transparent;
//            // 
//            // 
//            // 
//            this.lblHelp.BackgroundStyle.Class = "";
//            this.lblHelp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.lblHelp.Dock = System.Windows.Forms.DockStyle.Top;
//            this.lblHelp.Location = new System.Drawing.Point(0, 0);
//            this.lblHelp.Name = "lblHelp";
//            this.lblHelp.Size = new System.Drawing.Size(446, 61);
//            this.lblHelp.TabIndex = 0;
//            this.lblHelp.Text = "1.請選擇學年度及學期，依照選取用來產生每週課程分段。\r\n2.若是學年度及學期的結束日期小於開始日期則不產生。\r\n3.系統會為產生的週課程分課加上日期及週次。";
//            this.lblHelp.TextLineAlignment = System.Drawing.StringAlignment.Near;
//            this.lblHelp.WordWrap = true;
//            // 
//            // lstSchoolYearSemesterDate
//            // 
//            this.lstSchoolYearSemesterDate.Dock = System.Windows.Forms.DockStyle.Top;
//            this.lstSchoolYearSemesterDate.FormattingEnabled = true;
//            this.lstSchoolYearSemesterDate.ItemHeight = 17;
//            this.lstSchoolYearSemesterDate.Location = new System.Drawing.Point(0, 61);
//            this.lstSchoolYearSemesterDate.Name = "lstSchoolYearSemesterDate";
//            this.lstSchoolYearSemesterDate.Size = new System.Drawing.Size(446, 157);
//            this.lstSchoolYearSemesterDate.TabIndex = 1;
//            // 
//            // btnConfirm
//            // 
//            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
//            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
//            this.btnConfirm.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
//            this.btnConfirm.Enabled = false;
//            this.btnConfirm.Location = new System.Drawing.Point(207, 224);
//            this.btnConfirm.Name = "btnConfirm";
//            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
//            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.btnConfirm.TabIndex = 2;
//            this.btnConfirm.Text = "確   定";
//            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
//            // 
//            // btnClose
//            // 
//            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
//            this.btnClose.BackColor = System.Drawing.Color.Transparent;
//            this.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
//            this.btnClose.Enabled = false;
//            this.btnClose.Location = new System.Drawing.Point(370, 224);
//            this.btnClose.Name = "btnClose";
//            this.btnClose.Size = new System.Drawing.Size(75, 23);
//            this.btnClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.btnClose.TabIndex = 3;
//            this.btnClose.Text = "離   開";
//            // 
//            // btnExport
//            // 
//            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
//            this.btnExport.BackColor = System.Drawing.Color.Transparent;
//            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
//            this.btnExport.Location = new System.Drawing.Point(289, 224);
//            this.btnExport.Name = "btnExport";
//            this.btnExport.Size = new System.Drawing.Size(75, 23);
//            this.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.btnExport.TabIndex = 4;
//            this.btnExport.Text = "匯   出";
//            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
//            // 
//            // frmCreateWeekCourseSection
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(446, 250);
//            this.Controls.Add(this.btnExport);
//            this.Controls.Add(this.btnClose);
//            this.Controls.Add(this.btnConfirm);
//            this.Controls.Add(this.lstSchoolYearSemesterDate);
//            this.Controls.Add(this.lblHelp);
//            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
//            this.Name = "frmCreateWeekCourseSection";
//            this.Text = "批次建立週課表";
//            this.Load += new System.EventHandler(this.frmCreateWeekCourseSection_Load);
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevComponents.DotNetBar.LabelX lblHelp;
//        private System.Windows.Forms.ListBox lstSchoolYearSemesterDate;
//        private DevComponents.DotNetBar.ButtonX btnConfirm;
//        private DevComponents.DotNetBar.ButtonX btnClose;
//        private DevComponents.DotNetBar.ButtonX btnExport;
//    }
//}