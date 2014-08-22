using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using FISCA.Data;

namespace Sunset
{
    /// <summary>
    /// 檢查系統中是否有對應的教師姓名加暱稱
    /// </summary>
    public class TeacherNameCheck : IRowVaildator
    {
        private List<string> mTeacherNames;
        private Task mTask;

        /// <summary>
        /// 建構式，取得系統中的教師姓名加暱稱組合
        /// </summary>
        public TeacherNameCheck()
        {
            mTeacherNames = new List<string>();
            mTask = Task.Factory.StartNew
            (() =>
            {
                QueryHelper Helper = new QueryHelper();

                DataTable Table = Helper.Select("select teacher_name,nickname from teacher status=1");

                foreach (DataRow Row in Table.Rows)
                {
                    string TeacherName = Row.Field<string>("teacher_name");
                    string TeacherNickname = Row.Field<string>("nickname");
                    string TeacherKey = TeacherName + "," + TeacherNickname;

                    if (!mTeacherNames.Contains(TeacherKey))
                        mTeacherNames.Add(TeacherKey);
                }
            }
            );
        }

        #region IRowVaildator 成員

        /// <summary>
        /// 傳入要驗證的資料列，依教師姓名、暱稱檢查系統中是否有對應的教師
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool Validate(IRowStream Value)
        {
            if (Value.Contains("教師姓名") && Value.Contains("教師暱稱"))
            {
                string TeacherName = Value.GetValue("教師姓名");
                string TeacherNickName = Value.GetValue("教師暱稱");

                mTask.Wait();

                return mTeacherNames.Contains(TeacherName + "," + TeacherNickName);
            }

            return false;
        }

        /// <summary>
        /// 無提供自動修正
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Correct(IRowStream Value)
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