using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sunset
{
    /// <summary>
    /// 學生功課表
    /// </summary>
    public class StudentObj
    {
        /// <summary>
        /// 學生系統編號
        /// </summary>
        public string StudentID { get; set; }

        /// <summary>
        /// 班級編號
        /// </summary>
        public string ClassID { get; set; }

        /// <summary>
        /// 班級
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 座號
        /// </summary>
        public string SeatNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 學號
        /// </summary>
        public string StudentNumber { get; set; }

        /// <summary>
        /// 功課表細項
        /// 
        /// </summary>
        public List<n_PeriodObj> Items { get; set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public StudentObj(DataRow each)
        {
            Items = new List<n_PeriodObj>();

            StudentID = "" + each["student_id"];
            StudentName = "" + each["student_name"];
            ClassID = "" + each["ref_class_id"];
            SeatNo = "" + each["seat_no"];
            StudentNumber = "" + each["student_number"];
            ClassName = "" + each["class_name"];
        }

        public string GetPeriod(DataRow each)
        {
            StringBuilder sb = new StringBuilder();

            string d = "" + each["course_name"];
            string f = "" + each["teacher_name_1"];
            string g = "" + each["teacher_name_2"];
            string h = "" + each["teacher_name_3"];
            string i = "" + each["classroom_name"];

            int a = 0;
            int b = 0;
            int c = 0;

            if (!int.TryParse("" + each["period"], out a))
            {
                sb.Append("課程「" + d + "」「節次」資料錯誤!!");
            }
            if (!int.TryParse("" + each["weekday"], out b))
            {
                sb.Append("課程「" + d + "」「星期」資料錯誤!!");
            }

            if (!int.TryParse("" + each["length"], out c))
            {
                sb.Append("課程「" + d + "」「節數」資料錯誤!!");
            }

            n_PeriodObj n = new n_PeriodObj();
            n.Period = a;
            n.WeekDay = b;
            n.Length = c;
            n.CourseName = d;
            n.上課導師1 = f;
            n.上課導師2 = g;
            n.上課導師3 = h;
            n.上課場地 = i;
            Items.Add(n);

            return sb.ToString();
        }
    }
}