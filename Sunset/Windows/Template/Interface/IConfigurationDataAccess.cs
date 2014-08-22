using System.Collections.Generic;

namespace Sunset.Windows
{
    /// <summary>
    /// 提供與ConfigurationForm整合的資料存介面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConfigurationDataAccess<T>
    {
        /// <summary>
        /// 顯示名稱，例如TimeTable會顯示時間表
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 取得鍵值名稱，用來做畫面左方顯示
        /// </summary>
        /// <returns></returns>
        List<string> SelectKeys();

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="SearchText"></param>
        /// <returns></returns>
        List<string> Search(string SearchText);

        /// <summary>
        /// 根據名稱來新增，並可傳入複製名稱以複製現有資料
        /// </summary>
        /// <param name="NewKey">新的鍵值名稱</param>
        /// <param name="CopyKey">要複製的鍵值名稱</param>
        /// <returns></returns>
        string Insert(string NewKey,string CopyKey);

        /// <summary>
        /// 更改鍵值名稱
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="NewKey"></param>
        /// <returns></returns>
        string Update(string Key, string NewKey);

        /// <summary>
        /// 根據鍵值名稱刪除資料
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        string Delete(string Key);

        /// <summary>
        /// 根據鍵值名稱選出資料
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        T Select(string Key);
        
        /// <summary>
        /// 根據資料來更新值
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        string Save(T Value);

        ///// <summary>
        ///// 匯出
        ///// </summary>
        ///// <param name="Key"></param>
        //void Export(string Key);

        /// <summary>
        /// 匯出
        /// </summary>
        //void Import();

        List<ICommand> ExtraCommands { get; }
    }
}