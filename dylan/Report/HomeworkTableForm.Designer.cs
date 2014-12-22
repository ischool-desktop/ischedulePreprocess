namespace Sunset
{
    partial class HomeworkTableForm
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
            this.txtHelp1 = new DevComponents.DotNetBar.LabelX();
            this.intSchoolYear = new DevComponents.Editors.IntegerInput();
            this.intSemester = new DevComponents.Editors.IntegerInput();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.txtHelp3 = new DevComponents.DotNetBar.LabelX();
            this.txtHelp2 = new DevComponents.DotNetBar.LabelX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.cbIsPrintTo_ePort = new DevComponents.DotNetBar.Controls.CheckBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSemester)).BeginInit();
            this.SuspendLayout();
            // 
            // txtHelp1
            // 
            this.txtHelp1.AutoSize = true;
            this.txtHelp1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.txtHelp1.BackgroundStyle.Class = "";
            this.txtHelp1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtHelp1.Location = new System.Drawing.Point(14, 10);
            this.txtHelp1.Name = "txtHelp1";
            this.txtHelp1.Size = new System.Drawing.Size(127, 21);
            this.txtHelp1.TabIndex = 16;
            this.txtHelp1.Text = "請輸入學年度學期：";
            // 
            // intSchoolYear
            // 
            this.intSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.intSchoolYear.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intSchoolYear.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intSchoolYear.ButtonFreeText.Checked = true;
            this.intSchoolYear.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intSchoolYear.FreeTextEntryMode = true;
            this.intSchoolYear.Location = new System.Drawing.Point(94, 48);
            this.intSchoolYear.MaxValue = 999;
            this.intSchoolYear.MinValue = 90;
            this.intSchoolYear.Name = "intSchoolYear";
            this.intSchoolYear.ShowUpDown = true;
            this.intSchoolYear.Size = new System.Drawing.Size(64, 25);
            this.intSchoolYear.TabIndex = 15;
            this.intSchoolYear.Value = 90;
            // 
            // intSemester
            // 
            this.intSemester.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.intSemester.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intSemester.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intSemester.ButtonFreeText.Checked = true;
            this.intSemester.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intSemester.FreeTextEntryMode = true;
            this.intSemester.Location = new System.Drawing.Point(209, 48);
            this.intSemester.MaxValue = 2;
            this.intSemester.MinValue = 1;
            this.intSemester.Name = "intSemester";
            this.intSemester.ShowUpDown = true;
            this.intSemester.Size = new System.Drawing.Size(64, 25);
            this.intSemester.TabIndex = 14;
            this.intSemester.Value = 1;
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(251, 93);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "取消";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtHelp3
            // 
            this.txtHelp3.AutoSize = true;
            this.txtHelp3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.txtHelp3.BackgroundStyle.Class = "";
            this.txtHelp3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtHelp3.Location = new System.Drawing.Point(166, 50);
            this.txtHelp3.Name = "txtHelp3";
            this.txtHelp3.Size = new System.Drawing.Size(34, 21);
            this.txtHelp3.TabIndex = 12;
            this.txtHelp3.Text = "學期";
            // 
            // txtHelp2
            // 
            this.txtHelp2.AutoSize = true;
            this.txtHelp2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.txtHelp2.BackgroundStyle.Class = "";
            this.txtHelp2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtHelp2.Location = new System.Drawing.Point(30, 50);
            this.txtHelp2.Name = "txtHelp2";
            this.txtHelp2.Size = new System.Drawing.Size(47, 21);
            this.txtHelp2.TabIndex = 11;
            this.txtHelp2.Text = "學年度";
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(170, 93);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "列印";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Location = new System.Drawing.Point(14, 101);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(60, 17);
            this.linkLabel1.TabIndex = 17;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "設定樣版";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel2.Location = new System.Drawing.Point(80, 101);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(60, 17);
            this.linkLabel2.TabIndex = 18;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "欄位總表";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // cbIsPrintTo_ePort
            // 
            this.cbIsPrintTo_ePort.AutoSize = true;
            this.cbIsPrintTo_ePort.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.cbIsPrintTo_ePort.BackgroundStyle.Class = "";
            this.cbIsPrintTo_ePort.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.cbIsPrintTo_ePort.Location = new System.Drawing.Point(179, 10);
            this.cbIsPrintTo_ePort.Name = "cbIsPrintTo_ePort";
            this.cbIsPrintTo_ePort.Size = new System.Drawing.Size(147, 21);
            this.cbIsPrintTo_ePort.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbIsPrintTo_ePort.TabIndex = 19;
            this.cbIsPrintTo_ePort.Text = "列印至學生電子報表";
            this.cbIsPrintTo_ePort.TextColor = System.Drawing.Color.Red;
            // 
            // HomeworkTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 126);
            this.Controls.Add(this.cbIsPrintTo_ePort);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.txtHelp1);
            this.Controls.Add(this.intSchoolYear);
            this.Controls.Add(this.intSemester);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtHelp3);
            this.Controls.Add(this.txtHelp2);
            this.Controls.Add(this.btnSave);
            this.Name = "HomeworkTableForm";
            this.Text = "學生功課表";
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSemester)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX txtHelp1;
        private DevComponents.Editors.IntegerInput intSchoolYear;
        private DevComponents.Editors.IntegerInput intSemester;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.LabelX txtHelp3;
        private DevComponents.DotNetBar.LabelX txtHelp2;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private DevComponents.DotNetBar.Controls.CheckBoxX cbIsPrintTo_ePort;
    }
}