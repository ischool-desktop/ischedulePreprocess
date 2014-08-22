using System.Collections.Generic;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 匯入地點
    /// </summary>
    public class ImportLocation : ImportWizard
    {
        private const string constLocationName = "地點名稱";
        private ImportOption mOption;
        private AccessHelper mHelper;
        private StringBuilder mstrLog = new StringBuilder();
        private ImportLocationHelper mImportLocationHelper;

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/ischool.com.tw/leader/sunset/%E5%9C%B0%E9%BB%9E.xml";
        }

        /// <summary>
        /// 取得可匯入的動作
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.Insert | ImportAction.Delete;
        }

        /// <summary>
        /// 匯入準備
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
        /// <param name="Rows">IRowStream物件列表</param>
        /// <returns>分批匯入完成訊息</returns>
        public override string Import(List<IRowStream> Rows)
        {
            mstrLog.Clear();

            if (mOption.SelectedKeyFields.Count == 1 && 
                mOption.SelectedKeyFields.Contains(constLocationName))
            {
                if (mOption.Action == ImportAction.Insert)
                {
                    List<Location> Locations = mImportLocationHelper.Insert(Rows);

                    string Message = ImportLocationHelper.GetInsertMessage(Locations);

                    if (!string.IsNullOrEmpty(Message))
                        mstrLog.AppendLine(Message);
                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    List<Location> Locations = mImportLocationHelper.Delete(Rows);

                    string Message = ImportLocationHelper.GetDeleteMessage(Locations);

                    if (!string.IsNullOrEmpty(Message))
                        mstrLog.AppendLine(Message); 
                }
            }

            return mstrLog.ToString();
        }
    }
}