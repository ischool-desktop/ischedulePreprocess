using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Campus.Windows;
using FISCA.Permission;
using FISCA.UDT;
using FCode = FISCA.Permission.FeatureCodeAttribute;

namespace Sunset
{
    /// <summary>
    /// 教師不排課時段
    /// </summary>
    [FCode("Sunset.Detail0050", "班級不排課時段")]
    public class ClassBusyEditor : FISCA.Presentation.DetailContent, IContentEditor<List<ClassBusy>>
    {
        #region 常數宣告
        private const string sWeekdayError = "星期必須介於1到7之間";
        private const string sPeriodError = "節次必須介於0到20之間，午休時段請輸入0。";
        private const string sDisplayPeriodError = "顯示節次必須為數字";
        private const string sBeginHourError = "開始小時必須介於0到23之間";
        private const string sBeginMinuteError = "開始小時必須介於0到59之間";
        private const string sDuration = "持續分鐘必須介於1到1140之間";
        private const string sPeriodConflict = "不排課時段不允許時間（星期、開始時間、結束時間）有重疊";
        private const string sWeekdayPeriodDuplication = "星期節次不允許重覆";
        #endregion

        #region 參數宣告
        private FeatureAce UserPermission;
        private BackgroundWorker mbkwClassBusy = new BackgroundWorker();
        private AccessHelper mAccessHelper = new AccessHelper();
        private bool IsBusy = false;
        private List<ClassBusy> mClassBusys = new List<ClassBusy>();
        private ChangeListener DataListener;
        private int mSelectedRowIndex;
        #endregion

        #region DataGridView索引
        private const int iWeekDay = 0;
        private const int iStartTime = 1;
        private const int iEndTime = 2;
        private const int iBusyDesc = 3;
        #endregion

        private ContextMenuStrip contextMenuStripDelete;
        private IContainer components;
        private ToolStripMenuItem toolStripMenuItem2;
        private DataGridViewTextBoxColumn colWeekDay;
        private DataGridViewTextBoxColumn colStartHour;
        private DataGridViewTextBoxColumn colStartMinute;
        private DataGridViewTextBoxColumn colBusyDesc;

        #region DataGridView欄位
        private DevComponents.DotNetBar.Controls.DataGridViewX grdClassBusys;
        #endregion

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassBusyEditor()
        {
            InitializeComponent();

            Group = "班級不排課時段";
        }

        #region Private Functions
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
                        if (NewPeriod.IsTimeIntersectsWith(Period))
                            return true;

                Periods.Add(NewPeriod);
            }
            return false;
        }

        #endregion

        #region BackgroundWorker
        /// <summary>
        /// 背景執行，根據教師系統編號取得教師不排課時段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkwClassBusy_DoWork(object sender, DoWorkEventArgs e)
        {
            if (string.IsNullOrEmpty(this.PrimaryKey)) return;

            #region 取得班級不排課時段
            //將資料清空
            mClassBusys.Clear();

            //依班級系統編號取得班級不排課時段
            mClassBusys = mAccessHelper.Select<ClassBusy>(string.Format("ref_class_id={0}", this.PrimaryKey));

            //將班級不排課時段排序
            var SortedClassBusys = (from vClassBusy in mClassBusys orderby vClassBusy.WeekDay, vClassBusy.BeginTime select vClassBusy).ToList();

            //將班級不排課時段轉成List
            mClassBusys = SortedClassBusys.ToList();
            #endregion
        }

        /// <summary>
        /// 背景執行，執行完成時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkwClassBusy_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //若為忙碌則重新執行
            if (IsBusy)
            {
                IsBusy = false;
                mbkwClassBusy.RunWorkerAsync();
                return;
            }

            Prepare();

            //設定教師不排課時段內容
            Content = mClassBusys;

            this.Loading = false;
        }
        #endregion

        #region DataGridView相關程式碼
        /// <summary>
        /// 當DataGridView狀態變更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataListener_StatusChanged(object sender, ChangeEventArgs e)
        {
            SaveButtonVisible = (e.Status == ValueStatus.Dirty);
            CancelButtonVisible = (e.Status == ValueStatus.Dirty);
        }

        /// <summary>
        ///  當進入到某個Cell引發的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClassBusys_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //若選取的Cell數量等於1則開始編輯
            if (grdClassBusys.SelectedCells.Count == 1)
                grdClassBusys.BeginEdit(true);
        }

        /// <summary>
        /// 開始欄位編輯事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClassBusys_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        /// <summary>
        /// 結束欄位編輯事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClassBusys_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 當欄位狀態改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClassBusys_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridViewCell cell = grdClassBusys.CurrentCell;

            cell.ErrorText = string.Empty;

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

                string strStartTime = "" + grdClassBusys[iStartTime, cell.RowIndex].Value;
                string strEndTime = "" + grdClassBusys[iEndTime, cell.RowIndex].Value;

                if (!Result.Item1)
                    cell.ErrorText = Result.Item2;
                else if (IsPeriodConflict())
                    cell.ErrorText = sPeriodConflict;
                else if (!string.IsNullOrWhiteSpace(strStartTime) && !string.IsNullOrWhiteSpace(strEndTime)) //判斷兩者都不為空白才做檢查
                {
                    Tuple<DateTime, int> StorageTime = Utility.GetStorageTime("" + grdClassBusys[iStartTime, cell.RowIndex].Value, "" + grdClassBusys[iEndTime, cell.RowIndex].Value);

                    if (StorageTime.Item2 <= 0)
                        cell.ErrorText = "結束時間要大於開始時間！";
                    else
                        cell.ErrorText = string.Empty;
                }
                else
                    cell.ErrorText = string.Empty;
            }

            cell.Value = cell.EditedFormattedValue;
            grdClassBusys.EndEdit();
            grdClassBusys.BeginEdit(false);
        }

        /// <summary>
        /// 滑鼠右鍵用來刪除現有記錄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClassBusys_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //判斷選取的資料行索引大於0，欄位索引小於0，並且按下滑鼠右鍵
            if (e.RowIndex >= 0 && e.ColumnIndex < 0 && e.Button == MouseButtons.Right)
            {
                //將目前選取的資料行索引記錄下來
                mSelectedRowIndex = e.RowIndex;

                //將目前選取的資料列，除了滑鼠右鍵所在的列外都設為不選取
                foreach (DataGridViewRow var in grdClassBusys.SelectedRows)
                {
                    if (var.Index != mSelectedRowIndex)
                        var.Selected = false;
                }

                //選取目前滑鼠所在的列
                grdClassBusys.Rows[mSelectedRowIndex].Selected = true;

                //顯示滑鼠右鍵選單
                contextMenuStripDelete.Show(grdClassBusys, grdClassBusys.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location);
            }
        }

        /// <summary>
        /// 使用者刪除資料時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClassBusys_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            MessageBox.Show("UserDeletingRow");

            ClassBusy ClassBusy = e.Row.Tag as ClassBusy;

            if (ClassBusy != null)
                ClassBusy.Deleted = true;
        }

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
                ClassBusy ClassBusy = grdClassBusys.Rows[mSelectedRowIndex].Tag as ClassBusy;

                if (ClassBusy != null)
                    ClassBusy.Deleted = true;

                grdClassBusys.Rows.RemoveAt(mSelectedRowIndex);
            }
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

                if (mbkwClassBusy.IsBusy) //如果是忙碌的
                    IsBusy = true; //為True
                else
                    mbkwClassBusy.RunWorkerAsync(); //否則直接執行
            }
        }

        /// <summary>
        /// 當按下儲存按鈕時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSaveButtonClick(EventArgs e)
        {
            if (!Validate())
            {
                MsgBox.Show("輸入資料有誤，無法儲存。\n請檢查輸入資料。");
                return;
            }

            StringBuilder strBuilder = new StringBuilder();

            List<ClassBusy> SaveClassBusys = Content;

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SaveClassBusys))
            {
                mAccessHelper.SaveAll(SaveClassBusys);
                strBuilder.AppendLine("已儲存班級不排課時段共" + mClassBusys.Count + "筆");
            }

            SaveButtonVisible = false;
            CancelButtonVisible = false;

            this.Loading = true;

            DataListener.SuspendListen();     //終止變更判斷
            mbkwClassBusy.RunWorkerAsync(); //背景作業,取得並重新填入原資料

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

            if (!mbkwClassBusy.IsBusy)
                mbkwClassBusy.RunWorkerAsync();
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

        /// <summary>
        /// 初始化元件
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdClassBusys = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.contextMenuStripDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.colWeekDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartHour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartMinute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBusyDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdClassBusys)).BeginInit();
            this.contextMenuStripDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdClassBusys
            // 
            this.grdClassBusys.BackgroundColor = System.Drawing.Color.White;
            this.grdClassBusys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClassBusys.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWeekDay,
            this.colStartHour,
            this.colStartMinute,
            this.colBusyDesc});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdClassBusys.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdClassBusys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdClassBusys.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdClassBusys.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdClassBusys.HighlightSelectedColumnHeaders = false;
            this.grdClassBusys.Location = new System.Drawing.Point(10, 5);
            this.grdClassBusys.MultiSelect = false;
            this.grdClassBusys.Name = "grdClassBusys";
            this.grdClassBusys.RowHeadersWidth = 35;
            this.grdClassBusys.RowTemplate.Height = 24;
            this.grdClassBusys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdClassBusys.Size = new System.Drawing.Size(530, 140);
            this.grdClassBusys.TabIndex = 3;
            this.grdClassBusys.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdClassBusys_CellBeginEdit);
            this.grdClassBusys.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdClassBusys_CellEndEdit);
            this.grdClassBusys.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdClassBusys_CellEnter);
            this.grdClassBusys.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdClassBusys_CellMouseClick);
            this.grdClassBusys.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdClassBusys_CurrentCellDirtyStateChanged);
            this.grdClassBusys.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdClassBusys_UserDeletingRow);
            // 
            // contextMenuStripDelete
            // 
            this.contextMenuStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.contextMenuStripDelete.Name = "contextMenuStrip1";
            this.contextMenuStripDelete.ShowImageMargin = false;
            this.contextMenuStripDelete.Size = new System.Drawing.Size(70, 26);
            this.contextMenuStripDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(69, 22);
            this.toolStripMenuItem2.Text = "刪除";
            // 
            // colWeekDay
            // 
            this.colWeekDay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colWeekDay.HeaderText = "星期";
            this.colWeekDay.Name = "colWeekDay";
            this.colWeekDay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colWeekDay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWeekDay.ToolTipText = "只能輸入1到7";
            this.colWeekDay.Width = 40;
            // 
            // colStartHour
            // 
            this.colStartHour.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colStartHour.HeaderText = "開始時間";
            this.colStartHour.Name = "colStartHour";
            this.colStartHour.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStartHour.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStartHour.Width = 66;
            // 
            // colStartMinute
            // 
            this.colStartMinute.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colStartMinute.HeaderText = "結束時間";
            this.colStartMinute.Name = "colStartMinute";
            this.colStartMinute.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStartMinute.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStartMinute.Width = 66;
            // 
            // colBusyDesc
            // 
            this.colBusyDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBusyDesc.HeaderText = "不排課描述";
            this.colBusyDesc.Name = "colBusyDesc";
            // 
            // ClassBusyEditor
            // 
            this.Controls.Add(this.grdClassBusys);
            this.Name = "ClassBusyEditor";
            this.Load += new System.EventHandler(this.ClassBusyEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdClassBusys)).EndInit();
            this.contextMenuStripDelete.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region IContentEditor<List<TeacherBusy>> 成員
        /// <summary>
        /// 準備動作
        /// </summary>
        public void Prepare()
        {

        }


        /// <summary>
        /// 取得或設定內容
        /// </summary>
        public List<ClassBusy> Content
        {
            get
            {
                #region 從UI上取得班級不排課時段
                foreach (DataGridViewRow row in grdClassBusys.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Tag != null)
                    {
                        ClassBusy UpdateClassBusy = row.Tag as ClassBusy;

                        UpdateClassBusy.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);   //取得星期

                        #region 將畫面上的開始時間及結束時間，解析為開始時間及持續分鐘
                        string strStartTime = "" + row.Cells[iStartTime].Value;                           //取得開始時間
                        string strEndTime = "" + row.Cells[iEndTime].Value;                               //取得結束時間

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(strStartTime, strEndTime);

                        UpdateClassBusy.BeginTime = StorageTime.Item1;
                        UpdateClassBusy.Duration = StorageTime.Item2;                                   //取得持續時間
                        #endregion

                        UpdateClassBusy.BusyDesc = "" + row.Cells[iBusyDesc].Value;                     //取得不排課描述                        

                        mClassBusys.Add(UpdateClassBusy);
                    }
                    else
                    {
                        ClassBusy InsertClassBusy = new ClassBusy();
                        InsertClassBusy.ClassID = K12.Data.Int.Parse(this.PrimaryKey);                //取得教師系統編號
                        InsertClassBusy.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);   //取得星期

                        #region 將畫面上的開始時間及結束時間，解析為開始時間及持續分鐘
                        string strStartTime = "" + row.Cells[iStartTime].Value;                           //取得開始時間
                        string strEndTime = "" + row.Cells[iEndTime].Value;                               //取得結束時間

                        Tuple<DateTime, int> StorageTime = Utility.GetStorageTime(strStartTime, strEndTime);

                        InsertClassBusy.BeginTime = StorageTime.Item1;
                        InsertClassBusy.Duration = StorageTime.Item2;                                   //取得持續時間
                        #endregion

                        InsertClassBusy.BusyDesc = "" + row.Cells[iBusyDesc].Value;                     //取得忙碌描述

                        mClassBusys.Add(InsertClassBusy);
                    }
                }
                #endregion

                return mClassBusys;
            }
            set
            {
                grdClassBusys.Rows.Clear();

                if (!K12.Data.Utility.Utility.IsNullOrEmpty(mClassBusys))
                {
                    foreach (ClassBusy vClassBusy in mClassBusys)
                    {
                        int AddRowIndex = grdClassBusys.Rows.Add();
                        DataGridViewRow Row = grdClassBusys.Rows[AddRowIndex];
                        Row.Tag = vClassBusy;

                        Tuple<string, string> DisplayTime = Utility.GetDisplayTime(vClassBusy.BeginTime, vClassBusy.Duration);

                        grdClassBusys.Rows[AddRowIndex].SetValues(
                            vClassBusy.WeekDay,
                            DisplayTime.Item1,
                            DisplayTime.Item2,
                            vClassBusy.BusyDesc);
                    }
                }

                SaveButtonVisible = false;
                CancelButtonVisible = false;

                DataListener.Reset();
                DataListener.ResumeListen();
            }
        }

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

            return pass;
        }

        #endregion

        #region IContentViewer<List<TeacherBusy>> 成員

        /// <summary>
        /// 取得元件，傳回自己
        /// </summary>
        public UserControl Control
        {
            get { return this; }
        }

        #endregion

        private void ClassBusyEditor_Load(object sender, EventArgs e)
        {
            grdClassBusys.DataError += (vsender, ve) => { };

            //設定權限
            UserPermission = FISCA.Permission.UserAcl.Current[FCode.GetCode(GetType())];
            this.Enabled = UserPermission.Editable;

            //取得教師不排課時段
            mbkwClassBusy.DoWork += new DoWorkEventHandler(bkwClassBusy_DoWork);

            //完成取得教師不排課時段
            mbkwClassBusy.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkwClassBusy_RunWorkerCompleted);

            //判斷DataGridView狀態變更
            DataListener = new ChangeListener();
            DataListener.Add(new DataGridViewSource(grdClassBusys));
            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(DataListener_StatusChanged);

            //終止變更判斷
            DataListener.SuspendListen();
        }
    }
}