
namespace Sunset
{
    /// <summary>
    /// 排課使用教師延伸資訊
    /// </summary>
    [FISCA.UDT.TableName("scheduler.teacher_extension")]
    public class TeacherExtension : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 課程系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_teacher_id")]
        public int TeacherID { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        [FISCA.UDT.Field(Field = "comment")]
        public string Comment { get; set; }

        /// <summary>
        /// 基本節數
        /// </summary>
        [FISCA.UDT.Field(Field = "basic_length")]
        public int? BasicLength { get; set; }

        /// <summary>
        /// 兼課節數
        /// </summary>
        [FISCA.UDT.Field(Field = "extra_length")]
        public int? ExtraLength { get; set; }

        /// <summary>
        /// 輔導節數
        /// </summary>
        [FISCA.UDT.Field(Field = "counseling_length")]
        public int? CounselingLength { get; set; }
    }
}