using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.Data;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 匯入課程分段
    /// </summary>
    public class ImportSchedulerCourseSection : ImportWizard
    {
        private const string constCourseName = "課程名稱";
        private const string constSchoolYear = "學年度";
        private const string constSemester = "學期";
        private const string constLength = "節數";
        private const string constWeekday = "星期";
        private const string constPeriodNo = "節次";
        private const string constLongBreak = "跨中午";
        private const string constWeekdayCond = "星期條件";
        private const string constPeriodCond = "節次條件";
        private const string constClassroom = "場地名稱";
        private const string constWeekFlag = "單雙週";
        private const string constComment = "註記";
        private const string constTeacher1 = "授課教師一";
        private const string constTeacher2 = "授課教師二";
        private const string constTeacher3 = "授課教師三";

        private System.Object mLock = new System.Object();
        private AccessHelper mHelper;
        private List<string> mDeletedCourseIDs = new List<string>(); //已經刪除課程分段的課程系統編號
        private Dictionary<string, string> mCourseNameMapToID;
        private Dictionary<string, string> mIDMapToCourseName;
        private ImportClassroomHelper mImportClassroomHelper;
        private StringBuilder mstrLog = new StringBuilder();
        private ImportOption mOption;

        /// <summary>
        /// ischool教師
        /// </summary>
        private Dictionary<string, string> ischool_Teachers = new Dictionary<string, string>();

        /// <summary>
        /// 排課教師
        /// </summary>
        private Dictionary<string, string> Sunset_Teachers = new Dictionary<string, string>();

        private Task mTask;
        private Func<string, string> mConditionConvert = (x) =>
        {
            int Num;

            //若可直接解析為數字，則前面加上等號
            if (int.TryParse(x, out Num))
                return "=" + x;

            return x;
        };

        /// <summary>
        /// 建構式
        /// </summary>
        public ImportSchedulerCourseSection()
        {
            this.Complete = () =>
            {
                CourseEvents.RaiseChanged();
                return "匯入完成";
            };
        }

        /// <summary>
        /// 取得驗證規則
        /// </summary>
        /// <returns>驗證規則網址</returns>
        public override string GetValidateRule()
        {
            return Properties.Resources.CourseSection;
        }

        /// <summary>
        /// 取得支援的匯入動作
        /// </summary>
        /// <returns>匯入動作</returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.Cover;
        }

        /// <summary>
        /// 準備匯入動作
        /// </summary>
        /// <param name="Option">使用者匯入選項</param>
        public override void Prepare(ImportOption Option)
        {
            mHelper = new AccessHelper();
            mDeletedCourseIDs = new List<string>();
            mImportClassroomHelper = new ImportClassroomHelper();
            mCourseNameMapToID = new Dictionary<string, string>();
            mIDMapToCourseName = new Dictionary<string, string>();
            mTask = Task.Factory.StartNew
            (() =>
            {
                QueryHelper Helper = new QueryHelper();

                DataTable Table = Helper.Select("select uid,course_name,school_year,semester from $scheduler.scheduler_course_extension");

                foreach (DataRow Row in Table.Rows)
                {
                    string CourseID = Row.Field<string>("uid");
                    string CourseName = Row.Field<string>("course_name").Trim();
                    string SchoolYear = Row.Field<string>("school_year");
                    string Semester = Row.Field<string>("semester");
                    string CourseKey = CourseName + "," + SchoolYear + "," + Semester;

                    if (!mCourseNameMapToID.ContainsKey(CourseKey))
                        mCourseNameMapToID.Add(CourseKey, CourseID);

                    if (!mIDMapToCourseName.ContainsKey(CourseID))
                        mIDMapToCourseName.Add(CourseID, CourseKey);
                }

                //取得ischool教師清單
                ischool_Teachers = GiveMeTheList.GetischoolTeacher();
                //取得排課教師清單
                Sunset_Teachers = GiveMeTheList.GetSunsetTeacher();


            }
            );
            mOption = Option;
        }

        /// <summary>
        /// 分批執行匯入動作
        /// </summary>
        /// <param name="Rows">IRowStream集合</param>
        /// <returns>分批匯入完成訊息</returns>
        public override string Import(List<IRowStream> Rows)
        {
            mstrLog.Clear();

            #region Step1:針對每筆匯入每筆資料檢查，若場地不存在，則自動新增
            //List<Classroom> Classrooms = mImportClassroomHelper.Insert(Rows);
            //string strClassroomMessage = ImportClassroomHelper.GetInsertMessage(Classrooms);
            //if (!string.IsNullOrEmpty(strClassroomMessage))
            //    mstrLog.AppendLine(strClassroomMessage);
            #endregion

            #region Step2:針對每筆匯入資料尋找對應的課程，以及新增課程分段物件
            //課程系統編號對課程名稱
            List<string> CourseIDs = new List<string>();
            //要新增的課程分段物件列表
            List<SchedulerCourseSection> InsertRecords = new List<SchedulerCourseSection>();

            mTask.Wait();

            foreach (IRowStream Row in Rows)
            {
                string CourseName = Row.GetValue(constCourseName); //取得課程名稱
                string SchoolYear = Row.GetValue(constSchoolYear); //取得學年度
                string Semester = Row.GetValue(constSemester);     //取得學期
                int Length = K12.Data.Int.Parse(Row.GetValue(constLength)); //取得課程分段節數
                int Weekday = mOption.SelectedFields.Contains(constWeekday) ? K12.Data.Int.Parse(Row.GetValue(constWeekday)) : 0;
                int PeriodNo = mOption.SelectedFields.Contains(constPeriodNo) ? K12.Data.Int.Parse(Row.GetValue(constPeriodNo)) : 0;
                bool LongBreak = mOption.SelectedFields.Contains(constLongBreak) && Row.GetValue(constLongBreak).Equals("是") ? true : false;
                string WeekdayCond = mConditionConvert(mOption.SelectedFields.Contains(constWeekdayCond) ? Row.GetValue(constWeekdayCond) : string.Empty);
                string PeriodCond = mConditionConvert(mOption.SelectedFields.Contains(constPeriodCond) ? Row.GetValue(constPeriodCond) : string.Empty);
                string TeacherName1 = mOption.SelectedFields.Contains(constTeacher1) ? Row.GetValue(constTeacher1) : string.Empty;
                string TeacherName2 = mOption.SelectedFields.Contains(constTeacher2) ? Row.GetValue(constTeacher2) : string.Empty;
                string TeacherName3 = mOption.SelectedFields.Contains(constTeacher3) ? Row.GetValue(constTeacher3) : string.Empty;
                Classroom Classroom = mOption.SelectedFields.Contains(constClassroom) ? mImportClassroomHelper[Row.GetValue(constClassroom)] : null;
                int? ClassroomID = null;
                if (Classroom != null)
                    ClassroomID = K12.Data.Int.ParseAllowNull(Classroom.UID);
                string strWeekFalg = mOption.SelectedFields.Contains(constWeekFlag) ? Row.GetValue(constWeekFlag) : string.Empty;
                string strComment = mOption.SelectedFields.Contains(constComment) ? Row.GetValue(constComment) : string.Empty;


                //根據課程名稱、學年度及學期尋找是否有對應的課程
                string CourseKey = CourseName + "," + SchoolYear + "," + Semester;
                string CourseID = mCourseNameMapToID.ContainsKey(CourseKey) ? mCourseNameMapToID[CourseKey] : string.Empty;


                if (!string.IsNullOrEmpty(CourseID))
                {
                    CourseIDs.Add(CourseID);

                    SchedulerCourseSection vCourseSection = new SchedulerCourseSection();
                    vCourseSection.CourseID = K12.Data.Int.Parse(CourseID);
                    vCourseSection.Length = Length;
                    vCourseSection.WeekDay = Weekday;
                    vCourseSection.Period = PeriodNo;
                    vCourseSection.LongBreak = LongBreak;
                    vCourseSection.WeekDayCond = WeekdayCond;
                    vCourseSection.PeriodCond = PeriodCond;
                    vCourseSection.ClassroomID = ClassroomID;
                    vCourseSection.WeekFlag = Utility.GetWeekFlagInt(strWeekFalg);
                    vCourseSection.TeacherName1 = TeacherName1;
                    vCourseSection.TeacherName2 = TeacherName2;
                    vCourseSection.TeacherName3 = TeacherName3;

                    if (!string.IsNullOrWhiteSpace(TeacherName1) && ischool_Teachers.ContainsKey(TeacherName1))
                        vCourseSection.TeacherID1 = K12.Data.Int.Parse(ischool_Teachers[TeacherName1]);
                    else
                        vCourseSection.TeacherID1 = null;

                    if (!string.IsNullOrWhiteSpace(TeacherName2) && ischool_Teachers.ContainsKey(TeacherName2))
                        vCourseSection.TeacherID2 = K12.Data.Int.Parse(ischool_Teachers[TeacherName2]);
                    else
                        vCourseSection.TeacherID2 = null;

                    if (!string.IsNullOrWhiteSpace(TeacherName3) && ischool_Teachers.ContainsKey(TeacherName3))
                        vCourseSection.TeacherID3 = K12.Data.Int.Parse(ischool_Teachers[TeacherName3]);
                    else
                        vCourseSection.TeacherID3 = null;


                    vCourseSection.Comment = strComment;

                    InsertRecords.Add(vCourseSection);
                }
            }
            #endregion

            #region Step3:針對每筆課程系統編號，若還沒有刪除過其下的課程分段，那就刪除，並且將課程編號加入到已刪除清單

            lock (mLock)
            {
                #region Step3.1:找出沒有刪除過的課程系統編號
                List<string> DeleteCourseIDs = new List<string>();

                foreach (string DeleteCourseID in CourseIDs.Distinct())
                {
                    if (!mDeletedCourseIDs.Contains(DeleteCourseID))
                        DeleteCourseIDs.Add(DeleteCourseID);
                }
                #endregion

                #region Step3.2:實際刪除課程下的課程分段
                if (DeleteCourseIDs.Count > 0)
                {
                    //將DeleteCourseIDs組合成查詢條件，找出所屬課程下的課程分段
                    string strCondition = "ref_course_id in (" + string.Join(",", DeleteCourseIDs.ToArray()) + ")";
                    List<SchedulerCourseSection> CourseSections = mHelper
                        .Select<SchedulerCourseSection>(strCondition);

                    //實際刪除課程分段
                    if (CourseSections.Count > 0)
                    {
                        mHelper.DeletedValues(CourseSections);

                        //做Log
                        DeleteCourseIDs.ForEach(x => mstrLog.AppendLine("已刪除此課程下的課程分段：" + mIDMapToCourseName[x]));

                        mstrLog.AppendLine("總共已刪除" + DeleteCourseIDs.Count + "筆課程下的課程分段!");

                        //加入到已刪除的課程系統編號清單中
                        mDeletedCourseIDs.AddRange(DeleteCourseIDs);
                    }
                }
            }
                #endregion

            #endregion

            #region Step4:新增資料來源中的課程分段
            if (InsertRecords.Count > 0)
            {
                //新增課程分段
                List<string> IDs = mHelper.InsertValues(InsertRecords);

                //做Log
                mstrLog.AppendLine("已新增" + IDs.Count + "筆課程分段");


            }
            #endregion

            return mstrLog.ToString();
        }
    }
}