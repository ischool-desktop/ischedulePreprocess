using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DataRationality;
using FISCA;
using FISCA.Data;
using FISCA.Permission;
using FISCA.Presentation;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Data;
using Sunset.Properties;
using Sunset.Windows;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 新的課程進入點
    /// </summary>
    public static class EntryPoint
    {
        /// <summary>
        /// 註冊新的課程
        /// </summary>
        public static void Register(K12.Data.Configuration.ConfigData _cd)
        {
            MotherForm.AddPanel(CourseAdmin.Instance);

            #region 記錄是否曾經執行過SyncSchema

            K12.Data.Configuration.ConfigData cd = _cd;

            bool checkClubUDT = false;
            string name = "排課前處理UDT_20130429_27";

            //如果尚無設定值,預設為
            if (string.IsNullOrEmpty(cd[name]))
            {
                cd[name] = "false";
            }
            //檢查是否為布林
            bool.TryParse(cd[name], out checkClubUDT);
            if (!checkClubUDT)
            {
                SchemaManager Manager = new SchemaManager(FISCA.Authentication.DSAServices.DefaultConnection);
                Manager.SyncSchema(new SchedulerCourseExtension());
                Manager.SyncSchema(new SchedulerCourseSection());
                Manager.SyncSchema(new SchedulerProgramPlan());
                Manager.SyncSchema(new ClassEx());
                Manager.SyncSchema(new ClassExBusy());
                Manager.SyncSchema(new TeacherEx());
                Manager.SyncSchema(new TeacherExBusy());

                cd[name] = "true";
                cd.Save();
            }

            #endregion

            ////增加一個ListView
            //CourseAdmin.Instance.AddView(new CourseView());
            //課程的「科目」檢視。
            CourseAdmin.Instance.AddView(Course_SubjectView.Instnace); //科目
            CourseAdmin.Instance.AddView(Course_ClassNameView.Instnace); //班級
            CourseAdmin.Instance.AddView(Course_TeacherNameView.Instnace); //老師

            CourseAdmin.Instance.AddDetailBulider(new FISCA.Presentation.DetailBulider<CourseExtensionEditor>()); //課程
            CourseAdmin.Instance.AddDetailBulider(new FISCA.Presentation.DetailBulider<CourseSectionEditor>()); //課程分段
            //CourseAdmin.Instance.AddDetailBulider(new FISCA.Presentation.DetailBulider<CourseTimetableEditor>()); //排課
            //CourseAdmin.Instance.AddDetailBulider(new FISCA.Presentation.DetailBulider<CoursesBreakDefaultEditor>()); //課程分段預設值

            #region 資料合理性檢查

            //課程資料
            DataRationalityManager.Checks.Add(new SchedulerCourseRationality());
            //課程排課資料
            DataRationalityManager.Checks.Add(new SchedulerCourseExtensionRationality());
            //課程分段資料
            DataRationalityManager.Checks.Add(new SchedulerCourseSectionRationality());
            //課程分段群組
            DataRationalityManager.Checks.Add(new SchedulerCourseGroupRationality());
            //「課程班級」對應「ischool班級」
            DataRationalityManager.Checks.Add(new SchedulerCourseClassMapping());
            //「課程分段教師」對應「ischool教師」
            DataRationalityManager.Checks.Add(new SchedulerCourseSectionTeacherMapping());

            #endregion

            #region 課程/編輯
            RibbonBarItem rbEdit = MotherForm.RibbonBarItems["排課", "編輯"];
            RibbonBarItem rbItem = MotherForm.RibbonBarItems["排課", "資料統計"];

            RibbonBarButton rbAddButton = rbEdit["新增"];
            rbAddButton.Size = RibbonBarButton.MenuButtonSize.Large;
            rbAddButton.Image = Resources.btnAddCourse;
            rbAddButton.Enable = Permissions.新增課程權限;
            rbAddButton.Click += (sender, e) => new AddCourse().ShowDialog();

            #region 刪除

            RibbonBarButton rbDeleteButton = rbEdit["刪除"];
            rbDeleteButton.Size = RibbonBarButton.MenuButtonSize.Large;
            rbDeleteButton.Image = Resources.btnDeleteCourse;
            rbDeleteButton.Enable = false;
            rbDeleteButton.Click += (sender, e) =>
            {
                DeleteCourse();
            };

            #endregion

            #endregion

            #region 匯入匯出
            rbItem["匯入"].Image = Resources.Import_Image;
            rbItem["匯入"].Size = RibbonBarButton.MenuButtonSize.Large;

            rbItem["匯入"]["匯入課程資料"].Enable = Permissions.匯入課程資料權限;
            rbItem["匯入"]["匯入課程資料"].Click += (sender, e) => (new ImportSchedulerCourseExtension()).Execute();
            rbItem["匯入"]["匯入課程分段資料"].Enable = Permissions.匯入課程分段資料權限;
            rbItem["匯入"]["匯入課程分段資料"].Click += (sender, e) => (new ImportSchedulerCourseSection()).Execute();

            rbItem["匯出"].Image = Resources.Export_Image;
            rbItem["匯出"].Size = RibbonBarButton.MenuButtonSize.Large;

            rbItem["匯出"]["匯出課程資料"].Enable = Permissions.匯出課程資料權限;
            rbItem["匯出"]["匯出課程資料"].Click += (sender, e) => Export.ExportCourseExtension(CourseAdmin.Instance.SelectedSource);
            rbItem["匯出"]["匯出課程分段資料"].Enable = Permissions.匯出程分段資料權限;
            rbItem["匯出"]["匯出課程分段資料"].Click += (sender, e) => Export.ExportCourseSection(CourseAdmin.Instance.SelectedSource);
            #endregion

            #region 課程 - 課程規劃表
            MotherForm.RibbonBarItems["排課", "課程"]["管理課程規劃表"].Image = Properties.Resources.program_plan_128;
            MotherForm.RibbonBarItems["排課", "課程"]["管理課程規劃表"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "課程"]["管理課程規劃表"].Enable = Permissions.課程規劃表權限;
            MotherForm.RibbonBarItems["排課", "課程"]["管理課程規劃表"].Click += (sender, e) =>
            {
                Campus.Configuration.ConfigData config = Campus.Configuration.Config.User["EducationLevel"];

                string EducationLevel = config["EducationLevel"];

                if (string.IsNullOrEmpty(EducationLevel))
                {
                    frmComboBox Combobox = new frmComboBox("請選擇國中或高中", new List<string>() { "國中", "高中" });

                    if (Combobox.ShowDialog() == DialogResult.OK)
                    {
                        string Result = Combobox.SelectResult;

                        if (Result.Equals("國中"))
                            EducationLevel = "Junior";
                        else if (Result.Equals("高中"))
                            EducationLevel = "Senior";

                        config["EducationLevel"] = EducationLevel;
                        config.Save();
                    }
                }

                if (EducationLevel.Equals("Senior"))
                    new ConfigurationForm(new GraduationPlanConfiguration()).ShowDialog();
                else if (EducationLevel.Equals("Junior"))
                    new ProgramPlanManager().ShowDialog();
            };

            #endregion

            #region  課程 - 依課程規劃表開課
            MotherForm.RibbonBarItems["排課", "課程"]["依課程規劃開課"].Image = Properties.Resources.subject_add_128;
            MotherForm.RibbonBarItems["排課", "課程"]["依課程規劃開課"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "課程"]["依課程規劃開課"].Enable = Permissions.依課程規劃表開課權限;
            MotherForm.RibbonBarItems["排課", "課程"]["依課程規劃開課"].Click += (sender, e) => new frmOpenCourse().ShowDialog();
            #endregion

            #region 課程 - 產生課程分段
            MotherForm.RibbonBarItems["排課", "課程"]["產生課程分段"].Enable = false;
            MotherForm.RibbonBarItems["排課", "課程"]["產生課程分段"].Image = Resources.schedule_add_128;
            MotherForm.RibbonBarItems["排課", "課程"]["產生課程分段"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "課程"]["產生課程分段"].Click += (sender, e) =>
            {
                List<string> CourseIDs = CourseAdmin.Instance.SelectedSource;

                //建立Spliter
                FunctionSpliter<string, string> Spliter = new FunctionSpliter<string, string>(1000, 3);

                Spliter.Function = (x) => SunsetBL.CreateSchedulerCourseSectionByCourseIDs(x);
                //Spliter.ProgressChange = x => MotherForm.SetStatusBarMessage(string.Empty, x);
                List<string> NewCourseSectionIDs = Spliter.Execute(CourseIDs);

                MsgBox.Show("已成功新增" + NewCourseSectionIDs.Count + "筆課程分段!");
                CourseSectionEvents.RaiseChanged();
            };
            #endregion

            #region 課程 - 複製課程回ischool
            MotherForm.RibbonBarItems["排課", "課程"]["複製課程回ischool"].Enable = false;
            MotherForm.RibbonBarItems["排課", "課程"]["複製課程回ischool"].Image = Resources.schedule_add_64;
            MotherForm.RibbonBarItems["排課", "課程"]["複製課程回ischool"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "課程"]["複製課程回ischool"].Click += (sender, e) =>
            {

                CopyCourseIn_ischool copy = new CopyCourseIn_ischool();
                copy.ShowDialog();
            };

            #endregion

            MotherForm.RibbonBarItems["排課", "課程"]["班級教師檢查"].Enable = false;
            MotherForm.RibbonBarItems["排課", "課程"]["班級教師檢查"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "課程"]["班級教師檢查"].Image = Properties.Resources.trainning_info_64;
            MotherForm.RibbonBarItems["排課", "課程"]["班級教師檢查"].Click += delegate
            {
                TeacherAndClassCheck check = new TeacherAndClassCheck(CourseAdmin.Instance.SelectedSource);
                check.ShowDialog();
            };

            #region 指定分割設定
            MotherForm.RibbonBarItems["排課", "指定"]["分割設定"].Enable = false;
            MotherForm.RibbonBarItems["排課", "指定"]["分割設定"].Image = Resources.geometry_down_64;
            MotherForm.RibbonBarItems["排課", "指定"]["分割設定"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "指定"]["分割設定"].Click += (sender, e) =>
            {
                PartitioningSetup ps = new PartitioningSetup(CourseAdmin.Instance.SelectedSource);
                ps.Excute();
            };

            #endregion

            #region 開放查詢

            MotherForm.RibbonBarItems["排課", "指定"]["開放查詢"].Enable = false;
            MotherForm.RibbonBarItems["排課", "指定"]["開放查詢"].Image = Resources.lesson_planning_zoom_64;
            MotherForm.RibbonBarItems["排課", "指定"]["開放查詢"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "指定"]["開放查詢"]["開放"].Click += (sender, e) =>
            {
                Query();
            };

            MotherForm.RibbonBarItems["排課", "指定"]["開放查詢"]["不開放"].Click += (sender, e) =>
            {
                Not_Query();
            };

            #endregion

            #region 時間表管理
            MotherForm.RibbonBarItems["排課", "基本設定"]["時間表"].Image = Resources.lesson_planning_128;
            MotherForm.RibbonBarItems["排課", "基本設定"]["時間表"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "基本設定"]["時間表"].Enable = Permissions.時間表管理權限;
            MotherForm.RibbonBarItems["排課", "基本設定"]["時間表"].Click += delegate
            {
                TimeTableEditor vTimeTableEditor = new TimeTableEditor(); //單一時間表及時間表分段編輯嘼
                TimeTablePackageDataAccess vTimeTableDataAccess = new TimeTablePackageDataAccess(); //時間表資料存取
                winConfiguration<TimeTablePackage> configTimeTable = new winConfiguration<TimeTablePackage>(vTimeTableDataAccess, vTimeTableEditor); //時間表設定畫面

                configTimeTable.ShowDialog();
            };
            #endregion

            #region 場地管理
            ClassroomEditor vClassroomEditor = new ClassroomEditor();
            ClassroomPackageDataAccess vClassroomDataAccess = new ClassroomPackageDataAccess();
            winConfiguration<ClassroomPackage> configClassroom = new winConfiguration<ClassroomPackage>(vClassroomDataAccess, vClassroomEditor);

            MotherForm.RibbonBarItems["排課", "基本設定"]["場地"].Image = Resources.classroom_128;
            MotherForm.RibbonBarItems["排課", "基本設定"]["場地"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "基本設定"]["場地"].Enable = Permissions.場地管理權限;
            MotherForm.RibbonBarItems["排課", "基本設定"]["場地"].Click += delegate
            {
                vClassroomEditor.Prepare();
                configClassroom.ShowDialog();
            };

            //Features.Register("Sunset/Classroom", arg =>
            //{
            //    if (FISCA.Permission.UserAcl.Current["84b4232c-0d45-4f36-8af7-466f7a34011f"].Executable)
            //        configClassroom.ShowDialog();
            //    else
            //        MsgBox.Show("您無權限執行此功能!");
            //});
            #endregion

            #region 班級管理
            ClassEditor vClassEditor = new ClassEditor();
            ClassPackageDataAccess vClassDataAccess = new ClassPackageDataAccess();
            winConfiguration<ClassPackage> configClass = new winConfiguration<ClassPackage>(vClassDataAccess, vClassEditor);

            MotherForm.RibbonBarItems["排課", "基本設定"]["班級"].Image = Resources.classmate_128;
            MotherForm.RibbonBarItems["排課", "基本設定"]["班級"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "基本設定"]["班級"].Enable = Permissions.班級管理權限;
            MotherForm.RibbonBarItems["排課", "基本設定"]["班級"].Click += delegate
            {
                vClassEditor.Prepare();
                configClass.ShowDialog();
            };
            #endregion

            #region 教師管理
            TeacherEditor vTeacherEditor = new TeacherEditor();
            TeacherPackageDataAccess vTeacherDataAccess = new TeacherPackageDataAccess();
            winConfiguration<TeacherPackage> configTeacher = new winConfiguration<TeacherPackage>(vTeacherDataAccess, vTeacherEditor);

            MotherForm.RibbonBarItems["排課", "基本設定"]["教師"].Image = Resources.teacher_128;
            MotherForm.RibbonBarItems["排課", "基本設定"]["教師"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MotherForm.RibbonBarItems["排課", "基本設定"]["教師"].Enable = Permissions.教師管理權限;
            MotherForm.RibbonBarItems["排課", "基本設定"]["教師"].Click += delegate
            {
                vTeacherEditor.Prepare();
                configTeacher.ShowDialog();
            };
            #endregion

            #region 不開放查詢 ListPaneContexMenu
            CourseAdmin.Instance.ListPaneContexMenu["開放查詢"].BeginGroup = true;
            CourseAdmin.Instance.ListPaneContexMenu["開放查詢"].Enable = false;
            CourseAdmin.Instance.ListPaneContexMenu["開放查詢"].Image = Resources.lesson_planning_zoom_64;
            CourseAdmin.Instance.ListPaneContexMenu["開放查詢"]["開放"].Click += (sender, e) =>
            {
                Query();
            };

            CourseAdmin.Instance.ListPaneContexMenu["開放查詢"]["不開放"].Click += (sender, e) =>
            {
                Not_Query();
            };

            #endregion

            CourseAdmin.Instance.NavPaneContexMenu["重新整理"].Click += (sender, e) => CourseEvents.RaiseChanged();

            CourseAdmin.Instance.AddAssignClassroomButtons();
            //指定課程群組
            CourseAdmin.Instance.AddAssignCourseGroupButtons();
            CourseAdmin.Instance.AddAssignTimeTableButtons();

            #region 刪除 ListPaneContexMenu

            CourseAdmin.Instance.ListPaneContexMenu["刪除"].BeginGroup = true;
            CourseAdmin.Instance.ListPaneContexMenu["刪除"].Enable = false;
            CourseAdmin.Instance.ListPaneContexMenu["刪除"].Image = Resources.btnDeleteCourse;
            CourseAdmin.Instance.ListPaneContexMenu["刪除"].Click += (sender, e) =>
            {
                DeleteCourse();
            };

            #endregion

            #region 課程選擇變更事件

            //若有選取課程，以及有權限才將指定按鈕啟用
            CourseAdmin.Instance.SelectedSourceChanged += (sender, e) =>
            {
                bool SelectedCount = CourseAdmin.Instance.SelectedSource.Count > 0;

                MotherForm.RibbonBarItems["排課", "指定"]["分割設定"].Enable = (SelectedCount && Permissions.批次指定課程分割設定權限);
                CourseAdmin.Instance.ListPaneContexMenu["開放查詢"].Enable = (SelectedCount && Permissions.批次指定課程不開放查詢權限);
                MotherForm.RibbonBarItems["排課", "指定"]["開放查詢"].Enable = (SelectedCount && Permissions.批次指定課程不開放查詢權限);
                MotherForm.RibbonBarItems["排課", "課程"]["複製課程回ischool"].Enable = (SelectedCount && Permissions.複製課程回ischool權限);
                MotherForm.RibbonBarItems["排課", "課程"]["產生課程分段"].Enable = (SelectedCount && Permissions.批次產生課程分段權限);
                CourseAdmin.Instance.ListPaneContexMenu["刪除"].Enable = (SelectedCount && Permissions.刪除課程權限);
                MotherForm.RibbonBarItems["排課", "課程"]["班級教師檢查"].Enable = (SelectedCount && Permissions.班級教師檢查權限);
                //刪除課程
                rbDeleteButton.Enable = (SelectedCount && Permissions.刪除課程權限);
            };

            #endregion

            #region 權限註冊

            FISCA.Permission.Catalog CourseDetailContentCatalog = FISCA.Permission.RoleAclSource.Instance["排課"]["資料項目"];
            CourseDetailContentCatalog.Add(new DetailItemFeature(Permissions.課程資料項目, "課程"));
            //CourseDetailContentCatalog.Add(new DetailItemFeature(Permissions.排課資料項目, "排課"));
            //CourseDetailContentCatalog.Add(new DetailItemFeature(Permissions.課程分段預設值資料項目, "課程分段預設值"));
            CourseDetailContentCatalog.Add(new DetailItemFeature(Permissions.課程分段資料項目, "課程分段"));

            FISCA.Permission.Catalog CourseButtonCatalog = FISCA.Permission.RoleAclSource.Instance["排課"]["功能按鈕"];
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.新增課程, "新增課程"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.刪除課程, "刪除課程"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.場地管理, "場地管理"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.班級管理, "班級管理"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.教師管理, "教師管理"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.時間表管理, "時間表管理"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.課程規劃表, "管理課程規劃表"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.依課程規劃表開課, "依課程規劃表開課"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.指定課程時間表, "指定課程時間表"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.指定課程預設場地, "指定課程預設場地"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.批次產生課程分段, "批次產生課程分段"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.批次指定課程分割設定, "批次指定課程分割設定"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.批次指定課程不開放查詢, "批次指定課程不開放查詢"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.複製課程回ischool, "複製課程回ischool"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.班級教師檢查, "班級教師檢查"));
            CourseButtonCatalog.Add(new RibbonFeature(Permissions.重設國高中課規狀態, "重設課規樣式"));
            //目前沒有使用(4/12)
            //AdminCatalog.Add(new RibbonFeature(Permissions.開設課程, "開設課程"));

            FISCA.Permission.Catalog AdminCatalog = FISCA.Permission.RoleAclSource.Instance["排課"]["匯出匯入"];
            AdminCatalog.Add(new RibbonFeature(Permissions.匯出課程資料, "匯出課程資料"));
            AdminCatalog.Add(new RibbonFeature(Permissions.匯出程分段資料, "匯出程分段資料"));
            AdminCatalog.Add(new RibbonFeature(Permissions.匯入課程資料, "匯入課程資料"));
            AdminCatalog.Add(new RibbonFeature(Permissions.匯入課程分段資料, "匯入課程分段資料"));

            #endregion

            #region 其它

            //判斷當狀態為診斷模式時
            //此功能會被開啟
            if (FISCA.RTContext.IsDiagMode)
            {
                CourseButtonCatalog = FISCA.Permission.RoleAclSource.Instance["排課"]["開發"];
                CourseButtonCatalog.Add(new RibbonFeature(Permissions.重設國高中課規狀態, "重設課規樣式"));

                MotherForm.RibbonBarItems["排課", "開發"]["重設課規樣式"].Enable = Permissions.重設國高中課規狀態權限;
                MotherForm.RibbonBarItems["排課", "開發"]["重設課規樣式"].Click += (sender, e) =>
                {
                    DialogResult dr = MsgBox.Show("本功能會把課程規劃進行國/高中樣式的重置\n您確認要進行重置嗎?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                    //把課程規劃狀態重設
                    if (dr == DialogResult.Yes)
                    {
                        Campus.Configuration.ConfigData config = Campus.Configuration.Config.User["EducationLevel"];
                        config["EducationLevel"] = string.Empty; //清空
                        config.Save();
                    }
                    else
                    {
                        MsgBox.Show("已取消操作!!");
                    }
                };
            }

            #endregion
        }

        /// <summary>
        /// 刪除
        /// </summary>
        static private void DeleteCourse()
        {
            //當排課課程大於1時才進行刪除動作
            if (CourseAdmin.Instance.SelectedSource.Count >= 1)
            {
                //取得課程
                List<SchedulerCourseExtension> Courses = tool._A.Select<SchedulerCourseExtension>(CourseAdmin.Instance.SelectedSource);

                if (Courses.Count > 0)
                {
                    //取得課程分段
                    string Sql = string.Join("','", CourseAdmin.Instance.SelectedSource);
                    List<SchedulerCourseSection> CourseSections = tool._A.Select<SchedulerCourseSection>("ref_course_id in ('" + Sql.ToString() + "')");

                    string msg = string.Format("確定要刪除所選課程?\n(課程分段資料將被一併被刪除)");

                    if (MsgBox.Show(msg, "刪除課程", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        #region 實際刪除
                        Courses.ForEach(x => x.Deleted = true);
                        Courses.SaveAll();
                        CourseSections.ForEach(x => x.Deleted = true);
                        CourseSections.SaveAll();
                        #endregion

                        //LOG記錄
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("已刪除 " + Courses.Count + " 筆排課課程：");
                        foreach (SchedulerCourseExtension each in Courses)
                        {
                            sb.AppendLine("學年度「" + each.SchoolYear + "」學期「" + each.Semester + "」課程名稱「" + each.CourseName + "」");
                        }
                        FISCA.LogAgent.ApplicationLog.Log("排課", "刪除排課課程", sb.ToString());
                        CourseEvents.RaiseChanged();
                        MsgBox.Show("刪除成功!!");
                    }
                }
                else
                {
                    MsgBox.Show("未取得課程!!");
                }
            }
            else
            {
                MsgBox.Show("必須選擇課程才可以進行刪除動作!!");
            }
        }

        /// <summary>
        /// 開放查詢
        /// </summary>
        static private void Query()
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("已進行課程[開放查詢]檢查...請稍後");

            List<string> CourseIDs = CourseAdmin.Instance.SelectedSource;
            //取得課程物件
            List<SchedulerCourseExtension> CourseList = tool._A.Select<SchedulerCourseExtension>(CourseIDs);

            #region 檢查課程所屬班級

            //取得系統內的班級ID/班級名稱 清單
            DataTable dt1 = tool._Q.Select("select class_name from class");
            List<string> ClassNameList = new List<string>();

            foreach (DataRow row in dt1.Rows)
            {
                string class_name = "" + row["class_name"];

                if (!ClassNameList.Contains(class_name))
                {
                    ClassNameList.Add(class_name);
                }
            }
            //當排課課程[所屬班級]在ischool[班級]比不到資料時
            List<SchedulerCourseExtension> NotClassList = new List<SchedulerCourseExtension>();
            foreach (SchedulerCourseExtension each in CourseList)
            {
                if (!string.IsNullOrEmpty(each.ClassName))
                {
                    if (!ClassNameList.Contains(each.ClassName))
                    {
                        NotClassList.Add(each);
                    }
                }
            }

            #endregion

            #region 檢查課程授課教師

            DataTable dt2 = tool._Q.Select("select teacher_name,nickname from teacher where status=1");
            List<string> TeacherNameList = new List<string>();
            foreach (DataRow row in dt2.Rows)
            {
                string teacher_name = "" + row["teacher_name"];
                string nickname = "" + row["nickname"];
                if (string.IsNullOrEmpty(nickname))
                {
                    if (!TeacherNameList.Contains(teacher_name))
                    {
                        TeacherNameList.Add(teacher_name);
                    }
                }
                else
                {
                    teacher_name = teacher_name + "(" + nickname + ")";
                    if (!TeacherNameList.Contains(teacher_name))
                    {
                        TeacherNameList.Add(teacher_name);
                    }
                }
            }

            //當排課課程[任課教師]在ischool[教師]比不到資料時
            List<SchedulerCourseExtension> NotTeacherList1 = new List<SchedulerCourseExtension>();
            List<SchedulerCourseExtension> NotTeacherList2 = new List<SchedulerCourseExtension>();
            List<SchedulerCourseExtension> NotTeacherList3 = new List<SchedulerCourseExtension>();
            foreach (SchedulerCourseExtension each in CourseList)
            {
                if (!string.IsNullOrEmpty(each.TeacherName1))
                {
                    if (!TeacherNameList.Contains(each.TeacherName1))
                    {
                        NotTeacherList1.Add(each);
                    }
                }

                if (!string.IsNullOrEmpty(each.TeacherName2))
                {
                    if (!TeacherNameList.Contains(each.TeacherName2))
                    {
                        NotTeacherList2.Add(each);
                    }
                }

                if (!string.IsNullOrEmpty(each.TeacherName3))
                {
                    if (!TeacherNameList.Contains(each.TeacherName3))
                    {
                        NotTeacherList3.Add(each);
                    }
                }
            }
            #endregion

            #region 說明

            StringBuilder sb_1 = new StringBuilder();
            StringBuilder sb_2 = new StringBuilder();
            if (NotClassList.Count != 0)
            {
                foreach (SchedulerCourseExtension each in NotClassList)
                {
                    sb_1.AppendLine(string.Format("課程「{0}」所屬班級「{1}」", each.CourseName, each.ClassName));
                }
            }

            if (NotTeacherList1.Count + NotTeacherList2.Count + NotTeacherList3.Count != 0)
            {
                foreach (SchedulerCourseExtension each in NotTeacherList1)
                {
                    sb_2.AppendLine(string.Format("課程「{0}」教師一「{1}」", each.CourseName, each.TeacherName1));
                }
                foreach (SchedulerCourseExtension each in NotTeacherList2)
                {
                    sb_2.AppendLine(string.Format("課程「{0}」教師二「{1}」", each.CourseName, each.TeacherName2));
                }
                foreach (SchedulerCourseExtension each in NotTeacherList3)
                {
                    sb_2.AppendLine(string.Format("課程「{0}」教師三「{1}」", each.CourseName, each.TeacherName3));
                }
            }

            if (NotClassList.Count != 0 || NotTeacherList1.Count + NotTeacherList2.Count + NotTeacherList3.Count != 0)
            {
                HelpForm he = new HelpForm(sb_1.ToString(), sb_2.ToString());
                DialogResult dr = he.ShowDialog();
                if (dr == DialogResult.No)
                {
                    MotherForm.SetStatusBarMessage("已取消「課程開放查詢」作業!");

                    return;
                }
            }

            #endregion

            //根據選取的課程系統編號取得課程排課資料
            string strCondition = string.Join(",", CourseIDs.ToArray());

            string strSQL = "update $scheduler.scheduler_course_extension SET no_query=false where uid in (" + strCondition + ")";

            tool._Update.Execute(strSQL);

            #region Log

            StringBuilder sb_3 = new StringBuilder();
            sb_3.AppendLine("已將課程指定為「開放查詢」：");
            foreach (SchedulerCourseExtension each in CourseList)
            {
                sb_3.AppendLine("學年度「" + each.SchoolYear.ToString() + "」學期「" + each.Semester + "」課程名稱「" + each.CourseName + "」");
            }
            FISCA.LogAgent.ApplicationLog.Log("排課", "指定", sb_3.ToString());

            #endregion

            MotherForm.SetStatusBarMessage("已將 " + CourseAdmin.Instance.SelectedSource.Count + " 筆課程指定為「開放查詢」!");

            CourseAdmin.Instance.fieNoQuery.Reload();
        }


        /// <summary>
        /// 不開放查詢
        /// </summary>
        static private void Not_Query()
        {
            List<string> CourseIDs = CourseAdmin.Instance.SelectedSource;

            //根據選取的課程系統編號取得課程排課資料
            string strCondition = string.Join(",", CourseAdmin.Instance.SelectedSource.ToArray());

            string strSQL = "update $scheduler.scheduler_course_extension SET no_query=true where uid in (" + strCondition + ")";

            int result = tool._Update.Execute(strSQL);

            MotherForm.SetStatusBarMessage("已成功指定" + CourseIDs.Count + "筆課程「不開放」查詢!");

            CourseAdmin.Instance.fieNoQuery.Reload();

            //CourseEvents.RaiseChanged();
        }
    }
}