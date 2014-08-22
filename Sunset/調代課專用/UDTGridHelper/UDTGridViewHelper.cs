using System;
using System.Collections.Generic;
using FISCA.UDT;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;

namespace Sunset
{
    public class UDTGridViewHelper<T> where T : ActiveRecord, new()
    {
        internal AccessHelper DataHelper { get; set; }

        private BindingList<T> Records { get; set; }

        private List<T> PaddingRemoves { get; set; }

        private List<string> Uniques { get; set; }

        private Dictionary<Control, Dictionary<string, string>> Bindings { get; set; }

        private AsyncHelper Async { get; set; }

        public UDTGridViewHelper(DataGridView dgv)
        {
            DataHelper = new AccessHelper();
            Async = new UDTGridViewHelper<T>.AsyncHelper(this);
            Uniques = new List<string>();
            Bindings = new Dictionary<Control, Dictionary<string, string>>();
            DataGridView = dgv;

            //關閉自動產生欄位功能。
            DataGridView.AutoGenerateColumns = false;

            dgv.UserDeletingRow += dgv_UserDeletingRow;
            dgv.SelectionChanged += dgv_SelectionChanged;
            dgv.CurrentCellDirtyStateChanged += dgv_CurrentCellDirtyStateChanged;
        }

        /// <summary>
        /// 授管理的 DataGridView。
        /// </summary>
        public DataGridView DataGridView { get; set; }

        /// <summary>
        /// 設定 Control 的 Binding。
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="controlPropertyName"></param>
        /// <param name="objMember"></param>
        public void AddBinding(Control ctl, string controlPropertyName, string objMember)
        {
            if (!Bindings.ContainsKey(ctl))
                Bindings.Add(ctl, new Dictionary<string, string>());

            Bindings[ctl].Add(controlPropertyName, objMember);
        }

        /// <summary>
        /// 設定 TextBox 的 Binding。
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="objMember"></param>
        public void AddBinding(TextBox ctl, string objMember)
        {
            AddBinding(ctl, "Text", objMember);
        }

        /// <summary>
        /// 將控制項的資料寫入到資料繫結物件，這可能會引發控制項的相關事件。
        /// </summary>
        public void CommitControlValues()
        {
            DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);

            foreach (Control ctl in Bindings.Keys)
                foreach (Binding binding in ctl.DataBindings)
                    binding.WriteValue();
        }

        /// <summary>
        /// 載入 UDT 資料到 DataGridView 上。
        /// </summary>
        public void LoadData()
        {
            LoadData(DataHelper.Select<T>());
        }

        /// <summary>
        /// 依條件查詢 UDT 資料。
        /// </summary>
        /// <param name="condition"></param>
        public void LoadData(string condition)
        {
            LoadData(DataHelper.Select<T>(condition));
        }

        public void LoadDataAsync(string condition)
        {
            Async.DoTask(condition);
        }

        /// <summary>
        /// 載入 UDT 資料到 DataGridView 上。
        /// </summary>
        public void LoadData(List<T> records)
        {
            List<T> filtered = new List<T>(records.TakeWhile(x => x.Deleted == false));

            //取消之前的事件訂閱。
            if (Records != null)
                Records.AddingNew -= Records_AddingNew;

            //取得所有資料，並使用 BindingList 保存。
            //BindingList 的泛型類別，必須有預設建構式，否則無法使用。
            Records = new BindingList<T>(filtered);
            PaddingRemoves = new List<T>();

            //處理當 GridView 需要新記錄時的事件，也可以選擇不處理。
            Records.AddingNew += Records_AddingNew;

            DataGridView.DataSource = Records;

            //設定所有 Control 的 DataBinding 屬性。
            SetupControlsBinding();
            Records.ResetBindings();

            IsDataUniqued();
        }

        private void SetupControlsBinding()
        {
            //先把所有 Binding 設定清除。
            foreach (Control ctl in Bindings.Keys)
                ctl.DataBindings.Clear();

            foreach (Control ctl in Bindings.Keys)
            {
                Dictionary<string, string> bindingSetup = Bindings[ctl];
                foreach (string property in bindingSetup.Keys)
                {
                    Binding binding = new Binding(property, Records, bindingSetup[property]);
                    ctl.DataBindings.Add(binding);
                }
            }
        }

        public void Reset()
        {
            Records = new BindingList<T>(new List<T>());
            PaddingRemoves = new List<T>();
            DataGridView.DataSource = Records;
            SetupControlsBinding();
            Records.ResetBindings();
        }

        /// <summary>
        /// 儲存 UDT 資料。
        /// </summary>
        public bool SaveData()
        {
            if (IsDataUniqued())
            {
                Records.SaveAll();
                PaddingRemoves.SaveAll();
                Reset();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 當要新增物件時。
        /// </summary>
        public event AddingNewEventHandler AddingNewObject;

        /// <summary>
        /// 設定唯一值欄位。
        /// </summary>
        /// <param name="fieldNames"></param>
        public void SetUniqueFields(string[] fieldNames)
        {
            Uniques = new List<string>();
            Uniques.AddRange(fieldNames);
        }

        /// <summary>
        /// 取得準備要更新的資料清單。
        /// </summary>
        /// <returns></returns>
        public List<T> GetChanges()
        {
            List<T> allRecord = new List<T>();
            DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if (Records != null)
                allRecord.AddRange(Records);

            if (PaddingRemoves != null)
                allRecord.AddRange(PaddingRemoves);
            return allRecord;
        }

        /// <summary>
        /// 取得已選擇的項目，如果沒有選擇項目則回傳 Null。
        /// </summary>
        public T CurrentBoundItem
        {
            get
            {
                if (DataGridView.CurrentCell == null)
                    return null;

                int rowIndex = DataGridView.CurrentCell.RowIndex;
                DataGridViewRow row = DataGridView.Rows[rowIndex];
                return row.DataBoundItem as T;
            }
        }

        private void Records_AddingNew(object sender, AddingNewEventArgs e)
        {
            AddingNewEventArgs arg = new AddingNewEventArgs();

            if (AddingNewObject != null)
                AddingNewObject(this, arg);

            e.NewObject = arg.NewObject;
        }

        private void dgv_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (UserDeleteingRow != null)
                UserDeleteingRow(this, e);

            if (!e.Cancel)
            {
                T m = e.Row.DataBoundItem as T;
                m.Deleted = true;
                PaddingRemoves.Add(m);
            }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            //如果是選擇整個 Row，就不要自動進入編輯模式。
            //if (DataGridView.SelectedRows.Count > 0)
            //    return;

            //沒有 CurrentCell 也不能進入編輯模式。
            //if (DataGridView.CurrentCell == null)
            //    return;

            //DataGridView.BeginEdit(true);
        }

        private void dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            foreach (Control ctl in Bindings.Keys)
            {
                foreach (Binding binding in ctl.DataBindings)
                    binding.ReadValue();
            }
        }

        public event EventHandler<DataGridViewRowCancelEventArgs> UserDeleteingRow;

        /// <summary>
        /// 新增一筆記錄。
        /// </summary>
        /// <returns></returns>
        public T AddNew()
        {
            return Records.AddNew();
        }

        /// <summary>
        /// 確認 DataGridView 上的資料是否有重複。
        /// </summary>
        /// <returns></returns>
        public bool IsDataUniqued()
        {
            if (Uniques.Count <= 0) return true;

            //資料重複時的錯誤訊息。
            const string ErrorMsg = "資料重複。";

            bool valided = true;

            Dictionary<string, DataGridViewRow> keys = new Dictionary<string, DataGridViewRow>();
            //DataGridViewRow
            foreach (DataGridViewRow row in DataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                row.ErrorText = string.Empty;

                T record = row.DataBoundItem as T;
                Type t = record.GetType();
                List<string> values = new List<string>();

                foreach (string property in Uniques)
                    values.Add(t.GetProperty(property).GetValue(record, null) + "");

                string key = string.Join(":", values.ToArray());

                if (keys.ContainsKey(key))
                {
                    keys[key].ErrorText = ErrorMsg;
                    row.ErrorText = ErrorMsg;
                    valided = false;
                }
                else
                    keys.Add(key, row);
            }
            return valided;
        }

        #region AsyncHelper 非同步載入資料。
        private class AsyncHelper
        {
            private BackgroundWorker Task = new BackgroundWorker();

            private bool TaskPadding = false;

            private string Condition { get; set; }

            private UDTGridViewHelper<T> Helper { get; set; }

            private List<T> Result { get; set; }

            public AsyncHelper(UDTGridViewHelper<T> helper)
            {
                Helper = helper;

                Task.DoWork += (Task_DoWork);
                Task.RunWorkerCompleted += (Task_RunWorkerCompleted);
            }

            private void Task_DoWork(object sender, DoWorkEventArgs e)
            {
                Result = Helper.DataHelper.Select<T>(e.Argument.ToString());
            }

            private void Task_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                if (TaskPadding)
                    Task.RunWorkerAsync(Condition);
                else
                {
                    if (e.Error != null)
                        MessageBox.Show(e.Error.Message);
                    else
                    {
                        Helper.LoadData(Result);
                        Helper.DataGridView.Enabled = true;
                    }
                }

                TaskPadding = false;
            }

            public void DoTask(string condition)
            {
                Helper.DataGridView.Enabled = false;
                Condition = condition;
                if (Task.IsBusy)
                    TaskPadding = true;
                else
                    Task.RunWorkerAsync(condition);
            }
        }
        #endregion
    }
}
