using System;
using System.Collections.Generic;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 匯入時間表分段
    /// </summary>
    public class ImportTimeTableSec : ImportWizard
    {
        private const string constTimeTableName = "時間表名稱";
        private const string constWeekDay = "星期";
        private const string constPeriod = "節次";
        private const string constStartTime = "開始時間";
        private const string constEndTime = "結束時間";
        private const string constDispPeriod = "顯示節次";
        private const string constLocationName = "地點名稱";
        private const string constDisable = "不排課";
        private const string constDisableMessage = "不排課訊息";

        private StringBuilder mstrLog = new StringBuilder();
        private ImportOption mOption;
        private List<TimeTableSec> mTimeTableSecs;
        private AccessHelper mHelper;
        private ImportLocationHelper mImportLocationHelper;
        private ImportTimeTableHelper mImportTimeTableHelper;

        /// <summary>
        /// 空白建構式
        /// </summary>
        public ImportTimeTableSec()
        {
            this.CustomValidate += (Rows, Messages) => new TimeTableSecConflictHelper(Rows, Messages).CheckTimeConflict();
        }

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/kunhsiang.com/sunset/home/yan-zheng-gui-ze/TimeTableSec.xml";
        }

        /// <summary>
        /// 取得支援的匯入動作
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate | ImportAction.Delete;
        }

        /// <summary>
        /// 準備匯入
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            mOption = Option;
            mHelper = new AccessHelper();
            mTimeTableSecs = mHelper.Select<TimeTableSec>();
            mImportLocationHelper = new ImportLocationHelper(mHelper);
            mImportTimeTableHelper = new ImportTimeTableHelper(mHelper);
        }

        /// <summary>
        /// 分批執行匯入
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <returns>分批匯入完成訊息</returns>
        public override string Import(List<IRowStream> Rows)
        {
            mstrLog.Clear();

            //若選擇的鍵值有三個欄位，並且為時間表名稱。
            if (mOption.SelectedKeyFields.Count == 3 && 
                mOption.SelectedKeyFields.Contains(constTimeTableName) && 
                mOption.SelectedKeyFields.Contains(constWeekDay) && 
                mOption.SelectedKeyFields.Contains(constPeriod))
            {
                #region 新增或更新資料
                if (mOption.Action == ImportAction.InsertOrUpdate)
                {
                    #region Step1:針對每筆匯入每筆資料檢查，若時間表及地點不存在，則自動新增
                    List<Location> Locations = mImportLocationHelper.Insert(Rows);
                    string InsertLocationMessage = ImportLocationHelper.GetInsertMessage(Locations);
                    if (!string.IsNullOrEmpty(InsertLocationMessage))
                        mstrLog.AppendLine(InsertLocationMessage);

                    List<TimeTable> TimeTables = mImportTimeTableHelper.Insert(Rows);
                    string InsertTimeTableMessage = ImportTimeTableHelper.GetInsertMessage(TimeTables);
                    if (!string.IsNullOrEmpty(InsertTimeTableMessage))
                        mstrLog.AppendLine(InsertTimeTableMessage);
                    #endregion

                    #region Step2:針對每筆匯入每筆資料檢查，判斷是新增或是更新
                    List<TimeTableSec> InsertRecords = new List<TimeTableSec>();
                    List<TimeTableSec> UpdateRecords = new List<TimeTableSec>();

                    //int Progress = 0;

                    foreach (IRowStream Row in Rows)
                    {
                        string TimeTableName = Row.GetValue(constTimeTableName);
                        string TimeTableID = mImportTimeTableHelper[TimeTableName].UID;
                        int WeekDay = K12.Data.Int.Parse(Row.GetValue(constWeekDay));
                        int Period = K12.Data.Int.Parse(Row.GetValue(constPeriod));

                        #region 轉換開始時間及持續分鐘
                        string StartTime = Row.GetValue(constStartTime);
                        string EndTime = Row.GetValue(constEndTime);

                        Tuple<DateTime,int> StorageTime = Utility.GetStorageTime(StartTime, EndTime);
                        DateTime vBeginTime = StorageTime.Item1;
                        int Duration = StorageTime.Item2;
                        #endregion

                        int DispPeriod = K12.Data.Int.Parse(Row.GetValue(constDispPeriod));

                        TimeTableSec vTimeTableSec = mTimeTableSecs.Find(x=>                            
                            x.TimeTableID.Equals(K12.Data.Int.Parse(TimeTableID)) && 
                            x.WeekDay.Equals(WeekDay) && 
                            x.Period.Equals(Period));

                        //若TimeTableSec不為nul則為更新
                        if (vTimeTableSec != null)
                        {
                            vTimeTableSec.BeginTime = vBeginTime;
                            vTimeTableSec.Duration = Duration;
                            vTimeTableSec.DispPeriod = DispPeriod;                            

                            #region 若執行完後LocationID不為空白，代表來源資料中有地點名稱，而且也在後端資料中
                            if (mOption.SelectedFields.Contains(constLocationName))
                            {
                                string LocationID = string.Empty;

                                string LocationName = Row.GetValue(constLocationName);

                                Location vLocation = mImportLocationHelper[LocationName];

                                if (vLocation != null)
                                    LocationID = vLocation.UID;

                                vTimeTableSec.LocationID = K12.Data.Int.ParseAllowNull(LocationID);
                            }
                            #endregion

                            if (mOption.SelectedFields.Contains(constDisable))
                            {
                                bool Disable = Row.GetValue(constDisable).Equals("是") ? true : false;
                                vTimeTableSec.Disable = Disable;
                            }

                            if (mOption.SelectedFields.Contains(constDisableMessage))
                            {
                                string DisableMessage = Row.GetValue(constDisableMessage);
                                vTimeTableSec.DisableMessage  = DisableMessage;
                            }

                            UpdateRecords.Add(vTimeTableSec);
                        }
                        else //否則為新增
                        {
                            vTimeTableSec = new TimeTableSec();
                            vTimeTableSec.TimeTableID = K12.Data.Int.Parse(TimeTableID);
                            vTimeTableSec.WeekDay = WeekDay;
                            vTimeTableSec.Period = Period;
                            vTimeTableSec.BeginTime = vBeginTime;
                            vTimeTableSec.Duration = Duration;
                            vTimeTableSec.DispPeriod = DispPeriod;
                            vTimeTableSec.LocationID = null;

                            #region 若執行完後LocationID不為空白，代表來源資料中有地點名稱，而且也在後端資料中
                            if (mOption.SelectedFields.Contains(constLocationName))
                            {
                                string LocationID = string.Empty;

                                string LocationName = Row.GetValue(constLocationName);

                                Location vLocation = mImportLocationHelper[LocationName];

                                if (vLocation != null)
                                    LocationID = vLocation.UID;

                                vTimeTableSec.LocationID = K12.Data.Int.ParseAllowNull(LocationID);
                            }
                            #endregion

                            if (mOption.SelectedFields.Contains(constDisable))
                            {
                                bool Disable = Row.GetValue(constDisable).Equals("是") ? true : false;
                                vTimeTableSec.Disable = Disable;
                            }

                            if (mOption.SelectedFields.Contains(constDisableMessage))
                            {
                                string DisableMessage = Row.GetValue(constDisableMessage);
                                vTimeTableSec.DisableMessage = DisableMessage;
                            }
                            
                            InsertRecords.Add(vTimeTableSec);
                        }
                    }
                    #endregion

                    #region Step3:實際新增或更新資料
                    if (InsertRecords.Count > 0)
                    {
                        List<string> InsertIDs = mHelper.InsertValues(InsertRecords);
                        
                        mstrLog.AppendLine("已新增" + InsertRecords.Count + "筆時間表分段");

                        List<TimeTableSec> TimeTableSecs = mHelper.Select<TimeTableSec>("UID in ("+string.Join(",",InsertIDs.ToArray())+")");

                        TimeTableSecs.ForEach(x=>mTimeTableSecs.Add(x));
                    }
                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);
                        mstrLog.AppendLine("已更新" + UpdateRecords.Count + "筆時間表分段");
                    }

                    #endregion
                }
                #endregion
                #region 刪除資料
                else if (mOption.Action == ImportAction.Delete)
                {
                    //建立要刪除的資料
                    List<TimeTableSec> DeleteRecords = new List<TimeTableSec>();

                    //針對每筆記錄
                    foreach (IRowStream Row in Rows)
                    {
                        //取得鍵值資料，為時間表名稱、星期及節次
                        string TimeTableName = Row.GetValue(constTimeTableName);
                        TimeTable TimeTable = mImportTimeTableHelper[TimeTableName];
                        string TimeTableID = TimeTable!=null ? TimeTable.UID : string.Empty;
                        int WeekDay = K12.Data.Int.Parse(Row.GetValue(constWeekDay));
                        int Period = K12.Data.Int.Parse(Row.GetValue(constPeriod));

                        //若時間表編號不為空白，因其為鍵值
                        if (!string.IsNullOrEmpty(TimeTableID))
                        {
                            //嘗試取得時間表分段
                            TimeTableSec vTimeTableSec = mTimeTableSecs.Find(x => 
                                x.TimeTableID.Equals(K12.Data.Int.Parse(TimeTableID)) && 
                                x.WeekDay.Equals(WeekDay) && 
                                x.Period.Equals(Period));

                            //若TimeTableSec不為nul則為更新
                            if (vTimeTableSec != null)
                                DeleteRecords.Add(vTimeTableSec);
                        }
                    }

                    mHelper.DeletedValues(DeleteRecords);

                    mstrLog.AppendLine("已刪除"+DeleteRecords.Count+"筆時間表分段記錄");

                    DeleteRecords.ForEach(x => mTimeTableSecs.Remove(x));
                }
                #endregion
            }

            return mstrLog.ToString();
        }
    }
}