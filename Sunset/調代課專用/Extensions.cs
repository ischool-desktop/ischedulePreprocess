//using System;

//namespace Sunset
//{
//    public static class Extensions
//    {
//        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
//        {
//            int diff = dt.DayOfWeek - startOfWeek;
//            if (diff < 0)
//            {
//                diff += 7;
//            }

//            return dt.AddDays(-1 * diff).Date;
//        }

//        public static WeekCourseSection ToWeekCourseSection(this CourseSection CourseSection,int WeekDay,DateTime DateTime)
//        {
//            WeekCourseSection WeekCourseSection = new WeekCourseSection();

//            WeekCourseSection.Week = WeekDay;
//            WeekCourseSection.Date = DateTime;
//            WeekCourseSection.ClassroomID = CourseSection.ClassroomID;
//            WeekCourseSection.CourseID = CourseSection.CourseID;
//            WeekCourseSection.Length = CourseSection.Length;
//            WeekCourseSection.Lock = CourseSection.Lock;
//            WeekCourseSection.LongBreak = CourseSection.LongBreak;
//            WeekCourseSection.Period = CourseSection.Period;
//            WeekCourseSection.PeriodCond = CourseSection.PeriodCond;
//            WeekCourseSection.WeekDay = CourseSection.WeekDay;
//            WeekCourseSection.WeekDayCond = CourseSection.WeekDayCond;
//            WeekCourseSection.WeekFlag = CourseSection.WeekFlag;

//            return WeekCourseSection;
//        }
//    }
//}