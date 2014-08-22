using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 排課課程規劃對應班級
    /// </summary>
    public class SchedulerProgramPlanClassRecord
    {
        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        public int GradeYear { get; set; }

        /// <summary>
        /// 課程規劃系統編號
        /// </summary>
        public SchedulerProgramPlan ProgramPlan { get; set; }
    }
}