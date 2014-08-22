using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 班級不排課時段(舊TableName)
    /// </summary>
    [FISCA.UDT.TableName("scheduler.class_busy")]
    public class ClassBusy : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 班級系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_class_id")]
        public int ClassID { get; set; }

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
    }
}
