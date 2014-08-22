using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.NewCourse
{
    class Log_CourseSection
    {
        List<SchedulerCourseSection> _SectionsList { get; set; }

        Dictionary<string, SchedulerCourseSection> _SectionsDic { get; set; }
        /// <summary>
        /// 場地字典
        /// </summary>
        private Dictionary<string, string> _dic { get; set; }

        public Log_CourseSection(List<SchedulerCourseSection> mCourseSections, Dictionary<string, string> dic)
        {
            _dic = dic;
            _SectionsList = new List<SchedulerCourseSection>();
            _SectionsDic = new Dictionary<string, SchedulerCourseSection>();

            foreach (SchedulerCourseSection sch in mCourseSections)
            {
                _SectionsList.Add(sch.CopyExtension());

                if (!_SectionsDic.ContainsKey(sch.UID))
                {
                    _SectionsDic.Add(sch.UID, sch.CopyExtension());
                }
            }
        }

        /// <summary>
        /// 取得Log修改字串
        /// </summary>
        public string GetLog(List<SchedulerCourseSection> new_Sections, SchedulerCourseExtension Course)
        {
            //整理出新增資料與更新資料
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("已調整課程分段資料：\n(學年度「{0}」學期「{1}」課程「{2}」)", Course.SchoolYear.ToString(), Course.Semester, Course.CourseName));
            sb.AppendLine("");
            foreach (SchedulerCourseSection new_section in new_Sections)
            {
                if (!new_section.Deleted)
                {
                    //包含於字典的就是原資料的修改
                    if (_SectionsDic.ContainsKey(new_section.UID))
                    {
                        SchedulerCourseSection low_section = _SectionsDic[new_section.UID];

                        if (CheckChange(new_section, low_section))
                        {
                            #region 修改

                            sb.AppendLine(string.Format("修改課程分段(星期「{0}」節次「{1}」)：", low_section.WeekDay.ToString(), new_section.Period.ToString()));
                            if (low_section.WeekDay != new_section.WeekDay)
                                sb.AppendLine(Course_Log.SetUpdataValue("星期", low_section.WeekDay.ToString(), new_section.WeekDay.ToString()));

                            if (low_section.Period != new_section.Period)
                                sb.AppendLine(Course_Log.SetUpdataValue("節次", low_section.Period.ToString(), new_section.Period.ToString()));

                            if (low_section.Length != new_section.Length)
                                sb.AppendLine(Course_Log.SetUpdataValue("節數", low_section.Length.ToString(), new_section.Length.ToString()));

                            if (low_section.LongBreak != new_section.LongBreak)
                                sb.AppendLine(Course_Log.SetUpdataValue("跨中午", Course_Log.GetLongBreak(low_section.LongBreak), Course_Log.GetLongBreak(new_section.LongBreak)));

                            if (low_section.WeekFlag != new_section.WeekFlag)
                                sb.AppendLine(Course_Log.SetUpdataValue("單雙週", Course_Log.GetWeekFlagName(low_section.WeekFlag), Course_Log.GetWeekFlagName(new_section.WeekFlag)));

                            if (low_section.WeekDayCond != new_section.WeekDayCond)
                                sb.AppendLine(Course_Log.SetUpdataValue("星期條件", low_section.WeekDayCond, new_section.WeekDayCond));

                            if (low_section.PeriodCond != new_section.PeriodCond)
                                sb.AppendLine(Course_Log.SetUpdataValue("節次條件", low_section.PeriodCond, new_section.PeriodCond));

                            #region Classroom
                            string Classroomx1 = "";
                            if (low_section.ClassroomID.HasValue)
                            {
                                if (_dic.ContainsKey(low_section.ClassroomID.Value.ToString()))
                                {
                                    Classroomx1 = _dic[low_section.ClassroomID.Value.ToString()];
                                }
                            }
                            string Classroomx2 = "";
                            if (new_section.ClassroomID.HasValue)
                            {
                                if (_dic.ContainsKey(new_section.ClassroomID.Value.ToString()))
                                {
                                    Classroomx2 = _dic[new_section.ClassroomID.Value.ToString()];
                                }
                            }

                            if (Classroomx1 != Classroomx2)
                                sb.AppendLine(Course_Log.SetUpdataValue("上課場地", Classroomx1, Classroomx2));
                            #endregion

                            if (low_section.TeacherName1 != new_section.TeacherName1)
                                sb.AppendLine(Course_Log.SetUpdataValue("授課教師1", low_section.TeacherName1, new_section.TeacherName1));

                            if (low_section.TeacherName2 != new_section.TeacherName2)
                                sb.AppendLine(Course_Log.SetUpdataValue("授課教師2", low_section.TeacherName2, new_section.TeacherName2));

                            if (low_section.TeacherName3 != new_section.TeacherName3)
                                sb.AppendLine(Course_Log.SetUpdataValue("授課教師3", low_section.TeacherName3, new_section.TeacherName3));

                            if (low_section.Comment != new_section.Comment)
                                sb.AppendLine(Course_Log.SetUpdataValue("註記", low_section.Comment, new_section.Comment));

                            sb.AppendLine();
                            #endregion
                        }
                    }
                    else //不包含於字典,就是新增資料
                    {
                        #region 新增
                        sb.AppendLine(string.Format("新增課程分段(星期「{0}」節次「{1}」)：", new_section.WeekDay.ToString(), new_section.Period.ToString()));
                        sb.AppendLine(Course_Log.SetNewValue("星期", new_section.WeekDay.ToString()));

                        sb.AppendLine(Course_Log.SetNewValue("節次", new_section.Period.ToString()));

                        sb.AppendLine(Course_Log.SetNewValue("節數", new_section.Length.ToString()));

                        sb.AppendLine(Course_Log.SetNewValue("跨中午", Course_Log.GetLongBreak(new_section.LongBreak)));

                        sb.AppendLine(Course_Log.SetNewValue("單雙週", Course_Log.GetWeekFlagName(new_section.WeekFlag)));

                        sb.AppendLine(Course_Log.SetNewValue("星期條件", new_section.WeekDayCond));

                        sb.AppendLine(Course_Log.SetNewValue("節次條件", new_section.PeriodCond));

                        string Classroomx2 = "";
                        if (new_section.ClassroomID.HasValue)
                        {
                            if (_dic.ContainsKey(new_section.ClassroomID.Value.ToString()))
                            {
                                Classroomx2 = _dic[new_section.ClassroomID.Value.ToString()];
                            }
                        }
                        sb.AppendLine(Course_Log.SetNewValue("上課場地", Classroomx2));

                        sb.AppendLine(Course_Log.SetNewValue("授課教師1", new_section.TeacherName1));

                        sb.AppendLine(Course_Log.SetNewValue("授課教師2", new_section.TeacherName2));

                        sb.AppendLine(Course_Log.SetNewValue("授課教師3", new_section.TeacherName3));

                        sb.AppendLine(Course_Log.SetNewValue("註記", new_section.Comment));

                        sb.AppendLine();
                        #endregion
                    }
                }
                else
                {
                    sb.AppendLine("刪除課程分段：");
                    sb.Append("課程分段系統編號「" + new_section.UID + "」");
                    sb.Append("星期「" + new_section.WeekDay + "」");
                    sb.Append("節次「" + new_section.Period + "」");
                    sb.Append("節數「" + new_section.Length + "」");
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private bool CheckChange(SchedulerCourseSection new_section, SchedulerCourseSection low_section)
        {
            bool check = false;
            if (low_section.WeekDay != new_section.WeekDay)
                check = true;

            if (low_section.Period != new_section.Period)
                check = true;

            if (low_section.Length != new_section.Length)
                check = true;

            if (low_section.LongBreak != new_section.LongBreak)
                check = true;
            if (low_section.WeekFlag != new_section.WeekFlag)
                check = true;

            if (low_section.WeekDayCond != new_section.WeekDayCond)
                check = true;

            if (low_section.WeekDayCond != new_section.WeekDayCond)
                check = true;

            #region Classroom
            string Classroomx1 = "";
            if (low_section.ClassroomID.HasValue)
            {
                if (_dic.ContainsKey(low_section.ClassroomID.Value.ToString()))
                {
                    Classroomx1 = _dic[low_section.ClassroomID.Value.ToString()];
                }
            }
            string Classroomx2 = "";
            if (new_section.ClassroomID.HasValue)
            {
                if (_dic.ContainsKey(new_section.ClassroomID.Value.ToString()))
                {
                    Classroomx2 = _dic[new_section.ClassroomID.Value.ToString()];
                }
            }

            if (Classroomx1 != Classroomx2)
                check = true;

            #endregion

            if (low_section.TeacherName1 != new_section.TeacherName1)
                check = true;

            if (low_section.TeacherName2 != new_section.TeacherName2)
                check = true;

            if (low_section.TeacherName3 != new_section.TeacherName3)
                check = true;

            if (low_section.Comment != new_section.Comment)
                check = true;

            return check;
        }


    }
}
