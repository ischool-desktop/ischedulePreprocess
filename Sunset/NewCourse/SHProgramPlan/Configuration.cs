using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SmartSchool.Common;
using DevComponents.DotNetBar;
using SmartSchool.Customization.PlugIn.Configure;
using FISCA.Presentation;

namespace SmartSchool.Configure
{
    partial class Configuration : UserControl,INCPanel, IEntity,IConfigurationManager
    {
        //private static Configuration _Instance;

        //public static Configuration Instance
        //{
        //    get
        //    {
        //        if (_Instance == null)
        //        {
        //            _Instance = new Configuration();
        //            SmartSchool.Customization.PlugIn.Configure.SystemConfiguration.SetManager(_Instance);
        //        }
        //        return _Instance;
        //    }
        //}

        private List<IConfigurationItem> _ActivedItems = new List<IConfigurationItem>();

        private IConfigurationItem _SelectedItem;

        private Dictionary<string, TreeNode> _CategoryNode = new Dictionary<string, TreeNode>();

        private string tabGroup = "設定";

        public  Configuration(string tabGroupName)
        {
            InitializeComponent();
            tabGroup = tabGroupName;
        }

        #region IEntity 成員

        public string Title
        {
            get { return tabGroup; }
        }

        public DevComponents.DotNetBar.NavigationPanePanel NavPanPanel
        {
            get { return navigationPanePanel1; }
        }

        public Panel ContentPanel
        {
            get { return panelContant; }
        }

        public Image Picture
        {
            get { return Sunset.Properties.Resources.偷來改的齒輪; }
        }

        public void Actived()
        {

        }

        #endregion

        private  void AddItem(IConfigurationItem  item)
        {
            TreeNode newNode = new TreeNode();
            if (item.Image != null)
                newNode.ImageIndex = imageList1.Images.Add(item.Image,Color.White);
            newNode.Text = item.Caption;
            newNode.Tag = item;
            newNode.ToolTipText= item.Caption;
            if (item.Category == "")
                dragDropTreeView1.Nodes.Add(newNode);
            else
            {
                TreeNode parentNode;
                if (_CategoryNode.ContainsKey(item.Category))
                    parentNode = _CategoryNode[item.Category];
                else
                {
                    parentNode = new TreeNode();
                    parentNode.Text=item.Category;
                    dragDropTreeView1.Nodes.Add(parentNode);
                    _CategoryNode.Add(item.Category, parentNode);
                }
                parentNode.Nodes.Add(newNode);
                parentNode.Expand();
            }
        }

        private void dragDropTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }
        //private void newButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    ButtonItem checkedItem = (ButtonItem)sender;
        //    ConfigurationItem item = (ConfigurationItem)checkedItem.Tag;
        //    if (!_ActivedItems.Contains(item))
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //        item.ContentPanel.Dock = DockStyle.Fill;
        //        this.panelContant.Controls.Add(item.ContentPanel);
        //        if (item.HasControlPanel)
        //        {
        //            item.ControlPanel.Dock = DockStyle.Fill;
        //            tabControlPanel2.Controls.Add(item.ControlPanel);
        //        }
        //        item.Active();
        //        _ActivedItems.Add(item);
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //    }
        //    if (item.ContentPanel.Visible != checkedItem.Checked)
        //        item.ContentPanel.Visible = checkedItem.Checked;
        //    if (item.HasControlPanel)
        //    {
        //        tabItem.Text = item.Caption;
        //        if (item.ControlPanel.Visible != checkedItem.Checked)
        //            item.ControlPanel.Visible = checkedItem.Checked;
        //    }
        //    if (tabItem.Visible != (checkedItem.Checked && item.HasControlPanel))
        //    {
        //        tabItem.Visible = (checkedItem.Checked && item.HasControlPanel);
        //    }
        //    tabControl1.SelectedTab = (tabItem.Visible ? tabItem : tabMain);
        //}

        #region IConfigurationManager 成員

        public void AddConfigurationItem(IConfigurationItem configurationItem)
        {
            this.AddItem(configurationItem);
        }

        #endregion

        private void dragDropTreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ( e.Button != MouseButtons.Left )
                return;
            dragDropTreeView1.SelectedNode = e.Node;
            if ( e.Node.Tag is IConfigurationItem )
            {
                this.panelContant.SuspendLayout();
                if ( _SelectedItem != null )
                {
                    _SelectedItem.ContentPanel.Visible = false;
                    if ( _SelectedItem.HasControlPanel )
                    {
                        _SelectedItem.ControlPanel.Visible = false;
                    }
                    tabItem.Visible = false;
                }
                TreeNode selectItem = e.Node;
                IConfigurationItem item = (IConfigurationItem)selectItem.Tag;
                #region 如果沒有載入過則載入
                if ( !_ActivedItems.Contains(item) )
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    item.ContentPanel.Visible = false;
                    item.ContentPanel.Dock = DockStyle.Fill;
                    item.ContentPanel.Size = this.panelContant.Size;
                    this.panelContant.Controls.Add(item.ContentPanel);
                    if ( item.HasControlPanel )
                    {
                        item.ControlPanel.Visible = false;
                        item.ControlPanel.Dock = DockStyle.Fill;
                        tabControlPanel2.Controls.Add(item.ControlPanel);
                    }
                    item.Active();
                    _ActivedItems.Add(item);
                    //MotherForm.Instance.SetStyle();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                #endregion
                //item.ContentPanel.SuspendLayout();
                item.ContentPanel.Visible = true;
                //item.ContentPanel.ResumeLayout(false);
                //item.ContentPanel.PerformLayout();
                if ( item.HasControlPanel )
                {
                    //item.ContentPanel.SuspendLayout();
                    tabItem.Text = item.Caption;
                    item.ControlPanel.Visible = true;
                    tabItem.Visible = true;
                    //item.ContentPanel.ResumeLayout(false);
                }
                tabControl1.SelectedTab = ( tabItem.Visible ? tabItem : tabMain );
                _SelectedItem = item;

                this.panelContant.ResumeLayout(true);
            }
            else
            {
                dragDropTreeView1.SelectedNode = e.Node.Nodes[0];
            }
        }

        #region NavContentDivision 成員

        public string Group
        {
            get { return tabGroup; }
        }

        public Control NavigationPane
        {
            get { return navigationPanePanel1; }
        }

        public Control ContentPane
        {
            get { return panelContant; }
        }

        #endregion
    }
}
