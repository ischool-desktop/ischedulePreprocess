using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 檢查時間表名稱是否有在系統中
    /// </summary>
    public class TimeTableNameCheck : IFieldValidator
    {
        private List<string> mTimeTableNames;
        private Task mTask;

        /// <summary>
        /// 建構式，取得系統中所有時間表名稱
        /// </summary>
        public TimeTableNameCheck()
        {
            mTask = Task.Factory.StartNew(() =>
            {
                AccessHelper Helper = new AccessHelper();
                mTimeTableNames = Helper.Select<TimeTable>().Select(x => x.TimeTableName).ToList();
            }); 
        }

        #region IFieldValidator 成員

        /// <summary>
        /// 傳入要驗證的欄位值，驗證值是否有在現有時間表名稱中
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool Validate(string Value)
        {
            mTask.Wait();
            return mTimeTableNames.Contains(Value);
        }

        /// <summary>
        /// 無提供自動修正
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Correct(string Value)
        {
            return string.Empty;
        }

        /// <summary>
        /// 傳回預設樣版
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public string ToString(string template)
        {
            return template;
        }

        #endregion
    }
}