using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.NewCourse
{
    class Log_CourseExtension
    {
        public string CourseName { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public string Subject { get; set; }
        public int? Level { get; set; }
        public int? Credit { get; set; }
        public int? Period { get; set; }

        public string TeacherName1 { get; set; }
        public string TeacherName2 { get; set; }
        public string TeacherName3 { get; set; }
        public string ClassName { get; set; }
        public string Domain { get; set; }

        public string Entry { get; set; }
        public string CalculationFlag { get; set; }
        public bool NotIncludedInCalc { get; set; }
        public bool NotIncludedInCredit { get; set; }
        public string Required { get; set; }
        public string RequiredBy { get; set; }
        public string SubjectAliasName { get; set; }
        public string SplitSpec { get; set; }
        public int WeekFlag { get; set; }
        public bool AllowDup { get; set; }

        public bool LimitNextDay { get; set; }
        public int? ClassroomID { get; set; }
        public bool LongBreak { get; set; }
        public string WeekDayCond { get; set; }
        public string PeriodCond { get; set; }
        public bool NoQuery { get; set; }

        /// <summary>
        /// 場地字典
        /// </summary>
        private Dictionary<string, string> _dic { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public Log_CourseExtension(SchedulerCourseExtension low_sce, Dictionary<string, string> dic)
        {
            _dic = dic;
            CourseName = low_sce.CourseName;
            SchoolYear = low_sce.SchoolYear.ToString();
            Semester = low_sce.Semester;
            Subject = low_sce.Subject;
            Level = low_sce.Level;
            Credit = low_sce.Credit;
            Period = low_sce.Period;
            TeacherName1 = low_sce.TeacherName1;
            TeacherName2 = low_sce.TeacherName2;
            TeacherName3 = low_sce.TeacherName3;
            ClassName = low_sce.ClassName;
            Domain = low_sce.Domain;
            Entry = low_sce.Entry;
            CalculationFlag = low_sce.CalculationFlag; //是否列入學期成績
            NotIncludedInCalc = low_sce.NotIncludedInCalc; //評分設定
            NotIncludedInCredit = low_sce.NotIncludedInCredit; //學分設定
            Required = low_sce.Required;
            RequiredBy = low_sce.RequiredBy;
            SubjectAliasName = low_sce.SubjectAliasName;
            SplitSpec = low_sce.SplitSpec;
            WeekFlag = low_sce.WeekFlag;
            AllowDup = low_sce.AllowDup;
            LimitNextDay = low_sce.LimitNextDay;
            ClassroomID = low_sce.ClassroomID; //班級
            LongBreak = low_sce.LongBreak;

            WeekDayCond = low_sce.WeekDayCond;
            PeriodCond = low_sce.PeriodCond;

            NoQuery = low_sce.NoQuery;
        }

        /// <summary>
        /// 取得Log修改字串
        /// </summary>
        public string GetLog(SchedulerCourseExtension new_sce)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("已修改「排課課程：" + CourseName + "」基本資料:");

            #region Log

            if (CourseName != new_sce.CourseName)
                sb.AppendLine(Course_Log.SetUpdataValue("課程名稱", CourseName, new_sce.CourseName));

            if (SchoolYear != new_sce.SchoolYear.ToString())
                sb.AppendLine(Course_Log.SetUpdataValue("學年度", SchoolYear, new_sce.SchoolYear.ToString()));

            if (Semester != new_sce.Semester.ToString())
                sb.AppendLine(Course_Log.SetUpdataValue("學期", Semester, new_sce.Semester));

            if (Subject != new_sce.Subject)
                sb.AppendLine(Course_Log.SetUpdataValue("科目名稱", Subject, new_sce.Subject));

            if (Course_Log.SetPriValueBool(Level, new_sce.Level))
                sb.AppendLine(Course_Log.SetPerValue("級別", Level, new_sce.Level));

            if (Course_Log.SetPriValueBool(Credit, new_sce.Credit))
                sb.AppendLine(Course_Log.SetPerValue("學分數", Credit, new_sce.Credit));

            if (Course_Log.SetPriValueBool(Period, new_sce.Period))
                sb.AppendLine(Course_Log.SetPerValue("節數", Period, new_sce.Period));

            if (TeacherName1 != new_sce.TeacherName1)
                sb.AppendLine(Course_Log.SetUpdataValue("授課教師1", TeacherName1, new_sce.TeacherName1));

            if (TeacherName2 != new_sce.TeacherName2)
                sb.AppendLine(Course_Log.SetUpdataValue("授課教師2", TeacherName2, new_sce.TeacherName2));

            if (TeacherName3 != new_sce.TeacherName3)
                sb.AppendLine(Course_Log.SetUpdataValue("授課教師3", TeacherName3, new_sce.TeacherName3));

            if (ClassName != new_sce.ClassName)
                sb.AppendLine(Course_Log.SetUpdataValue("所屬班級", ClassName, new_sce.ClassName));

            if (Domain != new_sce.Domain)
                sb.AppendLine(Course_Log.SetUpdataValue("領域", Domain, new_sce.Domain));

            if (Entry != new_sce.Entry)
                sb.AppendLine(Course_Log.SetUpdataValue("分項類別", Entry, new_sce.Entry));

            if (CalculationFlag != new_sce.CalculationFlag)
                sb.AppendLine(Course_Log.SetUpdataValue("學期成績", "列入", "不列入"));

            if (NotIncludedInCalc != new_sce.NotIncludedInCalc)
                sb.AppendLine(Course_Log.SetBool("評分設定", "評分", "不評分", new_sce.NotIncludedInCalc));

            if (NotIncludedInCredit != new_sce.NotIncludedInCredit)
                sb.AppendLine(Course_Log.SetBool("學分設定", "計入", "不計入", new_sce.NotIncludedInCredit));

            if (Required != new_sce.Required)
                sb.AppendLine(Course_Log.SetUpdataValue("必選修", Required, new_sce.Required));

            if (RequiredBy != new_sce.RequiredBy)
                sb.AppendLine(Course_Log.SetUpdataValue("校部訂", RequiredBy, new_sce.RequiredBy));

            if (SubjectAliasName != new_sce.SubjectAliasName)
                sb.AppendLine(Course_Log.SetUpdataValue("科目簡稱", SubjectAliasName, new_sce.SubjectAliasName));

            if (SplitSpec != new_sce.SplitSpec)
                sb.AppendLine(Course_Log.SetUpdataValue("分割設定", SplitSpec, new_sce.SplitSpec));

            //3單雙,2雙,1單
            if (WeekFlag != new_sce.WeekFlag)
                sb.AppendLine(Course_Log.SetUpdataValue("單雙週條件", Course_Log.GetWeekFlagName(WeekFlag), Course_Log.GetWeekFlagName(new_sce.WeekFlag)));

            if (SplitSpec != new_sce.SplitSpec)
                sb.AppendLine(Course_Log.SetUpdataValue("分割設定", SplitSpec, new_sce.SplitSpec));

            if (AllowDup != new_sce.AllowDup)
                sb.AppendLine(Course_Log.SetBool("同天排課", "否", "是", new_sce.AllowDup));

            if (LimitNextDay != new_sce.LimitNextDay)
                sb.AppendLine(Course_Log.SetBool("隔天排課", "否", "是", new_sce.LimitNextDay));

            //場地條件

            #region Classroom
            string Classroomx1 = "";
            if (ClassroomID.HasValue)
            {
                if (_dic.ContainsKey(ClassroomID.Value.ToString()))
                {
                    Classroomx1 = _dic[ClassroomID.Value.ToString()];
                }
            }
            string Classroomx2 = "";
            if (new_sce.ClassroomID.HasValue)
            {
                if (_dic.ContainsKey(new_sce.ClassroomID.Value.ToString()))
                {
                    Classroomx2 = _dic[new_sce.ClassroomID.Value.ToString()];
                }
            }

            if (Classroomx1 != Classroomx2)
                sb.AppendLine(Course_Log.SetUpdataValue("場地條件", Classroomx1, Classroomx2));
            #endregion


            if (LongBreak != new_sce.LongBreak)
                sb.AppendLine(Course_Log.SetBool("可跨中午", "否", "是", new_sce.LongBreak));

            if (WeekDayCond != new_sce.WeekDayCond)
                sb.AppendLine(Course_Log.SetUpdataValue("星期條件", WeekDayCond, new_sce.WeekDayCond));

            if (PeriodCond != new_sce.PeriodCond)
                sb.AppendLine(Course_Log.SetUpdataValue("節次條件", PeriodCond, new_sce.PeriodCond));

            if (NoQuery != new_sce.NoQuery)
                sb.AppendLine(Course_Log.SetBool("開放查詢", "開放", "不開放", new_sce.NoQuery));

            #endregion

            return sb.ToString();
        }
    }
}
