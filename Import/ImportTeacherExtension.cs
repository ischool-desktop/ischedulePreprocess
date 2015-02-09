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
    /// 匯入場地
    /// </summary>
    public class ImportTeacherExtension : ImportWizard
    {
        private const string constTeacehrName = "教師姓名";
        private const string constTeacherNickName = "教師暱稱";
        private const string constBasicLength ="基本時數";
        private const string constExtraLength = "兼課時數";
        private const string constCounselingLength = "輔導時數";
        private const string constComment = "註記";
        private ImportOption mOption;
        private AccessHelper mHelper;
        private Dictionary<string, string> mTeacherNameIDs = new Dictionary<string, string>();
        private StringBuilder mstrLog = new StringBuilder();
        private Task mTask;

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "https://sites.google.com/a/kunhsiang.com/sunset/home/yan-zheng-gui-ze/TeacherExtension.xml";
        }

        /// <summary>
        /// 取得支援的匯入動作
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.Update;
        }

        /// <summary>
        /// 匯入前準備
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            mOption = Option;
            mHelper = new AccessHelper();
            mstrLog = new StringBuilder();

            mTask = Task.Factory.StartNew
            (() =>
            {
                QueryHelper Helper = new QueryHelper();

                DataTable Table = Helper.Select("select id,teacher_name,nickname from teacher status=1");

                foreach (DataRow Row in Table.Rows)
                {
                    string TeacherID = Row.Field<string>("id");
                    string TeacherName = Row.Field<string>("teacher_name");
                    string TeacherNickname = Row.Field<string>("nickname");
                    string TeacherKey = TeacherName + "," + TeacherNickname;

                    if (!mTeacherNameIDs.ContainsKey(TeacherKey))
                        mTeacherNameIDs.Add(TeacherKey, TeacherID);
                }
            }
            );
        }

        /// <summary>
        /// 實際分批匯入
        /// </summary>
        /// <param name="Rows"></param>
        /// <returns></returns>
        public override string Import(List<IRowStream> Rows)
        {
            mTask.Wait();

            mstrLog.Clear();

            //假設欄位只有1個，並且鍵值名稱為『名稱』
            if (mOption.SelectedKeyFields.Count == 2 && 
                mOption.SelectedKeyFields.Contains(constTeacehrName) &&
                mOption.SelectedKeyFields.Contains(constTeacherNickName))
            {
                #region 找出已經存在的教師排課資料
                List<string> TeacherIDs = new List<string>();

                foreach (IRowStream Row in Rows)
                {
                    string TeacherName = Row.GetValue(constTeacehrName);
                    string TeacherNickName = Row.GetValue(constTeacherNickName);
                    string TeacherKey = TeacherName + "," + TeacherNickName;

                    if (mTeacherNameIDs.ContainsKey(TeacherKey))
                        TeacherIDs.Add(mTeacherNameIDs[TeacherKey]);
                }

                string strCondition = "ref_teacher_id in (" + string.Join(",", TeacherIDs.ToArray()) + ")";

                AccessHelper helper = new AccessHelper();

                List<TeacherExtension> SourceRecords = helper.Select<TeacherExtension>(strCondition);
                #endregion

                //若使用者選擇的是新增或更新
                if (mOption.Action == ImportAction.Update)
                {
                    #region 將匯入資料轉成新增或更新的資料庫記錄
                    List<TeacherExtension> InsertRecords = new List<TeacherExtension>();
                    List<TeacherExtension> UpdateRecords = new List<TeacherExtension>();

                    //針對每筆資料判斷是新增還是更新
                    foreach (IRowStream Row in Rows)
                    {
                        string TeacherName = Row.GetValue(constTeacehrName);
                        string TeacherNickName = Row.GetValue(constTeacherNickName);
                        string TeacherKey = TeacherName + "," + TeacherNickName;

                        if (mTeacherNameIDs.ContainsKey(TeacherKey))
                        {
                            string TeacherID = mTeacherNameIDs[TeacherKey];

                            TeacherExtension Teacher = SourceRecords
                                .Find(x => ("" + x.TeacherID).Equals(TeacherID));

                            if (Teacher != null)
                            {
                                if (mOption.SelectedFields.Contains(constBasicLength))
                                    Teacher.BasicLength = K12.Data.Int.ParseAllowNull(Row.GetValue(constBasicLength));

                                if (mOption.SelectedFields.Contains(constCounselingLength))
                                    Teacher.CounselingLength = K12.Data.Int.ParseAllowNull(Row.GetValue(constCounselingLength));

                                if (mOption.SelectedFields.Contains(constExtraLength))
                                    Teacher.ExtraLength = K12.Data.Int.ParseAllowNull(Row.GetValue(constExtraLength));

                                if (mOption.SelectedFields.Contains(constComment))
                                    Teacher.Comment = Row.GetValue(constComment);

                                UpdateRecords.Add(Teacher);
                            }
                            else
                            {
                                Teacher = new TeacherExtension();
                                Teacher.TeacherID = K12.Data.Int.Parse(TeacherID);

                                if (mOption.SelectedFields.Contains(constBasicLength))
                                    Teacher.BasicLength = K12.Data.Int.ParseAllowNull(Row.GetValue(constBasicLength));

                                if (mOption.SelectedFields.Contains(constCounselingLength))
                                    Teacher.CounselingLength = K12.Data.Int.ParseAllowNull(Row.GetValue(constCounselingLength));

                                if (mOption.SelectedFields.Contains(constExtraLength))
                                    Teacher.ExtraLength = K12.Data.Int.ParseAllowNull(Row.GetValue(constExtraLength));

                                if (mOption.SelectedFields.Contains(constComment))
                                    Teacher.Comment = Row.GetValue(constComment);

                                InsertRecords.Add(Teacher);
                            }
                        }
                    }

                    #endregion

                    #region 將資料實際新增到資料庫
                    mHelper.InsertValues(InsertRecords);
                    mHelper.UpdateValues(UpdateRecords);
                    mstrLog.AppendLine("已成功新增或更新" + Rows.Count + "筆教師排課資料");
                    #endregion
                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    mHelper.DeletedValues(SourceRecords);

                    mstrLog.AppendLine("已成功刪除" + SourceRecords.Count + "筆教師排課資料");
                }
            }

            return mstrLog.ToString();
        }
    }
}