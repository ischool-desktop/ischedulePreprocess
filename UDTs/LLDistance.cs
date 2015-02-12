
namespace Sunset
{
    /// <summary>
    /// 地點距離(沒用到)
    /// </summary>
    [FISCA.UDT.TableName("scheduler.ll_distance")]
    public class LLDistance : FISCA.UDT.ActiveRecord
    { 
        /// <summary>
        /// 來源地點系統編號
        /// </summary>
        [FISCA.UDT.Field(Field="ref_locationa_id")]
        public int LocationAID { get; set; }

        /// <summary>
        /// 目的地點系統編號
        /// </summary>
        [FISCA.UDT.Field(Field="ref_llocationb_id")]
        public int LocationBID { get; set; }

        /// <summary>
        /// 開車時間
        /// </summary>
        [FISCA.UDT.Field(Field="drive_time")]
        public int DriveTime { get; set; }

        /// <summary>
        /// 距離
        /// </summary>
        [FISCA.UDT.Field(Field = "distance")]
        public int Distance { get; set; }
    }
}