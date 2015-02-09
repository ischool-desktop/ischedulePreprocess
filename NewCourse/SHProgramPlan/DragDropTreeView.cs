using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SmartSchool.Common
{
    public class DragDropTreeView:TreeView
    {
        private TreeNode _DragOverNode;
        private Color _DragOverNodeColor;
        private Timer _DragOverTimer;
        private DevComponents.DotNetBar.SuperTooltip _SuperTooltip;
        private Dictionary<TreeNode, NodeSuperTooltipProvider> _TreeNodeToopTipProviders;
        private TreeNode _SelectedNode;

        public DragDropTreeView()
        {
            _DragOverTimer = new Timer();
            _DragOverTimer.Interval = 1500;
            _DragOverTimer.Tick += new EventHandler(_DragOverTimer_Tick);
            _SuperTooltip = new DevComponents.DotNetBar.SuperTooltip();
            _SuperTooltip.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            _SuperTooltip.ShowTooltipDescription = false;
            _SuperTooltip.TooltipDuration = 0;
            _SuperTooltip.MinimumTooltipSize = new Size(1, 1);
            _TreeNodeToopTipProviders = new Dictionary<TreeNode, NodeSuperTooltipProvider>();
            AllowDrop = true;
            DragOver += new DragEventHandler(DragDropTreeView_DragOver);
            DragDrop += new DragEventHandler(DragDropTreeView_DragDrop);
            DragLeave += new EventHandler(DragDropTreeView_DragLeave);
            BeforeSelect += new TreeViewCancelEventHandler(DragDropTreeView_BeforeSelect);
            this.FontChanged += new EventHandler(DragDropTreeView_FontChanged);
            Application.Idle += new EventHandler(Application_Idle);
        }

        protected override void Dispose(bool disposing)
        {
            Application.Idle -= new EventHandler(Application_Idle);
            _DragOverTimer.Tick -= new EventHandler(_DragOverTimer_Tick);
            DragOver -= new DragEventHandler(DragDropTreeView_DragOver);
            DragDrop -= new DragEventHandler(DragDropTreeView_DragDrop);
            DragLeave -= new EventHandler(DragDropTreeView_DragLeave);
            BeforeSelect -= new TreeViewCancelEventHandler(DragDropTreeView_BeforeSelect);
            this.FontChanged -= new EventHandler(DragDropTreeView_FontChanged);
            if ( _DragOverTimer.Enabled )
                _DragOverTimer.Stop();
            base.Dispose(disposing);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            CheckNodeColors(this.Nodes);
        }

        private void CheckNodeColors(TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode node in treeNodeCollection)
            {
                if (node != this.SelectedNode)
                {
                    node.BackColor = this.BackColor;
                    node.ForeColor = this.ForeColor;
                }
                else
                {
                    node.BackColor = Color.CornflowerBlue;
                    node.ForeColor = Color.White;
                }
                CheckNodeColors(node.Nodes);
            }
        }

        void DragDropTreeView_FontChanged(object sender, EventArgs e)
        {
            _SuperTooltip.DefaultFont = new Font(this.Font, FontStyle.Regular);
        }

        private void _DragOverTimer_Tick(object sender, EventArgs e)
        {
            _DragOverNode.Expand();
            _DragOverTimer.Stop();
        }

        private void DragDropTreeView_DragLeave(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            if (_DragOverNode != null)
            {
                _DragOverNode.ForeColor = _DragOverNodeColor;
                _DragOverTimer.Stop();
                if (_TreeNodeToopTipProviders.ContainsKey(_DragOverNode))
                {
                    _TreeNodeToopTipProviders[_DragOverNode].Hide();
                }
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (_SelectedNode != e.Node)
            {
                _SelectedNode = e.Node;
                base.OnAfterSelect(e);
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            if (_SelectedNode != e.Node)
                base.OnBeforeSelect(e);
        }

        private void DragDropTreeView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode treeNode = this.GetNodeAt(this.PointToClient(new System.Drawing.Point(e.X, e.Y)));
            if (treeNode != null && treeNode is DragDropTreeNode)
            {
                ((DragDropTreeNode)treeNode).DragDrop(e.Data, e.KeyState);
            }
            Reset();
        }

        private void DragDropTreeView_DragOver(object sender, DragEventArgs e)
        {
            Point DragPoint = this.PointToClient(new System.Drawing.Point(e.X, e.Y));
            TreeNode treeNode = this.GetNodeAt(DragPoint);
            if (treeNode != null)
            {
                if (treeNode is DragDropTreeNode)
                {
                    e.Effect = ((DragDropTreeNode)treeNode).CheckDragDropEffects(e.Data, e.KeyState);
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
                //如果選取節點改變
                if (treeNode != _DragOverNode)
                {
                    //回復前一節點狀態
                    Reset();
                    //設定目前結點狀態
                    #region 如果是DragDropTreeNode而且有DragOverMessage且允許拖曳
                    if (treeNode is DragDropTreeNode && ((DragDropTreeNode)treeNode).DragOverMessage != "" && e.Effect != DragDropEffects.None)
                    {
                        NodeSuperTooltipProvider sp;
                        if (!_TreeNodeToopTipProviders.ContainsKey(treeNode))
                        {
                            sp = new NodeSuperTooltipProvider(treeNode);
                            _TreeNodeToopTipProviders.Add(treeNode, sp);
                            _SuperTooltip.SetSuperTooltip(sp, new DevComponents.DotNetBar.SuperTooltipInfo(((DragDropTreeNode)treeNode).DragOverMessage, "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Lemon));
                        }
                        else
                        {
                            sp = _TreeNodeToopTipProviders[treeNode];
                        }
                        sp.Show();
                    }
                    #endregion
                    _DragOverNode = treeNode;
                    _DragOverNodeColor = _DragOverNode.ForeColor;
                    _DragOverNode.ForeColor = Color.Red;
                    _DragOverTimer.Start();
                }
            }
            #region 上下捲
            if ((Height - DragPoint.Y) < Height * 0.15)
            {
                //0x115垂直捲動
                //0x1往下
                //詳情請見http://boynd.muicc.com/vb/API/messageapi.htm
                //千萬不要問我
                SendMessage(Handle, 0x115, 0x1, 0);
            }
            if (DragPoint.Y < Height * 0.15)
            {
                //0x115垂直捲動
                //0x0往上
                //詳情請見http://boynd.muicc.com/vb/API/messageapi.htm
                //千萬不要問我
                SendMessage(Handle, 0x115, 0x0, 0);
            } 
            #endregion
        }

        /// <summary>
        /// 當選取新結點時，先還原舊節點的Style並改變新節點的Style
        /// </summary>
        private void DragDropTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.SelectedNode != null)
            {
                this.SelectedNode.BackColor = this.BackColor;
                this.SelectedNode.ForeColor = this.ForeColor;
            }
            e.Node.BackColor = Color.CornflowerBlue;
            e.Node.ForeColor = Color.White;
        }

        [DllImport("user32", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr Hwnd, int wMsg, int wParam, int lParam);

    }
    /// <summary>
    /// Wrapper so SuperTooltips can be displayed for node objects. Copy From DotNetBarSample_SuperToolTip
    /// </summary>
    public class NodeSuperTooltipProvider : Component, DevComponents.DotNetBar.ISuperTooltipInfoProvider
    {
        private TreeNode m_Node = null;

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="node">Node to provide tooltip information for</param>
        public NodeSuperTooltipProvider(TreeNode node)
        {
            m_Node = node;
        }

        /// <summary>
        /// Call this method to show tooltip for given node.
        /// </summary>
        public void Show()
        {
            if (this.DisplayTooltip != null)
                DisplayTooltip(this, new EventArgs());
        }

        /// <summary>
        /// Call this method to hide tooltip for given node.
        /// </summary>
        public void Hide()
        {
            if (this.HideTooltip != null)
                this.HideTooltip(this, new EventArgs());
        }

        #region ISuperTooltipInfoProvider Members

        /// <summary>
        /// Returns screen coordinates of object.
        /// </summary>
        public System.Drawing.Rectangle ComponentRectangle
        {
            get
            {
                Rectangle r = m_Node.Bounds;
                r.Location = m_Node.TreeView.PointToScreen(r.Location);
                return r;
            }
        }

        public event EventHandler DisplayTooltip;
        public event EventHandler HideTooltip;

        #endregion
    }
}
