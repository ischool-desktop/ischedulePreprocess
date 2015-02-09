using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FISCA.Data;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 常用工具
    /// </summary>
    public static class Utility
    {
        private static AccessHelper mAccessHelper = new AccessHelper();
        private static QueryHelper mQueryHelper = new QueryHelper();

        /// <summary>
        /// 取得內建的AcessHelper
        /// </summary>
        public static AccessHelper AccessHelper { get { return mAccessHelper; } }

        /// <summary>
        /// 取內建的QueryHelper
        /// </summary>
        public static QueryHelper QueryHelper { get { return mQueryHelper; } }

        /// <summary>
        /// 取得匯入不排課時段時間格式
        /// </summary>
        public static string ImportBusyTimeFormat
        {
            get { return "HH:mm"; } 
        }

        /// <summary>
        /// 取得課程名稱列表
        /// </summary>
        /// <param name="CourseIDs"></param>
        /// <returns></returns>
        public static string GetCourseNames(IEnumerable<string> CourseIDs)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
                return string.Empty;

            QueryHelper helper = new QueryHelper();

            string strSQL = "select ('「' || school_year || ' ' || semester || ' ' || course_name || '」') as coursename from course where id in (" + string.Join(",", CourseIDs.Select(x => "'" + x + "'").ToArray()) + ") order by school_year,semester,course_name";

            DataTable tblCourseName = helper.Select(strSQL);

            List<string> CourseNames = new List<string>();

            foreach (DataRow Row in tblCourseName.Rows)
            {
                string CourseName = Row.Field<string>("coursename");

                if (!CourseNames.Contains(CourseName))
                    CourseNames.Add(CourseName);
            }

            return string.Join("、", CourseNames);
        }


        /// <summary>
        /// 檢查兩個時間表時段是否有衝突
        /// </summary>
        /// <param name="BeginPeriod"></param>
        /// <param name="TestPeriod"></param>
        /// <returns></returns>
        public static bool IsTimeIntersectsWith(this Period BeginPeriod, Period TestPeriod)
        {
            //若星期不相同則不會相衝
            if (BeginPeriod.Weekday != TestPeriod.Weekday)
                return false;

            //將TestTime的年、月、日及秒設為與Appointment一致，以確保只是單純針對小時及分來做時間差的運算
            DateTime BeginTime = new DateTime(1900, 1, 1, BeginPeriod.Hour, BeginPeriod.Minute, 0);

            DateTime TestTime = new DateTime(1900, 1, 1, TestPeriod.Hour, TestPeriod.Minute, 0);

            //將Appointment的NewBeginTime減去NewTestTime
            int nTimeDif = (int)BeginTime.Subtract(TestTime).TotalMinutes;

            //狀況一：假設nTimeDif為正，並且大於NewTestTime，代表兩者沒有交集，傳回false。
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為9：00，其Duration為50。
            if (nTimeDif >= TestPeriod.Duration)
                return false;

            //狀況二：假設nTimeDiff為正，並且小於TestDuration，代表兩者有交集，傳回true。
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為9：00，其Duration為80。
            if (nTimeDif >= 0)
                return true;
            //狀況三：假設nTimeDiff為負值，將nTimeDiff成為正值，若是-nTimeDiff小於Appointment.Duration；
            //代表NewBeginTime在NewTestTime之前，並且NewBegin與NewTestTime的絕對差值小於Appointment.Duration的時間
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為10：30，其Duration為20。
            else if (-nTimeDif < BeginPeriod.Duration)
                return true;

            //其他狀況傳回沒有交集
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為10：50，其Duration為20。
            return false;
        }

        /// <summary>
        /// 檢查是否為合法的命名規則
        /// </summary>
        /// <param name="namingRule">班級命名規則</param>
        /// <returns></returns>
        private static bool ValidateNamingRule(string namingRule)
        {
            return namingRule.IndexOf('{') < namingRule.IndexOf('}');
        }

        /// <summary>
        /// 根據班級命名規則及年級解析出班級名稱
        /// </summary>
        /// <param name="namingRule">班級命名規則</param>
        /// <param name="gradeYear">年級</param>
        /// <returns></returns>
        public static string ParseClassName(this string namingRule, int gradeYear)
        {
            if (gradeYear >= 6)
                gradeYear -= 6;
            gradeYear--;
            if (!ValidateNamingRule(namingRule))
                return namingRule;
            string classlist_firstname = "", classlist_lastname = "";
            if (namingRule.Length == 0) return "{" + (gradeYear + 1) + "}";

            string tmp_convert = namingRule;

            // 找出"{"之前文字 並放入 classlist_firstname , 並除去"{"
            if (tmp_convert.IndexOf('{') > 0)
            {
                classlist_firstname = tmp_convert.Substring(0, tmp_convert.IndexOf('{'));
                tmp_convert = tmp_convert.Substring(tmp_convert.IndexOf('{') + 1, tmp_convert.Length - (tmp_convert.IndexOf('{') + 1));
            }
            else tmp_convert = tmp_convert.TrimStart('{');

            // 找出 } 之後文字 classlist_lastname , 並除去"}"
            if (tmp_convert.IndexOf('}') > 0 && tmp_convert.IndexOf('}') < tmp_convert.Length - 1)
            {
                classlist_lastname = tmp_convert.Substring(tmp_convert.IndexOf('}') + 1, tmp_convert.Length - (tmp_convert.IndexOf('}') + 1));
                tmp_convert = tmp_convert.Substring(0, tmp_convert.IndexOf('}'));
            }
            else tmp_convert = tmp_convert.TrimEnd('}');

            // , 存入 array
            string[] listArray = new string[tmp_convert.Split(',').Length];
            listArray = tmp_convert.Split(',');

            // 檢查是否在清單範圍
            if (gradeYear >= 0 && gradeYear < listArray.Length)
            {
                tmp_convert = classlist_firstname + listArray[gradeYear] + classlist_lastname;
            }
            else
            {
                tmp_convert = classlist_firstname + "{" + (gradeYear + 1) + "}" + classlist_lastname;
            }
            return tmp_convert;
        }


        /// <summary>
        /// 取得級別對應阿拉伯數字
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetNumberString(string p)
        {
            string levelNumber;
            switch (p.Trim())
            {
                #region 對應levelNumber
                case "1":
                    levelNumber = "Ⅰ";
                    break;
                case "2":
                    levelNumber = "Ⅱ";
                    break;
                case "3":
                    levelNumber = "Ⅲ";
                    break;
                case "4":
                    levelNumber = "Ⅳ";
                    break;
                case "5":
                    levelNumber = "Ⅴ";
                    break;
                case "6":
                    levelNumber = "Ⅵ";
                    break;
                case "7":
                    levelNumber = "Ⅶ";
                    break;
                case "8":
                    levelNumber = "Ⅷ";
                    break;
                case "9":
                    levelNumber = "Ⅸ";
                    break;
                case "10":
                    levelNumber = "Ⅹ";
                    break;
                default:
                    levelNumber = p;
                    break;
                #endregion
            }
            return levelNumber;
        }

        public static string SelectIDCondition(string TableName,string Condition)
        {
            QueryHelper helper = new QueryHelper();

            string strSQL = "select uid from " + TableName + " where " + Condition;

            DataTable Table = helper.Select(strSQL);

            List<string> IDs = new List<string>();

            foreach (DataRow Row in Table.Rows)
                IDs.Add(Row.Field<string>("uid"));

            string strUDTCondition = string.Join(",", IDs.Select(x => "'" + x + "'"));

            return string.IsNullOrWhiteSpace(strUDTCondition) ? string.Empty : "uid in (" + strUDTCondition + ")";
        }

        /// <summary>
        /// 判斷是否為合法日期格式
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static Tuple<bool, string> IsValidateTime(string Time)
        {
            string[] Times = Time.Split(new char[] { ':' });

            if (Times.Length != 2)
                return new Tuple<bool, string>(false, "時間以「:」分隔，例「8:10」。");

            int Hour;

            if (!int.TryParse(Times[0], out Hour))
                return new Tuple<bool, string>(false, "小時須為數字。");

            if (!(Hour >= 0 && Hour <= 23))
                    return new Tuple<bool, string>(false, "小時須介於0到23之間。");

            int Minute;

            if (!int.TryParse(Times[1], out Minute))
                return new Tuple<bool, string>(false, "分鐘須為數字。");

            if (!(Minute >= 0 && Minute <= 59))
                return new Tuple<bool, string>(false, "分鐘須介於0到59");

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// 取得顯示時間
        /// </summary>
        /// <param name="BeginTime">開始時間</param>
        /// <param name="Duration">持續分鐘</param>
        /// <returns>回傳「開始時間」及「結束時間」的字串</returns>
        public static Tuple<string, string> GetDisplayTime(DateTime BeginTime, int Duration)
        {
            //組合結束時間
            DateTime EndTime = BeginTime.AddMinutes(Duration);

            //回傳顯示時間
            return new Tuple<string, string>(BeginTime.Hour.ToString("00") + ":" + BeginTime.Minute.ToString("00"), EndTime.Hour.ToString("00") + ":" + EndTime.Minute.ToString("00"));
        }

        /// <summary>
        /// 取得實際儲存時間
        /// </summary>
        /// <param name="strStartTime"></param>
        /// <param name="strEndTime"></param>
        /// <returns></returns>
        public static Tuple<DateTime, int> GetStorageTime(string strStartTime, string strEndTime)
        {
            if (IsValidateTime(strStartTime).Item1 && IsValidateTime(strEndTime).Item1)
            {
                string[] strStartTimes = strStartTime.Split(new char[] { ':' });
                string[] strEndTimes = strEndTime.Split(new char[] { ':' });

                DateTime StartTime = new DateTime(1900, 1, 1, int.Parse(strStartTimes[0]), int.Parse(strStartTimes[1]), 0);
                DateTime EndTime = new DateTime(1900, 1, 1, int.Parse(strEndTimes[0]), int.Parse(strEndTimes[1]), 0);

                TimeSpan Span = EndTime.Subtract(StartTime);

                return new Tuple<DateTime, int>(StartTime, Convert.ToInt32(Span.TotalMinutes));
            }

            return new Tuple<DateTime, int>(new DateTime() , 0);
        }

        /// <summary>
        /// 根據單雙週數字取得文字
        /// </summary>
        /// <param name="WeekFlag"></param>
        /// <returns></returns>
        public static string GetWeekFlagStr(this int WeekFlag)
        {
            switch(WeekFlag)
            {
                case 1:return "單";
                case 2:return "雙";
                case 3:return "單雙";
                default:return "單雙";
            }            
        }

        /// <summary>
        /// 根據單雙週文字取得數字
        /// </summary>
        /// <param name="WeekFlag"></param>
        /// <returns></returns>
        public static int GetWeekFlagInt(this string WeekFlag)
        {
            switch (WeekFlag)
            {
                case "單": return 1;
                case "雙": return 2;
                case "單雙": return 3;
                default: return 3;
            } 
        }
    }
}