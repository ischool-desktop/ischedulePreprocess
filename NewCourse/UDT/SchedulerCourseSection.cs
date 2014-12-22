
namespace Sunset.NewCourse
{
    /// <summary>
    /// 課程分段
    /// </summary>
    [FISCA.UDT.TableName("scheduler.scheduler_course_section")]
    public class SchedulerCourseSection : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 淺層複製SchedulerCourseExtension
        /// </summary>
        public SchedulerCourseSection CopyExtension()
        {
            return (SchedulerCourseSection)this.MemberwiseClone();
        }

        /// <summary>
        /// 課程編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_course_id")]
        public int CourseID { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        [FISCA.UDT.Field(Field = "weekday")]
        public int WeekDay { get; set; }

        /// <summary>
        /// 節次
        /// </summary>
        [FISCA.UDT.Field(Field = "period")]
        public int Period { get; set; }

        /// <summary>
        /// 節數
        /// </summary>
        [FISCA.UDT.Field(Field = "length")]
        public int Length { get; set; }

        /// <summary>
        /// 星期條件
        /// </summary>
        [FISCA.UDT.Field(Field = "weekday_condition")]
        public string WeekDayCond { get; set; }

        /// <summary>
        /// 節次條件
        /// </summary>
        [FISCA.UDT.Field(Field = "period_condition")]
        public string PeriodCond { get; set; }

        /// <summary>
        /// 單雙週
        /// </summary>
        [FISCA.UDT.Field(Field = "week_flag")]
        public int WeekFlag { get; set; }

        /// <summary>
        /// 跨中午
        /// </summary>
        [FISCA.UDT.Field(Field = "long_break")]
        public bool LongBreak { get; set; }

        /// <summary>
        /// 鎖定
        /// </summary>
        [FISCA.UDT.Field(Field = "lock")]
        public bool Lock { get; set; }

        /// <summary>
        /// 教室系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_classroom_id")]
        public int? ClassroomID { get; set; }

        /// <summary>
        /// 授課教師一
        /// </summary>
        [FISCA.UDT.Field(Field = "teacher_name_1")]
        public string TeacherName1 { get; set; }

        /// <summary>
        /// 授課教師二
        /// </summary>
        [FISCA.UDT.Field(Field = "teacher_name_2")]
        public string TeacherName2 { get; set; }

        /// <summary>
        /// 授課教師三
        /// </summary>
        [FISCA.UDT.Field(Field = "teacher_name_3")]
        public string TeacherName3 { get; set; }

        /// <summary>
        /// 授課教師系統編號一
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_teacher_id_1")]
        public int? TeacherID1 { get; set; }

        /// <summary>
        /// 授課教師系統編號二
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_teacher_id_2")]
        public int? TeacherID2 { get; set; }

        /// <summary>
        /// 授課教師系統編號三
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_teacher_id_3")]
        public int? TeacherID3 { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        [FISCA.UDT.Field(Field = "comment")]
        public string Comment { get; set; }

        /// <summary>
        /// 比對課程分段物件是否相同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            SchedulerCourseSection Target = obj as  SchedulerCourseSection;

            if (Target == null) return false;

            if (this.Length != Target.Length)
                return false;

            if (this.WeekFlag != Target.WeekFlag)
                return false;

            if (this.WeekDayCond != Target.WeekDayCond)
                return false;

            if (this.PeriodCond != Target.PeriodCond)
                return false;

            if (this.LongBreak != Target.LongBreak)
                return false;

            return true;
        }

        /// <summary>
        /// 取得雜湊代碼
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
