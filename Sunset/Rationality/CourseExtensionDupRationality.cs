using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataRationality;
using FISCA.Data;
using FISCA.UDT;

namespace Sunset
{
    /// <summary>
    /// 課程排課資料重覆檢查
    /// </summary>
    class CourseExtensionDupRationality : ICorrectableDataRationality
    {
        private List<string> UIDs = new List<string>();
        private List<string> CourseIDs = new List<string>();

        #region IDataRationality 成員

        public void AddToTemp(IEnumerable<string> EntityIDs)
        {
            K12.Presentation.NLDPanels.Course.AddToTemp(EntityIDs.ToList());
        }

        public void AddToTemp()
        {
            AddToTemp(CourseIDs);
        }

        public string Category
        {
            get { return "課程"; }
        }

        public string Description
        {
            get 
            {
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.AppendLine("檢查排課課程重覆（排課）");
                return strBuilder.ToString();
            }
        }

        public DataRationalityMessage Execute()
        {
            UIDs.Clear();

            #region 取得群組名稱不為空白的課程分段

            //if (K12.Presentation.NLDPanels.Course.SelectedSource.Count == 0)
            //{
            //    MessageBox.Show("請選擇課程！");
            //    return new DataRationalityMessage() ;
            //}

            //string CourseCondition = "(" + string.Join(",", K12.Presentation.NLDPanels.Course.SelectedSource.ToArray()) +")";

            QueryHelper helper = new QueryHelper();

            DataTable table = helper.Select("SELECT ce1.uid,course.course_name,course.school_year,course.semester,$scheduler.timetable.name as  timetable_name,ce1.split_spec FROM $scheduler.course_extension AS ce1 inner join course on course.id=ce1.ref_course_id left outer join $scheduler.timetable on $scheduler.timetable.uid=ce1.ref_timetable_id WHERE (EXISTS (SELECT ce2.uid,ce2.ref_course_id FROM $scheduler.course_extension AS ce2 WHERE (ce1.uid<>ce2.uid AND ce1.ref_course_id = ce2.ref_course_id))) order by school_year desc,semester,course_name");
            List<QueryCourse> QueryCourses = new List<QueryCourse>();

            foreach (DataRow row in table.Rows)
            {
                QueryCourse QuerySection = new QueryCourse(row);
                QueryCourses.Add(QuerySection);
            }
            #endregion

            #region 針對每個課程做檢查
            DataRationalityMessage Message = new DataRationalityMessage();

            List<object> Data = new List<object>();

            foreach (QueryCourse Course in QueryCourses)
            {
                Data.Add(new { 編號 = Course.UID, 課程名稱 = Course.CourseName, 學年度 = Course.SchoolYear, 學期 = Course.Semester, 上課時間表 = Course.TimeTableName,分割設定 = Course.SplitSpec });
                UIDs.Add(Course.UID);
                CourseIDs.Add(Course.CourseID);
            }
            #endregion

            Message.Data = Data;
            Message.Message = Data.Count > 0 ? "共有"+Data.Count+"筆課程排課資料有不合理！請檢查！" : "恭禧！所有課程排課資料皆正常！";

            return Message;
        }

        public string Name
        {
            get { return "課程排課資料重覆檢查（排課）"; }
        }

        #endregion

        #region ICorrectableDataRationality 成員

        public void ExecuteAutoCorrect(IEnumerable<string> EntityIDs)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(EntityIDs))
            {
               FISCA.Presentation.Controls.MsgBox.Show("未選取！");
            }

            if (MessageBox.Show("已選取" + EntityIDs.Count() + "筆資料，是否確認刪除？", "確認刪除課程排課資料", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                AccessHelper helper = new AccessHelper();

                List<CourseExtension> CourseExtensions = helper.Select<CourseExtension>("uid in (" + string.Join(",", EntityIDs.ToArray()) + ")");

                helper.DeletedValues(CourseExtensions);
            }
        }

        public void ExecuteAutoCorrect()
        {
            ExecuteAutoCorrect(UIDs);
        }

        #endregion
    }
}