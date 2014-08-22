using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace Sunset.NewCourse
{
    public partial class TextInputeForm : BaseForm
    {

        public TextInputeForm(StringBuilder sb1)
        {
            InitializeComponent();

            labelX1.Text = sb1.ToString();

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
