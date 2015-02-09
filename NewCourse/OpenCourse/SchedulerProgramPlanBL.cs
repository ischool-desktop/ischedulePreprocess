using System;
using System.Collections.Generic;
using K12.Data;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 課程規劃商業邏輯物件
    /// </summary>
    public class SchedulerProgramPlanBL
    {
        private List<SchedulerProgramPlanClassRecord> mProgramPlanClassRecords;

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
        public SchedulerProgramPlanBL(string SchoolYear, string Semester)
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
        /// 根據指定系統編號預開課程
        /// </summary>
        /// <param name="ProgramPlans">課程規劃列表</param>
        /// <returns></returns>
        public Tuple<bool,string> OpenCourse(List<SchedulerProgramPlanClassRecord> ProgramPlans
            ,bool IsCreateCourseSection)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(ProgramPlans))
                return new Tuple<bool, string>(false,"沒有選取班級！");

            List<SchedulerOpenCourseRecord> OpenCourseRecords = new List<SchedulerOpenCourseRecord>();

            #region 對每個班級看要開哪些新課
            foreach (SchedulerProgramPlanClassRecord ClassRecord in ProgramPlans)
            {
                //將課程規劃科目，傳入學年度、班級系統編號及班級名稱，轉為預開課程
                //取得指定年級及學期的課程規劃科目
                List<ProgramSubject> Subjects = ClassRecord.ProgramPlan.Subjects
                    .FindAll(x => 
                        (x.GradeYear.Equals(ClassRecord.GradeYear) || (x.GradeYear+6).Equals(ClassRecord.GradeYear))
                        && (""+x.Semester).Equals(Semesetr));

                OpenCourseRecords
                    .AddRange(Subjects.ToOpenCourseClass(
                    SchoolYear, ClassRecord.ClassName));
            }
            #endregion

            #region 去除重覆課程
            OpenCourseRecords = OpenCourseRecords.ToDistinct();
            #endregion

            //填入課程系統編號，沒有的填入空白
            OpenCourseRecords.FillCourseID();
            
            //找出尚未開的課程
            OpenCourseRecords = OpenCourseRecords
                .FindAll(x => string.IsNullOrWhiteSpace(x.CourseID));

            //實際進行開課
            Tuple<bool,string> Result = OpenCourseRecords.OpenCourse(IsCreateCourseSection);

            return Result;
        }
    }
}