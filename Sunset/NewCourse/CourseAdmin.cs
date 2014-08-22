using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using FISCA.Data;
using FISCA.Permission;
using FISCA.Presentation;
using FISCA.UDT;
using K12.Data;
using K12.Data.Configuration;
using Sunset.Properties;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 課程畫面
    /// </summary>
    public class CourseAdmin : NLDPanel
    {
        private static CourseAdmin mCourseAdmin;
        /// <summary>
        /// 預設學年度學期
        /// </summary>
        private string DefaultFiltedSemester = K12.Data.School.DefaultSchoolYear + "學年度 第" + K12.Data.School.DefaultSemester + "學期";

        private BackgroundWorker bwSelectCourse = new BackgroundWorker();

        private Dictionary<string, SchedulerCourseExtension> Courses = new Dictionary<string, SchedulerCourseExtension>();
        private Dictionary<string, SchedulerCourseExtension> TempCourses = new Dictionary<string, SchedulerCourseExtension>();
        private Dictionary<string, string> mTimeTables = new Dictionary<string, string>();
        private Dictionary<string, string> mClassrooms = new Dictionary<string, string>();
        private Dictionary<string, string> mNoQuerys = new Dictionary<string, string>();

        /// <summary>
        /// 依照學年度學期分類課程
        /// </summary>
        private SortedDictionary<string, List<string>> SemesterCourseIDs = new SortedDictionary<string, List<string>>();
        private bool isbusy = false;

        public ListPaneField fieCourseName { get; set; } //名稱
        public ListPaneField fieSchoolYear { get; set; } //學年度
        public ListPaneField fieSemester { get; set; }   //學期
        public ListPaneField fieSubject { get; set; }   //科目
        public ListPaneField fiePeriod { get; set; }     //節數
        public ListPaneField fieTeacherName1 { get; set; } //授課教師一
        public ListPaneField fieTeacherName2 { get; set; } //授課教師二
        public ListPaneField fieTeacherName3 { get; set; } //授課教師三
        public ListPaneField fieClassName { get; set; } //授課教師三

        public ListPaneField fieTimeTable { get; set; }  //時間表
        public ListPaneField fieClassroom { get; set; }  //場地
        public ListPaneField fieGroupName { get; set; }  //群組名稱

        public ListPaneField fieNoQuery { get; set; }    //不開放查詢

        private AccessHelper mHelper = new AccessHelper();
        private MenuButton SearchCourseName;
        private MenuButton SearchTeacherName1;
        private MenuButton SearchTeacherName2;
        private MenuButton SearchTeacherName3;
        private SearchEventArgs SearEvArgs = null;

        /// <summary>
        /// 取得實體
        /// </summary>
        public static CourseAdmin Instance
        {
            get
            {
                if (mCourseAdmin == null)
                {
                    mCourseAdmin = new CourseAdmin();
                }
                return mCourseAdmin;
            }
        }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public CourseAdmin()
        {
            Group = "排課";

            Campus.Configuration.ConfigData cd = Campus.Configuration.Config.User["OptionPreference"];

            if (cd != null)
            {
                string FilterMenuText = cd["FilterMenu"];
                if (!string.IsNullOrEmpty(FilterMenuText))
                    DefaultFiltedSemester = FilterMenuText;
            }

            //建立ListView欄位
            SetupField();

            //建立左下方篩選條件
            SetupFilter();

            //建立搜尋
            SetupSearch();

            #region 待處理更新
            this.TempSourceChanged += (sender, e) =>
            {
                TempCourses.Clear();

                foreach (string CourseID in CourseAdmin.Instance.TempSource)
                {
                    if (Courses.ContainsKey(CourseID) && !TempCourses.ContainsKey(CourseID))
                        TempCourses.Add(CourseID, Courses[CourseID]);
                    else
                    {
                        List<SchedulerCourseExtension> vCourses = mHelper
                            .Select<SchedulerCourseExtension>("uid=" + CourseID);

                        if (vCourses.Count > 0 && !TempCourses.ContainsKey(CourseID))
                            TempCourses.Add(CourseID, vCourses[0]);
                    }
                }
            };
            #endregion

            //處理事件更新
            CourseEvents.CourseChanged += (sender, e) =>
            {
                if (bwSelectCourse.IsBusy)
                    isbusy = true;
                else
                    bwSelectCourse.RunWorkerAsync();
            };

            LoadData();
        }

        /// <summary>
        /// 建立指定場地按鈕
        /// </summary>
        public void AddAssignClassroomButtons()
        {
            //新增指定按鈕並加入事件
            RibbonBarButton AssignTimeTableButton = MotherForm.RibbonBarItems["排課", "指定"]["預設場地"];
            AssignTimeTableButton.Image = Resources.classroom_down_128;
            AssignTimeTableButton.Enable = Permissions.指定課程預設場地權限;
            AssignTimeTableButton.PopupOpen += new EventHandler<PopupOpenEventArgs>(AssignClassroomButton_PopupOpen); //在指定的事件中會新增所有時間表

            //新增不指定按鈕並加入事件
            MenuButton NoNoAssignTimeTableButton = AssignTimeTableButton["不指定"];
            NoNoAssignTimeTableButton.Tag = string.Empty;
            NoNoAssignTimeTableButton.Click += new EventHandler(ChangeClassroomID);

            //若有選取課程，以及有權限才將指定按鈕啟用
            CourseAdmin.Instance.SelectedSourceChanged += (sender, e) =>
            {
                int SelectedCount = CourseAdmin.Instance.SelectedSource.Count;
                AssignTimeTableButton.Enable = (SelectedCount > 0 && Permissions.指定課程預設場地權限);
            };
        }

        /// <summary>
        /// 建立各別場地按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssignClassroomButton_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            RibbonBarButton button = sender as RibbonBarButton;
            if (CourseAdmin.Instance.SelectedSource.Count <= 0)
                return;

            //針對每個場地建立按鈕
            foreach (Classroom Classroom in mHelper.Select<Classroom>())
            {
                MenuButton mb = e.VirtualButtons[Classroom.ClassroomName];
                mb.Tag = Classroom.UID;
                mb.Click += new EventHandler(ChangeClassroomID);
            }
        }

        /// <summary>
        /// 改變場地函式，其中場地系統編號放在Tag中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeClassroomID(object sender, EventArgs e)
        {
            //取得觸發事件的來源按鈕，並且取出時間表系統編號
            MenuButton MenuButton = sender as MenuButton;
            int? AssignClassroomID = K12.Data.Int.ParseAllowNull("" + MenuButton.Tag);

            //根據選取的課程系統編號取得課程排課資料
            string strCondition = string.Join(",", CourseAdmin.Instance.SelectedSource.ToArray());

            UpdateHelper helper = new UpdateHelper();

            string strSQL = "update $scheduler.scheduler_course_extension SET ref_classroom_id =" + (AssignClassroomID.HasValue ? "" + AssignClassroomID.Value : "null") + " where uid in (" + strCondition + ")";

            int result = helper.Execute(strSQL);

            fieClassroom.Reload();

            //CourseEvents.RaiseChanged();
        }

        /// <summary>
        /// 建立指定時間表按鈕
        /// </summary>
        public void AddAssignTimeTableButtons()
        {
            //新增指定按鈕並加入事件
            RibbonBarButton AssignTimeTableButton = MotherForm.RibbonBarItems["排課", "指定"]["預設時間表"];
            AssignTimeTableButton.Image = Resources.lesson_planning_down_128;
            AssignTimeTableButton.Enable = false;
            AssignTimeTableButton.PopupOpen += new EventHandler<PopupOpenEventArgs>(AssignButton_PopupOpen); //在指定的事件中會新增所有時間表

            //新增不指定按鈕並加入事件
            MenuButton NoAssignTimeTableButton = AssignTimeTableButton["不指定"];
            NoAssignTimeTableButton.Enable = Permissions.指定課程時間表權限;
            NoAssignTimeTableButton.Click += new EventHandler(ChangeTimeTableID);

            //若有選取班級，以及有權限才將指定按鈕啟用
            CourseAdmin.Instance.SelectedSourceChanged += (sender, e) =>
            {
                int SelectedCount = CourseAdmin.Instance.SelectedSource.Count;
                AssignTimeTableButton.Enable = SelectedCount > 0 && Permissions.指定課程時間表權限;
            };
        }

        /// <summary>
        /// 建立各別時間表按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssignButton_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            RibbonBarButton button = sender as RibbonBarButton;
            if (CourseAdmin.Instance.SelectedSource.Count <= 0)
                return;

            //針對每個時間表建立按鈕
            foreach (TimeTable TimeTable in mHelper.Select<TimeTable>())
            {
                MenuButton mb = e.VirtualButtons[TimeTable.TimeTableName];
                mb.Tag = TimeTable.UID;
                mb.Click += new EventHandler(ChangeTimeTableID);
            }
        }

        /// <summary>
        /// 改變時間表函式，其中時間表系統編號放在Tag中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeTimeTableID(object sender, EventArgs e)
        {
            //取得觸發事件的來源按鈕，並且取出時間表系統編號
            MenuButton MenuButton = sender as MenuButton;
            int? AssignTimeTableID = K12.Data.Int.ParseAllowNull("" + MenuButton.Tag);

            //根據選取的課程系統編號取得課程排課資料
            string strCondition = string.Join(",", CourseAdmin.Instance.SelectedSource.ToArray());

            UpdateHelper helper = new UpdateHelper();

            string strSQL = "update $scheduler.scheduler_course_extension SET ref_timetable_id =" + (AssignTimeTableID.HasValue ? "" + AssignTimeTableID.Value : "null") + " where uid in (" + strCondition + ")";

            int result = helper.Execute(strSQL);

            fieTimeTable.Reload();

            //CourseEvents.RaiseChanged();
        }

        /// <summary>
        /// 指定課程群組
        /// </summary>
        public void AddAssignCourseGroupButtons()
        {
            //新增指定按鈕並加入事件
            MenuButton AssignCourseGroupButton = CourseAdmin.Instance.ListPaneContexMenu["課程群組"];
            AssignCourseGroupButton.Enable = false;
            AssignCourseGroupButton.Image = Resources.virtualcurse_lock_128;
            AssignCourseGroupButton["設定"].Click += (sender, e) =>
            {
                frmSchedulerGroupCourse GroupCourse = new frmSchedulerGroupCourse(CourseAdmin.Instance.SelectedSource);

                if (GroupCourse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    CourseEvents.RaiseChanged();
            };

            //新增不指定按鈕並加入事件
            AssignCourseGroupButton["取消"].Click += (sender, e) =>
            {
                //根據選取的課程系統編號取得課程排課資料
                string strCondition = string.Join(",", CourseAdmin.Instance.SelectedSource.ToArray());
                List<SchedulerCourseExtension> CourseExtensions = mHelper
                    .Select<SchedulerCourseExtension>("uid in (" + strCondition + ")");

                CourseExtensions.ForEach(x => x.CourseGroup = string.Empty);
                mHelper.SaveAll(CourseExtensions);
                CourseEvents.RaiseChanged();
            };

            RibbonBarButton AssignCourseGroupButton1 = MotherForm.RibbonBarItems["排課", "指定"]["課程群組"];
            AssignCourseGroupButton1.Enable = false;
            AssignCourseGroupButton1.Image = Resources.virtualcurse_lock_128;
            AssignCourseGroupButton1["設定"].Click += (sender, e) =>
            {
                frmSchedulerGroupCourse GroupCourse = new frmSchedulerGroupCourse(CourseAdmin.Instance.SelectedSource);

                if (GroupCourse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    CourseEvents.RaiseChanged();
            };

            //新增不指定按鈕並加入事件
            //NoAssignCourseGroupButton.Enable = Permissions.指定課程時間表權限;
            //NoAssignCourseGroupButton.Image = Resources.virtualcurse_unlock_128;
            AssignCourseGroupButton1["取消"].Click += (sender, e) =>
            {
                //根據選取的課程系統編號取得課程排課資料
                string strCondition = string.Join(",", CourseAdmin.Instance.SelectedSource.ToArray());
                List<SchedulerCourseExtension> CourseExtensions = mHelper
                    .Select<SchedulerCourseExtension>("uid in (" + strCondition + ")");

                CourseExtensions.ForEach(x => x.CourseGroup = string.Empty);
                mHelper.SaveAll(CourseExtensions);
                CourseEvents.RaiseChanged();
            };

            //若有選取課程，以及有權限才將指定按鈕啟用
            CourseAdmin.Instance.SelectedSourceChanged += (sender, e) =>
            {
                int SelectedCount = CourseAdmin.Instance.SelectedSource.Count;
                AssignCourseGroupButton.Enable = SelectedCount > 0 && Permissions.指定課程時間表權限;
                AssignCourseGroupButton1.Enable = SelectedCount > 0 && Permissions.指定課程時間表權限;
            };
        }

        /// <summary>
        /// 載入資料
        /// </summary>
        public void LoadData()
        {
            #region 取得課程
            bwSelectCourse.DoWork += (sender, e) =>
            {
                SemesterCourseIDs = DataAccess.GetSemesterCourseIDs();
                isbusy = false;
            };

            bwSelectCourse.RunWorkerCompleted += (sender, e) =>
            {
                if (isbusy)
                    bwSelectCourse.RunWorkerAsync();
                else
                    SetFilterSource(FilterMenu.Text);

                fieCourseName.Column.DataGridView.Sort(fieCourseName.Column, ListSortDirection.Ascending);
            };

            bwSelectCourse.RunWorkerAsync();
            #endregion
        }

        /// <summary>
        /// 建立ListView欄位
        /// </summary>
        private void SetupField()
        {
            #region 課程名稱
            fieCourseName = new ListPaneField("課程名稱");
            fieCourseName.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = Courses[e.Key].CourseName;
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = TempCourses[e.Key].CourseName;
            };
            this.AddListPaneField(fieCourseName);
            #endregion

            #region 學年度
            fieSchoolYear = new ListPaneField("學年度");
            fieSchoolYear.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = K12.Data.Int.GetString(Courses[e.Key].SchoolYear);
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = K12.Data.Int.GetString(TempCourses[e.Key].SchoolYear);
            };
            this.AddListPaneField(fieSchoolYear);
            #endregion

            #region 學期
            fieSemester = new ListPaneField("學期");
            fieSemester.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = Courses[e.Key].Semester;
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = TempCourses[e.Key].Semester;
            };
            this.AddListPaneField(fieSemester);
            #endregion

            #region 科目
            fieSubject = new ListPaneField("科目");
            fieSubject.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = Courses[e.Key].Subject;
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = TempCourses[e.Key].Subject;
            };
            this.AddListPaneField(fieSubject);
            #endregion

            #region 節數
            fiePeriod = new ListPaneField("節數");
            fiePeriod.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = K12.Data.Int.GetString(Courses[e.Key].Period);
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = K12.Data.Int.GetString(TempCourses[e.Key].Period);
            };
            this.AddListPaneField(fiePeriod);
            #endregion

            #region 授課教師一
            fieTeacherName1 = new ListPaneField("授課教師一");
            fieTeacherName1.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = Courses[e.Key].TeacherName1;
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = TempCourses[e.Key].TeacherName1;
            };
            this.AddListPaneField(fieTeacherName1);
            #endregion

            #region 授課教師二
            fieTeacherName2 = new ListPaneField("授課教師二");
            fieTeacherName2.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = Courses[e.Key].TeacherName2;
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = TempCourses[e.Key].TeacherName2;
            };
            this.AddListPaneField(fieTeacherName2);
            #endregion

            #region 授課教師三
            fieTeacherName3 = new ListPaneField("授課教師三");
            fieTeacherName3.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = Courses[e.Key].TeacherName3;
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = TempCourses[e.Key].TeacherName3;
            };
            this.AddListPaneField(fieTeacherName3);
            #endregion

            #region 所屬班級
            fieClassName = new ListPaneField("所屬班級");
            fieClassName.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = Courses[e.Key].ClassName;
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = TempCourses[e.Key].ClassName;
            };
            this.AddListPaneField(fieClassName);
            #endregion

            #region 時間表
            fieTimeTable = new ListPaneField("時間表");
            fieTimeTable.PreloadVariableBackground += (sender, e) =>
            {
                mTimeTables.Clear();

                QueryHelper helper = new QueryHelper();

                string strSQL = "select $scheduler.scheduler_course_extension.uid,$scheduler.timetable.name from $scheduler.scheduler_course_extension inner join $scheduler.timetable on $scheduler.scheduler_course_extension.ref_timetable_id=$scheduler.timetable.uid";

                DataTable table = helper.Select(strSQL);

                foreach (DataRow row in table.Rows)
                {
                    string uid = row.Field<string>("uid");
                    string name = row.Field<string>("name");

                    if (!mTimeTables.ContainsKey(uid))
                        mTimeTables.Add(uid, name);
                }
            };

            fieTimeTable.GetVariable += (sender, e) =>
            {
                e.Value = mTimeTables.ContainsKey(e.Key) ? mTimeTables[e.Key] : string.Empty;

                //if (Courses.ContainsKey(e.Key))
                //    e.Value = mTimeTables.ContainsKey( Courses[e.Key].TimeTableID.HasValue?mTimeTables[Courses[e.Key].TimeTableID.Value].TimeTableName:string.Empty ;
                //else if (TempCourses.ContainsKey(e.Key))
                //    e.Value = TempCourses[e.Key].TimeTableID.HasValue ? mTimeTables[TempCourses[e.Key].TimeTableID.Value].TimeTableName : string.Empty;
            };
            this.AddListPaneField(fieTimeTable);
            #endregion

            #region 課程群組
            fieGroupName = new ListPaneField("群組");
            fieGroupName.GetVariable += (sender, e) =>
            {
                if (Courses.ContainsKey(e.Key))
                    e.Value = Courses[e.Key].CourseGroup;
                else if (TempCourses.ContainsKey(e.Key))
                    e.Value = TempCourses[e.Key].CourseGroup;
            };

            this.AddListPaneField(fieGroupName);
            #endregion

            #region 場地
            fieClassroom = new ListPaneField("場地");
            fieClassroom.PreloadVariableBackground += (sender, e) =>
            {
                mClassrooms.Clear();

                QueryHelper helper = new QueryHelper();

                string strSQL = "select $scheduler.scheduler_course_extension.uid,$scheduler.classroom.name from $scheduler.scheduler_course_extension inner join $scheduler.classroom on $scheduler.scheduler_course_extension.ref_classroom_id=$scheduler.classroom.uid";

                DataTable table = helper.Select(strSQL);

                foreach (DataRow row in table.Rows)
                {
                    string uid = row.Field<string>("uid");
                    string name = row.Field<string>("name");

                    if (!mClassrooms.ContainsKey(uid))
                        mClassrooms.Add(uid, name);
                }
            };
            fieClassroom.GetVariable += (sender, e) =>
            {
                e.Value = mClassrooms.ContainsKey(e.Key) ? mClassrooms[e.Key] : string.Empty;
            };

            this.AddListPaneField(fieClassroom);
            #endregion

            #region 不開放
            fieNoQuery = new ListPaneField("開放查詢");
            fieNoQuery.PreloadVariableBackground += (sender, e) =>
            {
                mNoQuerys.Clear();

                QueryHelper helper = new QueryHelper();

                string strSQL = "select uid,no_query from $scheduler.scheduler_course_extension";

                DataTable table = helper.Select(strSQL);

                foreach (DataRow row in table.Rows)
                {
                    string uid = row.Field<string>("uid");
                    string noquery = row.Field<string>("no_query");

                    if (!mNoQuerys.ContainsKey(uid))
                        mNoQuerys.Add(uid, noquery);
                }
            };
            fieNoQuery.GetVariable += (sender, e) =>
            {
                if (mNoQuerys.ContainsKey(e.Key))
                    e.Value = mNoQuerys[e.Key].Equals("false") ? "開放" : "不開放";
            };

            this.AddListPaneField(fieNoQuery);
            #endregion
        }

        /// <summary>
        /// 建立左下方篩選
        /// </summary>
        private void SetupFilter()
        {
            #region 篩選
            FilterMenu.SupposeHasChildern = true;
            FilterMenu.PopupOpen += delegate(object sender, PopupOpenEventArgs e)
            {
                foreach (string item in SemesterCourseIDs.Keys)
                {
                    MenuButton mb = e.VirtualButtons[item];
                    mb.AutoCheckOnClick = true;
                    mb.AutoCollapseOnClick = true;
                    mb.Checked = (item == DefaultFiltedSemester);
                    mb.Tag = item;
                    mb.CheckedChanged += delegate(object sender1, EventArgs e1)
                    {
                        MenuButton mb1 = sender1 as MenuButton;
                        SetFilterSource(mb1.Text);
                        DefaultFiltedSemester = FilterMenu.Text = mb1.Text;
                        mb1.Checked = true;
                        Campus.Configuration.ConfigData config = Campus.Configuration.Config.User["OptionPreference"];
                        config["FilterMenu"] = this.FilterMenu.Text;
                        config.Save();
                    };
                }
            };

            SetFilterSource(DefaultFiltedSemester);
            FilterMenu.Text = DefaultFiltedSemester;
            #endregion
        }

        /// <summary>
        /// 建立搜尋
        /// </summary>
        private void SetupSearch()
        {
            #region 搜尋
            ConfigData cd = School.Configuration["ischeduleCourseOptionPreference"];

            SearchCourseName = SearchConditionMenu["課程名稱"];
            SearchCourseName.AutoCheckOnClick = true;
            SearchCourseName.AutoCollapseOnClick = false;
            SearchCourseName.Checked = cd["SearchCourseName"]
                .ToLower()
                .Equals("false") ? false : true;
            SearchCourseName.Click += delegate
            {
                cd["SearchCourseName"] = "" + SearchCourseName.Checked;
                BackgroundWorker async = new BackgroundWorker();
                async.DoWork += delegate(object sender, DoWorkEventArgs e) { (e.Argument as ConfigData).Save(); };
                async.RunWorkerAsync(cd);
            };

            SearchTeacherName1 = SearchConditionMenu["授課教師一"];
            SearchTeacherName1.AutoCheckOnClick = true;
            SearchTeacherName1.AutoCollapseOnClick = false;
            SearchTeacherName1.Checked = cd["SearchTeacherName1"]
                .ToLower()
                .Equals("false") ? false : true;
            SearchTeacherName1.Click += delegate
            {
                cd["SearchTeacherName1"] = "" + SearchTeacherName1.Checked;
                BackgroundWorker async = new BackgroundWorker();
                async.DoWork += delegate(object sender, DoWorkEventArgs e) { (e.Argument as ConfigData).Save(); };
                async.RunWorkerAsync(cd);
            };

            SearchTeacherName2 = SearchConditionMenu["授課教師二"];
            SearchTeacherName2.AutoCheckOnClick = true;
            SearchTeacherName2.AutoCollapseOnClick = false;
            SearchTeacherName2.Checked = cd["SearchTeacherName2"]
                .ToLower()
                .Equals("false") ? false : true;
            SearchTeacherName2.Click += delegate
            {
                cd["SearchTeacherName2"] = "" + SearchTeacherName2.Checked;
                BackgroundWorker async = new BackgroundWorker();
                async.DoWork += delegate(object sender, DoWorkEventArgs e) { (e.Argument as ConfigData).Save(); };
                async.RunWorkerAsync(cd);
            };

            SearchTeacherName3 = SearchConditionMenu["授課教師三"];
            SearchTeacherName3.AutoCheckOnClick = true;
            SearchTeacherName3.AutoCollapseOnClick = false;
            SearchTeacherName3.Checked = cd["SearchTeacherName3"]
                .ToLower()
                .Equals("false") ? false : true;
            SearchTeacherName3.Click += delegate
            {
                cd["SearchTeacherName3"] = "" + SearchTeacherName3.Checked;
                BackgroundWorker async = new BackgroundWorker();
                async.DoWork += delegate(object sender, DoWorkEventArgs e) { (e.Argument as ConfigData).Save(); };
                async.RunWorkerAsync(cd);
            };

            this.Search += new EventHandler<SearchEventArgs>(AssnAdmin_Search);
            #endregion
        }

        private void SetFilterSource(string NowSemester)
        {
            if (SemesterCourseIDs.ContainsKey(NowSemester))
            {
                Courses = DataAccess.GetCourseByIDs(SemesterCourseIDs[NowSemester]);
                SetFilteredSource(SemesterCourseIDs[NowSemester]);
            }
            else
                SetFilteredSource(new List<string>());
        }

        void AssnAdmin_Search(object sender, SearchEventArgs e)
        {
            SearEvArgs = e;
            Campus.Windows.BlockMessage.Display("資料搜尋中,請稍候....", new Campus.Windows.ProcessInvoker(ProcessSearch));
        }

        private void ProcessSearch(Campus.Windows.MessageArgs args)
        {
            List<string> results = new List<string>();
            List<string> SearchConditions = new List<string>();
            List<string> SearchCourseSectionConditions = new List<string>();

            if (SearchCourseName.Checked)
                SearchConditions.Add("course_name like '%" + SearEvArgs.Condition + "%'");

            if (SearchTeacherName1.Checked)
            {
                string Condition = "teacher_name_1 like '%" + SearEvArgs.Condition + "%' ";

                SearchConditions.Add(Condition);
                SearchCourseSectionConditions.Add(Condition);
            }

            if (SearchTeacherName2.Checked)
            {
                string Condition = "teacher_name_2 like '%" + SearEvArgs.Condition + "%' ";
                SearchConditions.Add(Condition);
                SearchCourseSectionConditions.Add(Condition);
            }

            if (SearchTeacherName3.Checked)
            {
                string Condition = "teacher_name_3 like '%" + SearEvArgs.Condition + "%' ";
                SearchConditions.Add(Condition);
                SearchCourseSectionConditions.Add(Condition);
            }

            QueryHelper helper = new QueryHelper();

            if (SearchConditions.Count > 0)
            {
                string strCourseSQL = "select uid from $scheduler.scheduler_course_extension where " + string.Join(" or ", SearchConditions.ToArray());
                DataTable table = helper.Select(strCourseSQL);

                foreach (DataRow row in table.Rows)
                {
                    string UID = row.Field<string>("uid");

                    if (!results.Contains(UID))
                        results.Add(UID);
                }
            }

            if (SearchCourseSectionConditions.Count > 0)
            {
                string strCourseSectionSQL = "select ref_course_id from $scheduler.scheduler_course_section where " + string.Join(" or ", SearchCourseSectionConditions.ToArray());

                DataTable table = helper.Select(strCourseSectionSQL);

                foreach (DataRow row in table.Rows)
                {
                    string UID = row.Field<string>("ref_course_id");

                    if (!results.Contains(UID))
                        results.Add(UID);
                }
            }

            SearEvArgs.Result.AddRange(results);
        }
    }
}