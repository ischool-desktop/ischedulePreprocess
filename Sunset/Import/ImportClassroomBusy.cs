using System;
using System.Collections.Generic;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 匯入場地不排課時段
    /// </summary>
    public class ImportClassroomBusy : ImportWizard
    {
        private const string constClassroomName = "場地名稱";
        private const string constWeekday = "星期";
        private const string constStartTime = "開始時間";
        private const string constEndTime = "結束時間";
        private const string constBusyDesc = "不排課描述";
        private const string constWeekFlag = "單雙週";
        private ImportOption mOption;
        private AccessHelper mHelper;
        private StringBuilder mstrLog = new StringBuilder();
        private ImportClassroomHelper mImportClassroomHelper;

        /// <summary>
        /// 空白建構式
        /// </summary>
        public ImportClassroomBusy()
        {
            this.CustomValidate += (Rows, Messages) => new ClassroomBusyTimeConflictHelper(Rows, Messages).CheckTimeConflict(); 
        }

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return Properties.Resources.ClassroomBusy;
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
        /// 初始化匯入動作
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            mOption = Option;
            mHelper = new AccessHelper();
            mImportClassroomHelper = new ImportClassroomHelper(mHelper);
        }

        /// <summary>
        /// 實際分批匯入
        /// </summary>
        /// <param name="Rows"></param>
        /// <returns></returns>
        public override string Import(List<IRowStream> Rows)
        {
            mstrLog.Clear();

            if (mOption.SelectedKeyFields.Count == 3 && 
                mOption.SelectedKeyFields.Contains(constClassroomName) && 
                mOption.SelectedKeyFields.Contains(constWeekday) &&
                mOption.SelectedKeyFields.Contains(constStartTime))
            {
                if (mOption.Action == ImportAction.InsertOrUpdate)
                {
                    //Step0:地點不存在自動新增的步驟已取消

                    #region Step1:針對每筆匯入資料檢查，轉換成對應的ClassroomBusy，並且組合成查詢條件
                    List<string> ClassroomBusyConditions = new List<string>();
                    Dictionary<ClassroomBusy, IRowStream> ClassroomBusyStreams = new Dictionary<ClassroomBusy, IRowStream>();

                    foreach (IRowStream Row in Rows)
                    {
                        string ClassroomName = Row.GetValue(constClassroomName);
                        Classroom Classroom = mImportClassroomHelper[ClassroomName];
                        int? ClassroomID = K12.Data.Int.ParseAllowNull(Classroom.UID);
                        int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekday));
                        string StartTime = Row.GetValue(constStartTime);
                        string EndTime = Row.GetValue(constEndTime);

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(StartTime, EndTime);

                        DateTime BeginDatetime = StorageTime.Item1; //將小時及分鐘轉成實際的DateTime
                        int Duration = StorageTime.Item2;

                        if (ClassroomID.HasValue)
                        {
                            ClassroomBusy vClassroomBusy = new ClassroomBusy();

                            string ClassroomBusyCondition = "(ref_classroom_id=" + ClassroomID
                                + " and weekday=" + Weekday
                                + " and begin_time='1900/1/1 " + BeginDatetime.Hour + ":" + BeginDatetime.Minute + "')";
                            ClassroomBusyConditions.Add(ClassroomBusyCondition);

                            vClassroomBusy.ClassroomID = ClassroomID.Value;
                            vClassroomBusy.WeekDay = Weekday;           
                            vClassroomBusy.BeginTime = BeginDatetime;
                            vClassroomBusy.Duration = Duration;

                            if (mOption.SelectedFields.Contains(constBusyDesc))
                            {
                                string ClassroomBusyDesc = Row.GetValue(constBusyDesc);
                                vClassroomBusy.BusyDesc = ClassroomBusyDesc;
                            }

                            if (mOption.SelectedFields.Contains(constWeekFlag))
                            {
                                string strWeekFlag = Row.GetValue(constWeekFlag);
                                vClassroomBusy.WeekFlag = strWeekFlag.GetWeekFlagInt();
                            }

                            if (!ClassroomBusyStreams.ContainsKey(vClassroomBusy))
                                ClassroomBusyStreams.Add(vClassroomBusy, Row);
                        }
                    }
                    #endregion

                    #region Step3:組合查詢條件，並選出系統中已存在的場地不排課時段
                    string strCondition = string.Join(" or ", ClassroomBusyConditions.ToArray());

                    List<ClassroomBusy> ExistClassroomBusys = mHelper.Select<ClassroomBusy>(strCondition);
                    #endregion

                    #region Step4:根據轉換的結構及已選出的系統資料決定新增及更新的記錄
                    List<ClassroomBusy> InsertRecords = new List<ClassroomBusy>();
                    List<ClassroomBusy> UpdateRecords = new List<ClassroomBusy>();

                    foreach (ClassroomBusy ClassroomBusy in ClassroomBusyStreams.Keys)
                    {
                        ClassroomBusy ExistClassroomBusy = ExistClassroomBusys.Find(x =>
                            x.ClassroomID.Equals(ClassroomBusy.ClassroomID) &&
                            x.WeekDay.Equals(ClassroomBusy.WeekDay) &&
                            DateTime.Equals(x.BeginTime, ClassroomBusy.BeginTime));

                        if (ExistClassroomBusy != null)
                        {
                            ExistClassroomBusy.Duration = ClassroomBusy.Duration;

                            if (mOption.SelectedFields.Contains(constBusyDesc))
                                ExistClassroomBusy.BusyDesc = ClassroomBusy.BusyDesc;

                            if (mOption.SelectedFields.Contains(constWeekFlag))
                                ExistClassroomBusy.WeekFlag = ClassroomBusy.WeekFlag;

                            UpdateRecords.Add(ExistClassroomBusy);
                        }
                        else
                            InsertRecords.Add(ClassroomBusy);
                    }
                    #endregion

                    #region Step5:將資料實際新增到資料庫，並且做Log
                    if (InsertRecords.Count > 0)
                    {
                        mHelper.InsertValues(InsertRecords);
                        mstrLog.AppendLine("已成功新增" + InsertRecords.Count + "筆場地不排課時段");
                    }
                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);
                        mstrLog.AppendLine("已成功更新" + UpdateRecords.Count + "筆場地不排課時段");
                    }
                    #endregion
                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    #region Step1:針對每筆匯入資料轉換成鍵值條件。
                    List<string> ClassroomBusyConditions = new List<string>();

                    foreach (IRowStream Row in Rows)
                    {
                        string ClassroomName = Row.GetValue(constClassroomName);
                        Classroom Classroom = mImportClassroomHelper[ClassroomName];
                        int? ClassroomID = null;
                        if (Classroom != null)
                            ClassroomID = K12.Data.Int.ParseAllowNull(Classroom.UID);
                        int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekday));
                        int StartHour = K12.Data.Int.Parse(Row.GetValue(constStartTime));
                        int StartMinute = K12.Data.Int.Parse(Row.GetValue(constEndTime));
                        DateTime BeginDatetime = new DateTime(1900, 1, 1, StartHour, StartMinute, 0); //將小時及分鐘轉成實際的DateTime

                        //若有找到『場地』才可匯入
                        if (ClassroomID.HasValue)
                        {
                            //根據『場地系統編號』、『星期』及『開始時間』
                            string ClassroomBusyCondition = "(ref_classroom_id=" + ClassroomID
                                + " and weekday=" + Weekday
                                + " and begin_time='1900/1/1 " + StartHour + ":" + StartMinute + "')";
                            ClassroomBusyConditions.Add(ClassroomBusyCondition);
                        }
                    }
                    #endregion

                    #region Step2:組合查詢條件，並選出系統中已存在的場地不排課時段
                    string strCondition = string.Join(" or ", ClassroomBusyConditions.ToArray());

                    string strUDTCondition = Utility.SelectIDCondition("$scheduler.classroom_busy", strCondition);

                    List<ClassroomBusy> ExistClassroomBusys = mHelper.Select<ClassroomBusy>(strUDTCondition);
                    #endregion

                    #region Step3:若有找出的記錄，則加以刪除並記錄
                    if (ExistClassroomBusys.Count > 0)
                    {
                        mHelper.DeletedValues(ExistClassroomBusys);
                        mstrLog.AppendLine("已成功刪除" + ExistClassroomBusys.Count + "筆場地不排課時段。");
                    }
                    #endregion
                }

                FISCA.LogAgent.ApplicationLog.Log("排課", "匯入場地不排課時段", mstrLog.ToString());
            }

            return "";
        }
    }
}