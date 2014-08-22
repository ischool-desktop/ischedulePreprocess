using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataRationality;
using FISCA.Data;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 課程排課資料合理性檢查
    /// </summary>
    class SchedulerCourseExtensionRationality : IDataRationality
    {
        private List<string> CourseIDs = new List<string>();

        #region IDataRationality 成員

        public void AddToTemp(IEnumerable<string> EntityIDs)
        {
            CourseAdmin.Instance.AddToTemp(EntityIDs.ToList());
        }

        public void AddToTemp()
        {
            AddToTemp(CourseIDs);
        }

        public string Category
        {
            get { return "排課"; }
        }

        public string Description
        {
            get
            {
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.AppendLine("檢查排課資料");
                strBuilder.AppendLine("1.檢查課程是否有指定上課時間表，若無指定則排課主程式不會下載課程分段。");
                strBuilder.AppendLine("2.檢查課程分割設定加總與課程節數不一致。");

                return strBuilder.ToString();
            }
        }

        public DataRationalityMessage Execute()
        {
            CourseIDs.Clear();

            #region 取得群組名稱不為空白的課程分段
            frmSchoolYearSemesterSelector Selector = new frmSchoolYearSemesterSelector();

            if (Selector.ShowDialog() == DialogResult.Cancel)
            {
                MessageBox.Show("請選擇學年度學期！");
                return new DataRationalityMessage();
            }

            string[] SchoolYearSemester = Selector.SchoolYearSemester
                .Split(new char[] { ' ' });

            string SchoolYear = SchoolYearSemester[0];
            string Semester = SchoolYearSemester[1];

            QueryHelper helper = new QueryHelper();

            DataTable table = helper.Select("select $scheduler.scheduler_course_extension.uid,split_spec,course_name,school_year,semester,$scheduler.timetable.name as timetable_name,period from $scheduler.scheduler_course_extension left outer join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.scheduler_course_extension.ref_timetable_id where $scheduler.scheduler_course_extension.school_year='" + SchoolYear + "' and $scheduler.scheduler_course_extension.semester='" + Semester + "' order by school_year desc,semester,course_name");
            List<QueryCourse> QueryCourses = new List<QueryCourse>();

            foreach (DataRow row in table.Rows)
            {
                QueryCourse QuerySection = new QueryCourse(row);
                QueryCourses.Add(QuerySection);
            }
            #endregion

            #region 針對每個課程做檢查
            DataRationalityMessage Message = new DataRationalityMessage();

            List<object> Data = new List<object>();

            foreach (QueryCourse Course in QueryCourses)
            {
                StringBuilder strBuilder = new StringBuilder();

                if (string.IsNullOrWhiteSpace(Course.TimeTableName))
                    strBuilder.AppendLine("未指定時間表。");

                if (!Course.IsPeriodEqualSplitSpec)
                    strBuilder.AppendLine("課程分割設定與節數不一致。");

                if (strBuilder.Length > 0)
                {
                    Data.Add(new { 編號 = Course.CourseID, 課程名稱 = Course.CourseName, 學年度 = Course.SchoolYear, 學期 = Course.Semester, 上課時間表 = Course.TimeTableName, 節數 = Course.Period, 分割設定 = Course.SplitSpec, 訊息 = strBuilder.ToString() });
                    CourseIDs.Add(Course.CourseID);
                }
            }
            #endregion

            Message.Data = Data;
            Message.Message = Data.Count > 0 ? "共有" + Data.Count + "筆課程排課資料有不合理！請檢查！" : "恭禧！所有課程排課資料皆正常！";

            return Message;
        }

        public string Name
        {
            get { return "課程排課資料"; }
        }

        #endregion
    }
}
