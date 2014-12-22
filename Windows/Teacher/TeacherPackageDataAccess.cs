using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using Sunset.Windows;
using System.Data;
using FISCA.Data;

namespace Sunset
{
    /// <summary>
    /// 教師資料存取
    /// </summary>
    public class TeacherPackageDataAccess : IConfigurationDataAccess<TeacherPackage>
    {
        private AccessHelper mAccessHelper;
        private QueryHelper mQueryHelper;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public TeacherPackageDataAccess()
        {
            mAccessHelper = new AccessHelper();
            mQueryHelper = new QueryHelper();
        }

        #region IConfigurationDataAccess<TeacherPackage> 成員

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string DisplayName
        {
            get { return "教師管理"; }
        }

        /// <summary>
        /// 取得所有教師名稱
        /// </summary>
        /// <returns></returns>
        public List<string> SelectKeys()
        {
            DataTable table = mQueryHelper.Select("select teacher_name,nickname from $scheduler.teacher_ex");

            List<string> Result = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string Name = row.Field<string>("teacher_name");
                string NickName = row.Field<string>("nickname");

                if (string.IsNullOrEmpty(NickName))
                    Result.Add(Name);
                else
                    Result.Add(Name + "(" + NickName + ")");
            }

            return Result;
        }

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="SearchText">搜尋文字</param>
        /// <returns></returns>
        public List<string> Search(string SearchText)
        {
            DataTable table = mQueryHelper.Select("select teacher_name,nickname from $scheduler.teacher_ex where teacher_name like '%" + SearchText + "%'");

            List<string> Result = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string Name = row.Field<string>("teacher_name");
                string NickName = row.Field<string>("nickname");

                if (string.IsNullOrEmpty(NickName))
                    Result.Add(Name);
                else
                    Result.Add(Name + "(" + NickName + ")");
            }

            return Result;
        }

        private Tuple<string, string> GetTeacherDetailName(string Name)
        {
            if (Name.Contains("(") && Name.EndsWith(")"))
            {
                int LeftIndex = Name.IndexOf("(");
                int RightIndex = Name.IndexOf(")");

                if (RightIndex > LeftIndex)
                {
                    string TeacherName = Name.Substring(0, LeftIndex);
                    string NickName = Name.Substring(LeftIndex + 1, RightIndex - LeftIndex - 1);

                    return new Tuple<string, string>(TeacherName, NickName);
                }
                else
                    return new Tuple<string, string>(Name, string.Empty);
            }
            else
                return new Tuple<string, string>(Name, string.Empty);
        }

        private string GetNicknameCondition(string Nickname)
        {
            if (string.IsNullOrEmpty(Nickname))
                return "nickname is null";
            else
                return "nickname='" + Nickname + "'";
        }

        public string Update(string Key, string NewKey)
        {
            #region 根據鍵值取得教師
            if (string.IsNullOrEmpty(Key))
                return "要更新的教師名稱不能為空白!";

            Tuple<string, string> KeyDetail = GetTeacherDetailName(Key);

            Tuple<string, string> NewKeyDetail = GetTeacherDetailName(NewKey);

            string strCondition = "teacher_name='" + KeyDetail.Item1 + "' and " + GetNicknameCondition(NewKeyDetail.Item2);

            List<TeacherEx> Teachers = mAccessHelper
                .Select<TeacherEx>(strCondition);

            if (Teachers.Count == 1)
            {
                Teachers[0].TeacherName = NewKeyDetail.Item1;
                Teachers[0].NickName = NewKeyDetail.Item2;
                Teachers.SaveAll();
                return string.Empty;
            }
            else
            {
                return "教師不存在或超過兩筆以上";
            }
            #endregion
        }

        /// <summary>
        /// 新增教師
        /// </summary>
        /// <param name="NewKey">新增教師名稱</param>
        /// <param name="CopyKey">要複製的教師名稱</param>
        /// <returns>傳回新增成功或失敗訊息</returns>
        public string Insert(string NewKey, string CopyKey)
        {
            #region 根據鍵值取得教師
            if (string.IsNullOrEmpty(NewKey))
                return "要新增的教師名稱不能為空白!";

            Tuple<string, string> NewKeyDetail = GetTeacherDetailName(NewKey);
            Tuple<string, string> CopyKeyDetail = GetTeacherDetailName(CopyKey);

            string strCondition = string.Empty;

            if (!string.IsNullOrEmpty(CopyKey))
                strCondition = "(teacher_name='" + NewKeyDetail.Item1 + "' and " + GetNicknameCondition(NewKeyDetail.Item2) + ") or (teacher_name='" + CopyKeyDetail.Item1 + "' and " + GetNicknameCondition(CopyKeyDetail.Item2) + ")";
            else
                strCondition = "teacher_name='" + NewKeyDetail.Item1 + "' and " + GetNicknameCondition(NewKeyDetail.Item2);

            List<TeacherEx> Teachers = mAccessHelper.Select<TeacherEx>(strCondition);

            if (Teachers.Find(x => x.FullTeacherName.Equals(NewKey)) != null)
                return "要新增的教師已存在，無法新增!";
            #endregion

            #region 新增教師
            TeacherEx NewTeacherEx = new TeacherEx();
            NewTeacherEx.TeacherName = NewKeyDetail.Item1;
            NewTeacherEx.NickName = NewKeyDetail.Item2;

            //尋找要複製的教師，若有找到的話則將Classroom內容複製過去
            TeacherEx CopyTeacher = Teachers
                .Find(x => x.FullTeacherName.Equals(CopyKey));

            if (CopyTeacher != null)
            {
                //NewTeacherEx.TeacherName = CopyTeacher.TeacherName;
                //NewTeacherEx.NickName = CopyTeacher.NickName;
            }

            List<TeacherEx> NewTeachers = new List<TeacherEx>();
            NewTeachers.Add(NewTeacherEx);

            List<string> NewTeacherIDs = mAccessHelper
                .InsertValues(NewTeachers);
            #endregion

            #region 複製教師排課時段
            List<string> NewTeacherBusyIDs = new List<string>();

            if (!string.IsNullOrEmpty(CopyKey) && NewTeacherIDs.Count == 1)
            {
                if (CopyTeacher == null)
                    return "要複製的教師不存在!";

                List<TeacherExBusy> TeacherBusys = mAccessHelper
                    .Select<TeacherExBusy>("ref_teacher_id=" + CopyTeacher.UID);

                TeacherBusys.ForEach(x => x.TeacherID = K12.Data.Int.Parse(NewTeacherIDs[0]));

                NewTeacherBusyIDs = mAccessHelper.InsertValues(TeacherBusys);
            }
            #endregion

            return "已成功新增" + NewTeacherIDs.Count + "筆教師及複製" + NewTeacherBusyIDs.Count + "筆教師不排課時段";
        }

        /// 新增教師
        /// </summary>
        /// <param name="NewKey">新增教師名稱</param>
        /// <param name="CopyKey">要複製的教師名稱</param>
        /// <param name="teachercode">教師代碼</param>
        /// <param name="teacherex">教師專長</param>
        /// <param name="note">註記</param>
        /// <returns>傳回新增成功或失敗訊息</returns>
        public string Insert_New(string NewKey, string CopyKey, string teachercode, string Expertise, string note)
        {
            #region 根據鍵值取得教師
            if (string.IsNullOrEmpty(NewKey))
                return "要新增的教師名稱不能為空白!";

            Tuple<string, string> NewKeyDetail = GetTeacherDetailName(NewKey);
            Tuple<string, string> CopyKeyDetail = GetTeacherDetailName(CopyKey);

            string strCondition = string.Empty;

            if (!string.IsNullOrEmpty(CopyKey))
                strCondition = "(teacher_name='" + NewKeyDetail.Item1 + "' and " + GetNicknameCondition(NewKeyDetail.Item2) + ") or (teacher_name='" + CopyKeyDetail.Item1 + "' and " + GetNicknameCondition(CopyKeyDetail.Item2) + ")";
            else
                strCondition = "teacher_name='" + NewKeyDetail.Item1 + "' and " + GetNicknameCondition(NewKeyDetail.Item2);

            List<TeacherEx> Teachers = mAccessHelper.Select<TeacherEx>(strCondition);

            if (Teachers.Find(x => x.FullTeacherName.Equals(NewKey)) != null)
                return "要新增的教師已存在，無法新增!";
            #endregion

            #region 新增教師
            TeacherEx NewTeacherEx = new TeacherEx();
            NewTeacherEx.TeacherName = NewKeyDetail.Item1;
            NewTeacherEx.NickName = NewKeyDetail.Item2;
            NewTeacherEx.TeacherCode = teachercode; //教師代碼
            NewTeacherEx.TeachingExpertise = Expertise; //特殊專長
            NewTeacherEx.Note = note; //註記

            //尋找要複製的教師，若有找到的話則將Classroom內容複製過去
            TeacherEx CopyTeacher = Teachers
                .Find(x => x.FullTeacherName.Equals(CopyKey));

            if (CopyTeacher != null)
            {
                //NewTeacherEx.TeacherName = CopyTeacher.TeacherName;
                //NewTeacherEx.NickName = CopyTeacher.NickName;
            }

            List<TeacherEx> NewTeachers = new List<TeacherEx>();
            NewTeachers.Add(NewTeacherEx);

            List<string> NewTeacherIDs = mAccessHelper
                .InsertValues(NewTeachers);
            #endregion

            #region 複製教師排課時段
            List<string> NewTeacherBusyIDs = new List<string>();

            if (!string.IsNullOrEmpty(CopyKey) && NewTeacherIDs.Count == 1)
            {
                if (CopyTeacher == null)
                    return "要複製的教師不存在!";

                List<TeacherExBusy> TeacherBusys = mAccessHelper
                    .Select<TeacherExBusy>("ref_teacher_id=" + CopyTeacher.UID);

                TeacherBusys.ForEach(x => x.TeacherID = K12.Data.Int.Parse(NewTeacherIDs[0]));

                NewTeacherBusyIDs = mAccessHelper.InsertValues(TeacherBusys);
            }
            #endregion

            return "已成功新增" + NewTeacherIDs.Count + "筆教師及複製" + NewTeacherBusyIDs.Count + "筆教師不排課時段";
        }

        /// <summary>
        /// 根據鍵值刪除教師及教師不排課時段
        /// </summary>
        /// <param name="Key">教師名稱</param>
        /// <returns>成功或失敗的訊息</returns>
        public string Delete(string Key)
        {
            Tuple<string, string> KeyDetail = GetTeacherDetailName(Key);

            //取得教師
            List<TeacherEx> vTeachers = mAccessHelper
                .Select<TeacherEx>("teacher_name='" + KeyDetail.Item1 + "' and " + GetNicknameCondition(KeyDetail.Item2));

            if (vTeachers.Count == 0)
            {
                return "找不到對應的教師，無法刪除!";
            }
            else
            {
                TeacherEx vTeacher = vTeachers[0];

                #region 取得教師不排課時段
                List<TeacherExBusy> vTeacherBusys = mAccessHelper.Select<TeacherExBusy>("ref_teacher_id=" + vTeacher.UID);
                #endregion

                #region 刪除教師及教師不排課時段
                List<TeacherEx> list = new List<TeacherEx>();
                list.Add(vTeacher);
                mAccessHelper.DeletedValues(list);

                if (vTeacherBusys.Count > 0)
                    mAccessHelper.DeletedValues(vTeacherBusys);
                #endregion

                return "已刪除教師「" + vTeacher.FullTeacherName + "」及「" + vTeacherBusys.Count + "」筆教師不排課時段!";
            }
        }

        /// <summary>
        /// 根據教師名稱取得教師及教師不排課時段
        /// </summary>
        /// <param name="Key">教師名稱</param>
        /// <returns>成功或失敗的訊息</returns>
        public TeacherPackage Select(string Key)
        {
            Tuple<string, string> KeyDetail = GetTeacherDetailName(Key);

            #region 產生預設的TeacherPackage，將Teacher為null，並將TeacherBusys產生為空集合
            TeacherPackage vTeacherPackage = new TeacherPackage();
            vTeacherPackage.Teacher = null;
            vTeacherPackage.TeacherBusys = new List<TeacherExBusy>();
            #endregion

            #region 根據鍵值取得時間表
            List<TeacherEx> vTeachers = mAccessHelper
                .Select<TeacherEx>("teacher_name='" + KeyDetail.Item1 + "' and " + GetNicknameCondition(KeyDetail.Item2));

            //若有教師，則設定教師，並再取得教師不排課時段
            if (vTeachers.Count == 1)
            {
                TeacherEx vTeacher = vTeachers[0];
                vTeacherPackage.Teacher = vTeacher;
                List<TeacherExBusy> vTeacherBusys = mAccessHelper
                    .Select<TeacherExBusy>("ref_teacher_id=" + vTeacher.UID);
                vTeacherPackage.TeacherBusys = vTeacherBusys;
            }
            #endregion

            return vTeacherPackage;
        }

        /// <summary>
        /// 根據TeacherPackage物件更新
        /// </summary>
        /// <param name="Value">TeacherPackage</param>
        /// <returns>成功或失敗的訊息</returns>
        public string Save(TeacherPackage Value)
        {
            StringBuilder strBuilder = new StringBuilder();

            //若Teacher不為null，且TeacherBusys不為空集合
            if (Value.Teacher != null)
            {
                List<TeacherEx> vClassrooms = new List<TeacherEx>();
                vClassrooms.Add(Value.Teacher);
                mAccessHelper.UpdateValues(vClassrooms);
                strBuilder.AppendLine("已成功更新教師『" + Value.Teacher.FullTeacherName + "』");
            }

            #region 將現有教師不排課時段刪除
            List<TeacherExBusy> Busys = mAccessHelper
                .Select<TeacherExBusy>("ref_teacher_id=" + Value.Teacher.UID);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Busys))
            {
                Busys.ForEach(x => x.Deleted = true);
                Busys.SaveAll();
            }
            #endregion

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Value.TeacherBusys))
            {
                mAccessHelper.SaveAll(Value.TeacherBusys);
                strBuilder.AppendLine("已成功更新教師不排課時段共" + Value.TeacherBusys.Count + "筆");
            }

            if (strBuilder.Length > 0)
                return strBuilder.ToString();

            return "教師物件為null或是教師不排課時段為空集合無法進行更新";
        }

        /// <summary>
        /// 其他指令
        /// </summary>
        public List<ICommand> ExtraCommands
        {
            get
            {
                List<ICommand> Commands = new List<ICommand>();

                Commands.Add(new ExportTeacherCommand());
                Commands.Add(new ImportTeacherCommand()); //匯入
                Commands.Add(new ExportTeacherBusyCommand());
                Commands.Add(new ImportTeacherBusyCommand());
                Commands.Add(new GetischoolTeacherList());
                Commands.Add(new DeleteTeacherBusyCommand());

                return Commands;
            }
        }
        #endregion
    }
}