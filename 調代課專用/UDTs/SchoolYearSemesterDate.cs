//using System;

//namespace Sunset
//{
//    /// <summary>
//    /// 學年度學期日期對照，調代課系統用來產生週課程分段
//    /// </summary>
//    [FISCA.UDT.TableName("scheduler.schoolyear_semester_date")]
//    public class SchoolYearSemesterDate : FISCA.UDT.ActiveRecord
//    {
//        /// <summary>
//        /// 學年度
//        /// </summary>
//        [FISCA.UDT.Field(Field = "schoolyear")]
//        public string SchoolYear { get; set; }

//        /// <summary>
//        /// 學期
//        /// </summary>
//        [FISCA.UDT.Field(Field = "semester")]
//        public string Semester { get; set; }

//        /// <summary>
//        /// 開始日期
//        /// </summary>
//        [FISCA.UDT.Field(Field = "start_date")]
//        public DateTime StartDate { get; set; }

//        /// <summary>
//        /// 結束日期
//        /// </summary>
//        [FISCA.UDT.Field(Field = "end_date")]
//        public DateTime EndDate { get; set; }
//    }
//}