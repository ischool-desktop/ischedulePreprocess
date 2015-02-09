using System;
using Campus.Windows;
using FISCA.Presentation;

namespace Sunset
{
    /// <summary>
    /// 預開課程
    /// </summary>
    public partial class frmPreOpenCourse : FISCA.Presentation.Controls.BaseForm
    {
        string NextSchoolYear = string.Empty;
        string NextSemetser = string.Empty;

        /// <summary>
        /// 建構式
        /// </summary>
        public frmPreOpenCourse()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //課程規劃商業邏輯
            ProgramPlanBL ProgramPlanBL = new ProgramPlanBL(NextSchoolYear ,NextSemetser);

            Tuple<bool,string> Result = ProgramPlanBL.PreOpenCourse(K12.Presentation.NLDPanels.Class.SelectedSource);

            if (Result.Item1)
                MotherForm.SetStatusBarMessage(Result.Item2);
            else
                MsgBox.Show(Result.Item2);
        }

        /// <summary>
        /// 根據學期數字取得學期字串
        /// </summary>
        /// <param name="Semester"></param>
        /// <returns></returns>
        private string GetSemetserStr(string Semester)
        {
            if (Semester.Equals("1"))
                return "上";
            else if (Semester.Equals("2"))
                return "下";
            else
                return string.Empty;
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPreOpenCourse_Load(object sender, EventArgs e)
        {
            lblCurrentSchoolYearSemester.Text = "目前" + K12.Data.School.DefaultSchoolYear + "學年度" + GetSemetserStr(K12.Data.School.DefaultSemester) + "學期";
             
            //若為上學期，則開下學期課程
            if (K12.Data.School.DefaultSemester.Equals("1"))
            {
                NextSchoolYear = K12.Data.School.DefaultSchoolYear;
                NextSemetser = "2";
            }//若為下學期，則開上學期課程
            else
            {
                NextSchoolYear = ""+(K12.Data.Int.Parse(K12.Data.School.DefaultSchoolYear) +1);
                NextSemetser = "1"; 
            }

            //顯示預開課程學年度及學期
            lblPreOpenSchoolYearSemester.Text = "預開" + NextSchoolYear + "學年度" + GetSemetserStr(NextSemetser) + "學期";
        }
    }
}