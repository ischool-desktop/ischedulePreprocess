namespace Sunset.Windows
{
    partial class CampusConfiguration<T>
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.pnlExpandable = new DevComponents.DotNetBar.ExpandablePanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grdNameList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnInsert = new DevComponents.DotNetBar.ButtonX();
            this.btnUpdate = new DevComponents.DotNetBar.ButtonX();
            this.btnDelete = new DevComponents.DotNetBar.ButtonX();
            this.btnMore = new DevComponents.DotNetBar.ButtonX();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlContent.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.pnlExpandable.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNameList)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControl
            // 
            this.pnlControl.Size = new System.Drawing.Size(170, 616);
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.pnlEditor);
            this.pnlContent.Controls.Add(this.expandableSplitter1);
            this.pnlContent.Controls.Add(this.panelEx1);
            this.pnlContent.Location = new System.Drawing.Point(173, 19);
            this.pnlContent.Size = new System.Drawing.Size(798, 616);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.pnlExpandable);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(156, 616);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // pnlExpandable
            // 
            this.pnlExpandable.CanvasColor = System.Drawing.SystemColors.Control;
            this.pnlExpandable.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.pnlExpandable.Controls.Add(this.panel2);
            this.pnlExpandable.Controls.Add(this.panel1);
            this.pnlExpandable.Controls.Add(this.btnInsert);
            this.pnlExpandable.Controls.Add(this.btnUpdate);
            this.pnlExpandable.Controls.Add(this.btnDelete);
            this.pnlExpandable.Controls.Add(this.btnMore);
            this.pnlExpandable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExpandable.ExpandButtonVisible = false;
            this.pnlExpandable.Location = new System.Drawing.Point(0, 0);
            this.pnlExpandable.Name = "pnlExpandable";
            this.pnlExpandable.Size = new System.Drawing.Size(156, 616);
            this.pnlExpandable.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.pnlExpandable.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.pnlExpandable.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.pnlExpandable.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.pnlExpandable.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.pnlExpandable.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.pnlExpandable.Style.GradientAngle = 90;
            this.pnlExpandable.TabIndex = 4;
            this.pnlExpandable.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.pnlExpandable.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.pnlExpandable.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.pnlExpandable.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.pnlExpandable.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.pnlExpandable.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.pnlExpandable.TitleStyle.GradientAngle = 90;
            this.pnlExpandable.TitleText = "Title Bar";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grdNameList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(156, 469);
            this.panel2.TabIndex = 11;
            // 
            // grdNameList
            // 
            this.grdNameList.AllowUserToAddRows = false;
            this.grdNameList.AllowUserToDeleteRows = false;
            this.grdNameList.AllowUserToResizeColumns = false;
            this.grdNameList.AllowUserToResizeRows = false;
            this.grdNameList.BackgroundColor = System.Drawing.Color.White;
            this.grdNameList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grdNameList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.grdNameList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdNameList.ColumnHeadersVisible = false;
            this.grdNameList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdNameList.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdNameList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdNameList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdNameList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdNameList.HighlightSelectedColumnHeaders = false;
            this.grdNameList.Location = new System.Drawing.Point(0, 0);
            this.grdNameList.Name = "grdNameList";
            this.grdNameList.ReadOnly = true;
            this.grdNameList.RowHeadersVisible = false;
            this.grdNameList.RowTemplate.Height = 24;
            this.grdNameList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdNameList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdNameList.Size = new System.Drawing.Size(156, 469);
            this.grdNameList.TabIndex = 8;
            this.grdNameList.SelectionChanged += new System.EventHandler(this.grdNameList_SelectionChanged);
            this.grdNameList.DoubleClick += new System.EventHandler(this.grdNameList_DoubleClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "名稱";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(156, 29);
            this.panel1.TabIndex = 10;
            // 
            // txtSearch
            // 
            // 
            // 
            // 
            this.txtSearch.Border.Class = "TextBoxBorder";
            this.txtSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(156, 25);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnInsert
            // 
            this.btnInsert.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnInsert.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnInsert.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnInsert.Location = new System.Drawing.Point(0, 524);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(156, 23);
            this.btnInsert.TabIndex = 6;
            this.btnInsert.Text = "新增";
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpdate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUpdate.Location = new System.Drawing.Point(0, 547);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(156, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "儲存";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDelete.Location = new System.Drawing.Point(0, 570);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(156, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "刪除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnMore
            // 
            this.btnMore.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMore.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnMore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnMore.Location = new System.Drawing.Point(0, 593);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(156, 23);
            this.btnMore.TabIndex = 9;
            this.btnMore.Text = "      更多..";
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.Expandable = false;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(156, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(3, 616);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 1;
            this.expandableSplitter1.TabStop = false;
            // 
            // pnlEditor
            // 
            this.pnlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEditor.Location = new System.Drawing.Point(159, 0);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Size = new System.Drawing.Size(639, 616);
            this.pnlEditor.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "名稱";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "學分數";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            this.dataGridViewTextBoxColumn2.Width = 50;
            // 
            // CampusConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.Name = "CampusConfiguration";
            this.pnlContent.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.pnlExpandable.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdNameList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ExpandablePanel pnlExpandable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DevComponents.DotNetBar.ButtonX btnUpdate;
        private DevComponents.DotNetBar.ButtonX btnDelete;
        private DevComponents.DotNetBar.ButtonX btnMore;
        private DevComponents.DotNetBar.ButtonX btnInsert;
        private System.Windows.Forms.Panel pnlEditor;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSearch;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.Controls.DataGridViewX grdNameList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;

    }
}
