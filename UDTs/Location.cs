
namespace Sunset
{
    /// <summary>
    /// 地點
    /// </summary>
    [FISCA.UDT.TableName("scheduler.location")]
    public class Location : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 地點名稱
        /// </summary>
        [FISCA.UDT.Field(Field = "name")]
        public string LocationName { get; set; }
    }
}