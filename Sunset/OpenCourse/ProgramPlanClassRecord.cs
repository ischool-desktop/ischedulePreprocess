using System;
using System.Collections.Generic;
using K12.Data;

namespace Sunset
{
    /// <summary>
    /// 課程規劃班級類別，可包含多個課程規劃系統編號
    /// </summary>
    public class ProgramPlanClassRecord
    {
        /// <summary>
        /// 班級記錄
        /// </summary>
        public ClassRecord ClassRecord { get; private set; }

        /// <summary>
        /// 根據年級取得班級名稱
        /// </summary>
        /// <param name="GradeYear"></param>
        /// <returns></returns>
        public string GetClassName(int GradeYear)
        {
            string NewClassName = ClassRecord.NamingRule.ParseClassName(GradeYear);

            return NewClassName;
        }

        /// <summary>
        /// 課程規劃系統編號
        /// </summary>
        public List<string> ProgramPalnIDs { get; private set; }

        /// <summary>
        /// 課程規劃記錄物件
        /// </summary>
        public List<ProgramPlanRecord> ProgramPlans { get; internal set;}

        /// <summary>
        /// 建構式，加入班級記錄
        /// </summary>
        /// <param name="vClassRecord"></param>
        public ProgramPlanClassRecord(ClassRecord vClassRecord)
        {
            if (vClassRecord == null)
                throw new NullReferenceException("參數vClassRecord不得為null");

            if (string.IsNullOrEmpty(vClassRecord.RefProgramPlanID))
                throw new Exception("班級課程規劃表系統編號為空白");

            ClassRecord = vClassRecord;

            ProgramPalnIDs = new List<string>();

            ProgramPalnIDs.Add(ClassRecord.RefProgramPlanID);
        }

        /// <summary>
        /// 增加課程規劃系統編號
        /// </summary>
        /// <param name="ProgramPlanID"></param>
        public void AddProgramPlanID(string ProgramPlanID)
        {
            if (!string.IsNullOrWhiteSpace(ProgramPlanID))
                if (!ProgramPalnIDs.Contains(ProgramPlanID))
                    ProgramPalnIDs.Add(ProgramPlanID);
        }
    }
}