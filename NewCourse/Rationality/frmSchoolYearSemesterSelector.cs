using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using FISCA.Data;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 選擇學年度及學期
    /// </summary>
    public partial class frmSchoolYearSemesterSelector : FISCA.Presentation.Controls.BaseForm
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        public frmSchoolYearSemesterSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 選取學年度學期
        /// </summary>
        public string SchoolYearSemester
        {
            get { return cmbSchoolYear.Text; }
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSchoolYearSemesterSelector_Load(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (vsender, ve) =>
            {
                QueryHelper helper = new QueryHelper();
                DataTable table = helper
                    .Select("select (school_year || ' ' || semester) school_year_semester from $scheduler.scheduler_course_extension group by school_year || ' ' || semester");

                List<string> SchoolYearSemester = new List<string>();

                foreach (DataRow row in table.Rows)
                {
                    string val = row.Field<string>("school_year_semester");

                    if (!string.IsNullOrWhiteSpace(val))
                        SchoolYearSemester.Add(val);
                }

                ve.Result = SchoolYearSemester;
            };

            worker.RunWorkerCompleted += (vsender, ve) =>
            {
                List<string> SchoolYearSemesters = ve.Result as List<string>;

                foreach (string SchoolYearSemester in SchoolYearSemesters)
                {
                    cmbSchoolYear.Items.Add(SchoolYearSemester);
                }
            };

            worker.RunWorkerAsync();
        }
    }
}
