using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 複製課程資料到其他學期。
    /// </summary>
    public partial class CopyCourseForm : BaseForm
    {
        public CopyCourseForm()
        {
            InitializeComponent();
        }

        private void CopyCourseForm_Load(object sender, EventArgs e)
        {
            try
            {
                int school_year = int.Parse(K12.Data.School.DefaultSchoolYear);

                for (int i = school_year + 1; i < (school_year + 3); i++)
                    cboSchoolYear.Items.Add(i);
            }
            catch { }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                int schoolYear;

                #region 資料檢查
                if (!int.TryParse(cboSchoolYear.Text, out schoolYear))
                {
                    MessageBox.Show("學年度必須是數字。");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cboSemester.Text))
                {
                    MessageBox.Show("學期必須有值。");
                    return;
                }
                #endregion

                List<string> selectedCourseId = CourseAdmin.Instance.SelectedSource;

                if (selectedCourseId.Count <= 0)
                    return;

                List<SchedulerCourseExtension> src_courses = tool._A.Select<SchedulerCourseExtension>(selectedCourseId);

                string strSelectedCourseId = "'" + string.Join("','", selectedCourseId.ToArray()) + "'";
                string strCondition = string.Format("ref_course_id in ({0})", strSelectedCourseId);
                List<SchedulerCourseSection> src_sections = tool._A.Select<SchedulerCourseSection>(strCondition);

                List<SchedulerCourseExtension> clone_courses = src_courses.ConvertAll(x => x.Clone());
                List<SchedulerCourseSection> clone_sections = src_sections.ConvertAll(x => x.Clone());

                clone_courses.ForEach(x =>
                {
                    x.SchoolYear = schoolYear;
                    x.Semester = cboSemester.Text.Trim();
                });

                #region 檢查新學期是否有重覆課程，如果有就直接不給複製。
                HashSet<string> exists = GetTargetSemesterCourses(schoolYear, cboSemester.Text.Trim());
                foreach (SchedulerCourseExtension course in clone_courses)
                {
                    if (exists.Contains(course.CourseName))
                    {
                        MessageBox.Show("課程名稱有重覆，無法複製。");
                        return;
                    }
                }
                #endregion

                List<string> new_courseIds = tool._A.InsertValues(clone_courses);
                List<SchedulerCourseExtension> new_courses = tool._A.Select<SchedulerCourseExtension>(new_courseIds);

                CourseLookup courseLookup = new CourseLookup(src_courses, new_courses);

                clone_sections.ForEach(x =>
                {
                    SchedulerCourseExtension course = courseLookup.LookupNewCourse(x.CourseID);

                    if (course != null)
                        x.CourseID = int.Parse(course.UID);
                    else
                        x.CourseID = 0;
                });

                tool._A.InsertValues(clone_sections);

                MessageBox.Show("複製完成。");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private HashSet<string> GetTargetSemesterCourses(int schoolYear, string semester)
        {
            string sql = "select course_name from $scheduler.scheduler_course_extension where school_year='{0}' and semester='{1}'";
            sql = string.Format(sql, schoolYear, semester);

            DataTable dt = tool._Q.Select(sql);
            HashSet<string> result = new HashSet<string>();
            foreach (DataRow row in dt.Rows)
            {
                string name = row["course_name"] + "";
                name = name.Trim();
                if (!result.Contains(name))
                    result.Add(name);
            }

            return result;
        }

        /// <summary>
        /// 處理用舊課程編號換新課程物件。
        /// </summary>
        class CourseLookup
        {
            private Dictionary<int, SchedulerCourseExtension> OldNewMapping;

            public CourseLookup(List<SchedulerCourseExtension> oldCourses,
                List<SchedulerCourseExtension> newCourses)
            {
                OldNewMapping = new Dictionary<int, SchedulerCourseExtension>();

                Dictionary<string, SchedulerCourseExtension> dicNewCourses = newCourses.ToDictionary(x =>
                {
                    return x.CourseName.Trim();
                });

                foreach (SchedulerCourseExtension oldCourse in oldCourses)
                {
                    OldNewMapping.Add(int.Parse(oldCourse.UID), dicNewCourses[oldCourse.CourseName]);
                }
            }

            public SchedulerCourseExtension LookupNewCourse(int oldCourseUid)
            {
                if (OldNewMapping.ContainsKey(oldCourseUid))
                    return OldNewMapping[oldCourseUid];
                else
                    return null;
            }
        }
    }
}
