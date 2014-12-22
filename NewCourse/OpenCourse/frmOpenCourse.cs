using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FISCA.UDT;
using System.Text;
using FISCA.Presentation;
using FISCA.Presentation.Controls;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 依課程規劃開課
    /// </summary>
    public partial class frmOpenCourse : FISCA.Presentation.Controls.BaseForm
    {
        private const int iProgramPlan = 2;
        private AccessHelper mHelper = new AccessHelper();
        private List<ClassEx> mClasses = new List<ClassEx>();
        private List<SchedulerProgramPlan> mProgramPlans = new List<SchedulerProgramPlan>();

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public frmOpenCourse()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 取得學年度
        /// </summary>
        public int? SchoolYear { get { return K12.Data.Int.ParseAllowNull("" + cmbSchoolYear.SelectedItem); } }

        /// <summary>
        /// 取得學期
        /// </summary>
        public int? Semester { get { return K12.Data.Int.ParseAllowNull("" + cmbSemester.SelectedItem); } }

        public List<SchedulerProgramPlanClassRecord> ProgramPlanClasses
        {
            get
            {
                List<SchedulerProgramPlanClassRecord> Records = new List<SchedulerProgramPlanClassRecord>();

                foreach (DataGridViewRow Row in grdClassList.Rows)
                {
                    string ProgramPlanName = "" + Row.Cells["colProgramPlan"].Value;

                    if (!string.IsNullOrEmpty(ProgramPlanName))
                    {
                        SchedulerProgramPlanClassRecord Record = new SchedulerProgramPlanClassRecord();

                        Record.ClassName = "" + Row.Cells["colClassName"].Value;
                        Record.GradeYear = K12.Data.Int.Parse("" + Row.Cells["colGradeYear"].Value);

                        SchedulerProgramPlan ProgramPlanRecord = mProgramPlans.Find(x => x.Name.Equals(ProgramPlanName));

                        if (ProgramPlanRecord != null)
                        {
                            Record.ProgramPlan = ProgramPlanRecord;
                            Records.Add(Record);
                        }
                    }
                }

                return Records;
            }
        }


        /// <summary>
        /// 準備表單資料
        /// </summary>‧／
        private void Prepare()
        {
            int SchoolYear = K12.Data.Int.Parse(K12.Data.School.DefaultSchoolYear);
            int Semester = K12.Data.Int.Parse(K12.Data.School.DefaultSemester);
            //初始學年度
            cmbSchoolYear.Items.Add(SchoolYear - 2);
            cmbSchoolYear.Items.Add(SchoolYear - 1);
            cmbSchoolYear.Items.Add(SchoolYear);
            cmbSchoolYear.Items.Add(SchoolYear + 1);
            cmbSchoolYear.Items.Add(SchoolYear + 2);
            cmbSchoolYear.Text = "" + SchoolYear;

            //初始學期
            cmbSemester.Items.Add("1");
            cmbSemester.Items.Add("2");
            cmbSemester.Text = "" + Semester;

            //取得課程規劃表
            mProgramPlans = mHelper.Select<SchedulerProgramPlan>();

            //初始化班級清單
            mClasses = mHelper.Select<ClassEx>();

            mClasses = mClasses
                .FindAll(x => x.GradeYear != null);

            mClasses = mClasses
                .OrderBy(x => x.GradeYear)
                .ToList();

            foreach (ClassEx Class in mClasses)
            {
                int RowIndex = grdClassList.Rows.Add();

                grdClassList.Rows[RowIndex]
                    .SetValues(Class.ClassName, K12.Data.Int.GetString(Class.GradeYear));

                //DataGridViewComboBoxCell Cell = grdClassList.Rows[RowIndex].Cells[iProgramPlan] as DataGridViewComboBoxCell;
                //mProgramPlans.ForEach(x => Cell.Items.Add(x.Name));
            }

            menuProgramPlan.Items.Clear();

            menuProgramPlan.Items.Add("不指定");

            foreach (SchedulerProgramPlan ProgramPlan in mProgramPlans)
            {
                menuProgramPlan.Items.Add(ProgramPlan.Name);
                menuProgramPlan.ItemClicked += (sender, e) =>
                {
                    foreach (DataGridViewRow Row in grdClassList.SelectedRows)
                    {
                        if (e.ClickedItem.Text.Equals("不指定"))
                            Row.Cells[iProgramPlan].Value = string.Empty;
                        else
                            Row.Cells[iProgramPlan].Value = e.ClickedItem.Text;
                    }
                };
            }
        }

        /// <summary>
        /// 載入表單事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOpenCourse_Load(object sender, EventArgs e)
        {
            Prepare();
        }

        /// <summary>
        /// 確認開課
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            StringBuilder strBuilder = new StringBuilder();

            if (SchoolYear == null)
                strBuilder.AppendLine("學年度必須為數字！");

            if (Semester == null)
                strBuilder.AppendLine("學期必須為數字！");

            if (strBuilder.Length > 0)
            {
                MessageBox.Show(strBuilder.ToString());
                return;
            }

            //課程規劃商業邏輯
            string vSchoolYear = K12.Data.Int.GetString(SchoolYear);
            string vSemester = K12.Data.Int.GetString(Semester);

            SchedulerProgramPlanBL ProgramPlanBL = new SchedulerProgramPlanBL(vSchoolYear, vSemester);

            Tuple<bool, string> Result = ProgramPlanBL.OpenCourse(
                ProgramPlanClasses
                , chkCreateCourseSection.Checked);

            if (Result.Item1)
            {
                CourseEvents.RaiseChanged();

                if (chkCreateCourseSection.Checked)
                    CourseSectionEvents.RaiseChanged();

                MotherForm.SetStatusBarMessage(Result.Item2);
            }
            else
                MsgBox.Show(Result.Item2);

        }
    }
}
