namespace Sunset
{
    partial class frmPreOpenCourse
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
            this.lblCurrentSchoolYearSemester = new DevComponents.DotNetBar.LabelX();
            this.lblPreOpenSchoolYearSemester = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // lblCurrentSchoolYearSemester
            // 
            this.lblCurrentSchoolYearSemester.AutoSize = true;
            this.lblCurrentSchoolYearSemester.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCurrentSchoolYearSemester.BackgroundStyle.Class = "";
            this.lblCurrentSchoolYearSemester.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCurrentSchoolYearSemester.Location = new System.Drawing.Point(29, 17);
            this.lblCurrentSchoolYearSemester.Name = "lblCurrentSchoolYearSemester";
            this.lblCurrentSchoolYearSemester.Size = new System.Drawing.Size(137, 21);
            this.lblCurrentSchoolYearSemester.TabIndex = 3;
            this.lblCurrentSchoolYearSemester.Text = "目前100學年度上學期";
            // 
            // lblPreOpenSchoolYearSemester
            // 
            this.lblPreOpenSchoolYearSemester.AutoSize = true;
            this.lblPreOpenSchoolYearSemester.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPreOpenSchoolYearSemester.BackgroundStyle.Class = "";
            this.lblPreOpenSchoolYearSemester.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPreOpenSchoolYearSemester.Location = new System.Drawing.Point(29, 52);
            this.lblPreOpenSchoolYearSemester.Name = "lblPreOpenSchoolYearSemester";
            this.lblPreOpenSchoolYearSemester.Size = new System.Drawing.Size(137, 21);
            this.lblPreOpenSchoolYearSemester.TabIndex = 7;
            this.lblPreOpenSchoolYearSemester.Text = "預排100學年度下學期";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(60, 87);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(50, 23);
            this.btnOK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "確定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(116, 87);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            // 
            // frmPreOpenCourse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 127);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPreOpenSchoolYearSemester);
            this.Controls.Add(this.lblCurrentSchoolYearSemester);
            this.Name = "frmPreOpenCourse";
            this.Text = "預開課程 - 確認學年度學期";
            this.Load += new System.EventHandler(this.frmPreOpenCourse_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lblCurrentSchoolYearSemester;
        private DevComponents.DotNetBar.LabelX lblPreOpenSchoolYearSemester;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
    }
}