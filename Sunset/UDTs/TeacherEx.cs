using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 排課專屬教師清單
    /// </summary>
    [FISCA.UDT.TableName("scheduler.teacher_ex")]
    public class TeacherEx : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 教師姓名
        /// </summary>
        [FISCA.UDT.Field(Field="teacher_name")]
        public string TeacherName { get; set; }

        /// <summary>
        /// 教師暱稱
        /// </summary>
        [FISCA.UDT.Field(Field="nickname")]
        public string NickName { get; set; }

        /// <summary>
        /// 完整教師名稱
        /// </summary>
        public string FullTeacherName 
        {
            get
            {
                return string.IsNullOrEmpty(NickName) ? TeacherName : TeacherName + "(" + NickName + ")";
            }
        }
    }
}