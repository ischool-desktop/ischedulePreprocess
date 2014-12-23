using System;
using System.Collections.Generic;

namespace Sunset
{
    public partial class frmComboBox : FISCA.Presentation.Controls.BaseForm
    {
        public frmComboBox(string TitleText,List<string> Items)
        {
            InitializeComponent();

            this.Text = TitleText;

            cmbSelector.Items.Clear();
            Items.ForEach(x => cmbSelector.Items.Add(x));
        }

        public string SelectResult
        {
            get { return "" + cmbSelector.SelectedItem; }
        }

        private void frmComboBox_Load(object sender, EventArgs e)
        {

        }
    }
}
