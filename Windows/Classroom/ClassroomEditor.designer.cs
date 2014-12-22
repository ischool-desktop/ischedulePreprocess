﻿namespace Sunset
{
    partial class ClassroomEditor
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
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.grdTimeTableBusyEditor = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.contextMenuBusy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuBusy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBusyDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFree = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbTimeTables = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboBoxEx1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.grdClassroomBusys = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colWeekDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartHour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartMinute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeekFlag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colBusyDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblName = new DevComponents.DotNetBar.LabelX();
            this.cmbLocation = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblLocation = new DevComponents.DotNetBar.LabelX();
            this.chkLocationOnly = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtCapacity = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblCapacity = new DevComponents.DotNetBar.LabelX();
            this.lblClassroomDesc = new DevComponents.DotNetBar.LabelX();
            this.txtClassroomCode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblClassroomCode = new DevComponents.DotNetBar.LabelX();
            this.contextMenuStripDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelEx1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTimeTableBusyEditor)).BeginInit();
            this.contextMenuBusy.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClassroomBusys)).BeginInit();
            this.panel1.SuspendLayout();
            this.contextMenuStripDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.panel2);
            this.panelEx1.Controls.Add(this.panel1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(530, 434);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(530, 400);
            this.panel2.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.BackColor = System.Drawing.Color.Transparent;
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Controls.Add(this.tabControlPanel2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(530, 400);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.Tabs.Add(this.tabItem2);
            this.tabControl1.Text = "tabControl1";
            this.tabControl1.DoubleClick += new System.EventHandler(this.tabControl1_DoubleClick);
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.grdTimeTableBusyEditor);
            this.tabControlPanel1.Controls.Add(this.panel3);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 29);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(530, 371);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            // 
            // grdTimeTableBusyEditor
            // 
            this.grdTimeTableBusyEditor.AllowUserToAddRows = false;
            this.grdTimeTableBusyEditor.AllowUserToDeleteRows = false;
            this.grdTimeTableBusyEditor.AllowUserToResizeColumns = false;
            this.grdTimeTableBusyEditor.AllowUserToResizeRows = false;
            this.grdTimeTableBusyEditor.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdTimeTableBusyEditor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.grdTimeTableBusyEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTimeTableBusyEditor.ContextMenuStrip = this.contextMenuBusy;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdTimeTableBusyEditor.DefaultCellStyle = dataGridViewCellStyle8;
            this.grdTimeTableBusyEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTimeTableBusyEditor.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdTimeTableBusyEditor.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdTimeTableBusyEditor.Location = new System.Drawing.Point(1, 35);
            this.grdTimeTableBusyEditor.Name = "grdTimeTableBusyEditor";
            this.grdTimeTableBusyEditor.RowHeadersVisible = false;
            this.grdTimeTableBusyEditor.RowTemplate.Height = 24;
            this.grdTimeTableBusyEditor.Size = new System.Drawing.Size(528, 335);
            this.grdTimeTableBusyEditor.TabIndex = 22;
            // 
            // contextMenuBusy
            // 
            this.contextMenuBusy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBusy,
            this.menuBusyDesc,
            this.menuFree});
            this.contextMenuBusy.Name = "contextMenuStrip1";
            this.contextMenuBusy.ShowImageMargin = false;
            this.contextMenuBusy.Size = new System.Drawing.Size(186, 70);
            // 
            // menuBusy
            // 
            this.menuBusy.Name = "menuBusy";
            this.menuBusy.Size = new System.Drawing.Size(185, 22);
            this.menuBusy.Text = "設定不排課時段";
            // 
            // menuBusyDesc
            // 
            this.menuBusyDesc.Name = "menuBusyDesc";
            this.menuBusyDesc.Size = new System.Drawing.Size(185, 22);
            this.menuBusyDesc.Text = "設定不排課時段(指定描述)";
            // 
            // menuFree
            // 
            this.menuFree.Name = "menuFree";
            this.menuFree.Size = new System.Drawing.Size(185, 22);
            this.menuFree.Text = "取消設定";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.labelX4);
            this.panel3.Controls.Add(this.labelX2);
            this.panel3.Controls.Add(this.cmbTimeTables);
            this.panel3.Controls.Add(this.comboBoxEx1);
            this.panel3.Controls.Add(this.labelX1);
            this.panel3.Controls.Add(this.labelX3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(1, 1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(528, 34);
            this.panel3.TabIndex = 21;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(242, 7);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(219, 21);
            this.labelX4.TabIndex = 24;
            this.labelX4.Text = "(可雙擊左鍵設定,或多選以右鍵設定)";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(11, 7);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.TabIndex = 23;
            this.labelX2.Text = "時間表：";
            // 
            // cmbTimeTables
            // 
            this.cmbTimeTables.DisplayMember = "TimeTableName";
            this.cmbTimeTables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTimeTables.FormattingEnabled = true;
            this.cmbTimeTables.ItemHeight = 19;
            this.cmbTimeTables.Location = new System.Drawing.Point(71, 5);
            this.cmbTimeTables.Name = "cmbTimeTables";
            this.cmbTimeTables.Size = new System.Drawing.Size(155, 25);
            this.cmbTimeTables.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbTimeTables.TabIndex = 18;
            this.cmbTimeTables.ValueMember = "UID";
            this.cmbTimeTables.SelectedIndexChanged += new System.EventHandler(this.cmbTimeTables_SelectedIndexChanged);
            // 
            // comboBoxEx1
            // 
            this.comboBoxEx1.DisplayMember = "Text";
            this.comboBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx1.FormattingEnabled = true;
            this.comboBoxEx1.ItemHeight = 19;
            this.comboBoxEx1.Location = new System.Drawing.Point(313, 37);
            this.comboBoxEx1.Name = "comboBoxEx1";
            this.comboBoxEx1.Size = new System.Drawing.Size(121, 25);
            this.comboBoxEx1.TabIndex = 12;
            this.comboBoxEx1.Visible = false;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(241, 41);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.TabIndex = 11;
            this.labelX1.Text = "所在地點：";
            this.labelX1.Visible = false;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(234, 41);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(74, 21);
            this.labelX3.TabIndex = 3;
            this.labelX3.Text = "場地代碼：";
            this.labelX3.Visible = false;
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "不排課時段";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.grdClassroomBusys);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 29);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(530, 371);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabItem2;
            // 
            // grdClassroomBusys
            // 
            this.grdClassroomBusys.BackgroundColor = System.Drawing.Color.White;
            this.grdClassroomBusys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClassroomBusys.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWeekDay,
            this.colStartHour,
            this.colStartMinute,
            this.colWeekFlag,
            this.colBusyDesc});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdClassroomBusys.DefaultCellStyle = dataGridViewCellStyle5;
            this.grdClassroomBusys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdClassroomBusys.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdClassroomBusys.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdClassroomBusys.HighlightSelectedColumnHeaders = false;
            this.grdClassroomBusys.Location = new System.Drawing.Point(1, 1);
            this.grdClassroomBusys.MultiSelect = false;
            this.grdClassroomBusys.Name = "grdClassroomBusys";
            this.grdClassroomBusys.RowHeadersWidth = 35;
            this.grdClassroomBusys.RowTemplate.Height = 24;
            this.grdClassroomBusys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdClassroomBusys.Size = new System.Drawing.Size(528, 369);
            this.grdClassroomBusys.TabIndex = 3;
            // 
            // colWeekDay
            // 
            this.colWeekDay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colWeekDay.HeaderText = "星期";
            this.colWeekDay.Name = "colWeekDay";
            this.colWeekDay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colWeekDay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWeekDay.ToolTipText = "只能輸入1到7";
            this.colWeekDay.Width = 40;
            // 
            // colStartHour
            // 
            this.colStartHour.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colStartHour.HeaderText = "開始時間";
            this.colStartHour.Name = "colStartHour";
            this.colStartHour.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStartHour.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStartHour.Width = 66;
            // 
            // colStartMinute
            // 
            this.colStartMinute.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colStartMinute.HeaderText = "結束時間";
            this.colStartMinute.Name = "colStartMinute";
            this.colStartMinute.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStartMinute.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStartMinute.Width = 66;
            // 
            // colWeekFlag
            // 
            this.colWeekFlag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colWeekFlag.HeaderText = "單雙週";
            this.colWeekFlag.Items.AddRange(new object[] {
            "單",
            "雙",
            "單雙"});
            this.colWeekFlag.Name = "colWeekFlag";
            this.colWeekFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colWeekFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colWeekFlag.ToolTipText = "單週、雙週或單雙週";
            this.colWeekFlag.Visible = false;
            // 
            // colBusyDesc
            // 
            this.colBusyDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBusyDesc.HeaderText = "不排課描述";
            this.colBusyDesc.Name = "colBusyDesc";
            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.tabControlPanel2;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "清單檢視";
            this.tabItem2.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.cmbLocation);
            this.panel1.Controls.Add(this.lblLocation);
            this.panel1.Controls.Add(this.chkLocationOnly);
            this.panel1.Controls.Add(this.txtCapacity);
            this.panel1.Controls.Add(this.lblCapacity);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 34);
            this.panel1.TabIndex = 3;
            this.panel1.DoubleClick += new System.EventHandler(this.panel1_DoubleClick);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            // 
            // 
            // 
            this.lblName.BackgroundStyle.Class = "";
            this.lblName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblName.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblName.Location = new System.Drawing.Point(4, 1);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(87, 32);
            this.lblName.TabIndex = 13;
            this.lblName.Text = "場地名稱";
            // 
            // cmbLocation
            // 
            this.cmbLocation.DisplayMember = "Text";
            this.cmbLocation.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.ItemHeight = 19;
            this.cmbLocation.Location = new System.Drawing.Point(313, 37);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(121, 25);
            this.cmbLocation.TabIndex = 12;
            this.cmbLocation.Visible = false;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            // 
            // 
            // 
            this.lblLocation.BackgroundStyle.Class = "";
            this.lblLocation.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLocation.Location = new System.Drawing.Point(241, 41);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(74, 21);
            this.lblLocation.TabIndex = 11;
            this.lblLocation.Text = "所在地點：";
            this.lblLocation.Visible = false;
            // 
            // chkLocationOnly
            // 
            this.chkLocationOnly.AutoSize = true;
            // 
            // 
            // 
            this.chkLocationOnly.BackgroundStyle.Class = "";
            this.chkLocationOnly.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkLocationOnly.Location = new System.Drawing.Point(29, 39);
            this.chkLocationOnly.Name = "chkLocationOnly";
            this.chkLocationOnly.Size = new System.Drawing.Size(187, 21);
            this.chkLocationOnly.TabIndex = 10;
            this.chkLocationOnly.Text = "無使用限制（無限容納數）";
            this.chkLocationOnly.Visible = false;
            // 
            // txtCapacity
            // 
            this.txtCapacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtCapacity.Border.Class = "TextBoxBorder";
            this.txtCapacity.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCapacity.Location = new System.Drawing.Point(452, 5);
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.Size = new System.Drawing.Size(50, 25);
            this.txtCapacity.TabIndex = 8;
            this.txtCapacity.TextChanged += new System.EventHandler(this.txtCapacity_TextChanged);
            // 
            // lblCapacity
            // 
            this.lblCapacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCapacity.AutoSize = true;
            // 
            // 
            // 
            this.lblCapacity.BackgroundStyle.Class = "";
            this.lblCapacity.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCapacity.Location = new System.Drawing.Point(351, 7);
            this.lblCapacity.Name = "lblCapacity";
            this.lblCapacity.Size = new System.Drawing.Size(101, 21);
            this.lblCapacity.TabIndex = 7;
            this.lblCapacity.Text = "可容納班級數：";
            // 
            // lblClassroomDesc
            // 
            this.lblClassroomDesc.AutoSize = true;
            // 
            // 
            // 
            this.lblClassroomDesc.BackgroundStyle.Class = "";
            this.lblClassroomDesc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblClassroomDesc.Location = new System.Drawing.Point(22, 8);
            this.lblClassroomDesc.Name = "lblClassroomDesc";
            this.lblClassroomDesc.Size = new System.Drawing.Size(0, 0);
            this.lblClassroomDesc.TabIndex = 5;
            this.lblClassroomDesc.Text = "場地描述：";
            // 
            // txtClassroomCode
            // 
            // 
            // 
            // 
            this.txtClassroomCode.Border.Class = "TextBoxBorder";
            this.txtClassroomCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtClassroomCode.Location = new System.Drawing.Point(313, 39);
            this.txtClassroomCode.Name = "txtClassroomCode";
            this.txtClassroomCode.Size = new System.Drawing.Size(121, 25);
            this.txtClassroomCode.TabIndex = 4;
            this.txtClassroomCode.Visible = false;
            // 
            // lblClassroomCode
            // 
            this.lblClassroomCode.AutoSize = true;
            // 
            // 
            // 
            this.lblClassroomCode.BackgroundStyle.Class = "";
            this.lblClassroomCode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblClassroomCode.Location = new System.Drawing.Point(234, 41);
            this.lblClassroomCode.Name = "lblClassroomCode";
            this.lblClassroomCode.Size = new System.Drawing.Size(0, 0);
            this.lblClassroomCode.TabIndex = 3;
            this.lblClassroomCode.Text = "場地代碼：";
            this.lblClassroomCode.Visible = false;
            // 
            // contextMenuStripDelete
            // 
            this.contextMenuStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.contextMenuStripDelete.Name = "contextMenuStrip1";
            this.contextMenuStripDelete.ShowImageMargin = false;
            this.contextMenuStripDelete.Size = new System.Drawing.Size(70, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(69, 22);
            this.toolStripMenuItem2.Text = "刪除";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn1.HeaderText = "星期";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.ToolTipText = "只能輸入1到7";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn2.HeaderText = "開始時間";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn3.HeaderText = "結束時間";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "不排課描述";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // colSubject
            // 
            this.colSubject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSubject.HeaderText = "科目";
            this.colSubject.Name = "colSubject";
            // 
            // colLevel
            // 
            this.colLevel.HeaderText = "級別";
            this.colLevel.Name = "colLevel";
            // 
            // ClassroomEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "ClassroomEditor";
            this.Size = new System.Drawing.Size(530, 434);
            this.panelEx1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTimeTableBusyEditor)).EndInit();
            this.contextMenuBusy.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControlPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdClassroomBusys)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStripDelete.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn colSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLevel;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtClassroomCode;
        private DevComponents.DotNetBar.LabelX lblClassroomCode;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.LabelX lblClassroomDesc;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCapacity;
        private DevComponents.DotNetBar.LabelX lblCapacity;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkLocationOnly;
        private DevComponents.DotNetBar.LabelX lblLocation;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbLocation;
        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.Controls.DataGridViewX grdClassroomBusys;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.ContextMenuStrip contextMenuBusy;
        private System.Windows.Forms.ToolStripMenuItem menuBusy;
        private System.Windows.Forms.ToolStripMenuItem menuFree;
        private System.Windows.Forms.Panel panel3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTimeTables;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeekDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartHour;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartMinute;
        private System.Windows.Forms.DataGridViewComboBoxColumn colWeekFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBusyDesc;
        private DevComponents.DotNetBar.Controls.DataGridViewX grdTimeTableBusyEditor;
        private System.Windows.Forms.ToolStripMenuItem menuBusyDesc;
        private DevComponents.DotNetBar.LabelX lblName;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX4;
    }
}
