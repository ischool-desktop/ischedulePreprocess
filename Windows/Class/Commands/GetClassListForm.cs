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
    public partial class GetClassListForm : BaseForm
    {
        public GetClassListForm()
        {
            InitializeComponent();
        }

        private void GetClassListForm_Load(object sender, EventArgs e)
        {
            btnClose.Click += (vsender, ve) => this.Close();

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            worker.RunWorkerAsync();
            TitleText = "複製班級清單(取得資料中...)";
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> result = new List<object>();

            List<ClassEx> records = tool._A.Select<ClassEx>();
            List<OBJ_Class> names = new List<OBJ_Class>();

            DataTable table = tool._Q.Select("select class_name,grade_year from class where grade_year is not null order by grade_year,class_name");

            foreach (DataRow row in table.Rows)
            {
                OBJ_Class obj = new OBJ_Class();
                obj.ClassName = row.Field<string>("class_name");
                obj.ClassGrade_year = row.Field<string>("grade_year");
                names.Add(obj);
            }

            result.Add(records);
            result.Add(names);

            e.Result = result;

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<object> result = e.Result as List<object>;

            List<ClassEx> records = (List<ClassEx>)result[0];
            List<OBJ_Class> names = (List<OBJ_Class>)result[1];

            foreach (OBJ_Class ex in names)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(grdProgramPlanList);
                row.Cells[1].Value = ex.ClassName;
                row.Cells[2].Value = ex.ClassGrade_year;
                row.Tag = ex;
                grdProgramPlanList.Rows.Add(row);
                //比較names是否有相同名稱的內容
                if (records.Find(x => x.ClassName.Equals(ex.ClassName)) != null)
                {
                    DataGridViewCellStyle Style = row.DefaultCellStyle;
                    Style.BackColor = Color.Yellow;
                }
            }

            TitleText = "複製班級清單";
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            DialogResult dr = MsgBox.Show("您確認要複製所選班級?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dr == System.Windows.Forms.DialogResult.No)
            {
                MsgBox.Show("已取消...");
                return;
            }

            try
            {
                #region 取得所要複製的ischool班級清單

                List<OBJ_Class> SelectRows = new List<OBJ_Class>();

                foreach (DataGridViewRow row in grdProgramPlanList.SelectedRows)
                {
                    SelectRows.Add(row.Tag as OBJ_Class);
                }

                #endregion

                //取得排課班級
                List<ClassEx> records = tool._A.Select<ClassEx>();

                #region 判斷新增或更新班級

                List<ClassEx> updaterecords = new List<ClassEx>();
                List<ClassEx> insertrecords = new List<ClassEx>();

                foreach (OBJ_Class each in SelectRows)
                {
                    //取得清單內是否有重覆"班級名稱"的物件
                    ClassEx srecord = records.Find(x => x.ClassName.Equals(each.ClassName));

                    if (srecord == null)
                    {
                        //新增
                        ClassEx ex = new ClassEx();
                        ex.ClassName = each.ClassName;
                        ex.GradeYear = int.Parse(each.ClassGrade_year);
                        insertrecords.Add(ex);
                    }
                    else
                    {
                        //更新
                        srecord.GradeYear = int.Parse(each.ClassGrade_year);
                        updaterecords.Add(srecord);
                    }
                }

                #endregion

                #region 開始新增或覆蓋

                StringBuilder log_sb = new StringBuilder();
                log_sb.AppendLine("複製「ischool班級」至「排課班級」：");

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("複製班級清單成功");

                if (insertrecords.Count > 0)
                {
                    sb.AppendLine("新增「" + insertrecords.Count + "」筆");
                    tool._A.InsertValues(insertrecords);

                    log_sb.AppendLine("新增清單「" + insertrecords.Count + "」");
                    foreach (ClassEx each in insertrecords)
                    {
                        log_sb.AppendLine("班級名稱「" + each.ClassName + "」年級「" + each.GradeYear.Value + "」");
                    }
                }

                if (updaterecords.Count > 0)
                {
                    DialogResult update_dr = MsgBox.Show("共有" + updaterecords.Count + "筆班級資料已存在，是否覆蓋？", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                    if (update_dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        sb.AppendLine("覆蓋「" + updaterecords.Count + "」筆");
                        tool._A.UpdateValues(updaterecords);

                        log_sb.AppendLine("更新清單「" + updaterecords.Count + "」");
                        foreach (ClassEx each in updaterecords)
                        {
                            log_sb.AppendLine("班級 " + each.ClassName + " 已修改年級為「" + each.GradeYear.Value + "」");
                        }
                    }
                }

                if (insertrecords.Count > 0 || updaterecords.Count > 0)
                {
                    FISCA.LogAgent.ApplicationLog.Log("排課", "匯入班級", log_sb.ToString());

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

    class OBJ_Class
    {
        public string ClassName { get; set; }
        public string ClassGrade_year { get; set; }
    }
}
