using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sunset.Windows;
using FISCA.UDT;
using System.Drawing;

namespace Sunset
{
    /// <summary>
    /// 單一時間表（含時間表分段）編輯器
    /// 需要補上資料行驗證訊息。
    /// </summary>
    public partial class ClassEditor : UserControl, IContentEditor<ClassPackage>
    {
        private const int iWeekDay = 0;
        private const int iStartTime = 1;
        private const int iEndTime = 2;
        private const int iBusyDesc = 3;
        private const string sWeekdayError = "星期必須介於1到7之間";
        private const string sPeriodError = "節次必須介於0到20之間，午休時段請輸入0。";
        private const string sDisplayPeriodError = "顯示節次必須為數字";
        private const string sBeginHourError = "開始小時必須介於0到23之間";
        private const string sBeginMinuteError = "開始小時必須介於0到59之間";
        private const string sDuration = "持續分鐘必須介於1到1140之間";
        private const string sPeriodConflict = "場地不排課時段不允許時間（星期、開始小時、開始分鐘、持續分鐘）有重疊";
        private const string sWeekdayPeriodDuplication = "星期節次不允許重覆";
        private AccessHelper mHelper = new AccessHelper();
        private TimeTableBusyEditor mTimeTableBusyEditor = null;
        private ClassPackage mClassPackage;
        private List<Period> mPeriods = new List<Period>();
        private int mSelectedRowIndex;
        private bool mIsDirty = false;
        private string mEditorName = string.Empty;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassEditor()
        {
            InitializeComponent();

            ReiEver();
        }

        #region private functions
        private int ParsePeriod(string strPeriod)
        {
            if (strPeriod.Equals("午休"))
                return 0;
            else
                return K12.Data.Int.Parse(strPeriod);
        }

        private int GetWeekFlagInt(string Weekflag)
        {
            switch (Weekflag)
            {
                case "單": return 1;
                case "雙": return 2;
                case "單雙": return 3;
                default: return 3;
            }
        }

        private string GetWeekFlagStr(int WeekFlag)
        {
            switch (WeekFlag)
            {
                case 1: return "單";
                case 2: return "雙";
                case 3: return "單雙";
                default: return "單雙";
            }
        }

        /// <summary>
        /// 檢查時間表所有時段是否有衝突
        /// </summary>
        /// <returns></returns>
        private bool IsPeriodConflict()
        {
            List<Period> Periods = new List<Period>();

            foreach (DataGridViewRow row in grdClassBusys.Rows)
            {
                if (row.IsNewRow) continue;

                Period NewPeriod = new Period();
                NewPeriod.Weekday = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);

                Tuple<DateTime, int> StorageTime = Utility.GetStorageTime("" + row.Cells[iStartTime].Value, "" + row.Cells[iEndTime].Value);

                NewPeriod.Hour = StorageTime.Item1.Hour;
                NewPeriod.Minute = StorageTime.Item1.Minute;
                NewPeriod.Duration = StorageTime.Item2;

                if (Periods.Count > 0)
                    foreach (Period Period in Periods)
                        if (IsTimeIntersectsWith(NewPeriod, Period))
                            return true;

                Periods.Add(NewPeriod);
            }
            return false;
        }

        /// <summary>
        /// 檢查兩個時間表時段是否有衝突
        /// </summary>
        /// <param name="BeginPeriod"></param>
        /// <param name="TestPeriod"></param>
        /// <returns></returns>
        private bool IsTimeIntersectsWith(Period BeginPeriod, Period TestPeriod)
        {
            //若星期不相同則不會相衝
            if (BeginPeriod.Weekday != TestPeriod.Weekday)
                return false;

            //將TestTime的年、月、日及秒設為與Appointment一致，以確保只是單純針對小時及分來做時間差的運算
            DateTime BeginTime = new DateTime(1900, 1, 1, BeginPeriod.Hour, BeginPeriod.Minute, 0);

            DateTime TestTime = new DateTime(1900, 1, 1, TestPeriod.Hour, TestPeriod.Minute, 0);

            //將Appointment的NewBeginTime減去NewTestTime
            int nTimeDif = (int)BeginTime.Subtract(TestTime).TotalMinutes;

            //狀況一：假設nTimeDif為正，並且大於NewTestTime，代表兩者沒有交集，傳回false。
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為9：00，其Duration為50。
            if (nTimeDif >= TestPeriod.Duration)
                return false;

            //狀況二：假設nTimeDiff為正，並且小於TestDuration，代表兩者有交集，傳回true。
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為9：00，其Duration為80。
            if (nTimeDif >= 0)
                return true;
            //狀況三：假設nTimeDiff為負值，將nTimeDiff成為正值，若是-nTimeDiff小於Appointment.Duration；
            //代表NewBeginTime在NewTestTime之前，並且NewBegin與NewTestTime的絕對差值小於Appointment.Duration的時間
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為10：30，其Duration為20。
            else if (-nTimeDif < BeginPeriod.Duration)
                return true;

            //其他狀況傳回沒有交集
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為10：50，其Duration為20。
            return false;
        }
        #endregion

        #region IContentEditor<List<TimeTableSec>> 成員

        private void ReiEver()
        {
            menuBusy.Click += (sender, e) =>
            {
                mTimeTableBusyEditor.SetBusy();
                IsDirty = true;
            };

            menuBusyDesc.Click += (sender, e) =>
            {
                mTimeTableBusyEditor.SetBusyDes();
                IsDirty = true;
            };

            menuFree.Click += (sender, e) =>
            {
                mTimeTableBusyEditor.SetFree();
                IsDirty = true;
            };

            grdTimeTableBusyEditor.CellDoubleClick += (sender, e) =>
            {
                foreach (DataGridViewCell Cell in grdTimeTableBusyEditor.SelectedCells)
                {
                    if (Cell.ColumnIndex != 0 && Cell.Style.BackColor != Color.LightGray)
                    {
                        Cell.Value = ("" + Cell.Value).Equals(string.Empty) ? mTimeTableBusyEditor.DefaultBusyMessage : string.Empty;
                        grdTimeTableBusyEditor.EndEdit();
                        IsDirty = true;
                    }
                }
            };

            txtGradeYear.TextChanged += (sender, e) =>
            {
                int vGradeYear;

                if (!int.TryParse(txtGradeYear.Text, out vGradeYear))
                    errGradeYear.SetError(txtGradeYear, "年級必須是數字！");
                else
                    errGradeYear.Clear();
                IsDirty = true;
            };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Prepare()
        {
            mTimeTableBusyEditor = new TimeTableBusyEditor(grdTimeTableBusyEditor);

            List<TimeTable> TimeTables = mHelper.Select<TimeTable>();
            TimeTables.Sort(tool.SortTimeTables);
            cmbTimeTables.Items.Clear();
            TimeTables.ForEach(x => cmbTimeTables.Items.Add(x));

            if (cmbTimeTables.Items.Count > 0)
                cmbTimeTables.SelectedIndex = 0;
        }

        /// <summary>
        /// 取得或設定編輯內容
        /// </summary>
        public ClassPackage Content
        {
            get
            {
                #region 取得場地設定值
                mClassPackage.Class.GradeYear = K12.Data.Int.ParseAllowNull(txtGradeYear.Text);
                #endregion

                #region 從UI上取得場地忙碌時段
                mPeriods = mTimeTableBusyEditor.GetPeriods();

                List<ClassExBusy> ClassBusys = new List<ClassExBusy>();

                foreach (Period Period in mPeriods)
                {
                    int ID = K12.Data.Int.Parse(mClassPackage.Class.UID);
                    ClassBusys.Add(Period.ToClassExBusy(ID));
                }

                mClassPackage.ClassBusys = ClassBusys;

                /*
                foreach (DataGridViewRow row in grdClassBusys.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Tag != null)
                    {
                        ClassExBusy UpdateClassBusy = row.Tag as ClassExBusy;

                        UpdateClassBusy.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);   //取得星期

                        #region 將畫面上的開始時間及結束時間，解析為開始時間及持續分鐘
                        string strStartTime = "" + row.Cells[iStartTime].Value;                           //取得開始時間
                        string strEndTime = "" + row.Cells[iEndTime].Value;                               //取得結束時間

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(strStartTime, strEndTime);

                        UpdateClassBusy.BeginTime = StorageTime.Item1;
                        UpdateClassBusy.Duration = StorageTime.Item2;                                   //取得持續時間
                        #endregion
                    
                        UpdateClassBusy.BusyDesc = "" + row.Cells[iBusyDesc].Value;                     //取得不排課描述                        
                    }
                    else
                    {                        
                        ClassExBusy InsertClassroomBusy = new ClassExBusy();
                        InsertClassroomBusy.ClassID = K12.Data.Int.Parse(mClassPackage.Class.UID);          //設定場地系統編號
                        InsertClassroomBusy.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);   //取得星期

                        #region 將畫面上的開始時間及結束時間，解析為開始時間及持續分鐘
                        string strStartTime = "" + row.Cells[iStartTime].Value;                           //取得開始時間
                        string strEndTime = "" + row.Cells[iEndTime].Value;                               //取得結束時間

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(strStartTime, strEndTime);

                        InsertClassroomBusy.BeginTime = StorageTime.Item1;
                        InsertClassroomBusy.Duration = StorageTime.Item2;                                   //取得持續時間
                        InsertClassroomBusy.BusyDesc = "" + row.Cells[iBusyDesc].Value;                     //取得忙碌描述
                        #endregion

                        mClassPackage.ClassBusys.Add(InsertClassroomBusy);
                    }
                }
                */
                #endregion

                grdClassBusys.Rows.Clear();

                return mClassPackage;
            }
            set
            {
                #region 清空之前的資料
                txtGradeYear.Text = string.Empty;
                if (cmbLocation.Items.Count > 0)
                    cmbLocation.SelectedIndex = 0;
                grdView.Rows.Clear();
                #endregion

                if (value != null)
                {
                    mClassPackage = value;

                    var SortClassBusys = from vClassBusy in mClassPackage.ClassBusys orderby vClassBusy.WeekDay, vClassBusy.BeginTime select vClassBusy;

                    mClassPackage.ClassBusys = SortClassBusys.ToList();

                    if (mClassPackage.Class != null)
                    {
                        mEditorName = mClassPackage.Class.ClassName;
                        txtGradeYear.Text = K12.Data.Int.GetString(mClassPackage.Class.GradeYear);
                    }

                    mPeriods = new List<Period>();

                    if (!K12.Data.Utility.Utility.IsNullOrEmpty(mClassPackage.ClassBusys))
                    {
                        foreach (ClassExBusy vClassBusy in mClassPackage.ClassBusys)
                        {
                            Period Period = vClassBusy.ToPeriod();
                            mPeriods.Add(Period);

                            int AddRowIndex = grdView.Rows.Add();
                            DataGridViewRow Row = grdView.Rows[AddRowIndex];
                            Row.Tag = vClassBusy;

                            Tuple<string, string> DisplayTime = Utility.GetDisplayTime(vClassBusy.BeginTime, vClassBusy.Duration);

                            grdView.Rows[AddRowIndex].SetValues(
                                vClassBusy.WeekDay,
                                DisplayTime.Item1,
                                DisplayTime.Item2,
                                vClassBusy.BusyDesc);
                        }
                    }

                    mTimeTableBusyEditor.SetPeriods(mPeriods);
                }

                IsDirty = false;
            }
        }

        /// <summary>
        /// 是否被修改
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return mIsDirty;
            }
            set
            {
                mIsDirty = value;

                lblName.Text = mIsDirty ? mEditorName + "<font color='red'>（已修改）</font>" : mEditorName;
            }
        }

        /// <summary>
        /// 將自己傳回
        /// </summary>
        public UserControl Control { get { return this; } }

        /// <summary>
        /// 驗證表單資料是否合法
        /// </summary>
        /// <returns></returns>
        public new bool Validate()
        {
            bool pass = true;
            foreach (DataGridViewRow row in grdClassBusys.Rows)
            {
                if (row.IsNewRow) continue;

                if (!string.IsNullOrEmpty(row.Cells[iWeekDay].ErrorText))
                    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iStartTime].ErrorText))
                    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iEndTime].ErrorText))
                    pass &= false;
            }

            if (!string.IsNullOrEmpty(errGradeYear.GetError(txtGradeYear)))
                pass &= false;

            return pass;
        }
        #endregion

        #region DataGrid事件
        /// <summary>
        ///  當進入到某個Cell引發的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 當欄位狀態改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 滑鼠右鍵用來刪除現有記錄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        /// <summary>
        /// 使用者刪除資料時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

        }
        #endregion

        /// <summary>
        /// 當按下刪除選單時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            //實際進行刪除列的動作
            if (mSelectedRowIndex >= 0 && grdClassBusys.Rows.Count - 1 > mSelectedRowIndex)
            {
                ClassExBusy ClassBusy = grdClassBusys.Rows[mSelectedRowIndex].Tag as ClassExBusy;

                if (ClassBusy != null)
                    ClassBusy.Deleted = true;

                grdClassBusys.Rows.RemoveAt(mSelectedRowIndex);
            }
        }

        private void cmbTimeTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mTimeTableBusyEditor == null)
                return;

            if (IsDirty)
                if (MessageBox.Show("您變更的資料尚未儲存，您確定要放棄變更的資料切換時間表？", "排課", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

            TimeTable TimeTable = cmbTimeTables.SelectedItem as TimeTable;

            if (TimeTable != null)
            {
                List<TimeTableSec> Secs = mHelper.Select<TimeTableSec>("ref_timetable_id=" + TimeTable.UID);

                mTimeTableBusyEditor.SetTimeTableSecs(Secs);
                mTimeTableBusyEditor.SetPeriods(mPeriods);
            }
        }

        private void tabControlPanel5_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_DoubleClick(object sender, EventArgs e)
        {
            tabItem4.Visible = !tabItem4.Visible;
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            tabItem4.Visible = !tabItem4.Visible;
        }

        public void SetTitle(string name)
        {
            lblName.Text = name;
        }
    }
}