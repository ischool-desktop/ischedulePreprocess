using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 學生功課表
    /// </summary>
    public class StudentLPView
    {
        /// <summary>
        /// 學生系統編號
        /// </summary>
        public string StudentID { get; set; }

        /// <summary>
        /// 學號
        /// </summary>
        public string StudentNumber { get; set; }

        /// <summary>
        /// 學生姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 功課表細項
        /// </summary>
        public SortedDictionary<int, LPViewItem> Items { get; set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public StudentLPView(int MaxPeriod)
        {
            Items = new SortedDictionary<int, LPViewItem>();

            for (int i = 1; i <= MaxPeriod; i++)
            {
                if (!Items.ContainsKey(i))
                {
                    Items.Add(i, new LPViewItem());
                    Items[i].PeriodNo = i;
                }
            }
        }
    }
}