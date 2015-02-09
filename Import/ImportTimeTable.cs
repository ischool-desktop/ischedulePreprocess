using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 匯入時間表
    /// </summary>
    public class ImportTimeTable : ImportWizard
    {
        private ImportOption mOption;
        private AccessHelper mHelper;
        private StringBuilder mstrBuilder;
        private const string constTimeTableName = "時間表名稱";
        private const string constTimeTableDesc = "時間表描述";

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/kunhsiang.com/sunset/home/yan-zheng-gui-ze/TimeTable.xml";
        }

        /// <summary>
        /// 取得支援的匯入動作
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            //傳回可以新增或更新及刪除
            return ImportAction.InsertOrUpdate | ImportAction.Delete;
        }

        /// <summary>
        /// 準備匯入動作
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            mOption = Option;
            mHelper = new AccessHelper();
            mstrBuilder = new StringBuilder();
        }

        /// <summary>
        /// 實際分批匯入
        /// </summary>
        /// <param name="Rows"></param>
        /// <returns></returns>
        public override string Import(List<IRowStream> Rows)
        {
            mstrBuilder.Clear();

            //假設欄位只有1個，並且鍵值名稱為『名稱』
            if (mOption.SelectedKeyFields.Count == 1 && 
                mOption.SelectedKeyFields.Contains(constTimeTableName))
            {
                #region 根據時間表名稱取得現有記錄，假設時間表名稱不會重覆
                List<string> SourceKeys = new List<string>();

                foreach (IRowStream Row in Rows)
                {
                    string TimeTableName = Row.GetValue(constTimeTableName);

                    SourceKeys.Add("'" + TimeTableName + "'");
                }

                Dictionary<string, TimeTable> SourceRecords = mHelper
                    .Select<TimeTable>("name in (" + string.Join(",", SourceKeys.ToArray()) + ")")
                    .ToDictionary(x => x.TimeTableName);
                #endregion
                //若使用者選擇的是新增或更新
                if (mOption.Action == ImportAction.InsertOrUpdate)
                {
                    #region 將匯入資料轉成新增或更新的資料庫記錄
                    List<TimeTable> InsertRecords = new List<TimeTable>();
                    List<TimeTable> UpdateRecords = new List<TimeTable>();

                    foreach (IRowStream Row in Rows)
                    {
                        string TimeTableName = Row.GetValue(constTimeTableName);
                        string TimeTableDesc = Row.GetValue(constTimeTableDesc);

                        if (SourceRecords.ContainsKey(TimeTableName))
                        {
                            TimeTable UpdateTimeTable = SourceRecords[TimeTableName];
                            UpdateTimeTable.TimeTableDesc = TimeTableDesc;
                            UpdateRecords.Add(UpdateTimeTable);
                        }
                        else
                        {
                            TimeTable NewTimeTable = new TimeTable();
                            NewTimeTable.TimeTableName = Row.GetValue(constTimeTableName);
                            NewTimeTable.TimeTableDesc = Row.GetValue(constTimeTableDesc);
                            InsertRecords.Add(NewTimeTable);
                        }
                    }
                    #endregion

                    #region 將資料實際新增到資料庫
                    mHelper.InsertValues(InsertRecords);
                    mHelper.UpdateValues(UpdateRecords);

                    mstrBuilder.AppendLine("已成功新增或更新" + Rows.Count + "筆時間表");
                    #endregion
                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    mHelper.DeletedValues(SourceRecords.Values);

                    string strCondition = string.Join(",",SourceRecords.Values.ToList().Select(x => x.UID).ToArray());

                    List<TimeTableSec> TimeTableSecs = mHelper
                        .Select<TimeTableSec>("ref_timetable_id in (" + strCondition + ")");

                    mHelper.DeletedValues(TimeTableSecs);

                    mstrBuilder.AppendLine("已成功刪除" + SourceRecords.Values.Count + "筆時間表");
                    mstrBuilder.AppendLine("已成功刪除" + TimeTableSecs.Count + "時間表分段");
                }
            }

            return mstrBuilder.ToString();
        }
    }
}