using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sunset.NewCourse
{
    public class TACC
    {
        public TACC()
        {
            CourseList = new List<string>();
            SchList = new List<SchedulerCourseExtension>();
            SchRowIndexDic = new Dictionary<string, DataGridViewRow>();
            _ClassNameDic = new Dictionary<string, string>();
            _ClassIDDic = new Dictionary<string, string>();
            _TeacherNameDic = new Dictionary<string, string>();
            _TeacherIDDic = new Dictionary<string, string>();
        }
        /// <summary>
        /// 課程ID
        /// </summary>
        public List<string> CourseList { get; set; }

        /// <summary>
        /// 課程物件
        /// </summary>
        public List<SchedulerCourseExtension> SchList { get; set; }

        /// <summary>
        /// 課程 - 用UID查ROW
        /// </summary>
        public Dictionary<string, DataGridViewRow> SchRowIndexDic = new Dictionary<string, DataGridViewRow>();

        /// <summary>
        /// 班級 - 用Name查ID
        /// </summary>
        public Dictionary<string, string> _ClassNameDic = new Dictionary<string, string>();
        /// <summary>
        /// 班級 - 用ID反查Name
        /// </summary>
        public Dictionary<string, string> _ClassIDDic = new Dictionary<string, string>();

        /// <summary>
        /// 教師 - 用Name查ID
        /// </summary>
        public Dictionary<string, string> _TeacherNameDic = new Dictionary<string, string>();
        /// <summary>
        /// 教師 - 用ID反查Name
        /// </summary>
        public Dictionary<string, string> _TeacherIDDic = new Dictionary<string, string>();

        public int Sortstring(string x, string y)
        {
            return x.CompareTo(y);
        }

        public int SortCourseName(SchedulerCourseExtension s1, SchedulerCourseExtension s2)
        {
            //string ss_s1 = s1.Level.HasValue ? s1.Level.Value.ToString().PadLeft(1, '0') : "0";
            //string ss_s2 = s2.Level.HasValue ? s2.Level.Value.ToString().PadLeft(1, '0') : "0";

            //ss_s1 += s1.CourseName.PadLeft(30, '0');
            //ss_s2 += s2.CourseName.PadLeft(30, '0');

            return s1.CourseName.CompareTo(s2.CourseName);
        }

    }
}
