using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace Sunset
{
    public partial class HelpForm : BaseForm
    {
        public HelpForm(string text1, string text2)
        {
            InitializeComponent();
            textBoxX1.Text = text1;
            textBoxX2.Text = text2;
        }
    }
} 
