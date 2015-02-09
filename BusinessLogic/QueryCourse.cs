using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sunset
{
    class QueryCourse
    {
        public string UID { get; private set; }

        public string CourseID { get; private set; }

        public string CourseName { get; private set; }

        public string SchoolYear { get; private set; }

        public string Semester { get; private set; }

        public string Subject { get; private set; }

        public string TimeTableName { get; private set; }

        public string TeacherName { get; private set; }

        public string CourseSectionCount { get; private set; }

        public string CourseSectionPeriod { get; private set;}

        public string Period { get; private set; }

        public string SplitSpec { get; private set; }

        public bool IsPeriodEqualSplitSpec        
        {
            get
            {
                int? vPeriod = K12.Data.Int.ParseAllowNull(Period);

                int? vSplitSpec = null;

                if (!string.IsNullOrEmpty(SplitSpec))
                {
                    vSplitSpec = 0;

                    string[] Splits = SplitSpec.Split(new char[] { ',' });

                    foreach (string Split in Splits)
                        vSplitSpec += K12.Data.Int.Parse(Split);
                }

                if (vPeriod.HasValue && vSplitSpec.HasValue)
                    return vPeriod.Value.Equals(vSplitSpec.Value);
                else if (vPeriod == null && vSplitSpec == null)
                    return true;
                else
                    return false;
            }
        }

        public QueryCourse(DataRow Row)
        {
            UID = Row.Table.Columns.Contains("uid") ? Row.Field<string>("uid") : string.Empty;
            CourseID = Row.Table.Columns.Contains("id") ? Row.Field<string>("id") : string.Empty;
            CourseName = Row.Table.Columns.Contains("course_name") ? Row.Field<string>("course_name") : string.Empty;
            Subject = Row.Table.Columns.Contains("subject") ? Row.Field<string>("subject") : string.Empty;
            SchoolYear = Row.Table.Columns.Contains("school_year") ? Row.Field<string>("school_year") : string.Empty;
            Semester = Row.Table.Columns.Contains("semester") ? Row.Field<string>("semester") : string.Empty;
            TimeTableName = Row.Table.Columns.Contains("timetable_name") ? Row.Field<string>("timetable_name") : string.Empty;
            TeacherName = Row.Table.Columns.Contains("teacher_name") ? Row.Field<string>("teacher_name") : string.Empty;
            CourseSectionCount = Row.Table.Columns.Contains("course_section_count") ? Row.Field<string>("course_section_count") : string.Empty;
            Period = Row.Table.Columns.Contains("period") ? Row.Field<string>("period") : string.Empty;
            SplitSpec = Row.Table.Columns.Contains("split_spec") ? Row.Field<string>("split_spec") : string.Empty;
            CourseSectionPeriod = Row.Table.Columns.Contains("course_section_period") ? Row.Field<string>("course_section_period") : string.Empty;

            if (string.IsNullOrWhiteSpace(CourseSectionCount))
                CourseSectionCount = "0";
        }
    }
}