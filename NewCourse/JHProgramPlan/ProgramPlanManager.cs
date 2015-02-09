using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Campus.Windows;
using DevComponents.DotNetBar;
using FISCA.DSAUtil;
using FISCA.UDT;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 國中課程規劃管理
    /// </summary>
    public partial class ProgramPlanManager : FISCA.Presentation.Controls.BaseForm
    {
        private BackgroundWorker _loader;
        private ButtonItem _selected_item;
        private List<SchedulerProgramPlan> mProgramPlans;
        private AccessHelper mHelper = new AccessHelper();

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ProgramPlanManager()
        {
            InitializeComponent();

            //ProgramPlan.Instance.ItemLoaded += delegate
            //{
            //    pictureBox1.Visible = false;
            //    btnAdd.Enabled = btnDelete.Enabled = true;
            //    LoadItems();
            //};
            //ProgramPlan.Instance.ItemUpdated += delegate(object sender, ItemUpdatedEventArgs e)
            //{
            //    if (e.PrimaryKeys.Count > 0)
            //        RefreshItemPanel(e.PrimaryKeys[0]);
            //};

            graduationPlanEditor1.IsDirtyChanged += (sender, e) => btnSave.Enabled = lblSaveWarning.Visible = graduationPlanEditor1.IsDirty;
            pictureBox1.Visible = true;
            graduationPlanEditor1.Enabled = false;
            lblName.Text =  "";

            _loader = new BackgroundWorker();
            _loader.DoWork += (sender,e)=> e.Result = mHelper.Select<SchedulerProgramPlan>();
            _loader.RunWorkerCompleted += (sender, e) =>
            {
                mProgramPlans = e.Result as List<SchedulerProgramPlan>;
                pictureBox1.Visible = false;
                btnAdd.Enabled = btnDelete.Enabled = true;
                LoadItems();
            };
            _loader.RunWorkerAsync();
        }

        /// <summary>
        /// 重新整理課程規劃項目
        /// </summary>
        /// <param name="id"></param>
        private void RefreshItemPanel(string id)
        {
            SchedulerProgramPlan record = mProgramPlans
                .Find(x => x.UID.Equals(id));
            ButtonItem updateItem = null;

            foreach (ButtonItem item in itemPanel1.Items)
            {
                SchedulerProgramPlan r = item.Tag as SchedulerProgramPlan;
                if (r.UID == id)
                {
                    updateItem = item;
                    break;
                }
            }

            if (record != null && updateItem == null) //Insert
            {
                ButtonItem item = new ButtonItem();
                item.Text = record.Name;
                item.Tag = record;
                item.OptionGroup = "ProgramPlan";
                item.Click += new EventHandler(item_Click);
                itemPanel1.Items.Add(item);
                item.RaiseClick();
            }
            else if (record != null && updateItem != null) //Update
            {
                updateItem.Tag = record;
                updateItem.RaiseClick();
            }
            else if (record == null && updateItem != null) //Delete
            {
                updateItem.Click -= new EventHandler(item_Click);
                itemPanel1.Items.Remove(updateItem);
                graduationPlanEditor1.Enabled = false;
                graduationPlanEditor1.SetSource(null);
                btnSave.Enabled = lblSaveWarning.Visible = false;
                lblName.Text = "";
            }

            itemPanel1.Refresh();
            itemPanel1.EnsureVisible(itemPanel1.Items[itemPanel1.Items.Count - 1]);
        }

        /// <summary>
        /// 設定項目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Click(object sender, EventArgs e)
        {
            ButtonItem item = sender as ButtonItem;
            graduationPlanEditor1.Enabled = true;
            lblName.Text = item.Text;
            btnSave.Enabled = lblSaveWarning.Visible = false;
            _selected_item = item;
            graduationPlanEditor1.SetSource(ConvertToXml(item.Tag as SchedulerProgramPlan));
            //SetListView(item.Tag as  SchedulerProgramPlan);
        }

        /// <summary>
        /// 載入項目
        /// </summary>
        private void LoadItems()
        {
            _selected_item = null;

            itemPanel1.SuspendLayout();
            itemPanel1.Items.Clear();

            List<ButtonItem> itemList = new List<ButtonItem>();

            foreach (var record in mProgramPlans)
            {
                ButtonItem item = new ButtonItem();
                item.Text = record.Name;
                item.Tag = record;
                item.OptionGroup = "ProgramPlan";
                item.Click += new EventHandler(item_Click);
                itemList.Add(item);
            }

            itemList.Sort(ItemComparer);
            foreach (var item in itemList)
                itemPanel1.Items.Add(item);

            itemPanel1.ResumeLayout();
            itemPanel1.Refresh();
        }

        private int ItemComparer(ButtonItem a, ButtonItem b)
        {
            return a.Text.CompareTo(b.Text);
        }

        private void SetListView(SchedulerProgramPlan record)
        {
            //listViewEx1.SuspendLayout();
            //listViewEx1.Items.Clear();
            //listViewEx1.Groups.Clear();

            //Dictionary<ClassRecord, int> classCount = new Dictionary<ClassRecord, int>();
            //List<StudentRecord> noClassStudents = new List<StudentRecord>();

            //foreach (StudentRecord stu in Student.Instance.Items)
            //{
            //    if (stu.GetProgramPlanRecord() == record)
            //    {
            //        if (stu.Class != null)
            //        {
            //            if (!classCount.ContainsKey(stu.Class))
            //                classCount.Add(stu.Class, 0);
            //            classCount[stu.Class]++;
            //        }
            //        else
            //            noClassStudents.Add(stu);
            //    }
            //}

            //foreach (ClassRecord cla in classCount.Keys)
            //{
            //    string groupKey;
            //    int a;
            //    if (int.TryParse(cla.GradeYear, out a))
            //    {
            //        groupKey = cla.GradeYear + "　年級";
            //    }
            //    else
            //        groupKey = cla.GradeYear;
            //    ListViewGroup group = listViewEx1.Groups[groupKey];
            //    if (group == null)
            //        group = listViewEx1.Groups.Add(groupKey, groupKey);
            //    listViewEx1.Items.Add(new ListViewItem(cla.Name + "(" + classCount[cla] + ")　", 0, group));
            //}
            //if (noClassStudents.Count > 0)
            //{
            //    ListViewGroup group = listViewEx1.Groups["未分班"];
            //    if (group == null)
            //        group = listViewEx1.Groups.Add("未分班", "未分班");
            //    foreach (StudentRecord stu in noClassStudents)
            //    {
            //        listViewEx1.Items.Add(new ListViewItem(stu.Name + "[" + stu.StudentNumber + "] 　", 1, group));
            //    }
            //}

            //listViewEx1.ResumeLayout();
        }

        private XmlElement ConvertToXml(SchedulerProgramPlan record)
        {
            DSXmlHelper helper = new DSXmlHelper("GraduationPlan");

            foreach (var subject in record.Subjects)
            {
                XmlElement element = null;

                element = helper.AddElement("Subject");
                element.SetAttribute("GradeYear", ""+subject.GradeYear);
                element.SetAttribute("Semester", ""+subject.Semester);
                element.SetAttribute("Credit", K12.Data.Decimal.GetString(subject.Credit));
                element.SetAttribute("Period", K12.Data.Decimal.GetString(subject.Period));
                element.SetAttribute("Domain", subject.Domain);
                element.SetAttribute("FullName", subject.FullName);
                element.SetAttribute("Level", K12.Data.Int.GetString(subject.Level));
                element.SetAttribute("CalcFlag", "" + subject.CalcFlag);
                element.SetAttribute("SubjectName", subject.SubjectName);

                element = helper.AddElement("Subject", "Grouping");
                element.SetAttribute("RowIndex", "" + subject.RowIndex);
            }

            return helper.BaseElement;

            //<Subject Credit="2" Domain="外國語文" FullName="ESL" Level="" NotIncludedInCalc="False" SubjectName="ESL">
            //    <Grouping RowIndex="1" />
            //</Subject>
        }

        /// <summary>
        /// 刪除課程規劃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selected_item == null) return;

            if (MsgBox.Show("您確定要刪除 '" + _selected_item.Text + "' 嗎？", "刪除課程規劃表", MessageBoxButtons.YesNo) == DialogResult.No) return;

            SchedulerProgramPlan editor = (_selected_item.Tag as SchedulerProgramPlan);
            editor.Deleted = true;
            mProgramPlans.SaveAll();

            _loader.RunWorkerAsync();
        }

        /// <summary>
        /// 新增課程規劃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            new GraduationPlanCreator().ShowDialog();
            _loader.RunWorkerAsync();
        }

        /// <summary>
        /// 儲存課程規劃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_selected_item == null) return;
            if (graduationPlanEditor1.IsValidated == false)
            {
                MsgBox.Show("課程規劃表內容有錯誤，請先修正後再儲存。");
                return;
            }

            SchedulerProgramPlan editor = (_selected_item.Tag as SchedulerProgramPlan);
            editor.Subjects = GetSubjectsFromXml(graduationPlanEditor1.GetSource());
            editor.Save();

            _selected_item.Tag = mProgramPlans
                .Find(x => x.UID.Equals(editor.UID));

            graduationPlanEditor1.SetSource(ConvertToXml(_selected_item.Tag as SchedulerProgramPlan));

            btnSave.Enabled = lblSaveWarning.Visible = false;
        }

        private List<ProgramSubject> GetSubjectsFromXml(XmlElement source)
        {
            List<ProgramSubject> list = new List<ProgramSubject>();

            foreach (XmlElement element in source.SelectNodes("Subject"))
                list.Add(new ProgramSubject(element));

            return list;
        }

        private void copyFromIschool_Click(object sender, EventArgs e)
        {
            new frmCopyProgramPlan().ShowDialog();
            _loader.RunWorkerAsync();
        }
    }
}
