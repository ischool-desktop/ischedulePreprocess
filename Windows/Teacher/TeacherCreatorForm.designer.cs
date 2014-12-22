namespace Sunset.Windows
{
    partial class TeacherCreatorForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtNickname = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmbTeacherName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblCopy = new DevComponents.DotNetBar.LabelX();
            this.cmbNames = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbItemDefault = new DevComponents.Editors.ComboItem();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.lblName = new DevComponents.DotNetBar.LabelX();
            this.errorNameDuplicate = new System.Windows.Forms.ErrorProvider(this.components);
            this.lbTeacherCode = new DevComponents.DotNetBar.LabelX();
            this.lbTeachingExpertise = new DevComponents.DotNetBar.LabelX();
            this.txtTeacherCode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtTeachingExpertise = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lbNote = new DevComponents.DotNetBar.LabelX();
            this.txtNote = new DevComponents.DotNetBar.Controls.TextBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.errorNameDuplicate)).BeginInit();
            this.SuspendLayout();
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
            this.labelX2.Location = new System.Drawing.Point(23, 198);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(203, 21);
            this.labelX2.TabIndex = 12;
            this.labelX2.Text = "(可選擇已設定不排課時段之教師)";
            // 
            // txtNickname
            // 
            // 
            // 
            // 
            this.txtNickname.Border.Class = "TextBoxBorder";
            this.txtNickname.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtNickname.Location = new System.Drawing.Point(98, 45);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(186, 25);
            this.txtNickname.TabIndex = 3;
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
            this.labelX1.Location = new System.Drawing.Point(23, 47);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "教師暱稱：";
            // 
            // cmbTeacherName
            // 
            this.cmbTeacherName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbTeacherName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTeacherName.DisplayMember = "Text";
            this.cmbTeacherName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTeacherName.FormattingEnabled = true;
            this.cmbTeacherName.ItemHeight = 19;
            this.cmbTeacherName.Location = new System.Drawing.Point(98, 14);
            this.cmbTeacherName.Name = "cmbTeacherName";
            this.cmbTeacherName.Size = new System.Drawing.Size(186, 25);
            this.cmbTeacherName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbTeacherName.TabIndex = 1;
            // 
            // lblCopy
            // 
            this.lblCopy.AutoSize = true;
            this.lblCopy.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblCopy.BackgroundStyle.Class = "";
            this.lblCopy.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCopy.Location = new System.Drawing.Point(24, 171);
            this.lblCopy.Name = "lblCopy";
            this.lblCopy.Size = new System.Drawing.Size(74, 21);
            this.lblCopy.TabIndex = 10;
            this.lblCopy.Text = "複製時段：";
            // 
            // cmbNames
            // 
            this.cmbNames.DisplayMember = "Text";
            this.cmbNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNames.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbNames.FormattingEnabled = true;
            this.cmbNames.Items.AddRange(new object[] {
            this.cmbItemDefault});
            this.cmbNames.Location = new System.Drawing.Point(98, 169);
            this.cmbNames.Name = "cmbNames";
            this.cmbNames.Size = new System.Drawing.Size(186, 25);
            this.cmbNames.TabIndex = 11;
            this.cmbNames.SelectedIndexChanged += new System.EventHandler(this.cmbNames_SelectedIndexChanged);
            // 
            // cmbItemDefault
            // 
            this.cmbItemDefault.Text = "不進行複製";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(224, 227);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnClose);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(143, 227);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblName.BackgroundStyle.Class = "";
            this.lblName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblName.Location = new System.Drawing.Point(23, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(74, 21);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "教師姓名：";
            // 
            // errorNameDuplicate
            // 
            this.errorNameDuplicate.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorNameDuplicate.ContainerControl = this;
            // 
            // lbTeacherCode
            // 
            this.lbTeacherCode.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbTeacherCode.BackgroundStyle.Class = "";
            this.lbTeacherCode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbTeacherCode.Location = new System.Drawing.Point(22, 77);
            this.lbTeacherCode.Name = "lbTeacherCode";
            this.lbTeacherCode.Size = new System.Drawing.Size(75, 23);
            this.lbTeacherCode.TabIndex = 4;
            this.lbTeacherCode.Text = "教師代碼：";
            // 
            // lbTeachingExpertise
            // 
            this.lbTeachingExpertise.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbTeachingExpertise.BackgroundStyle.Class = "";
            this.lbTeachingExpertise.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbTeachingExpertise.Location = new System.Drawing.Point(22, 108);
            this.lbTeachingExpertise.Name = "lbTeachingExpertise";
            this.lbTeachingExpertise.Size = new System.Drawing.Size(75, 23);
            this.lbTeachingExpertise.TabIndex = 6;
            this.lbTeachingExpertise.Text = "教學專長：";
            // 
            // txtTeacherCode
            // 
            // 
            // 
            // 
            this.txtTeacherCode.Border.Class = "TextBoxBorder";
            this.txtTeacherCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTeacherCode.Location = new System.Drawing.Point(98, 76);
            this.txtTeacherCode.Name = "txtTeacherCode";
            this.txtTeacherCode.Size = new System.Drawing.Size(186, 25);
            this.txtTeacherCode.TabIndex = 5;
            // 
            // txtTeachingExpertise
            // 
            // 
            // 
            // 
            this.txtTeachingExpertise.Border.Class = "TextBoxBorder";
            this.txtTeachingExpertise.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTeachingExpertise.Location = new System.Drawing.Point(98, 107);
            this.txtTeachingExpertise.Name = "txtTeachingExpertise";
            this.txtTeachingExpertise.Size = new System.Drawing.Size(186, 25);
            this.txtTeachingExpertise.TabIndex = 7;
            // 
            // lbNote
            // 
            this.lbNote.AutoSize = true;
            this.lbNote.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbNote.BackgroundStyle.Class = "";
            this.lbNote.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbNote.Location = new System.Drawing.Point(22, 140);
            this.lbNote.Name = "lbNote";
            this.lbNote.Size = new System.Drawing.Size(74, 21);
            this.lbNote.TabIndex = 8;
            this.lbNote.Text = "註　　記：";
            // 
            // txtNote
            // 
            // 
            // 
            // 
            this.txtNote.Border.Class = "TextBoxBorder";
            this.txtNote.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtNote.Location = new System.Drawing.Point(98, 138);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(186, 25);
            this.txtNote.TabIndex = 9;
            // 
            // TeacherCreatorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(310, 263);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.lbNote);
            this.Controls.Add(this.txtTeachingExpertise);
            this.Controls.Add(this.txtTeacherCode);
            this.Controls.Add(this.lbTeachingExpertise);
            this.Controls.Add(this.lbTeacherCode);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtNickname);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbTeacherName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblCopy);
            this.Controls.Add(this.cmbNames);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TeacherCreatorForm";
            this.Text = "";
            this.Load += new System.EventHandler(this.TeacherNameCreatorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorNameDuplicate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.LabelX lblCopy;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbNames;
        private DevComponents.Editors.ComboItem cmbItemDefault;
        private System.Windows.Forms.ErrorProvider errorNameDuplicate;
        private DevComponents.DotNetBar.LabelX lblName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTeacherName;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtNickname;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTeachingExpertise;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTeacherCode;
        private DevComponents.DotNetBar.LabelX lbTeachingExpertise;
        private DevComponents.DotNetBar.LabelX lbTeacherCode;
        private DevComponents.DotNetBar.Controls.TextBoxX txtNote;
        private DevComponents.DotNetBar.LabelX lbNote;
    }
}