using System.Collections.Generic;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 匯入地點間距離
    /// </summary>
    public class ImportLLDistance : ImportWizard
    {
        private const string constLocationA ="來源地點名稱";
        private const string constLocationB ="目的地點名稱";
        private const string constDriveTime = "開車分鐘";
        private const string constDistance ="公里數";
        private StringBuilder mstrLog = new StringBuilder();
        private ImportOption mOption;
        private AccessHelper mHelper;
        private ImportLocationHelper mImportLocationHelper;

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/ischool.com.tw/leader/sunset/%E5%9C%B0%E9%BB%9E%E9%96%93%E8%B7%9D%E9%9B%A2.xml";
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
            mImportLocationHelper = new ImportLocationHelper(mHelper);
        }

        /// <summary>
        /// 分批執行匯入
        /// </summary>
        /// <param name="Rows">IRowStream集合</param>
        /// <returns>分批匯入完成訊息</returns>
        public override string Import(List<IRowStream> Rows)
        {
            if (mOption.SelectedKeyFields.Count == 2 && 
                mOption.SelectedKeyFields.Contains(constLocationA) && 
                mOption.SelectedKeyFields.Contains(constLocationB))
            {
                if (mOption.Action == ImportAction.InsertOrUpdate)
                {
                    #region Step1:新增地點名稱
                    //自動新增來源地點名稱
                    List<Location> Locations = mImportLocationHelper.Insert(Rows, constLocationA);
                    string Message = ImportLocationHelper.GetInsertMessage(Locations);
                    if (!string.IsNullOrEmpty(Message))
                        mstrLog.AppendLine(Message);

                    //自動新增目的地點名稱
                    Locations = mImportLocationHelper.Insert(Rows,constLocationB);
                    Message = ImportLocationHelper.GetInsertMessage(Locations);
                    if (!string.IsNullOrEmpty(Message))
                        mstrLog.AppendLine(Message);
                    #endregion

                    #region Step2:將IRowStream轉換成LLDistance結構
                    List<string> Conditions = new List<string>();
                    Dictionary<LLDistance,IRowStream> LLDistanceRowStreams = new Dictionary<LLDistance,IRowStream>();

                    foreach (IRowStream Row in Rows)
                    {
                        string LocationAName = Row.GetValue(constLocationA);
                        Location LocationA = mImportLocationHelper[LocationAName];
                        int? LocationAID = K12.Data.Int.ParseAllowNull(LocationA.UID);

                        string LocationBName = Row.GetValue(constLocationB);
                        Location LocationB = mImportLocationHelper[LocationBName];
                        int? LocationBID = K12.Data.Int.ParseAllowNull(LocationB.UID);

                        int DriveTime = K12.Data.Int.Parse(Row.GetValue(constDriveTime));
                        int Distance = K12.Data.Int.Parse(Row.GetValue(constDistance));

                        if (LocationAID.HasValue && LocationBID.HasValue)
                        {
                            LLDistance vLLDistance = new LLDistance();

                            vLLDistance.LocationAID = LocationAID.Value;
                            vLLDistance.LocationBID = LocationBID.Value;
                            vLLDistance.DriveTime = DriveTime;
                            vLLDistance.Distance = Distance;

                            if (!LLDistanceRowStreams.ContainsKey(vLLDistance))
                                LLDistanceRowStreams.Add(vLLDistance,Row);

                            Conditions.Add("(ref_locationa_id="+LocationAID+" and ref_locationb_id="+LocationBID+")");
                        }
                    }
                    #endregion

                    #region Step3:組合條件取得已經存在的LLDistance
                    string strCondition = string.Join(" or ", Conditions.ToArray());
                    List<LLDistance> ExistLLDistances = mHelper.Select<LLDistance>(strCondition);
                    #endregion

                    #region Step4:判斷轉換的結構是新增還是更新
                    List<LLDistance> InsertRecords = new List<LLDistance>();
                    List<LLDistance> UpdateRecords = new List<LLDistance>();

                    foreach(LLDistance LLDistance in LLDistanceRowStreams.Keys)
                    {
                        LLDistance ExistLLDistance = ExistLLDistances.Find(x => x.LocationAID.Equals(LLDistance.LocationAID) && x.LocationBID.Equals(LLDistance.LocationBID));

                        if (ExistLLDistance != null)
                        {
                            ExistLLDistance.DriveTime = LLDistance.DriveTime;
                            ExistLLDistance.Distance = LLDistance.Distance;
                            UpdateRecords.Add(ExistLLDistance);
                        }
                        else
                            InsertRecords.Add(LLDistance);
                    }
                    #endregion

                    #region Step5:將資料直接新增或更新到資料庫
                    if (InsertRecords.Count > 0)
                    {
                        mHelper.InsertValues(InsertRecords);
                        mstrLog.AppendLine("已成功新增" + InsertRecords.Count + "筆地點間距離");
                    }

                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);
                        mstrLog.AppendLine("已成功更新" + InsertRecords.Count + "筆地點間距離");
                    }
                    #endregion
                }
                else if (mOption.Action ==  ImportAction.Delete)
                {
                    #region Step1:將IRowStream轉換成LLDistance結構
                    List<string> Conditions = new List<string>();

                    foreach (IRowStream Row in Rows)
                    {
                        string LocationAName = Row.GetValue(constLocationA);
                        Location LocationA = mImportLocationHelper[LocationAName];
                        string LocationAID = LocationA != null ? LocationA.UID : string.Empty;

                        string LocationBName = Row.GetValue(constLocationB);
                        Location LocationB = mImportLocationHelper[LocationBName];
                        string LocationBID = LocationB != null ? LocationB.UID : string.Empty;

                        int DriveTime = K12.Data.Int.Parse(Row.GetValue(constDriveTime));
                        int Distance = K12.Data.Int.Parse(Row.GetValue(constDistance));

                        if (!string.IsNullOrEmpty(LocationAID) && !string.IsNullOrEmpty(LocationBID))
                            Conditions.Add("(ref_locationa_id=" + LocationAID + " and ref_locationb_id=" + LocationBID + ")");
                    }
                    #endregion                    

                    #region Step2:組合條件取得已經存在的LLDistance
                    string strCondition = string.Join(" or ", Conditions.ToArray());
                    List<LLDistance> ExistLLDistances = mHelper.Select<LLDistance>(strCondition);
                    #endregion

                    #region Step3:將資料實際刪除
                    if (ExistLLDistances.Count > 0)
                    {
                        mHelper.DeletedValues(ExistLLDistances);
                        mstrLog.AppendLine("已成功刪除" + ExistLLDistances.Count + "筆地點間距離");
                    }
                    #endregion
                }
            }

            return mstrLog.ToString();
        }
    }
}