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
    /// 在來源資料中有地點名稱就自動新增或刪除，並傳回對應的地點物件
    /// </summary>
    public class ImportLocationHelper
    {
        private AccessHelper mHelper;
        private Dictionary<string, Location> mLocations; //地點名稱對應地點物件

        /// <summary>
        /// 取得新增訊息
        /// </summary>
        /// <param name="Locations">地點物件列表</param>
        /// <returns>新增訊息</returns>
        public static string GetInsertMessage(List<Location> Locations)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(Locations))
                return string.Empty;

            StringBuilder strBuilder = new StringBuilder();

            Locations.ForEach(x => strBuilder.AppendLine("已新增『" + x.LocationName + "』地點"));
            strBuilder.AppendLine("共新增" + Locations.Count + "筆地點");

            return strBuilder.ToString();
        }

        /// <summary>
        /// 取得刪除訊息
        /// </summary>
        /// <param name="Locations">地點物件列表</param>
        /// <returns>刪除訊息</returns>
        public static string GetDeleteMessage(List<Location> Locations)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(Locations))
                return string.Empty;

            StringBuilder strBuilder = new StringBuilder();

            Locations.ForEach(x => strBuilder.AppendLine("已刪除『" + x.LocationName + "』地點"));
            strBuilder.AppendLine("共刪除" + Locations.Count + "筆地點");

            return strBuilder.ToString(); 
        }

        /// <summary>
        /// 初始化取得資料
        /// </summary>
        private void Initialize()
        {
            mLocations = new Dictionary<string, Location>();

            try
            {
                List<Location> vLocations = mHelper.Select<Location>();
                mLocations = vLocations.ToDictionary(x => x.LocationName);
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
        public ImportLocationHelper(AccessHelper Helper)
        {
            mHelper = Helper;
            Initialize();
        }

        /// <summary>
        /// 無參數建構式，會自動建立UDT存取來源
        /// </summary>
        public ImportLocationHelper()
        {
            mHelper = new AccessHelper();
            Initialize();
        }

        /// <summary>
        /// 根據地點名稱取得地點物件，若不存在會回傳null。
        /// </summary>
        /// <param name="LocationName">地點名稱</param>
        /// <returns>地點物件</returns>
        public Location this[string LocationName]
        {
            get 
            {
                return mLocations.ContainsKey(LocationName) ? mLocations[LocationName] : null; 
            }
        }

        /// <summary>
        /// 根據IRowStream刪除地點，預設的LocationName為『地點名稱』
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <returns>刪除的地點物件列表</returns>
        public List<Location> Delete(List<IRowStream> Rows)
        {
            return Delete(Rows, "地點名稱");
        }

        /// <summary>
        /// 根據IRowStream刪除地點
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <param name="LocationNameField">LocationName的匯入欄位名稱</param>
        /// <returns>刪除的地點物件列表</returns>
        public List<Location> Delete(List<IRowStream> Rows,string LocationNameField)
        {
            if (!string.IsNullOrEmpty(LocationNameField) && Rows.Count > 0 && Rows[0].Contains(LocationNameField))
            {
                List<Location> DeleteLocations = new List<Location>();

                foreach (IRowStream Row in Rows)
                {
                    //判斷來源資料是否有包含地點欄位，若有的話才取值，否則傳回空白
                    string LocationName = Row.Contains(LocationNameField) ? Row.GetValue(LocationNameField) : string.Empty;

                    //若地點名稱不為空白，且現有記錄有包含，則加入到刪除的清單中
                    if (!string.IsNullOrEmpty(LocationName) && mLocations.ContainsKey(LocationName))
                        DeleteLocations.Add(mLocations[LocationName]);
                }

                mHelper.DeletedValues(DeleteLocations);

                DeleteLocations.ForEach(x =>
                    {
                        if (mLocations.ContainsKey(x.LocationName))
                            mLocations.Remove(x.LocationName);
                    }
                );

                return DeleteLocations;
            }

            return new List<Location>();
        }

        /// <summary>
        /// 根據IRowStream新增地點，預設的LocationName為『地點名稱』
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <returns>新增的地點物件列表</returns>
        public List<Location> Insert(List<IRowStream> Rows)
        {
            return Insert(Rows, "地點名稱");
        }

        /// <summary>
        /// 根據IRowStream新增地點
        /// </summary>
        /// <param name="Rows">IRowStream物件列表</param>
        /// <param name="LocationNameField">LocatioName的匯入欄位名稱</param>
        /// <returns>新增的地點物件列表</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Location> Insert(List<IRowStream> Rows,string LocationNameField)
        {
            //若來源筆數大於0，且來源資料有包含地點名稱才繼續
            if (!string.IsNullOrEmpty(LocationNameField) && Rows.Count > 0 && Rows[0].Contains(LocationNameField))
            {
                //要新增的地點集合
                List<Location> InsertLocations = new List<Location>();
                List<string> LocationNames = new List<string>();

                //針對每筆記錄
                foreach (IRowStream Row in Rows)
                {
                    //判斷來源資料是否有包含地點欄位，若有的話才取值，否則傳回空白
                    string LocationName = Row.Contains(LocationNameField) ? Row.GetValue(LocationNameField) : string.Empty;

                    //若地點名稱不為空白，且現有記錄中未包含此地點名稱，則建立新的物件
                    if (!string.IsNullOrEmpty(LocationName) && !mLocations.ContainsKey(LocationName))
                        if (!LocationNames.Contains("'" + LocationName + "'")) //若已新增過則不再新增
                        {
                            Location vLocation = new Location();
                            vLocation.LocationName = LocationName;
                            InsertLocations.Add(vLocation);
                            LocationNames.Add("'" + LocationName + "'");
                        }
                }

                //若有新的地點才進行新增，並將新增的資料放到集合中
                if (InsertLocations.Count > 0)
                {
                    mHelper.InsertValues(InsertLocations);

                    string strCondition = "name in (" + string.Join(",", LocationNames.ToArray()) + ")";

                    List<Location> vLocations = mHelper.Select<Location>();

                    vLocations.ForEach(x => 
                    {
                        if (!mLocations.ContainsKey(x.LocationName))
                            mLocations.Add(x.LocationName, x);
                    });

                    return vLocations;
                }
            }

            return new List<Location>();
        }
    }
}