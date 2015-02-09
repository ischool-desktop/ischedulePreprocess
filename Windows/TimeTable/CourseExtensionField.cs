using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FISCA.Data;
using FISCA.LogAgent;
using FISCA.Permission;
using FISCA.Presentation;
using FISCA.UDT;
using K12.Data;
using K12.Presentation;
using Sunset.Properties;

namespace Sunset
{
    /// <summary>
    /// 指定班級上課時間表
    /// </summary>
    public class CourseExtensionField
    {
        private Dictionary<int,TimeTable> mTimeTables;
        private Dictionary<int,CourseExtension> mCourseTables;
        private Dictionary<int, string> mCourseGroups;
        private AccessHelper mHelper;
        private RibbonBarButton AssignTimeTableButton;
        private RibbonBarButton AssignCourseGroupButton;
        private MenuButton NoAssignCourseGroupButton;
        private MenuButton NoAssignTimeTableButton;
        private LogSaver mLogSaver = ApplicationLog.CreateLogSaverInstance();
        private ListPaneField mCourseTimeTableField;
        private ListPaneField mCourseGroupField;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public CourseExtensionField()
        {
            mHelper = new AccessHelper();
            mTimeTables = new Dictionary<int , TimeTable>();
            mCourseTables = new Dictionary<int,CourseExtension>();
            mCourseTimeTableField = new ListPaneField("時間表");
            mCourseGroupField = new ListPaneField("群組");
        }

        /// <summary>
        /// 設定群組顯示名稱
        /// </summary>
        public void SetupGroupNameField()
        {
            mCourseGroupField.PreloadVariableBackground += (sender, e) =>
            {
                QueryHelper helper = new QueryHelper();

                DataTable table = helper.Select("select ref_course_id,course_group from $scheduler.course_extension");                

                mCourseGroups = new Dictionary<int, string>();

                foreach (DataRow row in table.Rows)
                {
                    int CourseID = K12.Data.Int.Parse(row.Field<string>("ref_course_id"));
                    string Group = row.Field<string>("course_group");

                    if (!mCourseGroups.ContainsKey(CourseID))
                        mCourseGroups.Add(CourseID, Group);
                }
            };

            mCourseGroupField.GetVariable += (sender, e) =>
            {
                int CourseID = int.Parse(e.Key);

                e.Value = mCourseGroups.ContainsKey(CourseID) ? mCourseGroups[CourseID] : string.Empty;
            };

            NLDPanels.Course.AddListPaneField(mCourseGroupField);

            //當班級資料變更時重新載入，需補上CourseExtension及TimeTable變動時也需重新載入；此部份需修改UDT。
            Course.AfterUpdate += (sender, e) => mCourseGroupField.Reload();
        }

        /// <summary>
        /// 設定班級時間表顯示欄位
        /// </summary>
        public void SetupTimeTableNameField()
        {
            //在載入欄位資料前的準備
            mCourseTimeTableField.PreloadVariableBackground += (sender, e) =>
            {
                //先將資料清空
                mTimeTables.Clear();
                mCourseTables.Clear();

                //取得所有時間表
                mTimeTables = mHelper
                    .Select<TimeTable>()
                    .ToDictionary(x => K12.Data.Int.Parse(x.UID));

                //取得所有課程排課資料
                foreach(CourseExtension vCourseExtension in mHelper.Select<CourseExtension>())
                {
                    if (!mCourseTables.ContainsKey(vCourseExtension.CourseID))
                        mCourseTables.Add(vCourseExtension.CourseID, vCourseExtension);
                }
            };

            //實際取得欄位值
            mCourseTimeTableField.GetVariable += (sender, e) =>
            {
                int CourseID = int.Parse(e.Key);

                //根據課程系統編號取得班級排課資料
                CourseExtension CourseTimeTable = mCourseTables.ContainsKey(CourseID) ? 
                    mCourseTables[CourseID] : null;

                //根據課程排課資料取得時間表 
                TimeTable TimeTable = GetTimeTable(CourseTimeTable);

                //顯示時間表名稱
                if (TimeTable != null)
                    e.Value = TimeTable.TimeTableName;
                else
                    e.Value = string.Empty;
            };

            NLDPanels.Course.AddListPaneField(mCourseTimeTableField);

            //當班級資料變更時重新載入，需補上CourseExtension及TimeTable變動時也需重新載入；此部份需修改UDT。
            Course.AfterUpdate += (sender, e) => mCourseTimeTableField.Reload();
        }

        /// <summary>
        /// 根據課程排課資料取得時間表
        /// </summary>
        /// <param name="CourseTimeTable">課程排課資料</param>
        /// <returns>時間表</returns>
        private TimeTable GetTimeTable(CourseExtension CourseTimeTable)
        {
            //若班級排課資料不為null，且班級排課資料的時間表不為空值
            if (CourseTimeTable != null && CourseTimeTable.TimeTableID.HasValue)
            {
                //判斷是否有包含時間表，有的話傳回，否則傳回null
                if (mTimeTables.ContainsKey(CourseTimeTable.TimeTableID.Value))
                    return mTimeTables[CourseTimeTable.TimeTableID.Value];
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// 建立指定時間表按鈕
        /// </summary>
        public void AddAssignCourseGroupButtons()
        {
            //新增指定按鈕並加入事件
            AssignCourseGroupButton = K12.Presentation.NLDPanels.Course.RibbonBarItems["排課"]["指定課程群組"];
            AssignCourseGroupButton.Enable = false;
            AssignCourseGroupButton.Image = Resources.virtualcurse_lock_128;
            AssignCourseGroupButton.Click += (sender, e) =>
            {
                frmGroupCourse GroupCourse = new frmGroupCourse(K12.Presentation.NLDPanels.Course.SelectedSource);

                if (GroupCourse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    mCourseGroupField.Reload();
            };

            //新增不指定按鈕並加入事件
            NoAssignCourseGroupButton = K12.Presentation.NLDPanels.Course.RibbonBarItems["排課"]["取消課程群組"];
            NoAssignCourseGroupButton.Enable = false;
            NoAssignCourseGroupButton.Image = Resources.virtualcurse_unlock_128;
            NoAssignCourseGroupButton.Click += (sender, e) =>
            {
                //根據選取的課程系統編號取得課程排課資料
                string strCondition = string.Join(",", NLDPanels.Course.SelectedSource.ToArray());
                List<CourseExtension> CourseExtensions = mHelper
                    .Select<CourseExtension>("ref_course_id in (" + strCondition + ")");

                CourseExtensions.ForEach(x => x.CourseGroup = string.Empty);
                mHelper.SaveAll(CourseExtensions);
                mCourseGroupField.Reload();
            };

            //若有選取課程，以及有權限才將指定按鈕啟用
            K12.Presentation.NLDPanels.Course.SelectedSourceChanged += (sender, e) =>
            {
                int SelectedCount = K12.Presentation.NLDPanels.Course.SelectedSource.Count;
                bool IsExecutable = UserAcl.Current["Sunset.Ribbon0100"].Executable;
                bool IsEnable = (SelectedCount > 0 && IsExecutable);
                AssignCourseGroupButton.Enable = IsEnable;
                NoAssignCourseGroupButton.Enable = IsEnable;
            };
        }

        /// <summary>
        /// 建立指定時間表按鈕
        /// </summary>
        public void AddAssignTimeTableButtons()
        {
            //新增指定按鈕並加入事件
            AssignTimeTableButton = K12.Presentation.NLDPanels.Course.RibbonBarItems["排課"]["指定預設時間表"];
            AssignTimeTableButton.Image = Resources.lesson_planning_down_128; 
            AssignTimeTableButton.Enable = false;
            AssignTimeTableButton.PopupOpen += new EventHandler<PopupOpenEventArgs>(AssignButton_PopupOpen); //在指定的事件中會新增所有時間表

            //新增不指定按鈕並加入事件
            NoAssignTimeTableButton = AssignTimeTableButton["不指定"];
            NoAssignTimeTableButton.Enable = false;
            NoAssignTimeTableButton.Click += new EventHandler(ChangeTimeTableID);

            //若有選取班級，以及有權限才將指定按鈕啟用
            K12.Presentation.NLDPanels.Course.SelectedSourceChanged += (sender, e) =>
            {
                int SelectedCount = K12.Presentation.NLDPanels.Course.SelectedSource.Count;
                bool IsExecutable = UserAcl.Current["Sunset.Ribbon0100"].Executable;
                bool IsEnable = (SelectedCount> 0 && IsExecutable);
                AssignTimeTableButton.Enable = IsEnable;
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
            if (K12.Presentation.NLDPanels.Course.SelectedSource.Count <= 0)
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

            mLogSaver.ClearBatch();

            //根據選取的課程系統編號取得課程排課資料
            string strCondition = string.Join(",", NLDPanels.Course.SelectedSource.ToArray());
            List<CourseExtension> CourseExtensions = mHelper
                .Select<CourseExtension>("ref_course_id in (" + strCondition + ")");
            List<int> CourseIDs = CourseExtensions
                .Select(x => x.CourseID)
                .ToList();

            //針對選取的每筆班級
            NLDPanels.Course.SelectedSource.ForEach
            (x=>
                {
                    int CourseID = K12.Data.Int.Parse(x);
                    //若班級排課課程資料有存在
                    if (CourseIDs.Contains(CourseID))
                    {
                        //指定該課程的時間表
                        CourseExtension UpdateCourseExtension = CourseExtensions
                            .Find(y => y.CourseID.Equals(K12.Data.Int.Parse(x)));
                        UpdateCourseExtension.TimeTableID = AssignTimeTableID;
                        CourseExtensions.Add(UpdateCourseExtension);
                    }
                    else
                    {
                        //若課程排課課程資料不存在則新增，並指定時間表
                        CourseExtension InsertCourseExtension = new CourseExtension();
                        InsertCourseExtension.CourseID = CourseID;
                        InsertCourseExtension.TimeTableID = AssignTimeTableID;
                        InsertCourseExtension.AllowDup = false;
                        InsertCourseExtension.ClassroomID = null;
                        InsertCourseExtension.LongBreak = false;
                        InsertCourseExtension.WeekDayCond = string.Empty;
                        InsertCourseExtension.PeriodCond = string.Empty;
                        InsertCourseExtension.SplitSpec = string.Empty;
                        InsertCourseExtension.WeekFlag = 3;
                        CourseExtensions.Add(InsertCourseExtension);
                    }
                }
            );

            mHelper.SaveAll(CourseExtensions);

            //此處需改用Listen UDT Event
            mCourseTimeTableField.Reload();

            //foreach (var cla in JHClass.SelectByIDs())
            //{
            //    cla.RefProgramPlanID = id;
            //    classList.Add(cla);

            //    string desc = string.Empty;
            //    if (string.IsNullOrEmpty(id))
            //        desc = string.Format("班級「{0}」不指定課程規劃", cla.Name);
            //    else
            //        desc = string.Format("班級「{0}」指定課程規劃為：{1}", cla.Name, ProgramPlan.Instance.Items[id].Name);
            //    mLogSaver.AddBatch("成績系統.課程規劃", "班級指定課程規劃", desc);
            //}

            //if (InsertClassTimeTables.Count >0)
            //{
            //    List<string> NewIDs = mHelper.InsertValues(InsertClassTimeTables);
            //    mLogSaver.LogBatch();
            //}
            //if (UpdateClassTimeTables.Count > 0)
            //{
            //    mHelper.UpdateValues(UpdateClassTimeTables);
            //}
        }
    }
}