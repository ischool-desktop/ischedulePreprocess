using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FISCA.Data;
using K12.Data;
using FISCA;

namespace Sunset
{
    /// <summary>
    /// 課程規劃商業邏輯延伸方法
    /// </summary>
    public static class ProgramPlanBL_Extension
    {
        /// <summary>
        /// 是否都有課程規劃
        /// </summary>
        /// <param name="ClassRecords"></param>
        /// <returns></returns>
        public static Tuple<bool,string> HasProgramPlan(this List<ClassRecord> ClassRecords)
        {
             List<ClassRecord> records = ClassRecords
                 .FindAll(x => string.IsNullOrWhiteSpace(x.RefProgramPlanID));

             if (records.Count == 0)
                 return new Tuple<bool, string>(true, string.Empty);
             else 
                 return new Tuple<bool,string>(false,"以下班級未指定課程規劃『"+ string.Join(",",records.Select(x=>x.Name)) +"』");
        }

        /// <summary>
        /// 是否都有年級
        /// </summary>
        /// <param name="ClassRecords"></param>
        /// <returns></returns>
        public static Tuple<bool,string> HasGradeYear(this List<ClassRecord> ClassRecords)
        {
            List<ClassRecord> records = ClassRecords.FindAll(x => !x.GradeYear.HasValue);

            if (records.Count == 0)
                return new Tuple<bool, string>(true, string.Empty);
            else
                return new Tuple<bool, string>(false, "以下班級未指定年級『" + string.Join(",", records.Select(x => x.Name)) + "』");
        }

        /// <summary>
        /// 是否都有命名規則
        /// </summary>
        /// <param name="ClassRecords"></param>
        /// <returns></returns>
        public static Tuple<bool, string> HasNamingRule(this List<ClassRecord> ClassRecords)
        {
            List<ClassRecord> records = ClassRecords.FindAll(x => string.IsNullOrWhiteSpace(x.NamingRule));

            if (records.Count == 0)
                return new Tuple<bool, string>(true, string.Empty);
            else
                return new Tuple<bool, string>(false, "以下班級未指定命名規則『" + string.Join(",", records.Select(x => x.Name)) + "』");
        }

        /// <summary>
        /// 轉為課程規劃班級物件
        /// </summary>
        /// <param name="ClassRecords"></param>
        /// <returns></returns>
        public static List<ProgramPlanClassRecord> ToProgrmPlanClass(this List<ClassRecord> ClassRecords)
        {
            List<ProgramPlanClassRecord> records = new List<ProgramPlanClassRecord>();

            foreach(ClassRecord ClassRecord in ClassRecords)
                if (!string.IsNullOrWhiteSpace(ClassRecord.RefProgramPlanID)) //有課程規劃系統編號及班級年級才加入 
                {
                    ProgramPlanClassRecord record = new ProgramPlanClassRecord(ClassRecord);
                    records.Add(record);
                }

            return records;
        }

        /// <summary>
        /// 取得學生課程規劃
        /// </summary>
        /// <param name="ProgramPlanClassRecords"></param>
        /// <returns></returns>
        public static void AddStudentProgramPlan(this List<ProgramPlanClassRecord> ProgramPlanClassRecords)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(ProgramPlanClassRecords))
                return;

            //取得班級系統編號列表
            List<string> ClassIDs = ProgramPlanClassRecords
                .Select(x=>x.ClassRecord.ID)
                .ToList();
            
            //根據班級系統編號列表取得學生中有指定課程規劃的
            string strSQL = "select ref_class_id,ref_graduation_plan_id from student where ref_class_id in ("+ string.Join(",",ClassIDs) +") and ref_graduation_plan_id is not null";

            QueryHelper helper = new QueryHelper();

            DataTable table = helper.Select(strSQL);

            //將學生課程規劃編號加入到班級中
            foreach (DataRow row in table.Rows)
            {
                string RefClassID = row.Field<string>("ref_class_id");
                string RefProgramPlanID = row.Field<string>("ref_graduation_plan_id");

                ProgramPlanClassRecord ClassRecord = ProgramPlanClassRecords.Find(x => x.ClassRecord.ID.Equals(RefClassID));

                if (ClassRecord != null)
                    ClassRecord.AddProgramPlanID(RefProgramPlanID);
            }
        }

        /// <summary>
        /// 填入實際課程規劃物件
        /// </summary>
        /// <param name="ProgramPlanClassRecords"></param>
        public static void FillProgramPlan(this List<ProgramPlanClassRecord> ProgramPlanClassRecords)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(ProgramPlanClassRecords))
                return;

            List<string> ProgramPlanIDs = new List<string>();

            #region 取得實際課程規劃物件
            foreach (ProgramPlanClassRecord ProgramPlanClassRecord in ProgramPlanClassRecords)
                ProgramPlanIDs.AddRange(ProgramPlanClassRecord.ProgramPalnIDs);

            List<ProgramPlanRecord> ProgramPlanRecords = ProgramPlan
                .SelectByIDs(ProgramPlanIDs.Distinct());
            #endregion

            //將課程規劃物件加入到班級中
            foreach (ProgramPlanClassRecord ProgramPlanClassRecord in ProgramPlanClassRecords)
                ProgramPlanClassRecord.ProgramPlans = ProgramPlanRecords.FindAll(x=> ProgramPlanClassRecord.ProgramPalnIDs.Contains(x.ID));
        }

        /// <summary>
        /// 將課程規劃科目轉換為預開課程
        /// </summary>
        /// <param name="Subjects"></param>
        /// <returns></returns>
        public static List<PreOpenCourseRecord> ToPreOpenCourseClass(this List<ProgramSubject> Subjects,string SchoolYear,string ClassID,string ClassName)
        {
            List<PreOpenCourseRecord> PreOpenCourseRecords = new List<PreOpenCourseRecord>();

            foreach (ProgramSubject Subject in Subjects)
            {
                PreOpenCourseRecord PreOpenCourseRecord = new PreOpenCourseRecord(Subject,SchoolYear,ClassID,ClassName);

                PreOpenCourseRecords.Add(PreOpenCourseRecord);
            }

            return PreOpenCourseRecords;
        }

        /// <summary>
        /// 去除重覆的預開課程
        /// </summary>
        /// <param name="PreOpenCourseRecords"></param>
        /// <returns></returns>
        public static List<PreOpenCourseRecord> ToDistinct(this List<PreOpenCourseRecord> PreOpenCourseRecords)
        {
            Dictionary<string,PreOpenCourseRecord> Result = new Dictionary<string,PreOpenCourseRecord>();

            foreach(PreOpenCourseRecord PreOpenCourseRecord in PreOpenCourseRecords)
            {
                if (!Result.ContainsKey(PreOpenCourseRecord.CourseKey))
                    Result.Add(PreOpenCourseRecord.CourseKey,PreOpenCourseRecord);
            }

            return Result.Values.ToList();
        }

        /// <summary>
        /// 填入現有系統編號
        /// </summary>
        /// <param name="PreOpenCourseRecords"></param>
        public static void FillCourseID(this List<PreOpenCourseRecord> PreOpenCourseRecords)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(PreOpenCourseRecords))
                return;

            QueryHelper helper = new QueryHelper();

            string strSQL = "select id,course_name,school_year,semester from course where " + string.Join(" or ",PreOpenCourseRecords.Select(x=>x.CourseKeyCondition).ToArray());

            DataTable Table = helper.Select(strSQL);

            PreOpenCourseRecords.ForEach(x => x.CourseID = string.Empty);

            foreach (DataRow Row in Table.Rows)
            {
                string CourseID = Row.Field<string>("id");
                string CourseName = Row.Field<string>("course_name");
                string SchoolYear = Row.Field<string>("school_year");
                string Semester = Row.Field<string>("semester");

                List<PreOpenCourseRecord> FindPreOpenCourseRecords = PreOpenCourseRecords
                    .FindAll(x => x.CourseName.Equals(CourseName) && x.SchoolYear.Equals(SchoolYear) && x.Semester.Equals(Semester));

                FindPreOpenCourseRecords.ForEach(x => x.CourseID = CourseID);
            }
        }

        /// <summary>
        /// 實際開設課程
        /// </summary>
        /// <param name="PreOpenCourseRecords"></param>
        /// <returns></returns>
        public static Tuple<bool,string> OpenCourse(this List<PreOpenCourseRecord> PreOpenCourseRecords)
        {
            try
            {
                List<K12CourseRecord> CourseRecords = new List<K12CourseRecord>();

                foreach (PreOpenCourseRecord PreOpenCourseRecord in PreOpenCourseRecords)
                {
                    K12CourseRecord CourseRecord = new K12CourseRecord();

                    CourseRecord.SchoolYear = K12.Data.Int.ParseAllowNull(PreOpenCourseRecord.SchoolYear);
                    CourseRecord.Semester = K12.Data.Int.ParseAllowNull(PreOpenCourseRecord.Semester);
                    CourseRecord.Name = PreOpenCourseRecord.CourseName;
                    CourseRecord.Subject = PreOpenCourseRecord.SubjectName;
                  
                    CourseRecord.RefClassID = PreOpenCourseRecord.ClassID;
                    CourseRecord.CalculationFlag = PreOpenCourseRecord.Subject.CalcFlag ? "1" : "2";
                    CourseRecord.Credit = PreOpenCourseRecord.Subject.Credit;
                    if (CourseRecord.Period.HasValue)
                        CourseRecord.Period = PreOpenCourseRecord.Subject.Period;
                    else
                        CourseRecord.Period = PreOpenCourseRecord.Subject.Credit;
                    CourseRecord.Level = PreOpenCourseRecord.Subject.Level;
                    CourseRecord.NotIncludedInCalc = PreOpenCourseRecord.Subject.NotIncludedInCalc;
                    CourseRecord.NotIncludedInCredit = PreOpenCourseRecord.Subject.NotIncludedInCredit;       
                    CourseRecord.Required = PreOpenCourseRecord.Subject.Required;
                    CourseRecord.RequiredBy = PreOpenCourseRecord.Subject.RequiredBy;

                    CourseRecord.Domain = PreOpenCourseRecord.Subject.Domain;
                    CourseRecord.Entry = PreOpenCourseRecord.Subject.Entry;

                    CourseRecords.Add(CourseRecord);
                }

                List<string> CourseIDs = new List<string>();

                if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseRecords))
                {
                    FunctionSpliter<K12CourseRecord, string> CourseFunc = new FunctionSpliter<K12CourseRecord, string>(300, 1);

                    CourseFunc.Function = (x)=> K12.Data.Course.Insert(x);
                    //CourseFunc.ProgressChange = (x) => FISCA.Presentation.MotherForm.SetStatusBarMessage("開設課程中..", x);

                    CourseIDs = CourseFunc.Execute(CourseRecords);

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
    }
}