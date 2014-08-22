//using System;

//namespace Sunset
//{
//    /// <summary>
//    /// 代課記錄，針對某個課程分段記錄代課教師、假別及鐘點費…等。
//    /// </summary>
//    [FISCA.UDT.TableName("scheduler.substitute_course_section")]
//    public class SubstituteCourseSection : FISCA.UDT.ActiveRecord
//    {
//        /// <summary>
//        /// 課程分段系統編號
//        /// </summary>
//        [FISCA.UDT.Field(Field = "ref_course_section_id")]
//        public int RefCourseSectionID { get; set; }

//        /// <summary>
//        /// 代課日期
//        /// </summary>
//        [FISCA.UDT.Field(Field = "substitute_date")]
//        public DateTime SubstituteDate { get; set; }

//        /// <summary>
//        /// 建立日期，包含日期及時間
//        /// </summary>
//        [FISCA.UDT.Field(Field = "create_time")]
//        public DateTime CreateTime { get; set; }

//        /// <summary>
//        /// 代課教師系統編號
//        /// </summary>
//        [FISCA.UDT.Field(Field = "ref_teacher_id")]
//        public int RefTeacherID  { get; set;}

//        /// <summary>
//        /// 假別名稱
//        /// </summary>
//        [FISCA.UDT.Field(Field = "absence_name")]
//        public string AbsenceName { get; set; }

//        /// <summary>
//        /// 鐘點費
//        /// </summary>
//        [FISCA.UDT.Field(Field = "hourly_pay")]
//        public int HourlyPay { get; set; }

//        /// <summary>
//        /// 代課原因
//        /// </summary>
//        [FISCA.UDT.Field(Field = "reason")]
//        public string Reason { get; set; }
//    }
//}