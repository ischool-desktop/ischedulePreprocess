using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus;

namespace SunsetCore
{
    /// <summary>
    /// 快取資料。
    /// </summary>
    public class CacheProvider
    {
        static CacheProvider()
        {
            Student = new DynamicCache();
            Class = new DynamicCache();
            Teacher = new DynamicCache();
            Course = new DynamicCache();
        }

        /// <summary>
        /// 學生資料快取。
        /// </summary>
        public static DynamicCache Student { get; private set; }

        /// <summary>
        /// 班級資料快取。
        /// </summary>
        public static DynamicCache Class { get; private set; }

        /// <summary>
        /// 教師資料快取。
        /// </summary>
        public static DynamicCache Teacher { get; private set; }

        /// <summary>
        /// 課程資料快取。
        /// </summary>
        public static DynamicCache Course { get; private set; }

        /// <summary>
        /// 清除所有快取資料。
        /// </summary>
        public static void ClearAll()
        {
            Student.Clear();
            Class.Clear();
            Teacher.Clear();
            Course.Clear();
        }
    }
}
