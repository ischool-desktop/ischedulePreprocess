using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevComponents.Editors;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 建立課程規劃
    /// </summary>
    public partial class SHGraduationPlanCreator : FISCA.Presentation.Controls.BaseForm
    {
        private BackgroundWorker _BKWGraduationPlanLoader;
        private AccessHelper _helper;
        private string _CopyString;
        private List<SchedulerProgramPlan> _records;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public SHGraduationPlanCreator()
        {
            InitializeComponent();
            _CopyString = "<GraduationPlan/>";
            comboItem1.Tag = _CopyString;
            comboBoxEx1.SelectedIndex = 0;
            #region 取得課程規劃
            _BKWGraduationPlanLoader = new BackgroundWorker();
            _BKWGraduationPlanLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_BKWGraduationPlanLoader_RunWorkerCompleted);
            _BKWGraduationPlanLoader.DoWork += new DoWorkEventHandler(_BKWGraduationPlanLoader_DoWork);
            _BKWGraduationPlanLoader.RunWorkerAsync();
            #endregion
        }

        private void _BKWGraduationPlanLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = _helper.Select<SchedulerProgramPlan>();
        }

        private void _BKWGraduationPlanLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            comboBoxEx1.Items.Remove(comboItem2);
            List<SchedulerProgramPlan> _records = e.Result as List<SchedulerProgramPlan>;
            foreach (SchedulerProgramPlan gPlan in _records)
            {
                DevComponents.Editors.ComboItem item = new DevComponents.Editors.ComboItem();
                item.Text = gPlan.Name;
                item.Tag = gPlan.Content;
                comboBoxEx1.Items.Add(item);
            }
        }

        /// <summary>
        /// 關閉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (textBoxX1.Text != "")
            {
                SchedulerProgramPlan record = new SchedulerProgramPlan();
                record.Name = textBoxX1.Text;
                record.Content = _CopyString;
                record.Save();

                this.Close();
            }
            else
                this.Close();
        }


        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEx1.SelectedItem == comboItem2)
                comboBoxEx1.SelectedIndex = 0;
            else
            {
                _CopyString =""+((ComboItem)comboBoxEx1.SelectedItem).Tag;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBoxX1, "");
            buttonX1.Enabled = true;
            if ( textBoxX1.Text == "" )
            {
                errorProvider1.SetError(textBoxX1, "不可空白。");
                buttonX1.Enabled = false;
                return;
            }

            foreach (SchedulerProgramPlan gPlan in _records)
            {
                if ( gPlan.Name == textBoxX1.Text )
                {
                    errorProvider1.SetError(textBoxX1, "名稱不可重複。");
                    buttonX1.Enabled = false;
                    return;
                }
            }
        }
    }
}