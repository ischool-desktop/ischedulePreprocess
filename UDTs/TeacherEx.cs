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

        /// <summary>
        /// 教師代碼
        /// </summary>
        [FISCA.UDT.Field(Field = "teacher_code")]
        public string TeacherCode { get; set; }

        /// <summary>
        /// 教學專長
        /// </summary>
        [FISCA.UDT.Field(Field = "teaching_expertise")]
        public string TeachingExpertise { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        [FISCA.UDT.Field(Field = "note")]
        public string Note { get; set; }
    }
}