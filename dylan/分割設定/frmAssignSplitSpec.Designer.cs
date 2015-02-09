namespace Sunset.NewCourse
{
    partial class frmAssignSplitSpec
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
            this.txtSplitSpec = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lbHelp2 = new DevComponents.DotNetBar.LabelX();
            this.chkCreateCourseSection = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.errSplitSpec = new System.Windows.Forms.ErrorProvider(this.components);
            this.lbHelp1 = new DevComponents.DotNetBar.LabelX();
            this.lbHelp3 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.errSplitSpec)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSplitSpec
            // 
            // 
            // 
            // 
            this.txtSplitSpec.Border.Class = "TextBoxBorder";
            this.txtSplitSpec.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSplitSpec.Location = new System.Drawing.Point(83, 45);
            this.txtSplitSpec.Name = "txtSplitSpec";
            this.txtSplitSpec.Size = new System.Drawing.Size(208, 25);
            this.txtSplitSpec.TabIndex = 26;
            this.txtSplitSpec.TextChanged += new System.EventHandler(this.txtSplitSpec_TextChanged);
            // 
            // lbHelp2
            // 
            this.lbHelp2.AutoSize = true;
            this.lbHelp2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbHelp2.BackgroundStyle.Class = "";
            this.lbHelp2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbHelp2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbHelp2.Location = new System.Drawing.Point(16, 47);
            this.lbHelp2.Name = "lbHelp2";
            this.lbHelp2.Size = new System.Drawing.Size(74, 21);
            this.lbHelp2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lbHelp2.TabIndex = 25;
            this.lbHelp2.Text = "分割設定：";
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
            this.chkCreateCourseSection.Location = new System.Drawing.Point(16, 115);
            this.chkCreateCourseSection.Name = "chkCreateCourseSection";
            this.chkCreateCourseSection.Size = new System.Drawing.Size(107, 21);
            this.chkCreateCourseSection.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCreateCourseSection.TabIndex = 28;
            this.chkCreateCourseSection.Text = "產生課程分段";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.AutoSize = true;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(225, 113);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(144, 113);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOK.TabIndex = 29;
            this.btnOK.Text = "確認";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // errSplitSpec
            // 
            this.errSplitSpec.ContainerControl = this;
            // 
            // lbHelp1
            // 
            this.lbHelp1.AutoSize = true;
            this.lbHelp1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbHelp1.BackgroundStyle.Class = "";
            this.lbHelp1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbHelp1.Location = new System.Drawing.Point(16, 13);
            this.lbHelp1.Name = "lbHelp1";
            this.lbHelp1.Size = new System.Drawing.Size(114, 21);
            this.lbHelp1.TabIndex = 31;
            this.lbHelp1.Text = "所選課程節數為：";
            // 
            // lbHelp3
            // 
            this.lbHelp3.AutoSize = true;
            this.lbHelp3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbHelp3.BackgroundStyle.Class = "";
            this.lbHelp3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbHelp3.ForeColor = System.Drawing.Color.Gray;
            this.lbHelp3.Location = new System.Drawing.Point(19, 81);
            this.lbHelp3.Name = "lbHelp3";
            this.lbHelp3.Size = new System.Drawing.Size(194, 21);
            this.lbHelp3.TabIndex = 32;
            this.lbHelp3.Text = "(如:4節數可分割為[2,2]或[2,1,1]";
            // 
            // frmAssignSplitSpec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 151);
            this.Controls.Add(this.lbHelp3);
            this.Controls.Add(this.lbHelp1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkCreateCourseSection);
            this.Controls.Add(this.txtSplitSpec);
            this.Controls.Add(this.lbHelp2);
            this.Name = "frmAssignSplitSpec";
            this.Text = "批次指定課程分割設定";
            ((System.ComponentModel.ISupportInitialize)(this.errSplitSpec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtSplitSpec;
        private DevComponents.DotNetBar.LabelX lbHelp2;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkCreateCourseSection;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private System.Windows.Forms.ErrorProvider errSplitSpec;
        private DevComponents.DotNetBar.LabelX lbHelp1;
        private DevComponents.DotNetBar.LabelX lbHelp3;
    }
}