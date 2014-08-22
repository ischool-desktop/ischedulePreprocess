using System.Drawing;
using System.Windows.Forms;

namespace Sunset.Windows
{
    /// <summary>
    /// 系統設定介面
    /// </summary>
    public partial class ConfigurationItem : UserControl,IConfigurationItem
    {
        private string mTabGroup = string.Empty;
        private bool mHasControlPanel;
        private string mCaption = string.Empty;
        private string mCategory = string.Empty;
        private Image mImage = null;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ConfigurationItem()
        {
            InitializeComponent();
        }

        #region IConfigurationItem 成員
        /// <summary>
        /// 
        /// </summary>
        public string TabGroup
        {
            get { return mTabGroup; }
            set { mTabGroup = value; }
        }
        /// <summary>
        /// 擁有ControlPanel
        /// </summary>
        public bool HasControlPanel
        {
            get { return mHasControlPanel; }
            set { mHasControlPanel = value; }
        }
        
        /// <summary>
        /// 左方的控制項目
        /// </summary>
        public Panel ControlPanel
        {
            get { return pnlControl; }
        }

        /// <summary>
        /// 右方的內容項目
        /// </summary>
        public Panel ContentPanel
        {
            get { return pnlContent; }
        }

        /// <summary>
        /// 項目名稱
        /// </summary>
        public string Caption
        {
            get { return mCaption; }
            set { mCaption = value; }
        }

        /// <summary>
        /// 項目類別
        /// </summary>
        public string Category
        {
            get { return mCategory; }
            set { mCategory = value; }
        }

        /// <summary>
        /// 照片
        /// </summary>
        public Image Image
        {
            get { return mImage; }
            set { mImage = value; }
        }
        
        /// <summary>
        /// 啟動時呼叫
        /// </summary>
        public virtual void Active()
        {
            OnActive();
        }

        #endregion
        /// <summary>
        /// 當此項目第一次被開啟時
        /// </summary>
        protected virtual void OnActive()
        {
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            this.splitter1.Visible = this.pnlControl.Visible = mHasControlPanel;
            this.captionLabel.Text = mCategory + (mCategory == "" ? "" : "/") + mCaption;
            base.OnPaint(e);
        }
    }
}