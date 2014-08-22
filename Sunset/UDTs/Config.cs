
namespace Sunset
{
    /// <summary>
    /// 場地不排課時段
    /// </summary>
    [FISCA.UDT.TableName("scheduler.config")]
    public class Config : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [FISCA.UDT.Field(Field = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [FISCA.UDT.Field(Field = "value")]
        public string Value { get; set; }
    }
}
