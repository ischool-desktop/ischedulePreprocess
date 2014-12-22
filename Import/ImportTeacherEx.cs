using System.Collections.Generic;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;

namespace Sunset
{
    public class ImportTeacherEx : ImportWizard
    {
        private const string constTeacherName = "教師姓名";
        private const string constNickname = "教師暱稱";
        private const string constTeacherCode = "教師代碼";
        private const string constTeachingExpertise = "教學專長";
        private const string constNote = "註記";

        private ImportOption mOption;
        Dictionary<string, TeacherEx> TeacherNameDic { get; set; }

        public override string Import(List<Campus.DocumentValidator.IRowStream> Rows)
        {
            List<TeacherEx> InsertList = new List<TeacherEx>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("匯入排課用教師資料：");

            foreach (IRowStream Row in Rows)
            {
                //教師名稱
                string TeacherName = Row.GetValue(constTeacherName);
                //暱稱
                string Nickname = Row.GetValue(constNickname);

                //暱稱
                string TeacherCode = Row.GetValue(constTeacherCode);

                //暱稱
                string TeachingExpertise = Row.GetValue(constTeachingExpertise);

                //暱稱
                string Note = Row.GetValue(constNote);

                //新增班級
                TeacherEx ex = new TeacherEx();
                ex.TeacherName = TeacherName;
                ex.NickName = Nickname;
                ex.TeacherCode = TeacherCode;
                ex.TeachingExpertise = TeachingExpertise;
                ex.Note = Note;
                InsertList.Add(ex);

            }

            if (InsertList.Count != 0)
            {
                sb.AppendLine("新增清單：");
                foreach (TeacherEx each in InsertList)
                {
                    sb.AppendLine(string.Format("教師姓名「{0}」教師暱稱「{1}」教師代碼「{2}」教師專長「{3}」註記「{4}」", each.TeacherName, each.NickName, each.TeacherCode, each.TeachingExpertise, each.Note));
                }

                tool._A.InsertValues(InsertList);

                FISCA.LogAgent.ApplicationLog.Log("排課", "匯入排課教師", sb.ToString());
            }

            return "";
        }

        public override ImportAction GetSupportActions()
        {
            return ImportAction.Insert;
        }

        public override string GetValidateRule()
        {
            return Properties.Resources.TeacherEx;
        }

        public override void Prepare(ImportOption Option)
        {
            mOption = Option;

            //取得目前系統內的教師全名清單
            //驗證規則定義是：重覆名稱會協助更新暱稱
            TeacherNameDic = new Dictionary<string, TeacherEx>();

            List<TeacherEx> list = tool._A.Select<TeacherEx>();
            foreach (TeacherEx row in list)
            {
                string FullTeacherName = row.FullTeacherName;
                if (!TeacherNameDic.ContainsKey(FullTeacherName))
                {
                    TeacherNameDic.Add(FullTeacherName, row);
                }
            }
        }
    }
}