using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Campus.Windows;
using DevComponents.DotNetBar.Controls;
using FISCA.Permission;
using FISCA.UDT;
using FCode = FISCA.Permission.FeatureCodeAttribute;
using FISCA.Data;
using System.Data;

namespace Sunset
{
    /// <summary>
    /// 排課課程資料
    /// </summary>
    [FCode("Sunset.Detail0020", "課程排課資料")]
    public class CourseExtensionEditor : FISCA.Presentation.DetailContent, IContentEditor<CourseExtension>
    {
        private FeatureAce UserPermission;
        private BackgroundWorker mbkwCourseExtension = new BackgroundWorker();
        private AccessHelper mAccessHelper = new AccessHelper();
        private bool IsBusy = false;
        private ChangeListener DataListener;
        private CourseExtension mCourseExtension;
        private decimal? Credit;
        private decimal? Period;
        List<TimeTable> mTimeTables;
        List<Classroom> mClassrooms;

        #region Components
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTimeTable;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSplitSpec;
        private ErrorProvider errSplitSpec;
        private IContainer components;
        private TextBoxX txtCourseAliasName;
        private DevComponents.DotNetBar.LabelX labelX7;
        private ErrorProvider errWeekdayCondition;
        private ErrorProvider errPeriodCondition;
        private DevComponents.DotNetBar.LabelX lblGroupName;
        private CheckBoxX chkLimitNextDay;
        private GroupPanel groupPanel1;
        private CheckBoxX chkLongBreak;
        private DevComponents.DotNetBar.LabelX labelX8;
        private TextBoxX txtPeriodCond;
        private TextBoxX txtWeekdayCond;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX5;
        private ComboBoxEx cmbWeekFlag;
        private DevComponents.DotNetBar.LabelX labelX4;
        private ComboBoxEx cmbClassroom;
        private DevComponents.DotNetBar.LabelX labelX3;
        private CheckBoxX chkAllowDup;
        #endregion

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public CourseExtensionEditor()
        {
            InitializeComponent();

            //設定資料項目名稱
            Group = "課程排課資料";
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

            string strCourseID = this.PrimaryKey;

            string strCondition = string.Format("ref_course_id={0}", strCourseID);

            #region 取得課程學分數或節數
            if (!string.IsNullOrWhiteSpace(strCourseID))
            {
                QueryHelper helper = new QueryHelper();

                DataTable table = helper.Select("select period,credit from course where id=" + strCourseID);

                if (table.Rows.Count == 1)
                {
                    Credit = K12.Data.Decimal.ParseAllowNull(""+table.Rows[0]["credit"]);
                    Period = K12.Data.Decimal.ParseAllowNull("" + table.Rows[0]["period"]);
                }
            }
            #endregion

            #region 依課程系統編號取得課程排課資料，若不存在則新增
            List<CourseExtension> mCourseExtensions = mAccessHelper.Select<CourseExtension>(strCondition);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(mCourseExtensions))
                mCourseExtension = mCourseExtensions[0];
            else
            {
                #region 若課程排課資料不存在則新增後再取得
                mCourseExtension = new CourseExtension();
                mCourseExtension.CourseID = K12.Data.Int.Parse(this.PrimaryKey);
                mCourseExtension.AllowDup = false;
                mCourseExtension.TimeTableID = null;
                mCourseExtension.ClassroomID = null;
                mCourseExtension.LongBreak = false;
                mCourseExtension.LimitNextDay = false;
                mCourseExtension.SplitSpec = string.Empty;
                mCourseExtension.WeekFlag = 3;
                mCourseExtension.WeekDayCond = string.Empty;
                mCourseExtension.PeriodCond = string.Empty;
                mCourseExtension.SubjectAliasName = string.Empty;

                List<CourseExtension> InsertRecords = new List<CourseExtension>();
                InsertRecords.Add(mCourseExtension);
                List<string> NewIDs = mAccessHelper.InsertValues(InsertRecords);

                if (NewIDs.Count > 0)
                {
                    List<CourseExtension> NewCourseExtensions = mAccessHelper.Select<CourseExtension>("UID="+NewIDs[0]);
                    if (NewCourseExtensions.Count > 0)
                        mCourseExtension = NewCourseExtensions[0];
                }
                #endregion
            }
            #endregion

            #region 取得所有時間表及場地
            mTimeTables = mAccessHelper
                .Select<TimeTable>()
                .OrderBy(x=>x.TimeTableName)
                .ToList();

            mClassrooms = mAccessHelper
                .Select<Classroom>()
                .OrderBy(x=>x.ClassroomName)
                .ToList();
            #endregion
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

            //設定排課課程資料
            Content = mCourseExtension;

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

            StringBuilder strBuilder = new StringBuilder();

            CourseExtension CourseExtension = Content;

            if (CourseExtension!=null)
            {
                List<CourseExtension> CourseExtensions = new List<CourseExtension>();
                CourseExtensions.Add(CourseExtension);

                mAccessHelper.SaveAll(CourseExtensions);
                strBuilder.AppendLine("已儲存課程排課資料");
            }

            SaveButtonVisible = false;
            CancelButtonVisible = false;

            this.Loading = true;

            DataListener.SuspendListen();         //終止變更判斷
            mbkwCourseExtension.RunWorkerAsync(); //背景作業,取得並重新填入原資料

            FISCA.Presentation.MotherForm.SetStatusBarMessage(strBuilder.ToString());
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
        }

        /// <summary>
        /// 取得或設定內容
        /// </summary>
        public CourseExtension Content
        {
            get
            {
                #region Step1:依名稱尋找是否有對應的時間表及場地
                TimeTable TimeTable = mTimeTables.Find(x => x.TimeTableName.Equals(""+cmbTimeTable.SelectedItem));
                Classroom Classroom = mClassrooms.Find(x => x.ClassroomName.Equals(""+cmbClassroom.SelectedItem));
                #endregion

                #region Step2:指定課程相關屬性
                mCourseExtension.TimeTableID = null;
                if (TimeTable != null ) 
                   mCourseExtension.TimeTableID = K12.Data.Int.ParseAllowNull(TimeTable.UID);
                mCourseExtension.SplitSpec = txtSplitSpec.Text;
                mCourseExtension.AllowDup = chkAllowDup.Checked;
                mCourseExtension.LimitNextDay = chkLimitNextDay.Checked;
                mCourseExtension.SubjectAliasName = txtCourseAliasName.Text;
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
                #region Step1:先尋找是否有對應的時間表及場地
                TimeTable TimeTable = mTimeTables.Find(x=>x.UID.Equals(K12.Data.Int.GetString(value.TimeTableID)));
                Classroom Classroom = mClassrooms.Find(x => x.UID.Equals(K12.Data.Int.GetString(value.ClassroomID)));
                #endregion

                #region Step2:指定課程相關屬性
                cmbTimeTable.SelectedItem = TimeTable!=null?TimeTable.TimeTableName:string.Empty;
                txtSplitSpec.Text = value.SplitSpec;
                chkAllowDup.Checked = value.AllowDup;
                chkLimitNextDay.Checked = value.LimitNextDay;
                txtCourseAliasName.Text = value.SubjectAliasName;
                #endregion

                #region Step3:指定課程分段預設屬性
                cmbClassroom.SelectedItem = Classroom!=null?Classroom.ClassroomName:string.Empty;
                chkLongBreak.Checked = value.LongBreak;
                txtWeekdayCond.Text = value.WeekDayCond;
                txtPeriodCond.Text = value.PeriodCond;
                cmbWeekFlag.SelectedItem = Utility.GetWeekFlagStr(value.WeekFlag);
                #endregion

                #region Step4:設定介面
                SaveButtonVisible = false;
                CancelButtonVisible = false;

                DataListener.Reset();
                DataListener.ResumeListen();
                #endregion
            }
        }

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
            DevComponents.DotNetBar.LabelX labelX1;
            this.cmbTimeTable = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtSplitSpec = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.errSplitSpec = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtCourseAliasName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.errWeekdayCondition = new System.Windows.Forms.ErrorProvider(this.components);
            this.errPeriodCondition = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblGroupName = new DevComponents.DotNetBar.LabelX();
            this.chkAllowDup = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkLimitNextDay = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chkLongBreak = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.txtPeriodCond = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtWeekdayCond = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.cmbWeekFlag = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cmbClassroom = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            labelX1 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.errSplitSpec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errWeekdayCondition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errPeriodCondition)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            labelX1.AutoSize = true;
            // 
            // 
            // 
            labelX1.BackgroundStyle.Class = "";
            labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            labelX1.Location = new System.Drawing.Point(29, 15);
            labelX1.Name = "labelX1";
            labelX1.Size = new System.Drawing.Size(87, 21);
            labelX1.TabIndex = 0;
            labelX1.Text = "上課時間表：";
            // 
            // cmbTimeTable
            // 
            this.cmbTimeTable.DisplayMember = "Text";
            this.cmbTimeTable.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTimeTable.FormattingEnabled = true;
            this.cmbTimeTable.ItemHeight = 19;
            this.cmbTimeTable.Location = new System.Drawing.Point(117, 13);
            this.cmbTimeTable.Name = "cmbTimeTable";
            this.cmbTimeTable.Size = new System.Drawing.Size(142, 25);
            this.cmbTimeTable.TabIndex = 1;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(309, 50);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(74, 21);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "分割設定：";
            // 
            // txtSplitSpec
            // 
            // 
            // 
            // 
            this.txtSplitSpec.Border.Class = "TextBoxBorder";
            this.txtSplitSpec.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSplitSpec.Location = new System.Drawing.Point(380, 49);
            this.txtSplitSpec.Name = "txtSplitSpec";
            this.txtSplitSpec.Size = new System.Drawing.Size(142, 25);
            this.txtSplitSpec.TabIndex = 3;
            this.txtSplitSpec.WatermarkText = "範例：1,2,2";
            // 
            // errSplitSpec
            // 
            this.errSplitSpec.ContainerControl = this;
            // 
            // txtCourseAliasName
            // 
            // 
            // 
            // 
            this.txtCourseAliasName.Border.Class = "TextBoxBorder";
            this.txtCourseAliasName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCourseAliasName.Location = new System.Drawing.Point(117, 49);
            this.txtCourseAliasName.Name = "txtCourseAliasName";
            this.txtCourseAliasName.Size = new System.Drawing.Size(142, 25);
            this.txtCourseAliasName.TabIndex = 8;
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(41, 50);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(74, 21);
            this.labelX7.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX7.TabIndex = 7;
            this.labelX7.Text = "科目簡稱：";
            // 
            // errWeekdayCondition
            // 
            this.errWeekdayCondition.ContainerControl = this;
            // 
            // errPeriodCondition
            // 
            this.errPeriodCondition.ContainerControl = this;
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            // 
            // 
            // 
            this.lblGroupName.BackgroundStyle.Class = "";
            this.lblGroupName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblGroupName.Location = new System.Drawing.Point(270, 54);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(0, 0);
            this.lblGroupName.TabIndex = 14;
            // 
            // chkAllowDup
            // 
            this.chkAllowDup.AutoSize = true;
            // 
            // 
            // 
            this.chkAllowDup.BackgroundStyle.Class = "";
            this.chkAllowDup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkAllowDup.Location = new System.Drawing.Point(309, 13);
            this.chkAllowDup.Name = "chkAllowDup";
            this.chkAllowDup.Size = new System.Drawing.Size(80, 21);
            this.chkAllowDup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkAllowDup.TabIndex = 15;
            this.chkAllowDup.Text = "同天排課";
            this.chkAllowDup.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // chkLimitNextDay
            // 
            this.chkLimitNextDay.AutoSize = true;
            // 
            // 
            // 
            this.chkLimitNextDay.BackgroundStyle.Class = "";
            this.chkLimitNextDay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkLimitNextDay.Location = new System.Drawing.Point(395, 13);
            this.chkLimitNextDay.Name = "chkLimitNextDay";
            this.chkLimitNextDay.Size = new System.Drawing.Size(94, 21);
            this.chkLimitNextDay.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkLimitNextDay.TabIndex = 16;
            this.chkLimitNextDay.Text = "不連天排課";
            this.chkLimitNextDay.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.Color.Transparent;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.chkLongBreak);
            this.groupPanel1.Controls.Add(this.labelX8);
            this.groupPanel1.Controls.Add(this.txtPeriodCond);
            this.groupPanel1.Controls.Add(this.txtWeekdayCond);
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Controls.Add(this.cmbWeekFlag);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.cmbClassroom);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(10, 80);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(530, 150);
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
            this.groupPanel1.TabIndex = 17;
            this.groupPanel1.Text = "產生課程分段預設值";
            // 
            // chkLongBreak
            // 
            this.chkLongBreak.AutoSize = true;
            // 
            // 
            // 
            this.chkLongBreak.BackgroundStyle.Class = "";
            this.chkLongBreak.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkLongBreak.Location = new System.Drawing.Point(367, 86);
            this.chkLongBreak.Name = "chkLongBreak";
            this.chkLongBreak.Size = new System.Drawing.Size(121, 21);
            this.chkLongBreak.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkLongBreak.TabIndex = 27;
            this.chkLongBreak.Text = "預設跨中午條件";
            this.chkLongBreak.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // labelX8
            // 
            this.labelX8.AutoSize = true;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.Class = "";
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelX8.Location = new System.Drawing.Point(4, 86);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(276, 21);
            this.labelX8.TabIndex = 26;
            this.labelX8.Text = "條件範例：=3、>3、>=3、<3、<=3、<>3";
            // 
            // txtPeriodCond
            // 
            // 
            // 
            // 
            this.txtPeriodCond.Border.Class = "TextBoxBorder";
            this.txtPeriodCond.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtPeriodCond.Location = new System.Drawing.Point(367, 50);
            this.txtPeriodCond.Name = "txtPeriodCond";
            this.txtPeriodCond.Size = new System.Drawing.Size(142, 25);
            this.txtPeriodCond.TabIndex = 25;
            // 
            // txtWeekdayCond
            // 
            // 
            // 
            // 
            this.txtWeekdayCond.Border.Class = "TextBoxBorder";
            this.txtWeekdayCond.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtWeekdayCond.Location = new System.Drawing.Point(104, 50);
            this.txtWeekdayCond.Name = "txtWeekdayCond";
            this.txtWeekdayCond.Size = new System.Drawing.Size(142, 25);
            this.txtWeekdayCond.TabIndex = 24;
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
            this.labelX6.Location = new System.Drawing.Point(271, 54);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(101, 21);
            this.labelX6.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX6.TabIndex = 23;
            this.labelX6.Text = "預設節次條件：";
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
            this.labelX5.Location = new System.Drawing.Point(4, 54);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(101, 21);
            this.labelX5.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX5.TabIndex = 22;
            this.labelX5.Text = "預設星期條件：";
            // 
            // cmbWeekFlag
            // 
            this.cmbWeekFlag.DisplayMember = "Text";
            this.cmbWeekFlag.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbWeekFlag.FormattingEnabled = true;
            this.cmbWeekFlag.ItemHeight = 19;
            this.cmbWeekFlag.Location = new System.Drawing.Point(367, 7);
            this.cmbWeekFlag.Name = "cmbWeekFlag";
            this.cmbWeekFlag.Size = new System.Drawing.Size(142, 25);
            this.cmbWeekFlag.TabIndex = 21;
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
            this.labelX4.Location = new System.Drawing.Point(258, 7);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(114, 21);
            this.labelX4.TabIndex = 20;
            this.labelX4.Text = "預設單雙週條件：";
            // 
            // cmbClassroom
            // 
            this.cmbClassroom.DisplayMember = "Text";
            this.cmbClassroom.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbClassroom.FormattingEnabled = true;
            this.cmbClassroom.ItemHeight = 19;
            this.cmbClassroom.Location = new System.Drawing.Point(104, 7);
            this.cmbClassroom.Name = "cmbClassroom";
            this.cmbClassroom.Size = new System.Drawing.Size(142, 25);
            this.cmbClassroom.TabIndex = 19;
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
            this.labelX3.Location = new System.Drawing.Point(4, 9);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(101, 21);
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX3.TabIndex = 18;
            this.labelX3.Text = "預設場地條件：";
            // 
            // CourseExtensionEditor
            // 
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.chkLimitNextDay);
            this.Controls.Add(this.chkAllowDup);
            this.Controls.Add(this.lblGroupName);
            this.Controls.Add(this.txtCourseAliasName);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.txtSplitSpec);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.cmbTimeTable);
            this.Controls.Add(labelX1);
            this.Name = "CourseExtensionEditor";
            this.Size = new System.Drawing.Size(550, 235);
            this.Load += new System.EventHandler(this.CourseExtensionEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errSplitSpec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errWeekdayCondition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errPeriodCondition)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
                return new Tuple<bool, string>(false,"未輸入學分數及節數！");
        }

        /// <summary>
        /// 資料項目載入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourseExtensionEditor_Load(object sender, EventArgs e)
        {
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
                    int TotalLen =0;
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

            //取得課程排課資料
            mbkwCourseExtension.DoWork += new DoWorkEventHandler(mbkwCourseExtension_DoWork);

            //完成取得課程排課資料
            mbkwCourseExtension.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mbkwCourseExtension_RunWorkerCompleted);

            //判斷DataGridView狀態變更
            DataListener = new ChangeListener();

            //加入TextBox狀態改變通知
            DataListener.Add(new TextBoxSource(txtWeekdayCond));
            DataListener.Add(new TextBoxSource(txtPeriodCond));
            DataListener.Add(new TextBoxSource(txtSplitSpec));
            DataListener.Add(new TextBoxSource(txtCourseAliasName));
            //加入ComboxBox狀態改變通知
            DataListener.Add(new ComboBoxSource(cmbClassroom, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbTimeTable, ComboBoxSource.ListenAttribute.Text));
            DataListener.Add(new ComboBoxSource(cmbWeekFlag, ComboBoxSource.ListenAttribute.Text));
            //加入CheckBox狀態改變通知
            //List<CheckBox> CheckBoxs = new List<CheckBox>() { chkAllowDup, chkLogBreak,chkLimitNextDay };

            //DataListener.Add(new CheckBoxSource(CheckBoxs.ToArray()));

            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(DataListener_StatusChanged);

            //終止變更判斷
            DataListener.SuspendListen();
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            this.SaveButtonVisible = true;
            this.CancelButtonVisible = true;
        }
    }
}