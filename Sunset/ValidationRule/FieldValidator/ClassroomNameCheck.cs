using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 檢查場地名稱是否有存在系統中
    /// </summary>
    public class ClassroomNameCheck : IFieldValidator
    {
        private List<string> mClassroomNames;
        private Task mTask;

        /// <summary>
        /// 建構式，取得所有場地名稱
        /// </summary>
        public ClassroomNameCheck()
        {
            mTask = Task.Factory.StartNew(() =>
            {
                AccessHelper Helper = new AccessHelper();
                mClassroomNames = Helper
                    .Select<Classroom>()
                    .Select(x => x.ClassroomName)
                    .ToList();
            });
        }

        #region IFieldValidator 成員

        /// <summary>
        /// 傳入要驗證的欄位值，驗證值是否有在現有場地名稱中
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool Validate(string Value)
        {
            mTask.Wait();
            return mClassroomNames.Contains(Value);
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