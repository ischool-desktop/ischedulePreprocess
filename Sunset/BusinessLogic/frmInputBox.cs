using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sunset
{
    public partial class frmInputBox : FISCA.Presentation.Controls.BaseForm
    {
        public frmInputBox(string TitleText)
        {
            InitializeComponent();

            this.TitleText = TitleText;
        }

        public frmInputBox(string TitleText,string Message)
        {
            InitializeComponent();

            this.TitleText = TitleText;
            txtMessage.Text = Message;
        }

        public string Message { get { return txtMessage.Text; } }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}