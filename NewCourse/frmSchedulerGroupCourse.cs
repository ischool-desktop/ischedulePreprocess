using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FISCA.Data;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 群組課程
    /// </summary>
    public partial class frmSchedulerGroupCourse : FISCA.Presentation.Controls.BaseForm
    {
        private List<string> mCourseIDs;
        private AccessHelper mHelper = new AccessHelper();

        /// <summary>
        /// 建構式，傳入課程系統編號列表
        /// </summary>
        /// <param name="CourseIDs"></param>
        public frmSchedulerGroupCourse(List<string> CourseIDs)
        {
            InitializeComponent();

            if (K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
                throw new NullReferenceException("課程系統編號為空集合！");

            mCourseIDs = CourseIDs;
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbGroup.Text))
            {
                MessageBox.Show("請輸入課程群組！");
                return;
            }

            #region 根據指定課程系統編號及群組名稱取得課程分課
            string GroupName = cmbGroup.Text;
            string strCondition = "$scheduler.scheduler_course_section.ref_course_id in (" + string.Join(",", mCourseIDs.ToArray()) + ")";
            string strSQL = "select $scheduler.scheduler_course_section.uid,$scheduler.scheduler_course_section.ref_course_id,course_group from $scheduler.scheduler_course_section inner join $scheduler.scheduler_course_extension on $scheduler.scheduler_course_section.ref_course_id=$scheduler.scheduler_course_extension.uid where course_group='" + GroupName + "' or " + strCondition;

            QueryHelper helper = new QueryHelper();
            DataTable table = helper.Select(strSQL);
            #endregion

            #region 取得課程分段中的系統編號及課程系統編號
            List<string> CourseIDs = new List<string>();
            List<string> CourseSectionIDs = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string CourseID = row.Field<string>("ref_course_id");
                string CourseSectionID = row.Field<string>("uid");

                CourseIDs.Add(CourseID);
                CourseSectionIDs.Add(CourseSectionID);
            }

            CourseIDs = CourseIDs.Distinct().ToList();
            #endregion

            #region 判斷是否有未產生課程分段的課程
            List<string> NotAssignCourseIDs = new List<string>();

            foreach(string mCourseID in mCourseIDs)
            {
                if (!CourseIDs.Contains(mCourseID))
                    NotAssignCourseIDs.Add(mCourseID);
            }

            if (NotAssignCourseIDs.Count > 0)
            {
                CourseAdmin.Instance.AddToTemp(NotAssignCourseIDs);
                string CourseNames = Utility.GetCourseNames(NotAssignCourseIDs);
                MessageBox.Show("課程" + CourseNames  + "未包含課程分段，已加入至待處理！");
                
                return;
            }

            if (mCourseIDs.Count > 3)
            {
                if (MessageBox.Show("課程群組數目大於3門課程，在排課時可能造成效率過慢，是否繼續？", "溫馨提醒！", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }

            #endregion

            #region 實際取得課程分段，判斷是否其屬性相同
            List<SchedulerCourseSection> Sections = mHelper
                .Select<SchedulerCourseSection>("uid in (" + string.Join(",", CourseSectionIDs.ToArray()) + ")");

            Tuple<bool,string> Result = Sections.IsSchedulerSameCourseGroup();

            if (!Result.Item1)
            {
                MessageBox.Show(Result.Item2);
                return;
            }
            #endregion

            //實際群組課程
            SunsetBL.GroupSchedulerCourse(mCourseIDs, GroupName);
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGroupCourse_Load(object sender, EventArgs e)
        {
            QueryHelper helper = new QueryHelper();

            DataTable table = helper.Select("select distinct course_group from $scheduler.scheduler_course_extension");

            List<string> CourseGroups = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                string CourseGroup = row.Field<string>("course_group");
                CourseGroups.Add(CourseGroup);
            }

            CourseGroups.ForEach(x => cmbGroup.Items.Add(x));
        }
    }
}