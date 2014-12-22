//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Threading.Tasks;
//using Campus.Windows;
//using FISCA.Data;
//using FISCA.Presentation;
//using FISCA.UDT;

//namespace ischedulePlus
//{
//    /// <summary>
//    /// 批次建立週課程分段
//    /// </summary>
//    public partial class frmCreateWeekCourseSection : FISCA.Presentation.Controls.BaseForm
//    {
//        private List<SchoolYearSemesterDate> mSchoolSemesterDates;
//        private SchoolYearSemesterDate mSelectSchoolYearSemester;
//        private AccessHelper mUDTHelper = new AccessHelper();
//        private QueryHelper mQueryHelper = new QueryHelper();
//        private const int PackageSize = 500;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        public frmCreateWeekCourseSection()
//        {
//            InitializeComponent();
//        }

//        /// <summary>
//        /// 表單載入資料
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void frmCreateWeekCourseSection_Load(object sender, EventArgs e)
//        {
//            Task vTask = Task.Factory.StartNew
//            ( () =>
//             {
//                 mSchoolSemesterDates = mUDTHelper.Select<SchoolYearSemesterDate>();
//             }
//            );

//            vTask.ContinueWith(x =>
//            {
//                mSchoolSemesterDates.ForEach
//                (y =>
//                    {
//                        lstSchoolYearSemesterDate.Items
//                            .Add("學年度學期：" + y.SchoolYear + y.Semester + "  日期：" + y.StartDate.ToString("yyyy-MM-dd") + " 至 " + y.EndDate.ToString("yyyy-MM-dd"));

//                        btnConfirm.Enabled = true;
//                        btnClose.Enabled = true;
//                    }
//                );

//            },TaskScheduler.FromCurrentSynchronizationContext());
//        }

//        /// <summary>
//        /// 批次刪除週課程分段
//        /// </summary>
//        /// <param name="CourseIDs"></param>
//        private void DeleteWeekCourseSection(List<string> CourseIDs)
//        {
//            string Query = "ref_course_id in (" + string.Join(",", CourseIDs.ToArray())+ ")";
//            //取得指定區間週課程分段
//            List<WeekCourseSection> WeekCourseSections = mUDTHelper.Select<WeekCourseSection>(Query);
//            //刪除指定區間週課程分段
//            mUDTHelper.DeletedValues(WeekCourseSections);
//        }

//        /// <summary>
//        /// 確認產生週課程分課
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void btnConfirm_Click(object sender, EventArgs e)
//        {
//            //判斷是否有選擇學年度及學期
//            if (lstSchoolYearSemesterDate.SelectedIndex < 0)
//            {
//                MsgBox.Show("請選擇指定的學年度及學期！");
//                return;
//            }

//            //取得使用者選取的學年度學期
//            mSelectSchoolYearSemester = mSchoolSemesterDates[lstSchoolYearSemesterDate.SelectedIndex];

//            //判斷開始日期是否大於等於結束日期
//            if (mSelectSchoolYearSemester.StartDate >= mSelectSchoolYearSemester.EndDate)
//            {
//                MsgBox.Show("開始日期大於等於結束日期，無法產生！");
//                return;
//            }

//            //向使用者確認是否要繼續執行
//            if (MsgBox.Show("執行時會先清空指定學年度學期的週課表再新增，是否繼續執行？", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
//                return;

//            BackgroundWorker worker = new BackgroundWorker();

//            worker.DoWork += (vsender, ve) =>
//            {
//                #region 取得指定學年度學期的課程系統編號
//                worker.ReportProgress(0, "取得學期課程分段中！");

//                string strQuery = "select id from course where school_year='" + mSelectSchoolYearSemester.SchoolYear + "' and semester='" + mSelectSchoolYearSemester.Semester + "'";

//                DataTable CourseIDTable = mQueryHelper.Select(strQuery);

//                List<string> CourseIDs = new List<string>();

//                foreach (DataRow Row in CourseIDTable.Rows)
//                    CourseIDs.Add("'" + Row.Field<string>("id") + "'");
//                #endregion

//                #region 刪除指定日期區間的週課程分段
//                worker.ReportProgress(0, "刪除現有『週』課程分段中！");                

//                //FunctionSpliter<string, string> DeleteSpliter = new FunctionSpliter<string, string>(2000, 3);

//                //DeleteSpliter.Function = (x) =>
//                //{
//                //    string Query = "ref_course_id in (" + string.Join(",", x.ToArray()) + ")";

//                //    //取得指定區間週課程分段
//                //    List<WeekCourseSection> WeekCourseSections = mUDTHelper.Select<WeekCourseSection>(Query);

//                //    //刪除指定區間週課程分段
//                //    mUDTHelper.DeletedValues(WeekCourseSections);

//                //    return new List<string>();
//                //};

//                //List<string> DeleteResult = DeleteSpliter.Execute(CourseIDs);

//                List<string> PackageCourseIDs = new List<string>();

//                for (int i = 0; i < CourseIDs.Count; i++)
//                {
//                    PackageCourseIDs.Add(CourseIDs[i]);

//                    if ((i > 0) && (i % PackageSize == 0))
//                    {
//                        DeleteWeekCourseSection(PackageCourseIDs);
//                        PackageCourseIDs.Clear();
//                    }

//                    worker.ReportProgress((int)(((float)i / (float)CourseIDs.Count) * 100),"刪除現有『週』課程分段中！");
//                }

//                if (PackageCourseIDs.Count > 0)
//                {
//                    DeleteWeekCourseSection(PackageCourseIDs);
//                    worker.ReportProgress(100,"已刪除現有『週』課程分段中！");
//                }
//                #endregion

//                #region 取得指定課程的課程分段，並產生週課程分段

//                //根據課程系統編號取得『已排課』的課程分段
//                FunctionSpliter<string, CourseSection> CourseSpliter = new FunctionSpliter<string, CourseSection>(1000, 3);

//                CourseSpliter.Function = (x) =>
//                {
//                    string Query = "ref_course_id in (" + string.Join(",", x.ToArray()) + ") and weekday<>0 and period<>0";

//                    List<CourseSection> vCourseSections = mUDTHelper.Select<CourseSection>(Query);

//                    return vCourseSections;
//                };
                
//                List<CourseSection> CourseSections = CourseSpliter.Execute(CourseIDs);

//                worker.ReportProgress(0, "批次產生週課程分段中！");

//                //要新增的課程分段集合
//                List<WeekCourseSection> NewWeekCourseSections = new List<WeekCourseSection>();

//                //開始日期的初始週日
//                DateTime StartWeekdayDate = mSelectSchoolYearSemester.StartDate.StartOfWeek(DayOfWeek.Sunday);
//                //結束日期的初始週日   
//                DateTime EndWeekdayDate = mSelectSchoolYearSemester.EndDate.StartOfWeek(DayOfWeek.Sunday);

//                for (int i = 0; i < CourseSections.Count; i++)
//                {
//                    CourseSection CurrentCourseSection = CourseSections[i];

//                    int CurrentWeekday = 1;

//                    //判斷開始日期的該週是否要產生課程分段
//                    if (CurrentCourseSection.WeekDay >= (int)mSelectSchoolYearSemester.StartDate.DayOfWeek)
//                    {
//                        NewWeekCourseSections.Add(CurrentCourseSection.ToWeekCourseSection(1,StartWeekdayDate.AddDays(CurrentCourseSection.WeekDay)));
//                        CurrentWeekday ++;
//                    }

//                    DateTime CurrentWeekdayDate = StartWeekdayDate.AddDays(7);

//                    //判斷中間週要產生多少課程分段
//                    while(CurrentWeekdayDate < EndWeekdayDate)
//                    {
//                        NewWeekCourseSections
//                            .Add(CurrentCourseSection.ToWeekCourseSection(CurrentWeekday,CurrentWeekdayDate.AddDays(CurrentCourseSection.WeekDay)));
//                        CurrentWeekdayDate = CurrentWeekdayDate.AddDays(7);
//                        CurrentWeekday++;
//                    }

//                    //判斷結束日期的該週是否要產生課程分段
//                    if (CurrentCourseSection.WeekDay <= (int)mSelectSchoolYearSemester.EndDate.DayOfWeek)
//                        NewWeekCourseSections.Add(CurrentCourseSection.ToWeekCourseSection(CurrentWeekday,EndWeekdayDate.AddDays(CurrentCourseSection.WeekDay)));

//                    worker.ReportProgress((int)(((float)i / (float)CourseSections.Count) * 100), "批次產生週課程分段中！");
//                }
//                #endregion

//                #region 實際新增週課程分段
//                //FunctionSpliter<WeekCourseSection, string> Spliter = new FunctionSpliter<WeekCourseSection, string>(1000, 5);

//                //Spliter.Function = x => mUDTHelper.InsertValues(x);

//                //worker.ReportProgress(0, "新增週課程分段中！");

//                //Spliter.ProgressChange = x => Invoke(new Action(() => worker.ReportProgress(x, "新增週課程分段中！")));

//                //List<string> NewIDs = Spliter.Execute(NewWeekCourseSections);

//                List<string> NewIDs = new List<string>();

//                List<WeekCourseSection> PackageRecords = new List<WeekCourseSection>();

//                for (int i = 0; i < NewWeekCourseSections.Count; i++)
//                {
//                    PackageRecords.Add(NewWeekCourseSections[i]);

//                    if ((i > 0) && (i % PackageSize == 0))
//                    {
//                        NewIDs.AddRange(mUDTHelper.InsertValues(PackageRecords));
//                        PackageRecords.Clear();
//                    }

//                    worker.ReportProgress((int)(((float)i / (float)NewWeekCourseSections.Count) * 100),"新增週課程分段中！");
//                }

//                if (PackageCourseIDs.Count > 0)
//                {
//                    NewIDs.AddRange(mUDTHelper.InsertValues(PackageRecords));
//                    worker.ReportProgress( 100,"新增週課程分段已完成！");
//                }
//                #endregion

//                ve.Result = NewIDs;
//            };

//            worker.ProgressChanged += (vsender, ve) => MotherForm.SetStatusBarMessage(""+ve.UserState,ve.ProgressPercentage);

//            worker.RunWorkerCompleted += (vsender, ve) =>
//            {
//                btnClose.Enabled = true;
//                btnConfirm.Enabled = true;
//                btnExport.Enabled = true;

//                MotherForm.SetStatusBarMessage("已成功新增" + (ve.Result as List<string>).Count + "筆週課程分段！");
//            };

//            worker.WorkerReportsProgress = true;

//            worker.RunWorkerAsync();

//            btnClose.Enabled = false;
//            btnConfirm.Enabled = false;
//            btnExport.Enabled = false;
//        }

//        /// <summary>
//        /// 匯出已產生的課程分段
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void btnExport_Click(object sender, EventArgs e)
//        {
//            //判斷是否有選擇學年度及學期
//            if (lstSchoolYearSemesterDate.SelectedIndex < 0)
//            {
//                MsgBox.Show("請選擇指定的學年度及學期！");
//                return;
//            }

//            //取得使用者選取的學年度學期
//            mSelectSchoolYearSemester = mSchoolSemesterDates[lstSchoolYearSemesterDate.SelectedIndex];

//            //匯出週課程分段
//            ExportSunset.ExportWeekdayCourseSection(mSelectSchoolYearSemester.StartDate, mSelectSchoolYearSemester.EndDate);
//        }
//    }
//}