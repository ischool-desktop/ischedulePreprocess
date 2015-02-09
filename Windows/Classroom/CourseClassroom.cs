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
    /// 指定課程預設場地
    /// </summary>
    public class CourseClassroom
    {
        private const string DefaultClassroom = "指定預設場地";
        private Dictionary<string,Classroom> mClassrooms;
        private Dictionary<string,CourseExtension> mCourseExtensions;
        private AccessHelper mHelper;
        private RibbonBarButton AssignTimeTableButton;
        private MenuButton NoNoAssignTimeTableButton;
        private LogSaver mLogSaver = ApplicationLog.CreateLogSaverInstance();
        private ListPaneField mCourseClassroomField;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public CourseClassroom()
        {
            mHelper = new AccessHelper();
            mClassrooms = new Dictionary<string, Classroom>();
            mCourseExtensions = new Dictionary<string,CourseExtension>();
            mCourseClassroomField = new ListPaneField(DefaultClassroom);
        }

        /// <summary>
        /// 設定班級時間表顯示欄位
        /// </summary>
        public void SetupClassroomNameField()
        {
            //在載入欄位資料前的準備
            mCourseClassroomField.PreloadVariableBackground += (sender, e) =>
            {
                //先將資料清空
                mClassrooms.Clear();
                mCourseExtensions.Clear();

                //取得所有場地
                mClassrooms = mHelper
                    .Select<Classroom>()
                    .ToDictionary(x => x.UID);

                //取得所有課程排課資料                
                foreach(CourseExtension vCourseExtension in mHelper.Select<CourseExtension>())
                {
                    if (!mCourseExtensions.ContainsKey(""+vCourseExtension.CourseID))
                        mCourseExtensions.Add(""+vCourseExtension.CourseID, vCourseExtension);
                }
            };

            //實際取得欄位值
            mCourseClassroomField.GetVariable += (sender, e) =>
            {
                //根據課程系統編號取得班級排課資料
                CourseExtension CourseTimeTable = mCourseExtensions.ContainsKey(e.Key) ? 
                    mCourseExtensions[e.Key] : null;

                //根據課程排課資料取得場地
                Classroom Classroom = GetClassroom(CourseTimeTable);

                //顯示場地名稱
                if (Classroom != null)
                    e.Value = Classroom.ClassroomName;
                else
                    e.Value = string.Empty;
            };

            NLDPanels.Course.AddListPaneField(mCourseClassroomField);

            //當課程資料變更時重新載入，需補上CourseExtension及Classroom變動時也需重新載入；此部份需修改UDT。
            Course.AfterUpdate += (sender, e) => mCourseClassroomField.Reload();
        }

        /// <summary>
        /// 根據課程排課資料取得場地
        /// </summary>
        /// <param name="CourseExtension">課程排課資料</param>
        /// <returns>場地</returns>
        private Classroom GetClassroom(CourseExtension CourseExtension)
        {
            //若課程排課資料不為null，且課程排課資料的場地不為空值
            if (CourseExtension != null && CourseExtension.ClassroomID.HasValue)
            {
                //判斷是否有包含場地，有的話傳回，否則傳回null
                string ClassroomID = K12.Data.Int.GetString(CourseExtension.ClassroomID);

                if (mClassrooms.ContainsKey(ClassroomID))
                    return mClassrooms[ClassroomID];
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// 建立指定場地按鈕
        /// </summary>
        public void AddAssignClassroomButtons()
        {
            //新增指定按鈕並加入事件
            AssignTimeTableButton = K12.Presentation.NLDPanels.Course.RibbonBarItems["排課"][DefaultClassroom];
            AssignTimeTableButton.Image = Resources.classroom_down_128;
            AssignTimeTableButton.Enable = false;
            AssignTimeTableButton.PopupOpen += new EventHandler<PopupOpenEventArgs>(AssignButton_PopupOpen); //在指定的事件中會新增所有時間表

            //新增不指定按鈕並加入事件
            NoNoAssignTimeTableButton = AssignTimeTableButton["不指定"];
            NoNoAssignTimeTableButton.Tag = string.Empty;
            NoNoAssignTimeTableButton.Click += new EventHandler(ChangeClassroomID);

            //若有選取課程，以及有權限才將指定按鈕啟用
            K12.Presentation.NLDPanels.Course.SelectedSourceChanged += (sender, e) =>
            {
                int SelectedCount = K12.Presentation.NLDPanels.Course.SelectedSource.Count;
                bool IsExecutable = UserAcl.Current["Sunset.Ribbon0110"].Executable;
                bool IsEnable = (SelectedCount> 0 && IsExecutable);
                AssignTimeTableButton.Enable = IsEnable;
            };
        }

        /// <summary>
        /// 建立各別場地按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssignButton_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            RibbonBarButton button = sender as RibbonBarButton;
            if (K12.Presentation.NLDPanels.Course.SelectedSource.Count <= 0)
                return;

            //針對每個場地建立按鈕
            foreach (Classroom Classroom in mHelper.Select<Classroom>())
            {
                MenuButton mb = e.VirtualButtons[Classroom.ClassroomName];
                mb.Tag = Classroom.UID;
                mb.Click += new EventHandler(ChangeClassroomID);
            }
        }

        /// <summary>
        /// 改變場地函式，其中場地系統編號放在Tag中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeClassroomID(object sender, EventArgs e)
        {
            //取得觸發事件的來源按鈕，並且取出場地系統編號
            MenuButton MenuButton = sender as MenuButton;
            int? AssignClassroomID = K12.Data.Int.ParseAllowNull("" + MenuButton.Tag);

            mLogSaver.ClearBatch();

            //根據選取的班級系統編號取得課程排課資料
            string strCondition = string.Join(",",NLDPanels.Course.SelectedSource.ToArray());
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
                        //指定該班級的時間表
                        CourseExtension UpdateCourseExtension = CourseExtensions
                            .Find(y => y.CourseID.Equals(CourseID));
                        UpdateCourseExtension.ClassroomID = AssignClassroomID;
                        CourseExtensions.Add(UpdateCourseExtension);
                    }
                    else
                    {
                        //若課程排課課程資料不存在則新增，並指定場地
                        CourseExtension InsertCourseExtension = new CourseExtension();

                        InsertCourseExtension.CourseID = CourseID;
                        InsertCourseExtension.ClassroomID = AssignClassroomID;
                        InsertCourseExtension.TimeTableID = null;
                        InsertCourseExtension.AllowDup = false;
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
            mCourseClassroomField.Reload();

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