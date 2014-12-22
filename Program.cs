using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Campus.DocumentValidator;
using Campus.Windows;
using DataRationality;
using FISCA;
using FISCA.Data;
using FISCA.Permission;
using FISCA.Presentation;
using FISCA.UDT;
using Sunset.NewCourse;
using Sunset.Properties;

namespace Sunset
{
    /// <summary>
    /// 模組進入點
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 主要進入函式
        /// </summary>
        [FISCA.MainMethod()]
        public static void Main()
        {
            #region 資料合理性檢查
            DataRationalityManager.Checks.Add(new CourseGroupRationality());
            DataRationalityManager.Checks.Add(new CourseRationality());
            DataRationalityManager.Checks.Add(new CourseExtensionRationality());
            DataRationalityManager.Checks.Add(new CourseSectionRationality());
            DataRationalityManager.Checks.Add(new CourseExtensionDupRationality());
            #endregion

            ServerModule.AutoManaged("http://module.ischool.com.tw/module/89/ischedule/udm.xml");

            #region 模組啟用先同步Schmea

            K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration["排課前處理UDT載入設定"];

            bool checkClubUDT = false;
            string name = "排課前處理UDT_20130429_17";

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

                Manager.SyncSchema(new ClassExtension());
                Manager.SyncSchema(new Classroom());
                Manager.SyncSchema(new ClassroomBusy());
                Manager.SyncSchema(new CourseExtension());
                Manager.SyncSchema(new CourseSection());
                Manager.SyncSchema(new LLDistance());
                Manager.SyncSchema(new Location());
                Manager.SyncSchema(new TeacherBusy());
                Manager.SyncSchema(new ClassBusy());
                Manager.SyncSchema(new TeacherExtension());
                Manager.SyncSchema(new TimeTable());
                Manager.SyncSchema(new TimeTableSec());
                Manager.SyncSchema(new Config());

                cd[name] = "true";
                cd.Save();
            }
            #endregion

            EntryPoint.Register(cd);

            #region 自訂驗證規則
            FactoryProvider.FieldFactory.Add(new SunsetFieldValidatorFactory());
            FactoryProvider.RowFactory.Add(new SunsetRowValidatorFactory());
            #endregion

            #region 學生排課資料
            MotherForm.RibbonBarItems["學生", "資料統計"]["報表"]["排課相關報表"]["學生功課表"].Click += (sender, e) => new frmStudentReport().ShowDialog();
            #endregion

            #region 班級排課資料
            //MotherForm.RibbonBarItems["班級", "排課"]["匯入"].Image = Resources.Import_Image;
            //MotherForm.RibbonBarItems["班級", "排課"]["匯入"].Size = RibbonBarButton.MenuButtonSize.Large;
            //MotherForm.RibbonBarItems["班級", "排課"]["匯入"]["匯入班級排課資料"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0130"].Executable;
            //MotherForm.RibbonBarItems["班級", "排課"]["匯入"]["匯入班級排課資料"].Click += (sender, e) => (new ImportClassExtension()).Execute();
            //MotherForm.RibbonBarItems["班級", "排課"]["匯入"]["匯入班級排課資料"].Image = Resources.sunrise;
            //MotherForm.RibbonBarItems["班級", "排課"]["匯出"].Image = Resources.Export_Image;
            //MotherForm.RibbonBarItems["班級", "排課"]["匯出"]["匯出班級排課資料"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0140"].Executable;
            //MotherForm.RibbonBarItems["班級", "排課"]["匯出"]["匯出班級排課資料"].Click += (sender, e) => ExportSunset.ExportClassExtenstion(K12.Presentation.NLDPanels.Class.SelectedSource);
            //MotherForm.RibbonBarItems["班級", "排課"]["匯出"]["匯出班級排課資料"].Image = Resources.sunrise;
            //MotherForm.RibbonBarItems["班級", "排課"]["匯出"].Size = RibbonBarButton.MenuButtonSize.Large;

            //ClassTimeTable ClassTimeTable = new ClassTimeTable();
            //ClassTimeTable.SetupTimeTableNameField();
            //ClassTimeTable.AddAssignTimeTableButtons();
            #endregion

            #region 預開課程
            MotherForm.RibbonBarItems["班級", "排課"]["預開課程"].Image = Resources.CourseIcon;
            MotherForm.RibbonBarItems["班級", "排課"]["預開課程"].Enable = false;
            MotherForm.RibbonBarItems["班級", "排課"]["預開課程"].Click += (sender, e) => new frmPreOpenCourse().ShowDialog();
            K12.Presentation.NLDPanels.Class.SelectedSourceChanged += (sender, e) => MotherForm.RibbonBarItems["班級", "排課"]["預開課程"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0150"].Executable && (K12.Presentation.NLDPanels.Class.SelectedSource.Count > 0);
            #endregion


            #region 課程排課資料
            //IDetailBulider CourseExtensionDetailBuilder = new FISCA.Presentation.DetailBulider<CourseExtensionEditor>();
            //K12.Presentation.NLDPanels.Course.AddDetailBulider(CourseExtensionDetailBuilder);
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"].Image = Resources.Import_Image;
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"]["匯入課程排課資料"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0060"].Executable;
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"]["匯入課程排課資料"].Click += (sender, e) => (new ImportCourseExtension()).Execute();
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"]["匯入課程排課資料"].Image = Resources.sunrise;
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"].Image = Resources.Export_Image;
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0070"].Executable;
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"]["匯出課程排課資料"].Click += (sender, e) => ExportSunset.ExportCourseExtension(K12.Presentation.NLDPanels.Course.SelectedSource);
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"]["匯出課程排課資料(含課程)"].Click += (sender, e) => ExportSunset.ExportCourseExtensionAndCourse(K12.Presentation.NLDPanels.Course.SelectedSource);
            //MotherForm.RibbonBarItems["課程", "排課"]["匯出"]["匯出課程排課資料(含課程分領域)"].Click += (sender, e) => ExportSunset.ExportCourseExtensionAndCourseByDomain(K12.Presentation.NLDPanels.Course.SelectedSource);

            MotherForm.RibbonBarItems["課程", "排課"]["批次產生課程分段"].Enable = false;
            MotherForm.RibbonBarItems["課程", "排課"]["批次產生課程分段"].Image = Resources.schedule_add_128;
            MotherForm.RibbonBarItems["課程", "排課"]["批次產生課程分段"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "排課"]["批次產生課程分段"].Click += (sender, e) =>
            {
                List<string> CourseIDs = K12.Presentation.NLDPanels.Course.SelectedSource;

                //建立Spliter
                FunctionSpliter<string, string> Spliter = new FunctionSpliter<string, string>(1000, 3);

                Spliter.Function = (x) => SunsetBL.CreateCourseSectionByCourseIDs(x);
                //Spliter.ProgressChange = x => MotherForm.SetStatusBarMessage(string.Empty, x);
                List<string> NewCourseSectionIDs = Spliter.Execute(CourseIDs);

                MsgBox.Show("已成功新增" + NewCourseSectionIDs.Count + "筆課程分段!");
            };

            //若有選取課程，以及有權限才將指定按鈕啟用
            K12.Presentation.NLDPanels.Course.SelectedSourceChanged += (sender, e) =>
            {
                int SelectedCount = K12.Presentation.NLDPanels.Course.SelectedSource.Count;
                bool IsCreateCourseSectionExecutable = UserAcl.Current["Sunset.Ribbon0120"].Executable;
                MotherForm.RibbonBarItems["課程", "排課"]["批次產生課程分段"].Enable = (SelectedCount > 0 && IsCreateCourseSectionExecutable);
            };

            Features.Register("Sunset/ImportCourseExtension", arg =>
            {
                if (FISCA.Permission.UserAcl.Current["Sunset.Ribbon0060"].Executable)
                {
                    new ImportCourseExtension().Execute();
                    CourseAdmin.Instance.LoadData();
                }
                else
                    MsgBox.Show("您無權限執行此功能!");
            });

            Features.Register("Sunset/ExportCourseExtension", arg =>
            {
                if (FISCA.Permission.UserAcl.Current["Sunset.Ribbon0070"].Executable)
                    ExportSunset.ExportCourseExtensionAndCourse(K12.Presentation.NLDPanels.Course.SelectedSource);
                else
                    MsgBox.Show("您無權限執行此功能!");
            });

            CourseClassroom CourseClassroom = new CourseClassroom();
            CourseClassroom.SetupClassroomNameField();

            CourseExtensionField CourseExtensionField = new CourseExtensionField();
            CourseExtensionField.SetupTimeTableNameField();
            CourseExtensionField.SetupGroupNameField();
            CourseExtensionField.AddAssignTimeTableButtons();
            CourseClassroom.AddAssignClassroomButtons();
            CourseExtensionField.AddAssignCourseGroupButtons();
            #endregion

            #region 課程分段資料
            //IDetailBulider CourseSectionDetailBuilder = new FISCA.Presentation.DetailBulider<CourseSectionEditor>();
            //K12.Presentation.NLDPanels.Course.AddDetailBulider(CourseSectionDetailBuilder);
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"].Image = Resources.Import_Image;
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"]["匯入課程分段資料"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0080"].Executable;
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"]["匯入課程分段資料"].Click += (sender, e) => (new ImportCourseSection()).Execute();
            MotherForm.RibbonBarItems["課程", "排課"]["匯入"]["匯入課程分段資料"].Image = Resources.sunrise;
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"].Image = Resources.Export_Image;
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"]["匯出課程分段資料"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0090"].Executable;
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"]["匯出課程分段資料"].Click += (sender, e) => ExportSunset.ExportCourseSection(K12.Presentation.NLDPanels.Course.SelectedSource);
            MotherForm.RibbonBarItems["課程", "排課"]["匯出"]["匯出課程分段資料"].Image = Resources.sunrise;

            Features.Register("Sunset/ImportCourseSection", arg =>
            {
                if (FISCA.Permission.UserAcl.Current["Sunset.Ribbon0080"].Executable)
                    new ImportCourseSection().Execute();
                else
                    MsgBox.Show("您無權限執行此功能!");
            });

            Features.Register("Sunset/ExportCourseSection", arg =>
            {
                if (FISCA.Permission.UserAcl.Current["Sunset.Ribbon0090"].Executable)
                    ExportSunset.ExportCourseSection(K12.Presentation.NLDPanels.Course.SelectedSource);
                else
                    MsgBox.Show("您無權限執行此功能!");
            });
            #endregion

            #region 教師不排課時段
            IDetailBulider TeacherBusyDetailBuilder = new FISCA.Presentation.DetailBulider<TeacherBusyEditor>();
            IDetailBulider TeacherExtensionDetailBuilder = new FISCA.Presentation.DetailBulider<TeacherExtensionEditor>();
            //IDetailBulider CourseExtensionDetailBuilder = new FISCA.Presentation.DetailBulider<CourseExtensionEditor>();
            //IDetailBulider CourseSectionDetailBuilder = new FISCA.Presentation.DetailBulider<CourseSectionEditor>();

            K12.Presentation.NLDPanels.Teacher.AddDetailBulider(TeacherBusyDetailBuilder);
            K12.Presentation.NLDPanels.Teacher.AddDetailBulider(TeacherExtensionDetailBuilder);

            //K12.Presentation.NLDPanels.Course.AddDetailBulider(CourseExtensionDetailBuilder);
            //K12.Presentation.NLDPanels.Course.AddDetailBulider(CourseSectionDetailBuilder);

            MotherForm.RibbonBarItems["教師", "排課"]["匯入"].Image = Resources.Import_Image;
            MotherForm.RibbonBarItems["教師", "排課"]["匯入"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["教師", "排課"]["匯入"]["匯入教師不排課時段"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0030"].Executable;
            MotherForm.RibbonBarItems["教師", "排課"]["匯入"]["匯入教師不排課時段"].Click += (sender, e) => (new ImportTeacherBusy()).Execute();
            MotherForm.RibbonBarItems["教師", "排課"]["匯入"]["匯入教師不排課時段"].Image = Resources.sunrise;
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"].Image = Resources.Export_Image;
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"]["匯出教師不排課時段"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0040"].Executable;
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"]["匯出教師不排課時段"].Click += (sender, e) => ExportSunset.ExportTeacherBusy(K12.Presentation.NLDPanels.Teacher.SelectedSource);
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"]["匯出教師不排課時段"].Image = Resources.sunrise;

            MotherForm.RibbonBarItems["教師", "排課"]["匯入"].Image = Resources.Import_Image;
            MotherForm.RibbonBarItems["教師", "排課"]["匯入"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["教師", "排課"]["匯入"]["匯入教師排課資料"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0035"].Executable;
            MotherForm.RibbonBarItems["教師", "排課"]["匯入"]["匯入教師排課資料"].Click += (sender, e) => (new ImportTeacherExtension()).Execute();
            MotherForm.RibbonBarItems["教師", "排課"]["匯入"]["匯入教師排課資料"].Image = Resources.sunrise;
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"].Image = Resources.Export_Image;
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"]["匯出教師排課資料"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0045"].Executable;
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"]["匯出教師排課資料"].Click += (sender, e) => ExportSunset.ExportTeacherExtension(K12.Presentation.NLDPanels.Teacher.SelectedSource);
            MotherForm.RibbonBarItems["教師", "排課"]["匯出"]["匯出教師排課資料"].Image = Resources.sunrise;


            Features.Register("Sunset/ImportTeacherBusy", arg =>
            {
                if (FISCA.Permission.UserAcl.Current["Sunset.Ribbon0030"].Executable)
                    new ImportTeacherBusy().Execute();
                else
                    MsgBox.Show("您無權限執行此功能!");
            });

            Features.Register("Sunset/ExportTeacherBusy", arg =>
            {
                if (FISCA.Permission.UserAcl.Current["Sunset.Ribbon0040"].Executable)
                    ExportSunset.ExportTeacherBusy(K12.Presentation.NLDPanels.Teacher.SelectedSource);
                else
                    MsgBox.Show("您無權限執行此功能!");
            });
            #endregion

            #region 班級不排課時段
            IDetailBulider ClassBusyDetailBuilder = new FISCA.Presentation.DetailBulider<ClassBusyEditor>();

            K12.Presentation.NLDPanels.Class.AddDetailBulider(ClassBusyDetailBuilder);

            MotherForm.RibbonBarItems["班級", "排課"]["匯入"].Image = Resources.Import_Image;
            MotherForm.RibbonBarItems["班級", "排課"]["匯入"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["班級", "排課"]["匯入"]["匯入班級不排課時段"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0220"].Executable;
            MotherForm.RibbonBarItems["班級", "排課"]["匯入"]["匯入班級不排課時段"].Click += (sender, e) => (new ImportClassBusy()).Execute();
            MotherForm.RibbonBarItems["班級", "排課"]["匯出"].Image = Resources.Export_Image;
            MotherForm.RibbonBarItems["班級", "排課"]["匯出"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["班級", "排課"]["匯出"]["匯出班級不排課時段"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon0140"].Executable;
            MotherForm.RibbonBarItems["班級", "排課"]["匯出"]["匯出班級不排課時段"].Click += (sender, e) => ExportSunset.ExportClassBusy(K12.Presentation.NLDPanels.Class.SelectedSource);
            #endregion


            #region 學年度學期日期對應
            //MotherForm.RibbonBarItems["教務作業", "調代課作業"]["學年度學期日期"].Image = Resources.x_office_calendar;
            //MotherForm.RibbonBarItems["教務作業", "調代課作業"]["學年度學期日期"].Size = RibbonBarButton.MenuButtonSize.Large;
            //MotherForm.RibbonBarItems["教務作業", "調代課作業"]["學年度學期日期"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon1010"].Executable;
            //MotherForm.RibbonBarItems["教務作業", "調代課作業"]["學年度學期日期"].Click += (sender, e) => new frmSchoolYearSemesterDate().ShowDialog();

            //Features.Register("Sunset/SchoolYearSemesterDate", arg =>
            //{
            //    if (FISCA.Permission.UserAcl.Current["Sunset.Ribbon10101"].Executable)
            //        new frmSchoolYearSemesterDate().ShowDialog();
            //    else
            //        MsgBox.Show("您無權限執行此功能!");
            //});
            #endregion

            #region 批次產生週課表
            //MotherForm.RibbonBarItems["教務作業", "排課"]["批次產生週課表"].Image = Resources.calendar;
            //MotherForm.RibbonBarItems["教務作業", "排課"]["批次產生週課表"].Size = RibbonBarButton.MenuButtonSize.Large;
            //MotherForm.RibbonBarItems["教務作業", "排課"]["批次產生週課表"].Enable = FISCA.Permission.UserAcl.Current["Sunset.Ribbon1020"].Executable;
            //MotherForm.RibbonBarItems["教務作業", "排課"]["批次產生週課表"].Click += (sender, e) => new frmCreateWeekCourseSection().ShowDialog();

            //Features.Register("Sunset/CreateWeekCourseSection", arg =>
            //{
            //    if (FISCA.Permission.UserAcl.Current["Sunset.Ribbon1020"].Executable)
            //        new frmCreateWeekCourseSection().ShowDialog();
            //    else
            //        MsgBox.Show("您無權限執行此功能!");
            //});
            #endregion

            #region 權限代碼註冊
            FISCA.Permission.Catalog TeacherDetailContentCatalog = FISCA.Permission.RoleAclSource.Instance["教師"]["資料項目"];
            TeacherDetailContentCatalog.Add(new DetailItemFeature("Sunset.Detail0010", "教師不排課時段"));
            TeacherDetailContentCatalog.Add(new DetailItemFeature("Sunset.Detail0040", "教師排課資料"));

            FISCA.Permission.Catalog CourseDetailContentCatalog = FISCA.Permission.RoleAclSource.Instance["課程"]["資料項目"];
            CourseDetailContentCatalog.Add(new DetailItemFeature("Sunset.Detail0020", "課程排課資料"));
            CourseDetailContentCatalog.Add(new DetailItemFeature("Sunset.Detail0030", "課程分段資料"));

            FISCA.Permission.Catalog CourseCatalog = FISCA.Permission.RoleAclSource.Instance["課程"]["功能按鈕"];
            CourseCatalog.Add(new RibbonFeature("Sunset.Ribbon0060", "匯入課程排課資料"));
            CourseCatalog.Add(new RibbonFeature("Sunset.Ribbon0070", "匯出課程排課資料"));
            CourseCatalog.Add(new RibbonFeature("Sunset.Ribbon0080", "匯入課程分段資料"));
            CourseCatalog.Add(new RibbonFeature("Sunset.Ribbon0090", "匯出課程分段資料"));
            CourseCatalog.Add(new RibbonFeature("Sunset.Ribbon0100", "指定課程時間表"));
            CourseCatalog.Add(new RibbonFeature("Sunset.Ribbon0110", "指定課程預設場地"));
            CourseCatalog.Add(new RibbonFeature("Sunset.Ribbon0120", "批次產生課程分段"));

            FISCA.Permission.Catalog TeacherCatalog = FISCA.Permission.RoleAclSource.Instance["教師"]["功能按鈕"];
            TeacherCatalog.Add(new RibbonFeature("Sunset.Ribbon0030", "匯入教師不排課時段"));
            TeacherCatalog.Add(new RibbonFeature("Sunset.Ribbon0035", "匯入教師排課資料"));
            TeacherCatalog.Add(new RibbonFeature("Sunset.Ribbon0040", "匯出教師不排課時段"));
            TeacherCatalog.Add(new RibbonFeature("Sunset.Ribbon0045", "匯出教師排課資料"));

            //AdminCatalog.Add(new RibbonFeature("Sunset.Ribbon1010", "學年度學期對應"));
            //AdminCatalog.Add(new RibbonFeature("Sunset.Ribbon1020", "批次產生週課表"));

            FISCA.Permission.Catalog ClassCatalog = FISCA.Permission.RoleAclSource.Instance["班級"]["功能按鈕"];
            //ClassCatalog.Add(new RibbonFeature("Sunset.Ribbon0050", "指定班級時間表"));
            ClassCatalog.Add(new RibbonFeature("Sunset.Ribbon0220", "匯入班級不排課時段"));
            ClassCatalog.Add(new RibbonFeature("Sunset.Ribbon0140", "匯出班級不排課時段"));
            ClassCatalog.Add(new RibbonFeature("Sunset.Ribbon0150", "預開課程"));

            FISCA.Permission.Catalog ClassDetailContentCatalog = FISCA.Permission.RoleAclSource.Instance["班級"]["資料項目"];
            ClassDetailContentCatalog.Add(new DetailItemFeature("Sunset.Detail0050", "班級不排課時段"));

            FISCA.Permission.Catalog ischeduleCatalog = FISCA.Permission.RoleAclSource.Instance["排調代課系統"];
            //ClassCatalog.Add(new RibbonFeature("Sunset.Ribbon0050", "指定班級時間表"));
            //ClassCatalog.Add(new RibbonFeature("Sunset.Ribbon0130", "匯入班級排課資料"));
            //ClassCatalog.Add(new RibbonFeature("Sunset.Ribbon0140", "匯出班級排課資料"));
            ischeduleCatalog.Add(new RibbonFeature("Sunset.Ribbon0200", "排課主系統"));
            ischeduleCatalog.Add(new RibbonFeature("Sunset.Ribbon0210", "調代課主系統"));
            #endregion


        }  
    }
}