using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FISCA.Data;
using FISCA.UDT;
using Sunset.Windows;

namespace Sunset
{
    /// <summary>
    /// 場地資料存取
    /// </summary>
    public class ClassroomPackageDataAccess : IConfigurationDataAccess<ClassroomPackage>
    {
        private AccessHelper mAccessHelper;
        private QueryHelper mQueryHelper;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassroomPackageDataAccess()
        {
            mAccessHelper = new AccessHelper();
            mQueryHelper = new QueryHelper();
        }

        #region IConfigurationDataAccess<ClassroomPackage> 成員

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string DisplayName
        {
            get { return "場地管理"; }
        } 

        /// <summary>
        /// 取得所有場地名稱
        /// </summary>
        /// <returns></returns>
        public List<string> SelectKeys()
        {
            DataTable table = mQueryHelper.Select("select name from $scheduler.classroom");

            List<string> Result = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string Name = row.Field<string>("name");
                Result.Add(Name);
            }

            return Result;
        }

        public List<string> Search(string SearchText)
        {
            DataTable table = mQueryHelper.Select("select name from $scheduler.classroom where name like '%" + SearchText + "%'");

            List<string> Result = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string Name = row.Field<string>("name");
                Result.Add(Name);
            }

            return Result;
        }

        /// <summary>
        /// 更新鍵值
        /// </summary>
        /// <param name="Key">原鍵值</param>
        /// <param name="NewKey">新鍵值</param>
        /// <returns></returns>
        public string Update(string Key, string NewKey)
        {
            #region 根據鍵值取得場地
            if (string.IsNullOrEmpty(Key))
                return "要更新的場地表名稱不能為空白!";

            string strCondition = "name='" + Key + "'";

            List<Classroom> Classrooms = mAccessHelper
                .Select<Classroom>(strCondition);

            if (Classrooms.Count == 1)
            {
                Classrooms[0].ClassroomName = NewKey;
                Classrooms.SaveAll();
                return string.Empty;
            }
            else
            {
                return "場地不存在或超過兩筆以上";
            }
            #endregion
        }

        /// <summary>
        /// 新增場地
        /// </summary>
        /// <param name="NewKey">新增場地名稱</param>
        /// <param name="CopyKey">要複製的場地名稱</param>
        /// <returns>傳回新增成功或失敗訊息</returns>
        public string Insert(string NewKey, string CopyKey)
        {
            #region 根據鍵值取得場地
            if (string.IsNullOrEmpty(NewKey))
                return "要新增的場地表名稱不能為空白!";

            string strCondition = string.Empty;

            if (!string.IsNullOrEmpty(CopyKey))
                strCondition = "name in ('" + NewKey + "','" + CopyKey + "')";
            else
                strCondition = "name in ('" + NewKey + "')";

            List<Classroom> Classrooms = mAccessHelper.Select<Classroom>(strCondition);

            if (Classrooms.Find(x => x.ClassroomName.Equals(NewKey)) != null)
                return "要新增的場地已存在，無法新增!";
            #endregion

            #region 新增場地
            Classroom NewClassroom = new Classroom();
            NewClassroom.ClassroomName = NewKey;
            NewClassroom.Capacity = 1;
            NewClassroom.ClassroomCode = string.Empty;
            NewClassroom.ClassroomDesc = string.Empty;
            NewClassroom.LocationOnly = false;

            //尋找要複製的場地，若有找到的話則將Classroom內容複製過去
            Classroom CopyClassroom = Classrooms.Find(x => x.ClassroomName.Equals(CopyKey));

            if (CopyClassroom != null)
            {
                NewClassroom.ClassroomDesc = CopyClassroom.ClassroomDesc;
                NewClassroom.ClassroomCode = CopyClassroom.ClassroomCode;
                NewClassroom.Capacity = CopyClassroom.Capacity;
                NewClassroom.LocationID = CopyClassroom.LocationID;
                NewClassroom.LocationOnly = CopyClassroom.LocationOnly;
            }

            List<Classroom> NewClassrooms = new List<Classroom>();
            NewClassrooms.Add(NewClassroom);

            List<string> NewClassroomIDs = mAccessHelper.InsertValues(NewClassrooms);
            #endregion

            #region 複製場地不排課時段
            List<string> NewClassroomBusyIDs = new List<string>();

            if (!string.IsNullOrEmpty(CopyKey) && NewClassroomIDs.Count == 1)
            {
                if (CopyClassroom == null)
                    return "要複製的場地不存在!";

                List<ClassroomBusy> ClassroomBusys = mAccessHelper.Select<ClassroomBusy>("ref_classroom_id=" + CopyClassroom.UID);

                ClassroomBusys.ForEach(x => x.ClassroomID = K12.Data.Int.Parse(NewClassroomIDs[0]));

                NewClassroomBusyIDs = mAccessHelper.InsertValues(ClassroomBusys);
            }
            #endregion

            return "已成功新增" + NewClassroomIDs.Count + "筆場地及複製" + NewClassroomBusyIDs.Count + "筆場地不排課時段";
        }

        /// <summary>
        /// 根據鍵值刪除場地及場地不排課時段
        /// </summary>
        /// <param name="Key">場地名稱</param>
        /// <returns>成功或失敗的訊息</returns>
        public string Delete(string Key)
        {
            #region 取得場地
            List<Classroom> vClassrooms = mAccessHelper.Select<Classroom>("name='" + Key + "'");

            if (vClassrooms.Count == 0)
                return "找不到對應的場地，無法刪除!";
            if (vClassrooms.Count > 1)
                return "根據場地名稱" + Key + "找到兩筆以上的場地，不知道要刪除哪筆!";

            Classroom vClassroom = vClassrooms[0];
            #endregion

            #region 取得場地不排課時段
            List<ClassroomBusy> vClassroomBusys = mAccessHelper.Select<ClassroomBusy>("ref_classroom_id=" + vClassroom.UID);
            #endregion

            #region 刪除場地及場地不排課時段
            mAccessHelper.DeletedValues(vClassrooms);

            if (vClassroomBusys.Count > 0)
                mAccessHelper.DeletedValues(vClassroomBusys);
            #endregion

            return "已刪除場地『" + vClassroom.ClassroomName + "』及" + vClassroomBusys.Count + "筆場地不排課時段!";
        }

        /// <summary>
        /// 根據場地名稱取得場地及場地不排課時段
        /// </summary>
        /// <param name="Key">場地名稱</param>
        /// <returns>成功或失敗的訊息</returns>
        public ClassroomPackage Select(string Key)
        {
            #region 產生預設的ClassroomPackage，將Classroom設為null，並將ClassroomBusys產生為空集合
            ClassroomPackage vClassroomPackage = new ClassroomPackage();
            vClassroomPackage.Classroom = null;
            vClassroomPackage.ClassroomBusys = new List<ClassroomBusy>();
            #endregion

            #region 根據鍵值取得時間表
            List<Classroom> vClassrooms = mAccessHelper.Select<Classroom>("name='" + Key + "'");

            //若有場地，則設定場地，並再取得場地不排課時段
            if (vClassrooms.Count == 1)
            {
                Classroom  vClassroom = vClassrooms[0];
                vClassroomPackage.Classroom = vClassroom;
                List<ClassroomBusy> vClassroomBusys = mAccessHelper.Select<ClassroomBusy>("ref_classroom_id=" + vClassroom.UID);
                vClassroomPackage.ClassroomBusys = vClassroomBusys;
            }
            #endregion

            return vClassroomPackage;
        }

        /// <summary>
        /// 根據ClassroomPackage物件更新
        /// </summary>
        /// <param name="Value">ClassroomPackage</param>
        /// <returns>成功或失敗的訊息</returns>
        public string Save(ClassroomPackage Value)
        {
            StringBuilder strBuilder = new StringBuilder();

            //若Classroom不為null，且ClassroomBusys不為空集合
            if (Value.Classroom != null)
            {
                List<Classroom> vClassrooms = new List<Classroom>();
                vClassrooms.Add(Value.Classroom);
                mAccessHelper.UpdateValues(vClassrooms);
                strBuilder.AppendLine("已成功更新場地『" + Value.Classroom.ClassroomName + "』");
            }

            #region 將現有班級不排課時段刪除
            List<ClassroomBusy> Busys = mAccessHelper
                .Select<ClassroomBusy>("ref_classroom_id=" + Value.Classroom.UID);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Busys))
            {
                Busys.ForEach(x => x.Deleted = true);
                Busys.SaveAll();
            }
            #endregion

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Value.ClassroomBusys))
            {
                mAccessHelper.SaveAll(Value.ClassroomBusys);
                strBuilder.AppendLine("已成功更新場地不排課時段共" + Value.ClassroomBusys.Count + "筆");
            }

            if (strBuilder.Length > 0)
                return strBuilder.ToString();

            return "場地物件為null或是場地不排課時段為空集合無法進行更新";
        }

        /// <summary>
        /// 其他指令
        /// </summary>
        public List<ICommand> ExtraCommands
        {
            get
            {
                List<ICommand> Commands = new List<ICommand>();

                Commands.Add(new ExportClassroomCommand());
                Commands.Add(new ImportClassroomCommand());
                Commands.Add(new ExportClassroomBusyCommand());
                Commands.Add(new ImportClassroomBusyCommand());

                return Commands;
            }
        }
        #endregion
    }
}