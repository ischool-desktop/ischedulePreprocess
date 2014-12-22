using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Windows;
using FISCA.UDT;
using System.Data;
using FISCA.Data;

namespace Sunset
{
    /// <summary>
    /// 班級資料存取
    /// </summary>
    public class ClassPackageDataAccess : IConfigurationDataAccess<ClassPackage>
    {
        private AccessHelper mAccessHelper;
        private QueryHelper mQueryHelper;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassPackageDataAccess()
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
            get { return "班級管理"; }
        }

        /// <summary>
        /// 取得所有教師名稱
        /// </summary>
        /// <returns></returns>
        public List<string> SelectKeys()
        {
            string strSQL = "select class_name from $scheduler.class_ex order by grade_year,class_name";

            DataTable table = mQueryHelper.Select(strSQL);

            List<string> Result = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string Name = row.Field<string>("class_name");
                Result.Add(Name);
            }

            return Result;
        }

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="SearchText"></param>
        /// <returns></returns>
        public List<string> Search(string SearchText)
        {
            string strSQL = "select class_name from $scheduler.class_ex where class_name like '%" + SearchText + "%'";

            int GradeYear;

            if (int.TryParse(SearchText, out GradeYear))
                strSQL += " or grade_year=" + GradeYear;

            strSQL += " order by grade_year,class_name";

            DataTable table = mQueryHelper.Select(strSQL);

            List<string> Result = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string Name = row.Field<string>("class_name");
                Result.Add(Name);
            }

            return Result;
        }

        public string Update(string Key, string NewKey)
        {
            #region 根據鍵值取得班級
            if (string.IsNullOrEmpty(Key))
                return "要更新的班級表名稱不能為空白!";

            string strCondition = "class_name='" + Key + "'";

            List<ClassEx> Classes = mAccessHelper.Select<ClassEx>(strCondition);

            if (Classes.Count == 1)
            {
                Classes[0].ClassName = NewKey;
                Classes.SaveAll();
                return string.Empty;
            }
            else
            {
                return "班級不存在或超過兩筆以上";
            }
            #endregion
        }

        /// <summary>
        /// 新增班級
        /// </summary>
        /// <param name="NewKey">新增班級名稱</param>
        /// <param name="CopyKey">要複製的班級名稱</param>
        /// <returns>傳回新增成功或失敗訊息</returns>
        public string Insert(string NewKey, string CopyKey)
        {
            #region 根據鍵值取得班級
            if (string.IsNullOrEmpty(NewKey))
                return "要新增的班級名稱不能為空白!";

            string strCondition = string.Empty;

            string[] NewKeys = NewKey.Split(new char[] { ',' });

            string ClassName = NewKeys[0];
            string GradeYear = NewKeys[1];

            if (!string.IsNullOrEmpty(CopyKey))
                strCondition = "class_name in ('" + ClassName + "','" + CopyKey + "')";
            else
                strCondition = "class_name in ('" + ClassName + "')";

            List<ClassEx> Classes = mAccessHelper
                .Select<ClassEx>(strCondition);

            if (Classes.Find(x => x.ClassName.Equals(ClassName)) != null)
                return "要新增的班級已存在，無法新增!";
            #endregion

            #region 新增班級
            ClassEx NewClass = new ClassEx();
            NewClass.ClassName = ClassName;
            NewClass.GradeYear = K12.Data.Int.ParseAllowNull(GradeYear);
            //尋找要複製的班級，若有找到的話則將Classroom內容複製過去
            ClassEx CopyClass = Classes
                .Find(x => x.ClassName.Equals(CopyKey));

            //if (CopyClass != null)
            //{
            //    NewClass.GradeYear = CopyClass.GradeYear;
            //}

            List<ClassEx> NewClasses = new List<ClassEx>();
            NewClasses.Add(NewClass);

            List<string> NewClassIDs = mAccessHelper.InsertValues(NewClasses);
            #endregion

            #region 複製班級不排課時段
            List<string> NewClassBusyIDs = new List<string>();

            if (!string.IsNullOrEmpty(CopyKey) && NewClassIDs.Count == 1)
            {
                if (CopyClass == null)
                    return "要複製的班級不存在!";

                List<ClassExBusy> TeacherBusys = mAccessHelper
                    .Select<ClassExBusy>("ref_class_id=" + CopyClass.UID);

                TeacherBusys.ForEach(x => x.ClassID = K12.Data.Int.Parse(NewClassIDs[0]));

                NewClassBusyIDs = mAccessHelper.InsertValues(TeacherBusys);
            }
            #endregion

            return "已成功新增" + NewClassIDs.Count + "筆班級及複製" + NewClassBusyIDs.Count + "筆班級不排課時段";
        }

        /// <summary>
        /// 新增班級
        /// </summary>
        /// <param name="NewKey">新增班級名稱</param>
        /// <param name="CopyKey">要複製的班級名稱</param>
        /// <returns>傳回新增成功或失敗訊息</returns>
        public string Insert_new(string NewKey, string CopyKey, string ClassCode,string Note)
        {
            #region 根據鍵值取得班級
            if (string.IsNullOrEmpty(NewKey))
                return "要新增的班級名稱不能為空白!";

            string strCondition = string.Empty;

            string[] NewKeys = NewKey.Split(new char[] { ',' });

            string ClassName = NewKeys[0];
            string GradeYear = NewKeys[1];

            if (!string.IsNullOrEmpty(CopyKey))
                strCondition = "class_name in ('" + ClassName + "','" + CopyKey + "')";
            else
                strCondition = "class_name in ('" + ClassName + "')";

            List<ClassEx> Classes = mAccessHelper
                .Select<ClassEx>(strCondition);

            if (Classes.Find(x => x.ClassName.Equals(ClassName)) != null)
                return "要新增的班級已存在，無法新增!";
            #endregion

            #region 新增班級
            ClassEx NewClass = new ClassEx();
            NewClass.ClassName = ClassName;
            NewClass.GradeYear = K12.Data.Int.ParseAllowNull(GradeYear);
            NewClass.ClassCode = ClassCode;
            NewClass.Note = Note;
            //尋找要複製的班級，若有找到的話則將Classroom內容複製過去
            ClassEx CopyClass = Classes
                .Find(x => x.ClassName.Equals(CopyKey));

            //if (CopyClass != null)
            //{
            //    NewClass.GradeYear = CopyClass.GradeYear;
            //}

            List<ClassEx> NewClasses = new List<ClassEx>();
            NewClasses.Add(NewClass);

            List<string> NewClassIDs = mAccessHelper.InsertValues(NewClasses);
            #endregion

            #region 複製班級不排課時段
            List<string> NewClassBusyIDs = new List<string>();

            if (!string.IsNullOrEmpty(CopyKey) && NewClassIDs.Count == 1)
            {
                if (CopyClass == null)
                    return "要複製的班級不存在!";

                List<ClassExBusy> ClassBusys = mAccessHelper
                    .Select<ClassExBusy>("ref_class_id=" + CopyClass.UID);

                ClassBusys.ForEach(x => x.ClassID = K12.Data.Int.Parse(NewClassIDs[0]));

                NewClassBusyIDs = mAccessHelper.InsertValues(ClassBusys);
            }
            #endregion

            return "已成功新增" + NewClassIDs.Count + "筆班級及複製" + NewClassBusyIDs.Count + "筆班級不排課時段";
        }

        /// <summary>
        /// 根據鍵值刪除班級及班級不排課時段
        /// </summary>
        /// <param name="Key">班級名稱</param>
        /// <returns>成功或失敗的訊息</returns>
        public string Delete(string Key)
        {
            //取得班級
            List<ClassEx> vClasses = mAccessHelper
                .Select<ClassEx>("class_name='" + Key + "'");

            if (vClasses.Count == 0)
            {
                return "找不到對應的班級，無法刪除!";
            }
            else
            {

                ClassEx vClass = vClasses[0];

                #region 取得班級不排課時段
                List<ClassExBusy> vClassBusys = mAccessHelper.Select<ClassExBusy>("ref_class_id=" + vClass.UID);
                #endregion

                #region 刪除場地及場地不排課時段
                List<ClassEx> list = new List<ClassEx>();
                list.Add(vClass);
                mAccessHelper.DeletedValues(list);

                if (vClassBusys.Count > 0)
                    mAccessHelper.DeletedValues(vClassBusys);
                #endregion

                return "已刪除班級「" + vClass.ClassName + "」及「" + vClassBusys.Count + "」筆班級不排課時段!";
            }
        }

        /// <summary>
        /// 根據班級名稱取得班級及班級不排課時段
        /// </summary>
        /// <param name="Key">班級名稱</param>
        /// <returns>成功或失敗的訊息</returns>
        public ClassPackage Select(string Key)
        {
            #region 產生預設的ClassPackage，將Class為null，並將ClassBusys產生為空集合
            ClassPackage vClassPackage = new ClassPackage();
            vClassPackage.Class = null;
            vClassPackage.ClassBusys = new List<ClassExBusy>();
            #endregion

            #region 根據鍵值取得時間表
            List<ClassEx> vClasses = mAccessHelper
                .Select<ClassEx>("class_name='" + Key + "'");

            //若有班級，則設定班級，並再取得班級不排課時段
            if (vClasses.Count == 1)
            {
                ClassEx vClass = vClasses[0];
                vClassPackage.Class = vClass;
                List<ClassExBusy> vClassBusys = mAccessHelper
                    .Select<ClassExBusy>("ref_class_id=" + vClass.UID);
                vClassPackage.ClassBusys = vClassBusys;
            }
            #endregion

            return vClassPackage;
        }

        /// <summary>
        /// 根據ClassPackage物件更新
        /// </summary>
        /// <param name="Value">ClassPackage</param>
        /// <returns>成功或失敗的訊息</returns>
        public string Save(ClassPackage Value)
        {
            StringBuilder strBuilder = new StringBuilder();

            #region 更新班級資訊
            if (Value.Class != null)
            {
                List<ClassEx> vClasses = new List<ClassEx>();
                vClasses.Add(Value.Class);
                mAccessHelper.UpdateValues(vClasses);
                strBuilder.AppendLine("已成功更新班級『" + Value.Class.ClassName + "』");
            }
            #endregion

            #region 將現有班級不排課時段刪除
            List<ClassExBusy> Busys = mAccessHelper
                .Select<ClassExBusy>("ref_class_id=" + Value.Class.UID);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Busys))
            {
                Busys.ForEach(x => x.Deleted = true);
                Busys.SaveAll();
            }
            #endregion

            #region 新增新的班級不排課時段
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Value.ClassBusys))
            {
                mAccessHelper.SaveAll(Value.ClassBusys);
                strBuilder.AppendLine("已成功更新班級不排課時段共" + Value.ClassBusys.Count + "筆");
            }
            #endregion

            if (strBuilder.Length > 0)
                return strBuilder.ToString();

            return "班級物件為null或是班級不排課時段為空集合無法進行更新";
        }

        /// <summary>
        /// 其他指令
        /// </summary>
        public List<ICommand> ExtraCommands
        {
            get
            {
                List<ICommand> Commands = new List<ICommand>();

                Commands.Add(new ExportClassCommand());
                Commands.Add(new ImportClassCommand());
                Commands.Add(new ExportClassBusyCommand());
                Commands.Add(new ImportClassBusyCommand());
                Commands.Add(new GetischoolClassList());
                Commands.Add(new DeleteClassBusyCommand());

                return Commands;
            }
        }
        #endregion
    }
}