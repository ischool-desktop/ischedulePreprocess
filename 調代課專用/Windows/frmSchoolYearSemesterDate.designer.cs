//namespace Sunset
//{
//    partial class frmSchoolYearSemesterDate
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
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
//            this.dgvMultiValues = new DevComponents.DotNetBar.Controls.DataGridViewX();
//            this.colSchoolYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.colSemester = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.colStartDate = new DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn();
//            this.colEndDate = new DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn();
//            this.btnSave = new DevComponents.DotNetBar.ButtonX();
//            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
//            ((System.ComponentModel.ISupportInitialize)(this.dgvMultiValues)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // dgvMultiValues
//            // 
//            this.dgvMultiValues.BackgroundColor = System.Drawing.Color.White;
//            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
//            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
//            dataGridViewCellStyle7.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
//            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
//            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
//            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
//            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
//            this.dgvMultiValues.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
//            this.dgvMultiValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.dgvMultiValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
//            this.colSchoolYear,
//            this.colSemester,
//            this.colStartDate,
//            this.colEndDate});
//            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
//            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
//            dataGridViewCellStyle5.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
//            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
//            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
//            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
//            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
//            this.dgvMultiValues.DefaultCellStyle = dataGridViewCellStyle5;
//            this.dgvMultiValues.Dock = System.Windows.Forms.DockStyle.Top;
//            this.dgvMultiValues.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
//            this.dgvMultiValues.Location = new System.Drawing.Point(0, 0);
//            this.dgvMultiValues.Name = "dgvMultiValues";
//            this.dgvMultiValues.RowTemplate.Height = 24;
//            this.dgvMultiValues.Size = new System.Drawing.Size(457, 322);
//            this.dgvMultiValues.TabIndex = 0;
//            this.dgvMultiValues.SelectionChanged += new System.EventHandler(this.dgvMultiValues_SelectionChanged);
//            // 
//            // colSchoolYear
//            // 
//            this.colSchoolYear.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
//            this.colSchoolYear.DataPropertyName = "SchoolYear";
//            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
//            this.colSchoolYear.DefaultCellStyle = dataGridViewCellStyle8;
//            this.colSchoolYear.HeaderText = "學年度";
//            this.colSchoolYear.Name = "colSchoolYear";
//            this.colSchoolYear.Width = 72;
//            // 
//            // colSemester
//            // 
//            this.colSemester.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
//            this.colSemester.DataPropertyName = "Semester";
//            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
//            this.colSemester.DefaultCellStyle = dataGridViewCellStyle9;
//            this.colSemester.HeaderText = "學期";
//            this.colSemester.Name = "colSemester";
//            this.colSemester.Width = 59;
//            // 
//            // colStartDate
//            // 
//            this.colStartDate.AllowEmptyState = false;
//            this.colStartDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
//            // 
//            // 
//            // 
//            this.colStartDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
//            this.colStartDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder";
//            this.colStartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.colStartDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
//            this.colStartDate.DataPropertyName = "StartDate";
//            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
//            this.colStartDate.DefaultCellStyle = dataGridViewCellStyle10;
//            this.colStartDate.HeaderText = "開始日期";
//            this.colStartDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Center;
//            // 
//            // 
//            // 
//            this.colStartDate.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
//            // 
//            // 
//            // 
//            this.colStartDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
//            this.colStartDate.MonthCalendar.BackgroundStyle.Class = "";
//            this.colStartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            // 
//            // 
//            // 
//            this.colStartDate.MonthCalendar.CommandsBackgroundStyle.Class = "";
//            this.colStartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.colStartDate.MonthCalendar.DisplayMonth = new System.DateTime(2011, 11, 1, 0, 0, 0, 0);
//            this.colStartDate.MonthCalendar.MarkedDates = new System.DateTime[0];
//            this.colStartDate.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
//            // 
//            // 
//            // 
//            this.colStartDate.MonthCalendar.NavigationBackgroundStyle.Class = "";
//            this.colStartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.colStartDate.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
//            this.colStartDate.Name = "colStartDate";
//            this.colStartDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
//            // 
//            // colEndDate
//            // 
//            this.colEndDate.AllowEmptyState = false;
//            this.colEndDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
//            // 
//            // 
//            // 
//            this.colEndDate.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
//            this.colEndDate.BackgroundStyle.Class = "DataGridViewDateTimeBorder";
//            this.colEndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.colEndDate.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
//            this.colEndDate.DataPropertyName = "EndDate";
//            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
//            this.colEndDate.DefaultCellStyle = dataGridViewCellStyle4;
//            this.colEndDate.HeaderText = "結束日期";
//            this.colEndDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Center;
//            // 
//            // 
//            // 
//            this.colEndDate.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
//            // 
//            // 
//            // 
//            this.colEndDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
//            this.colEndDate.MonthCalendar.BackgroundStyle.Class = "";
//            this.colEndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            // 
//            // 
//            // 
//            this.colEndDate.MonthCalendar.CommandsBackgroundStyle.Class = "";
//            this.colEndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.colEndDate.MonthCalendar.DisplayMonth = new System.DateTime(2011, 11, 1, 0, 0, 0, 0);
//            this.colEndDate.MonthCalendar.MarkedDates = new System.DateTime[0];
//            this.colEndDate.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
//            // 
//            // 
//            // 
//            this.colEndDate.MonthCalendar.NavigationBackgroundStyle.Class = "";
//            this.colEndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.colEndDate.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
//            this.colEndDate.Name = "colEndDate";
//            // 
//            // btnSave
//            // 
//            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
//            this.btnSave.BackColor = System.Drawing.Color.Transparent;
//            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
//            this.btnSave.Location = new System.Drawing.Point(297, 328);
//            this.btnSave.Name = "btnSave";
//            this.btnSave.Size = new System.Drawing.Size(75, 23);
//            this.btnSave.TabIndex = 1;
//            this.btnSave.Text = "儲存";
//            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//            // 
//            // btnCancel
//            // 
//            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
//            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
//            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
//            this.btnCancel.Location = new System.Drawing.Point(378, 328);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(75, 23);
//            this.btnCancel.TabIndex = 1;
//            this.btnCancel.Text = "離開";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
//            // 
//            // frmSchoolYearSemesterDate
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(457, 353);
//            this.Controls.Add(this.btnCancel);
//            this.Controls.Add(this.btnSave);
//            this.Controls.Add(this.dgvMultiValues);
//            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
//            this.Name = "frmSchoolYearSemesterDate";
//            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
//            this.Text = "學年度學期日期對應";
//            this.Load += new System.EventHandler(this.UDTDataGridView_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.dgvMultiValues)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevComponents.DotNetBar.Controls.DataGridViewX dgvMultiValues;
//        private DevComponents.DotNetBar.ButtonX btnSave;
//        private DevComponents.DotNetBar.ButtonX btnCancel;
//        private System.Windows.Forms.DataGridViewTextBoxColumn colSchoolYear;
//        private System.Windows.Forms.DataGridViewTextBoxColumn colSemester;
//        private DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn colStartDate;
//        private DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn colEndDate;
//    }
//}