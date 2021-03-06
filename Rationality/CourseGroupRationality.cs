﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataRationality;
using FISCA.Data;
using FISCA.UDT;

namespace Sunset
{
    class CourseGroupRationality : DataRationality.IDataRationality
    {
        #region IDataRationality 成員

        private List<string> CourseIDs = new List<string>();

        public void AddToTemp(IEnumerable<string> EntityIDs)
        {
            K12.Presentation.NLDPanels.Course.AddToTemp(EntityIDs.ToList());
        }

        public void AddToTemp()
        {
            AddToTemp(CourseIDs);
        }

        public string Category
        {
            get { return "課程"; }
        }

        public string Description
        {
            get 
            {
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.AppendLine("檢查課程群組的課程分段是否相同");
                strBuilder.AppendLine("1.檢查群組每一課程的課程分段數目是否相同。");
                strBuilder.AppendLine("2.檢查群組每一課程的課程分段節數是否相同。");
                strBuilder.AppendLine("3.檢查群組每一課程的課程分段屬性是否相同，包括單雙週、星期條件、節次條件、同天排課。");

                return strBuilder.ToString();
            }
        }

        private string GetWeekFlagStr(int WeekFlag)
        {
            if (WeekFlag == 1)
                return "單";
            else if (WeekFlag == 2)
                return "雙";
            else
                return "單雙"; 
        }

        public DataRationality.DataRationalityMessage Execute()
        {
            CourseIDs.Clear();

            #region 取得群組名稱不為空白的課程分段
            QueryHelper helper = new QueryHelper();

            DataTable table = helper.Select("select course.id,course.course_name,course.school_year,course.semester,$scheduler.course_section.uid,course_group from $scheduler.course_section inner join $scheduler.course_extension on $scheduler.course_extension.ref_course_id=$scheduler.course_section.ref_course_id inner join course on course.id=$scheduler.course_section.ref_course_id where course_group is not null");

            List<QueryCourseSection> QuerySections = new List<QueryCourseSection>();

            foreach (DataRow row in table.Rows)
            {
                QueryCourseSection QuerySection = new QueryCourseSection(row);
                QuerySections.Add(QuerySection);
            }

            AccessHelper udtHelper = new AccessHelper();

            List<CourseSection> CourseSections = udtHelper.Select<CourseSection>("uid in (" + string.Join(",", QuerySections.Select(x=>x.UID).ToArray()) + ")");

            QuerySections.ForEach(x => x.Source = CourseSections.Find(y => y.UID.Equals(x.UID)));
            #endregion

            #region 針對每個課程群組下的課程分段做檢查
            DataRationalityMessage Message = new DataRationalityMessage();

            List<object> Data = new List<object>();

            foreach (string CourseGroup in QuerySections.Select(x=>x.CourseGroup).Distinct())
            {
                List<QueryCourseSection> GroupQuerySections = QuerySections.FindAll(x => x.CourseGroup.Equals(CourseGroup));
                
                Tuple<bool, string> Result = GroupQuerySections
                    .FindAll(x=>x!=null)
                    .Select(x=>x.Source)
                    .ToList()
                    .IsSameCourseGroup();

                if (!Result.Item1)
                {
                    foreach(QueryCourseSection Section in GroupQuerySections)
                        Data.Add(new { 編號 = Section.CourseID, 課程名稱 = Section.CourseName, 學年度 = Section.SchoolYear, 學期 = Section.Semester, 課程群組 = CourseGroup, 星期 = Section.Source.WeekDay, 節次 = Section.Source.Period, 星期條件 = Section.Source.WeekDayCond, 節次條件 = Section.Source.PeriodCond, 節數 = Section.Source.Length, 單雙週 = GetWeekFlagStr(Section.Source.WeekFlag), 跨中午 = Section.Source.LongBreak?"是":string.Empty});

                    CourseIDs.AddRange(GroupQuerySections.Select(x => x.CourseID));
                }
            }
            #endregion

            Message.Data = Data;
            Message.Message = Data.Count>0?"有課程群組下的課程分段不相同，請檢查！":"恭禧！所有課程群組下的課程分段皆相同！";

            return Message;
        }

        public string Name
        {
            get { return "課程分段群組檢查（排課）"; }
        }

        #endregion
    }
}