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
    public partial class frmAssignSplitSpec : FISCA.Presentation.Controls.BaseForm
    {
        int _period { get; set; }
        public frmAssignSplitSpec(int period)
        {
            InitializeComponent();

            _period = period;

            lbHelp1.Text = "所選課程節數為：" + _period.ToString();
        }

        public bool IsCreateCourseSection { get { return chkCreateCourseSection.Checked; } }

        public string SplitSpec { get { return txtSplitSpec.Text; } }

        private void txtSplitSpec_TextChanged(object sender, EventArgs e)
        {
            errSplitSpec.Clear();

            if (!string.IsNullOrEmpty(txtSplitSpec.Text))
            {
                int a;
                int TotalLen = 0;
                string[] Specs = txtSplitSpec.Text.Split(new char[] { ',' });

                for (int i = 0; i < Specs.Length; i++)
                {
                    //每一個值都必須是數字型態
                    if (!int.TryParse(Specs[i], out a))
                        errSplitSpec.SetError(txtSplitSpec, "必須為以逗號分隔的數字組合\n例如:4節之課程,可分割為[2,2]或[2,1,1]");
                    else
                        TotalLen += a;
                }

                if (TotalLen != _period)
                {
                    errSplitSpec.SetError(txtSplitSpec, "分割設定與節次不符!");
                }
            }
            else
            {
                //如果是空值,則提示輸入內容
                errSplitSpec.SetError(txtSplitSpec, "請輸入分割設定!");
            }


        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(errSplitSpec.GetError(txtSplitSpec)))
                MsgBox.Show("分割設定有誤\n請修正後再儲存!");
            else
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
