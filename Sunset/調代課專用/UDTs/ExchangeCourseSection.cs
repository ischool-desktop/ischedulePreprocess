//using System;

//namespace Sunset
//{
//    /// <summary>
//    /// 調課記錄，大部份情況下，來源調課日期與目標調課日期相同。
//    /// </summary>
//    [FISCA.UDT.TableName("scheduler.exchange_course_section")]
//    public class ExchangeCourseSection : FISCA.UDT.ActiveRecord
//    { 
//        /// <summary>
//        /// 來源課程分段系統編號
//        /// </summary>
//        [FISCA.UDT.Field(Field = "src_course_section_id")]
//        public int SrcCourseSectionID { get; set; }

//        /// <summary>
//        /// 目標課程分段系統編號
//        /// </summary>
//        [FISCA.UDT.Field(Field = "des_course_section_id")]
//        public int DesCourseSectionID { get; set; }

//        /// <summary>
//        /// 來源調課日期
//        /// </summary>
//        [FISCA.UDT.Field(Field = "src_exchange_date")]
//        public DateTime SrcExchangeDate { get; set; }

//        /// <summary>
//        /// 目標調課日期
//        /// </summary>
//        [FISCA.UDT.Field(Field = "des_exchange_date")]
//        public DateTime DesExchangeDate { get; set; }

//        /// <summary>
//        /// 建立日期，包含日期及時間
//        /// </summary>
//        [FISCA.UDT.Field(Field = "create_time")]
//        public DateTime CreateTime { get; set; }

//        /// <summary>
//        /// 調課原因
//        /// </summary>
//        [FISCA.UDT.Field(Field = "reason")]
//        public string Reason { get; set; }
//    }
//}