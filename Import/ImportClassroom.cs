using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 匯入場地
    /// </summary>
    public class ImportClassroom : ImportWizard
    {
        private const string constClassroomName = "場地名稱";
        private const string constClassroomDesc = "場地描述";
        //private const string constClassroomCode = "場地代碼";
        private const string constClassroomCapacity = "場地班級容納數";
        private const string constLocationOnly = "無班級容納數限制";
        private const string constLocationName = "地點名稱";

        private StringBuilder mstrLog;
        private ImportLocationHelper mImportLocationHelper;
        private ImportOption mOption;
        private AccessHelper mHelper;

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/kunhsiang.com/sunset/home/yan-zheng-gui-ze/Classroom.xml";
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
        /// 匯入前準備
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            mOption = Option;
            mHelper = new AccessHelper();
            mImportLocationHelper = new ImportLocationHelper(mHelper);
            mstrLog = new StringBuilder();
        }

        /// <summary>
        /// 實際分批匯入
        /// </summary>
        /// <param name="Rows"></param>
        /// <returns></returns>
        public override string Import(List<IRowStream> Rows)
        {
            mstrLog.Clear();

            //假設欄位只有1個，並且鍵值名稱為『名稱』
            if (mOption.SelectedKeyFields.Count == 1 && 
                mOption.SelectedKeyFields.Contains(constClassroomName))
            {
                #region Step1:針對每筆匯入每筆資料檢查，若地點不存在，則自動新增
                List<Location> Locations = mImportLocationHelper.Insert(Rows);
                string InsertLocationMessage = ImportLocationHelper.GetInsertMessage(Locations);
                if (!string.IsNullOrEmpty(InsertLocationMessage))
                    mstrLog.AppendLine(InsertLocationMessage);
                #endregion

                #region 根據場地名稱取得現有記錄，假設場地名稱不會重覆
                List<string> SourceKeys = new List<string>();

                foreach (IRowStream Row in Rows)
                {
                    string ClassroomName = Row.GetValue(constClassroomName);

                    SourceKeys.Add("'" + ClassroomName + "'");
                }
                //ClassroomName in ('a','b','c')
                string strCondition = "name in (" + string.Join(",", SourceKeys.ToArray()) + ")";

                Dictionary<string, Classroom> SourceRecords = mHelper
                    .Select<Classroom>(strCondition)
                    .ToDictionary(x => x.ClassroomName);
                #endregion
                //若使用者選擇的是新增或更新
                if (mOption.Action == ImportAction.InsertOrUpdate)
                {
                    #region 將匯入資料轉成新增或更新的資料庫記錄
                    List<Classroom> InsertRecords = new List<Classroom>();
                    List<Classroom> UpdateRecords = new List<Classroom>();

                    foreach (IRowStream Row in Rows)
                    {
                        string ClassroomName = Row.GetValue(constClassroomName);

                        if (SourceRecords.ContainsKey(ClassroomName))
                        {
                            Classroom UpdateClassroom = SourceRecords[ClassroomName];

                            if (mOption.SelectedFields.Contains(constClassroomDesc))
                            {
                                string ClassroomDesc = Row.GetValue(constClassroomDesc);
                                UpdateClassroom.ClassroomDesc = ClassroomDesc;
                            }
                            
                            //if (mOption.SelectedFields.Contains(constClassroomCode))
                            //{
                            //    string ClassroomCode = Row.GetValue(constClassroomCode);
                            //    UpdateClassroom.ClassroomCode = ClassroomCode;
                            //}

                            if (mOption.SelectedFields.Contains(constClassroomCapacity))
                            {
                                string Capacity = Row.GetValue(constClassroomCapacity);
                                UpdateClassroom.Capacity = Convert.ToInt32(Capacity);
                            }
                            if (mOption.SelectedFields.Contains(constLocationOnly))
                            {
                                string LocationOnly = Row.GetValue(constLocationOnly);
                                UpdateClassroom.LocationOnly = LocationOnly == "是" ? true : false;
                            }
                            if (mOption.SelectedFields.Contains(constLocationName))
                            {
                                string LocationName = Row.GetValue(constLocationName);
                                int? LocationID = null;

                                Location vLocation = mImportLocationHelper[LocationName];

                                if (vLocation != null)
                                    LocationID = K12.Data.Int.ParseAllowNull(vLocation.UID);

                                UpdateClassroom.LocationID = LocationID;
                            }

                            UpdateRecords.Add(UpdateClassroom);
                        }
                        else
                        {
                            Classroom NewClassroom = new Classroom();

                            NewClassroom.ClassroomName = Row.GetValue(constClassroomName);
                            NewClassroom.Capacity = 1;
                            NewClassroom.LocationOnly = false;
                            NewClassroom.LocationID = null;

                            if (mOption.SelectedFields.Contains(constClassroomDesc))
                            {
                                string ClassroomDesc = Row.GetValue(constClassroomDesc);
                                NewClassroom.ClassroomDesc = ClassroomDesc;
                            }

                            //if (mOption.SelectedFields.Contains(constClassroomCode))
                            //{
                            //    string ClassroomCode = Row.GetValue(constClassroomCode);
                            //    NewClassroom.ClassroomCode = ClassroomCode;
                            //}

                            if (mOption.SelectedFields.Contains(constClassroomCapacity))
                            {
                                string Capacity = Row.GetValue(constClassroomCapacity);
                                NewClassroom.Capacity = Convert.ToInt32(Capacity);
                            }

                            if (mOption.SelectedFields.Contains(constLocationOnly))
                            {
                                string LocationOnly = Row.GetValue(constLocationOnly);
                                NewClassroom.LocationOnly = LocationOnly == "是" ? true : false;
                            }

                            if (mOption.SelectedFields.Contains(constLocationName))
                            {
                                string LocationName = Row.GetValue(constLocationName);

                                Location vLocation = mImportLocationHelper[LocationName];

                                if (vLocation != null)
                                    NewClassroom.LocationID = K12.Data.Int.ParseAllowNull(vLocation.UID);
                            }

                            InsertRecords.Add(NewClassroom);
                        }
                    }
                    #endregion

                    #region 將資料實際新增到資料庫
                    mHelper.InsertValues(InsertRecords);
                    mHelper.UpdateValues(UpdateRecords);
                    mstrLog.AppendLine("已成功新增或更新" + Rows.Count + "筆場地");
                    #endregion
                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    mHelper.DeletedValues(SourceRecords.Values);

                    string strClassroomBusyCondition = string.Join(",", SourceRecords.Values.ToList().Select(x => x.UID).ToArray());

                    List<ClassroomBusy> ClassroomBusys = mHelper
                        .Select<ClassroomBusy>("ref_classroom_id in (" + strClassroomBusyCondition + ")");

                    mHelper.DeletedValues(ClassroomBusys);

                    mstrLog.AppendLine("已成功刪除" + SourceRecords.Values.Count + "筆場地");
                    mstrLog.AppendLine("已成功刪除" + ClassroomBusys.Count + "筆場地不排課時段");
                }
            }

            return mstrLog.ToString();
        }
    }
}