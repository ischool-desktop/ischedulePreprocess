using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.OpenCourse
{
    public class SCAttendCourse : K12.Data.Course
    {
        public static List<SCAttendCourseRecord> SelectByIDs(List<string> CourseIDs)
        {
            return K12.Data.Course.SelectByIDs<SCAttendCourseRecord>(CourseIDs);
        }
    }
}