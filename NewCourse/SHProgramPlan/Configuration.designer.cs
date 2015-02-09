namespace SmartSchool.Configure
{
    partial class Configuration
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuration));
            this.navigationPane1 = new DevComponents.DotNetBar.NavigationPane();
            this.navigationPanePanel1 = new DevComponents.DotNetBar.NavigationPanePanel();
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.dragDropTreeView1 = new SmartSchool.Common.DragDropTreeView();
            this.tabMain = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItem = new DevComponents.DotNetBar.TabItem(this.components);
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
            this.panelContant = new DevComponents.DotNetBar.PanelEx();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.navigationPane1.SuspendLayout();
            this.navigationPanePanel1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.tabControl1 ) ).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationPane1
            // 
            this.navigationPane1.Controls.Add(this.navigationPanePanel1);
            this.navigationPane1.Dock = System.Windows.Forms.DockStyle.Left;
            this.navigationPane1.ItemPaddingBottom = 2;
            this.navigationPane1.ItemPaddingTop = 2;
            this.navigationPane1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.navigationPane1.Location = new System.Drawing.Point(0, 0);
            this.navigationPane1.Margin = new System.Windows.Forms.Padding(4);
            this.navigationPane1.Name = "navigationPane1";
            this.navigationPane1.NavigationBarHeight = 67;
            this.navigationPane1.Padding = new System.Windows.Forms.Padding(1);
            this.navigationPane1.Size = new System.Drawing.Size(184, 630);
            this.navigationPane1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.navigationPane1.TabIndex = 0;
            // 
            // 
            // 
            this.navigationPane1.TitlePanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.navigationPane1.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.navigationPane1.TitlePanel.Font = new System.Drawing.Font(SmartSchool.Common.FontStyles.GeneralFontFamily, 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ));
            this.navigationPane1.TitlePanel.Location = new System.Drawing.Point(1, 1);
            this.navigationPane1.TitlePanel.Margin = new System.Windows.Forms.Padding(4);
            this.navigationPane1.TitlePanel.Name = "panelTitle";
            this.navigationPane1.TitlePanel.Size = new System.Drawing.Size(182, 34);
            this.navigationPane1.TitlePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.navigationPane1.TitlePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.navigationPane1.TitlePanel.Style.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.navigationPane1.TitlePanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.navigationPane1.TitlePanel.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.navigationPane1.TitlePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.navigationPane1.TitlePanel.Style.GradientAngle = 90;
            this.navigationPane1.TitlePanel.Style.MarginLeft = 4;
            this.navigationPane1.TitlePanel.TabIndex = 0;
            this.navigationPane1.TitlePanel.Text = "設定";
            // 
            // navigationPanePanel1
            // 
            this.navigationPanePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.navigationPanePanel1.Controls.Add(this.tabControl1);
            this.navigationPanePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanePanel1.Location = new System.Drawing.Point(1, 35);
            this.navigationPanePanel1.Margin = new System.Windows.Forms.Padding(4);
            this.navigationPanePanel1.Name = "navigationPanePanel1";
            this.navigationPanePanel1.ParentItem = this.buttonItem1;
            this.navigationPanePanel1.Size = new System.Drawing.Size(182, 527);
            this.navigationPanePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.navigationPanePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.navigationPanePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.navigationPanePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.navigationPanePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.navigationPanePanel1.Style.GradientAngle = 90;
            this.navigationPanePanel1.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.BackColor = System.Drawing.Color.Transparent;
            this.tabControl1.CanReorderTabs = false;
            this.tabControl1.CloseButtonOnTabsAlwaysDisplayed = false;
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Controls.Add(this.tabControlPanel2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font(SmartSchool.Common.FontStyles.GeneralFontFamily, 9.75F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(182, 527);
            this.tabControl1.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Dock;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.MultilineNoNavigationBox;
            this.tabControl1.Tabs.Add(this.tabMain);
            this.tabControl1.Tabs.Add(this.tabItem);
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.dragDropTreeView1);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 28);
            this.tabControlPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(182, 499);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(( (int)( ( (byte)( 253 ) ) ) ), ( (int)( ( (byte)( 253 ) ) ) ), ( (int)( ( (byte)( 254 ) ) ) ));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(( (int)( ( (byte)( 157 ) ) ) ), ( (int)( ( (byte)( 188 ) ) ) ), ( (int)( ( (byte)( 227 ) ) ) ));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(( (int)( ( (byte)( 146 ) ) ) ), ( (int)( ( (byte)( 165 ) ) ) ), ( (int)( ( (byte)( 199 ) ) ) ));
            this.tabControlPanel1.Style.BorderSide = ( (DevComponents.DotNetBar.eBorderSide)( ( ( DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right )
                        | DevComponents.DotNetBar.eBorderSide.Bottom ) ) );
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabMain;
            // 
            // dragDropTreeView1
            // 
            this.dragDropTreeView1.AllowDrop = true;
            this.dragDropTreeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dragDropTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dragDropTreeView1.Location = new System.Drawing.Point(1, 1);
            this.dragDropTreeView1.Name = "dragDropTreeView1";
            this.dragDropTreeView1.ShowNodeToolTips = true;
            this.dragDropTreeView1.Size = new System.Drawing.Size(180, 497);
            this.dragDropTreeView1.TabIndex = 0;
            this.dragDropTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.dragDropTreeView1_AfterSelect);
            this.dragDropTreeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.dragDropTreeView1_NodeMouseClick);
            // 
            // tabMain
            // 
            this.tabMain.AttachedControl = this.tabControlPanel1;
            this.tabMain.Name = "tabMain";
            this.tabMain.Text = "項目";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 28);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(182, 499);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(( (int)( ( (byte)( 253 ) ) ) ), ( (int)( ( (byte)( 253 ) ) ) ), ( (int)( ( (byte)( 254 ) ) ) ));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(( (int)( ( (byte)( 157 ) ) ) ), ( (int)( ( (byte)( 188 ) ) ) ), ( (int)( ( (byte)( 227 ) ) ) ));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(( (int)( ( (byte)( 146 ) ) ) ), ( (int)( ( (byte)( 165 ) ) ) ), ( (int)( ( (byte)( 199 ) ) ) ));
            this.tabControlPanel2.Style.BorderSide = ( (DevComponents.DotNetBar.eBorderSide)( ( ( DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right )
                        | DevComponents.DotNetBar.eBorderSide.Bottom ) ) );
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabItem;
            // 
            // tabItem
            // 
            this.tabItem.AttachedControl = this.tabControlPanel2;
            this.tabItem.Name = "tabItem";
            this.tabItem.Text = "tabItem1";
            this.tabItem.Visible = false;
            // 
            // buttonItem1
            // 
            this.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem1.Checked = true;
            this.buttonItem1.Image = ( (System.Drawing.Image)( resources.GetObject("buttonItem1.Image") ) );
            this.buttonItem1.ImagePaddingHorizontal = 8;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.OptionGroup = "navBar";
            this.buttonItem1.Text = "設定";
            // 
            // buttonItem2
            // 
            this.buttonItem2.ImagePaddingHorizontal = 8;
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.Text = "buttonItem2";
            // 
            // buttonItem3
            // 
            this.buttonItem3.ImagePaddingHorizontal = 8;
            this.buttonItem3.Name = "buttonItem3";
            this.buttonItem3.Text = "buttonItem3";
            // 
            // panelContant
            // 
            this.panelContant.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelContant.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelContant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContant.Location = new System.Drawing.Point(184, 0);
            this.panelContant.Name = "panelContant";
            this.panelContant.Size = new System.Drawing.Size(617, 630);
            this.panelContant.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelContant.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelContant.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelContant.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelContant.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelContant.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelContant.Style.GradientAngle = 90;
            this.panelContant.TabIndex = 1;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContant);
            this.Controls.Add(this.navigationPane1);
            this.Font = new System.Drawing.Font(SmartSchool.Common.FontStyles.GeneralFontFamily, 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 136 ) ));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Configuration";
            this.Size = new System.Drawing.Size(801, 630);
            this.navigationPane1.ResumeLayout(false);
            this.navigationPanePanel1.ResumeLayout(false);
            ( (System.ComponentModel.ISupportInitialize)( this.tabControl1 ) ).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.NavigationPane navigationPane1;
        private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabMain;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabItem;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.ButtonItem buttonItem3;
        private DevComponents.DotNetBar.PanelEx panelContant;
        private SmartSchool.Common.DragDropTreeView dragDropTreeView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}
