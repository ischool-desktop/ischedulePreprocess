using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using Aspose.Words;
using System.Diagnostics;
using System.IO;
using SmartSchool.ePaper;

namespace Sunset
{
    public partial class HomeworkTableForm : BaseForm
    {
        /// 樣版
        /// </summary>
        string HomeworkTable_Config_1 = "Sunset.NewCourse.dylan.Report.HomeworkTableForm";

        SmartSchool.ePaper.ElectronicPaper paperForStudent { get; set; }

        Dictionary<string, string> DicTimeTable = new Dictionary<string, string>();

        Dictionary<string, DataRow> DataStudentList = new Dictionary<string, DataRow>();

        int SchoolYear { get; set; }
        int Semester { get; set; }

        bool IsPrintToW = false;

        BackgroundWorker BGW = new BackgroundWorker();

        public HomeworkTableForm()
        {
            InitializeComponent();

            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            int x;
            if (int.TryParse(K12.Data.School.DefaultSchoolYear, out x))
            {
                SchoolYear = intSchoolYear.Value = x;
            }
            if (int.TryParse(K12.Data.School.DefaultSemester, out x))
            {
                Semester = intSemester.Value = x;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!BGW.IsBusy)
            {
                btnSave.Enabled = false;
                SchoolYear = intSchoolYear.Value;
                Semester = intSemester.Value;
                IsPrintToW = cbIsPrintTo_ePort.Checked;
                BGW.RunWorkerAsync();
            }
            else
            {
                MsgBox.Show("忙碌中,稍後再試!!");
                return;
            }
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            #region 樣版

            //電子報表
            paperForStudent = new SmartSchool.ePaper.ElectronicPaper(string.Format("學生功課表({0}0{1})", "" + SchoolYear, "" + Semester), "" + SchoolYear, "" + Semester, SmartSchool.ePaper.ViewerType.Student);
            DataStudentList.Clear();

            Campus.Report.ReportConfiguration ConfigurationInCadre = new Campus.Report.ReportConfiguration(HomeworkTable_Config_1);
            Aspose.Words.Document Template;

            if (ConfigurationInCadre.Template == null)
            {
                //如果範本為空,則建立一個預設範本
                Campus.Report.ReportConfiguration ConfigurationInCadre_1 = new Campus.Report.ReportConfiguration(HomeworkTable_Config_1);
                ConfigurationInCadre_1.Template = new Campus.Report.ReportTemplate(Properties.Resources.學生排課功課表, Campus.Report.TemplateType.Word);
                Template = ConfigurationInCadre_1.Template.ToDocument();
            }
            else
            {
                //如果已有範本,則取得樣板
                Template = ConfigurationInCadre.Template.ToDocument();
            }

            #endregion

            #region 學生資料

            List<string> StudentIDList = K12.Presentation.NLDPanels.Student.SelectedSource;
            Dictionary<string, StudentObj> StudentIDDic = new Dictionary<string, StudentObj>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select student.id as student_id,student.name as student_name,");
            sb.Append("student.student_number,student.seat_no,");
            sb.Append("class.id as ref_class_id,class.class_name,");
            sb.Append("sc_attend.ref_course_id,");
            sb.Append("$scheduler.scheduler_course_extension.course_name as course_name,");
            sb.Append("$scheduler.scheduler_course_section.weekday as weekday,");
            sb.Append("$scheduler.scheduler_course_section.period  as period,");
            sb.Append("$scheduler.scheduler_course_section.length  as length,"); //
            sb.Append("$scheduler.scheduler_course_section.teacher_name_1 as teacher_name_1,");
            sb.Append("$scheduler.scheduler_course_section.teacher_name_2 as teacher_name_2,");
            sb.Append("$scheduler.scheduler_course_section.teacher_name_3 as teacher_name_3,");
            sb.Append("$scheduler.classroom.name as classroom_name ");
            sb.Append("from student ");
            sb.Append("join sc_attend on student.id=sc_attend.ref_student_id ");
            sb.Append("join $scheduler.scheduler_course_extension on $scheduler.scheduler_course_extension.ref_course_id=sc_attend.ref_course_id ");
            sb.Append("left join class on student.ref_class_id=class.id ");
            sb.Append("left join $scheduler.scheduler_course_section on $scheduler.scheduler_course_extension.uid=$scheduler.scheduler_course_section.ref_course_id ");
            sb.Append("left join $scheduler.classroom on $scheduler.classroom.uid=$scheduler.scheduler_course_section.ref_classroom_id ");
            sb.Append(string.Format("where student.id in ('{0}') ", string.Join("','", StudentIDList)));
            sb.Append(string.Format("and school_year='{0}' and semester='{1}' ", SchoolYear, Semester));
            sb.Append("and $scheduler.scheduler_course_section.weekday is not null ");
            sb.Append("and $scheduler.scheduler_course_section.period is not null ");
            sb.Append("and $scheduler.scheduler_course_section.length is not null ");
            sb.Append("order by student.student_number");

            DataTable dt = tool._Q.Select(sb.ToString());
            StringBuilder sb_leg = new StringBuilder();
            foreach (DataRow each in dt.Rows)
            {
                StudentObj obj = new StudentObj(each);
                if (!StudentIDDic.ContainsKey(obj.StudentID))
                {
                    StudentIDDic.Add(obj.StudentID, obj);
                }

                if (!string.IsNullOrEmpty(StudentIDDic[obj.StudentID].GetPeriod(each)))
                {
                    sb_leg.AppendLine(StudentIDDic[obj.StudentID].GetPeriod(each));
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(sb_leg.ToString()))
            {
                MsgBox.Show(sb_leg.ToString());
            }

            #region 欄位填值

            DataTable table = GetTableTitle();
            DataTable table_ePost = GetTableTitle();

            foreach (StudentObj each in StudentIDDic.Values)
            {
                DataRow row = table.NewRow();

                //電子報表
                DataRow row1 = table_ePost.NewRow();

                row1["學校名稱"] = row["學校名稱"] = K12.Data.School.ChineseName;
                row1["學校英文名稱"] = row["學校英文名稱"] = K12.Data.School.EnglishName;
                row1["學年度"] = row["學年度"] = SchoolYear;
                row1["學期"] = row["學期"] = Semester;
                row1["列印日期"] = row["列印日期"] = DateTime.Today.ToShortDateString();

                row1["班級"] = row["班級"] = each.ClassName;
                row1["座號"] = row["座號"] = each.SeatNo;
                row1["學號"] = row["學號"] = each.StudentNumber;
                row1["姓名"] = row["姓名"] = each.StudentName;

                foreach (n_PeriodObj per in each.Items)
                {
                    if (per.Period <= 30 && per.WeekDay <= 7)
                    {
                        for (int x = per.Period; x < per.Period + per.Length; x++)
                        {
                            string n_value = string.Format("星期{0}_{1}", per.WeekDay, x);
                            if (string.IsNullOrEmpty("" + row[n_value]))
                            {
                                row[n_value] = per.GetCourseName();
                                row1[n_value] = per.GetCourseName();
                            }
                            else
                            {
                                row[n_value] += "\n" + per.GetCourseName();
                                row1[n_value] = per.GetCourseName();
                            }
                        }
                    }
                }
                table.Rows.Add(row);

                //電子報表
                DataStudentList.Add(each.StudentID, row1);
            }

            #endregion

            Document PageOne = (Document)Template.Clone(true);
            PageOne.MailMerge.Execute(table);

            if (IsPrintToW)
            {
                foreach (string each in DataStudentList.Keys)
                {
                    MemoryStream stream = new MemoryStream();

                    table_ePost.Rows.Clear();
                    table_ePost.Rows.Add(DataStudentList[each]);

                    Document doc = (Document)Template.Clone(true);
                    doc.MailMerge.Execute(table_ePost);
                    doc.Save(stream, SaveFormat.Doc);

                    paperForStudent.Append(new PaperItem(PaperFormat.Office2003Doc, stream, each));
                }
            }

            e.Result = PageOne;
        }

        private DataTable GetTableTitle()
        {
            DataTable table = new DataTable();
            table.Columns.Add("學校名稱");
            table.Columns.Add("學校英文名稱");
            table.Columns.Add("學年度");
            table.Columns.Add("學期");
            table.Columns.Add("列印日期");

            table.Columns.Add("班級");
            table.Columns.Add("座號");
            table.Columns.Add("學號");
            table.Columns.Add("姓名");

            for (int x = 0; x <= 7; x++)
            {
                for (int y = 0; y <= 30; y++)
                {
                    table.Columns.Add(string.Format("星期{0}_{1}", x, y));
                }
            }

            return table;
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.Enabled = true;

            if (e.Cancelled)
            {
                MsgBox.Show("作業已被中止!!");
            }
            else
            {
                if (e.Error == null)
                {
                    Document inResult = (Document)e.Result;

                    try
                    {
                        SaveFileDialog SaveFileDialog1 = new SaveFileDialog();

                        SaveFileDialog1.Filter = "Word (*.doc)|*.doc|所有檔案 (*.*)|*.*";
                        SaveFileDialog1.FileName = "學生功課表(套表列印)";

                        if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            if (IsPrintToW)
                            {
                                //開始上傳
                                SmartSchool.ePaper.DispatcherProvider.Dispatch(paperForStudent);
                            }

                            inResult.Save(SaveFileDialog1.FileName);
                            Process.Start(SaveFileDialog1.FileName);
                        }
                        else
                        {
                            FISCA.Presentation.Controls.MsgBox.Show("檔案未儲存");
                            return;
                        }
                    }
                    catch
                    {
                        FISCA.Presentation.Controls.MsgBox.Show("檔案儲存錯誤,請檢查檔案是否開啟中!!");
                        return;
                    }

                    this.Close();
                }
                else
                {
                    MsgBox.Show("列印資料發生錯誤\n" + e.Error.Message);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //取得設定檔
            Campus.Report.ReportConfiguration ConfigurationInCadre = new Campus.Report.ReportConfiguration(HomeworkTable_Config_1);
            Campus.Report.TemplateSettingForm TemplateForm;
            //畫面內容(範本內容,預設樣式
            if (ConfigurationInCadre.Template != null)
            {
                TemplateForm = new Campus.Report.TemplateSettingForm(ConfigurationInCadre.Template, new Campus.Report.ReportTemplate(Properties.Resources.學生排課功課表, Campus.Report.TemplateType.Word));
            }
            else
            {
                ConfigurationInCadre.Template = new Campus.Report.ReportTemplate(Properties.Resources.學生排課功課表, Campus.Report.TemplateType.Word);
                TemplateForm = new Campus.Report.TemplateSettingForm(ConfigurationInCadre.Template, new Campus.Report.ReportTemplate(Properties.Resources.學生排課功課表, Campus.Report.TemplateType.Word));
            }

            //預設名稱
            TemplateForm.DefaultFileName = "學生功課表(套表列印範本)";

            //如果回傳為OK
            if (TemplateForm.ShowDialog() == DialogResult.OK)
            {
                //設定後樣試,回傳
                ConfigurationInCadre.Template = TemplateForm.Template;
                //儲存
                ConfigurationInCadre.Save();
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "另存新檔";
            sfd.FileName = "學生功課表_合併欄位總表.doc";
            sfd.Filter = "Word檔案 (*.doc)|*.doc|所有檔案 (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    fs.Write(Properties.Resources.學生排課功課表_欄位總表, 0, Properties.Resources.學生排課功課表_欄位總表.Length);
                    fs.Close();
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch
                {
                    FISCA.Presentation.Controls.MsgBox.Show("指定路徑無法存取。", "另存檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}
