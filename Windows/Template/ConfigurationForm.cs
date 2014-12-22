using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sunset.Windows
{
    /// <summary>
    /// 最後呈現的設定表單
    /// </summary>
    public partial class ConfigurationForm : FISCA.Presentation.Controls.BaseForm, IConfigurationItem
    {
        private IConfigurationItem mConfigurationItem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vConfigurationItem"></param>
        public ConfigurationForm(IConfigurationItem vConfigurationItem)
        {
            InitializeComponent();
            mConfigurationItem = vConfigurationItem;

            int vHeight, vWidth;
            if (mConfigurationItem.HasControlPanel)
            {
                vHeight = Math.Max(vConfigurationItem.ControlPanel.Height, vConfigurationItem.ContentPanel.Height);
                vWidth = vConfigurationItem.ControlPanel.Width + vConfigurationItem.ContentPanel.Width;
            }
            else
            {
                vHeight = vConfigurationItem.ContentPanel.Height;
                vWidth = vConfigurationItem.ContentPanel.Width;
            }

            Size = new Size(vWidth, vHeight + 20);
        }

        /// <summary>
        /// 載入表單事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            SC.Visible = mConfigurationItem.HasControlPanel;
            if (!mConfigurationItem.HasControlPanel)
            {
                Controls.Add(mConfigurationItem.ContentPanel);
            }
            else
            {
                SC.Panel1.Controls.Add(mConfigurationItem.ControlPanel);
                SC.Panel2.Controls.Add(mConfigurationItem.ContentPanel);
                mConfigurationItem.ControlPanel.Dock = DockStyle.Fill;
            }
            mConfigurationItem.ContentPanel.Dock = DockStyle.Fill;

            Text = Caption;
            mConfigurationItem.Active();
        }

        #region IConfigurationItem 成員

        /// <summary>
        /// 
        /// </summary>
        public void Active()
        {
            ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Caption
        {
            get { return mConfigurationItem.Caption; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Category
        {
            get { return mConfigurationItem.Category; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Panel ContentPanel
        {
            get { return mConfigurationItem.ContentPanel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Panel ControlPanel
        {
            get { return mConfigurationItem.ControlPanel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool HasControlPanel
        {
            get { return mConfigurationItem.HasControlPanel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Image Image
        {
            get { return mConfigurationItem.Image; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TabGroup
        {
            get { return mConfigurationItem.TabGroup; }
        }

        #endregion
    }
}