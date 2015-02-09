namespace SunsetCore
{
    partial class TreeNavViewBase
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
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
            this.ATree = new DevComponents.AdvTree.AdvTree();
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.node1 = new DevComponents.AdvTree.Node();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.CoreMenu = new DevComponents.DotNetBar.ButtonItem();
            this.btnRefreshAll = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ATree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // ATree
            // 
            this.ATree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.ATree.AllowDrop = true;
            this.ATree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.ATree.BackgroundStyle.Class = "TreeBorderKey";
            this.ATree.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ATree.Columns.Add(this.columnHeader1);
            this.ATree.ColumnsVisible = false;
            this.contextMenuBar1.SetContextMenuEx(this.ATree, this.CoreMenu);
            this.ATree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ATree.DragDropEnabled = false;
            this.ATree.ExpandWidth = 16;
            this.ATree.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.ATree.Location = new System.Drawing.Point(0, 0);
            this.ATree.Name = "ATree";
            this.ATree.NodesConnector = this.nodeConnector1;
            this.ATree.NodeStyle = this.elementStyle1;
            this.ATree.PathSeparator = ";";
            this.ATree.Size = new System.Drawing.Size(338, 514);
            this.ATree.Styles.Add(this.elementStyle1);
            this.ATree.TabIndex = 1;
            this.ATree.Text = "advTree1";
            this.ATree.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.Tree_AfterNodeSelect);
            this.ATree.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.Tree_NodeClick);
            this.ATree.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.Tree_NodeDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Width.Relative = 100;
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Class = "";
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.CoreMenu});
            this.contextMenuBar1.Location = new System.Drawing.Point(16, 11);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 27);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 2;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // CoreMenu
            // 
            this.CoreMenu.AutoExpandOnClick = true;
            this.CoreMenu.Name = "CoreMenu";
            this.CoreMenu.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnRefreshAll});
            this.CoreMenu.Text = "Menu";
            // 
            // btnRefreshAll
            // 
            this.btnRefreshAll.Name = "btnRefreshAll";
            this.btnRefreshAll.Text = "重新整理";
            this.btnRefreshAll.Tooltip = "強制重新讀取所有資料";
            this.btnRefreshAll.Click += new System.EventHandler(this.btnRefreshAll_Click);
            // 
            // TreeNavViewBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.contextMenuBar1);
            this.Controls.Add(this.ATree);
            this.Name = "TreeNavViewBase";
            this.Size = new System.Drawing.Size(338, 514);
            ((System.ComponentModel.ISupportInitialize)(this.ATree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.AdvTree ATree;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem CoreMenu;
        private DevComponents.DotNetBar.ButtonItem btnRefreshAll;
    }
}
