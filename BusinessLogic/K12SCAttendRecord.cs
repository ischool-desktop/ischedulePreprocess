using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// K12學生修課物件
    /// </summary>
    public class K12SCAttendRecord : K12.Data.SCAttendRecord
    {
        /// <summary>
        /// 覆蓋的校部訂資訊
        /// </summary>
        public new string OverrideRequiredBy
        {
            get { return base.OverrideRequiredBy ; }

            set { base.OverrideRequiredBy = value; }
        }

        /// <summary>
        /// 覆蓋的必選修資訊
        /// </summary>
        public new bool? OverrideRequired
        {
            get { return base.OverrideRequired; }

            set { base.OverrideRequired = value; }
        }
    }
}