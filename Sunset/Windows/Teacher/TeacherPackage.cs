using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 教師及教師不排課時段
    /// </summary>
    public class TeacherPackage
    {
        /// <summary>
        /// 教師
        /// </summary>
        public TeacherEx Teacher { get; set; }

        /// <summary>
        /// 教師不排課時段
        /// </summary>
        public List<TeacherExBusy> TeacherBusys { get; set; }
    }
}