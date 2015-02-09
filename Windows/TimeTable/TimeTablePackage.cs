using System.Collections.Generic;

namespace Sunset
{
    /// <summary>
    /// 時間表及時間分段物件集合
    /// </summary>
    public class TimeTablePackage
    {
        /// <summary>
        /// 時間表
        /// </summary>
        public TimeTable TimeTable { get; set; }

        /// <summary>
        /// 時間表分段
        /// </summary>
        public List<TimeTableSec> TimeTableSecs { get; set; }
    }
}