using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ClassTimeTable
    {
        private Dictionary<int,TimeTable> mTimeTables;
        private Dictionary<int,ClassExtension> mClassTimeTables;
        private AccessHelper mHelper;
        private RibbonBarButton AssignTimeTableButton;
        private MenuButton NoNoAssignTimeTableButton;
        private LogSaver mLogSaver = ApplicationLog.CreateLogSaverInstance();
        private ListPaneField mClassTimeTableField;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassTimeTable()
        {
            mHelper = new AccessHelper();
            mTimeTables = new Dictionary<int, TimeTable>();
            mClassTimeTables = new Dictionary<int, ClassExtension>();
            mClassTimeTableField = new ListPaneField("時間表");
        }

        /// <summary>
        /// 設定班級時間表顯示欄位
        /// </summary>
        public void SetupTimeTableNameField()
        {
            //在載入欄位資料前的準備
            mClassTimeTableField.PreloadVariableBackground += (sender, e) =>
            {
                //先將資料清空
                mTimeTables.Clear();
                mClassTimeTables.Clear();

                //取得所有時間表
                mTimeTables = mHelper
                    .Select<TimeTable>()
                    .ToDictionary(x => K12.Data.Int.Parse(x.UID));

                //取得所有班級排課資料
                mClassTimeTables = mHelper
                    .Select<ClassExtension>()
                    .ToDictionary(x => x.ClassID);
            };

            //實際取得欄位值
            mClassTimeTableField.GetVariable += (sender, e) =>
            {
                int ClassID = int.Parse(e.Key);
                //根據班級系統編號取得班級排課資料
                ClassExtension ClassTimeTable = mClassTimeTables.ContainsKey(ClassID) ?
                    mClassTimeTables[ClassID] : null;

                //根據班級排課資料取得時間表 
                TimeTable TimeTable = GetTimeTable(ClassTimeTable);

                //顯示時間表名稱
                if (TimeTable != null)
                    e.Value = TimeTable.TimeTableName;
                else
                    e.Value = string.Empty;
            };

            NLDPanels.Class.AddListPaneField(mClassTimeTableField);

            //當班級資料變更時重新載入，需補上ClassExtension及TimeTable變動時也需重新載入；此部份需修改UDT。
            Class.AfterUpdate += (sender, e) => mClassTimeTableField.Reload();
        }

        /// <summary>
        /// 根據班級排課資料取得時間表
        /// </summary>
        /// <param name="ClassTimeTable">班級排課資料</param>
        /// <returns>時間表</returns>
        private TimeTable GetTimeTable(ClassExtension ClassTimeTable)
        {
            //若班級排課資料不為null，且班級排課資料的時間表不為空值
            if (ClassTimeTable != null && ClassTimeTable.TimeTableID.HasValue)
            {
                //判斷是否有包含時間表，有的話傳回，否則傳回null
                if (mTimeTables.ContainsKey(ClassTimeTable.TimeTableID.Value))
                    return mTimeTables[ClassTimeTable.TimeTableID.Value];
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// 建立指定時間表按鈕
        /// </summary>
        public void AddAssignTimeTableButtons()
        {
            //新增指定按鈕並加入事件
            AssignTimeTableButton = K12.Presentation.NLDPanels.Class.RibbonBarItems["排課"]["指定預設時間表"];
            AssignTimeTableButton.Image = Resources.lesson_planning_down_128; 
            AssignTimeTableButton.Enable = false;
            AssignTimeTableButton.PopupOpen += new EventHandler<PopupOpenEventArgs>(AssignButton_PopupOpen); //在指定的事件中會新增所有時間表

            //新增不指定按鈕並加入事件
            NoNoAssignTimeTableButton = AssignTimeTableButton["不指定"];
            NoNoAssignTimeTableButton.Tag = string.Empty;
            NoNoAssignTimeTableButton.Click += new EventHandler(ChangeTimeTableID);

            //若有選取班級，以及有權限才將指定按鈕啟用
            K12.Presentation.NLDPanels.Class.SelectedSourceChanged += (sender, e) =>
            {
                int SelectedCount = K12.Presentation.NLDPanels.Class.SelectedSource.Count;
                bool IsExecutable = UserAcl.Current["Sunset.Ribbon0050"].Executable;
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
            if (K12.Presentation.NLDPanels.Class.SelectedSource.Count <= 0)
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

            //根據選取的班級系統編號取得課程排課資料
            string strCondition = string.Join(",",NLDPanels.Class.SelectedSource.ToArray());
            List<ClassExtension> ClassExtensions = mHelper
                .Select<ClassExtension>("ref_class_id in ("+strCondition+")");
            List<int> ClassIDs = ClassExtensions
                .Select(x => x.ClassID)
                .ToList();

            //針對選取的每筆班級
            NLDPanels.Class.SelectedSource.ForEach
            (x=>
                {
                    int ClassID = K12.Data.Int.Parse(x);

                    //若班級排課課程資料有存在
                    if (ClassIDs.Contains(ClassID))
                    {
                        //指定該班級的時間表
                        ClassExtension UpdateClassExtension = ClassExtensions
                            .Find(y => y.ClassID.Equals(ClassID));
                        UpdateClassExtension.TimeTableID = AssignTimeTableID;
                        ClassExtensions.Add(UpdateClassExtension);
                    }
                    else
                    {
                        //若班級排課課程資料不存在則新增，並指定時間表
                        ClassExtension InsertClassExtension = new ClassExtension();
                        InsertClassExtension.ClassID = ClassID;
                        InsertClassExtension.TimeTableID = AssignTimeTableID;
                        ClassExtensions.Add(InsertClassExtension);
                    }
                }
            );

            mHelper.SaveAll(ClassExtensions);

            //此處需改用Listen UDT Event
            mClassTimeTableField.Reload();

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