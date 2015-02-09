using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.Data;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 匯入教師不排課時段
    /// </summary>
    public class ImportTeacherBusy : ImportWizard
    {
        private const string constTeacehrName = "教師姓名";
        private const string constTeacherNickName = "教師暱稱";
        private const string constWeekday = "星期";
        private const string constStartTime = "開始時間";
        private const string constEndTime = "結束時間";
        private const string constBusyDesc = "不排課描述";
        private const string constLocationName = "所在地點";

        private ImportOption mOption;
        private AccessHelper mHelper;
        private Dictionary<string, string> mTeacherNameIDs = new Dictionary<string, string>();
        private StringBuilder mstrLog = new StringBuilder();
        private ImportLocationHelper mImportLocationHelper;
        private Task mTask;

        /// <summary>
        /// 建構式
        /// </summary>
        public ImportTeacherBusy()
        {
            this.CustomValidate += (Rows, Messages) => new TeacherBusyTimeConflictHelper(Rows, Messages).CheckTimeConflict();
        }

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/kunhsiang.com/sunset/home/yan-zheng-gui-ze/TeacherBusy.xml";
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
            mTask = Task.Factory.StartNew
            (() =>
            {
                QueryHelper Helper = new QueryHelper();

                DataTable Table = Helper.Select("select id,teacher_name,nickname from teacher where status=1");

                foreach (DataRow Row in Table.Rows)
                {
                    string TeacherID = Row.Field<string>("id");
                    string TeacherName = Row.Field<string>("teacher_name");
                    string TeacherNickname = Row.Field<string>("nickname");
                    string TeacherKey = TeacherName +","+TeacherNickname;

                    if (!mTeacherNameIDs.ContainsKey(TeacherKey))
                        mTeacherNameIDs.Add(TeacherKey,TeacherID);
                }
            }
            );
            
            mImportLocationHelper = new ImportLocationHelper(mHelper);   
        }


        /// <summary>
        /// 分批執行匯入
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <returns>分批匯入完成訊息</returns>
        public override string Import(List<IRowStream> Rows)
        {
            mstrLog.Clear();
            mTask.Wait();

            if (mOption.SelectedKeyFields.Count == 4 && 
                mOption.SelectedKeyFields.Contains(constTeacehrName) && 
                mOption.SelectedKeyFields.Contains(constTeacherNickName) && 
                mOption.SelectedKeyFields.Contains(constWeekday) && 
                mOption.SelectedKeyFields.Contains(constStartTime))
            {
                if (mOption.Action == ImportAction.InsertOrUpdate)
                {
                    //Step1:地點不存在自動新增的步驟已取消

                    #region Step2:針對每筆匯入資料檢查，轉換成對應的TeacherBusy，並且組合成查詢條件
                    List<string> TeacherBusyConditions = new List<string>();
                    Dictionary<TeacherBusy,IRowStream> TeachyBusyRowStreams = new Dictionary<TeacherBusy,IRowStream>();

                    foreach (IRowStream Row in Rows)
                    {
                        string TeacherName = Row.GetValue(constTeacehrName);
                        string TeacherNickName = Row.GetValue(constTeacherNickName);
                        int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekday));
                        string StartTime = Row.GetValue(constStartTime);
                        string EndTime = Row.GetValue(constEndTime);

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(StartTime,EndTime);

                        DateTime BeginDatetime = StorageTime.Item1;
                        int Duration = StorageTime.Item2;

                        //根據『教師姓名』及『教師暱稱』尋找是否有對應的『教師』
                        string TeacherKey = TeacherName + "," + TeacherNickName;
                        string TeacherID = mTeacherNameIDs.ContainsKey(TeacherKey) ? mTeacherNameIDs[TeacherKey] : string.Empty;

                        //若有找到『教師』才可匯入
                        if (!string.IsNullOrEmpty(TeacherID))
                        {
                            //根據『教師系統編號』、『星期』及『開始時間』
                            string TeacherBusyCondition = "(ref_teacher_id="+TeacherID
                                +" and weekday="+Weekday
                                +" and begin_time='1900/1/1 "+StorageTime.Item1.Hour+":"+ StorageTime.Item1.Minute+"')";
                            TeacherBusyConditions.Add(TeacherBusyCondition);

                            TeacherBusy vTeacherBusy = new TeacherBusy();
                            vTeacherBusy.TeacherID = K12.Data.Int.Parse(TeacherID);
                            vTeacherBusy.WeekDay = Weekday;
                            vTeacherBusy.BeginTime = BeginDatetime;
                            vTeacherBusy.Duration = Duration;
                            vTeacherBusy.BusyDesc = string.Empty;
                            vTeacherBusy.LocationID = null;

                            if (mOption.SelectedFields.Contains(constBusyDesc))
                            {
                                string TeacherBusyDesc = Row.GetValue(constBusyDesc);
                                vTeacherBusy.BusyDesc = TeacherBusyDesc;
                            }

                            if (mOption.SelectedFields.Contains(constLocationName))
                            {
                                string LocationName = Row.GetValue(constLocationName);

                                Location vLocation = mImportLocationHelper[LocationName];

                                int? vLocationID = null;                                                               

                                if (vLocation != null)
                                    vLocationID = K12.Data.Int.ParseAllowNull(vLocation.UID);

                                vTeacherBusy.LocationID = vLocationID;
                            }

                            if (!TeachyBusyRowStreams.ContainsKey(vTeacherBusy))
                                TeachyBusyRowStreams.Add(vTeacherBusy,Row);
                        }
                    }
                    #endregion

                    #region Step3:組合查詢條件，並選出系統中已存在的教師不排課時段
                    List<TeacherBusy> ExistTeacherBusys = new List<TeacherBusy>();
                    
                    string strCondition = string.Join(" or ", TeacherBusyConditions.ToArray());

                    //QueryHelper helper = new QueryHelper();

                    //DataTable Table = helper.Select("select uid from $scheduler.teacher_busy where " + strCondition);

                    //List<string> IDs = new List<string>();

                    //foreach(DataRow Row in Table.Rows)
                    //    IDs.Add(Row.Field<string>("uid"));

                    string strUDTCondition = Utility.SelectIDCondition("$scheduler.teacher_busy", strCondition);

                    ExistTeacherBusys = new List<TeacherBusy>();
                    
                    if (!string.IsNullOrWhiteSpace(strUDTCondition))
                      ExistTeacherBusys = mHelper.Select<TeacherBusy>(strUDTCondition);
                    #endregion

                    #region Step4:根據轉換的結構及已選出的系統資料決定新增及更新的記錄
                    List<TeacherBusy> InsertRecords = new List<TeacherBusy>();
                    List<TeacherBusy> UpdateRecords = new List<TeacherBusy>();

                    foreach (TeacherBusy TeacherBusy in TeachyBusyRowStreams.Keys)
                    {
                        TeacherBusy ExistTeacherBusy = ExistTeacherBusys.Find(x => 
                            x.TeacherID.Equals(TeacherBusy.TeacherID) && 
                            x.WeekDay.Equals(TeacherBusy.WeekDay) && 
                            DateTime.Equals(x.BeginTime, TeacherBusy.BeginTime));

                        if (ExistTeacherBusy != null)
                        {
                            ExistTeacherBusy.Duration = TeacherBusy.Duration;

                            if (mOption.SelectedFields.Contains(constBusyDesc))
                                ExistTeacherBusy.BusyDesc = TeacherBusy.BusyDesc;

                            if (mOption.SelectedFields.Contains(constLocationName))
                                ExistTeacherBusy.LocationID = TeacherBusy.LocationID;

                            UpdateRecords.Add(ExistTeacherBusy);
                        }
                        else
                            InsertRecords.Add(TeacherBusy);
                    }
                    #endregion

                    #region Step5:將資料實際新增到資料庫，並且做Log
                    if (InsertRecords.Count > 0)
                    {
                        mHelper.InsertValues(InsertRecords);
                        mstrLog.AppendLine("已成功新增"+InsertRecords.Count+"筆教師不排課時段");
                    }
                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);
                        mstrLog.AppendLine("已成功更新"+UpdateRecords.Count +"筆教師不排課時段");
                    }
                    #endregion
                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    #region Step1:針對每筆匯入資料轉換成鍵值條件。
                    List<string> TeacherBusyConditions = new List<string>();

                    foreach (IRowStream Row in Rows)
                    {
                        string TeacherName = Row.GetValue(constTeacehrName);
                        string TeacherNickName = Row.GetValue(constTeacherNickName);
                        int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekday));
                        int StartHour = K12.Data.Int.Parse(Row.GetValue(constStartTime));
                        int StartMinute = K12.Data.Int.Parse(Row.GetValue(constEndTime));
                        DateTime BeginDatetime = new DateTime(1900, 1, 1, StartHour, StartMinute, 0); //將小時及分鐘轉成實際的DateTime

                        //根據『教師姓名』及『教師暱稱』尋找是否有對應的『教師』
                        string TeacherKey = TeacherName + "," + TeacherNickName;
                        string TeacherID = mTeacherNameIDs.ContainsKey(TeacherKey) ? mTeacherNameIDs[TeacherKey] : string.Empty;

                        //若有找到『教師』才可匯入
                        if (!string.IsNullOrEmpty(TeacherID))
                        {
                            //根據『教師系統編號』、『星期』及『開始時間』
                            string TeacherBusyCondition = "(ref_teacher_id=" + TeacherID
                                + " and weekday=" + Weekday
                                + " and begin_time='1900/1/1 " + StartHour + ":" + StartMinute + "')";
                            TeacherBusyConditions.Add(TeacherBusyCondition);
                        }
                    }
                    #endregion

                    #region Step2:組合查詢條件，並選出系統中已存在的教師不排課時段

                    string strCondition = string.Join(" or ", TeacherBusyConditions.ToArray());

                    string strUDTCondition = Utility.SelectIDCondition("$scheduler.teacher_busy", strCondition);

                    List<TeacherBusy> ExistTeacherBusys = new List<TeacherBusy>();

                    if (!string.IsNullOrWhiteSpace(strUDTCondition))
                        ExistTeacherBusys = mHelper.Select<TeacherBusy>(strUDTCondition);
                    #endregion

                    #region Step3:若有找出的記錄，則加以刪除並記錄
                    if (ExistTeacherBusys.Count > 0)
                    {
                        mHelper.DeletedValues(ExistTeacherBusys);
                        mstrLog.AppendLine("已成功刪除"+ExistTeacherBusys.Count+"筆教師不排課時段。");
                    }
                    #endregion
                }
            }

            return mstrLog.ToString();
        }        
    }
}