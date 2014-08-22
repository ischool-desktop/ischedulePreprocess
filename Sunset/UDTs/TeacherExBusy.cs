using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 排課專屬教師不排課清單
    /// </summary>
    [FISCA.UDT.TableName("scheduler.teacher_ex_busy")]
    public class TeacherExBusy : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 教師系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_teacher_id")]
        public int TeacherID { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        [FISCA.UDT.Field(Field = "weekday")]
        public int WeekDay { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [FISCA.UDT.Field(Field = "begin_time")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 持續時間
        /// </summary>
        [FISCA.UDT.Field(Field = "duration")]
        public int Duration { get; set; }

        /// <summary>
        /// 不排課描述
        /// </summary>
        [FISCA.UDT.Field(Field = "busy_description")]
        public string BusyDesc { get; set; }

        /// <summary>
        /// 地點系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_location_id")]
        public int? LocationID { get; set; }        
    }
}