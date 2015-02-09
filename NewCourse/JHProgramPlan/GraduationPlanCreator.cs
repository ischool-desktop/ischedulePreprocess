using System;
using System.Collections.Generic;
using DevComponents.Editors;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    public partial class GraduationPlanCreator : FISCA.Presentation.Controls.BaseForm
    {
        private SchedulerProgramPlan _copy_record = null;
        private List<SchedulerProgramPlan> mrecords = null;

        public GraduationPlanCreator()
        {
            InitializeComponent();

            cboExistPlanList.SelectedIndex = 0;

            AccessHelper helper = new AccessHelper();

            mrecords = helper.Select<SchedulerProgramPlan>();

            foreach (var record in mrecords)
            {
                ComboItem item = new ComboItem();
                item.Text = record.Name;
                item.Tag = record;
                cboExistPlanList.Items.Add(item);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewName.Text))
            {
                SchedulerProgramPlan editor = new SchedulerProgramPlan();
                editor.Name = txtNewName.Text;
                if (_copy_record != null)
                    editor.Content = _copy_record.Content;
                editor.Save();
                this.Close();
            }
            else
                this.Close();
        }

        private void cboExistPlanList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboExistPlanList.SelectedItem == comboItem1)
                _copy_record = null;
            else
                _copy_record = (SchedulerProgramPlan)((ComboItem)cboExistPlanList.SelectedItem).Tag;
        }

        private void txtNewName_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtNewName, "");
            btnSave.Enabled = true;
            if (string.IsNullOrEmpty(txtNewName.Text))
            {
                errorProvider1.SetError(txtNewName, "不可空白。");
                btnSave.Enabled = false;
                return;
            }
            foreach (var record in mrecords)
            {
                if (record.Name == txtNewName.Text)
                {
                    errorProvider1.SetError(txtNewName, "名稱不可重複。");
                    btnSave.Enabled = false;
                    return;
                }
            }
        }
    }
}