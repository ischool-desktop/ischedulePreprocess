using System;

namespace Sunset
{ 
    /// <summary>
    /// 場地不排課時段
    /// </summary>
    [FISCA.UDT.TableName("scheduler.classroom_busy")]
    public class ClassroomBusy : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 場地系統編號
        /// </summary>
        [FISCA.UDT.Field(Field="ref_classroom_id")]
        public int ClassroomID { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        [FISCA.UDT.Field(Field="weekday")]
        public int WeekDay { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [FISCA.UDT.Field(Field="begin_time")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 持續時間
        /// </summary>
        [FISCA.UDT.Field(Field="duration")]
        public int Duration { get; set; }

        /// <summary>
        /// 單週雙
        /// </summary>
        [FISCA.UDT.Field(Field="week_flag")]
        public int WeekFlag { get; set; }

        /// <summary>
        /// 不排課描述
        /// </summary>
        [FISCA.UDT.Field(Field="busy_description")]
        public string BusyDesc { get; set; }
    }
}