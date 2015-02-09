using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.Data;
using FISCA.UDT;
using Sunset.Properties;

namespace Sunset
{
    /// <summary>
    /// 匯入班級不排課時段
    /// </summary>
    public class ImportClassBusy : ImportWizard
    {
        private const string constClassName = "班級名稱";
        private const string constWeekday = "星期";
        private const string constStartTime = "開始時間";
        private const string constEndTime = "結束時間";
        private const string constBusyDesc = "不排課描述";

        private ImportOption mOption;
        private AccessHelper mHelper;
        private Dictionary<string, string> mClassNameIDs = new Dictionary<string, string>();
        private StringBuilder mstrLog = new StringBuilder();
        private Task mTask;

        /// <summary>
        /// 建構式
        /// </summary>
        public ImportClassBusy()
        {
            //this.CustomValidate += (Rows, Messages) => new TeacherBusyTimeConflictHelper(Rows, Messages).CheckTimeConflict();
        }

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return Resources.ClassBusy;
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

                DataTable Table = Helper.Select("select id,class_name from class");

                foreach (DataRow Row in Table.Rows)
                {
                    string ClassID = Row.Field<string>("id");
                    string ClassName = Row.Field<string>("class_name");

                    if (!mClassNameIDs.ContainsKey(ClassName))
                        mClassNameIDs.Add(ClassName,ClassID);
                }
            }
            );
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

            if (mOption.SelectedKeyFields.Count == 3 && 
                mOption.SelectedKeyFields.Contains(constClassName) && 
                mOption.SelectedKeyFields.Contains(constWeekday) && 
                mOption.SelectedKeyFields.Contains(constStartTime))
            {
                if (mOption.Action == ImportAction.InsertOrUpdate)
                {
                    //Step1:地點不存在自動新增的步驟已取消

                    #region Step2:針對每筆匯入資料檢查，轉換成對應的TeacherBusy，並且組合成查詢條件
                    List<string> ClassBusyConditions = new List<string>();
                    Dictionary<ClassBusy,IRowStream> ClassBusyRowStreams = new Dictionary<ClassBusy,IRowStream>();

                    foreach (IRowStream Row in Rows)
                    {
                        string ClassName = Row.GetValue(constClassName);
                        int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekday));
                        string StartTime = Row.GetValue(constStartTime);
                        string EndTime = Row.GetValue(constEndTime);

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(StartTime,EndTime);

                        DateTime BeginDatetime = StorageTime.Item1;
                        int Duration = StorageTime.Item2;

                        //根據『教師姓名』及『教師暱稱』尋找是否有對應的『教師』
                        string ClassKey = ClassName;
                        string ClassID = mClassNameIDs.ContainsKey(ClassKey) ? mClassNameIDs[ClassKey] : string.Empty;

                        //若有找到『教師』才可匯入
                        if (!string.IsNullOrEmpty(ClassID))
                        {
                            //根據『教師系統編號』、『星期』及『開始時間』
                            string ClassBusyCondition = "(ref_class_id="+ClassID
                                +" and weekday="+Weekday
                                +" and begin_time='1900/1/1 "+StorageTime.Item1.Hour+":"+ StorageTime.Item1.Minute+"')";
                            ClassBusyConditions.Add(ClassBusyCondition);

                            ClassBusy vClassBusy = new ClassBusy();
                            vClassBusy.ClassID = K12.Data.Int.Parse(ClassID);
                            vClassBusy.WeekDay = Weekday;
                            vClassBusy.BeginTime = BeginDatetime;
                            vClassBusy.Duration = Duration;
                            vClassBusy.BusyDesc = string.Empty;

                            if (mOption.SelectedFields.Contains(constBusyDesc))
                            {
                                string TeacherBusyDesc = Row.GetValue(constBusyDesc);
                                vClassBusy.BusyDesc = TeacherBusyDesc;
                            }

                            if (!ClassBusyRowStreams.ContainsKey(vClassBusy))
                                ClassBusyRowStreams.Add(vClassBusy,Row);
                        }
                    }
                    #endregion

                    #region Step3:組合查詢條件，並選出系統中已存在的班級排課時段
                    List<ClassBusy> ExistClassBusys = new List<ClassBusy>();
                    
                    string strCondition = string.Join(" or ", ClassBusyConditions.ToArray());

                    string strUDTCondition = Utility.SelectIDCondition("$scheduler.class_busy", strCondition);

                    ExistClassBusys = new List<ClassBusy>();
                    
                    if (!string.IsNullOrWhiteSpace(strUDTCondition))
                      ExistClassBusys = mHelper
                          .Select<ClassBusy>(strUDTCondition);
                    #endregion

                    #region Step4:根據轉換的結構及已選出的系統資料決定新增及更新的記錄
                    List<ClassBusy> InsertRecords = new List<ClassBusy>();
                    List<ClassBusy> UpdateRecords = new List<ClassBusy>();

                    foreach (ClassBusy ClassBusy in ClassBusyRowStreams.Keys)
                    {
                        ClassBusy ExistTeacherBusy = ExistClassBusys.Find(x => 
                            x.ClassID.Equals(ClassBusy.ClassID) && 
                            x.WeekDay.Equals(ClassBusy.WeekDay) && 
                            DateTime.Equals(x.BeginTime, ClassBusy.BeginTime));

                        if (ExistTeacherBusy != null)
                        {
                            ExistTeacherBusy.Duration = ClassBusy.Duration;

                            if (mOption.SelectedFields.Contains(constBusyDesc))
                                ExistTeacherBusy.BusyDesc = ClassBusy.BusyDesc;

                            UpdateRecords.Add(ExistTeacherBusy);
                        }
                        else
                            InsertRecords.Add(ClassBusy);
                    }
                    #endregion

                    #region Step5:將資料實際新增到資料庫，並且做Log
                    if (InsertRecords.Count > 0)
                    {
                        mHelper.InsertValues(InsertRecords);
                        mstrLog.AppendLine("已成功新增"+InsertRecords.Count+"筆班級不排課時段");
                    }
                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);
                        mstrLog.AppendLine("已成功更新"+UpdateRecords.Count +"筆班級不排課時段");
                    }
                    #endregion
                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    #region Step1:針對每筆匯入資料轉換成鍵值條件。
                    List<string> ClassBusyConditions = new List<string>();

                    foreach (IRowStream Row in Rows)
                    {
                        string ClassName = Row.GetValue(constClassName);
                        int Weekday = K12.Data.Int.Parse(Row.GetValue(constWeekday));
                        int StartHour = K12.Data.Int.Parse(Row.GetValue(constStartTime));
                        int StartMinute = K12.Data.Int.Parse(Row.GetValue(constEndTime));
                        DateTime BeginDatetime = new DateTime(1900, 1, 1, StartHour, StartMinute, 0); //將小時及分鐘轉成實際的DateTime

                        //根據『教師姓名』及『教師暱稱』尋找是否有對應的『教師』
                        string ClassKey = ClassName;
                        string ClassID = mClassNameIDs.ContainsKey(ClassKey) ? mClassNameIDs[ClassKey] : string.Empty;

                        //若有找到『教師』才可匯入
                        if (!string.IsNullOrEmpty(ClassID))
                        {
                            //根據『教師系統編號』、『星期』及『開始時間』
                            string ClassBusyCondition = "(ref_class_id=" + ClassID
                                + " and weekday=" + Weekday
                                + " and begin_time='1900/1/1 " + StartHour + ":" + StartMinute + "')";
                            ClassBusyConditions.Add(ClassBusyCondition);
                        }
                    }
                    #endregion

                    #region Step2:組合查詢條件，並選出系統中已存在的班級不排課時段
                    string strCondition = string.Join(" or ", ClassBusyConditions.ToArray());

                    List<ClassBusy> ExistcClassBusys = mHelper.Select<ClassBusy>(strCondition);
                    #endregion

                    #region Step3:若有找出的記錄，則加以刪除並記錄
                    if (ExistcClassBusys.Count > 0)
                    {
                        mHelper.DeletedValues(ExistcClassBusys);
                        mstrLog.AppendLine("已成功刪除"+ExistcClassBusys.Count+"筆班級不排課時段。");
                    }
                    #endregion
                }
            }

            return mstrLog.ToString();
        }        
    }
}