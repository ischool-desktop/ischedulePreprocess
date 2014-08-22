using System;
using System.Collections.Generic;
using Campus.Import;
using Campus.DocumentValidator;

namespace Sunset
{
    /// <summary>
    /// 匯入測試
    /// </summary>
    public class ImportTest : ImportWizard
    {
        private int UpdateCount;
        private int InsertCount;

        /// <summary>
        /// 建構式
        /// </summary>
        public ImportTest()
        {
            InsertCount = 0;
            UpdateCount = 0;

            this.IsSplit = false;

            //this.Complete = (Message) => Message = "成功新增"+InsertCount+"筆，及成功更新"+UpdateCount"筆";

            this.Complete = ()=> "匯入完成!";           

            this.CustomValidate = (Rows, Messages) =>
            {
                //Rows.ForEach(x =>
                //{
                //    Messages[x.Position].MessageItems.Add(
                //        new Campus.Validator.MessageItem(
                //            Campus.Validator.ErrorType.Error, 
                //            Campus.Validator.ValidatorType.Row, 
                //            "Test"));
                //}
                //);
            };
        }

        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/kunhsiang.com/sunset/home/yan-zheng-gui-ze/ImportTest.txt";
        }

        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate;
        }

        public override void Prepare(ImportOption Option)
        {
        }

        public override string Import(List<IRowStream> Rows)
        {

            List<IRowStream> vRows = Rows;

            this.ImportMessages[Rows[0].Position] = "test";

            throw new Exception("Exception Test");

            InsertCount += 10;
            UpdateCount += 10;

            return "匯入測試";
        }
    }
}