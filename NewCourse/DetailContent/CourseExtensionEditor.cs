using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Campus.Windows;
using DevComponents.DotNetBar.Controls;
using FISCA.Data;
using FISCA.Permission;
using FISCA.UDT;
using FCode = FISCA.Permission.FeatureCodeAttribute;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 課程資料
    /// </summary>
    [FCode("2c70625c-7f60-4456-a164-27dacfc32dee", "課程")]
    public class CourseExtensionEditor : FISCA.Presentation.DetailContent, IContentEditor<SchedulerCourseExtension>
    {
        private FeatureAce UserPermission;
        private BackgroundWorker mbkwCourseExtension = new BackgroundWorker();
        private AccessHelper mAccessHelper = new AccessHelper();
        private bool IsBusy = false;
        private ChangeListener DataListener;
        private SchedulerCourseExtension mCourseExtension;
        private decimal? Credit;
        private decimal? Period;

        /// <summary>
        /// ischool教師
        /// </summary>
        private Dictionary<string, string> ischool_Teachers = new Dictionary<string, string>();

        /// <summary>
        /// 排課教師
        /// </summary>
        private Dictionary<string, string> Sunset_Teachers = new Dictionary<string, string>();

        /// <summary>
        /// ischool班級
        /// </summary>
        private Dictionary<string, string> ischool_ClassNameIDs = new Dictionary<string, string>();
        /// <summary>
        /// 排課班級
        /// </summary>
        private Dictionary<string, string> Sunset_ClassNameIDs = new Dictionary<string, string>();

        private List<TimeTable> mTimeTables;
        private List<Classroom> mClassrooms;
        private Dictionary<string, string> mClassroomName; //場地ID : 場地名稱

        private string TeacherName1 = string.Empty;
        private string TeacherName2 = string.Empty;
        private string TeacherName3 = string.Empty;
        private int CurrentTeacherIndex = 1;

        private Log_CourseExtension log { get; set; }

        #region Components

        private ErrorProvider errSplitSpec;
        private IContainer components;
        private ErrorProvider errWeekdayCondition;
        private ErrorProvider errPeriodCondition;
        private GroupPanel groupPanel1;
        private TextBoxX txtPeriodCond;
        private TextBoxX txtWeekdayCond;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX5;
        private ComboBoxEx cmbWeekFlag;
        private DevComponents.DotNetBar.LabelX labelX4;
        private ComboBoxEx cmbClassroom;
        private DevComponents.DotNetBar.LabelX labelX3;
        private TextBoxX txtCourseAliasName;
        private DevComponents.DotNetBar.LabelX labelX7;
        private TextBoxX txtSplitSpec;
        private DevComponents.DotNetBar.LabelX labelX2;
        private ComboBoxEx cmbTimeTable;
        private DevComponents.DotNetBar.LabelX labelX1;
        private GroupPanel groupPanel3;
        private TextBoxX txtCourseName;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.LabelX labelX12;
        private TextBoxX txtSubject;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.LabelX labelX15;
        private TextBoxX txtCredit;
        private ComboBoxEx cmbSchoolYear;
        private ComboBoxEx cmbLevel;
        private DevComponents.DotNetBar.LabelX labelX17;
        private ErrorProvider errCreditPeriod;
        private ErrorProvider errSchoolYear;
        private ErrorProvider errLevel;
        private ErrorProvider errSemester;
        private ComboBoxEx cmbSemester;
        private ComboBoxEx cmbClassName;
        private ErrorProvider warnClassName;
        private ErrorProvider warnTeacherName1;
        private ErrorProvider warnTeacherName2;
        private ErrorProvider warnTeacherName3;
        private DevComponents.DotNetBar.LabelX labelX20;
        private DevComponents.DotNetBar.LabelX labelX21;
        private TextBoxX txtDomain;
        private ComboBoxEx cmbRequired;
        private ComboBoxEx cmbRequiredBy;
        private DevComponents.DotNetBar.LabelX labelX23;
        private DevComponents.DotNetBar.LabelX labelX22;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private ComboBoxEx cmbEntry;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.Editors.ComboItem comboItem6;
        private DevComponents.Editors.ComboItem comboItem7;
        private DevComponents.Editors.ComboItem comboItem8;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.Editors.ComboItem comboItem10;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem11;
        private DevComponents.Editors.ComboItem comboItem12;
        private DevComponents.DotNetBar.LabelX labelX24;
        private Panel panel1;
        private Panel panel2;
        private DevComponents.DotNetBar.LabelX labelX25;
        private Panel panel3;
        private DevComponents.DotNetBar.LabelX labelX26;
        private Panel panel4;
        private DevComponents.DotNetBar.LabelX labelX27;
        private DevComponents.DotNetBar.ButtonX btnTeacherName;
        private DevComponents.DotNetBar.ButtonItem btnTeacherName2;
        private DevComponents.DotNetBar.ButtonItem btnTeacherName3;
        private CheckBoxX chkCalc;
        private CheckBoxX chkNoCalc;
        private CheckBoxX chkNoCredit;
        private CheckBoxX chkCredit;
        private CheckBoxX chkNoCalculationFlag;
        private CheckBoxX chkCalculationFlag;
        private CheckBoxX chkCloseQuery;
        private CheckBoxX chkOpenQuery;
        private Panel panel5;
        private CheckBoxX chkNoAllowDup;
        private CheckBoxX chkAllowDup;
        private DevComponents.DotNetBar.LabelX labelX16;
        private Panel panel6;
        private CheckBoxX chkNoLimitNextDay;
        private CheckBoxX chkLimitNextDay;
        private DevComponents.DotNetBar.LabelX labelX18;
        private Panel panel7;
        private CheckBoxX chkNoLongBreak;
        private CheckBoxX chkLongBreak;
        private DevComponents.DotNetBar.LabelX labelX19;
        private DevComponents.DotNetBar.ButtonItem btnTeacherName1;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel1;
        private ComboBoxEx cmbTeacherName3;
        private ComboBoxEx cmbTeacherName2;
        private ComboBoxEx cmbTeacherName1;
        private LinkLabel linkLabel4;
        private LinkLabel linkLabel3;
        private DevComponents.Editors.ComboItem comboItem13;
        private GroupPanel groupPanel2;
        #endregion

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public CourseExtensionEditor()
        {
            InitializeComponent();

            //設定資料項目名稱
            Group = "課程";
        }

        /// <summary>
        /// 當控制項狀態變更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataListener_StatusChanged(object sender, ChangeEventArgs e)
        {
            SaveButtonVisible = (e.Status == ValueStatus.Dirty);
            CancelButtonVisible = (e.Status == ValueStatus.Dirty);
        }


        #region BackgroundWorker
        private void mbkwCourseExtension_DoWork(object sender, DoWorkEventArgs e)
        {
            if (string.IsNullOrEmpty(this.PrimaryKey)) return;

            mCourseExtension = null;

            QueryHelper helper = new QueryHelper();

            string strCourseID = this.PrimaryKey;

            string strCondition = string.Format("uid={0}", strCourseID);

            #region 取得課程學分數或節數
            if (!string.IsNullOrWhiteSpace(strCourseID))
            {
                DataTable table = helper.Select("select period,credit from $scheduler.scheduler_course_extension where uid=" + strCourseID);

                if (table.Rows.Count == 1)
                {
                    Credit = K12.Data.Decimal.ParseAllowNull("" + table.Rows[0]["credit"]);
                    Period = K12.Data.Decimal.ParseAllowNull("" + table.Rows[0]["period"]);
                }
            }
            #endregion

            #region 依課程系統編號取得課程排課資料，若不存在則新增
            List<SchedulerCourseExtension> mCourseExtensions = mAccessHelper
                .Select<SchedulerCourseExtension>(strCondition);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(mCourseExtensions))
                mCourseExtension = mCourseExtensions[0];
            //else
            //{
            //    //課程名稱 所屬班級 科目名稱 學分/節數 學年度 學期

            //    #region 若課程排課資料不存在則新增後再取得
            //    mCourseExtension = new SchedulerCourseExtension();
            //    mCourseExtension.CourseName = string.Empty;
            //    mCourseExtension.SchoolYear = K12.Data.Int.Parse(K12.Data.School.DefaultSchoolYear);
            //    mCourseExtension.Semester = K12.Data.Int.Parse(K12.Data.School.DefaultSemester);

            //    mCourseExtension.CourseID = null;
            //    mCourseExtension.AllowDup = false;
            //    mCourseExtension.TimeTableID = null;
            //    mCourseExtension.ClassroomID = null;
            //    mCourseExtension.LongBreak = false;
            //    mCourseExtension.LimitNextDay = false;
            //    mCourseExtension.SplitSpec = string.Empty;
            //    mCourseExtension.WeekFlag = 3;
            //    mCourseExtension.WeekDayCond = string.Empty;
            //    mCourseExtension.PeriodCond = string.Empty;
            //    mCourseExtension.SubjectAliasName = string.Empty;

            //    List<SchedulerCourseExtension> InsertRecords = new List<SchedulerCourseExtension>();
            //    InsertRecords.Add(mCourseExtension);
            //    List<string> NewIDs = mAccessHelper.InsertValues(InsertRecords);

            //    if (NewIDs.Count > 0)
            //    {
            //        List<SchedulerCourseExtension> NewCourseExtensions = mAccessHelper.Select<SchedulerCourseExtension>("UID="+NewIDs[0]);
            //        if (NewCourseExtensions.Count > 0)
            //            mCourseExtension = NewCourseExtensions[0];
            //    }
            //    #endregion
            //}
            #endregion

            #region 取得所有時間表及場地
            mTimeTables = mAccessHelper.Select<TimeTable>().OrderBy(x => x.TimeTableName).ToList();

            mClassrooms = mAccessHelper.Select<Classroom>().OrderBy(x => x.ClassroomName).ToList();

            mClassroomName = new Dictionary<string, string>();
            foreach (Classroom each in mClassrooms)
            {
                if (!mClassroomName.ContainsKey(each.UID))
                {
                    mClassroomName.Add(each.UID, each.ClassroomName);
                }
            }
            #endregion

            //取得ischool教師清單
            ischool_Teachers = GiveMeTheList.GetischoolTeacher();
            //取得排課教師清單
            Sunset_Teachers = GiveMeTheList.GetSunsetTeacher();

            //取得ischool班級清單
            ischool_ClassNameIDs = GiveMeTheList.GetischoolClass();
            //取得排課班級清單
            Sunset_ClassNameIDs = GiveMeTheList.GetSunsetClass();
        }

        /// <summary>
        /// 取得資料完成時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbkwCourseExtension_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //若為忙碌則重新執行
            if (IsBusy)
            {
                IsBusy = false;
                mbkwCourseExtension.RunWorkerAsync();
                return;
            }

            //準備畫面上的資料
            Prepare();

            if (mCourseExtension != null)
            {
                log = new Log_CourseExtension(mCourseExtension, mClassroomName);

                //設定排課課程資料
                Content = mCourseExtension;
            }

            this.Loading = false;
        }
        #endregion


        #region DetailContent相關程式碼
        /// <summary>
        /// 當選取教師改變時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            //若教師系統編號不空白才執行
            if (!string.IsNullOrEmpty(this.PrimaryKey))
            {
                this.Loading = true;

                if (mbkwCourseExtension.IsBusy) //如果是忙碌的
                    IsBusy = true; //為True
                else
                    mbkwCourseExtension.RunWorkerAsync(); //否則直接執行
            }
        }

        /// <summary>
        /// 當按下儲存按鈕時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSaveButtonClick(EventArgs e)
        {
            if (!string.IsNullOrEmpty(errSplitSpec.GetError(txtSplitSpec)) ||
                !string.IsNullOrEmpty(errWeekdayCondition.GetError(txtWeekdayCond)) ||
                !string.IsNullOrEmpty(errPeriodCondition.GetError(txtPeriodCond)))
            {
                MessageBox.Show("輸入資料有誤，無法儲存!");

                return;
            }

            SchedulerCourseExtension CourseExtension = Content;

            BackgroundWorker Save_BGW = new BackgroundWorker();
            Save_BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Save_BGW_RunWorkerCompleted);
            Save_BGW.DoWork += new DoWorkEventHandler(Save_BGW_DoWork);
            if (!Save_BGW.IsBusy)
            {
                FISCA.Presentation.MotherForm.SetStatusBarMessage("排課課程「基本資料」儲存中...");

                Save_BGW.RunWorkerAsync(CourseExtension);
            }
            else
            {
                MsgBox.Show("系統忙碌中,稍後再試!!");
            }
        }

        void Save_BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            SchedulerCourseExtension CourseExtension = (SchedulerCourseExtension)e.Argument;

            if (CourseExtension != null)
            {
                //2013/5/7 - 新增判斷
                if (Tsd.ChangeDataSnyc(CourseExtension))
                {
                    DialogResult dr = MsgBox.Show("「課程分段預設值」欄位已被更動\n您是否要依據「預設值」內容同步修改「課程分段」資料?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {

                            //取得相對應的課程分段資料
                            List<SchedulerCourseSection> mCourseSections = mAccessHelper.Select<SchedulerCourseSection>(string.Format("ref_course_id={0}", this.PrimaryKey));
                            StringBuilder sb_LogSection = new StringBuilder();
                            sb_LogSection.AppendLine(string.Format("由於修改排課課程「{0}」的「預設值」欄位\n系統已同步「課程分段」資料:", CourseExtension.CourseName));
                            //修改為預設值
                            foreach (SchedulerCourseSection each in mCourseSections)
                            {
                                sb_LogSection.AppendLine("星期「" + each.WeekDay + "」節次「" + each.Period + "」" + "時數「" + each.Length + "」");
                                #region 修改為預設值
                                if (Tsd.場地是否更動過())
                                {
                                    string id_1 = "";
                                    string id_2 = "";

                                    string name_1 = "";
                                    string name_2 = "";

                                    if (CourseExtension.ClassroomID.HasValue)
                                        id_1 = CourseExtension.ClassroomID.Value.ToString();

                                    if (each.ClassroomID.HasValue)
                                        id_2 = each.ClassroomID.Value.ToString();

                                    if (mClassroomName.ContainsKey(id_1))
                                        name_1 = mClassroomName[id_1];

                                    if (mClassroomName.ContainsKey(id_2))
                                        name_2 = mClassroomName[id_2];

                                    sb_LogSection.AppendLine(Course_Log.GetLogString("上課場地", name_2, name_1));
                                    if (CourseExtension.ClassroomID.HasValue)
                                        each.ClassroomID = CourseExtension.ClassroomID.Value;
                                    else
                                        each.ClassroomID = null;
                                }

                                if (Tsd.星期是否更動過())
                                {
                                    string s_name = Course_Log.GetLogString("星期條件", each.WeekDayCond, CourseExtension.WeekDayCond);
                                    sb_LogSection.AppendLine(s_name);
                                    each.WeekDayCond = CourseExtension.WeekDayCond;
                                }

                                if (Tsd.單雙週是否更動過())
                                {
                                    string s_name = Course_Log.GetLogString("單雙週", Course_Log.GetWeekFlagName(each.WeekFlag), Course_Log.GetWeekFlagName(CourseExtension.WeekFlag));
                                    sb_LogSection.AppendLine(s_name);
                                    each.WeekFlag = CourseExtension.WeekFlag;
                                }

                                if (Tsd.節次是否更動過())
                                {
                                    string s_name = Course_Log.GetLogString("節次條件", each.PeriodCond, CourseExtension.PeriodCond);
                                    sb_LogSection.AppendLine(s_name);
                                    each.PeriodCond = CourseExtension.PeriodCond;
                                }

                                if (Tsd.跨中午是否更動過())
                                {
                                    string s_name = Course_Log.GetLogString("可跨中午", Course_Log.GetLongBreak(each.LongBreak), Course_Log.GetLongBreak(CourseExtension.LongBreak));
                                    sb_LogSection.AppendLine(s_name);
                                    each.LongBreak = CourseExtension.LongBreak;
                                }

                                if (Tsd.教師1是否更動過())
                                {
                                    #region 教師1

                                    string s_name = Course_Log.GetLogString("授課教師1", each.TeacherName1, CourseExtension.TeacherName1);
                                    sb_LogSection.AppendLine(s_name);
                                    each.TeacherName1 = CourseExtension.TeacherName1;

                                    if (ischool_Teachers.ContainsKey(CourseExtension.TeacherName1))
                                    {
                                        int x;
                                        int.TryParse(ischool_Teachers[CourseExtension.TeacherName1], out x);
                                        each.TeacherID1 = x;
                                    }
                                    else
                                    {
                                        each.TeacherID1 = null;
                                    }

                                    #endregion
                                }

                                if (Tsd.教師2是否更動過())
                                {
                                    #region 教師2
                                    string s_name = Course_Log.GetLogString("授課教師1", each.TeacherName2, CourseExtension.TeacherName2);
                                    sb_LogSection.AppendLine(s_name);
                                    each.TeacherName2 = CourseExtension.TeacherName2;

                                    if (ischool_Teachers.ContainsKey(CourseExtension.TeacherName2))
                                    {
                                        int x;
                                        int.TryParse(ischool_Teachers[CourseExtension.TeacherName2], out x);
                                        each.TeacherID2 = x;
                                    }
                                    else
                                    {
                                        each.TeacherID2 = null;
                                    }

                                    #endregion
                                }

                                if (Tsd.教師3是否更動過())
                                {
                                    #region 教師3

                                    string s_name = Course_Log.GetLogString("授課教師3", each.TeacherName3, CourseExtension.TeacherName3);
                                    sb_LogSection.AppendLine(s_name);
                                    each.TeacherName3 = CourseExtension.TeacherName3;

                                    if (ischool_Teachers.ContainsKey(CourseExtension.TeacherName3))
                                    {
                                        int x;
                                        int.TryParse(ischool_Teachers[CourseExtension.TeacherName3], out x);
                                        each.TeacherID3 = x;
                                    }
                                    else
                                    {
                                        each.TeacherID3 = null;
                                    }

                                    #endregion
                                }
                                sb_LogSection.AppendLine("");
                                #endregion
                            }

                            //儲存排課課程分段的調整
                            tool._A.UpdateValues(mCourseSections);
                            FISCA.LogAgent.ApplicationLog.Log("排課", "同步修改", sb_LogSection.ToString());
                        }
                        catch (Exception ve)
                        {
                            SmartSchool.ErrorReporting.ReportingService.ReportException(ve);
                            MessageBox.Show("修改課程分段屬性發生錯誤，已為您回報，錯誤訊息如下！" + System.Environment.NewLine + ve.Message);
                        }
                    }
                }

                try
                {
                    //儲存排課基本資料
                    List<SchedulerCourseExtension> CourseExtensions = new List<SchedulerCourseExtension>();
                    CourseExtensions.Add(CourseExtension);
                    mAccessHelper.SaveAll(CourseExtensions);

                    //Log
                    FISCA.LogAgent.ApplicationLog.Log("排課", "修改", log.GetLog(CourseExtension));
                }
                catch (Exception ve)
                {
                    SmartSchool.ErrorReporting.ReportingService.ReportException(ve);
                    MessageBox.Show("修改課程屬性發生錯誤，已為您回報，錯誤訊息如下！" + System.Environment.NewLine + ve.Message); 
                }
            }
        }

        void Save_BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                SaveButtonVisible = false;
                CancelButtonVisible = false;

                this.Loading = true;

                DataListener.SuspendListen();         //終止變更判斷
                mbkwCourseExtension.RunWorkerAsync(); //背景作業,取得並重新填入原資料

                //CourseEvents.RaiseChanged();
                //CourseSectionEvents.RaiseChanged();

                FISCA.Presentation.MotherForm.SetStatusBarMessage("課程基本資料儲存成功!!");
            }
            else
            {
                FISCA.Presentation.MotherForm.SetStatusBarMessage("資料儲存失敗!!");
                MsgBox.Show("資料儲存失敗!!");
            }
        }

        /// <summary>
        /// 當按下取消按鈕時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCancelButtonClick(EventArgs e)
        {
            SaveButtonVisible = false;
            CancelButtonVisible = false;

            this.Loading = true;

            DataListener.SuspendListen(); //終止變更判斷

            if (!mbkwCourseExtension.IsBusy)
                mbkwCourseExtension.RunWorkerAsync();
        }

        /// <summary>
        /// 儲存按鈕是否可見
        /// </summary>
        public new bool SaveButtonVisible
        {
            get { return base.SaveButtonVisible; }
            set
            {
                //判斷權限
                if (Attribute.IsDefined(GetType(), typeof(FeatureCodeAttribute)))
                {
                    FeatureCodeAttribute fca = Attribute.GetCustomAttribute(GetType(), typeof(FeatureCodeAttribute)) as FeatureCodeAttribute;
                    if (fca != null)
                        if (FISCA.Permission.UserAcl.Current[fca.Code].Editable)
                            base.SaveButtonVisible = value;
                }
                else //沒有定義權限就按照平常方法處理。
                    base.SaveButtonVisible = value;
            }
        }

        /// <summary>
        /// 取消按鈕是否可見
        /// </summary>
        public new bool CancelButtonVisible
        {
            get { return base.CancelButtonVisible; }
            set
            {
                //判斷權限
                if (Attribute.IsDefined(GetType(), typeof(FeatureCodeAttribute)))
                {
                    FeatureCodeAttribute fca = Attribute.GetCustomAttribute(GetType(), typeof(FeatureCodeAttribute)) as FeatureCodeAttribute;

                    if (fca != null)
                        if (FISCA.Permission.UserAcl.Current[fca.Code].Editable)
                            base.CancelButtonVisible = value;
                }
                else //沒有定義權限就按照平常方法處理。
                    base.CancelButtonVisible = value;
            }
        }
        #endregion

        #region IContentEditor<CourseExtension> 成員
        /// <summary>
        /// 準備資料
        /// </summary>
        public void Prepare()
        {
            if (cmbWeekFlag.Items.Count == 0)
            {
                cmbWeekFlag.Items.Add("單雙");
                cmbWeekFlag.Items.Add("單");
                cmbWeekFlag.Items.Add("雙");
            }

            //所有時間表名稱加入到選項
            cmbTimeTable.Items.Clear();
            cmbTimeTable.Items.Add(string.Empty);
            mTimeTables.ForEach(x => cmbTimeTable.Items.Add(x.TimeTableName));

            //所有場地加入到選項中
            cmbClassroom.Items.Clear();
            cmbClassroom.Items.Add(string.Empty);
            mClassrooms.ForEach(x => cmbClassroom.Items.Add(x.ClassroomName));

            cmbClassName.Items.Clear();
            Sunset_ClassNameIDs.Keys.ToList().ForEach(x => cmbClassName.Items.Add(x));

            cmbTeacherName1.Items.Clear();
            cmbTeacherName2.Items.Clear();
            cmbTeacherName3.Items.Clear();

            foreach (string TeacherName in Sunset_Teachers.Keys)
            {
                cmbTeacherName1.Items.Add(TeacherName);
                cmbTeacherName2.Items.Add(TeacherName);
                cmbTeacherName3.Items.Add(TeacherName);
            }

        }

        /// <summary>
        /// 取得或設定內容
        /// </summary>
        public SchedulerCourseExtension Content
        {
            get
            {
                #region Step1:依名稱尋找是否有對應的時間表及場地
                TimeTable TimeTable = mTimeTables.Find(x => x.TimeTableName.Equals("" + cmbTimeTable.SelectedItem));
                Classroom Classroom = mClassrooms.Find(x => x.ClassroomName.Equals("" + cmbClassroom.SelectedItem));
                #endregion

                #region Step2:指定課程相關屬性
                mCourseExtension.TeacherName1 = cmbTeacherName1.Text;
                mCourseExtension.TeacherName2 = cmbTeacherName2.Text;
                mCourseExtension.TeacherName3 = cmbTeacherName3.Text;

                mCourseExtension.NoQuery = chkOpenQuery.Checked ? false : true;

                mCourseExtension.ClassName = cmbClassName.Text;

                //2013/5/29 要把classid填入ischool班級id的功能移除
                if (!string.IsNullOrWhiteSpace(cmbClassName.Text) && ischool_ClassNameIDs.ContainsKey(cmbClassName.Text))
                    mCourseExtension.ClassID = K12.Data.Int.Parse(ischool_ClassNameIDs[cmbClassName.Text]);
                else
                    mCourseExtension.ClassID = null;

                mCourseExtension.CourseName = txtCourseName.Text;
                mCourseExtension.SchoolYear = K12.Data.Int.Parse(cmbSchoolYear.Text);
                mCourseExtension.Semester = cmbSemester.Text;
                mCourseExtension.Subject = txtSubject.Text;
                mCourseExtension.Level = K12.Data.Int.ParseAllowNull(cmbLevel.Text);
                string strCreditPeriod = txtCredit.Text;

                //解析學分數及節數
                string[] strCreditPeriods = strCreditPeriod.Split(new char[] { '/' });

                #region 判斷儲存學分數及節數 Ⅰ若只有一個值則學分數等於節數 Ⅱ 若有2個以上的值，第1個為學分數，第2個為節數 Ⅲ 若值為空白則清空學分數及節數
                if (strCreditPeriods.Length == 1)
                {
                    mCourseExtension.Credit = K12.Data.Int.ParseAllowNull(strCreditPeriods[0]);
                    mCourseExtension.Period = K12.Data.Int.ParseAllowNull(strCreditPeriods[0]);
                }
                else if (strCreditPeriods.Length >= 2)
                {
                    mCourseExtension.Credit = K12.Data.Int.ParseAllowNull(strCreditPeriods[0]);
                    mCourseExtension.Period = K12.Data.Int.ParseAllowNull(strCreditPeriods[1]);
                }
                else if (string.IsNullOrEmpty(strCreditPeriod))
                {
                    mCourseExtension.Credit = null;
                    mCourseExtension.Period = null;
                }
                #endregion

                mCourseExtension.TimeTableID = null;
                if (TimeTable != null)
                    mCourseExtension.TimeTableID = K12.Data.Int.ParseAllowNull(TimeTable.UID);
                mCourseExtension.SplitSpec = txtSplitSpec.Text;
                mCourseExtension.AllowDup = chkAllowDup.Checked;
                mCourseExtension.LimitNextDay = chkLimitNextDay.Checked;
                mCourseExtension.SubjectAliasName = txtCourseAliasName.Text;
                #endregion

                #region 指定國中高課程專屬屬性
                mCourseExtension.Domain = txtDomain.Text;
                mCourseExtension.Entry = cmbEntry.Text;
                mCourseExtension.NotIncludedInCredit = chkNoCredit.Checked ? true : false;
                mCourseExtension.NotIncludedInCalc = chkNoCalc.Checked ? true : false;

                mCourseExtension.Required = cmbRequired.Text;
                mCourseExtension.RequiredBy = cmbRequiredBy.Text;
                mCourseExtension.CalculationFlag = chkCalculationFlag.Checked ? "是" : "否";
                #endregion

                #region Step3:指定課程分段預設屬性
                mCourseExtension.ClassroomID = null;
                if (Classroom != null)
                    mCourseExtension.ClassroomID = K12.Data.Int.ParseAllowNull(Classroom.UID);
                mCourseExtension.LongBreak = chkLongBreak.Checked;
                mCourseExtension.WeekDayCond = txtWeekdayCond.Text;
                mCourseExtension.PeriodCond = txtPeriodCond.Text;
                mCourseExtension.WeekFlag = Utility.GetWeekFlagInt("" + cmbWeekFlag.SelectedItem);
                #endregion

                return mCourseExtension;
            }
            set
            {
                if (value == null)
                    return;

                #region Step1:先尋找是否有對應的時間表及場地
                TimeTable TimeTable = mTimeTables.Find(x => x.UID.Equals(K12.Data.Int.GetString(value.TimeTableID)));
                Classroom Classroom = mClassrooms.Find(x => x.UID.Equals(K12.Data.Int.GetString(value.ClassroomID)));
                #endregion

                cmbTeacherName1.Text = value.TeacherName1;
                cmbTeacherName2.Text = value.TeacherName2;
                cmbTeacherName3.Text = value.TeacherName3;

                //btnTeacherName1.Tag = value.TeacherName1;
                //btnTeacherName2.Tag = value.TeacherName2;
                //btnTeacherName3.Tag = value.TeacherName3;

                //if (CurrentTeacherIndex == 1)
                //{
                //    cmbTeacherName.Text = TeacherName1;
                //    btnTeacherName.Text = "教師一";
                //}
                //else if (CurrentTeacherIndex == 2)
                //{
                //    cmbTeacherName.Text = TeacherName2;
                //    btnTeacherName.Text = "教師二"; 
                //}
                //else if (CurrentTeacherIndex == 3)
                //{
                //    cmbTeacherName.Text = TeacherName3;
                //    btnTeacherName.Text = "教師三";  
                //}

                txtCourseName.Text = value.CourseName;
                cmbSchoolYear.Text = "" + value.SchoolYear;
                cmbSemester.Text = value.Semester;
                cmbLevel.Text = K12.Data.Int.GetString(value.Level);
                txtSubject.Text = value.Subject;
                cmbClassName.Text = value.ClassName;

                if (value.NoQuery == true) //不開放查詢為True就是不開放
                    chkCloseQuery.Checked = true; //不開放
                else
                    chkOpenQuery.Checked = true; //開放

                if (value.NotIncludedInCredit == true)
                    chkNoCredit.Checked = true;
                else
                    chkCredit.Checked = true;

                if (value.NotIncludedInCalc == true)
                    chkNoCalc.Checked = true;
                else
                    chkCalc.Checked = true;

                //chkNoQuery.Checked = value.NoQuery;

                string Credit = K12.Data.Int.GetString(value.Credit);  //取得學分數
                string Period = K12.Data.Int.GetString(value.Period);  //取得節數

                #region 顯示學分數：
                //Ⅰ若兩者相同只顯示一個數字 
                //Ⅱ若兩者不同且都不為空白則以斜線區隔 
                //Ⅲ若其中一個為空白則顯示有值的
                if (Credit.Equals(Period))
                    txtCredit.Text = Credit;
                else if (!string.IsNullOrEmpty(Credit) && !string.IsNullOrEmpty(Period))
                    txtCredit.Text = Credit + "/" + Period;
                else
                    txtCredit.Text = !string.IsNullOrEmpty(Credit) ? Credit : Period;
                #endregion


                #region 指定國中高課程專屬屬性
                txtDomain.Text = mCourseExtension.Domain;
                cmbEntry.Text = mCourseExtension.Entry;

                //chkNotIncludedInCredit.Checked = mCourseExtension.NotIncludedInCredit;
                //chkNotIncludedInCalc.Checked = mCourseExtension.NotIncludedInCalc;

                if (mCourseExtension.RequiredBy.StartsWith("校"))
                    cmbRequiredBy.Text = "校訂";
                else if (mCourseExtension.RequiredBy.StartsWith("部"))
                    cmbRequiredBy.Text = "部訂";
                else
                    cmbRequiredBy.Text = string.Empty;

                if (mCourseExtension.Required.StartsWith("選"))
                    cmbRequired.Text = "選修";
                else if (mCourseExtension.Required.StartsWith("必"))
                    cmbRequired.Text = "必修";
                else
                    cmbRequired.Text = string.Empty;

                if (mCourseExtension.CalculationFlag.Equals("否"))
                    chkNoCalculationFlag.Checked = true;
                else
                    chkCalculationFlag.Checked = true;
                #endregion

                #region Step2:指定課程相關屬性
                cmbTimeTable.SelectedItem = TimeTable != null ? TimeTable.TimeTableName : string.Empty;
                txtSplitSpec.Text = value.SplitSpec;

                if (value.AllowDup)
                    chkAllowDup.Checked = true;
                else
                    chkNoAllowDup.Checked = true;

                if (value.LimitNextDay)
                    chkLimitNextDay.Checked = true;
                else
                    chkNoLimitNextDay.Checked = true;

                txtCourseAliasName.Text = value.SubjectAliasName;
                #endregion

                #region Step3:指定課程分段預設屬性
                cmbClassroom.SelectedItem = Classroom != null ? Classroom.ClassroomName : string.Empty;

                if (value.LongBreak)
                    chkLongBreak.Checked = true;
                else
                    chkNoLongBreak.Checked = true;

                txtWeekdayCond.Text = value.WeekDayCond;
                txtPeriodCond.Text = value.PeriodCond;
                cmbWeekFlag.SelectedItem = Utility.GetWeekFlagStr(value.WeekFlag);
                #endregion

                //2013/5/7 - 新增判斷
                Tsd = new TypeSyneData(value);

                #region Step4:設定介面
                SaveButtonVisible = false;
                CancelButtonVisible = false;

                DataListener.Reset();
                DataListener.ResumeListen();
                #endregion
            }
        }

        //2013/5/7 - 新增判斷
        TypeSyneData Tsd { get; set; }

        /// <summary>
        /// 傳回本身
        /// </summary>
        public UserControl Control
        {
            get { return this; }
        }
        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CourseExtensionEditor));
            this.errSplitSpec = new System.Windows.Forms.ErrorProvider(this.components);
            this.errWeekdayCondition = new System.Windows.Forms.ErrorProvider(this.components);
            this.errPeriodCondition = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.cmbTeacherName1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.panel7 = new System.Windows.Forms.Panel();
            this.chkNoLongBreak = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkLongBreak = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.btnTeacherName = new DevComponents.DotNetBar.ButtonX();
            this.btnTeacherName1 = new DevComponents.DotNetBar.ButtonItem();
            this.btnTeacherName2 = new DevComponents.DotNetBar.ButtonItem();
            this.btnTeacherName3 = new DevComponents.DotNetBar.ButtonItem();
            this.txtPeriodCond = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtWeekdayCond = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.cmbWeekFlag = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cmbClassroom = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cmbTeacherName3 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbTeacherName2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.chkNoLimitNextDay = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkLimitNextDay = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chkNoAllowDup = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkAllowDup = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkCloseQuery = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkOpenQuery = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX27 = new DevComponents.DotNetBar.LabelX();
            this.txtCourseAliasName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.txtSplitSpec = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbTimeTable = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkNoCalculationFlag = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkCalculationFlag = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX26 = new DevComponents.DotNetBar.LabelX();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkNoCalc = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkCalc = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX25 = new DevComponents.DotNetBar.LabelX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkNoCredit = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkCredit = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX24 = new DevComponents.DotNetBar.LabelX();
            this.cmbEntry = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.comboItem10 = new DevComponents.Editors.ComboItem();
            this.comboItem13 = new DevComponents.Editors.ComboItem();
            this.labelX23 = new DevComponents.DotNetBar.LabelX();
            this.labelX22 = new DevComponents.DotNetBar.LabelX();
            this.cmbRequired = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem12 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.cmbRequiredBy = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem11 = new DevComponents.Editors.ComboItem();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.txtDomain = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX21 = new DevComponents.DotNetBar.LabelX();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.cmbClassName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbSemester = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbLevel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX17 = new DevComponents.DotNetBar.LabelX();
            this.cmbSchoolYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtCredit = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.txtSubject = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.txtCourseName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.errCreditPeriod = new System.Windows.Forms.ErrorProvider(this.components);
            this.errSchoolYear = new System.Windows.Forms.ErrorProvider(this.components);
            this.errLevel = new System.Windows.Forms.ErrorProvider(this.components);
            this.errSemester = new System.Windows.Forms.ErrorProvider(this.components);
            this.warnClassName = new System.Windows.Forms.ErrorProvider(this.components);
            this.warnTeacherName1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.warnTeacherName2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.warnTeacherName3 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errSplitSpec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errWeekdayCondition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errPeriodCondition)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errCreditPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errSchoolYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errSemester)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warnClassName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warnTeacherName1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warnTeacherName2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warnTeacherName3)).BeginInit();
            this.SuspendLayout();
            // 
            // errSplitSpec
            // 
            this.errSplitSpec.ContainerControl = this;
            // 
            // errWeekdayCondition
            // 
            this.errWeekdayCondition.ContainerControl = this;
            // 
            // errPeriodCondition
            // 
            this.errPeriodCondition.ContainerControl = this;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.Color.Transparent;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.linkLabel2);
            this.groupPanel1.Controls.Add(this.linkLabel1);
            this.groupPanel1.Controls.Add(this.cmbTeacherName1);
            this.groupPanel1.Controls.Add(this.panel7);
            this.groupPanel1.Controls.Add(this.labelX19);
            this.groupPanel1.Controls.Add(this.btnTeacherName);
            this.groupPanel1.Controls.Add(this.txtPeriodCond);
            this.groupPanel1.Controls.Add(this.txtWeekdayCond);
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Controls.Add(this.cmbWeekFlag);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.cmbClassroom);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.cmbTeacherName3);
            this.groupPanel1.Controls.Add(this.cmbTeacherName2);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(13, 417);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(530, 138);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = "";
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = "";
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = "";
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 2;
            this.groupPanel1.Text = "課程分段預設值";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(491, 44);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(34, 17);
            this.linkLabel2.TabIndex = 11;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "說明";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(491, 11);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(34, 17);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "說明";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // cmbTeacherName1
            // 
            this.cmbTeacherName1.DisplayMember = "Text";
            this.cmbTeacherName1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTeacherName1.FormattingEnabled = true;
            this.cmbTeacherName1.ItemHeight = 19;
            this.cmbTeacherName1.Location = new System.Drawing.Point(97, 76);
            this.cmbTeacherName1.Name = "cmbTeacherName1";
            this.cmbTeacherName1.Size = new System.Drawing.Size(142, 25);
            this.cmbTeacherName1.TabIndex = 73;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.chkNoLongBreak);
            this.panel7.Controls.Add(this.chkLongBreak);
            this.panel7.Location = new System.Drawing.Point(347, 76);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(142, 25);
            this.panel7.TabIndex = 13;
            // 
            // chkNoLongBreak
            // 
            this.chkNoLongBreak.AutoSize = true;
            // 
            // 
            // 
            this.chkNoLongBreak.BackgroundStyle.Class = "";
            this.chkNoLongBreak.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoLongBreak.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkNoLongBreak.Location = new System.Drawing.Point(68, 2);
            this.chkNoLongBreak.Name = "chkNoLongBreak";
            this.chkNoLongBreak.Size = new System.Drawing.Size(40, 21);
            this.chkNoLongBreak.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoLongBreak.TabIndex = 1;
            this.chkNoLongBreak.Text = "否";
            // 
            // chkLongBreak
            // 
            this.chkLongBreak.AutoSize = true;
            // 
            // 
            // 
            this.chkLongBreak.BackgroundStyle.Class = "";
            this.chkLongBreak.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkLongBreak.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkLongBreak.Location = new System.Drawing.Point(8, 2);
            this.chkLongBreak.Name = "chkLongBreak";
            this.chkLongBreak.Size = new System.Drawing.Size(40, 21);
            this.chkLongBreak.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkLongBreak.TabIndex = 0;
            this.chkLongBreak.Text = "是";
            // 
            // labelX19
            // 
            this.labelX19.AutoSize = true;
            // 
            // 
            // 
            this.labelX19.BackgroundStyle.Class = "";
            this.labelX19.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX19.Location = new System.Drawing.Point(281, 78);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(60, 21);
            this.labelX19.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX19.TabIndex = 12;
            this.labelX19.Text = "可跨中午";
            // 
            // btnTeacherName
            // 
            this.btnTeacherName.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTeacherName.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTeacherName.Location = new System.Drawing.Point(12, 77);
            this.btnTeacherName.Name = "btnTeacherName";
            this.btnTeacherName.Size = new System.Drawing.Size(75, 23);
            this.btnTeacherName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnTeacherName.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnTeacherName1,
            this.btnTeacherName2,
            this.btnTeacherName3});
            this.btnTeacherName.TabIndex = 4;
            this.btnTeacherName.Text = "教師一";
            // 
            // btnTeacherName1
            // 
            this.btnTeacherName1.GlobalItem = false;
            this.btnTeacherName1.Name = "btnTeacherName1";
            this.btnTeacherName1.Text = "教師一";
            this.btnTeacherName1.Click += new System.EventHandler(this.btnTeacherName1_Click);
            // 
            // btnTeacherName2
            // 
            this.btnTeacherName2.GlobalItem = false;
            this.btnTeacherName2.Name = "btnTeacherName2";
            this.btnTeacherName2.Text = "教師二";
            this.btnTeacherName2.Click += new System.EventHandler(this.btnTeacherName2_Click);
            // 
            // btnTeacherName3
            // 
            this.btnTeacherName3.GlobalItem = false;
            this.btnTeacherName3.Name = "btnTeacherName3";
            this.btnTeacherName3.Text = "教師三";
            this.btnTeacherName3.Click += new System.EventHandler(this.btnTeacherName3_Click);
            // 
            // txtPeriodCond
            // 
            // 
            // 
            // 
            this.txtPeriodCond.Border.Class = "TextBoxBorder";
            this.txtPeriodCond.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtPeriodCond.Location = new System.Drawing.Point(347, 40);
            this.txtPeriodCond.Name = "txtPeriodCond";
            this.txtPeriodCond.Size = new System.Drawing.Size(142, 25);
            this.txtPeriodCond.TabIndex = 10;
            // 
            // txtWeekdayCond
            // 
            // 
            // 
            // 
            this.txtWeekdayCond.Border.Class = "TextBoxBorder";
            this.txtWeekdayCond.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtWeekdayCond.Location = new System.Drawing.Point(347, 7);
            this.txtWeekdayCond.Name = "txtWeekdayCond";
            this.txtWeekdayCond.Size = new System.Drawing.Size(142, 25);
            this.txtWeekdayCond.TabIndex = 7;
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = "";
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX6.Location = new System.Drawing.Point(281, 42);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(60, 21);
            this.labelX6.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX6.TabIndex = 9;
            this.labelX6.Text = "節次條件";
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX5.Location = new System.Drawing.Point(281, 9);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(60, 21);
            this.labelX5.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX5.TabIndex = 6;
            this.labelX5.Text = "星期條件";
            // 
            // cmbWeekFlag
            // 
            this.cmbWeekFlag.DisplayMember = "Text";
            this.cmbWeekFlag.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbWeekFlag.FormattingEnabled = true;
            this.cmbWeekFlag.ItemHeight = 19;
            this.cmbWeekFlag.Location = new System.Drawing.Point(97, 40);
            this.cmbWeekFlag.Name = "cmbWeekFlag";
            this.cmbWeekFlag.Size = new System.Drawing.Size(142, 25);
            this.cmbWeekFlag.TabIndex = 3;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX4.Location = new System.Drawing.Point(18, 42);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(74, 21);
            this.labelX4.TabIndex = 2;
            this.labelX4.Text = "單雙週條件";
            // 
            // cmbClassroom
            // 
            this.cmbClassroom.DisplayMember = "Text";
            this.cmbClassroom.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbClassroom.FormattingEnabled = true;
            this.cmbClassroom.ItemHeight = 19;
            this.cmbClassroom.Location = new System.Drawing.Point(97, 7);
            this.cmbClassroom.Name = "cmbClassroom";
            this.cmbClassroom.Size = new System.Drawing.Size(142, 25);
            this.cmbClassroom.TabIndex = 1;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX3.Location = new System.Drawing.Point(32, 9);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(60, 21);
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "場地條件";
            // 
            // cmbTeacherName3
            // 
            this.cmbTeacherName3.DisplayMember = "Text";
            this.cmbTeacherName3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTeacherName3.FormattingEnabled = true;
            this.cmbTeacherName3.ItemHeight = 19;
            this.cmbTeacherName3.Location = new System.Drawing.Point(97, 76);
            this.cmbTeacherName3.Name = "cmbTeacherName3";
            this.cmbTeacherName3.Size = new System.Drawing.Size(142, 25);
            this.cmbTeacherName3.TabIndex = 75;
            this.cmbTeacherName3.Visible = false;
            // 
            // cmbTeacherName2
            // 
            this.cmbTeacherName2.DisplayMember = "Text";
            this.cmbTeacherName2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTeacherName2.FormattingEnabled = true;
            this.cmbTeacherName2.ItemHeight = 19;
            this.cmbTeacherName2.Location = new System.Drawing.Point(97, 76);
            this.cmbTeacherName2.Name = "cmbTeacherName2";
            this.cmbTeacherName2.Size = new System.Drawing.Size(142, 25);
            this.cmbTeacherName2.TabIndex = 74;
            this.cmbTeacherName2.Visible = false;
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.Color.Transparent;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.linkLabel4);
            this.groupPanel2.Controls.Add(this.linkLabel3);
            this.groupPanel2.Controls.Add(this.panel6);
            this.groupPanel2.Controls.Add(this.labelX18);
            this.groupPanel2.Controls.Add(this.panel5);
            this.groupPanel2.Controls.Add(this.labelX16);
            this.groupPanel2.Controls.Add(this.panel4);
            this.groupPanel2.Controls.Add(this.labelX27);
            this.groupPanel2.Controls.Add(this.txtCourseAliasName);
            this.groupPanel2.Controls.Add(this.labelX7);
            this.groupPanel2.Controls.Add(this.txtSplitSpec);
            this.groupPanel2.Controls.Add(this.labelX2);
            this.groupPanel2.Controls.Add(this.cmbTimeTable);
            this.groupPanel2.Controls.Add(this.labelX1);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(10, 265);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(530, 146);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.Class = "";
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.Class = "";
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.Class = "";
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 1;
            this.groupPanel2.Text = "排課條件";
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(491, 88);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(34, 17);
            this.linkLabel4.TabIndex = 76;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "說明";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(491, 51);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(34, 17);
            this.linkLabel3.TabIndex = 12;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "說明";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.chkNoLimitNextDay);
            this.panel6.Controls.Add(this.chkLimitNextDay);
            this.panel6.Location = new System.Drawing.Point(350, 47);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(142, 25);
            this.panel6.TabIndex = 9;
            // 
            // chkNoLimitNextDay
            // 
            this.chkNoLimitNextDay.AutoSize = true;
            // 
            // 
            // 
            this.chkNoLimitNextDay.BackgroundStyle.Class = "";
            this.chkNoLimitNextDay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoLimitNextDay.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkNoLimitNextDay.Location = new System.Drawing.Point(72, 2);
            this.chkNoLimitNextDay.Name = "chkNoLimitNextDay";
            this.chkNoLimitNextDay.Size = new System.Drawing.Size(40, 21);
            this.chkNoLimitNextDay.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoLimitNextDay.TabIndex = 1;
            this.chkNoLimitNextDay.Text = "否";
            // 
            // chkLimitNextDay
            // 
            this.chkLimitNextDay.AutoSize = true;
            // 
            // 
            // 
            this.chkLimitNextDay.BackgroundStyle.Class = "";
            this.chkLimitNextDay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkLimitNextDay.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkLimitNextDay.Location = new System.Drawing.Point(12, 2);
            this.chkLimitNextDay.Name = "chkLimitNextDay";
            this.chkLimitNextDay.Size = new System.Drawing.Size(40, 21);
            this.chkLimitNextDay.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkLimitNextDay.TabIndex = 0;
            this.chkLimitNextDay.Text = "是";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            // 
            // 
            // 
            this.labelX18.BackgroundStyle.Class = "";
            this.labelX18.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX18.Location = new System.Drawing.Point(284, 49);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(60, 21);
            this.labelX18.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX18.TabIndex = 8;
            this.labelX18.Text = "隔天排課";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.chkNoAllowDup);
            this.panel5.Controls.Add(this.chkAllowDup);
            this.panel5.Location = new System.Drawing.Point(349, 84);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(142, 25);
            this.panel5.TabIndex = 11;
            // 
            // chkNoAllowDup
            // 
            this.chkNoAllowDup.AutoSize = true;
            // 
            // 
            // 
            this.chkNoAllowDup.BackgroundStyle.Class = "";
            this.chkNoAllowDup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoAllowDup.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkNoAllowDup.Location = new System.Drawing.Point(72, 2);
            this.chkNoAllowDup.Name = "chkNoAllowDup";
            this.chkNoAllowDup.Size = new System.Drawing.Size(40, 21);
            this.chkNoAllowDup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoAllowDup.TabIndex = 1;
            this.chkNoAllowDup.Text = "否";
            // 
            // chkAllowDup
            // 
            this.chkAllowDup.AutoSize = true;
            // 
            // 
            // 
            this.chkAllowDup.BackgroundStyle.Class = "";
            this.chkAllowDup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkAllowDup.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkAllowDup.Location = new System.Drawing.Point(12, 2);
            this.chkAllowDup.Name = "chkAllowDup";
            this.chkAllowDup.Size = new System.Drawing.Size(40, 21);
            this.chkAllowDup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkAllowDup.TabIndex = 0;
            this.chkAllowDup.Text = "是";
            // 
            // labelX16
            // 
            this.labelX16.AutoSize = true;
            // 
            // 
            // 
            this.labelX16.BackgroundStyle.Class = "";
            this.labelX16.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX16.Location = new System.Drawing.Point(284, 86);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(60, 21);
            this.labelX16.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX16.TabIndex = 10;
            this.labelX16.Text = "同天排課";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chkCloseQuery);
            this.panel4.Controls.Add(this.chkOpenQuery);
            this.panel4.Location = new System.Drawing.Point(350, 10);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(142, 25);
            this.panel4.TabIndex = 7;
            // 
            // chkCloseQuery
            // 
            this.chkCloseQuery.AutoSize = true;
            // 
            // 
            // 
            this.chkCloseQuery.BackgroundStyle.Class = "";
            this.chkCloseQuery.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCloseQuery.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkCloseQuery.Location = new System.Drawing.Point(72, 2);
            this.chkCloseQuery.Name = "chkCloseQuery";
            this.chkCloseQuery.Size = new System.Drawing.Size(67, 21);
            this.chkCloseQuery.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCloseQuery.TabIndex = 1;
            this.chkCloseQuery.Text = "不開放";
            // 
            // chkOpenQuery
            // 
            this.chkOpenQuery.AutoSize = true;
            // 
            // 
            // 
            this.chkOpenQuery.BackgroundStyle.Class = "";
            this.chkOpenQuery.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkOpenQuery.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkOpenQuery.Location = new System.Drawing.Point(12, 2);
            this.chkOpenQuery.Name = "chkOpenQuery";
            this.chkOpenQuery.Size = new System.Drawing.Size(54, 21);
            this.chkOpenQuery.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkOpenQuery.TabIndex = 0;
            this.chkOpenQuery.Text = "開放";
            // 
            // labelX27
            // 
            this.labelX27.AutoSize = true;
            // 
            // 
            // 
            this.labelX27.BackgroundStyle.Class = "";
            this.labelX27.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX27.Location = new System.Drawing.Point(284, 12);
            this.labelX27.Name = "labelX27";
            this.labelX27.Size = new System.Drawing.Size(60, 21);
            this.labelX27.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX27.TabIndex = 6;
            this.labelX27.Text = "開放查詢";
            // 
            // txtCourseAliasName
            // 
            // 
            // 
            // 
            this.txtCourseAliasName.Border.Class = "TextBoxBorder";
            this.txtCourseAliasName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCourseAliasName.Location = new System.Drawing.Point(97, 47);
            this.txtCourseAliasName.Name = "txtCourseAliasName";
            this.txtCourseAliasName.Size = new System.Drawing.Size(142, 25);
            this.txtCourseAliasName.TabIndex = 3;
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(32, 49);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(60, 21);
            this.labelX7.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX7.TabIndex = 2;
            this.labelX7.Text = "科目簡稱";
            // 
            // txtSplitSpec
            // 
            // 
            // 
            // 
            this.txtSplitSpec.Border.Class = "TextBoxBorder";
            this.txtSplitSpec.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSplitSpec.Location = new System.Drawing.Point(97, 84);
            this.txtSplitSpec.Name = "txtSplitSpec";
            this.txtSplitSpec.Size = new System.Drawing.Size(142, 25);
            this.txtSplitSpec.TabIndex = 5;
            this.txtSplitSpec.WatermarkText = "範例：1,2,2";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(31, 86);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "分割設定";
            // 
            // cmbTimeTable
            // 
            this.cmbTimeTable.DisplayMember = "Text";
            this.cmbTimeTable.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTimeTable.FormattingEnabled = true;
            this.cmbTimeTable.ItemHeight = 19;
            this.cmbTimeTable.Location = new System.Drawing.Point(97, 10);
            this.cmbTimeTable.Name = "cmbTimeTable";
            this.cmbTimeTable.Size = new System.Drawing.Size(142, 25);
            this.cmbTimeTable.TabIndex = 1;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(18, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "上課時間表";
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.Color.Transparent;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.panel3);
            this.groupPanel3.Controls.Add(this.labelX26);
            this.groupPanel3.Controls.Add(this.panel2);
            this.groupPanel3.Controls.Add(this.labelX25);
            this.groupPanel3.Controls.Add(this.panel1);
            this.groupPanel3.Controls.Add(this.labelX24);
            this.groupPanel3.Controls.Add(this.cmbEntry);
            this.groupPanel3.Controls.Add(this.labelX23);
            this.groupPanel3.Controls.Add(this.labelX22);
            this.groupPanel3.Controls.Add(this.cmbRequired);
            this.groupPanel3.Controls.Add(this.cmbRequiredBy);
            this.groupPanel3.Controls.Add(this.txtDomain);
            this.groupPanel3.Controls.Add(this.labelX21);
            this.groupPanel3.Controls.Add(this.labelX20);
            this.groupPanel3.Controls.Add(this.cmbClassName);
            this.groupPanel3.Controls.Add(this.cmbSemester);
            this.groupPanel3.Controls.Add(this.cmbLevel);
            this.groupPanel3.Controls.Add(this.labelX17);
            this.groupPanel3.Controls.Add(this.cmbSchoolYear);
            this.groupPanel3.Controls.Add(this.txtCredit);
            this.groupPanel3.Controls.Add(this.labelX15);
            this.groupPanel3.Controls.Add(this.labelX14);
            this.groupPanel3.Controls.Add(this.labelX13);
            this.groupPanel3.Controls.Add(this.txtSubject);
            this.groupPanel3.Controls.Add(this.labelX12);
            this.groupPanel3.Controls.Add(this.labelX11);
            this.groupPanel3.Controls.Add(this.txtCourseName);
            this.groupPanel3.Controls.Add(this.labelX10);
            this.groupPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(10, 5);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(530, 254);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.Class = "";
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseDown.Class = "";
            this.groupPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseOver.Class = "";
            this.groupPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel3.TabIndex = 0;
            this.groupPanel3.Text = "課程基本資料";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkNoCalculationFlag);
            this.panel3.Controls.Add(this.chkCalculationFlag);
            this.panel3.Location = new System.Drawing.Point(97, 195);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(142, 25);
            this.panel3.TabIndex = 13;
            // 
            // chkNoCalculationFlag
            // 
            this.chkNoCalculationFlag.AutoSize = true;
            // 
            // 
            // 
            this.chkNoCalculationFlag.BackgroundStyle.Class = "";
            this.chkNoCalculationFlag.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoCalculationFlag.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkNoCalculationFlag.Location = new System.Drawing.Point(68, 2);
            this.chkNoCalculationFlag.Name = "chkNoCalculationFlag";
            this.chkNoCalculationFlag.Size = new System.Drawing.Size(67, 21);
            this.chkNoCalculationFlag.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoCalculationFlag.TabIndex = 1;
            this.chkNoCalculationFlag.Text = "不列入";
            // 
            // chkCalculationFlag
            // 
            this.chkCalculationFlag.AutoSize = true;
            // 
            // 
            // 
            this.chkCalculationFlag.BackgroundStyle.Class = "";
            this.chkCalculationFlag.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCalculationFlag.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkCalculationFlag.Location = new System.Drawing.Point(8, 2);
            this.chkCalculationFlag.Name = "chkCalculationFlag";
            this.chkCalculationFlag.Size = new System.Drawing.Size(54, 21);
            this.chkCalculationFlag.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCalculationFlag.TabIndex = 0;
            this.chkCalculationFlag.Text = "列入";
            // 
            // labelX26
            // 
            this.labelX26.AutoSize = true;
            // 
            // 
            // 
            this.labelX26.BackgroundStyle.Class = "";
            this.labelX26.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX26.Location = new System.Drawing.Point(32, 197);
            this.labelX26.Name = "labelX26";
            this.labelX26.Size = new System.Drawing.Size(60, 21);
            this.labelX26.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX26.TabIndex = 12;
            this.labelX26.Text = "學期成績";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkNoCalc);
            this.panel2.Controls.Add(this.chkCalc);
            this.panel2.Location = new System.Drawing.Point(97, 130);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(142, 25);
            this.panel2.TabIndex = 9;
            // 
            // chkNoCalc
            // 
            this.chkNoCalc.AutoSize = true;
            // 
            // 
            // 
            this.chkNoCalc.BackgroundStyle.Class = "";
            this.chkNoCalc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoCalc.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkNoCalc.Location = new System.Drawing.Point(68, 2);
            this.chkNoCalc.Name = "chkNoCalc";
            this.chkNoCalc.Size = new System.Drawing.Size(67, 21);
            this.chkNoCalc.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoCalc.TabIndex = 1;
            this.chkNoCalc.Text = "不評分";
            // 
            // chkCalc
            // 
            this.chkCalc.AutoSize = true;
            // 
            // 
            // 
            this.chkCalc.BackgroundStyle.Class = "";
            this.chkCalc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCalc.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkCalc.Location = new System.Drawing.Point(8, 2);
            this.chkCalc.Name = "chkCalc";
            this.chkCalc.Size = new System.Drawing.Size(54, 21);
            this.chkCalc.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCalc.TabIndex = 0;
            this.chkCalc.Text = "評分";
            // 
            // labelX25
            // 
            this.labelX25.AutoSize = true;
            // 
            // 
            // 
            this.labelX25.BackgroundStyle.Class = "";
            this.labelX25.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX25.Location = new System.Drawing.Point(32, 132);
            this.labelX25.Name = "labelX25";
            this.labelX25.Size = new System.Drawing.Size(60, 21);
            this.labelX25.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX25.TabIndex = 8;
            this.labelX25.Text = "評分設定";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkNoCredit);
            this.panel1.Controls.Add(this.chkCredit);
            this.panel1.Location = new System.Drawing.Point(97, 164);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(142, 25);
            this.panel1.TabIndex = 11;
            // 
            // chkNoCredit
            // 
            this.chkNoCredit.AutoSize = true;
            // 
            // 
            // 
            this.chkNoCredit.BackgroundStyle.Class = "";
            this.chkNoCredit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoCredit.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkNoCredit.Location = new System.Drawing.Point(68, 2);
            this.chkNoCredit.Name = "chkNoCredit";
            this.chkNoCredit.Size = new System.Drawing.Size(67, 21);
            this.chkNoCredit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoCredit.TabIndex = 1;
            this.chkNoCredit.Text = "不計入";
            // 
            // chkCredit
            // 
            this.chkCredit.AutoSize = true;
            // 
            // 
            // 
            this.chkCredit.BackgroundStyle.Class = "";
            this.chkCredit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCredit.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkCredit.Location = new System.Drawing.Point(8, 2);
            this.chkCredit.Name = "chkCredit";
            this.chkCredit.Size = new System.Drawing.Size(54, 21);
            this.chkCredit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCredit.TabIndex = 0;
            this.chkCredit.Text = "計入";
            // 
            // labelX24
            // 
            this.labelX24.AutoSize = true;
            // 
            // 
            // 
            this.labelX24.BackgroundStyle.Class = "";
            this.labelX24.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX24.Location = new System.Drawing.Point(32, 166);
            this.labelX24.Name = "labelX24";
            this.labelX24.Size = new System.Drawing.Size(60, 21);
            this.labelX24.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX24.TabIndex = 10;
            this.labelX24.Text = "學分設定";
            // 
            // cmbEntry
            // 
            this.cmbEntry.DisplayMember = "Text";
            this.cmbEntry.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEntry.FormattingEnabled = true;
            this.cmbEntry.ItemHeight = 19;
            this.cmbEntry.Items.AddRange(new object[] {
            this.comboItem5,
            this.comboItem6,
            this.comboItem7,
            this.comboItem8,
            this.comboItem9,
            this.comboItem10,
            this.comboItem13});
            this.cmbEntry.Location = new System.Drawing.Point(350, 164);
            this.cmbEntry.Name = "cmbEntry";
            this.cmbEntry.Size = new System.Drawing.Size(142, 25);
            this.cmbEntry.TabIndex = 25;
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "學業";
            // 
            // comboItem7
            // 
            this.comboItem7.Text = "體育";
            // 
            // comboItem8
            // 
            this.comboItem8.Text = "國防通識";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "健康與護理";
            // 
            // comboItem10
            // 
            this.comboItem10.Text = "實習科目";
            // 
            // comboItem13
            // 
            this.comboItem13.Text = "專業科目";
            // 
            // labelX23
            // 
            this.labelX23.AutoSize = true;
            // 
            // 
            // 
            this.labelX23.BackgroundStyle.Class = "";
            this.labelX23.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX23.Location = new System.Drawing.Point(297, 101);
            this.labelX23.Name = "labelX23";
            this.labelX23.Size = new System.Drawing.Size(47, 21);
            this.labelX23.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX23.TabIndex = 20;
            this.labelX23.Text = "必選修";
            // 
            // labelX22
            // 
            this.labelX22.AutoSize = true;
            // 
            // 
            // 
            this.labelX22.BackgroundStyle.Class = "";
            this.labelX22.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX22.Location = new System.Drawing.Point(297, 197);
            this.labelX22.Name = "labelX22";
            this.labelX22.Size = new System.Drawing.Size(47, 21);
            this.labelX22.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX22.TabIndex = 26;
            this.labelX22.Text = "校部訂";
            // 
            // cmbRequired
            // 
            this.cmbRequired.DisplayMember = "Text";
            this.cmbRequired.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRequired.FormattingEnabled = true;
            this.cmbRequired.ItemHeight = 19;
            this.cmbRequired.Items.AddRange(new object[] {
            this.comboItem12,
            this.comboItem3,
            this.comboItem4});
            this.cmbRequired.Location = new System.Drawing.Point(350, 99);
            this.cmbRequired.Name = "cmbRequired";
            this.cmbRequired.Size = new System.Drawing.Size(142, 25);
            this.cmbRequired.TabIndex = 21;
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "必修";
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "選修";
            // 
            // cmbRequiredBy
            // 
            this.cmbRequiredBy.DisplayMember = "Text";
            this.cmbRequiredBy.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRequiredBy.FormattingEnabled = true;
            this.cmbRequiredBy.ItemHeight = 19;
            this.cmbRequiredBy.Items.AddRange(new object[] {
            this.comboItem11,
            this.comboItem1,
            this.comboItem2});
            this.cmbRequiredBy.Location = new System.Drawing.Point(350, 195);
            this.cmbRequiredBy.Name = "cmbRequiredBy";
            this.cmbRequiredBy.Size = new System.Drawing.Size(142, 25);
            this.cmbRequiredBy.TabIndex = 27;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "校訂";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "部訂";
            // 
            // txtDomain
            // 
            // 
            // 
            // 
            this.txtDomain.Border.Class = "TextBoxBorder";
            this.txtDomain.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtDomain.Location = new System.Drawing.Point(97, 99);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(142, 25);
            this.txtDomain.TabIndex = 7;
            // 
            // labelX21
            // 
            this.labelX21.AutoSize = true;
            // 
            // 
            // 
            this.labelX21.BackgroundStyle.Class = "";
            this.labelX21.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX21.Location = new System.Drawing.Point(58, 101);
            this.labelX21.Name = "labelX21";
            this.labelX21.Size = new System.Drawing.Size(34, 21);
            this.labelX21.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX21.TabIndex = 6;
            this.labelX21.Text = "領域";
            // 
            // labelX20
            // 
            this.labelX20.AutoSize = true;
            // 
            // 
            // 
            this.labelX20.BackgroundStyle.Class = "";
            this.labelX20.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX20.Location = new System.Drawing.Point(284, 166);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(60, 21);
            this.labelX20.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX20.TabIndex = 24;
            this.labelX20.Text = "分項類別";
            // 
            // cmbClassName
            // 
            this.cmbClassName.DisplayMember = "Text";
            this.cmbClassName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbClassName.FormattingEnabled = true;
            this.cmbClassName.ItemHeight = 19;
            this.cmbClassName.Location = new System.Drawing.Point(350, 6);
            this.cmbClassName.Name = "cmbClassName";
            this.cmbClassName.Size = new System.Drawing.Size(142, 25);
            this.cmbClassName.TabIndex = 15;
            // 
            // cmbSemester
            // 
            this.cmbSemester.DisplayMember = "Text";
            this.cmbSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSemester.FormattingEnabled = true;
            this.cmbSemester.ItemHeight = 19;
            this.cmbSemester.Location = new System.Drawing.Point(350, 68);
            this.cmbSemester.Name = "cmbSemester";
            this.cmbSemester.Size = new System.Drawing.Size(142, 25);
            this.cmbSemester.TabIndex = 19;
            // 
            // cmbLevel
            // 
            this.cmbLevel.DisplayMember = "Text";
            this.cmbLevel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.ItemHeight = 19;
            this.cmbLevel.Location = new System.Drawing.Point(350, 130);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(142, 25);
            this.cmbLevel.TabIndex = 23;
            // 
            // labelX17
            // 
            this.labelX17.AutoSize = true;
            // 
            // 
            // 
            this.labelX17.BackgroundStyle.Class = "";
            this.labelX17.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX17.Location = new System.Drawing.Point(284, 132);
            this.labelX17.Name = "labelX17";
            this.labelX17.Size = new System.Drawing.Size(60, 21);
            this.labelX17.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX17.TabIndex = 22;
            this.labelX17.Text = "科目級別";
            // 
            // cmbSchoolYear
            // 
            this.cmbSchoolYear.DisplayMember = "Text";
            this.cmbSchoolYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSchoolYear.FormattingEnabled = true;
            this.cmbSchoolYear.ItemHeight = 19;
            this.cmbSchoolYear.Location = new System.Drawing.Point(350, 37);
            this.cmbSchoolYear.Name = "cmbSchoolYear";
            this.cmbSchoolYear.Size = new System.Drawing.Size(142, 25);
            this.cmbSchoolYear.TabIndex = 17;
            // 
            // txtCredit
            // 
            // 
            // 
            // 
            this.txtCredit.Border.Class = "TextBoxBorder";
            this.txtCredit.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCredit.Location = new System.Drawing.Point(97, 68);
            this.txtCredit.Name = "txtCredit";
            this.txtCredit.Size = new System.Drawing.Size(142, 25);
            this.txtCredit.TabIndex = 5;
            this.txtCredit.TextChanged += new System.EventHandler(this.txtCredit_TextChanged);
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            // 
            // 
            // 
            this.labelX15.BackgroundStyle.Class = "";
            this.labelX15.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX15.Location = new System.Drawing.Point(26, 70);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(66, 21);
            this.labelX15.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX15.TabIndex = 4;
            this.labelX15.Text = "學分/節數";
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.Class = "";
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX14.Location = new System.Drawing.Point(310, 70);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(34, 21);
            this.labelX14.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX14.TabIndex = 18;
            this.labelX14.Text = "學期";
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.Class = "";
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX13.Location = new System.Drawing.Point(297, 39);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(47, 21);
            this.labelX13.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX13.TabIndex = 16;
            this.labelX13.Text = "學年度";
            // 
            // txtSubject
            // 
            // 
            // 
            // 
            this.txtSubject.Border.Class = "TextBoxBorder";
            this.txtSubject.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSubject.Location = new System.Drawing.Point(97, 37);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(142, 25);
            this.txtSubject.TabIndex = 3;
            // 
            // labelX12
            // 
            this.labelX12.AutoSize = true;
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.Class = "";
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX12.Location = new System.Drawing.Point(32, 39);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(60, 21);
            this.labelX12.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX12.TabIndex = 2;
            this.labelX12.Text = "科目名稱";
            // 
            // labelX11
            // 
            this.labelX11.AutoSize = true;
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.Class = "";
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX11.Location = new System.Drawing.Point(284, 8);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(60, 21);
            this.labelX11.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX11.TabIndex = 14;
            this.labelX11.Text = "所屬班級";
            // 
            // txtCourseName
            // 
            // 
            // 
            // 
            this.txtCourseName.Border.Class = "TextBoxBorder";
            this.txtCourseName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCourseName.Location = new System.Drawing.Point(97, 6);
            this.txtCourseName.Name = "txtCourseName";
            this.txtCourseName.Size = new System.Drawing.Size(142, 25);
            this.txtCourseName.TabIndex = 1;
            // 
            // labelX10
            // 
            this.labelX10.AutoSize = true;
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.Class = "";
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX10.Location = new System.Drawing.Point(32, 8);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(60, 21);
            this.labelX10.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX10.TabIndex = 0;
            this.labelX10.Text = "課程名稱";
            // 
            // errCreditPeriod
            // 
            this.errCreditPeriod.ContainerControl = this;
            // 
            // errSchoolYear
            // 
            this.errSchoolYear.ContainerControl = this;
            // 
            // errLevel
            // 
            this.errLevel.ContainerControl = this;
            // 
            // errSemester
            // 
            this.errSemester.ContainerControl = this;
            // 
            // warnClassName
            // 
            this.warnClassName.ContainerControl = this;
            this.warnClassName.Icon = ((System.Drawing.Icon)(resources.GetObject("warnClassName.Icon")));
            // 
            // warnTeacherName1
            // 
            this.warnTeacherName1.ContainerControl = this;
            this.warnTeacherName1.Icon = ((System.Drawing.Icon)(resources.GetObject("warnTeacherName1.Icon")));
            // 
            // warnTeacherName2
            // 
            this.warnTeacherName2.ContainerControl = this;
            this.warnTeacherName2.Icon = ((System.Drawing.Icon)(resources.GetObject("warnTeacherName2.Icon")));
            // 
            // warnTeacherName3
            // 
            this.warnTeacherName3.ContainerControl = this;
            this.warnTeacherName3.Icon = ((System.Drawing.Icon)(resources.GetObject("warnTeacherName3.Icon")));
            // 
            // CourseExtensionEditor
            // 
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.Name = "CourseExtensionEditor";
            this.Size = new System.Drawing.Size(550, 565);
            this.Load += new System.EventHandler(this.CourseExtensionEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errSplitSpec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errWeekdayCondition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errPeriodCondition)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errCreditPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errSchoolYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errSemester)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warnClassName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warnTeacherName1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warnTeacherName2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warnTeacherName3)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 驗證條件
        /// </summary>
        /// <param name="ErrProvider"></param>
        /// <param name="CondText"></param>
        private void ValidateCondition(ErrorProvider ErrProvider, TextBoxX CondText)
        {
            ErrProvider.Clear();

            string strCond = CondText.Text;

            if (string.IsNullOrEmpty(strCond))
                return;

            List<string> RelationOperators = new List<string>() { "=", ">", "<", ">=", "<=", "<>" };

            string UseOperator = string.Empty;

            RelationOperators.ForEach(x =>
            {
                if (strCond.StartsWith(x))
                    UseOperator = x;
            }
            );

            if (!string.IsNullOrEmpty(UseOperator))
            {
                int value;

                try
                {
                    string Operand = strCond.Substring(UseOperator.Length, strCond.Length - UseOperator.Length);

                    if (!int.TryParse(Operand, out value))
                        ErrProvider.SetError(CondText, "輸入條件有誤，範例：=3、>3、>=3、<3、<=3、<>3");
                }
                catch
                {
                    ErrProvider.SetError(CondText, "輸入條件有誤，範例：=3、>3、>=3、<3、<=3、<>3");
                }
            }
            else
                ErrProvider.SetError(CondText, "輸入條件有誤，範例：=3、>3、>=3、<3、<=3、<>3");
        }

        /// <summary>
        /// 檢查課程分段總長度
        /// </summary>
        /// <param name="Total"></param>
        /// <returns></returns>
        private Tuple<bool, string> ValidateTotalLen(decimal Total)
        {
            //若有節數則用節數檢查
            if (Period.HasValue)
            {
                if (Period.Value.Equals(Total))
                    return new Tuple<bool, string>(true, string.Empty);
                else
                    return new Tuple<bool, string>(false, "分割節數與課程節數不相同！");
            }//沒有節數改用學分數檢查
            else if (Credit.HasValue)
            {
                if (Credit.Value.Equals(Total))
                    return new Tuple<bool, string>(true, string.Empty);
                else
                    return new Tuple<bool, string>(false, "分割節數與課程學分數不相同！");

            }//沒有輸入學分數及節數的狀況
            else
                return new Tuple<bool, string>(false, "未輸入學分數及節數！");
        }

        /// <summary>
        /// 資料項目載入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourseExtensionEditor_Load(object sender, EventArgs e)
        {
            //學期
            cmbSemester.Items.Add("1");
            cmbSemester.Items.Add("2");
            //科目級別
            cmbLevel.Items.Add("1");
            cmbLevel.Items.Add("2");
            cmbLevel.Items.Add("3");
            cmbLevel.Items.Add("4");
            cmbLevel.Items.Add("5");
            cmbLevel.Items.Add("6");
            cmbLevel.Items.Add("7");
            cmbLevel.Items.Add("8");
            cmbLevel.Items.Add("9");
            //學年度
            int SchoolYear = K12.Data.Int.Parse(K12.Data.School.DefaultSchoolYear);

            for (int i = SchoolYear - 5; i <= SchoolYear + 5; i++)
                cmbSchoolYear.Items.Add("" + i);

            //設定權限
            UserPermission = FISCA.Permission.UserAcl.Current[FCode.GetCode(GetType())];
            this.Enabled = UserPermission.Editable;

            #region 驗證分割課程分段設定
            txtSplitSpec.TextChanged += (vsender, ve) =>
            {
                errSplitSpec.Clear();

                if (!string.IsNullOrEmpty(txtSplitSpec.Text))
                {
                    int a;
                    int TotalLen = 0;
                    bool AllPass = true;

                    string[] Specs = txtSplitSpec.Text.Split(new char[] { ',' });

                    for (int i = 0; i < Specs.Length; i++)
                    {
                        if (!int.TryParse(Specs[i], out a))
                        {
                            AllPass = false;
                            errSplitSpec.SetError(txtSplitSpec, "必須為以逗號分隔的數字組合，例如1,2,1");
                        }
                        else
                            TotalLen += a;
                    }

                    if (AllPass)
                    {
                        Tuple<bool, string> result = ValidateTotalLen(TotalLen);

                        if (!result.Item1)
                            errSplitSpec.SetError(txtSplitSpec, result.Item2);
                    }
                }
            };
            #endregion

            #region 驗證星期及節次條件
            txtWeekdayCond.TextChanged += (vsender, ve) => ValidateCondition(errWeekdayCondition, txtWeekdayCond);

            txtPeriodCond.TextChanged += (vsender, ve) => ValidateCondition(errPeriodCondition, txtPeriodCond);
            #endregion

            cmbSchoolYear.TextChanged += (vsender, ve) =>
            {
                int a;

                errSchoolYear.Clear();

                if (!int.TryParse(cmbSchoolYear.Text, out a))
                    errSchoolYear.SetError(cmbSchoolYear, "學年度請輸入整數！");
            };

            cmbLevel.TextChanged += (vsender, ve) =>
            {
                int a;

                errLevel.Clear();

                if (!string.IsNullOrWhiteSpace(cmbLevel.Text) && !int.TryParse(cmbLevel.Text, out a))
                    errLevel.SetError(cmbLevel, "級別請輸入整數！");
            };

            cmbSemester.TextChanged += (vsender, ve) =>
            {
                errSemester.Clear();

                if (string.IsNullOrWhiteSpace(cmbSemester.Text))
                    errSemester.SetError(cmbSemester, "學期必需要有值！");
            };

            cmbClassName.TextChanged += (vsender, ve) =>
            {
                warnClassName.Clear();

                string SetString = "";

                if (!string.IsNullOrWhiteSpace(cmbClassName.Text))
                {
                    if (!Sunset_ClassNameIDs.ContainsKey(cmbClassName.Text))
                        SetString = "無法對應到排課班級名稱！";

                    if (!ischool_ClassNameIDs.ContainsKey(cmbClassName.Text))
                    {
                        if (string.IsNullOrEmpty(SetString))
                            SetString = "無法對應到ischool班級名稱！";
                        else
                            SetString += "\n無法對應到ischool班級名稱！";
                    }
                }

                warnClassName.SetError(cmbClassName, SetString);
            };

            cmbTeacherName1.TextChanged += (vsender, ve) =>
            {
                warnTeacherName1.Clear();

                string SetString = "";

                if (!string.IsNullOrWhiteSpace(cmbTeacherName1.Text) && !Sunset_Teachers.ContainsKey(cmbTeacherName1.Text))
                    SetString = "無法對應到排課教師名稱！";

                if (!string.IsNullOrWhiteSpace(cmbTeacherName1.Text) && !ischool_Teachers.ContainsKey(cmbTeacherName1.Text))
                {
                    if (string.IsNullOrEmpty(SetString))
                        SetString = "無法對應到ischool教師名稱！";
                    else
                        SetString += "\n無法對應到ischool教師名稱！";
                }

                warnTeacherName1.SetError(cmbTeacherName1, SetString);
            };

            cmbTeacherName2.TextChanged += (vsender, ve) =>
            {
                warnTeacherName2.Clear();

                string SetString = "";

                if (!string.IsNullOrWhiteSpace(cmbTeacherName2.Text) && !Sunset_Teachers.ContainsKey(cmbTeacherName2.Text))
                    SetString = "無法對應到排課教師名稱！";

                if (!string.IsNullOrWhiteSpace(cmbTeacherName2.Text) && !ischool_Teachers.ContainsKey(cmbTeacherName2.Text))
                {
                    if (string.IsNullOrEmpty(SetString))
                        SetString = "無法對應到ischool教師名稱！";
                    else
                        SetString += "\n無法對應到ischool教師名稱！";
                }

                warnTeacherName2.SetError(cmbTeacherName2, SetString);
            };

            cmbTeacherName3.TextChanged += (vsender, ve) =>
            {
                warnTeacherName3.Clear();

                string SetString = "";

                if (!string.IsNullOrWhiteSpace(cmbTeacherName3.Text) && !Sunset_Teachers.ContainsKey(cmbTeacherName3.Text))
                    SetString = "無法對應到排課教師名稱！";

                if (!string.IsNullOrWhiteSpace(cmbTeacherName3.Text) && !ischool_Teachers.ContainsKey(cmbTeacherName3.Text))
                {
                    if (string.IsNullOrEmpty(SetString))
                        SetString = "無法對應到ischool教師名稱！";
                    else
                        SetString += "\n無法對應到ischool教師名稱！";
                }

                warnTeacherName3.SetError(cmbTeacherName3, SetString);
            };

            chkCloseQuery.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkOpenQuery.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkCredit.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkNoCredit.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkCalc.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkNoCalc.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkCalculationFlag.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkNoCalculationFlag.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkLimitNextDay.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkNoLimitNextDay.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkAllowDup.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkNoAllowDup.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkLongBreak.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            chkNoLongBreak.CheckedChanged += (vsender, ve) =>
            {
                this.SaveButtonVisible = true;
                this.CancelButtonVisible = true;
            };

            #region 註解

            //btnTeacherName.Click += (vsender, ve) =>
            //{
            //    cmbTeacherName.Text = TeacherName1;
            //    CurrentTeacherIndex = 1;
            //};



            //btnTeacherName.Text = "教師一";
            //cmbTeacherName.Text = TeacherName1;
            //CurrentTeacherIndex = 1;

            //btnTeacherName1.Click += (vsender, ve) =>
            //{
            //    btnTeacherName.Text = "教師一";
            //    cmbTeacherName.Text = "" + btnTeacherName1.Tag;
            //    CurrentTeacherIndex = 1;
            //};

            //btnTeacherName2.Click += (vsender, ve) =>
            //{
            //    btnTeacherName.Text = "教師二";
            //    cmbTeacherName.Text = "" + btnTeacherName2.Tag;
            //    CurrentTeacherIndex = 2;
            //};

            //btnTeacherName3.Click += (vsender, ve) =>
            //{
            //    btnTeacherName.Text = "教師三";
            //    cmbTeacherName.Text = "" + btnTeacherName3.Tag;
            //    CurrentTeacherIndex = 3;
            //};

            //cmbTeacherName.SelectedValueChanged += (vsender, ve) =>
            //{
            //    if (CurrentTeacherIndex == 1)
            //    {
            //        TeacherName1 = cmbTeacherName.Text;
            //    }
            //    else if (CurrentTeacherIndex == 2)
            //    {
            //        TeacherName2 = cmbTeacherName.Text;
            //    }
            //    else if (CurrentTeacherIndex == 3)
            //    {
            //        TeacherName3 = cmbTeacherName.Text;
            //    }

            //    SaveButtonVisible = true;
            //    CancelButtonVisible = true;
            //};

            //cmbTeacherName.TextChanged += (vsender, ve) =>
            //{
            //    if (CurrentTeacherIndex == 1)
            //    {
            //        btnTeacherName1.Tag = cmbTeacherName.Text;
            //    }
            //    else if (CurrentTeacherIndex == 2)
            //    {
            //        btnTeacherName2.Tag = cmbTeacherName.Text;
            //    }
            //    else if (CurrentTeacherIndex == 3)
            //    {
            //        btnTeacherName3.Tag = cmbTeacherName.Text;
            //    }

            //    SaveButtonVisible = true;
            //    CancelButtonVisible = true;
            //};

            //cmbTeacherName2.TextChanged += (vsender, ve) =>
            //{
            //    warnTeacherName2.Clear();

            //    if (!string.IsNullOrWhiteSpace(cmbTeacherName2.Text)
            //        && !mTeacherNameIDs.ContainsKey(cmbTeacherName2.Text))
            //        warnTeacherName2.SetError(cmbTeacherName2, "無法對應到ischool教師名稱！");
            //};

            //cmbTeacherName3.TextChanged += (vsender, ve) =>
            //{
            //    warnTeacherName3.Clear();

            //    if (!string.IsNullOrWhiteSpace(cmbTeacherName3.Text)
            //        && !mTeacherNameIDs.ContainsKey(cmbTeacherName3.Text))
            //        warnTeacherName3.SetError(cmbTeacherName3, "無法對應到ischool教師名稱！");
            //}; 

            #endregion

            //取得課程排課資料
            mbkwCourseExtension.DoWork += new DoWorkEventHandler(mbkwCourseExtension_DoWork);

            //完成取得課程排課資料
            mbkwCourseExtension.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mbkwCourseExtension_RunWorkerCompleted);

            //判斷DataGridView狀態變更
            DataListener = new ChangeListener();

            //加入TextBox狀態改變通知
            //DataListener.Add(new TextBoxSource(txtte
            DataListener.Add(new ComboBoxSource(cmbClassName, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new TextBoxSource(txtCourseName));
            DataListener.Add(new TextBoxSource(txtCredit));
            DataListener.Add(new TextBoxSource(txtSubject));
            //DataListener.Add(new ComboBoxSource(cmbTeacherName, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbTeacherName1, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbTeacherName2, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbTeacherName3, ComboBoxSource.ListenAttribute.Text));

            DataListener.Add(new TextBoxSource(txtWeekdayCond));
            DataListener.Add(new TextBoxSource(txtPeriodCond));
            DataListener.Add(new TextBoxSource(txtSplitSpec));
            DataListener.Add(new TextBoxSource(txtCourseAliasName));

            DataListener.Add(new TextBoxSource(txtDomain));

            //加入ComboxBox狀態改變通知
            DataListener.Add(new ComboBoxSource(cmbLevel, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbSchoolYear, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbSemester, ComboBoxSource.ListenAttribute.Text));

            DataListener.Add(new ComboBoxSource(cmbClassroom, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbTimeTable, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbWeekFlag, ComboBoxSource.ListenAttribute.Text));

            DataListener.Add(new ComboBoxSource(cmbRequired, ComboBoxSource.ListenAttribute.SelectedIndex));
            DataListener.Add(new ComboBoxSource(cmbRequiredBy, ComboBoxSource.ListenAttribute.SelectedIndex));

            DataListener.Add(new ComboBoxSource(cmbEntry, ComboBoxSource.ListenAttribute.Text));
            //加入CheckBox狀態改變通知
            //List<CheckBox> CheckBoxs = new List<CheckBox>() { chkAllowDup, chkLogBreak,chkLimitNextDay,chkNoQuery };

            //DataListener.Add(new CheckBoxSource(CheckBoxs.ToArray()));

            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(DataListener_StatusChanged);

            //終止變更判斷
            DataListener.SuspendListen();

            CourseEvents.CourseChanged += (vsender, ve) => OnPrimaryKeyChanged(new EventArgs());
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            this.SaveButtonVisible = true;
            this.CancelButtonVisible = true;
        }

        private void txtCredit_TextChanged(object sender, EventArgs e)
        {
            errCreditPeriod.SetError(txtCredit, "");

            string strCreditPeriod = txtCredit.Text;

            //解析學分數及節數
            string[] strCreditPeriods = strCreditPeriod.Split(new char[] { '/' });

            if (strCreditPeriods.Length == 1)
            {
                int a;

                if (!(int.TryParse(strCreditPeriod, out a)))
                {
                    errCreditPeriod.SetError(txtCredit, "學分數/節數只能輸入整數！");
                    return;
                }
            }
            else if (strCreditPeriods.Length >= 2)
            {
                int a;

                if (!(int.TryParse(strCreditPeriods[0], out a)))
                {
                    errCreditPeriod.SetError(txtCredit, "學分數/節數只能輸入整數！");
                    return;
                }

                if (!(int.TryParse(strCreditPeriods[1], out a)))
                {
                    errCreditPeriod.SetError(txtCredit, "學分數/節數只能輸入整數！");
                    return;
                }
            }
            else if (string.IsNullOrEmpty(strCreditPeriod))
            {

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine("星期條件說明：");
            sb1.AppendLine("設定課程排課時，允許排入的「星期條件」。");
            sb1.AppendLine("像是您可以指定「國文課」需固定排在「每週三、四、五」。");
            sb1.AppendLine("");
            sb1.AppendLine("樣式：");
            sb1.AppendLine("=3：排在星期三。");
            sb1.AppendLine("<3：排在星期三之前，但不包含星期三；意即星期二、星期一。");
            sb1.AppendLine(">3：排在星期三之後，但不包含星期三；意即星期四、星期五、星期六。");
            sb1.AppendLine("<=3：排在星期三之前，包含星期三；意即星期三、星期二、星期一。");
            sb1.AppendLine(">=3：排在星期三之後，包含星期三；意即星期三、星期四、星期五、星期六。");
            sb1.AppendLine("<>：不排在星期三；意即除了星期三之外均可排課。");

            TextInputeForm htf = new TextInputeForm(sb1);
            htf.Width = 500;
            htf.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine("節次條件說明：");
            sb1.AppendLine("設定課程排課時，允許排入的「節次條件」。");
            sb1.AppendLine("像是您可以指定「國文課」需固定排在「第三節」。");
            sb1.AppendLine("");
            sb1.AppendLine("樣式：");
            sb1.AppendLine("=3：排在第三節。");
            sb1.AppendLine("<3：排在第三節之前，但不包含第三節；意即第二節、第一節。");
            sb1.AppendLine(">3：排在第三節之後，但不包含第三節；意即第四節、第五節、第六節。");
            sb1.AppendLine("<=3：排在第三節之前，包含第三節；意即第三節、第二節、第一 節。");
            sb1.AppendLine(">=3：排在第三節之後，包含第三節；意即第三節、第四節、第五節、第六節。");
            sb1.AppendLine("<>：不排在第三節；意即除了第三節之外均可排課。");

            TextInputeForm htf = new TextInputeForm(sb1);
            htf.Width = 500;
            htf.ShowDialog();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb2 = new StringBuilder();
            sb2.AppendLine("隔天排課說明：");
            sb2.AppendLine("這個條件可以協助設定課程可進行「隔天排課」的規則");
            sb2.AppendLine("像是體育課(可設定「隔天排課」好讓學生的「體育服裝」有換洗的時間。)");
            sb2.AppendLine("");
            sb2.AppendLine("系統預設「無隔天排課」條件。");

            TextInputeForm htf = new TextInputeForm(sb2);
            htf.Height = 170;
            htf.ShowDialog();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb2 = new StringBuilder();
            sb2.AppendLine("同天排課說明：");
            sb2.AppendLine("這個條件可以協助設定課程中的多個分段是否允許安排在同天");
            sb2.AppendLine("像是上午「第一節」已排國文，是否仍允許同天的「其它節次」再排國文。");
            sb2.AppendLine("");
            sb2.AppendLine("系統預設「不可同天排課」。");

            TextInputeForm htf = new TextInputeForm(sb2);
            htf.Height = 170;
            htf.ShowDialog();
        }

        目前教師 _teacher = 目前教師.教師1;

        private void btnTeacherName1_Click(object sender, EventArgs e)
        {
            btnTeacherName.Text = btnTeacherName1.Text; //切換按鈕名稱
            _teacher = 目前教師.教師1; //切換狀態
            cmbTeacherName1.Visible = true;
            cmbTeacherName2.Visible = false;
            cmbTeacherName3.Visible = false;
        }

        private void btnTeacherName2_Click(object sender, EventArgs e)
        {
            btnTeacherName.Text = btnTeacherName2.Text; //切換按鈕名稱
            _teacher = 目前教師.教師2; //切換狀態
            cmbTeacherName1.Visible = false;
            cmbTeacherName2.Visible = true;
            cmbTeacherName3.Visible = false;
        }

        private void btnTeacherName3_Click(object sender, EventArgs e)
        {
            btnTeacherName.Text = btnTeacherName3.Text; //切換按鈕名稱
            _teacher = 目前教師.教師3; //切換狀態    
            cmbTeacherName1.Visible = false;
            cmbTeacherName2.Visible = false;
            cmbTeacherName3.Visible = true;
        }
    }


    enum 目前教師 { 教師1, 教師2, 教師3 }
}