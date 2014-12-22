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
    public class ImportTeacherExBusy : ImportWizard
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

        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate | ImportAction.Delete;
        }

        public override string GetValidateRule()
        {
            return Properties.Resources.TeacherExBusy;
        }

        public override string Import(List<Campus.DocumentValidator.IRowStream> Rows)
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
                    Dictionary<TeacherExBusy, IRowStream> TeachyBusyRowStreams = new Dictionary<TeacherExBusy, IRowStream>();

                    foreach (IRowStream Row in Rows)
                    {
                        string TeacherName = Row.GetValue(constTeacehrName);
                        string TeacherNickName = Row.GetValue(constTeacherNickName);
                        int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekday));
                        string StartTime = Row.GetValue(constStartTime);
                        string EndTime = Row.GetValue(constEndTime);

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(StartTime, EndTime);

                        DateTime BeginDatetime = StorageTime.Item1;
                        int Duration = StorageTime.Item2;

                        //根據『教師姓名』及『教師暱稱』尋找是否有對應的『教師』
                        string TeacherKey = TeacherName + "," + TeacherNickName;
                        string TeacherID = mTeacherNameIDs.ContainsKey(TeacherKey) ? mTeacherNameIDs[TeacherKey] : string.Empty;

                        //若有找到『教師』才可匯入
                        if (!string.IsNullOrEmpty(TeacherID))
                        {
                            //根據『教師系統編號』、『星期』及『開始時間』
                            string TeacherBusyCondition = "(ref_teacher_id=" + TeacherID
                                + " and weekday=" + Weekday
                                + " and begin_time='1900/1/1 " + StorageTime.Item1.ToString(Utility.ImportBusyTimeFormat) + "')";
                            TeacherBusyConditions.Add(TeacherBusyCondition);

                            TeacherExBusy vTeacherBusy = new TeacherExBusy();
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
                                TeachyBusyRowStreams.Add(vTeacherBusy, Row);
                        }
                    }
                    #endregion

                    #region Step3:組合查詢條件，並選出系統中已存在的教師不排課時段
                    List<TeacherExBusy> ExistTeacherBusys = new List<TeacherExBusy>();

                    string strCondition = string.Join(" or ", TeacherBusyConditions.ToArray());

                    //QueryHelper helper = new QueryHelper();

                    //DataTable Table = helper.Select("select uid from $scheduler.teacher_busy where " + strCondition);

                    //List<string> IDs = new List<string>();

                    //foreach(DataRow Row in Table.Rows)
                    //    IDs.Add(Row.Field<string>("uid"));

                    string strUDTCondition = Utility.SelectIDCondition("$scheduler.teacher_ex_busy", strCondition);

                    ExistTeacherBusys = new List<TeacherExBusy>();

                    if (!string.IsNullOrWhiteSpace(strUDTCondition))
                        ExistTeacherBusys = mHelper.Select<TeacherExBusy>(strUDTCondition);
                    #endregion

                    #region Step4:根據轉換的結構及已選出的系統資料決定新增及更新的記錄
                    List<TeacherExBusy> InsertRecords = new List<TeacherExBusy>();
                    List<TeacherExBusy> UpdateRecords = new List<TeacherExBusy>();

                    foreach (TeacherExBusy TeacherBusy in TeachyBusyRowStreams.Keys)
                    {
                        TeacherExBusy ExistTeacherBusy = ExistTeacherBusys.Find(x =>
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
                        mstrLog.AppendLine("已成功新增" + InsertRecords.Count + "筆教師不排課時段");
                    }
                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);
                        mstrLog.AppendLine("已成功更新" + UpdateRecords.Count + "筆教師不排課時段");
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
                        string StartTime = Row.GetValue(constStartTime);
                        string EndTime = Row.GetValue(constEndTime);

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(StartTime, EndTime);

                        //根據『教師姓名』及『教師暱稱』尋找是否有對應的『教師』
                        string TeacherKey = TeacherName + "," + TeacherNickName;
                        string TeacherID = mTeacherNameIDs.ContainsKey(TeacherKey) ? mTeacherNameIDs[TeacherKey] : string.Empty;

                        //若有找到『教師』才可匯入
                        if (!string.IsNullOrEmpty(TeacherID))
                        {
                            //根據『教師系統編號』、『星期』及『開始時間』
                            string TeacherBusyCondition = "(ref_teacher_id=" + TeacherID
                                + " and weekday=" + Weekday
                                + " and begin_time='1900/1/1 " + StorageTime.Item1.ToString(Utility.ImportBusyTimeFormat) + "')";
                            TeacherBusyConditions.Add(TeacherBusyCondition);
                        }
                    }
                    #endregion

                    #region Step2:組合查詢條件，並選出系統中已存在的教師不排課時段

                    string strCondition = string.Join(" or ", TeacherBusyConditions.ToArray());

                    string strUDTCondition = Utility.SelectIDCondition("$scheduler.teacher_ex_busy", strCondition);

                    List<TeacherExBusy> ExistTeacherBusys = new List<TeacherExBusy>();

                    if (!string.IsNullOrWhiteSpace(strUDTCondition))
                        ExistTeacherBusys = mHelper.Select<TeacherExBusy>(strUDTCondition);
                    #endregion

                    #region Step3:若有找出的記錄，則加以刪除並記錄
                    if (ExistTeacherBusys.Count > 0)
                    {
                        mHelper.DeletedValues(ExistTeacherBusys);
                        mstrLog.AppendLine("已成功刪除" + ExistTeacherBusys.Count + "筆教師不排課時段。");
                    }
                    #endregion
                }

                FISCA.LogAgent.ApplicationLog.Log("排課", "匯入教師不排課時段", mstrLog.ToString());
            }

            return "";
        }

        /// <summary>
        /// 準備資料
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

                DataTable Table = Helper.Select("select uid,teacher_name,nickname from $scheduler.teacher_ex");

                foreach (DataRow Row in Table.Rows)
                {
                    string TeacherID = Row.Field<string>("uid");
                    string TeacherName = Row.Field<string>("teacher_name");
                    string TeacherNickname = Row.Field<string>("nickname");
                    string TeacherKey = TeacherName + "," + TeacherNickname;

                    if (!mTeacherNameIDs.ContainsKey(TeacherKey))
                        mTeacherNameIDs.Add(TeacherKey, TeacherID);
                }
            }
            );

            mImportLocationHelper = new ImportLocationHelper(mHelper);   
        }
    }
}