using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.Presentation;
using K12.Data;
using FISCA.Data;

namespace Sunset.NewCourse
{
    public partial class CopyCourseIn_ischool : BaseForm
    {
        /// <summary>
        /// 資料準備
        /// </summary>
        BackgroundWorker BGW = new BackgroundWorker();

        /// <summary>
        /// 開始複製
        /// </summary>
        BackgroundWorker StartBGW = new BackgroundWorker();

        /// <summary>
        /// 使用者所選擇要複製的課程
        /// </summary>
        List<SchedulerCourseExtension> Courses = new List<SchedulerCourseExtension>();

        List<K12CourseRecord> NewK12Courses = new List<K12CourseRecord>();

        List<SimpleCourse> NewCourses { get; set; }
        List<string> ExistCourseNames { get; set; }
        Dictionary<string, string> mClassNameIDs { get; set; }

        /// <summary>
        /// 複製課程回ischool
        /// </summary>
        public CopyCourseIn_ischool()
        {
            InitializeComponent();
        }

        private void CopyCourseIn_ischool_Load(object sender, EventArgs e)
        {
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);
            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);

            StartBGW.DoWork += new DoWorkEventHandler(StartBGW_DoWork);
            StartBGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(StartBGW_RunWorkerCompleted);
            StartBGW.ProgressChanged += new ProgressChangedEventHandler(StartBGW_ProgressChanged);
            StartBGW.WorkerReportsProgress = true;

            btnStart.Enabled = false;
            this.Text = "複製課程回ischool(資料取得中...)";
            BGW.RunWorkerAsync();
        }

        void StartBGW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage(e.UserState.ToString(), e.ProgressPercentage);
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            //根據選取的課程系統編號取得課程排課資料
            List<string> CourseIDs = CourseAdmin.Instance.SelectedSource;
            string strCondition = string.Join(",", CourseAdmin.Instance.SelectedSource.ToArray());
            Courses.Clear();
            Courses = tool._A.Select<SchedulerCourseExtension>("uid in (" + strCondition + ")");
            Courses.Sort(SortCourse);
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = "複製課程回ischool";
            btnStart.Enabled = true;
            labelX2.Text = string.Format("已選擇「{0}」筆課程資料", Courses.Count.ToString());

            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    foreach (SchedulerCourseExtension each in Courses)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.Tag = each;
                        row.CreateCells(dataGridViewX1);
                        row.Cells[0].Value = each.SchoolYear.ToString();
                        row.Cells[1].Value = each.Semester;
                        row.Cells[2].Value = each.CourseName;
                        dataGridViewX1.Rows.Add(row);
                    }
                }
                else
                {
                    MsgBox.Show("資料取得發生錯誤!!\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("作業已取消!!");
            }
        }

        /// <summary>
        /// 開始複製作業
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!StartBGW.IsBusy)
            {
                this.Text = "複製課程回ischool(開始複製...)";
                btnStart.Enabled = false;
                StartBGW.RunWorkerAsync();
            }
            else
            {
                MsgBox.Show("系統忙碌中,稍後再試!");
            }
        }

        void StartBGW_DoWork(object sender, DoWorkEventArgs e)
        {
            Log_Eng log = new Log_Eng();

            StartBGW.ReportProgress(0, "取得班級學生清單");
            //取得所有班級並依班級名稱排序
            mClassNameIDs = RepObj.GetClassList();

            StartBGW.ReportProgress(0, "建立課程鍵值清單");
            //取得Key值的組合清單 (課程名稱 / 學年度 / 學期)
            ExistCourseNames = RepObj.GetExistCourseNames(Courses);

            StartBGW.ReportProgress(0,"建立課程複製清單");
            //取得複製回ischool課程的組合物件
            NewK12Courses = RepObj.GetK12CourseOBJ(Courses, ExistCourseNames, mClassNameIDs);

            //如果新增課程為0,那麼就離開吧!~
            if (K12.Data.Utility.Utility.IsNullOrEmpty(NewK12Courses))
            {
                StartBGW.ReportProgress(100, "複製課程為0,已取消作業...");
                e.Cancel = true;
                return;
            }

            //建立Spliter上傳課程資料
            StartBGW.ReportProgress(0, "開始上傳課程資料...");

            List<string> NewIDs = K12.Data.Course.Insert(NewK12Courses);

            StartBGW.ReportProgress(100, "已上傳課程資料!");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty((NewIDs)))
            {
                #region 加入教師教授課程
                List<TCInstructRecord> TCInstructs = new List<TCInstructRecord>();
                List<TeacherRecord> TeacherRecords = Teacher
                    .SelectAll()
                    .FindAll(x => x.Status == TeacherRecord.TeacherStatus.一般);
                NewCourses = new List<SimpleCourse>();

                //選出新的課程系統編號、課程名稱、學年度及學期
                List<string> strConditions = NewK12Courses
                       .Select(x => "(school_year=" + K12.Data.Int.GetString(x.SchoolYear) + " and semester=" + K12.Data.Int.GetString(x.Semester) +")")
                       .Distinct()
                       .ToList();

                string strSQL = "select id,course_name,school_year,semester,ref_class_id from course where " + string.Join(" or ",strConditions.ToArray());

                DataTable tblCourse = tool._Q.Select(strSQL);

                foreach (DataRow Row in tblCourse.Rows)
                {
                    string CourseID = Row.Field<string>("id");
                    string CourseName = Row.Field<string>("course_name");
                    string SchoolYear = Row.Field<string>("school_year");
                    string Semester = Row.Field<string>("semester");
                    string ClassID = Row.Field<string>("ref_class_id");

                    if (NewK12Courses.Find(x =>
                        x.Name.Equals(CourseName) &&
                        K12.Data.Int.GetString(x.SchoolYear).Equals(SchoolYear) &&
                        K12.Data.Int.GetString(x.Semester).Equals(Semester)) != null)
                    {
                        SimpleCourse SimpleCourse = new SimpleCourse();

                        SimpleCourse.ID = Row.Field<string>("id");
                        SimpleCourse.CourseName = Row.Field<string>("course_name");
                        SimpleCourse.SchoolYear = Row.Field<string>("school_year");
                        SimpleCourse.Semester = Row.Field<string>("semester");
                        SimpleCourse.RefClassID = Row.Field<string>("ref_class_id");

                        NewCourses.Add(SimpleCourse); 
                    }
                }

                //將新增課程的課程ID,回填至複製之功能(2013/8/14)
                List<SchedulerCourseExtension> Save_List = new List<SchedulerCourseExtension>();

                int Progress = 0;
                int ProgressPercent = 0;

                foreach (SchedulerCourseExtension lowCourse in Courses)
                {
                    SimpleCourse newCourse = NewCourses.Find(x =>
                        x.CourseName.Equals(lowCourse.CourseName) &&
                        x.SchoolYear.Equals("" + lowCourse.SchoolYear) &&
                        x.Semester.Equals("" + lowCourse.Semester));

                    if (newCourse != null)
                    {
                        lowCourse.CourseID = K12.Data.Int.ParseAllowNull(newCourse.ID);
                        Save_List.Add(lowCourse); 
                    }

                    Progress++;
                    ProgressPercent = (int)((float)Progress /(float)Courses.Count * 100);
                    StartBGW.ReportProgress(ProgressPercent,"更新排課課程對應ischool編號");
                }

                StartBGW.ReportProgress(0, "更新排課課程中！");

                tool._A.UpdateValues(Save_List);

                StartBGW.ReportProgress(100, "已更新排課課程！");

                log.SetCourseLog(NewCourses);

                Progress = 0;
                ProgressPercent = 0;

                //針對每筆找出課程
                foreach (SimpleCourse SimpleCourse in NewCourses)
                {
                    string CourseID = SimpleCourse.ID;
                    string CourseName = SimpleCourse.CourseName;
                    string SchoolYear = SimpleCourse.SchoolYear;
                    string Semester = SimpleCourse.Semester;

                    //取得符合條件的排課課程
                    //1.課程名稱相同
                    //2.學年度相同
                    //3.學期相同
                    SchedulerCourseExtension Course = Courses
                        .Find(x => x.CourseName.Equals(CourseName) && 
                            ("" + x.SchoolYear).Equals(SchoolYear) && 
                            ("" + x.Semester).Equals(Semester));

                    //若有找到對應的課程
                    if (Course != null)
                    {
                        #region 建立課程授課教師一
                        if (!string.IsNullOrEmpty(Course.TeacherName1))
                        {
                            TeacherRecord vTeacher = TeacherRecords.Find(x => Course.TeacherName1.Equals(string.IsNullOrEmpty(x.Nickname) ? x.Name : x.Name + "(" + x.Nickname + ")"));

                            if (vTeacher != null)
                            {
                                TCInstructRecord TCInstruct = new TCInstructRecord(vTeacher.ID, CourseID, 1);
                                TCInstructs.Add(TCInstruct);
                            }
                        }
                        #endregion

                        #region 建立課程授課教師二
                        if (!string.IsNullOrEmpty(Course.TeacherName2))
                        {
                            TeacherRecord vTeacher = TeacherRecords
                                 .Find(x => Course.TeacherName2.Equals(string.IsNullOrEmpty(x.Nickname) ? x.Name : x.Name + "(" + x.Nickname + ")"));

                            if (vTeacher != null)
                            {
                                TCInstructRecord TCInstruct = new TCInstructRecord(vTeacher.ID, CourseID, 2);
                                TCInstructs.Add(TCInstruct);
                            }
                        }
                        #endregion

                        #region 建立課程授課教師二
                        if (!string.IsNullOrEmpty(Course.TeacherName3))
                        {
                            TeacherRecord vTeacher = TeacherRecords
                                 .Find(x => Course.TeacherName3.Equals(string.IsNullOrEmpty(x.Nickname) ? x.Name : x.Name + "(" + x.Nickname + ")"));

                            if (vTeacher != null)
                            {
                                TCInstructRecord TCInstruct = new TCInstructRecord(vTeacher.ID, CourseID, 3);
                                TCInstructs.Add(TCInstruct);
                            }
                        }
                        #endregion
                    }

                    Progress++;
                    ProgressPercent = (int)((float)Progress / (float)Courses.Count * 100);
                    StartBGW.ReportProgress(ProgressPercent, "建立課程授課教師中...");
                }

                if (!K12.Data.Utility.Utility.IsNullOrEmpty(TCInstructs))
                {
                    //建立Spliter
                    StartBGW.ReportProgress(0, "上傳課程授課教師中...");
 
                    FunctionSpliter<TCInstructRecord, string> TCSpliter = new FunctionSpliter<TCInstructRecord, string>(1000, 1);
                    TCSpliter.Function = (x) => TCInstruct.Insert(x);
                    TCSpliter.ProgressChange = x => StartBGW.ReportProgress(0, "上傳課程授課教師中..."); 
                    List<string> IDs = TCSpliter.Execute(TCInstructs);

                    log.SetTeacherLog(TeacherRecords, TCInstructs);

                    StartBGW.ReportProgress(100, "已上傳課程授課教師中..."); 
                }
                #endregion
            }

            #region 加入班級學生修課

            //取得班級學生
            StartBGW.ReportProgress(0, "取得課程上課學生...");
            List<StudObj> Students = RepObj.GetStudentByClassID(NewCourses);

            //建立學生修課物件
            List<SCAttendRecord> SCAttends = new List<SCAttendRecord>();
            //針對每筆課程
            foreach (SimpleCourse NewCourse in NewCourses)
            {
                //若課程班級不為null
                if (!string.IsNullOrWhiteSpace(NewCourse.RefClassID))
                {
                    //尋找出課程班級的學生列表
                    List<StudObj> ClassStudents = Students.FindAll(x => x.ClassID.Equals(NewCourse.RefClassID));
                    //針對每位學生加入學生修課
                    foreach (StudObj Student in ClassStudents)
                    {
                        SCAttendRecord SCAttend = new SCAttendRecord();
                        SCAttend.RefCourseID = NewCourse.ID;
                        SCAttend.RefStudentID = Student.StudentID;
                        SCAttends.Add(SCAttend);
                    }
                }
            }

            //學生修課記錄如果不為空
            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SCAttends))
            {
                StartBGW.ReportProgress(0, "上傳學生修課記錄中...");
                //建立Spliter
                FunctionSpliter<SCAttendRecord, string> SCSpliter = new FunctionSpliter<SCAttendRecord, string>(1000, 1);
                SCSpliter.Function = (x) => SCAttend.Insert(x);
                SCSpliter.ProgressChange = x => StartBGW.ReportProgress((int)((float)x / (float)SCAttends.Count * 100), "上傳學生修課記錄中...");
                List<string> SCAttendIDs = SCSpliter.Execute(SCAttends);
                log.SetSCAttendLog(SCAttends, Students);
            }
            #endregion


            StartBGW.ReportProgress(89, "建立Log記錄...");
            log.GetSCAttendLog();
            FISCA.LogAgent.ApplicationLog.Log("排課", "建立教師授課", log.GetTeacherLog());
            FISCA.LogAgent.ApplicationLog.Log("排課", "複製課程", log.GetCourseLog());
            StartBGW.ReportProgress(100, "複製作業完成!!");
        }

        void StartBGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = "複製課程回ischool(完成)";
            btnStart.Visible = false;
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    //更新課程頁籤
                    FISCA.Features.Invoke("CourseSyncAllBackground");

                    foreach (SimpleCourse each in NewCourses)
                    {
                        foreach (DataGridViewRow row in dataGridViewX1.Rows)
                        {
                            SchedulerCourseExtension sce = (SchedulerCourseExtension)row.Tag;
                            if (sce.CourseName.Equals(each.CourseName))
                            {
                                row.Cells[Column4.Index].Value = "成功!!";
                                row.Cells[Column4.Index].Style.BackColor = Color.Green;
                                row.Cells[Column4.Index].Style.ForeColor = Color.White;
                                break;
                            }
                        }
                    }

                    List<string> list = new List<string>();
                    foreach (DataGridViewRow row in dataGridViewX1.Rows)
                    {
                        if (string.IsNullOrEmpty("" + row.Cells[3].Value))
                        {
                            SchedulerCourseExtension sce = (SchedulerCourseExtension)row.Tag;

                            row.Cells[Column4.Index].Value = "課程重覆!!";
                            row.Cells[Column4.Index].Style.BackColor = Color.Red;
                            row.Cells[Column4.Index].Style.ForeColor = Color.White;

                            list.Add(sce.UID);

                        }
                    }
                    CourseAdmin.Instance.AddToTemp(list);

                    MsgBox.Show("已複製「" + NewCourses.Count + "」筆課程回ischool！\n(ischool課程重覆之課程,會自動加入待處理)");
                    FISCA.Presentation.MotherForm.SetStatusBarMessage("已複製「" + NewCourses.Count + "」筆課程回ischool！");

                }
                else
                {
                    FISCA.Presentation.MotherForm.SetStatusBarMessage("資料取得發生錯誤!!");
                    SmartSchool.ErrorReporting.ReportingService.ReportException(e.Error);
                    MsgBox.Show("資料取得發生錯誤!!\n" + e.Error.Message);
                }
            }
            else
            {
                FISCA.Presentation.MotherForm.SetStatusBarMessage("作業取消!\n未複製任何課程!!");
                MsgBox.Show("作業取消!\n未複製任何課程!!");
            }
        }

        private int SortCourse(SchedulerCourseExtension a1, SchedulerCourseExtension a2)
        {
            return a1.CourseName.CompareTo(a2.CourseName);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// 簡單課程
    /// </summary>
    public class SimpleCourse
    {
        /// <summary>
        /// 課程系統編號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester { get; set; }

        /// <summary>
        /// 班級系統編號
        /// </summary>
        public string RefClassID { get; set; }
    }
}