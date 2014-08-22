//using System;
//using System.Windows.Forms;
//using FISCA.Presentation.Controls;

//namespace Sunset
//{
//    /// <summary>
//    /// 學年度學期及日期對應表單
//    /// </summary>
//    public partial class frmSchoolYearSemesterDate : BaseForm
//    {
//        private UDTGridViewHelper<SchoolYearSemesterDate> GridViewHelper = null;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        public frmSchoolYearSemesterDate()
//        {
//            InitializeComponent();
//        }

//        private void UDTDataGridView_Load(object sender, EventArgs e)
//        {
//            //寫在 Form_Load 事件中請一定要處理 Exception。
//            try
//            {
//                //指定 Helper 所要管理的 DataGridView 控制項。
//                GridViewHelper = new UDTGridViewHelper<SchoolYearSemesterDate>(dgvMultiValues);

//                //設定不允許重複的欄位清單。
//                GridViewHelper.SetUniqueFields(new string[] { "SchoolYear","Semester" });

//                //GridViewHelper.AddBinding(textBoxX1, "Name");

//                //呼叫 Helper 載入資料，此方法是同步執行。
//                GridViewHelper.LoadData();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        private void btnSave_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                //呼叫儲存資料，並檢查回傳值是否儲存成功。
//                if (GridViewHelper.SaveData())
//                    Close(); //儲存成功關閉畫面。
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        private void btnCancel_Click(object sender, EventArgs e)
//        {
//            Close();
//        }

//        private void dgvMultiValues_SelectionChanged(object sender, EventArgs e)
//        {

//        }

//        private void btnNew_Click(object sender, EventArgs e)
//        {
//            //GridViewHelper.AddNew()..Code = "new code";
//        }
//    }
//}