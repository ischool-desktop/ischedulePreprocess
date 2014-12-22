using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using FISCA.Data;

namespace Sunset
{
    /// <summary>
    /// 檢查班級名稱是否有在系統中
    /// </summary>
    public class ClassInCheck : IFieldValidator
    {
        private List<string> mClassNames;
        private Task mTask;

        /// <summary>
        /// 取得排課班級清單
        /// </summary>
        public ClassInCheck()
        {
            mClassNames = new List<string>();

            mTask = Task.Factory.StartNew(() =>
            {
                QueryHelper Helper = new QueryHelper();

                DataTable Table = Helper.Select("select class_name from $scheduler.class_ex");

                foreach (DataRow Row in Table.Rows)
                {
                    string ClassName = Row.Field<string>("class_name");

                    if (!mClassNames.Contains(ClassName))
                        mClassNames.Add(ClassName);
                }
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
            return !mClassNames.Contains(Value);
     
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