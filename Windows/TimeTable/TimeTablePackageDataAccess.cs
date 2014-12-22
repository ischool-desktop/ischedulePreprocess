using System.Collections.Generic;
using System.Data;
using System.Text;
using FISCA.Data;
using FISCA.UDT;
using Sunset.Windows;

namespace Sunset
{
    /// <summary>
    /// 時間表資料存取
    /// </summary>
    public class TimeTablePackageDataAccess : IConfigurationDataAccess<TimeTablePackage>
    {
        private AccessHelper mAccessHelper;
        private QueryHelper mQueryHelper;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public TimeTablePackageDataAccess()
        {
            mAccessHelper = new AccessHelper();
            mQueryHelper = new QueryHelper(); 
        }

        #region IConfigurationDataAccess<TimeTablePackage> 成員

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string DisplayName
        {
            get { return "時間表管理"; }
        }

        /// <summary>
        /// 取得所有時間表名稱
        /// </summary>
        /// <returns></returns>
        public List<string> SelectKeys()
        {
            DataTable table = mQueryHelper.Select("select name from $scheduler.timetable");

            List<string> Result = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string Name = row.Field<string>("name");
                Result.Add(Name);
            }

            return Result;
        }

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="SearchText">搜尋文字</param>
        /// <returns></returns>
        public List<string> Search(string SearchText)
        {
            DataTable table = mQueryHelper.Select("select name from $scheduler.timetable where name like '%"+ SearchText +"%'");

            List<string> Result = new List<string>();

            foreach (DataRow row in table.Rows)
            {
               string Name = row.Field<string>("name");
               Result.Add(Name);
            }

            return Result;
        }

        public string Update(string Key, string NewKey)
        {
            #region 根據鍵值取得時間表
            if (string.IsNullOrEmpty(Key))
                return "要新增的時間表名稱不能為空白!";

            string strCondition = "name='" + Key + "'";

            List<TimeTable> TimeTables = mAccessHelper.Select<TimeTable>(strCondition);

            if (TimeTables.Count == 1)
            {
                TimeTables[0].TimeTableName = NewKey;
                TimeTables.SaveAll();
                return string.Empty;
            }
            else
            {
                return "時間表不存在或超過兩筆以上";
            }
            #endregion
        }

        /// <summary>
        /// 新增時間表
        /// </summary>
        /// <param name="NewKey">時間表名稱</param>
        /// <param name="CopyKey">要複製的時間表名稱</param>
        /// <returns>傳回新增成功或失敗訊息</returns>
        public string Insert(string NewKey, string CopyKey)
        {
            #region 根據鍵值取得時間表
            if (string.IsNullOrEmpty(NewKey))
                return "要新增的時間表名稱不能為空白!";

            string strCondition = string.Empty;

            if (!string.IsNullOrEmpty(CopyKey))
                strCondition = "name in ('" + NewKey + "','" + CopyKey + "')";
            else
                strCondition = "name in ('" + NewKey + "')";

            List<TimeTable> TimeTables = mAccessHelper.Select<TimeTable>(strCondition);

            if (TimeTables.Find(x=>x.TimeTableName.Equals(NewKey))!=null)
                return "要新增的時間表已存在，無法新增!";
            #endregion

            #region 新增時間表
            TimeTable NewTimeTable = new TimeTable();
            NewTimeTable.TimeTableName = NewKey;
            //尋找要複製的時間表，若有找到的話則將TimeTableDesc複製過去
            TimeTable CopyTimeTable = TimeTables.Find(x => x.TimeTableName.Equals(CopyKey));
            NewTimeTable.TimeTableDesc = CopyTimeTable != null ? CopyTimeTable.TimeTableDesc : string.Empty;
            List<TimeTable> NewTimeTables = new List<TimeTable>();
            NewTimeTables.Add(NewTimeTable);

            List<string> NewTimeTableIDs = mAccessHelper.InsertValues(NewTimeTables);
            #endregion            
            
            #region 複製時間表分段
            List<string> NewTimeTableSecIDs = new List<string>();

            if (!string.IsNullOrEmpty(CopyKey) && NewTimeTableIDs.Count==1)
            {
                if (CopyTimeTable == null)
                    return "要複製的時間表不存在!";

                List<TimeTableSec> TimeTableSecs = mAccessHelper.Select<TimeTableSec>("ref_timetable_id=" + CopyTimeTable.UID);

                TimeTableSecs.ForEach(x=>x.TimeTableID = K12.Data.Int.Parse(NewTimeTableIDs[0]));

                NewTimeTableSecIDs = mAccessHelper.InsertValues(TimeTableSecs);
            }
            #endregion

            return "已成功新增" + NewTimeTableIDs.Count + "筆時間表及複製" + NewTimeTableSecIDs.Count + "筆時間表分段";
        }

        /// <summary>
        /// 根據鍵值刪除時間表及時間表分段
        /// </summary>
        /// <param name="Key">時間表名稱</param>
        /// <returns></returns>
        public string Delete(string Key)
        {
            #region 取得時間表
            List<TimeTable> vTimeTables = mAccessHelper.Select<TimeTable>("name='"+Key+"'");

            if (vTimeTables.Count == 0)
                return "找不到對應的時間表，無法刪除!";
            if (vTimeTables.Count > 1)
                return "根據時間表名稱" + Key + "找到兩筆以上的時間表，不知道要刪除哪筆!";

            TimeTable vTimeTable = vTimeTables[0];
            #endregion

            #region 取得時間表分段
            List<TimeTableSec> vTimeTableSecs = mAccessHelper.Select<TimeTableSec>("ref_timetable_id="+vTimeTable.UID);
            #endregion

            #region 刪除資料時間表及時間表分段
            mAccessHelper.DeletedValues(vTimeTables);

            if (vTimeTableSecs.Count>0)
                mAccessHelper.DeletedValues(vTimeTableSecs);
            #endregion

            return "已刪除時間表『" + vTimeTable.TimeTableName + "』及" + vTimeTableSecs.Count + "筆時間表分段!";
        }

        /// <summary>
        /// 根據時間表名稱取得時間表及時間表分段
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public TimeTablePackage Select(string Key)
        {
            #region 產生預設的TimeTablePackage，將TimeTable設為null，並將TimeTableSecs產生為空集合
            TimeTablePackage vTimeTablePackage = new TimeTablePackage();
            vTimeTablePackage.TimeTable = null;
            vTimeTablePackage.TimeTableSecs = new List<TimeTableSec>();
            #endregion

            #region 根據鍵值取得時間表
            List<TimeTable> vTimeTables = mAccessHelper.Select<TimeTable>("name='" + Key + "'");

            //若有時間表，則設定時間表，並再取得時間表分段
            if (vTimeTables.Count == 1)
            {
                TimeTable vTimeTable = vTimeTables[0];
                vTimeTablePackage.TimeTable = vTimeTable;
                List<TimeTableSec> vTimeTableSecs = mAccessHelper.Select<TimeTableSec>("ref_timetable_id=" + vTimeTable.UID);
                vTimeTablePackage.TimeTableSecs = vTimeTableSecs;
            }
            #endregion

            return vTimeTablePackage;
        }

        /// <summary>
        /// 根據TimeTablePackage物件更新
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Save(TimeTablePackage Value)
        {
            StringBuilder strBuilder = new StringBuilder();

            //若TimeTable不為null，且TimeTableSecs不為空集合
            if (Value.TimeTable != null)
            {
                List<TimeTable> vTimeTables = new List<TimeTable>();
                vTimeTables.Add(Value.TimeTable);
                mAccessHelper.UpdateValues(vTimeTables);
                strBuilder.AppendLine("已成功更新時間表『" + Value.TimeTable.TimeTableName + "』");
            }

            if(!K12.Data.Utility.Utility.IsNullOrEmpty(Value.TimeTableSecs))
            {
                mAccessHelper.SaveAll(Value.TimeTableSecs);
                strBuilder.AppendLine("已成功更新時間表分段共" + Value.TimeTableSecs.Count + "筆");
            }

            if (strBuilder.Length > 0)
                return strBuilder.ToString();

            return "時間表物件為null或是時間表分段為空集合無法進行更新";
        }

        /// <summary>
        /// 其它功能
        /// </summary>
        public List<ICommand> ExtraCommands
        {
            get
            {
                List<ICommand> Commands = new List<ICommand>();

                //Commands.Add(new ExportTimeTableCommand());
                Commands.Add(new ExportTimeTableSecCommand());
                //Commands.Add(new ImportTimeTableCommand());
                Commands.Add(new ImportTimeTableSecCommand());

                return Commands;
            }
        }

        #endregion

    }
}