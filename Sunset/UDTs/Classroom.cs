
namespace Sunset
{
    /// <summary>
    /// 場地
    /// </summary>
    [FISCA.UDT.TableName("scheduler.classroom")]
    public class Classroom : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 場地代碼
        /// </summary>
        [FISCA.UDT.Field(Field="code")]
        public string ClassroomCode { get; set; }

        /// <summary>
        /// 場地名稱
        /// </summary>
        [FISCA.UDT.Field(Field="name")]
        public string ClassroomName { get; set; }

        /// <summary>
        /// 容納數
        /// </summary>
        [FISCA.UDT.Field(Field="capacity")]
        public int Capacity { get; set; }

        /// <summary>
        /// 無使用限制
        /// </summary>
        [FISCA.UDT.Field(Field = "location_only")]
        public bool LocationOnly { get; set; }

        /// <summary>
        /// 場地描述
        /// </summary>
        [FISCA.UDT.Field(Field="description")]
        public string ClassroomDesc { get; set; }

        /// <summary>
        /// 地點系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_location_id")]
        public int? LocationID { get; set; }
    }
}