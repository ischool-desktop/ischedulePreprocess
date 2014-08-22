using System;

namespace Sunset
{
    /// <summary>
    /// 時間表時段
    /// </summary>
    [FISCA.UDT.TableName("scheduler.timetable_section")]
    public class TimeTableSec : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 時間表系統編號
        /// </summary>
        [FISCA.UDT.Field(Field="ref_timetable_id")]
        public int TimeTableID { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        [FISCA.UDT.Field(Field = "weekday")]
        public int WeekDay { get; set; }

        /// <summary>
        /// 節次
        /// </summary>
        [FISCA.UDT.Field(Field = "period")]
        public int Period { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [FISCA.UDT.Field(Field = "begin_time")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 持續分鐘
        /// </summary>
        [FISCA.UDT.Field(Field="duration")]
        public int Duration { get; set; }

        /// <summary>
        /// 顯示節次
        /// </summary>
        [FISCA.UDT.Field(Field="display_period")]
        public int DispPeriod { get; set; }

        /// <summary>
        /// 地點系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_location_id")]
        public int? LocationID { get; set; }

        /// <summary>
        /// 是否不排課
        /// </summary>
        [FISCA.UDT.Field(Field = "disable")]
        public bool Disable { get; set; }

        /// <summary>
        /// 不可排課出現的訊息
        /// </summary>
        [FISCA.UDT.Field(Field = "disable_message")]
        public string DisableMessage { get; set; }
    }
}