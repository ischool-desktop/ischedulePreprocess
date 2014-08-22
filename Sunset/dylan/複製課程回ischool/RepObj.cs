using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using K12.Data;

namespace Sunset.NewCourse
{
    public static class RepObj
    {
        /// <summary>
        /// 取得Key值的組合清單
        /// 課程名稱 / 學年度 / 學期
        /// </summary>
        static public List<string> GetExistCourseNames(List<SchedulerCourseExtension> Courses)
        {
            List<string> Conditions = new List<string>();
            foreach (SchedulerCourseExtension Course in Courses)
                Conditions.Add("(school_year=" + Course.SchoolYear + " and semester=" + Course.Semester + " and course_name='" + Course.CourseName + "')");

            string strSQL = "select course_name,school_year,semester from course where " + string.Join(" or ", Conditions.ToArray());
            DataTable table = tool._Q.Select(strSQL);

            List<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                string CourseName = row.Field<string>("course_name");
                string SchoolYear = row.Field<string>("school_year");
                string Semester = row.Field<string>("semester");
                string CourseKey = CourseName + SchoolYear + Semester;

                if (!list.Contains(CourseKey))
                    list.Add(CourseKey);
            }
            return list;
        }

        /// <summary>
        /// 取得複製回ischool課程的組合物件
        /// </summary>
        static public List<K12CourseRecord> GetK12CourseOBJ(List<SchedulerCourseExtension> Courses, List<string> ExistCourseNames, Dictionary<string, string> mClassNameIDs)
        {
            List<K12CourseRecord> list = new List<K12CourseRecord>();
            foreach (SchedulerCourseExtension Course in Courses)
            {
                string CourseKey = Course.CourseName + Course.SchoolYear + Course.Semester;

                if (!ExistCourseNames.Contains(CourseKey))
                {
                    K12CourseRecord NewK12Course = new K12CourseRecord();
                    NewK12Course.Name = Course.CourseName;
                    NewK12Course.SchoolYear = Course.SchoolYear;
                    NewK12Course.Semester = K12.Data.Int.ParseAllowNull(Course.Semester);
                    //注意重新取得班級系統編號
                    NewK12Course.Subject = Course.Subject;
                    NewK12Course.Credit = Course.Credit;
                    NewK12Course.Period = Course.Period;
                    NewK12Course.Level = K12.Data.Decimal.ParseAllowNull(K12.Data.Int.GetString(Course.Level));
                    NewK12Course.RefClassID = mClassNameIDs.ContainsKey(Course.ClassName) ? mClassNameIDs[Course.ClassName] : string.Empty;
                    NewK12Course.CalculationFlag = Course.CalculationFlag.Equals("否") ? "2" : "1";
                    NewK12Course.Domain = Course.Domain;
                    NewK12Course.Entry = Course.Entry;
                    NewK12Course.NotIncludedInCalc = Course.NotIncludedInCalc;
                    NewK12Course.NotIncludedInCredit = Course.NotIncludedInCredit;
                    NewK12Course.RefAssessmentSetupID = string.Empty;
                    NewK12Course.Required = Course.Required.StartsWith("選") ? false : true;
                    NewK12Course.RequiredBy = Course.RequiredBy;

                    list.Add(NewK12Course);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得所有班級並依班級名稱排序
        /// </summary>
        static public Dictionary<string, string> GetClassList()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            DataTable mtblClass = tool._Q.Select("select id,class_name from class where status=1 order by grade_year");

            foreach (DataRow row in mtblClass.Rows)
            {
                string ClassName = row.Field<string>("class_name");
                string ClassID = row.Field<string>("id");

                if (!list.ContainsKey(ClassName))
                    list.Add(ClassName, ClassID);
            }

            return list;
        }

        static public List<StudObj> GetStudentByClassID(List<CourseRecord> NewCourses)
        {
            List<StudObj> list = new List<StudObj>();

            //取得所屬班級ID
            List<string> CourseClassID = new List<string>();
            foreach (CourseRecord each in NewCourses)
            {
                if (string.IsNullOrEmpty(each.RefClassID))
                    continue;

                if (!CourseClassID.Contains(each.RefClassID))
                {
                    CourseClassID.Add(each.RefClassID);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("select student.id,student.name,student.seat_no,student.student_number,class.id as classID,class.class_name ");
            sb.Append("from student join class on student.ref_class_id=class.id ");
            sb.Append(string.Format("where class.id in ('{0}') and student.status in(1,2)", string.Join("','", CourseClassID)));
            DataTable dt = tool._Q.Select(sb.ToString());

            foreach (DataRow row in dt.Rows)
            {
                StudObj obj = new StudObj(row);
                list.Add(obj);
            }

            //K12.Data.Student.SelectByClassIDs(NewCourses.Select(x => x.RefClassID));

            return list;
        }
    }
}
