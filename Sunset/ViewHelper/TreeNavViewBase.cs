using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Campus;
using DevComponents.AdvTree;
using FISCA.Data;
using FISCA.Presentation;
using K12.Data;
using System.Diagnostics;

namespace SunsetCore
{
    /// <summary>
    /// 提供 NavView 的基本功能。
    /// </summary>
    public partial class TreeNavViewBase : NavView
    {
        private Node Loading { get; set; }

        private KeyCatalogFactory KCFactory { get; set; }

        /// <summary>
        /// 是否顯示根項目。
        /// </summary>
        protected bool ShowRoot { get; set; }

        /// <summary>
        /// 根項目標題。
        /// </summary>
        protected string RootCaption { get; set; }

        protected TaskScheduler UISyncContext { get; set; }

        protected static QueryHelper Backend { get; set; }

        /// <summary>
        /// 建立年班檢視。
        /// </summary>
        public TreeNavViewBase()
        {
            InitializeComponent();

            NameComparer = new CustomStringComparer();
            KCFactory = new KeyCatalogFactory() { NameSorter = KeyCatalogComparer, ToStringFormatter = KeyCatalogTitleFormat };
            ShowRoot = true;
            RootCaption = "所有項目";
            Loading = new Node("讀取中...");
            UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();

            //當 Student.SetFilteredSource 被呼叫時。
            SourceChanged += new EventHandler(StudentGradeClassView_SourceChanged);

            try
            {//在設計模式下會爆炸，懶得處理了...
                if (Backend == null)
                    Backend = new FISCA.Data.QueryHelper();
            }
            catch { }
        }

        #region KeyCatalog 排序處理
        /// <summary>
        /// 排序 KeyCatalog。
        /// </summary>
        private CustomStringComparer NameComparer { get; set; }

        /// <summary>
        /// 排序 KeyCatalog，如果不改寫則使用 Name 屬性排序。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected virtual int KeyCatalogComparer(KeyCatalog x, KeyCatalog y)
        {
            return NameComparer.Compare(x.Name, y.Name);
        }

        /// <summary>
        /// 提供 KeyCatalog 標題的格式化方法。
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        protected virtual string KeyCatalogTitleFormat(KeyCatalog catalog)
        {
            return string.Format("{0}({1})", catalog.Name, catalog.TotalKeyCount);
        }
        #endregion

        private void StudentGradeClassView_SourceChanged(object sender, EventArgs e)
        {
            RenderTreeView(true);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void RenderTreeView()
        {
            RenderTreeView(true);
        }

        /// <summary>
        /// 產生資料到畫面上，請透過 GenerateDataModel 產生資料結構。
        /// </summary>
        protected void RenderTreeView(bool reserveSelection)
        {
            if (reserveSelection)
                ReserveTreeSelection();

            KeyCatalog userroot = new KeyCatalog(RootCaption, KCFactory);
            Task task = Task.Factory.StartNew((x) =>
            {
                GenerateTreeStruct(x as KeyCatalog);
            }, userroot);

            task.ContinueWith(x =>
            {
                ATree.Nodes.Clear();

                KeyCatalog rkc = x.AsyncState as KeyCatalog;

                if (ShowRoot)
                {
                    KeyCatalog root = new KeyCatalog("", KCFactory);
                    root.Subcatalogs.Add(rkc);
                    RenderNodes(root, ATree.Nodes, RestoreLevel);
                }
                else
                    RenderNodes(rkc, ATree.Nodes, RestoreLevel);

                foreach (Node n in ATree.Nodes)
                    n.Expand();

            }, UISyncContext);
        }

        /// <summary>
        /// 顯示「載入中...」節點。
        /// </summary>
        public void ShowLoading()
        {
            ReserveTreeSelection();
            ATree.Nodes.Clear();
            ATree.Nodes.Add(Loading);
        }

        /// <summary>
        /// 產生資料模型，將資料產生在 Root 屬性上。
        /// </summary>
        protected virtual void GenerateTreeStruct(KeyCatalog root)
        {
            throw new NotImplementedException("您應該實作此方法。");
        }

        /// <summary>
        /// 選擇的節點名稱集合，一個項目一個層次。
        /// </summary>
        private string SelectionNodeName = string.Empty;
        /// <summary>
        /// 目前還原到第幾層。
        /// </summary>
        private int RestoreLevel = 0;

        /// <summary>
        /// 保留目前在 TreeView 上的選擇項目。
        /// </summary>
        private void ReserveTreeSelection()
        {
            SelectionNodeName = string.Empty;
            RestoreLevel = 0;
            KeyNode kn = ATree.SelectedNode as KeyNode;

            if (kn == null) return; //如果選擇的不是 KeyNode 就不需要處理了。

            //記錄選擇的 Node 名稱與他的層級。
            SelectionNodeName = kn.Catalog.Name;
            RestoreLevel = kn.Level;
        }

        private void RenderNodes(KeyCatalog catalog, NodeCollection nodes, int restoreLevel)
        {
            restoreLevel--;
            foreach (KeyCatalog sub in catalog.Subcatalogs.SortedValues)
            {
                KeyNode n = new KeyNode(sub.ToString()) { Catalog = sub };
                nodes.Add(n);

                if (restoreLevel < 0)
                {
                    if (SelectionNodeName == sub.Name)
                        ATree.SelectedNode = n;
                }

                if (!sub.IsLeaf)
                    RenderNodes(sub, n.Nodes, restoreLevel);
            }
        }

        private void Tree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            SetListPanel(e.Node as KeyNode);
        }

        private void Tree_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            SetListPanel(e.Node as KeyNode);
        }

        private void Tree_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            SetListPanel(e.Node as KeyNode);
        }

        private void SetListPanel(KeyNode node)
        {
            if (node == null) return;

            try
            {
                bool selAll = (Control.ModifierKeys & Keys.Control) == Keys.Control;
                bool addToTemp = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
                SetListPaneSource(node.Catalog.AllKey, selAll, addToTemp);
            }
            catch (Exception) { }
        }

        private void btnRefreshAll_Click(object sender, EventArgs e)
        {
            ReserveTreeSelection();
            ShowLoading();
            CacheProvider.ClearAll();
            RenderTreeView(false);
        }
    }
}