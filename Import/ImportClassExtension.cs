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
    /// 匯入班級排課資料
    /// </summary>
    public class ImportClassExtension : ImportWizard  
    {
        private const string constClassName = "班級名稱";
        private const string constTimeTableName = "時間表名稱";
        private StringBuilder mstrLog = new StringBuilder();
        private ImportOption mOption;
        private Dictionary<string, string> mClassNameIDs;
        private ImportTimeTableHelper mImportTimeTableHelper;
        private Task mTask;
        private AccessHelper mHelper; 

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/kunhsiang.com/sunset/home/yan-zheng-gui-ze/ClassExtension.xml";
        }

        /// <summary>
        /// 取得支援的匯入動作
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.Update | ImportAction.Delete;
        }

        /// <summary>
        /// 匯入前準備
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            mHelper = new AccessHelper();
            mOption = Option;
            mClassNameIDs = new Dictionary<string, string>();
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
                        mClassNameIDs.Add(ClassName, ClassID);
                }
            }
            );

            mImportTimeTableHelper = new ImportTimeTableHelper(mHelper);
        }

        /// <summary>
        /// 實際匯入
        /// </summary>
        /// <param name="Rows"></param>
        /// <returns></returns>
        public override string Import(List<IRowStream> Rows)
        {
            mTask.Wait();

            mstrLog.Clear();

            if (mOption.SelectedKeyFields.Count == 1 &&
                mOption.SelectedKeyFields.Contains(constClassName))
            {
                #region 取得已存在的排課課程資料
                List<ClassExtension> mClassExtensions = new List<ClassExtension>();
                List<string> ClassIDs = new List<string>();

                foreach (IRowStream Row in Rows)
                {
                    string ClassName = Row.GetValue(constClassName);
                    string ClassID = mClassNameIDs.ContainsKey(ClassName) ? mClassNameIDs[ClassName] : string.Empty;

                    if (!string.IsNullOrEmpty(ClassID))
                        ClassIDs.Add(ClassID);
                }

                string ClassIDsCondition = string.Join(",", ClassIDs.ToArray());
                mClassExtensions = mHelper.Select<ClassExtension>("ref_class_id in (" + ClassIDsCondition + ")");
                #endregion

                if (mOption.Action == ImportAction.Update)
                {
                    #region Step1:針對每筆匯入每筆資料檢查，若時間表及場地不存在，則自動新增
                    List<TimeTable> TimeTables = mImportTimeTableHelper.Insert(Rows);
                    string strTimeTableMessage = ImportTimeTableHelper.GetInsertMessage(TimeTables);
                    if (!string.IsNullOrEmpty(strTimeTableMessage))
                        mstrLog.AppendLine(strTimeTableMessage);
                    #endregion

                    #region Step2:針對每筆匯入每筆資料檢查，判斷是新增或是更新
                    List<ClassExtension> InsertRecords = new List<ClassExtension>();
                    List<ClassExtension> UpdateRecords = new List<ClassExtension>();

                    foreach (IRowStream Row in Rows)
                    {
                        string ClassName = Row.GetValue(constClassName);
                        int? ClassID = null;

                        if (mClassNameIDs.ContainsKey(ClassName))
                            ClassID = K12.Data.Int.ParseAllowNull(mClassNameIDs[ClassName]);

                        if (ClassID.HasValue)
                        {
                            ClassExtension vClassExtension = mClassExtensions
                                .Find(x => x.ClassID.Equals(ClassID.Value));

                            if (vClassExtension != null)
                            {
                                if (mOption.SelectedFields.Contains(constTimeTableName))
                                {
                                    string vTimeTableName = Row.GetValue(constTimeTableName);

                                    TimeTable vTimeTable = mImportTimeTableHelper[vTimeTableName];

                                    if (vTimeTable != null)
                                        vClassExtension.TimeTableID = K12.Data.Int.ParseAllowNull(vTimeTable.UID);
                                }

                                UpdateRecords.Add(vClassExtension);
                            }
                            else
                            {
                                vClassExtension = new ClassExtension();
                                vClassExtension.ClassID = ClassID.Value;

                                if (mOption.SelectedFields.Contains(constTimeTableName))
                                {
                                    string vTimeTableName = Row.GetValue(constTimeTableName);

                                    TimeTable vTimeTable = mImportTimeTableHelper[vTimeTableName];

                                    if (vTimeTable != null)
                                        vClassExtension.TimeTableID = K12.Data.Int.ParseAllowNull(vTimeTable.UID);
                                }

                                InsertRecords.Add(vClassExtension);
                            }
                        }
                    }

                    if (InsertRecords.Count > 0)
                    {
                        List<string> NewIDs = mHelper.InsertValues(InsertRecords);
                        mstrLog.AppendLine("已新增" + InsertRecords.Count + "筆排課班級資料。");
                        //在新增完後不需更新mCourseExtensions變數，因為來源資料不允許相同的課程重覆做新增
                    }
                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);
                        mstrLog.AppendLine("已更新" + UpdateRecords.Count + "筆排課班級資料。");
                        //在更新完後不需更新mCourseExtensions變數，因為來源資料不允許相同的課程重覆做更新
                    }
                    #endregion
                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    List<ClassExtension> DeleteRecords = new List<ClassExtension>();

                    //針對每筆記錄
                    foreach (IRowStream Row in Rows)
                    {
                        string ClassName = Row.GetValue(constClassName);
                        string ClassID = mClassNameIDs.ContainsKey(ClassName) ? mClassNameIDs[ClassName] : string.Empty;

                        //若有找到課程資料
                        if (!string.IsNullOrEmpty(ClassID))
                        {
                            //尋找是否有對應的排課班級資料
                            ClassExtension vClassExtension = mClassExtensions
                                .Find(x => x.ClassID.Equals(K12.Data.Int.Parse(ClassID)));
                            //若有找到則加入到刪除的集合中
                            if (vClassExtension != null)
                                DeleteRecords.Add(vClassExtension);
                        }
                    }

                    //若是要刪除的集合大於0才執行
                    if (DeleteRecords.Count > 0)
                    {
                        mHelper.DeletedValues(DeleteRecords);
                        mstrLog.AppendLine("已刪除" + DeleteRecords.Count + "筆排課班級資料。");
                    }
                }
            }

            return mstrLog.ToString();
        }
    }
}