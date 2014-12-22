using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// K12課程記錄物件
    /// </summary>
    public class K12CourseRecord : K12.Data.CourseRecord
    {
        /// <summary>
        /// 分項
        /// </summary>
        public new string Entry
        {
            get { return base.Entry; }

            set { base.Entry = value; }
        }

        /// <summary>
        /// 領域
        /// </summary>
        public new string Domain
        {
            get { return base.Domain; }

            set { base.Domain = value; }
        }

        /// <summary>
        /// 是否列入學期成績計算
        /// </summary>
        public new string CalculationFlag
        {
            get { return base.CalculationFlag; }

            set { base.CalculationFlag = value; }
        }

        /// <summary>
        /// 級別
        /// </summary>
        public new decimal? Level
        {
            get { return base.Level; }

            set { base.Level = value; }
        }

        /// <summary>
        /// 不評分
        /// </summary>
        public new bool NotIncludedInCalc
        {
            get { return base.NotIncludedInCalc; }

            set { base.NotIncludedInCalc = value; }
        }

        /// <summary>
        /// 不計學分
        /// </summary>
        public new bool NotIncludedInCredit
        {
            get { return base.NotIncludedInCredit; }

            set { base.NotIncludedInCredit = value; }
        }

        /// <summary>
        /// 必修
        /// </summary>
        public new bool Required
        {
            get { return base.Required; }

            set { base.Required = value; } 
        }

        /// <summary>
        /// 校/部訂
        /// </summary>
        public new string RequiredBy
        {
            get { return base.RequiredBy; }

            set { base.RequiredBy = value; }
        }
    }
}