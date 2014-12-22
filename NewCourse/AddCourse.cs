using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FISCA.UDT;
using FISCA.Presentation.Controls;
using System.Text;
using System.Data;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 新增課程
    /// </summary>
    public partial class AddCourse : FISCA.Presentation.Controls.BaseForm
    {
        int _SchoolYear = 0;
        string _Semester = "";

        public AddCourse()
        {
            InitializeComponent();
        }


        private void AddCourse_Load(object sender, EventArgs e)
        {
            if (int.TryParse(K12.Data.School.DefaultSchoolYear, out _SchoolYear))
            {
                cboSchoolYear.Items.Add(_SchoolYear - 2);
                cboSchoolYear.Items.Add(_SchoolYear - 1);
                cboSchoolYear.Items.Add(_SchoolYear);
                cboSchoolYear.Items.Add(_SchoolYear + 1);
                cboSchoolYear.Items.Add(_SchoolYear + 2);
                cboSchoolYear.Text = K12.Data.School.DefaultSchoolYear;
            }

            DataTable dt = tool._Q.Select("select semester from " + Tn._CourseExtension);
            List<string> semesterList = new List<string>();
            semesterList.Add("1");
            semesterList.Add("2");
            foreach (DataRow row in dt.Rows)
            {
                string semester = "" + row["semester"];
                if (string.IsNullOrEmpty(semester))
                    continue;

                if (!semesterList.Contains(semester))
                {
                    semesterList.Add(semester);
                }
            }
            semesterList.Sort();
            cboSemester.Items.AddRange(semesterList.ToArray());
            _Semester = cboSemester.Text = K12.Data.School.DefaultSemester;

            cboSchoolYear.TextChanged += new EventHandler(cboSchoolYear_TextChanged);
            cboSemester.TextChanged += new EventHandler(cboSemester_TextChanged);

        }

        void cboSchoolYear_TextChanged(object sender, EventArgs e)
        {
            errSchoolYear.Clear();

            if (!int.TryParse(cboSchoolYear.Text, out _SchoolYear))
            {
                errSchoolYear.SetError(cboSchoolYear, "學年度必須是數字！");
            }
        }

        void cboSemester_TextChanged(object sender, EventArgs e)
        {
            errSemester.Clear();

            if (string.IsNullOrWhiteSpace(cboSemester.Text))
            {
                errSemester.SetError(cboSemester, "學期不得為空白！");
            }
            else
            {
                _Semester = cboSemester.Text;
            }
        }

        /// <summary>
        /// 依據課程名稱,學年度學期取得課程
        /// </summary>
        private SchedulerCourseExtension GetCourse(string CourseName, string SchoolYear, string Semester)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("course_name='{0}' and school_year='{1}' and semester='{2}'", CourseName, SchoolYear, Semester);
            List<SchedulerCourseExtension> Courses = tool._A.Select<SchedulerCourseExtension>(sb.ToString());

            //當取得課程大於1個的時候,只傳出第一個課程
            return Courses.Count > 0 ? Courses[0] : null;
        }

        /// <summary>
        /// 儲存
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //當課程名稱未輸入時
            //即不新增課程
            if (txtName.Text.Trim() == "")
            {
                MsgBox.Show("請輸入課程名稱!!");
                return;
            }

            if (!string.IsNullOrEmpty(errSchoolYear.GetError(cboSchoolYear)))
            {
                MsgBox.Show("請修正學年度資料!!");
                return;
            }

            if (!string.IsNullOrEmpty(errSemester.GetError(cboSemester)))
            {
                MsgBox.Show("請修正學期資料!!");
                return;
            }

            if (!(GetCourse(txtName.Text, _SchoolYear.ToString(), _Semester) != null))
            {
                //一個新的排課課程
                SchedulerCourseExtension Course = new SchedulerCourseExtension();

                Course.CourseName = txtName.Text.Trim();
                Course.SchoolYear = _SchoolYear;
                //學期可能不是數字
                //是因為排課時學期可能是"暑期"...等文字狀態的學期
                Course.Semester = _Semester.ToString();
                Course.Save();

                //引發課程修改事件
                CourseEvents.RaiseChanged();

                //如果使用者勾選輸入其它內容
                if (chkInputData.Checked == true)
                {
                    //如果課程有新增成功
                    //SchedulerCourseExtension NewCourse = GetCourse(Course.CourseName, "" + Course.SchoolYear, Course.Semester);
                    if (!string.IsNullOrEmpty(Course.UID))
                    {
                        CourseAdmin.Instance.PopupDetailPane(Course.UID);
                    }
                }

                //LOG記錄
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("新增排課課程：");
                sb.AppendLine("課程名稱「" + Course.CourseName + "」");
                sb.AppendLine("學年度「" + Course.SchoolYear + "」");
                sb.AppendLine("學期「" + Course.Semester + "」");
                FISCA.LogAgent.ApplicationLog.Log("排課", "新增課程", sb.ToString());

                MsgBox.Show("課程新增完成!!");
                this.Close();

            }
            else
            {
                MessageBox.Show("課程名稱重複");
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
