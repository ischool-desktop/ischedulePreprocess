//using System;
//using FISCA.UDT;

//namespace Sunset
//{
//    /// <summary>
//    /// 週課程分段
//    /// </summary>
//    [FISCA.UDT.TableName("scheduler.week_course_section")]
//    public class WeekCourseSection : ActiveRecord
//    {
//        /// <summary>
//        /// 課程編號
//        /// </summary>
//        [FISCA.UDT.Field(Field="ref_course_id")]
//        public int CourseID { get; set; }

//        /// <summary>
//        /// 課程分段日期
//        /// </summary>
//        [FISCA.UDT.Field(Field = "date")]
//        public DateTime Date { get; set; }

//        /// <summary>
//        /// 週次
//        /// </summary>
//        [FISCA.UDT.Field(Field = "week")]
//        public int Week { get; set; }

//        /// <summary>
//        /// 星期
//        /// </summary>
//        [FISCA.UDT.Field(Field="weekday")]
//        public int WeekDay { get; set; }

//        /// <summary>
//        /// 節次
//        /// </summary>
//        [FISCA.UDT.Field(Field="period")]
//        public int Period { get; set; }

//        /// <summary>
//        /// 節數
//        /// </summary>
//        [FISCA.UDT.Field(Field="length")]
//        public int Length { get; set; }

//        /// <summary>
//        /// 星期條件
//        /// </summary>
//        [FISCA.UDT.Field(Field="weekday_condition")]
//        public string WeekDayCond { get; set; }

//        /// <summary>
//        /// 節次條件
//        /// </summary>
//        [FISCA.UDT.Field(Field="period_condition")]
//        public string PeriodCond { get; set; }

//        /// <summary>
//        /// 單雙週
//        /// </summary>
//        [FISCA.UDT.Field(Field="week_flag")]
//        public int WeekFlag { get; set; }

//        /// <summary>
//        /// 跨中午
//        /// </summary>
//        [FISCA.UDT.Field(Field="long_break")]
//        public bool LongBreak { get; set; }

//        /// <summary>
//        /// 鎖定
//        /// </summary>
//        [FISCA.UDT.Field(Field="lock")]
//        public bool Lock { get; set; }

//        /// <summary>
//        /// 教室系統編號
//        /// </summary>
//        [FISCA.UDT.Field(Field = "ref_classroom_id")]
//        public int? ClassroomID { get; set; }
//    }
//}