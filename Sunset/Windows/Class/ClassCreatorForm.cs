using System;
using System.Collections.Generic;
using System.Data;
using FISCA.Data;
using FISCA.Presentation.Controls;

namespace Sunset.Windows
{
    /// <summary>
    /// 根據名稱產生新記錄的表單
    /// </summary>
    public partial class ClassCreatorForm : FISCA.Presentation.Controls.BaseForm
    {
        private QueryHelper mHelper = new QueryHelper();
        private string mTypeName = string.Empty;
        private ClassPackageDataAccess mDataAccess;
        private List<string> mNames;

        /// <summary>
        /// 建構式，傳入NameCreatorForm所需要的資料存取物件
        /// </summary>
        /// <param name="vDataAccess">資料存取物件</param>
        public ClassCreatorForm(ClassPackageDataAccess vDataAccess)
        {
            InitializeComponent();

            mDataAccess = vDataAccess;
            mNames = mDataAccess.SelectKeys(); //取得現有名稱列表
            mTypeName = vDataAccess.DisplayName; //取得資料型態名稱

            this.Text = "新增" + mTypeName;

            cmbNames.SelectedItem = cmbItemDefault;

            mNames.Sort();
            mNames.ForEach(x => cmbNames.Items.Add(x)); //將名稱加入到ComboBox當中

            DataTable table = mHelper.Select("select class_name from class");

            foreach (DataRow row in table.Rows)
            {
                string ClassName = row.Field<string>("class_name");
                cmbClassName.Items.Add(ClassName);
            }

            cmbClassName.Focus();
        }

        /// <summary>
        /// 新的名稱
        /// </summary>
        public string NewName 
        {
            get 
            {
                return cmbClassName.Text;
            }
        }

        /// <summary>
        /// 要複製的名稱
        /// </summary>
        public string DuplicateName { get { return "" + cmbNames.SelectedItem; } }

        /// <summary>
        /// 當選擇名稱改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            //判斷名稱是否重覆
            if (mNames.Contains(NewName))
            {
                errorNameDuplicate.SetError(cmbClassName, "名稱不可重複。");
                errorNameDuplicate.SetError(txtGradeYear, "名稱不可重複。");
            }
            else
                errorNameDuplicate.Clear();
        }

        /// <summary>
        /// 按下關閉按鈕時的動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 當按下儲存按鈕時所觸發的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NewName))
            {
                if (!string.IsNullOrEmpty(errorGradeYear.GetError(txtGradeYear))
                    || string.IsNullOrEmpty(txtGradeYear.Text))
                {
                    MsgBox.Show("年級必須為數字！");
                    return;
                }

                if (mNames.Contains(NewName))
                {
                    MsgBox.Show("名稱不可重複。");
                    return;
                }

                try
                {
                    mDataAccess.Insert(NewName+","+txtGradeYear.Text, "" + cmbNames.SelectedItem);
                }
                catch(Exception ex)
                {
                    SmartSchool.ErrorReporting.ReportingService.ReportException(ex);
                    MsgBox.Show("新增" + mTypeName + "時發生未預期之錯誤。\n系統已回報此錯誤內容。");
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MsgBox.Show("必需輸入" + mTypeName + "名稱。");
            }
        }

        /// <summary>
        /// 當選擇項目改變時的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNames_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TeacherNameCreatorForm_Load(object sender, EventArgs e)
        {

        }

        private void txtGradeYear_TextChanged(object sender, EventArgs e)
        {
            int GradeYear;

            if (!int.TryParse(txtGradeYear.Text, out GradeYear))
                errorGradeYear.SetError(txtGradeYear, "年級必須為數字！");
            else
                errorGradeYear.Clear();
        }
    }
}