using System.Collections.Generic;
using System.Data;
using FISCA.Data;
using K12.Data;
using System.Linq;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    public class DataAccess
    {
        /// <summary>
        /// 根據課程系統編號取得課程
        /// </summary>
        /// <param name="CourseIDs"></param>
        /// <returns></returns>
        public static Dictionary<string ,SchedulerCourseExtension> GetCourseByIDs(List<string> CourseIDs)
        {
            Dictionary<string, SchedulerCourseExtension> result = new Dictionary<string, SchedulerCourseExtension>();

            AccessHelper helper = new AccessHelper();

            List<SchedulerCourseExtension> Courses = helper
                .Select<SchedulerCourseExtension>();

            result = Courses
                .ToDictionary(x => x.UID);

            return result;
        }

        /// <summary>
        /// 取依照學年度及學期分類的課程系統編號
        /// </summary>
        /// <returns></returns>
        public static SortedDictionary<string, List<string>> GetSemesterCourseIDs()
        {
            SortedDictionary<string, List<string>> SemesterCourse = new SortedDictionary<string, List<string>>();
            QueryHelper helper = new QueryHelper();
            DataTable dt = helper.Select("select uid,school_year,semester from $scheduler.scheduler_course_extension");
            List<string> CourseIDs = new List<string>();

            SemesterCourse.Add("未分學年度學期", new List<string>());

            foreach (DataRow row in dt.Rows)
            {
                string CourseID = row.Field<string>("uid");
                string SchoolYear = row.Field<string>("school_year");
                string Semester = row.Field<string>("semester");

                if (!string.IsNullOrWhiteSpace(SchoolYear) &&
                    !string.IsNullOrWhiteSpace(Semester))
                {
                    string SchoolYearSemester = SchoolYear + "學年度 第" + Semester + "學期";

                    if (!SemesterCourse.ContainsKey(SchoolYearSemester))
                        SemesterCourse.Add(SchoolYearSemester, new List<string>());

                    SemesterCourse[SchoolYearSemester].Add(CourseID);
                }
                else 
                    SemesterCourse["未分學年度學期"].Add(CourseID);
            }

            return SemesterCourse;
        }
    }
}