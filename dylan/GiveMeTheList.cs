using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sunset.NewCourse
{
    static public class GiveMeTheList
    {
        /// <summary>
        /// 取得ischool班級清單
        /// </summary>
        static public Dictionary<string, string> GetischoolClass()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DataTable dt = tool._Q.Select("select id,class_name from class where status=1 order by grade_year,display_order,class_name");
            foreach (DataRow row in dt.Rows)
            {
                string ClassName = row.Field<string>("class_name");
                string ClassID = row.Field<string>("id");
                if (!dic.ContainsKey(ClassName))
                {
                    dic.Add(ClassName, ClassID);
                }
            }
            return dic;
        }

        /// <summary>
        /// 取得排課課程清單
        /// </summary>
        static public Dictionary<string, string> GetSunsetClass()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            DataTable dt = tool._Q.Select("select uid,class_name from $scheduler.class_ex order by grade_year,class_name");

            foreach (DataRow row in dt.Rows)
            {
                string ClassName = row.Field<string>("class_name");
                string ClassID = row.Field<string>("uid");

                if (!dic.ContainsKey(ClassName))
                {
                    dic.Add(ClassName, ClassID);
                }
            }
            return dic;
        }

        /// <summary>
        /// 取得ischool教師清單
        /// </summary>
        static public Dictionary<string, string> GetischoolTeacher()
        {
            DataTable mtblTeacher = tool._Q.Select("select id,(CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) as teachername from teacher where status=1 order by teachername");

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (DataRow row in mtblTeacher.Rows)
            {
                string TeacherName = row.Field<string>("teachername");
                string TeacherID = row.Field<string>("id");

                if (!dic.ContainsKey(TeacherName))
                    dic.Add(TeacherName, TeacherID);
            }
            return dic;
        }

        /// <summary>
        /// 取得排課教師清單
        /// </summary>
        static public Dictionary<string, string> GetSunsetTeacher()
        {
            DataTable mtblTeacher = tool._Q.Select("select * from $scheduler.teacher_ex order by teacher_name");

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (DataRow row in mtblTeacher.Rows)
            {
                string TeacherName = row.Field<string>("teacher_name");
                string nickname = row.Field<string>("nickname");
                string uid = row.Field<string>("uid");

                if (!string.IsNullOrEmpty(nickname))
                {
                    if (!dic.ContainsKey(TeacherName + "(" + nickname + ")"))
                        dic.Add(TeacherName + "(" + nickname + ")", uid);
                }
                else
                {
                    //加暱稱
                    if (!dic.ContainsKey(TeacherName))
                        dic.Add(TeacherName, uid);
                }
            }
            return dic;
        }
    }
}
