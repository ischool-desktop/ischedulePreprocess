using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataRationality;
using FISCA.Data;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 「課程班級」對應「ischool班級」檢查
    /// </summary>
    class SchedulerCourseClassMapping : ICorrectableDataRationality
    {
        private List<string> CourseIDs = new List<string>();
        private List<SchedulerCourseExtension> CorrectCourses = new List<SchedulerCourseExtension>();

        #region ICorrectableDataRationality 成員

        public void ExecuteAutoCorrect(IEnumerable<string> EntityIDs)
        {
            List<SchedulerCourseExtension> Sections = CorrectCourses
                .FindAll(x => EntityIDs.Contains(x.UID));

            MessageBox.Show("自動更新完成！");
        }

        public void ExecuteAutoCorrect()
        {
            CorrectCourses.SaveAll();

            MessageBox.Show("自動更新完成！");
        }

        #endregion

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

                strBuilder.AppendLine("檢查課程班級是否對應到ischool班級。");

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


            #region 取得所有班級
            QueryHelper qhelper = new QueryHelper();

            DataTable mtblClass = qhelper.Select("select id,class_name from class where status=1");

            Dictionary<string, string> mClassNameIDs = new Dictionary<string, string>();

            mClassNameIDs.Clear();

            foreach (DataRow row in mtblClass.Rows)
            {
                string ClassName = row.Field<string>("class_name");
                string ClassID = row.Field<string>("id");

                if (!mClassNameIDs.ContainsKey(ClassName))
                    mClassNameIDs.Add(ClassName, ClassID);
            }
            #endregion

            // string CourseCondition = "(" + string.Join(",", CourseAdmin.Instance.SelectedSource.ToArray()) + ")";

            AccessHelper helper = new AccessHelper();

            List<SchedulerCourseExtension> Courses = helper.Select<SchedulerCourseExtension>("school_year='" + SchoolYear + "' and semester='" + Semester + "'");

            List<object> Data = new List<object>();

            CorrectCourses.Clear();

            foreach (SchedulerCourseExtension Course in Courses)
            {
                StringBuilder strBuilder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(Course.ClassName) &&
                    mClassNameIDs.ContainsKey(Course.ClassName) &&
                    Course.ClassID == null)
                {
                    strBuilder.Append("排課課程之班級需要對應ischool系統編號。");
                    Course.ClassID = K12.Data.Int.ParseAllowNull(mClassNameIDs[Course.ClassName]);
                }

                if (strBuilder.Length > 0)
                {
                    CorrectCourses.Add(Course);
                    Data.Add(new { 編號 = Course.UID, 課程名稱 = Course.CourseName, 學年度 = Course.SchoolYear, 學期 = Course.Semester, 節數 = Course.Period, 訊息 = strBuilder.ToString() });
                    CourseIDs.Add("" + Course.CourseID);
                }
            }
            #endregion

            DataRationalityMessage Message = new DataRationalityMessage();

            Message.Data = Data;
            Message.Message = Data.Count > 0 ? "共有" + Data.Count + "筆課程排課資料有不合理！請檢查！" : "恭禧！所有課程排課資料皆正常！";

            return Message;
        }

        public string Name
        {
            get { return "[課程班級]對應[ischool班級]"; }
        }
        #endregion
    }
}
