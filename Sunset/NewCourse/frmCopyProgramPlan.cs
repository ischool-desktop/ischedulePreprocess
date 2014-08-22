using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using FISCA.DSAUtil;
using FISCA.Presentation;
using FISCA.UDT;
using K12.Data;
using FISCA.Data;
using System.Drawing;
using FISCA.Presentation.Controls;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 複課程規劃
    /// </summary>
    public partial class frmCopyProgramPlan : FISCA.Presentation.Controls.BaseForm
    {
        private QueryHelper mQueryHelper = new QueryHelper();
        private List<ProgramPlanRecord> mProgramPlans = new List<ProgramPlanRecord>();
        private List<string> mSchedulerProgramPlanNames = new List<string>();

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public frmCopyProgramPlan()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCopyProgramPlan_Load(object sender, EventArgs e)
        {
            btnClose.Click += (vsender, ve) => this.Close();

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (vsender, ve) =>
            {
                List<object> result = new List<object>();

                List<ProgramPlanRecord> records = K12.Data.ProgramPlan.SelectAll();
                List<string> names = new List<string>();

                DataTable table = mQueryHelper.Select("select name from $scheduler.scheduler_program_plan");

                foreach (DataRow row in table.Rows)
                {
                    string name = row.Field<string>("name");
                    names.Add(name);
                }

                result.Add(records);
                result.Add(names);

                ve.Result = result;
            };

            worker.RunWorkerCompleted += (vsender, ve) =>
            {
                List<object> result = ve.Result as List<object>;

                mProgramPlans = result[0] as List<ProgramPlanRecord>;

                mSchedulerProgramPlanNames = result[1] as List<string>;

                foreach (ProgramPlanRecord ProgramPlan in mProgramPlans)
                {
                    int Row = grdProgramPlanList.Rows.Add(ProgramPlan.ID, ProgramPlan.Name);

                    if (mSchedulerProgramPlanNames.Contains(ProgramPlan.Name))
                    {
                        DataGridViewCellStyle Style = grdProgramPlanList.Rows[Row].DefaultCellStyle;
                        Style.BackColor = Color.Yellow;
                    }

                    grdProgramPlanList.Rows[Row].Tag = ProgramPlan;
                }

                //grdProgramPlanList.Refresh();

                TitleText = "複製課程規劃";
            };

            worker.RunWorkerAsync();
            TitleText = "複製課程規劃(取得資料中...)";
        }

        private string GetSubject(K12.Data.ProgramSubject Subject)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlElement element = xmldoc.CreateElement("Subject");

            element.SetAttribute("GradeYear", K12.Data.Int.GetString(Subject.GradeYear));
            element.SetAttribute("Semester", K12.Data.Int.GetString(Subject.Semester));
            element.SetAttribute("Credit", K12.Data.Decimal.GetString(Subject.Credit));
            element.SetAttribute("Period", K12.Data.Decimal.GetString(Subject.Period));
            element.SetAttribute("Domain", Subject.Domain);
            element.SetAttribute("FullName", Subject.FullName);
            element.SetAttribute("CalcFlag", "" + Subject.CalcFlag);
            element.SetAttribute("SubjectName", Subject.SubjectName);

            XmlElement elmGrouping = xmldoc.CreateElement("Grouping");
            elmGrouping.SetAttribute("RowIndex", K12.Data.Int.GetString(Subject.RowIndex));
            elmGrouping.SetAttribute("startLevel", K12.Data.Int.GetString(Subject.StartLevel));

            element.AppendChild(elmGrouping);
            element.SetAttribute("NotIncludedInCalc", "" + Subject.NotIncludedInCalc);
            element.SetAttribute("NotIncludedInCredit", "" + Subject.NotIncludedInCredit);
            element.SetAttribute("RequiredBy", Subject.RequiredBy);
            element.SetAttribute("Required", Subject.Required ? "必修" : "選修");
            element.SetAttribute("Category", Subject.Category);
            element.SetAttribute("Entry", Subject.Entry);
            element.SetAttribute("Level", K12.Data.Int.GetString(Subject.Level));

            return element.OuterXml;
        }

        private string GetSubjects(List<K12.Data.ProgramSubject> Subjects)
        {
            DSXmlHelper helper = new DSXmlHelper("GraduationPlan");

            foreach (var subject in Subjects)
            {
                string strXML = GetSubject(subject);
                helper.AddXmlString(".", strXML);
            }

            string result = helper.BaseElement.OuterXml;

            return result;
        }

        /// <summary>
        /// 複製課程規劃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            DialogResult dr = MsgBox.Show("您確認要複製所選課程規畫表?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dr == System.Windows.Forms.DialogResult.No)
            {
                MsgBox.Show("已取消...");
                return;
            }

            try
            {
                #region 取得選取課程規劃
                List<ProgramPlanRecord> records = new List<ProgramPlanRecord>();

                foreach (DataGridViewRow row in grdProgramPlanList.SelectedRows)
                    records.Add(row.Tag as ProgramPlanRecord);

                List<string> Names = records
                    .Select(x => x.Name)
                    .ToList();
                #endregion

                #region 取得排課課程規劃
                AccessHelper helper = new AccessHelper();

                List<SchedulerProgramPlan> srecords = new List<SchedulerProgramPlan>();

                if (Names.Count > 0)
                {
                    string strQuery = "name in (" + string.Join(",", Names.Select(x => "'" + x + "'").ToArray()) + ")";
                    srecords = helper.Select<SchedulerProgramPlan>(strQuery);
                }
                #endregion

                #region 判斷新增或更新課程規劃
                List<SchedulerProgramPlan> updaterecords = new List<SchedulerProgramPlan>();
                List<SchedulerProgramPlan> insertrecords = new List<SchedulerProgramPlan>();

                foreach (ProgramPlanRecord record in records)
                {
                    SchedulerProgramPlan srecord = srecords
                        .Find(x => x.Name.Equals(record.Name));

                    if (srecord != null)
                    {
                        srecord.Content = GetSubjects(record.Subjects);
                        updaterecords.Add(srecord);
                    }
                    else
                    {
                        srecord = new SchedulerProgramPlan();
                        srecord.Name = record.Name;
                        srecord.Content = GetSubjects(record.Subjects);
                        insertrecords.Add(srecord);
                    }
                }
                #endregion

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("複製課程規劃成功");

                if (insertrecords.Count > 0)
                {
                    sb.AppendLine("新增「" + insertrecords.Count + "」筆");
                    insertrecords.SaveAll();
                }
                //實際新增或更新

                if (updaterecords.Count > 0)
                {
                    DialogResult update_dr = MsgBox.Show("共有" + updaterecords.Count + "筆課程規劃已存在，是否覆蓋？", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                    if (update_dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        sb.AppendLine("覆蓋「" + updaterecords.Count + "」筆");
                        updaterecords.SaveAll();
                    }
                }

                if (insertrecords.Count > 0 || updaterecords.Count > 0)
                {
                    MsgBox.Show(sb.ToString());
                }
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
}