
namespace Sunset.NewCourse
{
    /// <summary>
    /// 排課使用課程延伸資訊
    /// </summary>
    [FISCA.UDT.TableName("scheduler.scheduler_course_extension")]
    public class SchedulerCourseExtension : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        public SchedulerCourseExtension()
        {
            Domain = string.Empty;
            Entry = string.Empty;
            NotIncludedInCalc = false;
            NotIncludedInCredit = false;
            Required = string.Empty;
            RequiredBy = string.Empty;

            //不開放查詢 - 預設true不開放
            NoQuery = true;

            //單雙週預設為"單雙"
            WeekFlag = 3;

            //是否列入學期成績 - 預設為"是"
            CalculationFlag = "是";
        }

        #region 課程基本資料
        /// <summary>
        /// 課程名稱
        /// </summary>
        [FISCA.UDT.Field(Field="course_name")]
        public string CourseName { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        [FISCA.UDT.Field(Field="school_year")]
        public int SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        [FISCA.UDT.Field(Field="semester")]
        public string Semester { get; set; }

        /// <summary>
        /// 科目名稱
        /// </summary>
        [FISCA.UDT.Field(Field="subject")]
        public string Subject { get; set; }

        /// <summary>
        /// 級別
        /// </summary>
        [FISCA.UDT.Field(Field="level")]
        public int? Level { get; set; }

        /// <summary>
        /// 學分數
        /// </summary>
        [FISCA.UDT.Field(Field="credit")]
        public int? Credit { get; set; }

        /// <summary>
        /// 節數
        /// </summary>
        [FISCA.UDT.Field(Field="period")]
        public int? Period { get; set; }

        /// <summary>
        /// 預設課程授課教師一
        /// </summary>
        [FISCA.UDT.Field(Field="teacher_name_1")]
        public string TeacherName1 { get; set; }

        /// <summary>
        /// 預設課程授課教師二
        /// </summary>
        [FISCA.UDT.Field(Field = "teacher_name_2")]
        public string TeacherName2 { get; set; }

        /// <summary>
        /// 預設課程授課教師三
        /// </summary>
        [FISCA.UDT.Field(Field = "teacher_name_3")]
        public string TeacherName3 { get; set; }

        /// <summary>
        /// 班級名稱
        /// </summary>
        [FISCA.UDT.Field(Field="class_name")]
        public string ClassName { get; set; }

        /// <summary>
        /// 班級系統編號
        /// </summary>
        [FISCA.UDT.Field(Field="ref_class_id")]
        public int? ClassID { get; set; }

        /// <summary>
        /// ischool課程系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_course_id")]
        public int? CourseID { get; set; }
        #endregion

        #region
        /// <summary>
        /// 領域
        /// </summary>
        [FISCA.UDT.Field(Field ="domain")]
        public string Domain { get; set; }

        /// <summary>
        /// 分項
        /// </summary>
        [FISCA.UDT.Field(Field = "entry")]
        public string Entry { get; set; }

        /// <summary>
        /// 是否列入學期成績計算
        /// </summary>
        [FISCA.UDT.Field(Field = "calculation_flag")]
        public string CalculationFlag { get; set;}

        /// <summary>
        /// 不計分(國中不使用)
        /// </summary>
        [FISCA.UDT.Field(Field = "not_included_in_calc")]
        public bool NotIncludedInCalc { get; set; }

        /// <summary>
        /// 不計入學分(國中不使用)
        /// </summary>
        [FISCA.UDT.Field(Field = "not_included_in_credit")]
        public bool NotIncludedInCredit { get; set;}

        /// <summary>
        /// 必選修
        /// </summary>
        [FISCA.UDT.Field(Field = "required")]
        public string Required { get; set; }

        /// <summary>
        /// 校部訂
        /// </summary>
        [FISCA.UDT.Field(Field = "required_by")]
        public string RequiredBy { get; set; }

        //[FISCA.UDT.Field(Field = "ref_assessment_setup_id")]
        //public string RefAssessmentSetupID { get; set; }
        #endregion

        #region 排課相關屬性
        /// <summary>
        /// 課程簡稱
        /// </summary>
        [FISCA.UDT.Field(Field = "subject_alias_name")]
        public string SubjectAliasName { get; set; }

        /// <summary>
        /// 課程所屬時間表編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_timetable_id")]
        public int? TimeTableID { get; set; }

        /// <summary>
        /// 課程分段分割設定
        /// </summary>
        [FISCA.UDT.Field(Field = "split_spec")]
        public string SplitSpec { get; set; }

        /// <summary>
        /// 課程分段樣版，單雙週
        /// </summary>
        [FISCA.UDT.Field(Field = "week_flag")]
        public int WeekFlag { get; set; }

        /// <summary>
        /// 課程分段樣版，是否允許課程分段排在同一天
        /// </summary>
        [FISCA.UDT.Field(Field = "allow_duplicate")]
        public bool AllowDup { get; set; }

        /// <summary>
        /// 課程分段樣版，是否限制課程與課程分段間，不能排在隔天，例如週一上國文、週二也上國文
        /// </summary>
        [FISCA.UDT.Field(Field = "limit_next_day")]
        public bool LimitNextDay { get; set; }

        /// <summary>
        /// 課程分段樣版，教室編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_classroom_id")]
        public int? ClassroomID { get; set; }

        /// <summary>
        /// 課程分段樣版，是否允許排中午時段
        /// </summary>
        [FISCA.UDT.Field(Field = "long_break")]
        public bool LongBreak { get; set; }

        /// <summary>
        /// 課程分段樣版，星期條件
        /// </summary>
        [FISCA.UDT.Field(Field = "weekday_condition")]
        public string WeekDayCond { get; set; }

        /// <summary>
        /// 課程分段樣版，節次條件
        /// </summary>
        [FISCA.UDT.Field(Field = "period_condition")]
        public string PeriodCond { get; set; }

        /// <summary>
        /// 課程所屬群組，必須要課程分段數目以及屬性（除了場地）外都一樣
        /// </summary>
        [FISCA.UDT.Field(Field = "course_group")]
        public string CourseGroup { get; set; }

        /// <summary>
        /// 不開放查詢
        /// </summary>
        [FISCA.UDT.Field(Field = "no_query")]
        public bool NoQuery { get; set; }
        #endregion
    }
}