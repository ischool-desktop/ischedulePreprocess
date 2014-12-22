using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 場地及場地不排課時段
    /// </summary>
    public class ClassroomPackage
    {
        /// <summary>
        /// 場地
        /// </summary>
        public Classroom Classroom { get; set; }

        /// <summary>
        /// 場地不排課時段
        /// </summary>
        public List<ClassroomBusy> ClassroomBusys { get; set; }
    }
}