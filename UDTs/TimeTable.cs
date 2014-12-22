
namespace Sunset
{
    /// <summary>
    /// 時間表
    /// </summary>
    [FISCA.UDT.TableName("scheduler.timetable")]
    public class TimeTable : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 時間表名稱
        /// </summary>
        [FISCA.UDT.Field(Field = "name")]
        public string TimeTableName { get; set; }

        /// <summary>
        /// 時間表描述
        /// </summary>
        [FISCA.UDT.Field(Field = "description")]
        public string TimeTableDesc { get; set; }
    }
}