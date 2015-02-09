using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sunset.Windows;


namespace Sunset
{
    /// <summary>
    /// 單一時間表（含時間表分段）編輯器
    /// </summary>
    public partial class TimeTableEditor : UserControl, IContentEditor<TimeTablePackage>
    {
        private const int iWeekDay = 0;
        private const int iPeriod = 1;
        //private const int iDispPeriod = 2;
        private const int iStartTime = 2;
        private const int iEndTime = 3;
        //private const int iLocation = 4;
        private const int iDisable = 4;
        private const int iDisableMessage = 5;
        private const string sWeekdayError = "星期必須介於1到7之間";
        private const string sPeriodError = "節次必須介於1到20之間，中午時段請輸入0、中午、休午或午。";
        private const string sDisplayPeriodError = "顯示節次必須為數字";
        private const string sBeginHourError = "開始小時必須介於0到23之間";
        private const string sBeginMinuteError = "開始小時必須介於0到59之間";
        private const string sDuration = "持續分鐘必須介於1到1140之間";
        private const string sTimeTableSecConflict = "時間表分段間不允許時間（星期、開始時間、結束時間）有重疊";
        private const string sWeekdayPeriodDuplication = "星期節次不允許重覆";
        //private AccessHelper mHelper = new AccessHelper();
        //private List<Location> mLocations;
        private TimeTablePackage mTimeTablePackage;
        private List<string> LongBreaks = new List<string>() {"中午","午休","午"};
        private string mEditorName = string.Empty;
        private bool mIsDirty = false;

        private int mSelectedRowIndex;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public TimeTableEditor()
        {
            InitializeComponent();

            grdTimeTableSecs.DataError += (sender, e) => { };
        
            Prepare();
        }

        #region private functions
        private int ParsePeriod(string strPeriod)
        {
            if (LongBreaks.Contains(strPeriod))
                return 0;
            else
                return K12.Data.Int.Parse(strPeriod);
        }

        private bool ParseBool(string strValue)
        {
            if (strValue.ToLower().Equals("false"))
                return false;
            else if (strValue.ToLower().Equals("true"))
                return true;
            else
                return false;
        }
        #endregion

        #region IContentEditor<List<TimeTableSec>> 成員
        /// <summary>
        /// 準備資料
        /// </summary>
        public void Prepare()
        {
            //mLocations = mHelper.Select<Location>();
            //DataGridViewComboBoxColumn Column = grdTimeTableSecs.Columns[iLocation] as DataGridViewComboBoxColumn;
            //Column.Items.Clear();
            //Column.Items.Add(string.Empty);
            //mLocations.ForEach(x => Column.Items.Add(x.LocationName));
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
        public TimeTablePackage Content
        {
            get
            {
                mTimeTablePackage.TimeTable.TimeTableDesc = txtTimeTableDesc.Text;

                foreach (DataGridViewRow row in grdTimeTableSecs.Rows)
                {
                    #region 從UI上取得時間表分段
                    if (row.IsNewRow) continue;

                    //Location vLocation = mLocations.Find(x=>x.LocationName.Equals(row.Cells[iLocation].Value));
                    //int? LocationID = null;
                    //if (vLocation!=null)
                    //   LocationID = K12.Data.Int.Parse(vLocation.UID);

                    if (row.Tag != null)
                    {
                        TimeTableSec UpdateTimeTableSec = row.Tag as TimeTableSec;

                        UpdateTimeTableSec.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);    //取得星期
                        UpdateTimeTableSec.Period = ParsePeriod("" + row.Cells[iPeriod].Value);             //取得節次
                        //UpdateTimeTableSec.DispPeriod = ParsePeriod("" + row.Cells[iDispPeriod].Value);     //取得顯示節次

                        #region 將畫面上的開始時間及結束時間，解析為開始時間及持續分鐘
                        string strStartTime = "" + row.Cells[iStartTime].Value;                              //取得開始時間
                        string strEndTime   = "" + row.Cells[iEndTime].Value;                                //取得結束時間

                        Tuple<DateTime,int> StorageTime = Utility.GetStorageTime(strStartTime, strEndTime);

                        UpdateTimeTableSec.BeginTime = StorageTime.Item1;                                    //將開始小時及開始分鐘轉成DateTime
                        UpdateTimeTableSec.Duration = StorageTime.Item2;                                     //取得持續時間
                        #endregion

                        //UpdateTimeTableSec.LocationID = LocationID;                                          //取得地點
                        UpdateTimeTableSec.Disable = ParseBool(""+row.Cells[iDisable].Value);               //是否不排課
                        UpdateTimeTableSec.DisableMessage = "" + row.Cells[iDisableMessage].Value;                  //不排課訊息
                    }
                    else
                    {
                        TimeTableSec InsertTimeTableSec = new TimeTableSec();
                        InsertTimeTableSec.TimeTableID = K12.Data.Int.Parse(mTimeTablePackage.TimeTable.UID);                 //時間表系統編號用來原來的
                        InsertTimeTableSec.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);  //取得星期
                        InsertTimeTableSec.Period = ParsePeriod("" + row.Cells[iPeriod].Value);           //取得節次
                        //InsertTimeTableSec.DispPeriod = ParsePeriod("" + row.Cells[iDispPeriod].Value);   //取得顯示節次

                        #region 將畫面上的開始時間及結束時間，解析為開始時間及持續分鐘
                        string strStartTime = "" + row.Cells[iStartTime].Value;                           //取得開始時間
                        string strEndTime = "" + row.Cells[iEndTime].Value;                               //取得結束時間

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(strStartTime, strEndTime);

                        InsertTimeTableSec.BeginTime = StorageTime.Item1;                                 //將開始小時及開始分鐘轉成DateTime
                        InsertTimeTableSec.Duration = StorageTime.Item2;                                  //取得持續時間                        
                        #endregion

                        //InsertTimeTableSec.LocationID = LocationID;                                       //取得地點
                        InsertTimeTableSec.Disable = ParseBool("" + row.Cells[iDisable].Value);          //是否不排課
                        InsertTimeTableSec.DisableMessage = "" + row.Cells[iDisableMessage].Value;               //不排課訊息

                        mTimeTablePackage.TimeTableSecs.Add(InsertTimeTableSec);                          //加入時間表分段 
                    }
                    #endregion
                }

                grdTimeTableSecs.Rows.Clear();

                return mTimeTablePackage;
            }
            set 
            {
                //將開始時間及結束時間顯示在畫面上
                txtTimeTableDesc.Text = string.Empty;
                grdTimeTableSecs.Rows.Clear();

                if (value != null)
                {
                    Prepare();

                    mTimeTablePackage = value;

                    var SortTimeTableSecs = from vTimeTableSec in mTimeTablePackage.TimeTableSecs orderby vTimeTableSec.WeekDay, vTimeTableSec.Period select vTimeTableSec;

                    mTimeTablePackage.TimeTableSecs = SortTimeTableSecs.ToList();

                    if (mTimeTablePackage.TimeTable != null)
                    {
                        mEditorName = value.TimeTable.TimeTableName;
                        txtTimeTableDesc.Text = mTimeTablePackage.TimeTable.TimeTableDesc;
                    }

                    if (!K12.Data.Utility.Utility.IsNullOrEmpty(mTimeTablePackage.TimeTableSecs))
                    {
                        foreach (TimeTableSec vTimeTableSec in mTimeTablePackage.TimeTableSecs)
                        {
                            int AddRowIndex = grdTimeTableSecs.Rows.Add();
                            DataGridViewRow Row = grdTimeTableSecs.Rows[AddRowIndex];
                            Row.Tag = vTimeTableSec;
                            //Location vLocation = mLocations.Find(x => x.UID.Equals(K12.Data.Int.GetString(vTimeTableSec.LocationID)));
                            //string LocationName = vLocation != null ? vLocation.LocationName : string.Empty;

                            Tuple<string, string> DisplayTime = Utility.GetDisplayTime(vTimeTableSec.BeginTime, vTimeTableSec.Duration);

                            //grdTimeTableSecs.Rows[AddRowIndex].SetValues(vTimeTableSec.WeekDay,
                            //    vTimeTableSec.Period,
                            //    vTimeTableSec.DispPeriod,
                            //    DisplayTime.Item1,
                            //    DisplayTime.Item2,
                            //    LocationName,
                            //    vTimeTableSec.Disable,
                            //    vTimeTableSec.DisableMessage);

                            string strPeriod = vTimeTableSec.Period.Equals(0) ? "中午" : ""+vTimeTableSec.Period;

                            grdTimeTableSecs.Rows[AddRowIndex].SetValues(vTimeTableSec.WeekDay,
                            strPeriod,
                            DisplayTime.Item1,
                            DisplayTime.Item2,
                            vTimeTableSec.Disable,
                            vTimeTableSec.DisableMessage);
                        }
                    }
                }

                IsDirty = false;
            }
        }

        /// <summary>
        /// 將自己傳回
        /// </summary>
        public UserControl Control { get { return this; } }
        #endregion

        #region 資料驗證
        private void ValidateField()
        {
            IsDirty = true;

            DataGridViewCell cell = grdTimeTableSecs.CurrentCell;

            if (cell.ColumnIndex == iWeekDay)
            {
                int CheckWeekDay = K12.Data.Int.Parse("" + cell.Value);

                if (CheckWeekDay <= 0 || CheckWeekDay > 7)
                    cell.ErrorText = sWeekdayError;
                else if (IsTimeTableSecConflict())
                    cell.ErrorText = sTimeTableSecConflict;
                else if (IsWeekDayPeriodDuplicate())
                    cell.ErrorText = sWeekdayPeriodDuplication;
                else
                    cell.ErrorText = string.Empty;
            }

            if (cell.ColumnIndex == iPeriod)
            {
                string strCellValue = "" + cell.Value;

                int? CheckPeriod = K12.Data.Int.ParseAllowNull(strCellValue);

                if (LongBreaks.Contains(strCellValue))
                    CheckPeriod = 0;

                if (CheckPeriod == null)
                    cell.ErrorText = sPeriodError;
                else if (CheckPeriod.Value < 0 || CheckPeriod.Value > 20)
                    cell.ErrorText = sPeriodError;
                else if (IsWeekDayPeriodDuplicate())
                    cell.ErrorText = sWeekdayPeriodDuplication;
                else
                    cell.ErrorText = string.Empty;
            }

            //if (cell.ColumnIndex == iDispPeriod)
            //{
            //    int? CheckDisplayPeriod = K12.Data.Int.ParseAllowNull("" + cell.Value);

            //    if (CheckDisplayPeriod == null)
            //        cell.ErrorText = sDisplayPeriodError;
            //    else
            //        cell.ErrorText = string.Empty;
            //}

            #region 檢查合法時間
            if ((cell.ColumnIndex == iStartTime) || (cell.ColumnIndex == iEndTime))
            {
                string strStartTime = "" + grdTimeTableSecs[iStartTime, cell.RowIndex].Value;
                string strEndTime = "" + grdTimeTableSecs[iEndTime, cell.RowIndex].Value;
                Tuple<bool,string> Result = Utility.IsValidateTime("" + cell.Value);

                if (!Result.Item1)
                    cell.ErrorText = Result.Item2;
                else if (IsTimeTableSecConflict())
                    cell.ErrorText = sTimeTableSecConflict;
                else if (!string.IsNullOrWhiteSpace(strStartTime) && !string.IsNullOrWhiteSpace(strEndTime))
                {
                    Tuple<DateTime, int> StorageTime = Utility.GetStorageTime("" + grdTimeTableSecs[iStartTime, cell.RowIndex].Value, "" + grdTimeTableSecs[iEndTime, cell.RowIndex].Value);

                    if (StorageTime.Item2 <= 0)
                        cell.ErrorText = "結束時間要大於開始時間！";
                    else
                        cell.ErrorText = string.Empty;
                }
                else
                    cell.ErrorText = string.Empty;
            }
            #endregion

            cell.Value = cell.EditedFormattedValue;
            grdTimeTableSecs.EndEdit();
            grdTimeTableSecs.BeginEdit(false);
        }

        /// <summary>
        /// 驗證表單資料是否合法
        /// </summary>
        /// <returns></returns>
        public new bool Validate()
        {
            bool pass = true;
            foreach (DataGridViewRow row in grdTimeTableSecs.Rows)
            {
                if (row.IsNewRow) continue;

                if (!string.IsNullOrEmpty(row.Cells[iWeekDay].ErrorText))
                    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iPeriod].ErrorText))
                    pass &= false;

                //if (!string.IsNullOrEmpty(row.Cells[iDispPeriod].ErrorText))
                //    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iStartTime].ErrorText))
                    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iEndTime].ErrorText))
                    pass &= false;
            }

            return pass;
        }

        private string GetCellValue(string Value)
        {
            if (LongBreaks.Contains(Value))
                return "0";
            else 
                return Value;
        }

        /// <summary>
        /// 檢查資料列中的星期及節次是否重覆
        /// </summary>
        /// <returns></returns>
        private bool IsWeekDayPeriodDuplicate()
        {
            List<string> WeekdayPeriods = new List<string>();

            foreach (DataGridViewRow row in grdTimeTableSecs.Rows)
            {
                if (row.IsNewRow) continue;

                string NewWeekdayPeriod = GetCellValue("" + row.Cells[iWeekDay].Value) + "^_^" + GetCellValue(""+ row.Cells[iPeriod].Value);

                foreach (string WeekdayPeriod in WeekdayPeriods)
                {
                    if (NewWeekdayPeriod.Equals(WeekdayPeriod))
                        return true;
                }

                WeekdayPeriods.Add(NewWeekdayPeriod);
            }

            return false;
        }

        /// <summary>
        /// 檢查時間表所有時段是否有衝突
        /// </summary>
        /// <returns></returns>
        private bool IsTimeTableSecConflict()
        {
            List<Period> Periods = new List<Period>();

            foreach (DataGridViewRow row in grdTimeTableSecs.Rows)
            {
                if (row.IsNewRow) continue;

                Period NewPeriod = new Period();

                Tuple<DateTime, int> StorageTime = Utility.GetStorageTime("" + row.Cells[iStartTime].Value, "" + row.Cells[iEndTime].Value);

                NewPeriod.Weekday = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);
                NewPeriod.Hour = StorageTime.Item1.Hour;
                NewPeriod.Minute = StorageTime.Item1.Minute;
                NewPeriod.Duration = StorageTime.Item2;

                if (Periods.Count > 0)
                    foreach (Period Period in Periods)
                        if (Period.IsTimeIntersectsWith(NewPeriod))
                            return true;

                Periods.Add(NewPeriod);
            }
            return false;
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
            if (grdTimeTableSecs.SelectedCells.Count == 1)
                grdTimeTableSecs.BeginEdit(true);
        }

        /// <summary>
        /// 當欄位狀態改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {          
            ValidateField();
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
                foreach (DataGridViewRow var in grdTimeTableSecs.SelectedRows)
                {
                    if (var.Index != mSelectedRowIndex)
                        var.Selected = false;
                }

                //選取目前滑鼠所在的列
                grdTimeTableSecs.Rows[mSelectedRowIndex].Selected = true;

                //顯示滑鼠右鍵選單
                contextMenuStripDelete.Show(grdTimeTableSecs, grdTimeTableSecs.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location);
            }
        }

        /// <summary>
        /// 使用者刪除資料時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTimeTableSecs_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            TimeTableSec TimeTableSec = e.Row.Tag as TimeTableSec;

            if (TimeTableSec != null)
                TimeTableSec.Deleted = true;
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
            if (mSelectedRowIndex >= 0 && grdTimeTableSecs.Rows.Count - 1 > mSelectedRowIndex)
            {
                TimeTableSec TimeTableSec = grdTimeTableSecs.Rows[mSelectedRowIndex].Tag as TimeTableSec;

                if (TimeTableSec != null)
                    TimeTableSec.Deleted = true;

                grdTimeTableSecs.Rows.RemoveAt(mSelectedRowIndex);
            }
        }

        public void SetTitle(string name)
        {
            lblName.Text = name;
        }
    }
}