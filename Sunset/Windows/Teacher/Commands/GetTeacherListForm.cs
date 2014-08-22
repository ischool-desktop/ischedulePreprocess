using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.Presentation;

namespace Sunset
{
    public partial class GetTeacherListForm : BaseForm
    {
        public GetTeacherListForm()
        {
            InitializeComponent();
        }

        private void GetTeacherListForm_Load(object sender, EventArgs e)
        {
            btnClose.Click += (vsender, ve) => this.Close();

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            worker.RunWorkerAsync();
            TitleText = "複製教師清單(取得資料中...)";
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> result = new List<object>();

            List<TeacherEx> records = tool._A.Select<TeacherEx>();
            List<OBJ_Teacher> names = new List<OBJ_Teacher>();

            DataTable table = tool._Q.Select("select teacher_name,nickname from teacher where status=1 order by teacher_name,nickname");

            foreach (DataRow row in table.Rows)
            {
                OBJ_Teacher obj = new OBJ_Teacher();
                obj.TeacherName = row.Field<string>("teacher_name");
                obj.NickName = row.Field<string>("nickname");
                names.Add(obj);
            }

            result.Add(records);
            result.Add(names);

            e.Result = result;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<object> result = e.Result as List<object>;

            List<TeacherEx> records = (List<TeacherEx>)result[0];
            List<OBJ_Teacher> names = (List<OBJ_Teacher>)result[1];

            foreach (OBJ_Teacher ex in names)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(grdProgramPlanList);
                row.Cells[1].Value = ex.TeacherName;
                row.Cells[2].Value = ex.NickName;
                row.Tag = ex;
                grdProgramPlanList.Rows.Add(row);
                //比較names是否有相同名稱的內容
                if (records.Find(x => x.FullTeacherName.Equals(ex.FullTeacherName)) != null)
                {
                    DataGridViewCellStyle Style = row.DefaultCellStyle;
                    Style.BackColor = Color.Yellow;
                }
            }

            TitleText = "複製教師清單";
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            //儲存
            DialogResult dr = MsgBox.Show("您確認要複製所選教師?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dr == System.Windows.Forms.DialogResult.No)
            {
                MsgBox.Show("已取消...");
                return;
            }


            try
            {
                #region 取得所要複製的ischool班級清單

                List<OBJ_Teacher> SelectRows = new List<OBJ_Teacher>();

                foreach (DataGridViewRow row in grdProgramPlanList.SelectedRows)
                {
                    SelectRows.Add(row.Tag as OBJ_Teacher);
                }

                #endregion

                //取得排課教師
                List<TeacherEx> records = tool._A.Select<TeacherEx>();

                #region 判斷新增或更新班級

                List<TeacherEx> updaterecords = new List<TeacherEx>();
                List<TeacherEx> insertrecords = new List<TeacherEx>();

                foreach (OBJ_Teacher each in SelectRows)
                {
                    //取得清單內是否有重覆"班級名稱"的物件
                    TeacherEx srecord = records.Find(x => x.FullTeacherName.Equals(each.FullTeacherName));

                    if (srecord == null)
                    {
                        //新增
                        TeacherEx ex = new TeacherEx();
                        ex.TeacherName = each.TeacherName;
                        ex.NickName = each.NickName;
                        insertrecords.Add(ex);
                    }
                    else
                    {
                        //更新
                        srecord.NickName = each.NickName;
                        updaterecords.Add(srecord);
                    }
                }

                #endregion

                #region 開始新增或覆蓋

                StringBuilder log_sb = new StringBuilder();
                log_sb.AppendLine("複製「ischool教師」至「排課教師」：");

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("複製教師清單成功");

                if (insertrecords.Count > 0)
                {
                    sb.AppendLine("新增「" + insertrecords.Count + "」筆");
                    tool._A.InsertValues(insertrecords);

                    log_sb.AppendLine("新增清單「" + insertrecords.Count + "」");
                    foreach (TeacherEx each in insertrecords)
                    {
                        log_sb.AppendLine("教師名稱「" + each.TeacherName + "」暱稱「" + each.NickName + "」");
                    }
                }

                if (updaterecords.Count > 0)
                {
                    DialogResult update_dr = MsgBox.Show("共有" + updaterecords.Count + "筆教師資料已存在，是否覆蓋？", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                    if (update_dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        sb.AppendLine("覆蓋「" + updaterecords.Count + "」筆");
                        tool._A.UpdateValues(updaterecords);

                        log_sb.AppendLine("更新清單「" + updaterecords.Count + "」");
                        foreach (TeacherEx each in updaterecords)
                        {
                            log_sb.AppendLine("教師 " + each.TeacherName + " 已修改暱稱為「" + each.NickName + "」");
                        }
                    }
                }

                if (insertrecords.Count > 0 || updaterecords.Count > 0)
                {
                    FISCA.LogAgent.ApplicationLog.Log("排課", "匯入教師", log_sb.ToString());

                    MsgBox.Show(sb.ToString());
                }

                #endregion









            }
            catch (Exception ve)
            {
                FISCA.ErrorBox.Show("在複製過程中發生錯誤！", ve);
                MotherForm.SetStatusBarMessage("在複製過程中發生錯誤！");
                SmartSchool.ErrorReporting.ReportingService.ReportException(ve);
            }





        }

        private void grdProgramPlanList_SelectionChanged(object sender, EventArgs e)
        {
            btnCopy.Text = "複製所選(" + grdProgramPlanList.SelectedRows.Count + ")";
        }
    }

    class OBJ_Teacher
    {
        public string TeacherName { get; set; }
        public string NickName { get; set; }

        public string FullTeacherName
        {
            get
            {
                return string.IsNullOrEmpty(NickName) ? TeacherName : TeacherName + "(" + NickName + ")";
            }
        }
    }
}
