using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Campus.DocumentValidator;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 在來源資料中有時間表名稱就自動新增或刪除，並傳回對應的時間表物件
    /// </summary>
    public class ImportTimeTableHelper
    {
        private AccessHelper mHelper;
        private Dictionary<string, TimeTable> mTimeTables;

        /// <summary>
        /// 取得新增訊息
        /// </summary>
        /// <param name="TimeTables">時間表物件列表</param>
        /// <returns>新增訊息</returns>
        public static string GetInsertMessage(List<TimeTable> TimeTables)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(TimeTables))
                return string.Empty;

            StringBuilder strBuilder = new StringBuilder();

            TimeTables.ForEach(x => strBuilder.AppendLine("已新增『" + x.TimeTableName + "』時間表"));
            strBuilder.AppendLine("共新增" + TimeTables.Count + "筆時間表");

            return strBuilder.ToString();
        }

        /// <summary>
        /// 取得刪除訊息
        /// </summary>
        /// <param name="TimeTables">時間表物件列表</param>
        /// <returns>刪除訊息</returns>
        public static string GetDeleteMessage(List<TimeTable> TimeTables)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(TimeTables))
                return string.Empty;

            StringBuilder strBuilder = new StringBuilder();

            TimeTables.ForEach(x => strBuilder.AppendLine("已刪除『" + x.TimeTableName + "』時間表"));
            strBuilder.AppendLine("共刪除" + TimeTables.Count + "筆時間表");

            return strBuilder.ToString();
        }

        /// <summary>
        /// 初始化取得資料
        /// </summary>
        private void Initialize()
        {
            mTimeTables = new Dictionary<string, TimeTable>();

            try
            {
                List<TimeTable> vTimeTables = mHelper.Select<TimeTable>();
                mTimeTables = vTimeTables.ToDictionary(x => x.TimeTableName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 建構式，會傳入預設的UDT存取來源
        /// </summary>
        /// <param name="Helper"></param>
        public ImportTimeTableHelper(AccessHelper Helper)
        {
            mHelper = Helper;
            Initialize();
        }

        /// <summary>
        /// 無參數建構式，會自動建立UDT存取來源
        /// </summary>
        public ImportTimeTableHelper()
        {
            mHelper = new AccessHelper();
            Initialize();
        }

        /// <summary>
        /// 根據時間表名稱取得時間表物件，若不存在會回傳null。
        /// </summary>
        /// <param name="TimeTableName">時間表名稱</param>
        /// <returns>時間表物件</returns>
        public TimeTable this[string TimeTableName]
        {
            get 
            {
                return mTimeTables.ContainsKey(TimeTableName) ? mTimeTables[TimeTableName] : null; 
            }
        }

        /// <summary>
        /// 根據IRowStream刪除時間表，預設的TimeTableName為『時間表名稱』
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <returns>刪除的時間表物件列表</returns>
        public List<TimeTable> Delete(List<IRowStream> Rows)
        {
            return Delete(Rows, "時間表名稱");
        }

        /// <summary>
        /// 根據IRowStream刪除時間表
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <param name="TimeTableNameField">TimeTableName的匯入欄位名稱</param>
        /// <returns>刪除的時間表物件列表</returns>
        public List<TimeTable> Delete(List<IRowStream> Rows, string TimeTableNameField)
        {
            if (!string.IsNullOrEmpty(TimeTableNameField) && Rows.Count > 0 && Rows[0].Contains(TimeTableNameField))
            {
                List<TimeTable> DeleteTimeTables = new List<TimeTable>();

                foreach (IRowStream Row in Rows)
                {
                    //判斷來源資料是否有包含地點欄位，若有的話才取值，否則傳回空白
                    string TimeTableName = Row.Contains(TimeTableNameField) ? Row.GetValue(TimeTableNameField) : string.Empty;

                    //若地點名稱不為空白，且現有記錄有包含，則加入到刪除的清單中
                    if (!string.IsNullOrEmpty(TimeTableName) && mTimeTables.ContainsKey(TimeTableName))
                        DeleteTimeTables.Add(mTimeTables[TimeTableName]);
                }

                mHelper.DeletedValues(DeleteTimeTables);

                DeleteTimeTables.ForEach(x =>
                    {
                        if (mTimeTables.ContainsKey(x.TimeTableName))
                            mTimeTables.Remove(x.TimeTableName);
                    }
                );

                return DeleteTimeTables;
            }

            return new List<TimeTable>();
        }

        /// <summary>
        /// 根據IRowStream新增地點，預設的TimeTableName為『時間表名稱』
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <returns>新增的時間表物件列表</returns>
        public List<TimeTable> Insert(List<IRowStream> Rows)
        {
            return Insert(Rows, "時間表名稱");
        }

        /// <summary>
        /// 根據IRowStream新增時間表
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <param name="TimeTableNameField">TimeTableName的匯入欄位名稱</param>
        /// <returns>新增的時間表物件列表</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<TimeTable> Insert(List<IRowStream> Rows, string TimeTableNameField)
        {
            //若來源筆數大於0，且來源資料有包含地點名稱才繼續
            if (!string.IsNullOrEmpty(TimeTableNameField) && Rows.Count > 0 && Rows[0].Contains(TimeTableNameField))
            {
                //要新增的地點集合
                List<TimeTable> InsertTimeTables = new List<TimeTable>();
                List<string> TimeTableNames = new List<string>();

                //針對每筆記錄
                foreach (IRowStream Row in Rows)
                {
                    //判斷來源資料是否有包含地點欄位，若有的話才取值，否則傳回空白
                    string TimeTableName = Row.Contains(TimeTableNameField) ? Row.GetValue(TimeTableNameField) : string.Empty;

                    //若地點名稱不為空白，且現有記錄中未包含此地點名稱，則建立新的物件
                    if (!string.IsNullOrEmpty(TimeTableName) && !mTimeTables.ContainsKey(TimeTableName))
                        if (!TimeTableNames.Contains(TimeTableName))
                        {
                            TimeTable vTimeTable = new TimeTable();
                            vTimeTable.TimeTableName = TimeTableName;
                            vTimeTable.TimeTableDesc = "此時間表為在匯入時自動建立。";
                            InsertTimeTables.Add(vTimeTable);
                            TimeTableNames.Add(TimeTableName);
                        }
                }

                //若有新的地點才進行新增，並將新增的資料放到集合中
                if (InsertTimeTables.Count > 0)
                {
                    List<string> NewIDs = mHelper.InsertValues(InsertTimeTables);

                    //重新取得時間表資料
                    string strCondition = "uid in (" + string.Join(",", NewIDs.ToArray()) + ")";

                    List<TimeTable> vTimeTables = mHelper.Select<TimeTable>();

                    vTimeTables.ForEach(x => 
                    {
                        if (!mTimeTables.ContainsKey(x.TimeTableName))
                            mTimeTables.Add(x.TimeTableName, x);
                    });

                    return vTimeTables;
                }
            }

            return new List<TimeTable>();
        }
    }
}