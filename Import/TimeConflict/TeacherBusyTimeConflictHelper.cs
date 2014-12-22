using System;
using System.Collections.Generic;
using Campus.DocumentValidator;
using Campus.Validator;

namespace Sunset
{
    /// <summary>
    /// 匯入教師不排課時段檢查
    /// </summary>
    public class TeacherBusyTimeConflictHelper
    {
        private const string constTeacehrName = "教師姓名";
        private const string constTeacherNickName = "教師暱稱";
        private const string constWeekday = "星期";
        private const string constStartTime = "開始時間";
        private const string constEndTime = "結束時間";
        private const string constBusyDesc = "不排課描述";
        private const string constLocationName = "所在地點";
        private Dictionary<string,List<Period>> mTeacherPeriods;
        private RowMessages mMessages;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Messages"></param>
        public TeacherBusyTimeConflictHelper(List<IRowStream> Rows,RowMessages Messages)
        {
            this.mTeacherPeriods = new Dictionary<string, List<Period>>();
            this.mMessages = Messages;

            foreach(IRowStream Row in Rows)
            {
                string TeacherFullName = Row.GetValue(constTeacehrName) + Row.GetValue(constTeacherNickName);

                if (!mTeacherPeriods.ContainsKey(TeacherFullName))
                    mTeacherPeriods.Add(TeacherFullName, new List<Period>());

                int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekday));
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

                mTeacherPeriods[TeacherFullName].Add(Period);
            }
        }

        /// <summary>
        /// 檢查時間是否有重覆
        /// </summary>
        public void CheckTimeConflict()
        {
            //針對每位教師檢查時段是否重覆
            foreach (string TeacherName in mTeacherPeriods.Keys)
            {
                for(int i=0;i<mTeacherPeriods[TeacherName].Count-1;i++)
                {
                    Period Period = mTeacherPeriods[TeacherName][i];

                    for(int j=i+1;j<mTeacherPeriods[TeacherName].Count;j++)
                    {
                        Period TestPeriod = mTeacherPeriods[TeacherName][j];

                        if (Period.IsTimeIntersectsWith(TestPeriod))
                        {
                            mMessages[Period.Position].MessageItems.Add(new MessageItem(Campus.Validator.ErrorType.Error, Campus.Validator.ValidatorType.Row,"不排課時段不允許時間（星期、開始時間、結束時間）有重疊"));
                            mMessages[TestPeriod.Position].MessageItems.Add(new MessageItem(Campus.Validator.ErrorType.Error, Campus.Validator.ValidatorType.Row, "不排課時段不允許時間（星期、開始時間、結束時間）有重疊"));
                        }
                    }
                }
            }
        }
    }
}