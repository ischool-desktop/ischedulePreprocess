using System.Collections.Generic;
using Campus.DocumentValidator;

namespace Sunset
{
    /// <summary>
    /// 授課教師名稱重覆
    /// </summary>
    public class TeacherNameRepeatCheck : IRowVaildator
    {
        #region IRowVaildator 成員

        /// <summary>
        /// 傳入要驗證的資料列，依教師姓名、暱稱檢查系統中是否有對應的教師
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool Validate(IRowStream Value)
        {
             List<string> Names = new List<string>();

             if (Value.Contains("授課教師一"))
             {
                 string TeacherName1 = Value.GetValue("授課教師一");

                 if (!string.IsNullOrEmpty(TeacherName1))
                 {
                     if (Names.Contains(TeacherName1))
                         return false;
                     else
                         Names.Add(TeacherName1);
                 }
             }

             if (Value.Contains("授課教師二"))
             {
                 string TeacherName2 = Value.GetValue("授課教師二");

                 if (!string.IsNullOrEmpty(TeacherName2))
                 {
                     if (Names.Contains(TeacherName2))
                         return false;
                     else
                         Names.Add(TeacherName2);
                 }
             }

             if (Value.Contains("授課教師三"))
             {
                 string TeacherName3 = Value.GetValue("授課教師三");

                 if (!string.IsNullOrEmpty(TeacherName3))
                 {
                     if (Names.Contains(TeacherName3))
                         return false;
                     else
                         Names.Add(TeacherName3);
                 }
             }

             return true;
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