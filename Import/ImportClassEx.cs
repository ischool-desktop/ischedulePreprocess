using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Import;
using Campus.DocumentValidator;
using System.Data;

namespace Sunset
{
    /// <summary>
    /// 匯入班級
    /// </summary>
    public class ImportClassEx : ImportWizard
    {

        private const string constClassName = "班級名稱";
        private const string constGradeYear = "班級年級";
        private const string constClassCode = "班級代碼";
        private const string constNote = "註記";
        private ImportOption mOption;
        Dictionary<string, ClassEx> ClassNameDic { get; set; }

        public override string Import(List<Campus.DocumentValidator.IRowStream> Rows)
        {
            if (mOption.SelectedKeyFields.Count == 1 && mOption.SelectedKeyFields.Contains(constClassName))
            {
                List<ClassEx> InsertList = new List<ClassEx>();
                List<ClassEx> UpdateList = new List<ClassEx>();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("匯入排課用班級資料：");
                foreach (IRowStream Row in Rows)
                {
                    //班級名稱
                    string ClassName = Row.GetValue(constClassName);
                    //班級年級
                    int GradeYear = int.Parse("" + Row.GetValue(constGradeYear));

                    string ClassCode = Row.GetValue(constClassCode);

                    string ClassNote = Row.GetValue(constNote);

                    if (!ClassNameDic.ContainsKey(ClassName))
                    {
                        //新增班級
                        ClassEx ex = new ClassEx();
                        ex.ClassName = ClassName;
                        ex.GradeYear = GradeYear;
                        ex.ClassCode = ClassCode;
                        ex.Note = ClassNote;
                        InsertList.Add(ex);
                    }
                    else
                    {
                        //更新班級
                        ClassEx ex = ClassNameDic[ClassName];
                        if (ex.GradeYear.HasValue)
                        {
                            if (ex.GradeYear.Value != GradeYear)
                            {
                                ex.GradeYear = GradeYear; //更新年級
                                UpdateList.Add(ex);
                            }
                        }
                        ex.ClassCode = ClassCode;
                        ex.Note = ClassNote;
                    }
                }
                if (InsertList.Count != 0)
                {
                    sb.AppendLine("新增清單：");
                    foreach (ClassEx each in InsertList)
                    {
                        sb.AppendLine(string.Format("班級名稱「{0}」班級年級「{1}」班級代碼「{2}」註記「{3}」", each.ClassName, each.GradeYear, each.ClassCode, each.Note));
                    }

                    tool._A.InsertValues(InsertList);
                }

                if (UpdateList.Count != 0)
                {
                    sb.AppendLine("修改清單：");
                    foreach (ClassEx each in InsertList)
                    {
                        sb.AppendLine(string.Format("班級「{0}」年級修改為「{1}」代碼修改為「{2}」註記修改為「{3}」", each.ClassName, each.GradeYear, each.ClassCode, each.Note));
                    }
                    tool._A.UpdateValues(UpdateList);
                }
                FISCA.LogAgent.ApplicationLog.Log("排課", "匯入排課班級", sb.ToString());
            }

            return "";
        }

        /// <summary>
        /// 相同名稱則進行更新
        /// (一般只能更新年級吧...)
        /// </summary>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate;
        }

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        public override string GetValidateRule()
        {
            return Properties.Resources.ClassEx;
        }

        public override void Prepare(ImportOption Option)
        {
            mOption = Option;

            //取得目前系統內的班級清單
            //驗證規則定義是：重覆名稱會協助更新年級資料
            ClassNameDic = new Dictionary<string, ClassEx>();

            List<ClassEx> list = tool._A.Select<ClassEx>();
            foreach (ClassEx row in list)
            {
                string className = row.ClassName;
                if (!ClassNameDic.ContainsKey(className))
                {
                    ClassNameDic.Add(className, row);
                }
            }
        }
    }
}