using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.Data;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 匯入排課課程資料
    /// </summary>
    public class ImportSchedulerCourseExtension : ImportWizard
    {
        private const string constCourseName = "課程名稱"; //ok
        private const string constSubject = "科目名稱"; //ok
        private const string constCourseAliasName = "科目簡稱"; //ok
        private const string constSchoolYear = "學年度"; //ok
        private const string constSemester = "學期"; //ok
        private const string constClassName = "所屬班級"; //ok
        private const string constPeriod = "節數"; //ok
        private const string constCredit = "學分"; //ok
        private const string constLevel = "科目級別"; //ok
        private const string constTeacherName一 = "授課教師一"; //ok
        private const string constTeacherName二 = "授課教師二"; //ok
        private const string constTeacherName三 = "授課教師三"; //ok
        private const string constTimeTableName = "上課時間表"; //ok
        private const string constAllowDup = "同天排課"; //ok
        private const string constLimitNextDay = "隔天排課"; //ok
        private const string constSplitSpec = "分割設定"; //ok

        private const string constClassroom = "場地條件"; //改
        private const string constWeekdayCond = "星期條件"; //改
        private const string constPeriodCond = "節次條件"; //改
        private const string constLongBreak = "可跨中午"; //改
        private const string constWeekFlag = "單雙週條件"; //改

        private const string constCanQuery = "開放查詢"; //ok
        private const string constDomain = "領域"; //ok
        private const string constEntry = "分項"; //ok
        private const string constRequiredBy = "校部訂"; //ok
        private const string constRequired = "必選修"; //ok

        private const string constNotIncludedInCredit = "學分設定"; //改
        private const string constNotIncludedInCalc = "評分設定"; //改

        private const string constCalculationFlag = "學期成績"; //ok

        private ImportOption mOption;
        private Dictionary<string, string> mCourseNameIDs;

        /// <summary>
        /// ischool班級
        /// </summary>
        private Dictionary<string, string> ischool_ClassNameIDs = new Dictionary<string, string>();

        private ImportTimeTableHelper mImportTimeTableHelper;
        private ImportClassroomHelper mImportClassroomHelper;
        private Task mTask;
        private AccessHelper mHelper;
        private Func<string, string> mConditionConvert = (x) =>
        {
            int Num;

            //若可直接解析為數字，則前面加上等號
            if (int.TryParse(x, out Num))
                return "=" + x;

            return x;
        };

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ImportSchedulerCourseExtension()
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
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return Properties.Resources.CourseExtension;
        }

        /// <summary>
        /// 取得支援的匯入型態
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate | ImportAction.Delete;
        }

        /// <summary>
        /// 匯入前準備
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            K12.Data.Course.RemoveAll();
            mHelper = new AccessHelper();
            mOption = Option;
            mCourseNameIDs = new Dictionary<string, string>();
            mTask = Task.Factory.StartNew
            (() =>
                {
                    QueryHelper Helper = new QueryHelper();

                    DataTable Table = Helper.Select("select uid,course_name,school_year,semester from $scheduler.scheduler_course_extension");

                    foreach (DataRow Row in Table.Rows)
                    {
                        string CourseID = Row.Field<string>("uid");
                        string CourseName = Row.Field<string>("course_name");
                        string SchoolYear = Row.Field<string>("school_year");
                        string Semester = Row.Field<string>("semester");
                        string CourseKey = CourseName + "," + SchoolYear + "," + Semester;

                        if (!mCourseNameIDs.ContainsKey(CourseKey))
                            mCourseNameIDs.Add(CourseKey, CourseID);
                    }

                    //取得ischool班級清單
                    ischool_ClassNameIDs = GiveMeTheList.GetischoolClass();
                }
            );

            mImportTimeTableHelper = new ImportTimeTableHelper(mHelper);
            mImportClassroomHelper = new ImportClassroomHelper(mHelper);
        }

        /// <summary>
        /// 執行分批匯入
        /// </summary>
        /// <param name="Rows">IRowStream集合</param>
        /// <returns>回傳分批匯入執行完成訊息</returns>
        public override string Import(List<IRowStream> Rows)
        {
            mTask.Wait();

            if (mOption.SelectedKeyFields.Count == 3 &&
                mOption.SelectedKeyFields.Contains(constSchoolYear) &&
                mOption.SelectedKeyFields.Contains(constSemester) &&
                mOption.SelectedKeyFields.Contains(constCourseName))
            {
                #region 取得已存在的排課課程資料
                List<SchedulerCourseExtension> mCourseExtensions = new List<SchedulerCourseExtension>();
                List<string> CourseIDs = new List<string>();

                foreach (IRowStream Row in Rows)
                {
                    string CourseName = Row.GetValue(constCourseName);
                    string SchoolYear = Row.GetValue(constSchoolYear);
                    string Semester = Row.GetValue(constSemester);

                    //根據課程名稱、學年度及學期尋找是否有對應的課程
                    string CourseKey = CourseName + "," + SchoolYear + "," + Semester;
                    string CourseID = mCourseNameIDs.ContainsKey(CourseKey) ? mCourseNameIDs[CourseKey] : string.Empty;

                    if (!string.IsNullOrEmpty(CourseID))
                        CourseIDs.Add(CourseID);
                }

                string CourseIDsCondition = string.Join(",", CourseIDs.ToArray());
                mCourseExtensions = new List<SchedulerCourseExtension>();

                if (!string.IsNullOrEmpty(CourseIDsCondition))
                    mCourseExtensions = mHelper.Select<SchedulerCourseExtension>("uid in (" + CourseIDsCondition + ")");
                #endregion

                if (mOption.Action == ImportAction.InsertOrUpdate)
                {
                    #region Step1:針對每筆匯入每筆資料檢查，若時間表及場地不存在，則自動新增
                    //List<TimeTable> TimeTables = mImportTimeTableHelper.Insert(Rows);
                    //string strTimeTableMessage = ImportTimeTableHelper.GetInsertMessage(TimeTables);
                    ////if (!string.IsNullOrEmpty(strTimeTableMessage))
                    ////    mstrLog.AppendLine(strTimeTableMessage);
                    //List<Classroom> Classrooms = mImportClassroomHelper.Insert(Rows, constClassroom);
                    //string strClassroomMessage = ImportClassroomHelper.GetInsertMessage(Classrooms);
                    ////if (!string.IsNullOrEmpty(strClassroomMessage))
                    ////    mstrLog.AppendLine(strClassroomMessage);
                    #endregion

                    #region Step2:針對每筆匯入每筆資料檢查，判斷是新增或是更新
                    List<SchedulerCourseExtension> InsertRecords = new List<SchedulerCourseExtension>();
                    List<SchedulerCourseExtension> UpdateRecords = new List<SchedulerCourseExtension>();

                    foreach (IRowStream Row in Rows)
                    {
                        string CourseName = Row.GetValue(constCourseName);
                        string SchoolYear = Row.GetValue(constSchoolYear);
                        string Semester = Row.GetValue(constSemester);

                        //根據課程名稱、學年度及學期尋找是否有對應的課程
                        string CourseKey = CourseName + "," + SchoolYear + "," + Semester;
                        int? CourseID = null;
                        if (mCourseNameIDs.ContainsKey(CourseKey))
                            CourseID = K12.Data.Int.ParseAllowNull(mCourseNameIDs[CourseKey]);

                        //尋找是否有對應的課程排課資料
                        SchedulerCourseExtension vCourseExtension = mCourseExtensions
                            .Find(x => x.UID.Equals(K12.Data.Int.GetString(CourseID)));

                        #region 更新CourseExtension
                        if (vCourseExtension != null)
                        {
                            #region 課程資料屬性
                            //科目名稱
                            if (mOption.SelectedFields.Contains(constSubject))
                                vCourseExtension.Subject = Row.GetValue(constSubject);
                            //科目簡稱
                            if (mOption.SelectedFields.Contains(constCourseAliasName))
                                vCourseExtension.SubjectAliasName = Row.GetValue(constCourseAliasName);
                            //班級名稱
                            if (mOption.SelectedFields.Contains(constClassName))
                            {
                                vCourseExtension.ClassName = Row.GetValue(constClassName);

                                if (!string.IsNullOrWhiteSpace(vCourseExtension.ClassName) && ischool_ClassNameIDs.ContainsKey(vCourseExtension.ClassName))
                                    vCourseExtension.ClassID = K12.Data.Int.Parse(ischool_ClassNameIDs[vCourseExtension.ClassName]);
                                else
                                    vCourseExtension.ClassID = null;
                            }
                            //級別
                            if (mOption.SelectedFields.Contains(constLevel))
                                vCourseExtension.Level = K12.Data.Int.ParseAllowNull(Row.GetValue(constLevel));
                            //節數
                            if (mOption.SelectedFields.Contains(constPeriod))
                                vCourseExtension.Period = K12.Data.Int.ParseAllowNull(Row.GetValue(constPeriod));
                            //學分數
                            if (mOption.SelectedFields.Contains(constCredit))
                                vCourseExtension.Credit = K12.Data.Int.ParseAllowNull(Row.GetValue(constCredit));
                            //預設授課教師一
                            if (mOption.SelectedFields.Contains(constTeacherName一))
                                vCourseExtension.TeacherName1 = Row.GetValue(constTeacherName一);
                            //預設授課教師二
                            if (mOption.SelectedFields.Contains(constTeacherName二))
                                vCourseExtension.TeacherName2 = Row.GetValue(constTeacherName二);
                            //預設授課教師三
                            if (mOption.SelectedFields.Contains(constTeacherName三))
                                vCourseExtension.TeacherName3 = Row.GetValue(constTeacherName三);
                            //開放查詢
                            if (mOption.SelectedFields.Contains(constCanQuery))
                                vCourseExtension.NoQuery = Row.GetValue(constCanQuery).Equals("開放") ? false : true;

                            //領域
                            if (mOption.SelectedFields.Contains(constDomain))
                                vCourseExtension.Domain = Row.GetValue(constDomain);

                            //分項
                            if (mOption.SelectedFields.Contains(constEntry))
                                vCourseExtension.Entry = Row.GetValue(constEntry);

                            //校部訂
                            if (mOption.SelectedFields.Contains(constRequiredBy))
                                vCourseExtension.RequiredBy = Row.GetValue(constRequiredBy);

                            //必選修
                            if (mOption.SelectedFields.Contains(constRequired))
                                vCourseExtension.Required = Row.GetValue(constRequired);

                            //學分設定
                            if (mOption.SelectedFields.Contains(constNotIncludedInCredit))
                                vCourseExtension.NotIncludedInCredit = Row.GetValue(constNotIncludedInCredit).Equals("不計入") ? true : false;

                            //評分設定
                            if (mOption.SelectedFields.Contains(constNotIncludedInCalc))
                                vCourseExtension.NotIncludedInCalc = Row.GetValue(constNotIncludedInCalc).Equals("不評分") ? true : false;

                            //學期成績
                            if (mOption.SelectedFields.Contains(constCalculationFlag))
                                vCourseExtension.CalculationFlag = Row.GetValue(constCalculationFlag).Equals("不列入") ? "否" : "是";
                            #endregion

                            #region 課程排課資料屬性
                            //時間表名稱
                            if (mOption.SelectedFields.Contains(constTimeTableName))
                            {
                                string vTimeTableName = Row.GetValue(constTimeTableName);

                                TimeTable vTimeTable = mImportTimeTableHelper[vTimeTableName];

                                int? vTimeTableID = null;

                                if (vTimeTable != null)
                                    vTimeTableID = K12.Data.Int.ParseAllowNull(vTimeTable.UID);

                                vCourseExtension.TimeTableID = vTimeTableID;
                            }

                            //課程分段分割設定
                            if (mOption.SelectedFields.Contains(constSplitSpec))
                                vCourseExtension.SplitSpec = Row.GetValue(constSplitSpec);

                            //允許重覆
                            if (mOption.SelectedFields.Contains(constAllowDup))
                                vCourseExtension.AllowDup = Row.GetValue(constAllowDup).Equals("是") ? true : false;

                            //隔天排課
                            if (mOption.SelectedFields.Contains(constLimitNextDay))
                                vCourseExtension.LimitNextDay = Row.GetValue(constLimitNextDay).Equals("是") ? true : false;

                            //if (mOption.SelectedFields.Contains(constCanQuery))
                            //    vCourseExtension.NoQuery = Row.GetValue(constCanQuery).Equals("是") ? true : false;
                            #endregion

                            #region 課程分段預設屬性
                            //跨中午
                            if (mOption.SelectedFields.Contains(constLongBreak))
                                vCourseExtension.LongBreak = Row.GetValue(constLongBreak).Equals("是") ? true : false;

                            //場地
                            if (mOption.SelectedFields.Contains(constClassroom))
                            {
                                string vClassroomName = Row.GetValue(constClassroom);

                                Classroom vClassroom = mImportClassroomHelper[vClassroomName];

                                int? vClassroomID = null;

                                if (vClassroom != null)
                                    vClassroomID = K12.Data.Int.ParseAllowNull(vClassroom.UID);

                                vCourseExtension.ClassroomID = vClassroomID;
                            }

                            //星期條件
                            if (mOption.SelectedFields.Contains(constWeekdayCond))
                                vCourseExtension.WeekDayCond = mConditionConvert(Row.GetValue(constWeekdayCond));

                            //節次條件
                            if (mOption.SelectedFields.Contains(constPeriodCond))
                                vCourseExtension.PeriodCond = mConditionConvert(Row.GetValue(constPeriodCond));

                            //單雙週
                            if (mOption.SelectedFields.Contains(constWeekFlag))
                            {
                                string strWeekFalg = Row.GetValue(constWeekFlag);
                                vCourseExtension.WeekFlag = Utility.GetWeekFlagInt(strWeekFalg);
                            }
                            #endregion

                            UpdateRecords.Add(vCourseExtension);
                        }
                        #endregion
                        #region 新增CourseExtension
                        else
                        {
                            vCourseExtension = new SchedulerCourseExtension();
                            vCourseExtension.CourseName = Row.GetValue(constCourseName);
                            vCourseExtension.SchoolYear = K12.Data.Int.Parse(Row.GetValue(constSchoolYear));
                            vCourseExtension.Semester = Row.GetValue(constSemester);
                            vCourseExtension.Subject = string.Empty;
                            vCourseExtension.SubjectAliasName = string.Empty;
                            vCourseExtension.Credit = null;
                            vCourseExtension.Level = null;
                            vCourseExtension.Period = null;
                            vCourseExtension.NoQuery = true; //匯入預設為不開放查詢
                            vCourseExtension.WeekFlag = 3; //預設的單雙週條件
                            vCourseExtension.AllowDup = false;
                            vCourseExtension.LimitNextDay = false;
                            vCourseExtension.LongBreak = false;
                            vCourseExtension.ClassroomID = null;
                            vCourseExtension.TimeTableID = null;
                            vCourseExtension.WeekDayCond = string.Empty;
                            vCourseExtension.PeriodCond = string.Empty;
                            vCourseExtension.SplitSpec = string.Empty;

                            #region 課程資料屬性
                            //科目名稱
                            if (mOption.SelectedFields.Contains(constSubject))
                                vCourseExtension.Subject = Row.GetValue(constSubject);
                            //科目簡稱
                            if (mOption.SelectedFields.Contains(constCourseAliasName))
                                vCourseExtension.SubjectAliasName = Row.GetValue(constCourseAliasName);
                            //班級名稱
                            if (mOption.SelectedFields.Contains(constClassName))
                            {
                                vCourseExtension.ClassName = Row.GetValue(constClassName);

                                if (!string.IsNullOrWhiteSpace(vCourseExtension.ClassName) && ischool_ClassNameIDs.ContainsKey(vCourseExtension.ClassName))
                                    vCourseExtension.ClassID = K12.Data.Int.Parse(ischool_ClassNameIDs[vCourseExtension.ClassName]);
                                else
                                    vCourseExtension.ClassID = null;
                            }
                            //級別
                            if (mOption.SelectedFields.Contains(constLevel))
                                vCourseExtension.Level = K12.Data.Int.ParseAllowNull(Row.GetValue(constLevel));
                            //節數
                            if (mOption.SelectedFields.Contains(constPeriod))
                                vCourseExtension.Period = K12.Data.Int.ParseAllowNull(Row.GetValue(constPeriod));
                            //學分數
                            if (mOption.SelectedFields.Contains(constCredit))
                                vCourseExtension.Credit = K12.Data.Int.ParseAllowNull(Row.GetValue(constCredit));
                            //預設授課教師一
                            if (mOption.SelectedFields.Contains(constTeacherName一))
                                vCourseExtension.TeacherName1 = Row.GetValue(constTeacherName一);
                            //預設授課教師二
                            if (mOption.SelectedFields.Contains(constTeacherName二))
                                vCourseExtension.TeacherName2 = Row.GetValue(constTeacherName二);
                            //預設授課教師三
                            if (mOption.SelectedFields.Contains(constTeacherName三))
                                vCourseExtension.TeacherName3 = Row.GetValue(constTeacherName三);

                            //開放查詢
                            if (mOption.SelectedFields.Contains(constCanQuery))
                                vCourseExtension.NoQuery = Row.GetValue(constCanQuery).Equals("開放") ? false : true;

                            //領域
                            if (mOption.SelectedFields.Contains(constDomain))
                                vCourseExtension.Domain = Row.GetValue(constDomain);

                            //分項
                            if (mOption.SelectedFields.Contains(constEntry))
                                vCourseExtension.Entry = Row.GetValue(constEntry);

                            //校部訂
                            if (mOption.SelectedFields.Contains(constRequiredBy))
                                vCourseExtension.RequiredBy = Row.GetValue(constRequiredBy);

                            //必選修
                            if (mOption.SelectedFields.Contains(constRequired))
                                vCourseExtension.Required = Row.GetValue(constRequired);

                            //不計入學分
                            if (mOption.SelectedFields.Contains(constNotIncludedInCredit))
                                vCourseExtension.NotIncludedInCredit = Row.GetValue(constNotIncludedInCredit).Equals("不計入") ? true : false;

                            //不評分
                            if (mOption.SelectedFields.Contains(constNotIncludedInCalc))
                                vCourseExtension.NotIncludedInCalc = Row.GetValue(constNotIncludedInCalc).Equals("不評分") ? true : false;

                            //學期成績
                            if (mOption.SelectedFields.Contains(constCalculationFlag))
                                vCourseExtension.CalculationFlag = Row.GetValue(constCalculationFlag).Equals("不列入") ? "否" : "是";
                            #endregion

                            #region 課程排課資料屬性
                            //時間表名稱
                            if (mOption.SelectedFields.Contains(constTimeTableName))
                            {
                                string vTimeTableName = Row.GetValue(constTimeTableName);

                                TimeTable vTimeTable = mImportTimeTableHelper[vTimeTableName];

                                if (vTimeTable != null)
                                    vCourseExtension.TimeTableID = K12.Data.Int.ParseAllowNull(vTimeTable.UID);
                            }

                            //允許重覆
                            if (mOption.SelectedFields.Contains(constAllowDup))
                                vCourseExtension.AllowDup = Row.GetValue(constAllowDup).Equals("是") ? true : false;

                            //課程分段分割設定
                            if (mOption.SelectedFields.Contains(constSplitSpec))
                                vCourseExtension.SplitSpec = Row.GetValue(constSplitSpec);

                            //隔天排課
                            if (mOption.SelectedFields.Contains(constLimitNextDay))
                                vCourseExtension.LimitNextDay = Row.GetValue(constLimitNextDay).Equals("是") ? true : false;
                            #endregion

                            #region 課程分段預設屬性
                            //課程分段預設星期條件
                            if (mOption.SelectedFields.Contains(constWeekdayCond))
                                vCourseExtension.WeekDayCond = Row.GetValue(constWeekdayCond);

                            //課程分段預設節次條件
                            if (mOption.SelectedFields.Contains(constPeriodCond))
                                vCourseExtension.PeriodCond = Row.GetValue(constPeriodCond);

                            //課程分段預設單雙週
                            if (mOption.SelectedFields.Contains(constWeekFlag))
                            {
                                string strWeekFalg = Row.GetValue(constWeekFlag);
                                vCourseExtension.WeekFlag = Utility.GetWeekFlagInt(strWeekFalg);
                            }

                            //課程分段預設跨中午
                            if (mOption.SelectedFields.Contains(constLongBreak))
                                vCourseExtension.LongBreak = Row.GetValue(constLongBreak).Equals("是") ? true : false;

                            //課程分段預設場地
                            if (mOption.SelectedFields.Contains(constClassroom))
                            {
                                string vClassroomName = Row.GetValue(constClassroom);

                                Classroom vClassroom = mImportClassroomHelper[vClassroomName];

                                if (vClassroom != null)
                                    vCourseExtension.ClassroomID = K12.Data.Int.ParseAllowNull(vClassroom.UID);
                            }
                            #endregion

                            InsertRecords.Add(vCourseExtension);
                        }
                        #endregion
                    }
                    #endregion

                    #region Step3:實際新增或更新資料
                    if (InsertRecords.Count > 0)
                    {
                        List<string> NewIDs = mHelper.InsertValues(InsertRecords);

                        SunsetBL.CreateSchedulerCourseSectionByCourseIDs(NewIDs);


                        //在新增完後不需更新mCourseExtensions變數，因為來源資料不允許相同的課程重覆做新增
                    }
                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);

                        SunsetBL.CreateSchedulerCourseSectionByCourseIDs(UpdateRecords.Select(x => x.UID));


                        //在更新完後不需更新mCourseExtensions變數，因為來源資料不允許相同的課程重覆做更新
                    }
                    #endregion

                    #region Log
                    StringBuilder mstrLog = new StringBuilder();
                    if (InsertRecords.Count > 0)
                    {
                        mstrLog.AppendLine("已新增 " + InsertRecords.Count + " 筆排課課程資料：");
                        foreach (SchedulerCourseExtension each in InsertRecords)
                        {
                            mstrLog.AppendLine(string.Format("學年度「{0}」學期「{1}」課程名稱「{2}」", each.SchoolYear.ToString(), each.Semester, each.CourseName));
                        }
                    }

                    if (UpdateRecords.Count > 0)
                    {
                        mstrLog.AppendLine("已更新 " + UpdateRecords.Count + " 筆排課課程資料：");
                        foreach (SchedulerCourseExtension each in InsertRecords)
                        {
                            mstrLog.AppendLine(string.Format("學年度「{0}」學期「{1}」課程名稱「{2}」", each.SchoolYear.ToString(), each.Semester, each.CourseName));
                        }
                    }

                    if (InsertRecords.Count > 0 || UpdateRecords.Count > 0)
                    {
                        FISCA.LogAgent.ApplicationLog.Log("排課", "匯入排課課程", mstrLog.ToString());
                    }
                    #endregion

                }
                else if (mOption.Action == ImportAction.Delete)
                {
                    #region 刪除資料
                    ////要刪除的排課課程資料
                    //List<CourseExtension> DeleteRecords = new List<CourseExtension>();

                    ////針對每筆記錄
                    //foreach (IRowStream Row in Rows)
                    //{
                    //    //取得鍵值為課程名稱、學年度及學期
                    //    string CourseName = Row.GetValue(constCourseName);
                    //    string SchoolYear = Row.GetValue(constSchoolYear);
                    //    string Semester = Row.GetValue(constSemester);

                    //    //根據課程名稱、學年度及學期尋找是否有對應的課程
                    //    string CourseKey = CourseName + "," + SchoolYear + "," + Semester;
                    //    string CourseID = mCourseNameIDs.ContainsKey(CourseKey) ? mCourseNameIDs[CourseKey] : string.Empty;

                    //    //若有找到課程資料
                    //    if (!string.IsNullOrEmpty(CourseID))
                    //    {
                    //        //尋找是否有對應的排課課程資料
                    //        CourseExtension vCourseExtension = mCourseExtensions
                    //            .Find(x => x.CourseID.Equals(K12.Data.Int.Parse(CourseID)));
                    //        //若有找到則加入到刪除的集合中
                    //        if (vCourseExtension != null)
                    //            DeleteRecords.Add(vCourseExtension);
                    //    }
                    //}

                    ////若是要刪除的集合大於0才執行
                    //if (DeleteRecords.Count > 0)
                    //{
                    //    //mHelper.DeletedValues(DeleteRecords);                      
                    //    //mstrLog.AppendLine("已刪除"+DeleteRecords.Count+"筆排課課程資料。");
                    //    //在刪除完後不需更新mCourseExtensions變數，因為來源資料不允許相同的課程重覆做刪除
                    //}
                    #endregion
                }
            }

            return "";
        }
    }
}