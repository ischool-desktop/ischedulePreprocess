using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Aspose.Cells;
using Campus.Report;
using FISCA.Data;
using ReportHelper;
using Sunset.Properties;

namespace Sunset
{
    /// <summary>
    /// 學生功課表
    /// </summary>
    public partial class frmStudentReport : FISCA.Presentation.Controls.BaseForm
    {
        private QueryHelper Helper = new QueryHelper();

        /// <summary>
        /// 建構式
        /// </summary>
        public frmStudentReport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStudentReport_Load(object sender, EventArgs e)
        {
            intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear);
            intSemester.Value = int.Parse(K12.Data.School.DefaultSemester);
        }

        /// <summary>
        /// 列印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region 介面參數驗證
            int SchoolYear;
            int Semester;

            if (!(K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0))
            {
                MessageBox.Show("請選擇學生!");
                return;
            }

            if (!int.TryParse(intSchoolYear.Text, out SchoolYear) || !int.TryParse(intSemester.Text, out Semester))
            {
                MessageBox.Show("『學年度』及『學期』必須為數字!");
                return;
            }
            #endregion

            #region 下SQL指令
            string strStudentCondition = string.Join(",", K12.Presentation.NLDPanels.Student.SelectedSource
                .Select(x => "'" + x + "'")
                .ToArray());

            string strStudentSQL = "SELECT student.id,class.class_name AS ClassName from student LEFT OUTER JOIN class ON class.id = student.ref_class_id WHERE student.id in (" + strStudentCondition + ")";

            DataTable StudentTable = Helper.Select(strStudentSQL);

            Dictionary<string, string> StudentClassNames = new Dictionary<string, string>();

            foreach (DataRow Row in StudentTable.Rows)
            {
                string StudentID = Row.Field<string>("id");
                string ClassName = Row.Field<string>("ClassName");

                if (!StudentClassNames.ContainsKey(StudentID))
                    StudentClassNames.Add(StudentID, ClassName);
            }

            StringBuilder strBuilder = new StringBuilder();

            //學生ID / 學生學號
            strBuilder.Append("SELECT student.id AS StudentID,student.student_number as StudentNumber,");
            //學生姓名 / 課程ID / 課程名稱
            strBuilder.Append("student.name as StudentName,course.id AS CourseID,course.course_name AS CourseName,");
            //班級ID / 班級名稱 / 老師ID
            strBuilder.Append("class.id AS ClassID,class.class_name AS ClassName,teacher.id AS TeacherID,");
            //老師姓名 / 場地ID(ClassroomID)
            strBuilder.Append("teacher.teacher_name AS TeacherName, $scheduler.classroom.uid AS ClassroomID,");
            //場地名稱 / 地理位置ID(已過時)
            strBuilder.Append("$scheduler.classroom.name AS ClassroomName, $scheduler.location.uid AS LocationID,");
            //地理位置名稱(已過時) / 課程分段(節數)
            strBuilder.Append("$scheduler.location.name AS LocationName,  $scheduler.course_section.length AS Length,");
            //課程分段(節次) / 課程分段(星期)
            strBuilder.Append("$scheduler.course_section.period AS Period,$scheduler.course_section.weekday AS Weekday ");

            //課程
            strBuilder.Append("FROM course ");
            //課程分段參考課程ID == 課程ID
            strBuilder.Append("LEFT OUTER JOIN $scheduler.course_section ON $scheduler.course_section.ref_course_id = course.id ");
            //班級ID == 課程參考班級ID
            strBuilder.Append("LEFT OUTER JOIN class ON class.id = course.ref_class_id ");
            //教師 == 課程ID
            strBuilder.Append("LEFT OUTER JOIN tc_instruct ON tc_instruct.ref_course_id = course.id ");
            //教師ID == 教師課程授課記錄ID
            strBuilder.Append("LEFT OUTER JOIN teacher ON teacher.id = tc_instruct.ref_teacher_id ");
            //學生修課記錄ID == 課程ID
            strBuilder.Append("LEFT OUTER JOIN sc_attend ON sc_attend.ref_course_id = course.id ");
            //學生ID == 學生修課記錄ID
            strBuilder.Append("LEFT OUTER JOIN student on student.id = sc_attend.ref_student_id ");
            //場地ID == 課程分段場地ID
            strBuilder.Append("LEFT OUTER JOIN $scheduler.classroom ON $scheduler.classroom.uid =  $scheduler.course_section.ref_classroom_id ");
            //地理位置ID(已過時) == 場地ID
            strBuilder.Append("LEFT OUTER JOIN $scheduler.location ON $scheduler.location.uid = $scheduler.classroom.ref_location_id ");
            //條件:學年度/學期/於學生ID清單 依據學生ID排列
            strBuilder.AppendFormat("WHERE (course.school_year='{0}'  AND course.semester='{1}' ) AND student.id in ({2}) order by student.id", SchoolYear, Semester, strStudentCondition);

            DataTable Table = Helper.Select(strBuilder.ToString());
            #endregion

            #region 將結果轉成功課表物件
            Dictionary<string, StudentLPView> StudentLPViews = new Dictionary<string, StudentLPView>();

            foreach (DataRow Row in Table.Rows)
            {
                string StudentID = Row.Field<string>("StudentID");
                string StudentName = Row.Field<string>("StudentName");
                string StudentNumber = Row.Field<string>("StudentNumber");
                string CourseName = Row.Field<string>("CourseName");
                string ClassName = Row.Field<string>("ClassName");
                string TeacherName = Row.Field<string>("TeacherName");
                string ClassroomName = Row.Field<string>("ClassroomName");
                string LocationName = Row.Field<string>("LocationName");
                string Length = Row.Field<string>("Length");
                string Weekday = Row.Field<string>("Weekday");
                string Period = Row.Field<string>("Period");
                string Text = CourseName + System.Environment.NewLine + TeacherName + System.Environment.NewLine + ClassroomName + System.Environment.NewLine + LocationName;

                if (!StudentLPViews.ContainsKey(StudentID))
                {
                    StudentLPViews.Add(StudentID, new StudentLPView(9));
                    StudentLPViews[StudentID].Name = StudentName;
                    StudentLPViews[StudentID].StudentNumber = StudentNumber;
                    StudentLPViews[StudentID].ClassName = StudentClassNames.ContainsKey(StudentID) ? StudentClassNames[StudentID] : "未分班";
                }

                StudentLPView StudentView = StudentLPViews[StudentID];

                if (!string.IsNullOrEmpty(Weekday) &&
                    !string.IsNullOrEmpty(Period) &&
                    !string.IsNullOrEmpty(Length))
                {
                    int intWeekday = K12.Data.Int.Parse(Weekday);
                    int intPeriod = K12.Data.Int.Parse(Period);
                    int intLength = K12.Data.Int.Parse(Length);

                    if (intPeriod > 0)
                    {
                        for (int i = intPeriod; i < intPeriod + intLength; i++)
                        {
                            if (!StudentView.Items.ContainsKey(i))
                            {
                                StudentView.Items.Add(i, new LPViewItem());
                                StudentView.Items[i].PeriodNo = i;
                            }

                            StudentView.Items[i].SetWeekDayText(intWeekday, Text);
                        }
                    }
                }
            }
            #endregion


            #region 轉換為功課表輸出物件
            Dictionary<string, List<DataSet>> LPViews = new Dictionary<string, List<DataSet>>();

            foreach (StudentLPView StudentView in StudentLPViews.Values)
            {
                DataSet LPView = new DataSet("DataSection");

                if (!LPViews.ContainsKey(StudentView.ClassName))
                    LPViews.Add(StudentView.ClassName, new List<DataSet>());

                DataTable tabLPViewName = StudentView.Name.ToDataTable("ScheduleName", "資源名稱");
                DataTable tabLPViewType = "學生".ToDataTable("ScheduleType", "資源類別");
                DataTable tabClassName = StudentView.ClassName.ToDataTable("ClassName", "班級名稱");
                DataTable tabStudentNumber = StudentView.StudentNumber.ToDataTable("StudentNumber", "學號");

                LPView.Tables.Add(tabLPViewName);
                LPView.Tables.Add(tabLPViewType);
                LPView.Tables.Add(tabClassName);
                LPView.Tables.Add(tabStudentNumber);

                #region 轉換功課表內容
                DataTable tabSchedule = new DataTable("ScheduleDetail");

                int MaxWeekDay = 5;

                tabSchedule.Columns.Add("PeriodNo");

                for (int i = 1; i <= MaxWeekDay; i++)
                    tabSchedule.Columns.Add("" + i);

                List<string> WeekDays = new List<string>() { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

                foreach (LPViewItem Item in StudentView.Items.Values)
                {
                    DataRow Row = tabSchedule.NewRow();

                    Row.SetField("PeriodNo", Item.PeriodNo);

                    for (int i = 1; i <= MaxWeekDay; i++)
                    {
                        PropertyInfo Property = Item.GetType().GetProperty(WeekDays[i - 1]);

                        string Value = "" + Property.GetValue(Item, null);

                        Row.SetField("" + i, Value);
                    }

                    tabSchedule.Rows.Add(Row);
                }

                LPView.Tables.Add(tabSchedule);
                #endregion

                LPViews[StudentView.ClassName].Add(LPView);
            }
            #endregion

            #region 實際輸出報表
            try
            {
                MemoryStream Stream = new MemoryStream(Resources.功課表);

                Workbook wb = ReportHelper.Report.Produce(LPViews, Stream);

                string mSaveFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Reports\\功課表.xls";

                ReportSaver.SaveWorkbook(wb, mSaveFilePath);
            }
            catch (Exception ve)
            {
                MessageBox.Show(ve.Message);
            }
            #endregion
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}