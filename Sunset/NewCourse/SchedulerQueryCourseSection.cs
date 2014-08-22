using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sunset.NewCourse
{
    public class SchedulerQueryCourseSection
    {
        public string UID { get; private set; }

        public string CourseGroup { get; private set; }

        public string CourseID { get; private set; }

        public string CourseName { get; private set; }

        public string SchoolYear { get; private set; }

        public string Semester { get; private set; }

        public SchedulerCourseSection Source { get; set; }

        public SchedulerQueryCourseSection(DataRow Row)
        {
            UID = Row.Field<string>("uid");
            CourseGroup = Row.Field<string>("course_group");
            CourseID = Row.Field<string>("id");
            CourseName = Row.Field<string>("course_name");
            SchoolYear = Row.Field<string>("school_year");
            Semester = Row.Field<string>("semester");
        }
    }
}