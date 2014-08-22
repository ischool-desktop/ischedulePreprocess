using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Campus.DocumentValidator;
using Campus.Import;
using FISCA.Data;
using FISCA.UDT;
using K12.Data;

namespace Sunset
{
    /// <summary>
    /// 匯入排課課程資料
    /// </summary>
    public class ImportCourseExtension : ImportWizard
    {
        private const string constCourseName="課程名稱";
        private const string constCourseAliasName = "科目簡稱";
        private const string constSchoolYear="學年度";
        private const string constSemester="學期";
        //private const string constPeriod = "節數";
        //private const string constTeacherName = "授課教師一";
        private const string constTimeTableName = "上課時間表";
        private const string constAllowDup = "同天排課";
        private const string constLimitNextDay = "不連天排課";
        private const string constSplitSpec = "分割設定";
        private const string constClassroom = "預設場地條件";
        private const string constWeekdayCond = "預設星期條件";
        private const string constPeriodCond = "預設節次條件";
        private const string constLongBreak = "預設跨中午條件";
        private const string constWeekFlag = "預設單雙週條件";
        private StringBuilder mstrLog = new StringBuilder();
        private ImportOption mOption;
        private Dictionary<string, string> mCourseNameIDs;
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
        /// 取得驗證規則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return "http://sites.google.com/a/kunhsiang.com/sunset/home/yan-zheng-gui-ze/CourseExtension.xml";
        }

        /// <summary>
        /// 取得支援的匯入型態
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.Update | ImportAction.Delete;
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
            mCourseNameIDs = new Dictionary<string,string>();
            mTask = Task.Factory.StartNew
            (() =>
                {
                    QueryHelper Helper = new QueryHelper();

                    DataTable Table = Helper.Select("select id,course_name,school_year,semester from course");

                    foreach (DataRow Row in Table.Rows)
                    {
                        string CourseID = Row.Field<string>("id");
                        string CourseName = Row.Field<string>("course_name");
                        string SchoolYear = Row.Field<string>("school_year");
                        string Semester = Row.Field<string>("semester");
                        string CourseKey = CourseName + "," + SchoolYear + "," + Semester;

                        if (!mCourseNameIDs.ContainsKey(CourseKey))
                            mCourseNameIDs.Add(CourseKey, CourseID);
                    }
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

            mstrLog.Clear();

            if (mOption.SelectedKeyFields.Count == 3 && 
                mOption.SelectedKeyFields.Contains(constSchoolYear) && 
                mOption.SelectedKeyFields.Contains(constSemester) && 
                mOption.SelectedKeyFields.Contains(constCourseName))
            {
                #region 取得已存在的排課課程資料
                List<CourseExtension> mCourseExtensions = new List<CourseExtension>();
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
                mCourseExtensions = mHelper.Select<CourseExtension>("ref_course_id in (" + CourseIDsCondition + ")");
                //List<CourseRecord> mCourses = K12.Data.Course.SelectByIDs(CourseIDs);
                List<TCInstructRecord> mTCInstructs = K12.Data.TCInstruct.SelectByTeacherIDAndCourseID(new List<string>(), CourseIDs);
                #endregion

                if (mOption.Action == ImportAction.Update)
                {
                    #region Step1:針對每筆匯入每筆資料檢查，若時間表及場地不存在，則自動新增
                    List<TimeTable> TimeTables = mImportTimeTableHelper.Insert(Rows);
                    string strTimeTableMessage = ImportTimeTableHelper.GetInsertMessage(TimeTables);
                    if (!string.IsNullOrEmpty(strTimeTableMessage))
                        mstrLog.AppendLine(strTimeTableMessage);

                    List<Classroom> Classrooms = mImportClassroomHelper.Insert(Rows,constClassroom);
                    string strClassroomMessage = ImportClassroomHelper.GetInsertMessage(Classrooms);
                    if (!string.IsNullOrEmpty(strClassroomMessage))
                        mstrLog.AppendLine(strClassroomMessage);
                    #endregion

                    #region Step2:針對每筆匯入每筆資料檢查，判斷是新增或是更新
                    List<CourseExtension> InsertRecords = new List<CourseExtension>();
                    List<CourseExtension> UpdateRecords = new List<CourseExtension>();
                    //List<CourseRecord> UpdateCourses = new List<CourseRecord>();
                    List<TCInstructRecord> InsertTCInstructs = new List<TCInstructRecord>();
                    List<TCInstructRecord> UpdateTCInstructs = new List<TCInstructRecord>();

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

                        if (CourseID.HasValue)
                        {
                            //尋找是否有對應的課程排課資料
                            CourseExtension vCourseExtension = mCourseExtensions
                                .Find(x=>x.CourseID.Equals(CourseID));

                            //CourseRecord vCourse = mCourses.Find(x => x.ID.Equals(""+CourseID));

                            #region 更新主要授課教師
                            //if (mOption.SelectedFields.Contains(constTeacherName))
                            //{
                            //    //根據『教師姓名』及『教師暱稱』尋找是否有對應的『教師』
                            //    string TeacherKey = Row.GetValue(constTeacherName);
                            //    string TeacherID = mTeacherNameIDs.ContainsKey(TeacherKey) ? mTeacherNameIDs[TeacherKey] : string.Empty;

                            //    //若有找到『教師』才可匯入
                            //    if (!string.IsNullOrEmpty(TeacherID))
                            //    {
                            //        TCInstructRecord vTCInstruct = mTCInstructs
                            //            .Find(x => x.RefCourseID.Equals("" + CourseID) && x.Sequence == 1);

                            //        if (vTCInstruct != null)
                            //        {
                            //            vTCInstruct.RefTeacherID = TeacherID;
                            //            UpdateTCInstructs.Add(vTCInstruct);
                            //        }
                            //        else
                            //        {
                            //            vTCInstruct = new TCInstructRecord();
                            //            vTCInstruct.RefCourseID = K12.Data.Int.GetString(CourseID);
                            //            vTCInstruct.RefTeacherID = TeacherID;
                            //            vTCInstruct.Sequence = 1;
                            //            InsertTCInstructs.Add(vTCInstruct);
                            //        }
                            //    }
                            //}
                            #endregion

                            //更新Course屬性
                            //if (vCourse != null && mOption.SelectedFields.Contains(constPeriod))
                            //{
                            //    vCourse.Period = K12.Data.Decimal.ParseAllowNull(Row.GetValue(constPeriod));
                            //    //UpdateCourses.Add(vCourse);
                            //}

                            #region 更新CourseExtension
                            if (vCourseExtension != null)
                            {
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

                                //課程簡稱
                                if (mOption.SelectedFields.Contains(constCourseAliasName))
                                    vCourseExtension.SubjectAliasName = Row.GetValue(constCourseAliasName);

                                //課程分段分割設定
                                if (mOption.SelectedFields.Contains(constSplitSpec))
                                    vCourseExtension.SplitSpec = Row.GetValue(constSplitSpec);

                                //允許重覆
                                if (mOption.SelectedFields.Contains(constAllowDup))
                                    vCourseExtension.AllowDup = Row.GetValue(constAllowDup).Equals("是")?true:false;

                                //隔天排課
                                if (mOption.SelectedFields.Contains(constLimitNextDay))
                                    vCourseExtension.LimitNextDay = Row.GetValue(constLimitNextDay).Equals("是") ? true : false;
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

                                    if (vClassroom!=null)
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
                                vCourseExtension = new CourseExtension();
                                vCourseExtension.CourseID = CourseID.Value;
                                vCourseExtension.SubjectAliasName = string.Empty;
                                vCourseExtension.WeekFlag = 3; //預設的單雙週條件
                                vCourseExtension.AllowDup = false;
                                vCourseExtension.LimitNextDay = false;
                                vCourseExtension.LongBreak = false;
                                vCourseExtension.ClassroomID = null;
                                vCourseExtension.TimeTableID = null;
                                vCourseExtension.WeekDayCond = string.Empty;
                                vCourseExtension.PeriodCond = string.Empty;
                                vCourseExtension.SplitSpec = string.Empty;

                                #region 課程排課資料屬性
                                //時間表名稱
                                if (mOption.SelectedFields.Contains(constTimeTableName))
                                {
                                    string vTimeTableName = Row.GetValue(constTimeTableName);

                                    TimeTable vTimeTable = mImportTimeTableHelper[vTimeTableName];

                                    if (vTimeTable != null)
                                        vCourseExtension.TimeTableID = K12.Data.Int.ParseAllowNull(vTimeTable.UID);
                                }

                                //課程簡稱
                                if (mOption.SelectedFields.Contains(constCourseAliasName))
                                    vCourseExtension.SubjectAliasName = Row.GetValue(constCourseAliasName);

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
                    }
                    #endregion

                    #region Step3:實際新增或更新資料
                    if (InsertRecords.Count > 0)
                    {
                        List<string> NewIDs = mHelper.InsertValues(InsertRecords);
                        mstrLog.AppendLine("已新增" + InsertRecords.Count + "筆排課課程資料。");
                        //在新增完後不需更新mCourseExtensions變數，因為來源資料不允許相同的課程重覆做新增
                    }
                    if (UpdateRecords.Count > 0)
                    {
                        mHelper.UpdateValues(UpdateRecords);
                        mstrLog.AppendLine("已更新" + UpdateRecords.Count + "筆排課課程資料。");
                        //在更新完後不需更新mCourseExtensions變數，因為來源資料不允許相同的課程重覆做更新
                    }
                    //if (UpdateCourses.Count > 0)
                    //{
                    //    //int UpdateCount = Course.Update(UpdateCourses);
                    //    //mstrLog.AppendLine("已新增" + UpdateCount + "筆課程資料。");
                    //    //在新增完後不需更新mCourseExtensions變數，因為來源資料不允許相同的課程重覆做新增
                    //}
                    if (InsertTCInstructs.Count > 0)
                    {
                        List<string> NewIDs = TCInstruct.Insert(InsertTCInstructs);
                        mstrLog.AppendLine("已新增" + NewIDs.Count + "筆課程授主要課教師");
                    }
                    if (UpdateTCInstructs.Count > 0)
                    {
                        int UpdateCount = TCInstruct.Update(UpdateTCInstructs);
                        mstrLog.AppendLine("已更新" + UpdateCount + "筆課程主要授課教師");
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

            return mstrLog.ToString();
        }
    }
}