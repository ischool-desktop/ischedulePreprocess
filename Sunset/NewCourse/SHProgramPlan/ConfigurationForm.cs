using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SmartSchool.Common;
using SmartSchool.Customization.PlugIn.Configure;
using FISCA.Presentation.Controls;

namespace Sunset.NewCourse
{
    public partial class ConfigurationForm : BaseForm, SmartSchool.Customization.PlugIn.Configure.IConfigurationItem
    {
        private IConfigurationItem Adapt;

        public ConfigurationForm(IConfigurationItem adapt)
        {
            InitializeComponent();
            Adapt = adapt;

            int height, width;
            if (Adapt.HasControlPanel)
            {
                height = Math.Max(adapt.ControlPanel.Height, adapt.ContentPanel.Height);
                width = adapt.ControlPanel.Width + adapt.ContentPanel.Width;
            }
            else
            {
                height = adapt.ContentPanel.Height;
                width = adapt.ContentPanel.Width;
            }

            Size = new Size(width, height + 20);
        }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            SC.Visible = Adapt.HasControlPanel;
            if (!Adapt.HasControlPanel)
            {
                Controls.Add(Adapt.ContentPanel);
            }
            else
            {
                SC.Panel1.Controls.Add(Adapt.ControlPanel);
                SC.Panel2.Controls.Add(Adapt.ContentPanel);
                Adapt.ControlPanel.Dock = DockStyle.Fill;
            }
            Adapt.ContentPanel.Dock = DockStyle.Fill;

            Text = Caption;
            Adapt.Active();
        }

        #region IConfigurationItem 成員

        public void Active()
        {
            ShowDialog();
        }

        public string Caption
        {
            get { return Adapt.Caption; }
        }

        public string Category
        {
            get { return Adapt.Category; }
        }

        public Panel ContentPanel
        {
            get { return Adapt.ContentPanel; }
        }

        public Panel ControlPanel
        {
            get { return Adapt.ControlPanel; }
        }

        public bool HasControlPanel
        {
            get { return Adapt.HasControlPanel; }
        }

        public Image Image
        {
            get { return Adapt.Image; }
        }

        public string TabGroup
        {
            get { return Adapt.TabGroup; }
        }

        #endregion
    }
}
