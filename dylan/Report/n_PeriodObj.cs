using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 功課表節次項目
    /// </summary>
    public class n_PeriodObj
    {
        public n_PeriodObj()
        {

        }

        public string GetCourseName()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CourseName);

            if (!string.IsNullOrEmpty(GetTeacherName()))
            {
                sb.AppendLine();
                sb.Append(GetTeacherName());
            }
            if (!string.IsNullOrEmpty(上課場地))
            {
                sb.AppendLine();
                sb.Append(上課場地);
            }

            return sb.ToString();
        }

        private string GetTeacherName()
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(上課導師1))
            {
                list.Add(上課導師1);
            }

            if (!string.IsNullOrEmpty(上課導師2))
            {
                list.Add(上課導師2);
            }

            if (!string.IsNullOrEmpty(上課導師3))
            {
                list.Add(上課導師3);
            }

            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Join("，", list));
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 節次編號
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 長度
        /// </summary>
        public int Length { get; set; }

        public string 上課場地 { get; set; }

        public string 上課導師1 { get; set; }

        public string 上課導師2 { get; set; }

        public string 上課導師3 { get; set; }
    }
}