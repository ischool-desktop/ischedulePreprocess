using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataRationality;
using System.Windows.Forms;
using FISCA.Data;
using System.Data;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 「課程分段教師」對應「ischool教師」檢查
    /// </summary>
    class SchedulerCourseSectionTeacherMapping : ICorrectableDataRationality
    {
        private List<string> CourseIDs = new List<string>();
        private List<SchedulerCourseSection> CorrectCourseSection = new List<SchedulerCourseSection>();

        #region ICorrectableDataRationality 成員

        public void ExecuteAutoCorrect(IEnumerable<string> EntityIDs)
        {
            List<SchedulerCourseSection> Sections = CorrectCourseSection
                .FindAll(x => EntityIDs.Contains(x.UID));

            MessageBox.Show("自動更新完成！");
        }

        public void ExecuteAutoCorrect()
        {
            CorrectCourseSection.SaveAll();

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

                strBuilder.AppendLine("檢查課程分段教師是否對應到ischool授課教師。");
               
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
                .Split(new char[]{' '});

            string SchoolYear = SchoolYearSemester[0];
            string Semester = SchoolYearSemester[1];


            #region 取得所有教師並依教師名稱排序
            QueryHelper qhelper = new QueryHelper();

            DataTable mtblTeacher = qhelper.Select("select id,(CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) as teachername from teacher where status=1 order by teachername");

            Dictionary<string, string> mTeacherNameIDs = new Dictionary<string, string>();

            mTeacherNameIDs.Clear();

            foreach (DataRow row in mtblTeacher.Rows)
            {
                string TeacherName = row.Field<string>("teachername");
                string TeacherID = row.Field<string>("id");

                if (!mTeacherNameIDs.ContainsKey(TeacherName))
                    mTeacherNameIDs.Add(TeacherName, TeacherID);
            }
            #endregion             

            AccessHelper helper = new AccessHelper();

            List<SchedulerCourseExtension> Courses = helper.Select<SchedulerCourseExtension>("school_year='"+ SchoolYear+"' and semester='" + Semester + "'");

            string CourseCondition = "(" + string.Join(",", Courses.Select(x=>x.UID).ToArray()) + ")";

            List<SchedulerCourseSection> Sections = helper.Select<SchedulerCourseSection>("ref_course_id in "+CourseCondition);

            List<object> Data = new List<object>();

            CorrectCourseSection.Clear();

            foreach (SchedulerCourseSection Section in Sections)
            {
                StringBuilder strBuilder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(Section.TeacherName1) &&
                    mTeacherNameIDs.ContainsKey(Section.TeacherName1) &&
                    Section.TeacherID1 ==null)
                {
                    strBuilder.Append("授課教師一應對應ischool系統編號。");
                    Section.TeacherID1 = K12.Data.Int.ParseAllowNull(mTeacherNameIDs[Section.TeacherName1]);
                }

                if (!string.IsNullOrWhiteSpace(Section.TeacherName2) &&
                    mTeacherNameIDs.ContainsKey(Section.TeacherName2) &&
                    Section.TeacherID2 ==null)
                {
                    strBuilder.Append("授課教師二應對應ischool系統編號。");
                    Section.TeacherID2 = K12.Data.Int.ParseAllowNull(mTeacherNameIDs[Section.TeacherName2]);
                }

                if (!string.IsNullOrWhiteSpace(Section.TeacherName3) &&
                    mTeacherNameIDs.ContainsKey(Section.TeacherName3) &&
                    Section.TeacherID3 == null)
                {
                    strBuilder.Append("授課教師三應對應ischool系統編號。");
                    Section.TeacherID3 = K12.Data.Int.ParseAllowNull(mTeacherNameIDs[Section.TeacherName3]);
                }

                if (strBuilder.Length > 0)
                {
                    CorrectCourseSection.Add(Section);
                    SchedulerCourseExtension Course = Courses.Find(x => x.UID.Equals(""+Section.CourseID));
                    Data.Add(new { 編號 = Section.CourseID, 課程名稱 = Course.CourseName, 學年度 = Course.SchoolYear, 學期 = Course.Semester, 節數 = Course.Period, 訊息 = strBuilder.ToString() });
                    CourseIDs.Add("" + Section.CourseID);
                }
            }
            #endregion

            DataRationalityMessage Message = new DataRationalityMessage();

            Message.Data = Data;
            Message.Message = Data.Count > 0 ? "共有" + Data.Count + "筆課程分段資料有不合理！請檢查！" : "恭禧！所有課程排課資料皆正常！";

            return Message;
        }

        public string Name
        {
            get { return "[課程分段教師]對應[ischool教師]"; }
        }
        #endregion
    }
}
