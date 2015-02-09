using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 排課課程更新事件
    /// </summary>
    public static class CourseEvents
    {
        /// <summary>
        /// 引發排課課程更新事件
        /// </summary>
        public static void RaiseChanged()
        {
            if (CourseChanged != null)
                CourseChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// 註冊排課課程更新事件
        /// </summary>
        public static event EventHandler CourseChanged;
    }

    /// <summary>
    /// 課程分段更新事件
    /// </summary>
    public static class CourseSectionEvents
    {
        /// <summary>
        /// 引發課程分段更新事件
        /// </summary>
        public static void RaiseChanged()
        {
            if (CourseSectionChanged != null)
                CourseSectionChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// 註冊課程分段更新事件
        /// </summary>
        public static event EventHandler CourseSectionChanged;
    }
}