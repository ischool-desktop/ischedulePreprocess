using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Campus.Windows;
using FISCA.Data;
using FISCA.Permission;
using FISCA.Presentation;
using FISCA.UDT;
using FCode = FISCA.Permission.FeatureCodeAttribute;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 課程分段資料項目
    /// </summary>
    [FCode("713b6da0-6f44-4544-9a9e-a976ee771270", "課程分段")]
    public class CourseSectionEditor : FISCA.Presentation.DetailContent, IContentEditor<List<SchedulerCourseSection>>
    {
        #region 常數宣告
        private const string sWeekdayError = "星期必須介於0到7之間，0代表未排課。";
        private const string sPeriodError = "節次必須介於0到20之間，0代表午休或未排課（星期為）。";
        private const string sLengthError = "節數必須介於1到20之間。";
        private const string sTeacherRepeatError = "授課教師重覆。";
        private const string sTeacherNoExist = "授課教師不存在於排課教師清單。";
        #endregion

        #region DataGridView欄位索引
        private const int iWeekDay = 0;
        private const int iPeriod = 1;
        private const int iLength = 2;
        private const int iLongBreak = 3;
        private const int iWeekFlag = 4;
        private const int iWeekdayCond = 5;
        private const int iPeriodCond = 6;
        private const int iClassroom = 7;
        private const int iTeacher1 = 8;
        private const int iTeacher2 = 9;
        private const int iTeacher3 = 10;
        private const int iComment = 11;
        #endregion

        #region 參數宣告
        private FeatureAce UserPermission;
        private BackgroundWorker mbkwCourseSection = new BackgroundWorker();
        private AccessHelper mAccessHelper = new AccessHelper();
        private bool IsBusy = false;
        private List<SchedulerCourseSection> mCourseSections = new List<SchedulerCourseSection>();
        private List<Classroom> mClassrooms = new List<Classroom>();
        private DataTable mtblTeacher;
        private ChangeListener DataListener;
        private int mSelectedRowIndex;

        /// <summary>
        /// ischool教師
        /// </summary>
        private Dictionary<string, string> ischool_Teachers = new Dictionary<string, string>();

        /// <summary>
        /// 排課教師
        /// </summary>
        private Dictionary<string, string> Sunset_Teachers = new Dictionary<string, string>();

        private Dictionary<string, string> mClassroomName; //場地ID : 場地名稱
        #endregion

        #region Componetns
        private DevComponents.DotNetBar.Controls.DataGridViewX grdCourseSections;
        private ContextMenuStrip contextMenuStripDelete;
        private ToolStripMenuItem toolStripMenuItem2;
        private DataGridViewTextBoxColumn colWeekDay;
        private DataGridViewTextBoxColumn colPeriod;
        private DataGridViewTextBoxColumn colLength;
        private DataGridViewCheckBoxColumn colLongBreak;
        private DataGridViewComboBoxColumn colWeekFlag;
        private DataGridViewTextBoxColumn colWeekdayPeriod;
        private DataGridViewTextBoxColumn colPeriodCond;
        private DataGridViewComboBoxColumn colClassroom;
        private DataGridViewTextBoxColumn Comment;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewComboBoxExColumn colTeacher1;
        private DataGridViewComboBoxExColumn colTeacher2;
        private DataGridViewComboBoxExColumn colTeacher3;
        private DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn1;
        private DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn2;
        private DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn3;
        private IContainer components;
        #endregion

        Log_CourseSection log { get; set; }
        List<SchedulerCourseExtension> s_CourseList { get; set; }
        SchedulerCourseExtension Course { get; set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public CourseSectionEditor()
        {
            InitializeComponent();

            //設定資料項目名稱
            Group = "課程分段";
        }

        #region BackgroundWorker
        /// <summary>
        /// 背景執行，根據教師系統編號取得教師不排課時段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkwCourseSection_DoWork(object sender, DoWorkEventArgs e)
        {
            if (string.IsNullOrEmpty(this.PrimaryKey)) return;

            #region 取得課程分段
            //將資料清空
            mCourseSections.Clear();

            string strCourseID = "" + this.PrimaryKey;

            //依課程系統編號取得課程分段
            mCourseSections = tool._A.Select<SchedulerCourseSection>(string.Format("ref_course_id={0}", strCourseID));

            //取得原課程
            s_CourseList = tool._A.Select<SchedulerCourseExtension>(string.Format("uid={0}", strCourseID));
            Course = s_CourseList[0];

            //將課程分段依星期及節次排序
            var SortedCourseSections = (from vCourseSection in mCourseSections orderby vCourseSection.WeekDay, vCourseSection.Period select vCourseSection);

            //將教師不排課時段轉成List
            mCourseSections = SortedCourseSections.ToList();
            #endregion

            #region 取得所有場地並依場地名稱排序
            mClassrooms.Clear();

            mClassrooms = mAccessHelper.Select<Classroom>();

            var SortedClassrooms = mClassrooms.OrderBy(x => x.ClassroomName);

            mClassrooms = SortedClassrooms.ToList();

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
        }

        /// <summary>
        /// 背景執行，執行完成時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkwCourseSection_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //若為忙碌則重新執行
            if (IsBusy)
            {
                IsBusy = false;
                mbkwCourseSection.RunWorkerAsync();
                return;
            }

            Prepare();

            //開始Log
            log = new Log_CourseSection(mCourseSections, mClassroomName);

            //設定課程分段
            Content = mCourseSections;

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
        private void grdCourseSections_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //若選取的Cell數量等於1則開始編輯
            if (grdCourseSections.SelectedCells.Count == 1)
                grdCourseSections.BeginEdit(true);
        }

        /// <summary>
        /// 開始欄位編輯事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCourseSections_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        /// <summary>
        /// 結束欄位編輯事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCourseSections_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 當欄位狀態改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCourseSections_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridViewCell cell = grdCourseSections.CurrentCell;

            cell.ErrorText = string.Empty;

            if (cell.ColumnIndex == iTeacher1 ||
                cell.ColumnIndex == iTeacher2 ||
                cell.ColumnIndex == iTeacher3)
            {
                string TeacherName1 = "" + grdCourseSections.Rows[cell.RowIndex].Cells[iTeacher1].Value;
                string TeacherName2 = "" + grdCourseSections.Rows[cell.RowIndex].Cells[iTeacher2].Value;
                string TeacherName3 = "" + grdCourseSections.Rows[cell.RowIndex].Cells[iTeacher3].Value;

                if (cell.ColumnIndex == iTeacher1)
                {
                    cell.ErrorText = string.Empty;

                    if (!string.IsNullOrWhiteSpace(TeacherName1))
                    {
                        //判斷是否名稱重覆
                        if (TeacherName1.Equals(TeacherName2) || TeacherName1.Equals(TeacherName3))
                            cell.ErrorText = sTeacherRepeatError;
                        //判斷是否存在排課教師內
                        if (!Sunset_Teachers.ContainsKey(TeacherName1))
                            cell.ErrorText += sTeacherNoExist;
                    }
                }

                if (cell.ColumnIndex == iTeacher2)
                {
                    cell.ErrorText = string.Empty;

                    if (!string.IsNullOrWhiteSpace(TeacherName2))
                    {
                        if (TeacherName2.Equals(TeacherName1) || TeacherName2.Equals(TeacherName3))
                            cell.ErrorText = sTeacherRepeatError;
                        if (!Sunset_Teachers.ContainsKey(TeacherName2))
                            cell.ErrorText += sTeacherNoExist;
                    }
                }

                if (cell.ColumnIndex == iTeacher3)
                {
                    cell.ErrorText = string.Empty;

                    if (!string.IsNullOrWhiteSpace(TeacherName3))
                    {
                        if (TeacherName3.Equals(TeacherName1) || TeacherName3.Equals(TeacherName2))
                            cell.ErrorText = sTeacherRepeatError;
                        if (!Sunset_Teachers.ContainsKey(TeacherName3))
                            cell.ErrorText += sTeacherNoExist;
                    }
                }
            }

            if (cell.ColumnIndex == iWeekDay)
            {
                int CheckWeekDay;

                if (int.TryParse("" + cell.Value, out CheckWeekDay))
                {
                    if (CheckWeekDay < 0 || CheckWeekDay > 7)
                        cell.ErrorText = sWeekdayError;
                    else
                        cell.ErrorText = string.Empty;

                }
                else
                    cell.ErrorText = sWeekdayError;
            }

            if (cell.ColumnIndex == iPeriod)
            {
                int CheckPeriod;

                if (int.TryParse("" + cell.Value, out CheckPeriod))
                {
                    if (CheckPeriod < 0 || CheckPeriod > 20)
                        cell.ErrorText = sPeriodError;
                    else
                        cell.ErrorText = string.Empty;

                }
                else
                    cell.ErrorText = sPeriodError;
            }

            if (cell.ColumnIndex == iLength)
            {
                int CheckLength = K12.Data.Int.Parse("" + cell.Value);

                if (CheckLength < 1 || CheckLength > 20)
                    cell.ErrorText = sLengthError;
                else
                    cell.ErrorText = string.Empty;
            }

            if (!(cell.ColumnIndex == iTeacher1 ||
                cell.ColumnIndex == iTeacher2 ||
                cell.ColumnIndex == iTeacher3))
            {

                cell.Value = cell.EditedFormattedValue;
                grdCourseSections.EndEdit();
                grdCourseSections.BeginEdit(false);
            }
        }

        /// <summary>
        /// 滑鼠右鍵用來刪除現有記錄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCourseSections_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //判斷選取的資料行索引大於0，欄位索引小於0，並且按下滑鼠右鍵
            if (e.RowIndex >= 0 && e.ColumnIndex < 0 && e.Button == MouseButtons.Right)
            {
                //將目前選取的資料行索引記錄下來
                mSelectedRowIndex = e.RowIndex;

                //將目前選取的資料列，除了滑鼠右鍵所在的列外都設為不選取
                foreach (DataGridViewRow var in grdCourseSections.SelectedRows)
                {
                    if (var.Index != mSelectedRowIndex)
                        var.Selected = false;
                }

                //選取目前滑鼠所在的列
                grdCourseSections.Rows[mSelectedRowIndex].Selected = true;

                //顯示滑鼠右鍵選單
                contextMenuStripDelete.Show(grdCourseSections, grdCourseSections.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location);
            }
            else if (e.RowIndex > -1 && e.ColumnIndex == -1 && e.Button == MouseButtons.Left)
            {
                grdCourseSections.Focus();
            }

        }

        /// <summary>
        /// 使用者刪除資料時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCourseSections_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Index != -1 && !e.Row.IsNewRow)
            {
                DialogResult dr = MsgBox.Show("確認要刪除資料?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.Yes)
                {
                    SchedulerCourseSection CourseSection = e.Row.Tag as SchedulerCourseSection;

                    if (CourseSection != null)
                        CourseSection.Deleted = true;
                }
                else
                {
                    e.Cancel = true; //取消刪除作業
                }
            }
        }

        /// <summary>
        /// 當按下刪除選單時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            //實際進行刪除列的動作
            if (mSelectedRowIndex >= 0 && grdCourseSections.Rows.Count - 1 > mSelectedRowIndex)
            {
                SchedulerCourseSection CourseSection = grdCourseSections.Rows[mSelectedRowIndex].Tag as SchedulerCourseSection;

                if (CourseSection != null)
                    CourseSection.Deleted = true;

                grdCourseSections.Rows.RemoveAt(mSelectedRowIndex);
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

                if (mbkwCourseSection.IsBusy) //如果是忙碌的
                    IsBusy = true; //為True
                else
                    mbkwCourseSection.RunWorkerAsync(); //否則直接執行
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

            List<SchedulerCourseSection> SaveCourseSections = Content;

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SaveCourseSections))
            {
                //儲存Log
                //log.GetLog(SaveCourseSections, Course);

                FISCA.LogAgent.ApplicationLog.Log("排課", "修改", log.GetLog(SaveCourseSections, Course));

                mAccessHelper.SaveAll(SaveCourseSections);
                strBuilder.AppendLine("已儲存課程分段共" + mCourseSections.Count + "筆");
            }

            SaveButtonVisible = false;
            CancelButtonVisible = false;

            this.Loading = true;

            DataListener.SuspendListen();       //終止變更判斷
            mbkwCourseSection.RunWorkerAsync(); //背景作業,取得並重新填入原資料

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

            if (!mbkwCourseSection.IsBusy)
                mbkwCourseSection.RunWorkerAsync();
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

        #region IContentEditor<List<CourseSection>> 成員
        /// <summary>
        /// 準備動作
        /// </summary>
        public void Prepare()
        {
            DataGridViewComboBoxColumn Column = grdCourseSections.Columns[iClassroom] as DataGridViewComboBoxColumn;

            if (Column != null)
            {
                Column.Items.Clear();
                Column.Items.Add(string.Empty);
                mClassrooms.ForEach(x => Column.Items.Add(x.ClassroomName));
            }

            DataGridViewComboBoxExColumn colTeacher1 = grdCourseSections.Columns[iTeacher1] as DataGridViewComboBoxExColumn;
            DataGridViewComboBoxExColumn colTeacher2 = grdCourseSections.Columns[iTeacher2] as DataGridViewComboBoxExColumn;
            DataGridViewComboBoxExColumn colTeacher3 = grdCourseSections.Columns[iTeacher3] as DataGridViewComboBoxExColumn;

            colTeacher1.Items.Clear();
            colTeacher2.Items.Clear();
            colTeacher3.Items.Clear();

            colTeacher1.Items.AddRange(Sunset_Teachers.Keys);
            colTeacher2.Items.AddRange(Sunset_Teachers.Keys);
            colTeacher3.Items.AddRange(Sunset_Teachers.Keys);
        }

        /// <summary>
        /// 驗證表單資料是否合法
        /// </summary>
        /// <returns></returns>
        public new bool Validate()
        {
            bool pass = true;
            foreach (DataGridViewRow row in grdCourseSections.Rows)
            {
                if (row.IsNewRow) continue;

                if (!string.IsNullOrEmpty(row.Cells[iWeekDay].ErrorText))
                    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iPeriod].ErrorText))
                    pass &= false;

                if (!string.IsNullOrEmpty(row.Cells[iLength].ErrorText))
                    pass &= false;
            }

            return pass;
        }

        /// <summary>
        /// 取得或設定內容
        /// </summary>
        public List<SchedulerCourseSection> Content
        {
            get
            {
                #region 從UI上取得課程分段
                foreach (DataGridViewRow row in grdCourseSections.Rows)
                {
                    if (row.IsNewRow) continue;

                    string ClassroomName = "" + row.Cells[iClassroom].Value;
                    Classroom Classroom = mClassrooms.Find(x => x.ClassroomName.Equals(ClassroomName));
                    int? ClassroomID = null;
                    if (Classroom != null)
                        ClassroomID = K12.Data.Int.ParseAllowNull(Classroom.UID);

                    string TeacherName1 = "" + row.Cells[iTeacher1].Value;
                    string TeacherName2 = "" + row.Cells[iTeacher2].Value;
                    string TeacherName3 = "" + row.Cells[iTeacher3].Value;

                    if (row.Tag != null)
                    {
                        SchedulerCourseSection UpdateCourseSection = row.Tag as SchedulerCourseSection;
                        UpdateCourseSection.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);
                        UpdateCourseSection.Period = K12.Data.Int.Parse("" + row.Cells[iPeriod].Value);
                        UpdateCourseSection.Length = K12.Data.Int.Parse("" + row.Cells[iLength].Value);
                        UpdateCourseSection.LongBreak = bool.Parse("" + row.Cells[iLongBreak].Value);
                        UpdateCourseSection.WeekFlag = Utility.GetWeekFlagInt("" + row.Cells[iWeekFlag].Value);
                        UpdateCourseSection.WeekDayCond = "" + row.Cells[iWeekdayCond].Value;
                        UpdateCourseSection.PeriodCond = "" + row.Cells[iPeriodCond].Value;
                        UpdateCourseSection.ClassroomID = ClassroomID;

                        UpdateCourseSection.TeacherName1 = "" + row.Cells[iTeacher1].Value;
                        UpdateCourseSection.TeacherName2 = "" + row.Cells[iTeacher2].Value;
                        UpdateCourseSection.TeacherName3 = "" + row.Cells[iTeacher3].Value;

                        if (!string.IsNullOrWhiteSpace(TeacherName1) && ischool_Teachers.ContainsKey(TeacherName1))
                            UpdateCourseSection.TeacherID1 = K12.Data.Int.Parse(ischool_Teachers[TeacherName1]);
                        else
                            UpdateCourseSection.TeacherID1 = null;

                        if (!string.IsNullOrWhiteSpace(TeacherName2) && ischool_Teachers.ContainsKey(TeacherName2))
                            UpdateCourseSection.TeacherID2 = K12.Data.Int.Parse(ischool_Teachers[TeacherName2]);
                        else
                            UpdateCourseSection.TeacherID2 = null;

                        if (!string.IsNullOrWhiteSpace(TeacherName3) && ischool_Teachers.ContainsKey(TeacherName3))
                            UpdateCourseSection.TeacherID3 = K12.Data.Int.Parse(ischool_Teachers[TeacherName3]);
                        else
                            UpdateCourseSection.TeacherID3 = null;

                        UpdateCourseSection.Comment = "" + row.Cells[iComment].Value;
                    }
                    else
                    {
                        SchedulerCourseSection InsertCourseSection = new SchedulerCourseSection();
                        InsertCourseSection.CourseID = K12.Data.Int.Parse(this.PrimaryKey);
                        InsertCourseSection.WeekDay = K12.Data.Int.Parse("" + row.Cells[iWeekDay].Value);
                        InsertCourseSection.Period = K12.Data.Int.Parse("" + row.Cells[iPeriod].Value);
                        InsertCourseSection.Length = K12.Data.Int.Parse("" + row.Cells[iLength].Value);
                        #region 解析跨中午，若無法解析預設為false
                        bool TryLongBrak;
                        string strLongBreak = "" + row.Cells[iLongBreak].Value;
                        if (bool.TryParse(strLongBreak, out TryLongBrak))
                            InsertCourseSection.LongBreak = TryLongBrak;
                        else
                            InsertCourseSection.LongBreak = false;
                        #endregion
                        InsertCourseSection.WeekFlag = Utility.GetWeekFlagInt("" + row.Cells[iWeekFlag].Value);
                        InsertCourseSection.WeekDayCond = "" + row.Cells[iWeekdayCond].Value;
                        InsertCourseSection.PeriodCond = "" + row.Cells[iPeriodCond].Value;
                        InsertCourseSection.ClassroomID = ClassroomID;
                        InsertCourseSection.TeacherName1 = "" + row.Cells[iTeacher1].Value;
                        InsertCourseSection.TeacherName2 = "" + row.Cells[iTeacher2].Value;
                        InsertCourseSection.TeacherName3 = "" + row.Cells[iTeacher3].Value;

                        if (!string.IsNullOrWhiteSpace(TeacherName1) && ischool_Teachers.ContainsKey(TeacherName1))
                            InsertCourseSection.TeacherID1 = K12.Data.Int.Parse(ischool_Teachers[TeacherName1]);
                        else
                            InsertCourseSection.TeacherID1 = null;

                        if (!string.IsNullOrWhiteSpace(TeacherName2) && ischool_Teachers.ContainsKey(TeacherName2))
                            InsertCourseSection.TeacherID2 = K12.Data.Int.Parse(ischool_Teachers[TeacherName2]);
                        else
                            InsertCourseSection.TeacherID2 = null;

                        if (!string.IsNullOrWhiteSpace(TeacherName3) && ischool_Teachers.ContainsKey(TeacherName3))
                            InsertCourseSection.TeacherID3 = K12.Data.Int.Parse(ischool_Teachers[TeacherName3]);
                        else
                            InsertCourseSection.TeacherID3 = null;

                        InsertCourseSection.Comment = "" + row.Cells[iComment].Value;

                        mCourseSections.Add(InsertCourseSection);
                    }
                }
                #endregion

                return mCourseSections;
            }
            set
            {
                grdCourseSections.Rows.Clear();

                if (!K12.Data.Utility.Utility.IsNullOrEmpty(mCourseSections))
                {
                    foreach (SchedulerCourseSection vCourseSection in mCourseSections)
                    {
                        Classroom Classroom = mClassrooms.Find(x => x.UID.Equals(K12.Data.Int.GetString(vCourseSection.ClassroomID)));
                        string ClassroomName = Classroom != null ? Classroom.ClassroomName : string.Empty;

                        #region 改寫

                        DataGridViewRow Row = new DataGridViewRow();
                        Row.CreateCells(grdCourseSections);
                        Row.Tag = vCourseSection;
                        Row.Cells[colWeekDay.Index].Value = vCourseSection.WeekDay;
                        Row.Cells[colPeriod.Index].Value = vCourseSection.Period;
                        Row.Cells[colLength.Index].Value = vCourseSection.Length;
                        Row.Cells[colLongBreak.Index].Value = vCourseSection.LongBreak;
                        Row.Cells[colWeekFlag.Index].Value = Utility.GetWeekFlagStr(vCourseSection.WeekFlag);
                        Row.Cells[colWeekdayPeriod.Index].Value = vCourseSection.WeekDayCond;
                        Row.Cells[colPeriodCond.Index].Value = vCourseSection.PeriodCond;
                        Row.Cells[colClassroom.Index].Value = ClassroomName;

                        Row.Cells[colTeacher1.Index].Value = vCourseSection.TeacherName1;
                        Row.Cells[colTeacher2.Index].Value = vCourseSection.TeacherName2;
                        Row.Cells[colTeacher3.Index].Value = vCourseSection.TeacherName3;

                        #region 教師1
                        string errorString1 = "";

                        if (!string.IsNullOrWhiteSpace(vCourseSection.TeacherName1))
                        {
                            if (!Sunset_Teachers.ContainsKey(vCourseSection.TeacherName1))
                                errorString1 = "不存在於排課教師清單";

                            if (!ischool_Teachers.ContainsKey(vCourseSection.TeacherName1))
                            {
                                if (string.IsNullOrEmpty(errorString1))
                                    errorString1 = "不存在於ischool教師清單";
                                else
                                    errorString1 += "\n不存在於ischool教師清單";
                            }
                        }

                        Row.Cells[colTeacher1.Index].ErrorText = errorString1; 
                        #endregion

                        #region 教師2
                        string errorString2 = "";

                        if (!string.IsNullOrWhiteSpace(vCourseSection.TeacherName2))
                        {
                            if (!Sunset_Teachers.ContainsKey(vCourseSection.TeacherName2))
                                errorString2 = "不存在於排課教師清單";

                            if (!ischool_Teachers.ContainsKey(vCourseSection.TeacherName2))
                            {
                                if (string.IsNullOrEmpty(errorString2))
                                    errorString2 = "不存在於ischool教師清單";
                                else
                                    errorString2 += "\n不存在於ischool教師清單";
                            }
                        }

                        Row.Cells[colTeacher2.Index].ErrorText = errorString2; 
                        #endregion

                        #region 教師3
                        string errorString3 = "";

                        if (!string.IsNullOrWhiteSpace(vCourseSection.TeacherName3))
                        {
                            if (!Sunset_Teachers.ContainsKey(vCourseSection.TeacherName3))
                                errorString3 = "不存在於排課教師清單";

                            if (!ischool_Teachers.ContainsKey(vCourseSection.TeacherName3))
                            {
                                if (string.IsNullOrEmpty(errorString3))
                                    errorString3 = "不存在於ischool教師清單";
                                else
                                    errorString3 += "\n不存在於ischool教師清單";
                            }
                        }

                        Row.Cells[colTeacher3.Index].ErrorText = errorString3; 
                        #endregion

                        Row.Cells[Comment.Index].Value = vCourseSection.Comment;

                        grdCourseSections.Rows.Add(Row);
                        #endregion

                        //int AddRowIndex = grdCourseSections.Rows.Add();
                        //DataGridViewRow Row = grdCourseSections.Rows[AddRowIndex];
                        //Row.Tag = vCourseSection;
                        //grdCourseSections.Rows[AddRowIndex].SetValues(
                        //    vCourseSection.WeekDay,
                        //    vCourseSection.Period,
                        //    vCourseSection.Length,
                        //    vCourseSection.LongBreak,
                        //    Utility.GetWeekFlagStr(vCourseSection.WeekFlag),
                        //    vCourseSection.WeekDayCond,
                        //    vCourseSection.PeriodCond,
                        //    ClassroomName,
                        //    vCourseSection.TeacherName1,
                        //    vCourseSection.TeacherName2,
                        //    vCourseSection.TeacherName3,
                        //    vCourseSection.Comment
                        //    );
                    }
                }

                SaveButtonVisible = false;
                CancelButtonVisible = false;

                DataListener.Reset();
                DataListener.ResumeListen();
            }
        }

        /// <summary>
        /// 取得元件，傳回自己
        /// </summary>
        public UserControl Control
        {
            get { return this; }
        }
        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CourseSectionEditor));
            this.grdCourseSections = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colLongBreak = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colWeekFlag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colClassroom = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.contextMenuStripDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxExColumn1 = new Sunset.DataGridViewComboBoxExColumn();
            this.dataGridViewComboBoxExColumn2 = new Sunset.DataGridViewComboBoxExColumn();
            this.dataGridViewComboBoxExColumn3 = new Sunset.DataGridViewComboBoxExColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeekDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeekdayPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriodCond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTeacher1 = new Sunset.DataGridViewComboBoxExColumn();
            this.colTeacher2 = new Sunset.DataGridViewComboBoxExColumn();
            this.colTeacher3 = new Sunset.DataGridViewComboBoxExColumn();
            this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdCourseSections)).BeginInit();
            this.contextMenuStripDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdCourseSections
            // 
            this.grdCourseSections.AllowUserToResizeRows = false;
            this.grdCourseSections.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdCourseSections.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdCourseSections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCourseSections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWeekDay,
            this.colPeriod,
            this.colLength,
            this.colLongBreak,
            this.colWeekFlag,
            this.colWeekdayPeriod,
            this.colPeriodCond,
            this.colClassroom,
            this.colTeacher1,
            this.colTeacher2,
            this.colTeacher3,
            this.Comment});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdCourseSections.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdCourseSections.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdCourseSections.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdCourseSections.HighlightSelectedColumnHeaders = false;
            this.grdCourseSections.Location = new System.Drawing.Point(10, 8);
            this.grdCourseSections.MultiSelect = false;
            this.grdCourseSections.Name = "grdCourseSections";
            this.grdCourseSections.RowHeadersWidth = 35;
            this.grdCourseSections.RowTemplate.Height = 24;
            this.grdCourseSections.Size = new System.Drawing.Size(530, 194);
            this.grdCourseSections.TabIndex = 4;
            this.grdCourseSections.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdCourseSections_CellBeginEdit);
            this.grdCourseSections.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdCourseSections_CellEndEdit);
            this.grdCourseSections.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdCourseSections_CellEnter);
            this.grdCourseSections.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdCourseSections_CellMouseClick);
            this.grdCourseSections.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdCourseSections_CurrentCellDirtyStateChanged);
            this.grdCourseSections.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdCourseSections_UserDeletingRow);
            // 
            // colLongBreak
            // 
            this.colLongBreak.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colLongBreak.HeaderText = "跨中午";
            this.colLongBreak.Name = "colLongBreak";
            this.colLongBreak.Width = 53;
            // 
            // colWeekFlag
            // 
            this.colWeekFlag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colWeekFlag.HeaderText = "單雙週";
            this.colWeekFlag.Items.AddRange(new object[] {
            "單雙",
            "單",
            "雙"});
            this.colWeekFlag.Name = "colWeekFlag";
            this.colWeekFlag.Width = 53;
            // 
            // colClassroom
            // 
            this.colClassroom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClassroom.HeaderText = "上課場地";
            this.colClassroom.Name = "colClassroom";
            this.colClassroom.Width = 66;
            // 
            // contextMenuStripDelete
            // 
            this.contextMenuStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.contextMenuStripDelete.Name = "contextMenuStrip1";
            this.contextMenuStripDelete.ShowImageMargin = false;
            this.contextMenuStripDelete.Size = new System.Drawing.Size(70, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(69, 22);
            this.toolStripMenuItem2.Text = "刪除";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "星期";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.ToolTipText = "只能輸入0到7，0代表末排課";
            this.dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.HeaderText = "節次";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ToolTipText = "節次必須介於0到20之間，0代表午休或未排課（星期為）。";
            this.dataGridViewTextBoxColumn2.Width = 59;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.HeaderText = "節數";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ToolTipText = "節數必須介於1到20之間。";
            this.dataGridViewTextBoxColumn3.Width = 59;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn4.HeaderText = "星期條件";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 85;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn5.HeaderText = "節次條件";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 85;
            // 
            // dataGridViewComboBoxExColumn1
            // 
            this.dataGridViewComboBoxExColumn1.HeaderText = "授課教師一";
            this.dataGridViewComboBoxExColumn1.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("dataGridViewComboBoxExColumn1.Items")));
            this.dataGridViewComboBoxExColumn1.Name = "dataGridViewComboBoxExColumn1";
            this.dataGridViewComboBoxExColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxExColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewComboBoxExColumn2
            // 
            this.dataGridViewComboBoxExColumn2.HeaderText = "授課教師二";
            this.dataGridViewComboBoxExColumn2.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("dataGridViewComboBoxExColumn2.Items")));
            this.dataGridViewComboBoxExColumn2.Name = "dataGridViewComboBoxExColumn2";
            this.dataGridViewComboBoxExColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxExColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewComboBoxExColumn3
            // 
            this.dataGridViewComboBoxExColumn3.HeaderText = "授課教師三";
            this.dataGridViewComboBoxExColumn3.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("dataGridViewComboBoxExColumn3.Items")));
            this.dataGridViewComboBoxExColumn3.Name = "dataGridViewComboBoxExColumn3";
            this.dataGridViewComboBoxExColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxExColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "註記";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // colWeekDay
            // 
            this.colWeekDay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colWeekDay.HeaderText = "星期";
            this.colWeekDay.Name = "colWeekDay";
            this.colWeekDay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colWeekDay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWeekDay.ToolTipText = "只能輸入0到7，0代表末排課";
            this.colWeekDay.Width = 40;
            // 
            // colPeriod
            // 
            this.colPeriod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPeriod.HeaderText = "節次";
            this.colPeriod.Name = "colPeriod";
            this.colPeriod.ToolTipText = "節次必須介於0到20之間，0代表午休或未排課（星期為）。";
            this.colPeriod.Width = 59;
            // 
            // colLength
            // 
            this.colLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLength.HeaderText = "節數";
            this.colLength.MaxInputLength = 20;
            this.colLength.Name = "colLength";
            this.colLength.ToolTipText = "節數必須介於1到20之間。";
            this.colLength.Width = 59;
            // 
            // colWeekdayPeriod
            // 
            this.colWeekdayPeriod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colWeekdayPeriod.HeaderText = "星期條件";
            this.colWeekdayPeriod.Name = "colWeekdayPeriod";
            this.colWeekdayPeriod.Width = 85;
            // 
            // colPeriodCond
            // 
            this.colPeriodCond.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colPeriodCond.HeaderText = "節次條件";
            this.colPeriodCond.Name = "colPeriodCond";
            this.colPeriodCond.Width = 85;
            // 
            // colTeacher1
            // 
            this.colTeacher1.HeaderText = "授課教師一";
            this.colTeacher1.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("colTeacher1.Items")));
            this.colTeacher1.Name = "colTeacher1";
            this.colTeacher1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTeacher1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colTeacher2
            // 
            this.colTeacher2.HeaderText = "授課教師二";
            this.colTeacher2.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("colTeacher2.Items")));
            this.colTeacher2.Name = "colTeacher2";
            this.colTeacher2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTeacher2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colTeacher3
            // 
            this.colTeacher3.HeaderText = "授課教師三";
            this.colTeacher3.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("colTeacher3.Items")));
            this.colTeacher3.Name = "colTeacher3";
            this.colTeacher3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTeacher3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Comment
            // 
            this.Comment.HeaderText = "註記";
            this.Comment.Name = "Comment";
            // 
            // CourseSectionEditor
            // 
            this.Controls.Add(this.grdCourseSections);
            this.Name = "CourseSectionEditor";
            this.Size = new System.Drawing.Size(550, 210);
            this.Load += new System.EventHandler(this.CourseSectionEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCourseSections)).EndInit();
            this.contextMenuStripDelete.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void CourseSectionEditor_Load(object sender, EventArgs e)
        {
            //設定權限
            UserPermission = FISCA.Permission.UserAcl.Current[FCode.GetCode(GetType())];
            this.Enabled = UserPermission.Editable;

            //處理DataError事件
            grdCourseSections.DataError += (vsender, ve) => { };

            //取得課程分段
            mbkwCourseSection.DoWork += new DoWorkEventHandler(bkwCourseSection_DoWork);

            //完成取得課程分段
            mbkwCourseSection.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkwCourseSection_RunWorkerCompleted);

            //判斷DataGridView狀態變更
            DataListener = new ChangeListener();
            DataListener.Add(new DataGridViewSource(grdCourseSections));
            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(DataListener_StatusChanged);

            //終止變更判斷
            DataListener.SuspendListen();

            CourseSectionEvents.CourseSectionChanged += (vsender, ve) => OnPrimaryKeyChanged(new EventArgs());
        }
    }
}