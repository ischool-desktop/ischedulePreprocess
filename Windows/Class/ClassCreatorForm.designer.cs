namespace Sunset.Windows
{
    partial class ClassCreatorForm
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
            this.txtGradeYear = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmbClassName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblCopy = new DevComponents.DotNetBar.LabelX();
            this.cmbNames = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbItemDefault = new DevComponents.Editors.ComboItem();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.lblName = new DevComponents.DotNetBar.LabelX();
            this.errorNameDuplicate = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorGradeYear = new System.Windows.Forms.ErrorProvider(this.components);
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX2 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.errorNameDuplicate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorGradeYear)).BeginInit();
            this.SuspendLayout();
            // 
            // txtGradeYear
            // 
            // 
            // 
            // 
            this.txtGradeYear.Border.Class = "TextBoxBorder";
            this.txtGradeYear.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtGradeYear.Location = new System.Drawing.Point(98, 44);
            this.txtGradeYear.Name = "txtGradeYear";
            this.txtGradeYear.Size = new System.Drawing.Size(187, 25);
            this.txtGradeYear.TabIndex = 5;
            this.txtGradeYear.TextChanged += new System.EventHandler(this.txtGradeYear_TextChanged);
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
            this.labelX1.Location = new System.Drawing.Point(21, 46);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.TabIndex = 4;
            this.labelX1.Text = "班級年級：";
            // 
            // cmbClassName
            // 
            this.cmbClassName.DisplayMember = "Text";
            this.cmbClassName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbClassName.FormattingEnabled = true;
            this.cmbClassName.ItemHeight = 19;
            this.cmbClassName.Location = new System.Drawing.Point(98, 12);
            this.cmbClassName.Name = "cmbClassName";
            this.cmbClassName.Size = new System.Drawing.Size(187, 25);
            this.cmbClassName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbClassName.TabIndex = 3;
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
            this.lblCopy.Location = new System.Drawing.Point(21, 142);
            this.lblCopy.Name = "lblCopy";
            this.lblCopy.Size = new System.Drawing.Size(74, 21);
            this.lblCopy.TabIndex = 0;
            this.lblCopy.Text = "複製班級：";
            // 
            // cmbNames
            // 
            this.cmbNames.DisplayMember = "Text";
            this.cmbNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNames.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbNames.FormattingEnabled = true;
            this.cmbNames.Items.AddRange(new object[] {
            this.cmbItemDefault});
            this.cmbNames.Location = new System.Drawing.Point(97, 140);
            this.cmbNames.Name = "cmbNames";
            this.cmbNames.Size = new System.Drawing.Size(188, 25);
            this.cmbNames.TabIndex = 1;
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
            this.btnCancel.Location = new System.Drawing.Point(210, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnClose);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(129, 204);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 0;
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
            this.lblName.Location = new System.Drawing.Point(21, 14);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(74, 21);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "班級名稱：";
            // 
            // errorNameDuplicate
            // 
            this.errorNameDuplicate.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorNameDuplicate.ContainerControl = this;
            // 
            // errorGradeYear
            // 
            this.errorGradeYear.ContainerControl = this;
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
            this.labelX2.Location = new System.Drawing.Point(21, 78);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(74, 21);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "班級代碼：";
            // 
            // textBoxX1
            // 
            // 
            // 
            // 
            this.textBoxX1.Border.Class = "TextBoxBorder";
            this.textBoxX1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX1.Location = new System.Drawing.Point(98, 76);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(187, 25);
            this.textBoxX1.TabIndex = 7;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(21, 174);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(203, 21);
            this.labelX3.TabIndex = 13;
            this.labelX3.Text = "(可選擇已設定不排課時段之班級)";
            // 
            // textBoxX2
            // 
            // 
            // 
            // 
            this.textBoxX2.Border.Class = "TextBoxBorder";
            this.textBoxX2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX2.Location = new System.Drawing.Point(98, 108);
            this.textBoxX2.Name = "textBoxX2";
            this.textBoxX2.Size = new System.Drawing.Size(187, 25);
            this.textBoxX2.TabIndex = 15;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(21, 110);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(74, 21);
            this.labelX4.TabIndex = 14;
            this.labelX4.Text = "註　　記：";
            // 
            // ClassCreatorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(306, 241);
            this.Controls.Add(this.textBoxX2);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.textBoxX1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtGradeYear);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cmbClassName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblCopy);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbNames);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ClassCreatorForm";
            this.Text = "";
            this.Load += new System.EventHandler(this.TeacherNameCreatorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorNameDuplicate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorGradeYear)).EndInit();
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
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbClassName;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGradeYear;
        private System.Windows.Forms.ErrorProvider errorGradeYear;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX2;
        private DevComponents.DotNetBar.LabelX labelX4;
    }
}