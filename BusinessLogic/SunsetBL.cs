using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FISCA.Data;
using FISCA.UDT;
using K12.Data;
using Sunset.NewCourse;

namespace Sunset
{
    /// <summary>
    /// 排課前處理商業邏輯
    /// </summary>
    public static class SunsetBL
    {
        /// <summary>
        /// 取得課程長度，先取得節數，若無取得學分數
        /// </summary>
        /// <param name="CourseRecord"></param>
        /// <returns></returns>
        private static int GetSchedulerCourseLen(SchedulerCourseExtension CourseRecord)
        {
            if (CourseRecord.Period.HasValue)
                return (int)CourseRecord.Period.Value;
            else if (CourseRecord.Credit.HasValue)
                return (int)CourseRecord.Credit.Value;
            else
                return 0;
        }

        /// <summary>
        /// 取得課程長度，先取得節數，若無取得學分數
        /// </summary>
        /// <param name="CourseRecord"></param>
        /// <returns></returns>
        private static int GetCourseLen(CourseRecord CourseRecord) 
        {
            if (CourseRecord.Period.HasValue)
                return (int)CourseRecord.Period.Value;
            else if (CourseRecord.Credit.HasValue)
                return (int)CourseRecord.Credit.Value;
            else
                return 0;
        }

        /// <summary>
        /// 根據課程系統編號列表產生課程分段
        /// </summary>
        /// <param name="CourseIDs"></param>
        /// <returns></returns>
        public static List<string> CreateCourseSectionByCourseIDs(IEnumerable<string> CourseIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
            {
                #region 將課程系統編號的課程分段選出並且刪除
                AccessHelper AccessHelper = new AccessHelper();

                string strCondition = "ref_course_id in (" + string.Join(",", CourseIDs.ToArray()) + ")";

                List<CourseSection> DeleteCourseSections = AccessHelper
                    .Select<CourseSection>(strCondition);

                if (DeleteCourseSections.Count>0)
                    AccessHelper.DeletedValues(DeleteCourseSections);
                #endregion

                #region 根據課程排課資料新增課程分段
                List<CourseSection> InsertCourseSections = new List<CourseSection>();

                List<CourseExtension> CourseExtensions = AccessHelper
                    .Select<CourseExtension>(strCondition);

                K12.Data.Course.RemoveByIDs(CourseIDs);

                Dictionary<string, CourseRecord> Courses = K12.Data.Course
                                                .SelectByIDs(CourseIDs)
                                                .ToDictionary(x => x.ID);

                foreach (CourseExtension CourseExtension in CourseExtensions)
                {
                    if (!string.IsNullOrEmpty(CourseExtension.SplitSpec))
                    {
                        string[] Lens = CourseExtension.SplitSpec.Split(new char[] { ',' });

                        if (Lens.Length > 0)
                        {
                            for (int i = 0; i < Lens.Length; i++)
                            {
                                int Len;

                                if (int.TryParse(Lens[i], out Len))
                                {
                                    CourseSection NewCourseSection = new CourseSection();
                                    NewCourseSection.CourseID = CourseExtension.CourseID;
                                    NewCourseSection.ClassroomID = CourseExtension.ClassroomID;
                                    NewCourseSection.Length = Len;
                                    NewCourseSection.Lock = false;
                                    NewCourseSection.LongBreak = CourseExtension.LongBreak;
                                    NewCourseSection.PeriodCond = CourseExtension.PeriodCond;
                                    NewCourseSection.WeekDayCond = CourseExtension.WeekDayCond;
                                    NewCourseSection.WeekFlag = CourseExtension.WeekFlag;
                                    InsertCourseSections.Add(NewCourseSection);
                                }
                            }
                        }
                    }
                    else if (Courses.ContainsKey(""+CourseExtension.CourseID))
                    {
                        CourseRecord Course = Courses["" + CourseExtension.CourseID];

                        CourseSection NewCourseSection = new CourseSection();
                        NewCourseSection.CourseID = CourseExtension.CourseID;
                        NewCourseSection.ClassroomID = CourseExtension.ClassroomID;
                        NewCourseSection.Length = GetCourseLen(Course);
                        NewCourseSection.Lock = false;
                        NewCourseSection.LongBreak = CourseExtension.LongBreak;
                        NewCourseSection.PeriodCond = CourseExtension.PeriodCond;
                        NewCourseSection.WeekDayCond = CourseExtension.WeekDayCond;
                        NewCourseSection.WeekFlag = CourseExtension.WeekFlag;
                        InsertCourseSections.Add(NewCourseSection); 
                    }
                }

                if (InsertCourseSections.Count > 0)
                {
                    List<string> NewIDs = AccessHelper.InsertValues(InsertCourseSections);

                    return NewIDs;
                }
                #endregion
            }

            return new List<string>();
        }

        /// <summary>
        /// 根據課程系統編號列表產生課程分段
        /// </summary>
        /// <param name="CourseIDs"></param>
        /// <returns></returns>
        public static List<string> CreateSchedulerCourseSectionByCourseIDs(IEnumerable<string> CourseIDs)
        {
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
            {
                #region 取得所有教師並依教師名稱排序
                QueryHelper qhelper = new QueryHelper();

                DataTable mtblTeacher = qhelper.Select("select id,(CASE WHEN teacher.nickname='' THEN teacher.teacher_name ELSE teacher.teacher_name || '(' || teacher.nickname || ')'  END) as teachername from teacher where status=1 order by teachername");

                Dictionary<string, string> mTeacherNameIDs = new Dictionary<string, string>();

                mTeacherNameIDs.Clear();

                foreach (DataRow row in mtblTeacher.Rows)
                {
                    string TeacherName = row.Field<string>("teachername");
                    string TeacherID = row.Field<string>("id");

                    if (!mTeacherNameIDs.ContainsKey(TeacherName))
                        mTeacherNameIDs.Add(TeacherName, TeacherID);
                }
                #endregion 

                #region 將課程系統編號的課程分段選出並且刪除
                AccessHelper AccessHelper = new AccessHelper();

                List<SchedulerCourseSection> DeleteCourseSections = AccessHelper
                    .Select<SchedulerCourseSection>("ref_course_id in (" + string.Join(",", CourseIDs.ToArray()) + ")");

                if (DeleteCourseSections.Count > 0)
                    AccessHelper.DeletedValues(DeleteCourseSections);
                #endregion

                #region 根據課程排課資料新增課程分段
                List<SchedulerCourseSection> InsertCourseSections = new List<SchedulerCourseSection>();

                List<SchedulerCourseExtension> CourseExtensions = AccessHelper
                    .Select<SchedulerCourseExtension>("uid in (" + string.Join(",", CourseIDs.ToArray()) + ")");

                foreach (SchedulerCourseExtension CourseExtension in CourseExtensions)
                {
                    if (!string.IsNullOrEmpty(CourseExtension.SplitSpec))
                    {
                        string[] Lens = CourseExtension.SplitSpec.Split(new char[] { ',' });

                        if (Lens.Length > 0)
                        {
                            for (int i = 0; i < Lens.Length; i++)
                            {
                                int Len;

                                if (int.TryParse(Lens[i], out Len))
                                {
                                    SchedulerCourseSection NewCourseSection = new SchedulerCourseSection();
                                    NewCourseSection.TeacherName1 = CourseExtension.TeacherName1;
                                    NewCourseSection.TeacherName2 = CourseExtension.TeacherName2;
                                    NewCourseSection.TeacherName3 = CourseExtension.TeacherName3;
                                    NewCourseSection.TeacherID1 = mTeacherNameIDs.ContainsKey(NewCourseSection.TeacherName1) ? K12.Data.Int.ParseAllowNull(mTeacherNameIDs[NewCourseSection.TeacherName1]) : null;
                                    NewCourseSection.TeacherID2 = mTeacherNameIDs.ContainsKey(NewCourseSection.TeacherName2) ? K12.Data.Int.ParseAllowNull(mTeacherNameIDs[NewCourseSection.TeacherName2]) : null;
                                    NewCourseSection.TeacherID3 = mTeacherNameIDs.ContainsKey(NewCourseSection.TeacherName3) ? K12.Data.Int.ParseAllowNull(mTeacherNameIDs[NewCourseSection.TeacherName3]) : null;
                                    NewCourseSection.CourseID = K12.Data.Int.Parse(CourseExtension.UID);
                                    NewCourseSection.ClassroomID = CourseExtension.ClassroomID;
                                    NewCourseSection.Length = Len;
                                    NewCourseSection.Lock = false;
                                    NewCourseSection.LongBreak = CourseExtension.LongBreak;
                                    NewCourseSection.PeriodCond = CourseExtension.PeriodCond;
                                    NewCourseSection.WeekDayCond = CourseExtension.WeekDayCond;
                                    NewCourseSection.WeekFlag = CourseExtension.WeekFlag;
                                    InsertCourseSections.Add(NewCourseSection);
                                }
                            }
                        }
                    }
                    else
                    {
                        SchedulerCourseSection NewCourseSection = new SchedulerCourseSection();
                        NewCourseSection.TeacherName1 = CourseExtension.TeacherName1;
                        NewCourseSection.TeacherName2 = CourseExtension.TeacherName2;
                        NewCourseSection.TeacherName3 = CourseExtension.TeacherName3;
                        NewCourseSection.TeacherID1 = mTeacherNameIDs.ContainsKey(NewCourseSection.TeacherName1) ? K12.Data.Int.ParseAllowNull(mTeacherNameIDs[NewCourseSection.TeacherName1]) : null;
                        NewCourseSection.TeacherID2 = mTeacherNameIDs.ContainsKey(NewCourseSection.TeacherName2) ? K12.Data.Int.ParseAllowNull(mTeacherNameIDs[NewCourseSection.TeacherName2]) : null;
                        NewCourseSection.TeacherID3 = mTeacherNameIDs.ContainsKey(NewCourseSection.TeacherName3) ? K12.Data.Int.ParseAllowNull(mTeacherNameIDs[NewCourseSection.TeacherName3]) : null;
                        NewCourseSection.CourseID = K12.Data.Int.Parse(CourseExtension.UID);
                        NewCourseSection.ClassroomID = CourseExtension.ClassroomID;
                        NewCourseSection.Length = GetSchedulerCourseLen(CourseExtension);
                        NewCourseSection.Lock = false;
                        NewCourseSection.LongBreak = CourseExtension.LongBreak;
                        NewCourseSection.PeriodCond = CourseExtension.PeriodCond;
                        NewCourseSection.WeekDayCond = CourseExtension.WeekDayCond;
                        NewCourseSection.WeekFlag = CourseExtension.WeekFlag;
                        InsertCourseSections.Add(NewCourseSection);
                    }
                }

                if (InsertCourseSections.Count > 0)
                {
                    List<string> NewIDs = AccessHelper.InsertValues(InsertCourseSections);

                    return NewIDs;
                }
                #endregion
            }

          

            return new List<string>();
        }


        /// <summary>
        /// 是否為相同的課程群組
        /// </summary>
        /// <param name="Sections"></param>
        /// <returns></returns>
        public static Tuple<bool,string> IsSameCourseGroup(this List<CourseSection> Sections)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(Sections))
                return new Tuple<bool, string>(false, "無課程分段");

            List<string> CourseIDs = Sections.Select(x => ""+x.CourseID).ToList();

            Dictionary<int, CourseSections> CourseSectionGroup = new Dictionary<int, CourseSections>();

            foreach(CourseSection Section in Sections)
            {
                if (!CourseSectionGroup.ContainsKey(Section.CourseID))
                    CourseSectionGroup.Add(Section.CourseID, new CourseSections());

                CourseSectionGroup[Section.CourseID].Add(Section);
            }

            List<CourseSections> CourseSectionsList = CourseSectionGroup.Values.ToList();

            for (int i = 1; i < CourseSectionsList.Count; i++)
                if (!CourseSectionsList[i].Equals(CourseSectionsList[0]))
                {
                    K12.Presentation.NLDPanels.Course.AddToTemp(CourseIDs);

                    return new Tuple<bool, string>(false, "課程" + Utility.GetCourseNames(CourseIDs) + "分段細節不相同！已加入至待處理！");
                }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// 將課程依群組名稱進行群組
        /// </summary>
        /// <param name="vCourseIDs">課程系統編號列表</param>
        /// <param name="GroupName">課程群組名稱</param>
        public static void GroupCourse(List<string> vCourseIDs, string GroupName)
        {
            string strCondition = "ref_course_id in (" + string.Join(",", vCourseIDs.ToArray()) + ")";

            AccessHelper mHelper = new AccessHelper();

            List<CourseExtension> CourseExtensions = mHelper.Select<CourseExtension>(strCondition);

            List<int> CourseIDs = CourseExtensions.Select(x => x.CourseID).ToList();

            //針對選取的每筆班級
            foreach (string x in vCourseIDs)
            {
                int CourseID = K12.Data.Int.Parse(x);
                //若班級排課課程資料有存在
                if (CourseIDs.Contains(CourseID))
                {
                    //指定該課程的時間表
                    CourseExtension UpdateCourseExtension = CourseExtensions
                        .Find(y => y.CourseID.Equals(K12.Data.Int.Parse(x)));
                    UpdateCourseExtension.CourseGroup = GroupName;
                    CourseExtensions.Add(UpdateCourseExtension);
                }
                else
                {
                    //若課程排課課程資料不存在則新增，並指定時間表
                    CourseExtension InsertCourseExtension = new CourseExtension();
                    InsertCourseExtension.CourseID = CourseID;
                    InsertCourseExtension.TimeTableID = null;
                    InsertCourseExtension.AllowDup = false;
                    InsertCourseExtension.ClassroomID = null;
                    InsertCourseExtension.LongBreak = false;
                    InsertCourseExtension.WeekDayCond = string.Empty;
                    InsertCourseExtension.PeriodCond = string.Empty;
                    InsertCourseExtension.SplitSpec = string.Empty;
                    InsertCourseExtension.WeekFlag = 3;
                    InsertCourseExtension.CourseGroup = GroupName;
                    CourseExtensions.Add(InsertCourseExtension);
                }
            }

            mHelper.SaveAll(CourseExtensions);
        }

        /// <summary>
        /// 是否為相同的課程群組
        /// </summary>
        /// <param name="Sections"></param>
        /// <returns></returns>
        public static Tuple<bool, string> IsSchedulerSameCourseGroup(this List<SchedulerCourseSection> Sections)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(Sections))
                return new Tuple<bool, string>(false, "無課程分段");

            List<string> CourseIDs = Sections.Select(x => "" + x.CourseID).ToList();

            Dictionary<int, SchedulerCourseSections> CourseSectionGroup = new Dictionary<int, SchedulerCourseSections>();

            foreach (SchedulerCourseSection Section in Sections)
            {
                if (!CourseSectionGroup.ContainsKey(Section.CourseID))
                    CourseSectionGroup.Add(Section.CourseID, new SchedulerCourseSections());

                CourseSectionGroup[Section.CourseID].Add(Section);
            }

            List<SchedulerCourseSections> CourseSectionsList = CourseSectionGroup.Values.ToList();

            for (int i = 1; i < CourseSectionsList.Count; i++)
                if (!CourseSectionsList[i].Equals(CourseSectionsList[0]))
                {
                    //CourseAdmin.Instance.AddToTemp(CourseIDs);

                    return new Tuple<bool, string>(false, "課程" + Utility.GetCourseNames(CourseIDs) + "分段細節不相同！");
                }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// 將課程依群組名稱進行群組
        /// </summary>
        /// <param name="vCourseIDs">課程系統編號列表</param>
        /// <param name="GroupName">課程群組名稱</param>
        public static void GroupSchedulerCourse(List<string> vCourseIDs, string GroupName)
        {
            string strCondition = "uid in (" + string.Join(",", vCourseIDs.ToArray()) + ")";

            AccessHelper mHelper = new AccessHelper();

            List<SchedulerCourseExtension> CourseExtensions = mHelper
                .Select<SchedulerCourseExtension>(strCondition);

            List<string> CourseIDs = CourseExtensions.Select(x => x.UID).ToList();

            //針對選取的每筆班級
            foreach (string x in vCourseIDs)
            {
                //若班級排課課程資料有存在
                if (CourseIDs.Contains(x))
                {
                    //指定該課程的時間表
                    SchedulerCourseExtension UpdateCourseExtension = CourseExtensions
                        .Find(y => y.UID.Equals(x));
                    UpdateCourseExtension.CourseGroup = GroupName;
                    CourseExtensions.Add(UpdateCourseExtension);
                }
                else
                {
                    //若課程排課課程資料不存在則新增，並指定時間表
                    SchedulerCourseExtension InsertCourseExtension = new SchedulerCourseExtension();
                    //InsertCourseExtension.CourseID = CourseID;
                    InsertCourseExtension.TimeTableID = null;
                    InsertCourseExtension.AllowDup = false;
                    InsertCourseExtension.ClassroomID = null;
                    InsertCourseExtension.LongBreak = false;
                    InsertCourseExtension.WeekDayCond = string.Empty;
                    InsertCourseExtension.PeriodCond = string.Empty;
                    InsertCourseExtension.SplitSpec = string.Empty;
                    InsertCourseExtension.WeekFlag = 3;
                    InsertCourseExtension.CourseGroup = GroupName;
                    CourseExtensions.Add(InsertCourseExtension);
                }
            }

            mHelper.SaveAll(CourseExtensions);
        }
    }
}