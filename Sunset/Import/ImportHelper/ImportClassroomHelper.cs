using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Campus.DocumentValidator;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 在來源資料中有場地名稱就自動新增或刪除，並傳回對應的場地物件
    /// </summary>
    public class ImportClassroomHelper
    {
        private AccessHelper mHelper;
        private Dictionary<string, Classroom> mClassrooms; //場地名稱對應場地物件字典

        /// <summary>
        /// 取得新增訊息
        /// </summary>
        /// <param name="Classrooms">場地物件列表</param>
        /// <returns>新增訊息</returns>
        public static string GetInsertMessage(List<Classroom> Classrooms)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(Classrooms))
                return string.Empty;

            StringBuilder strBuilder = new StringBuilder();

            Classrooms.ForEach(x => strBuilder.AppendLine("已新增『" + x.ClassroomName + "』場地"));
            strBuilder.AppendLine("共新增" + Classrooms.Count + "筆場地");

            return strBuilder.ToString();
        }

        /// <summary>
        /// 取得刪除訊息
        /// </summary>
        /// <param name="Classrooms">地點物件列表</param>
        /// <returns>刪除訊息</returns>
        public static string GetDeleteMessage(List<Classroom> Classrooms)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(Classrooms))
                return string.Empty;

            StringBuilder strBuilder = new StringBuilder();

            Classrooms.ForEach(x => strBuilder.AppendLine("已刪除『" + x.ClassroomName + "』場地"));
            strBuilder.AppendLine("共刪除" + Classrooms.Count + "筆場地");

            return strBuilder.ToString();
        }

        /// <summary>
        /// 初始化取得資料
        /// </summary>
        private void Initialize()
        {
            mClassrooms = new Dictionary<string, Classroom>();

            try
            {
                List<Classroom> vClassrooms = mHelper.Select<Classroom>();
                mClassrooms = vClassrooms.ToDictionary(x => x.ClassroomName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 建構式，會傳入預設的UDT存取來源
        /// </summary>
        /// <param name="Helper"></param>
        public ImportClassroomHelper(AccessHelper Helper)
        {
            mHelper = Helper;
            Initialize();
        }

        /// <summary>
        /// 無參數建構式，會自動建立UDT存取來源
        /// </summary>
        public ImportClassroomHelper()
        {
            mHelper = new AccessHelper();
            Initialize();
        }

        /// <summary>
        /// 根據場地名稱取得場地物件，若不存在會回傳null。
        /// </summary>
        /// <param name="ClassroomName">場地名稱</param>
        /// <returns>場地物件</returns>
        public Classroom this[string ClassroomName]
        {
            get
            {
                return mClassrooms.ContainsKey(ClassroomName) ? mClassrooms[ClassroomName] : null;
            }
        }

        /// <summary>
        /// 根據IRowStream刪除場地，預設的ClassroomName為『場地名稱』
        /// </summary>
        /// <param name="Rows">IRowStream場地列表</param>
        /// <returns>刪除的場地物件列表</returns>
        public List<Classroom> Delete(List<IRowStream> Rows)
        {
            return Delete(Rows, "場地名稱");
        }

        /// <summary>
        /// 根據IRowStream刪除場地
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <param name="ClassroomNameField">ClassroomName的匯入欄位名稱</param>
        /// <returns>刪除的場地物件列表</returns>
        public List<Classroom> Delete(List<IRowStream> Rows, string ClassroomNameField)
        {
            if (!string.IsNullOrEmpty(ClassroomNameField) && Rows.Count > 0 && Rows[0].Contains(ClassroomNameField))
            {
                List<Classroom> DeleteClassrooms = new List<Classroom>();

                foreach (IRowStream Row in Rows)
                {
                    //判斷來源資料是否有包含地點欄位，若有的話才取值，否則傳回空白
                    string ClassroomName = Row.Contains(ClassroomNameField) ? Row.GetValue(ClassroomNameField) : string.Empty;

                    //若地點名稱不為空白，且現有記錄有包含，則加入到刪除的清單中
                    if (!string.IsNullOrEmpty(ClassroomName) && mClassrooms.ContainsKey(ClassroomName))
                        DeleteClassrooms.Add(mClassrooms[ClassroomName]);
                }

                mHelper.DeletedValues(DeleteClassrooms);

                DeleteClassrooms.ForEach(x =>
                    {
                        if (mClassrooms.ContainsKey(x.ClassroomName))
                            mClassrooms.Remove(x.ClassroomName);
                    }
                    );

                return DeleteClassrooms;
            }

            return new List<Classroom>();
        }

        /// <summary>
        /// 根據IRowStream新增場地，預設的ClassroomName為『場地名稱』
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <returns>新增的場地物件列表</returns>
        public List<Classroom> Insert(List<IRowStream> Rows)
        {
            return Insert(Rows, "場地名稱");
        }

        /// <summary>
        /// 根據IRowStream新增場地
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <param name="ClassroomNameField">ClassroomName的匯入欄位名稱</param>
        /// <returns>新增的場地物件列表</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Classroom> Insert(List<IRowStream> Rows, string ClassroomNameField)
        {
            //若來源筆數大於0，且來源資料有包含地點名稱才繼續
            if (!string.IsNullOrEmpty(ClassroomNameField) && Rows.Count > 0 && Rows[0].Contains(ClassroomNameField))
            {
                //要新增的地點集合
                List<Classroom> InsertClassrooms = new List<Classroom>();
                List<string> ClassroomNames = new List<string>();

                //針對每筆記錄
                foreach (IRowStream Row in Rows)
                {
                    //判斷來源資料是否有包含地點欄位，若有的話才取值，否則傳回空白
                    string ClassroomName = Row.Contains(ClassroomNameField) ? Row.GetValue(ClassroomNameField) : string.Empty;

                    //若地點名稱不為空白，且現有記錄中未包含此地點名稱，則建立新的物件
                    if (!string.IsNullOrEmpty(ClassroomName) && !mClassrooms.ContainsKey(ClassroomName))
                        if (!ClassroomNames.Contains("'" + ClassroomName + "'"))
                        {
                            Classroom vClassroom = new Classroom();

                            vClassroom.ClassroomName = ClassroomName;
                            vClassroom.ClassroomCode = ClassroomName;
                            vClassroom.Capacity = 1;
                            vClassroom.ClassroomDesc = "此場地為在匯入排課課程資料時自動建立。";
                            vClassroom.LocationID = null;
                            vClassroom.LocationOnly = false;

                            InsertClassrooms.Add(vClassroom);
                            ClassroomNames.Add("'" + ClassroomName + "'");
                        }
                }

                //若有新的場地才進行新增，並將新增的資料放到集合中
                if (InsertClassrooms.Count > 0)
                {
                    mHelper.InsertValues(InsertClassrooms);

                    string strCondition = "name in (" + string.Join(",", ClassroomNames.ToArray()) + ")";

                    List<Classroom> vClassrooms = mHelper.Select<Classroom>(strCondition);

                    vClassrooms.ForEach(x =>
                    {
                        if (!mClassrooms.ContainsKey(x.ClassroomName))
                            mClassrooms.Add(x.ClassroomName, x);
                    });

                    return vClassrooms;
                }
            }

            return new List<Classroom>();
        }
    }
}