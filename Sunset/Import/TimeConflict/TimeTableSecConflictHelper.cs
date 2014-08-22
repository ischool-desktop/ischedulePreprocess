using System;
using System.Collections.Generic;
using Campus.DocumentValidator;
using Campus.Validator;

namespace Sunset
{
    /// <summary>
    /// 匯入教師不排課時段檢查
    /// </summary>
    public class TimeTableSecConflictHelper
    {
        private const string constTimeTableName = "時間表名稱";
        private const string constWeekDay = "星期";
        private const string constPeriod = "節次";
        private const string constStartTime = "開始時間";
        private const string constEndTime = "結束時間";
        private const string constDispPeriod = "顯示節次";
        private const string constLocationName = "地點名稱";

        private Dictionary<string, List<Period>> mPeriods;
        private RowMessages mMessages;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Messages"></param>
        public TimeTableSecConflictHelper(List<IRowStream> Rows, RowMessages Messages)
        {
            this.mPeriods = new Dictionary<string, List<Period>>();
            this.mMessages = Messages;

            foreach (IRowStream Row in Rows)
            {
                string Key = Row.GetValue(constTimeTableName);

                if (!mPeriods.ContainsKey(Key))
                    mPeriods.Add(Key, new List<Period>());

                int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekDay));
                string StartTime = Row.GetValue(constStartTime);
                string EndTime = Row.GetValue(constEndTime);

                Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(StartTime, EndTime);

                DateTime BeginDatetime = StorageTime.Item1;
                int Duration = StorageTime.Item2;

                Period Period = new Period();
                Period.Weekday = Weekday;
                Period.Hour = BeginDatetime.Hour;
                Period.Minute = BeginDatetime.Minute;
                Period.Duration = Duration;
                Period.Position = Row.Position;

                mPeriods[Key].Add(Period);
            }
        }

        /// <summary>
        /// 檢查時間是否有重覆
        /// </summary>
        public void CheckTimeConflict()
        {
            foreach (string Key in mPeriods.Keys)
            {
                for (int i = 0; i < mPeriods[Key].Count - 1; i++)
                {
                    Period Period = mPeriods[Key][i];

                    for (int j = i + 1; j < mPeriods[Key].Count; j++)
                    {
                        Period TestPeriod = mPeriods[Key][j];

                        if (Period.IsTimeIntersectsWith(TestPeriod))
                        {
                            mMessages[Period.Position].MessageItems.Add(new MessageItem(Campus.Validator.ErrorType.Error, Campus.Validator.ValidatorType.Row, "時間表分段不允許時間（星期、開始時間、結束時間）有重疊"));
                            mMessages[TestPeriod.Position].MessageItems.Add(new MessageItem(Campus.Validator.ErrorType.Error, Campus.Validator.ValidatorType.Row, "時間表分段不允許時間（星期、開始時間、結束時間）有重疊"));
                        }
                    }
                }
            }
        }
    }
}
