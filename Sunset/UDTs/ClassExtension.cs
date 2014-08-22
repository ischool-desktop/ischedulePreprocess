
namespace Sunset
{
    /// <summary>
    /// 班級延伸資訊
    /// </summary>
    [FISCA.UDT.TableName("scheduler.class_extension")]
    public class ClassExtension : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 班級系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_class_id")]
        public int ClassID { get; set; }

        /// <summary>
        /// 時間表系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_timetable_id")]
        public int? TimeTableID { get; set; }
    }
}