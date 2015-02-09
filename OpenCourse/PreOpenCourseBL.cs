using System;
using System.Collections.Generic;
using System.Linq;
using K12.Data;

namespace Sunset
{
    /// <summary>
    /// 課程規劃商業邏輯物件
    /// </summary>
    public class ProgramPlanBL
    {
        private List<ProgramPlanClassRecord> mProgramPlanClassRecords;

        /// <summary>
        /// 預開課程學年度
        /// </summary>
        public string SchoolYear { get; private set;}

        /// <summary>
        /// 預開課程學期
        /// </summary>
        public string Semesetr { get; private set; }

        /// <summary>
        /// 建構式，傳入學年度及學期
        /// </summary>
        /// <param name="SchoolYear"></param>
        /// <param name="Semester"></param>
        public ProgramPlanBL(string SchoolYear,string Semester)
        {
            this.SchoolYear = SchoolYear;
            this.Semesetr = Semester;
        }

        /// <summary>
        /// 取得預開年級
        /// </summary>
        /// <param name="NextSemester">預開學期</param>
        /// <param name="CurrentGradeYear">目前班級年級</param>
        /// <returns>預開年級</returns>
        private int GetNextGradeYearByNextSemesterAndCurrentGradeYear(int NextSemester,int? CurrentGradeYear)
        {
            //假設下個學期為2，而且年級有值則回傳目前年級
            if (NextSemester == 2 && CurrentGradeYear.HasValue && CurrentGradeYear.Value>=1)
                return CurrentGradeYear.Value;
            //假設下個學期為1，而且年級有值則回傳目前年級+1
            else if (NextSemester == 1 && CurrentGradeYear.HasValue && CurrentGradeYear.Value>=1)
                return CurrentGradeYear.Value + 1;
            //其他狀況回傳1年級。
            else
                return 1;
        }

        /// <summary>
        /// 取得預開班級名稱
        /// </summary>
        /// <param name="Class">班級</param>
        /// <param name="NextSemester">預開學期</param>
        /// <param name="NextGradeYear">預開年級</param>
        /// <returns>預開班級名稱</returns>
        private string GetNextClassNameByNextSemesterAndCurrentGradeYear(ProgramPlanClassRecord Class,int NextSemester, int NextGradeYear)
        {
            string NextClassName = string.Empty;

            //若預開學期為1，並且年級大於等於1，就採用班級升級後的名稱
            //if (NextSemester == 1 && Class.ClassRecord.GradeYear.HasValue && Class.ClassRecord.GradeYear>=1)

            //若預開學期為1，則採用班級升級後的名稱，否則就傳回目前班級名稱
            if (NextSemester == 1)
                NextClassName = Class.GetClassName(NextGradeYear);
            else 
                NextClassName = Class.ClassRecord.Name;

            //若是最後的結果為空白，則回傳原來的班級名稱，否則就回傳原本的名稱
            return !string.IsNullOrWhiteSpace(NextClassName) ? NextClassName : Class.ClassRecord.Name;
        }

        /// <summary>
        /// 根據班級名稱
        /// </summary>
        /// <param name="ClassNames"></param>
        /// <returns></returns>
        public Tuple<bool, string> OpenCourse(List<string> ClassNames)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(ClassNames))
                return new Tuple<bool, string>(false, "沒有選取班級！");

            return new Tuple<bool, string>(false, string.Empty);
        }

        /// <summary>
        /// 根據指定系統編號預開課程
        /// </summary>
        /// <param name="ClassIDs">班級系統編號列表</param>
        /// <returns></returns>
        public Tuple<bool,string> PreOpenCourse(List<string> ClassIDs)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(ClassIDs))
                return new Tuple<bool, string>(false,"沒有選取班級！");

            #region 取得原始班級資料物件
            Class.RemoveByIDs(ClassIDs);

            List<ClassRecord> ClassRecords = Class.SelectByIDs(ClassIDs);
            #endregion

            //檢查是否都有指定課程規劃
            Tuple<bool, string> AllHasProgramPlan = ClassRecords.HasProgramPlan();

            if (!AllHasProgramPlan.Item1)
                return AllHasProgramPlan;

            //檢查是否都有指定年級
            //Tuple<bool, string> AllHasGradeYear = ClassRecords.HasGradeYear();

            //if (!AllHasGradeYear.Item1)
            //    return AllHasGradeYear;

            //檢查是否都有指定命名規則
            Tuple<bool, string> AllHasNamingRule = ClassRecords.HasNamingRule();

            if (!AllHasNamingRule.Item1)
                return AllHasNamingRule;

            //將原始班級資料轉換為課程規劃班級物件（主要可包含多個課程規劃系統編號）
            mProgramPlanClassRecords = ClassRecords.ToProgrmPlanClass();

            //加入學生課程規劃
            mProgramPlanClassRecords.AddStudentProgramPlan();

            //填入課程規劃物件
            mProgramPlanClassRecords.FillProgramPlan();

            List<PreOpenCourseRecord> PreOpenCourseRecords = new List<PreOpenCourseRecord>();

            #region 對每個班級看要開哪些新課
            foreach (ProgramPlanClassRecord ProgramPlanClassRecord in mProgramPlanClassRecords)
            {
                //取得下個年級及學期
                int NextSemester = K12.Data.Int.Parse(Semesetr);
                //若預開課程的年級為2，則年級不變；若為1的話則為新的年級
                int NextGradeYear = GetNextGradeYearByNextSemesterAndCurrentGradeYear(NextSemester, ProgramPlanClassRecord.ClassRecord.GradeYear);
                //是否採用班級升級後的名稱
                string NextClassName = GetNextClassNameByNextSemesterAndCurrentGradeYear(ProgramPlanClassRecord, NextSemester, NextGradeYear);

                //將課程規劃科目，傳入學年度、班級系統編號及班級名稱，轉為預開課程
                #region 針對每個班級的課程規劃表
                foreach (ProgramPlanRecord ProgramPlanRecord in ProgramPlanClassRecord.ProgramPlans)
                {
                    //取得指定年級及學期的課程規劃科目
                    List<ProgramSubject> Subjects = ProgramPlanRecord.Subjects
                        .FindAll(x=>x.GradeYear.Equals(NextGradeYear) && x.Semester.Equals(NextSemester));

                    PreOpenCourseRecords
                        .AddRange(Subjects.ToPreOpenCourseClass(SchoolYear,ProgramPlanClassRecord.ClassRecord.ID,NextClassName));
                }
                #endregion
            }
            #endregion

            #region 去除重覆課程
            PreOpenCourseRecords = PreOpenCourseRecords.ToDistinct();
            #endregion

            //填入課程系統編號，沒有的填入空白
            PreOpenCourseRecords.FillCourseID();
            
            //找出尚未開的課程
            PreOpenCourseRecords = PreOpenCourseRecords
                .FindAll(x => string.IsNullOrWhiteSpace(x.CourseID));

            //實際進行開課
            Tuple<bool,string> Result = PreOpenCourseRecords.OpenCourse();

            return Result;
        }
    }
}