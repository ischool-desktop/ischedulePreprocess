using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FISCA.Data;
using K12.Data;
using FISCA;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 課程規劃商業邏輯延伸方法
    /// </summary>
    public static class SchedulerProgramPlanBL_Extension
    {
        /// <summary>
        /// 填入現有系統編號
        /// </summary>
        /// <param name="PreOpenCourseRecords"></param>
        public static void FillCourseID(this List<SchedulerOpenCourseRecord> PreOpenCourseRecords)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(PreOpenCourseRecords))
                return;

            QueryHelper helper = new QueryHelper();

            string strSQL = "select uid,course_name,school_year,semester from $scheduler.scheduler_course_extension where " +
                string.Join(" or ", PreOpenCourseRecords.Select(x => x.CourseKeyCondition).ToArray());

            DataTable Table = helper.Select(strSQL);

            PreOpenCourseRecords.ForEach(x => x.CourseID = string.Empty);

            foreach (DataRow Row in Table.Rows)
            {
                string CourseID = Row.Field<string>("uid");
                string CourseName = Row.Field<string>("course_name");
                string SchoolYear = Row.Field<string>("school_year");
                string Semester = Row.Field<string>("semester");

                List<SchedulerOpenCourseRecord> FindeOpenCourseRecords = PreOpenCourseRecords
                    .FindAll(x =>
                        x.CourseName.Equals(CourseName) &&
                        x.SchoolYear.Equals(SchoolYear) &&
                        x.Semester.Equals(Semester));

                FindeOpenCourseRecords.ForEach(x => x.CourseID = CourseID);
            }
        }

        /// <summary>
        /// 去除重覆的預開課程
        /// </summary>
        /// <param name="PreOpenCourseRecords"></param>
        /// <returns></returns>
        public static List<SchedulerOpenCourseRecord> ToDistinct(this List<SchedulerOpenCourseRecord> OpenCourseRecords)
        {
            Dictionary<string, SchedulerOpenCourseRecord> Result = new Dictionary<string, SchedulerOpenCourseRecord>();

            foreach (SchedulerOpenCourseRecord OpenCourseRecord in OpenCourseRecords)
            {
                if (!Result.ContainsKey(OpenCourseRecord.CourseKey))
                    Result.Add(OpenCourseRecord.CourseKey, OpenCourseRecord);
            }

            return Result.Values.ToList();
        }

        /// <summary>
        /// 將課程規劃科目轉換為預開課程
        /// </summary>
        /// <param name="Subjects"></param>
        /// <returns></returns>
        public static List<SchedulerOpenCourseRecord> ToOpenCourseClass(
            this List<ProgramSubject> Subjects,
            string SchoolYear,
            string ClassName)
        {
            List<SchedulerOpenCourseRecord> OpenCourseRecords = new List<SchedulerOpenCourseRecord>();

            foreach (ProgramSubject Subject in Subjects)
            {
                SchedulerOpenCourseRecord PreOpenCourseRecord = new SchedulerOpenCourseRecord(Subject, SchoolYear, ClassName);

                OpenCourseRecords.Add(PreOpenCourseRecord);
            }

            return OpenCourseRecords;
        }

        /// <summary>
        /// 實際開設課程
        /// </summary>
        /// <param name="PreOpenCourseRecords"></param>
        /// <returns></returns>
        public static Tuple<bool, string> OpenCourse(
            this List<SchedulerOpenCourseRecord> SchedulerOpenCourseRecord
            , bool IsCreateCourseSection)
        {

            Campus.Configuration.ConfigData config = Campus.Configuration.Config.User["EducationLevel"];

            string EducationLevel = config["EducationLevel"];

            try
            {
                List<SchedulerCourseExtension> CourseRecords = new List<SchedulerCourseExtension>();

                foreach (SchedulerOpenCourseRecord OpenCourseRecord in SchedulerOpenCourseRecord)
                {
                    SchedulerCourseExtension CourseRecord = new SchedulerCourseExtension();

                    CourseRecord.SchoolYear = K12.Data.Int.Parse(OpenCourseRecord.SchoolYear);
                    CourseRecord.Semester = OpenCourseRecord.Semester;
                    CourseRecord.CourseName = OpenCourseRecord.CourseName;
                    CourseRecord.Subject = OpenCourseRecord.SubjectName;
                    CourseRecord.ClassName = OpenCourseRecord.ClassName;
                    //學分數
                    CourseRecord.Credit = K12.Data.Int.ParseAllowNull(K12.Data.Decimal.GetString(OpenCourseRecord.Subject.Credit));

                    //CourseRecord.Period = K12.Data.Int.Parse(K12.Data.Decimal.GetString(OpenCourseRecord.Subject.Period));

                    //2013/7/19 註解
                    //2013/7/31 請騉翔調整
                    //是否有學分數
                    if (OpenCourseRecord.Subject.Period.HasValue)
                    {
                        //有學分數則將"上課時段"填入"節數"
                        CourseRecord.Period = K12.Data.Int.Parse(K12.Data.Decimal.GetString(OpenCourseRecord.Subject.Period));
                    }
                    else
                    {
                        //沒有學分數則將"學分數"填入"節數"
                        CourseRecord.Period = K12.Data.Int.ParseAllowNull(K12.Data.Decimal.GetString(OpenCourseRecord.Subject.Credit));
                    }

                    CourseRecord.Level = OpenCourseRecord.Subject.Level;
                    CourseRecord.NotIncludedInCalc = OpenCourseRecord.Subject.NotIncludedInCalc;
                    CourseRecord.NotIncludedInCredit = OpenCourseRecord.Subject.NotIncludedInCredit;
                    //1.若是國中則沒有必選修
                    //2.若是高中沒有列入學期成績
                    if (EducationLevel.Equals("Junior"))
                    {
                        //2013/5/20 - 必選修 預設為空白
                        CourseRecord.Required = string.Empty;

                        //2013/5/20 - 由1 or 2 改為 是 or 否
                        CourseRecord.CalculationFlag = OpenCourseRecord.Subject.CalcFlag ? "是" : "否";
                    }
                    else //高中
                    {
                        //2013/5/20 - 列入學期成績 預設為空白 
                        CourseRecord.CalculationFlag = string.Empty;

                        //2013/5/1 - 修正
                        CourseRecord.Required = OpenCourseRecord.Subject.Required == true ? "必修" : "選修";
                    }
                    CourseRecord.RequiredBy = OpenCourseRecord.Subject.RequiredBy;

                    CourseRecord.Domain = OpenCourseRecord.Subject.Domain;
                    CourseRecord.Entry = OpenCourseRecord.Subject.Entry;

                    CourseRecords.Add(CourseRecord);
                }

                List<string> CourseIDs = new List<string>();

                if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseRecords))
                {
                    FunctionSpliter<SchedulerCourseExtension, string> CourseFunc = new FunctionSpliter<SchedulerCourseExtension, string>(300, 1);

                    AccessHelper helper = new AccessHelper();

                    CourseFunc.Function = (x) => helper.InsertValues(x);
                    //CourseFunc.ProgressChange = (x) => FISCA.Presentation.MotherForm.SetStatusBarMessage("開設課程中..", x);

                    CourseIDs = CourseFunc.Execute(CourseRecords);

                    if (IsCreateCourseSection)
                    {
                        //建立Spliter
                        FunctionSpliter<string, string> Spliter = new FunctionSpliter<string, string>(300, 3);

                        Spliter.Function = (x) => SunsetBL.CreateSchedulerCourseSectionByCourseIDs(x);
                        List<string> NewCourseSectionIDs = Spliter.Execute(CourseIDs);
                    }

                    //Features.Invoke("CourseSyncAllBackground");
                }

                return new Tuple<bool, string>(true, "已成功開設" + CourseIDs.Count + "門課程");
            }
            catch (Exception e)
            {
                FISCA.RTOut.WriteError(e);

                SmartSchool.ErrorReporting.ReportingService.ReportException(e);

                return new Tuple<bool, string>(false, "開設課程失敗，錯誤訊息：『" + e.Message + "』");
            }
        }

        ///// <summary>
        ///// 是否都有課程規劃
        ///// </summary>
        ///// <param name="ClassRecords"></param>
        ///// <returns></returns>
        //public static Tuple<bool, string> HasProgramPlan(this List<ClassRecord> ClassRecords)
        //{
        //    List<ClassRecord> records = ClassRecords
        //        .FindAll(x => string.IsNullOrWhiteSpace(x.RefProgramPlanID));

        //    if (records.Count == 0)
        //        return new Tuple<bool, string>(true, string.Empty);
        //    else
        //        return new Tuple<bool, string>(false, "以下班級未指定課程規劃『" + string.Join(",", records.Select(x => x.Name)) + "』");
        //}

        ///// <summary>
        ///// 是否都有年級
        ///// </summary>
        ///// <param name="ClassRecords"></param>
        ///// <returns></returns>
        //public static Tuple<bool, string> HasGradeYear(this List<ClassRecord> ClassRecords)
        //{
        //    List<ClassRecord> records = ClassRecords.FindAll(x => !x.GradeYear.HasValue);

        //    if (records.Count == 0)
        //        return new Tuple<bool, string>(true, string.Empty);
        //    else
        //        return new Tuple<bool, string>(false, "以下班級未指定年級『" + string.Join(",", records.Select(x => x.Name)) + "』");
        //}

        ///// <summary>
        ///// 是否都有命名規則
        ///// </summary>
        ///// <param name="ClassRecords"></param>
        ///// <returns></returns>
        //public static Tuple<bool, string> HasNamingRule(this List<ClassRecord> ClassRecords)
        //{
        //    List<ClassRecord> records = ClassRecords.FindAll(x => string.IsNullOrWhiteSpace(x.NamingRule));

        //    if (records.Count == 0)
        //        return new Tuple<bool, string>(true, string.Empty);
        //    else
        //        return new Tuple<bool, string>(false, "以下班級未指定命名規則『" + string.Join(",", records.Select(x => x.Name)) + "』");
        //}

        ///// <summary>
        ///// 轉為課程規劃班級物件
        ///// </summary>
        ///// <param name="ClassRecords"></param>
        ///// <returns></returns>
        //public static List<ProgramPlanClassRecord> ToProgrmPlanClass(this List<ClassRecord> ClassRecords)
        //{
        //    List<ProgramPlanClassRecord> records = new List<ProgramPlanClassRecord>();

        //    foreach (ClassRecord ClassRecord in ClassRecords)
        //        if (!string.IsNullOrWhiteSpace(ClassRecord.RefProgramPlanID)) //有課程規劃系統編號及班級年級才加入 
        //        {
        //            ProgramPlanClassRecord record = new ProgramPlanClassRecord(ClassRecord);
        //            records.Add(record);
        //        }

        //    return records;
        //}

        ///// <summary>
        ///// 取得學生課程規劃
        ///// </summary>
        ///// <param name="ProgramPlanClassRecords"></param>
        ///// <returns></returns>
        //public static void AddStudentProgramPlan(this List<ProgramPlanClassRecord> ProgramPlanClassRecords)
        //{
        //    if (K12.Data.Utility.Utility.IsNullOrEmpty(ProgramPlanClassRecords))
        //        return;

        //    //取得班級系統編號列表
        //    List<string> ClassIDs = ProgramPlanClassRecords
        //        .Select(x => x.ClassRecord.ID)
        //        .ToList();

        //    //根據班級系統編號列表取得學生中有指定課程規劃的
        //    string strSQL = "select ref_class_id,ref_graduation_plan_id from student where ref_class_id in (" + string.Join(",", ClassIDs) + ") and ref_graduation_plan_id is not null";

        //    QueryHelper helper = new QueryHelper();

        //    DataTable table = helper.Select(strSQL);

        //    //將學生課程規劃編號加入到班級中
        //    foreach (DataRow row in table.Rows)
        //    {
        //        string RefClassID = row.Field<string>("ref_class_id");
        //        string RefProgramPlanID = row.Field<string>("ref_graduation_plan_id");

        //        ProgramPlanClassRecord ClassRecord = ProgramPlanClassRecords.Find(x => x.ClassRecord.ID.Equals(RefClassID));

        //        if (ClassRecord != null)
        //            ClassRecord.AddProgramPlanID(RefProgramPlanID);
        //    }
        //}

        ///// <summary>
        ///// 填入實際課程規劃物件
        ///// </summary>
        ///// <param name="ProgramPlanClassRecords"></param>
        //public static void FillProgramPlan(this List<ProgramPlanClassRecord> ProgramPlanClassRecords)
        //{
        //    if (K12.Data.Utility.Utility.IsNullOrEmpty(ProgramPlanClassRecords))
        //        return;

        //    List<string> ProgramPlanIDs = new List<string>();

        //    #region 取得實際課程規劃物件
        //    foreach (ProgramPlanClassRecord ProgramPlanClassRecord in ProgramPlanClassRecords)
        //        ProgramPlanIDs.AddRange(ProgramPlanClassRecord.ProgramPalnIDs);

        //    List<ProgramPlanRecord> ProgramPlanRecords = ProgramPlan
        //        .SelectByIDs(ProgramPlanIDs.Distinct());
        //    #endregion

        //    //將課程規劃物件加入到班級中
        //    foreach (ProgramPlanClassRecord ProgramPlanClassRecord in ProgramPlanClassRecords)
        //        ProgramPlanClassRecord.ProgramPlans = ProgramPlanRecords.FindAll(x => ProgramPlanClassRecord.ProgramPalnIDs.Contains(x.ID));
        //}
    }
}