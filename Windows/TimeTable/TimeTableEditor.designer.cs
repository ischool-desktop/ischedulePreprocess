namespace Sunset
{
    partial class TimeTableEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grdTimeTableSecs = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colWeekDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDisableMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblName = new DevComponents.DotNetBar.LabelX();
            this.txtTimeTableDesc = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblTimeTableDesc = new DevComponents.DotNetBar.LabelX();
            this.contextMenuStripDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelEx1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTimeTableSecs)).BeginInit();
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
            this.panel2.Controls.Add(this.grdTimeTableSecs);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(530, 396);
            this.panel2.TabIndex = 4;
            // 
            // grdTimeTableSecs
            // 
            this.grdTimeTableSecs.BackgroundColor = System.Drawing.Color.White;
            this.grdTimeTableSecs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTimeTableSecs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWeekDay,
            this.colPeriod,
            this.colStartTime,
            this.colEndTime,
            this.colEnable,
            this.colDisableMessage});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdTimeTableSecs.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdTimeTableSecs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTimeTableSecs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdTimeTableSecs.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdTimeTableSecs.HighlightSelectedColumnHeaders = false;
            this.grdTimeTableSecs.Location = new System.Drawing.Point(0, 0);
            this.grdTimeTableSecs.MultiSelect = false;
            this.grdTimeTableSecs.Name = "grdTimeTableSecs";
            this.grdTimeTableSecs.RowHeadersWidth = 35;
            this.grdTimeTableSecs.RowTemplate.Height = 24;
            this.grdTimeTableSecs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdTimeTableSecs.Size = new System.Drawing.Size(530, 396);
            this.grdTimeTableSecs.TabIndex = 2;
            this.grdTimeTableSecs.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdTimeTableSecs_CellEnter);
            this.grdTimeTableSecs.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdTimeTableSecs_CellMouseClick);
            this.grdTimeTableSecs.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdTimeTableSecs_CurrentCellDirtyStateChanged);
            this.grdTimeTableSecs.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdTimeTableSecs_UserDeletingRow);
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
            // colPeriod
            // 
            this.colPeriod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colPeriod.HeaderText = "節次";
            this.colPeriod.Name = "colPeriod";
            this.colPeriod.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPeriod.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPeriod.ToolTipText = "只能輸入0到20，其中0為中午節次";
            this.colPeriod.Width = 40;
            // 
            // colStartTime
            // 
            this.colStartTime.HeaderText = "開始時間";
            this.colStartTime.Name = "colStartTime";
            this.colStartTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStartTime.ToolTipText = "24小時制，例如8:00";
            this.colStartTime.Width = 80;
            // 
            // colEndTime
            // 
            this.colEndTime.HeaderText = "結束時間";
            this.colEndTime.Name = "colEndTime";
            this.colEndTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEndTime.ToolTipText = "24小時制，例如9:00";
            this.colEndTime.Width = 80;
            // 
            // colEnable
            // 
            this.colEnable.HeaderText = "不排課";
            this.colEnable.Name = "colEnable";
            this.colEnable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEnable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colEnable.Width = 74;
            // 
            // colDisableMessage
            // 
            this.colDisableMessage.HeaderText = "不排課訊息";
            this.colDisableMessage.Name = "colDisableMessage";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.txtTimeTableDesc);
            this.panel1.Controls.Add(this.lblTimeTableDesc);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 38);
            this.panel1.TabIndex = 3;
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
            this.lblName.Location = new System.Drawing.Point(6, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(0, 0);
            this.lblName.TabIndex = 5;
            // 
            // txtTimeTableDesc
            // 
            this.txtTimeTableDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtTimeTableDesc.Border.Class = "TextBoxBorder";
            this.txtTimeTableDesc.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTimeTableDesc.Location = new System.Drawing.Point(394, 7);
            this.txtTimeTableDesc.Name = "txtTimeTableDesc";
            this.txtTimeTableDesc.Size = new System.Drawing.Size(131, 25);
            this.txtTimeTableDesc.TabIndex = 4;
            this.txtTimeTableDesc.Visible = false;
            // 
            // lblTimeTableDesc
            // 
            this.lblTimeTableDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeTableDesc.AutoSize = true;
            // 
            // 
            // 
            this.lblTimeTableDesc.BackgroundStyle.Class = "";
            this.lblTimeTableDesc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTimeTableDesc.Location = new System.Drawing.Point(304, 9);
            this.lblTimeTableDesc.Name = "lblTimeTableDesc";
            this.lblTimeTableDesc.Size = new System.Drawing.Size(87, 21);
            this.lblTimeTableDesc.TabIndex = 3;
            this.lblTimeTableDesc.Text = "時間表描述：";
            this.lblTimeTableDesc.Visible = false;
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
            this.dataGridViewTextBoxColumn2.HeaderText = "節次";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.ToolTipText = "只能輸入0到20，其中0為中午節次";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "開始時間";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.ToolTipText = "24小時制，例如8:00";
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "結束時間";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.ToolTipText = "24小時制，例如9:00";
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "不排課訊息";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
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
            // TimeTableEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "TimeTableEditor";
            this.Size = new System.Drawing.Size(530, 434);
            this.panelEx1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTimeTableSecs)).EndInit();
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
        private DevComponents.DotNetBar.Controls.TextBoxX txtTimeTableDesc;
        private DevComponents.DotNetBar.LabelX lblTimeTableDesc;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.Controls.DataGridViewX grdTimeTableSecs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeekDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisableMessage;
        private DevComponents.DotNetBar.LabelX lblName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}
