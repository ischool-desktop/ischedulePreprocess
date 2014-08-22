using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FISCA.UDT;
using System.Drawing;
using Sunset.Windows;
using System.ComponentModel;

namespace Sunset
{
    /// <summary>
    /// 單一時間表（含時間表分段）編輯器
    /// 需要補上資料行驗證訊息。
    /// </summary>
    public partial class ClassroomEditor : UserControl, IContentEditor<ClassroomPackage>
    {
        private const int iWeekDay = 0;
        private const int iStartTime = 1;
        private const int iEndTime = 2;
        private const int iWeekflag = 3;
        private const int iBusyDesc = 4;
        private const string sWeekdayError = "星期必須介於1到7之間";
        private const string sPeriodError = "節次必須介於0到20之間，午休時段請輸入0。";
        private const string sDisplayPeriodError = "顯示節次必須為數字";
        private const string sBeginHourError = "開始小時必須介於0到23之間";
        private const string sBeginMinuteError = "開始小時必須介於0到59之間";
        private const string sDuration = "持續分鐘必須介於1到1140之間";
        private const string sPeriodConflict = "場地不排課時段不允許時間（星期、開始小時、開始分鐘、持續分鐘）有重疊";
        private const string sWeekdayPeriodDuplication = "星期節次不允許重覆";
        private AccessHelper mHelper = new AccessHelper();
        private ClassroomPackage mClassroomPackage;
        private TimeTableBusyEditor mTimeTableBusyEditor = null;
        private List<Period> mPeriods = new List<Period>();
        private string mEditorName;
        private bool mIsDirty = false;
        private int mSelectedRowIndex;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassroomEditor()
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

            foreach (DataGridViewRow row in grdClassroomBusys.Rows)
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
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Prepare()
        {
            mTimeTableBusyEditor = new TimeTableBusyEditor(grdTimeTableBusyEditor);

            BackgroundWorker BGW = new BackgroundWorker();

            List<TimeTable> TimeTables = new List<TimeTable>();
            List<Location> Locations = new List<Location>();

            TimeTables = mHelper.Select<TimeTable>();
            TimeTables.Sort(tool.SortTimeTables);

            Locations = mHelper.Select<Location>();


            cmbTimeTables.Items.Clear();
            TimeTables.ForEach(x => cmbTimeTables.Items.Add(x));

            if (cmbTimeTables.Items.Count > 0)
                cmbTimeTables.SelectedIndex = 0;

            cmbLocation.DisplayMember = "LocationName";
            cmbLocation.ValueMember = "UID";
            cmbLocation.DataSource = Locations;

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
        /// 取得或設定編輯內容
        /// </summary>
        public ClassroomPackage Content
        {
            get
            {
                #region 取得場地設定值
                mClassroomPackage.Classroom.Capacity = K12.Data.Int.Parse(txtCapacity.Text);
                mClassroomPackage.Classroom.LocationOnly = chkLocationOnly.Checked;
                int? LocationID = K12.Data.Int.ParseAllowNull("" + cmbLocation.SelectedValue);
                mClassroomPackage.Classroom.LocationID = LocationID;
                #endregion

                #region 從UI上取得場地忙碌時段
                mPeriods = mTimeTableBusyEditor.GetPeriods();

                List<ClassroomBusy> ClassroomBusys = new List<ClassroomBusy>();

                foreach (Period Period in mPeriods)
                {
                    int ID = K12.Data.Int.Parse(mClassroomPackage.Classroom.UID);
                    ClassroomBusys.Add(Period.ToClassroomBusy(ID));
                }

                mClassroomPackage.ClassroomBusys = ClassroomBusys;

                /*
                foreach (DataGridViewRow row in grdClassroomBusys.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Tag != null)
                    {
                        ClassroomBusy UpdateClassroomBusy = row.Tag as ClassroomBusy;

                        UpdateClassroomBusy.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);   //取得星期

                        #region 將畫面上的開始時間及結束時間，解析為開始時間及持續分鐘
                        string strStartTime = "" + row.Cells[iStartTime].Value;                           //取得開始時間
                        string strEndTime = "" + row.Cells[iEndTime].Value;                               //取得結束時間

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(strStartTime, strEndTime);

                        UpdateClassroomBusy.BeginTime = StorageTime.Item1;
                        UpdateClassroomBusy.Duration = StorageTime.Item2;                                   //取得持續時間
                        #endregion

                        string WeekFlag = "" + (row.Cells[iWeekflag] as DataGridViewComboBoxCell).Value;
                        int intWeekFlag = GetWeekFlagInt(WeekFlag);
                        UpdateClassroomBusy.WeekFlag = intWeekFlag;
                        UpdateClassroomBusy.BusyDesc = "" + row.Cells[iBusyDesc].Value;                     //取得不排課描述                        
                    }
                    else
                    {                        
                        ClassroomBusy InsertClassroomBusy = new ClassroomBusy();
                        InsertClassroomBusy.ClassroomID = K12.Data.Int.Parse(mClassroomPackage.Classroom.UID);          //設定場地系統編號
                        InsertClassroomBusy.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);   //取得星期

                        #region 將畫面上的開始時間及結束時間，解析為開始時間及持續分鐘
                        string strStartTime = "" + row.Cells[iStartTime].Value;                           //取得開始時間
                        string strEndTime = "" + row.Cells[iEndTime].Value;                               //取得結束時間

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(strStartTime, strEndTime);

                        InsertClassroomBusy.BeginTime = StorageTime.Item1;
                        InsertClassroomBusy.Duration = StorageTime.Item2;                                   //取得持續時間
                        #endregion

                        string WeekFlag = "" + (row.Cells[iWeekflag] as DataGridViewComboBoxCell).Value;
                        int intWeekFlag = GetWeekFlagInt(WeekFlag);
                        InsertClassroomBusy.WeekFlag = intWeekFlag;
                        InsertClassroomBusy.BusyDesc = "" + row.Cells[iBusyDesc].Value;                     //取得忙碌描述

                        mClassroomPackage.ClassroomBusys.Add(InsertClassroomBusy);                          //加入場地不排課時段
                    }
                }
                 */
                #endregion

                grdClassroomBusys.Rows.Clear();

                return mClassroomPackage;
            }
            set
            {
                #region 清空之前的資料
                txtCapacity.Text = string.Empty;
                chkLocationOnly.Checked = false;
                if (cmbLocation.Items.Count > 0)
                    cmbLocation.SelectedIndex = 0;
                grdClassroomBusys.Rows.Clear();
                #endregion

                if (value != null)
                {
                    mClassroomPackage = value;

                    var SortClassroomBusys = from vClassroomBusy in mClassroomPackage.ClassroomBusys orderby vClassroomBusy.WeekDay, vClassroomBusy.BeginTime select vClassroomBusy;

                    mClassroomPackage.ClassroomBusys = SortClassroomBusys.ToList();

                    if (mClassroomPackage.Classroom != null)
                    {
                        mEditorName = mClassroomPackage.Classroom.ClassroomName;
                        lblName.Text = mEditorName;
                        txtCapacity.Text = "" + mClassroomPackage.Classroom.Capacity;
                        chkLocationOnly.Checked = mClassroomPackage.Classroom.LocationOnly;
                        cmbLocation.SelectedValue = K12.Data.Int.GetString(mClassroomPackage.Classroom.LocationID);
                    }

                    mPeriods = new List<Period>();

                    if (!K12.Data.Utility.Utility.IsNullOrEmpty(mClassroomPackage.ClassroomBusys))
                    {
                        foreach (ClassroomBusy vClassromBusy in mClassroomPackage.ClassroomBusys)
                        {
                            Period Period = vClassromBusy.ToPeriod();
                            mPeriods.Add(Period);

                            int AddRowIndex = grdClassroomBusys.Rows.Add();
                            DataGridViewRow Row = grdClassroomBusys.Rows[AddRowIndex];
                            Row.Tag = vClassromBusy;

                            Tuple<string, string> DisplayTime = Utility.GetDisplayTime(vClassromBusy.BeginTime, vClassromBusy.Duration);

                            grdClassroomBusys.Rows[AddRowIndex].SetValues(
                                vClassromBusy.WeekDay,
                                DisplayTime.Item1,
                                DisplayTime.Item2,
                                GetWeekFlagStr(vClassromBusy.WeekFlag),
                                vClassromBusy.BusyDesc);
                        }
                    }

                    mTimeTableBusyEditor.SetPeriods(mPeriods);
                }

                IsDirty = false;
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
            foreach (DataGridViewRow row in grdClassroomBusys.Rows)
            {
                if (row.IsNewRow) continue;

                if (!string.IsNullOrEmpty(row.Cells[iWeekDay].ErrorText))
                    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iStartTime].ErrorText))
                    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iEndTime].ErrorText))
                    pass &= false;
            }

            if (!string.IsNullOrEmpty(errorProvider1.GetError(txtCapacity)))
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
            //若選取的Cell數量等於1則開始編輯
            if (grdClassroomBusys.SelectedCells.Count == 1)
                grdClassroomBusys.BeginEdit(true);
        }

        /// <summary>
        /// 開始欄位編輯事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        /// <summary>
        /// 結束欄位編輯事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 當欄位狀態改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridViewCell cell = grdClassroomBusys.CurrentCell;

            if (cell.ColumnIndex == iWeekDay)
            {
                int CheckWeekDay = K12.Data.Int.Parse("" + cell.Value);

                if (CheckWeekDay <= 0 || CheckWeekDay > 7)
                    cell.ErrorText = sWeekdayError;
                else if (IsPeriodConflict())
                    cell.ErrorText = sPeriodConflict;
                else
                    cell.ErrorText = string.Empty;
            }

            if ((cell.ColumnIndex == iStartTime) || (cell.ColumnIndex == iEndTime))
            {
                Tuple<bool, string> Result = Utility.IsValidateTime("" + cell.Value);

                string strStartTime = "" + grdClassroomBusys[iStartTime, cell.RowIndex].Value;
                string strEndTime = "" + grdClassroomBusys[iEndTime, cell.RowIndex].Value;

                if (!Result.Item1)
                    cell.ErrorText = Result.Item2;
                else if (IsPeriodConflict())
                    cell.ErrorText = sPeriodConflict;
                else if (!string.IsNullOrWhiteSpace(strStartTime) && !string.IsNullOrWhiteSpace(strEndTime)) //判斷兩者都不為空白才做檢查
                {
                    Tuple<DateTime, int> StorageTime = Utility.GetStorageTime("" + grdClassroomBusys[iStartTime, cell.RowIndex].Value, "" + grdClassroomBusys[iEndTime, cell.RowIndex].Value);

                    if (StorageTime.Item2 <= 0)
                        cell.ErrorText = "結束時間要大於開始時間！";
                    else
                        cell.ErrorText = string.Empty;
                }
                else
                    cell.ErrorText = string.Empty;
            }

            cell.Value = cell.EditedFormattedValue;
            grdClassroomBusys.EndEdit();
            grdClassroomBusys.BeginEdit(false);
        }

        /// <summary>
        /// 滑鼠右鍵用來刪除現有記錄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //判斷選取的資料行索引大於0，欄位索引小於0，並且按下滑鼠右鍵
            if (e.RowIndex >= 0 && e.ColumnIndex < 0 && e.Button == MouseButtons.Right)
            {
                //將目前選取的資料行索引記錄下來
                mSelectedRowIndex = e.RowIndex;

                //將目前選取的資料列，除了滑鼠右鍵所在的列外都設為不選取
                foreach (DataGridViewRow var in grdClassroomBusys.SelectedRows)
                {
                    if (var.Index != mSelectedRowIndex)
                        var.Selected = false;
                }

                //選取目前滑鼠所在的列
                grdClassroomBusys.Rows[mSelectedRowIndex].Selected = true;

                //顯示滑鼠右鍵選單
                contextMenuStripDelete.Show(grdClassroomBusys, grdClassroomBusys.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location);
            }
        }

        /// <summary>
        /// 使用者刪除資料時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            ClassroomBusy ClassroomBusy = e.Row.Tag as ClassroomBusy;

            if (ClassroomBusy != null)
                ClassroomBusy.Deleted = true;
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
            if (mSelectedRowIndex >= 0 && grdClassroomBusys.Rows.Count - 1 > mSelectedRowIndex)
            {
                ClassroomBusy ClassroomBusy = grdClassroomBusys.Rows[mSelectedRowIndex].Tag as ClassroomBusy;

                if (ClassroomBusy != null)
                    ClassroomBusy.Deleted = true;

                grdClassroomBusys.Rows.RemoveAt(mSelectedRowIndex);
            }
        }

        /// <summary>
        /// 當容納數內容改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCapacity_TextChanged(object sender, EventArgs e)
        {
            int a;
            if (!int.TryParse(txtCapacity.Text, out a))
                errorProvider1.SetError(txtCapacity, "班級容納數必須為數字");
            else
                errorProvider1.SetError(txtCapacity, string.Empty);

            IsDirty = true;
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

        private void tabControl1_DoubleClick(object sender, EventArgs e)
        {
            tabItem2.Visible = !tabItem2.Visible;
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            tabItem2.Visible = !tabItem2.Visible;
        }

        public void SetTitle(string name)
        {
            lblName.Text = name;
        }
    }
}