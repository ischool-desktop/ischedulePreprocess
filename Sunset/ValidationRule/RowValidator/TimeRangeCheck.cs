using Campus.DocumentValidator;

namespace Sunset
{
    /// <summary>
    /// 檢查結束時間是否大於開始時間
    /// </summary>
    public class TimeRangeCheck : IRowVaildator
    {
        #region IRowVaildator 成員

        public string Correct(IRowStream Value)
        {
            return string.Empty;
        }

        public string ToString(string template)
        {
            return template;
        }

        public bool Validate(IRowStream Value)
        {
            if (Value.Contains("開始時間") && Value.Contains("結束時間"))
            {
                string StartTime = Value.GetValue("開始時間");
                string EndTime = Value.GetValue("結束時間");

                if (Utility.IsValidateTime(StartTime).Item1 && Utility.IsValidateTime(EndTime).Item1)
                    return Utility.GetStorageTime(StartTime, EndTime).Item2 > 0;
            }

            return false;
        }

        #endregion
    }
}