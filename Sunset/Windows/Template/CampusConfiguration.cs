using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FISCA.Presentation.Controls;
using System.ComponentModel;

namespace Sunset.Windows
{
    /// <summary>
    /// 通用型的新增、更新、刪除及匯入匯出表單
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class CampusConfiguration<T> : ConfigurationItem
    {
        private IConfigurationDataAccess<T> mDataAccess;
        private IContentEditor<T> mEditor;
        private string mCurrentSelectedName;
        private int mCurrentSelectedRow;
        private bool mIsSimpleSwitch = false;

        /// <summary>
        /// 建構式，傳入DataAccess物件
        /// </summary>
        /// <param name="vEditor"></param>
        /// <param name="vDataAccess"></param>
        public CampusConfiguration(IConfigurationDataAccess<T> vDataAccess, IContentEditor<T> vEditor)
        {
            InitializeComponent();

            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            mDataAccess = vDataAccess;
            mEditor = vEditor;
            this.pnlExpandable.TitleText = mDataAccess.DisplayName;
            this.Caption = mDataAccess.DisplayName;
            this.pnlEditor.Controls.Add(mEditor.Control);
            mEditor.Control.Dock = DockStyle.Fill;

            if (mDataAccess.ExtraCommands != null)
                mDataAccess.ExtraCommands.ForEach
                (x =>
                    {
                        ButtonItem ButtonItem = new ButtonItem(x.Name, x.Text);
                        ButtonItem.Click += (sender, e) =>
                        {
                            x.Execute(GetSelectedName());
                            if (x.IsChangeData)
                            {
                                mEditor.Prepare();
                                Refill();
                            }
                        };
                        btnMore.SubItems.Add(ButtonItem);
                    }
                );

            //btnMore.Click += (sender, e) =>
            //{
            //    string SelectedName = GetSelectedName();

            //    mDataAccess.Export(SelectedName);
            //};

            //btnImport.Click += (sender, e) =>
            //{
            //    mDataAccess.Import();
            //    mEditor.Prepare();
            //    Refill();
            //};
        }

        /// <summary>
        /// 取得選取名稱
        /// </summary>
        /// <returns>選擇名稱</returns>
        private string GetSelectedName()
        {
            string SelectedName = string.Empty;
            if (grdNameList.SelectedRows.Count == 1)
            {
                DataGridViewRow Row = grdNameList.SelectedRows[0];

                if (Row != null)
                {
                    DataGridViewCell Cell = Row.Cells[0];

                    if (Cell != null)
                        SelectedName = "" + Cell.Value;
                }
            }

            return SelectedName;
        }

        private void SetSelectedName(string Name)
        {
            if (grdNameList.SelectedRows.Count == 1)
            {
                DataGridViewRow Row = grdNameList.SelectedRows[0];

                if (Row != null)
                {
                    DataGridViewCell Cell = Row.Cells[0];

                    Cell.Value = Name;
                }
            }
        }

        /// <summary>
        /// 重新取得資料項目名稱
        /// </summary>
        private void Refill()
        {
            string SelectedName = GetSelectedName();

            grdNameList.Rows.Clear();

            BackgroundWorker BGW = new BackgroundWorker();
            BGW.WorkerReportsProgress = true;

            List<string> Names = new List<string>();

            BGW.DoWork += delegate
            {
                Names = mDataAccess.SelectKeys();
                Names.Sort();
            };

            BGW.RunWorkerCompleted += delegate
            {
                foreach (string Name in Names)
                {
                    if (Name.Equals(SelectedName))
                        grdNameList.Rows[grdNameList.Rows.Add(Name)].Selected = true;
                    else
                        grdNameList.Rows.Add(Name);
                }
                FISCA.Presentation.MotherForm.SetStatusBarMessage("完成!!");
            };

            BGW.RunWorkerAsync();

            FISCA.Presentation.MotherForm.SetStatusBarMessage("取得時間表清單...");
        }

        /// <summary>
        /// 重新取得資料項目名稱
        /// </summary>
        private void Search()
        {
            //當內容有改變時告知使用者
            if (this.mEditor.IsDirty)
            {
                MessageBox.Show("您未儲存目前資料，請先儲存後再進行搜尋。");
                return;
            }

            string SelectedName = GetSelectedName();

            grdNameList.Rows.Clear();

            List<string> Names = mDataAccess.Search(txtSearch.Text);

            Names.Sort();

            foreach (string Name in Names)
            {
                int RowIndex = grdNameList.Rows.Add();

                grdNameList.Rows[RowIndex].SetValues(Name);

                if (Name.Equals(SelectedName))
                    grdNameList.Rows[RowIndex].Selected = true;
            }
        }

        /// <summary>
        /// 當被呼叫時填入時
        /// </summary>
        protected override void OnActive()
        {
            Refill();
        }

        /// <summary>
        /// 新增項目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e)
        {
            DialogResult Result;

            if (mDataAccess.DisplayName.Equals("教師管理"))
            {
                TeacherCreatorForm vCreatorForm = new TeacherCreatorForm(mDataAccess as TeacherPackageDataAccess);

                Result = vCreatorForm.ShowDialog();
            }
            else if (mDataAccess.DisplayName.Equals("班級管理"))
            {
                ClassCreatorForm vCreatorForm = new ClassCreatorForm(mDataAccess as ClassPackageDataAccess);

                Result = vCreatorForm.ShowDialog();
            }
            else
            {
                NameCreatorForm<T> vCreatorForm = new NameCreatorForm<T>(mDataAccess);

                Result = vCreatorForm.ShowDialog();
            }

            if (Result == DialogResult.OK)
            {
                //string NewName = vCreatorForm.NewName;
                //string DuplicateName = vCreatorForm.DuplicateName;

                //if (!string.IsNullOrEmpty(NewName))
                //{
                //    mDataAccess.Insert(NewName, DuplicateName);
                Refill();
                //}
            }
        }

        /// <summary>
        /// 刪除項目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SelectedName = GetSelectedName();

            if (!string.IsNullOrEmpty(SelectedName))
            {
                if (MsgBox.Show("確定要刪除 '" + SelectedName + "' ？", "確定", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    mDataAccess.Delete(SelectedName);

                    Refill();
                }
            }
        }

        /// <summary>
        /// 更新項目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (grdNameList.SelectedRows.Count == 1)
            {
                if (mEditor.Validate())
                {
                    T UpdateRecord = mEditor.Content;

                    mDataAccess.Save(UpdateRecord);

                    mEditor.IsDirty = false;

                    Refill();
                }
                else
                {
                    MsgBox.Show("輸入資料有誤，無法儲存。\n請檢查輸入資料。");
                }
            }
        }

        /// <summary>
        /// 當選取項目改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdNameList_SelectionChanged(object sender, EventArgs e)
        {
            if (mIsSimpleSwitch)
            {
                mIsSimpleSwitch = false;
                return;
            }

            UserControl vControl = this.mEditor.Control;
            vControl.Visible = (grdNameList.SelectedRows.Count == 1);
            SelectedName = GetSelectedName();

            //當內容有改變時告知使用者
            if (this.mEditor.IsDirty)
            {
                DialogResult Result = MessageBox.Show("您未儲存目前資料，是否要儲存？", "排課", MessageBoxButtons.YesNoCancel);

                if (Result == DialogResult.Yes)
                {
                    #region 儲存後再切換
                    if (mEditor.Validate())
                    {
                        T DirtyRecord = mEditor.Content;

                        mDataAccess.Save(DirtyRecord);
                    }
                    else
                    {
                        MsgBox.Show("輸入資料有誤，無法儲存。\n請檢查輸入資料。");
                        mIsSimpleSwitch = false;
                        grdNameList.Rows[mCurrentSelectedRow].Selected = true;
                    }

                    if (!string.IsNullOrEmpty(SelectedName))
                    {
                        T SelectRecord = mDataAccess.Select(SelectedName);

                        mEditor.Content = SelectRecord;

                        mCurrentSelectedName = SelectedName;
                    }
                    #endregion
                }
                else if (Result == DialogResult.No)
                {
                    #region 直接切換
                    if (!string.IsNullOrEmpty(SelectedName))
                    {
                        T SelectRecord = mDataAccess.Select(SelectedName);

                        mEditor.Content = SelectRecord;

                        mCurrentSelectedName = SelectedName;
                    }
                    #endregion
                }
                else if (Result == DialogResult.Cancel)
                {
                    //回到選取的資料列
                    mIsSimpleSwitch = true;
                    if (grdNameList.Rows.Count > 0)
                        grdNameList.Rows[mCurrentSelectedRow].Selected = true;
                }
            }
            //正常切換模式
            else if (!string.IsNullOrEmpty(SelectedName))
            {
                if (BGW.IsBusy)
                {
                    IsBusy = true;
                }
                else
                {
                    mEditor.SetTitle("(資料取得中...)");
                    BGW.RunWorkerAsync();
                }
            }
        }

        BackgroundWorker BGW = new BackgroundWorker();
        bool IsBusy = false;
        string SelectedName;

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            T SelectRecord = mDataAccess.Select(SelectedName);
            e.Result = SelectRecord;
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsBusy)
            {
                IsBusy = false;
                BGW.RunWorkerAsync();
                return;
            }

            T SelectRecord = (T)e.Result;
            mEditor.Content = SelectRecord;
            mCurrentSelectedName = SelectedName;
            mCurrentSelectedRow = grdNameList.SelectedRows[0].Index;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void grdNameList_DoubleClick(object sender, EventArgs e)
        {
            string SelectedName = GetSelectedName();

            if (!string.IsNullOrEmpty(SelectedName))
            {
                frmInputBox InputBox = new frmInputBox("修改名稱", SelectedName);

                if (InputBox.ShowDialog() == DialogResult.OK)
                {
                    string NewName = InputBox.Message;

                    if (string.IsNullOrEmpty(NewName))
                    {
                        MessageBox.Show("名稱不得為空白！");
                        return;
                    }

                    string Result = mDataAccess.Update(SelectedName, NewName);

                    if (string.IsNullOrEmpty(Result))
                        SetSelectedName(NewName);
                    else
                        MsgBox.Show(Result);
                }
            }
        }
    }
}