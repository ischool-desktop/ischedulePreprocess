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
    /// 課程基本資料合理性檢查
    /// </summary>
    class SchedulerCourseRationality : IDataRationality
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

                strBuilder.AppendLine("檢查課程資料");
                strBuilder.AppendLine("1.檢查課程是否有科目名稱，若為空白則排課主程式不會下載課程分段。");
                strBuilder.AppendLine("2.檢查課程是否有節數，若無則容易造成排課結果與節數不一致。");

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


            //string CourseCondition = "(" + string.Join(",",CourseAdmin.Instance.SelectedSource.ToArray()) +")";

            QueryHelper helper = new QueryHelper();

            DataTable table = helper.Select("select uid,course_name,school_year,semester,subject,period from $scheduler.scheduler_course_extension where school_year='" + SchoolYear + "' and semester='" + Semester + "' order by school_year desc,semester,course_name");

            List<SchedulerQueryCourse> QueryCourses = new List<SchedulerQueryCourse>();

            foreach (DataRow row in table.Rows)
            {
                SchedulerQueryCourse QuerySection = new SchedulerQueryCourse(row);
                QueryCourses.Add(QuerySection);
            }
            #endregion

            #region 針對每個課程做檢查
            DataRationalityMessage Message = new DataRationalityMessage();

            List<object> Data = new List<object>();

            foreach (SchedulerQueryCourse Course in QueryCourses)
            {
                StringBuilder strBuilder = new StringBuilder();

                if (string.IsNullOrWhiteSpace(Course.Subject))
                    strBuilder.AppendLine("科目名稱為空白。");

                if (string.IsNullOrWhiteSpace(Course.Period))
                    strBuilder.AppendLine("節數為空白。");

                if (strBuilder.Length > 0)
                {
                    Data.Add(new
                    {
                        編號 = Course.UID,
                        課程名稱 = Course.CourseName,
                        學年度 = Course.SchoolYear,
                        學期 = Course.Semester,
                        節數 = Course.Period,
                        科目名稱 = Course.Subject,
                        訊息 = strBuilder.ToString()
                    });
                    CourseIDs.Add(Course.UID);
                }
            }
            #endregion

            Message.Data = Data;
            Message.Message = Data.Count > 0 ? "共有" + Data.Count + "筆課程排課資料有不合理！請檢查！" : "恭禧！所有課程排課資料皆正常！";

            return Message;
        }

        public string Name
        {
            get { return "課程基本資料"; }
        }

        #endregion
    }
}