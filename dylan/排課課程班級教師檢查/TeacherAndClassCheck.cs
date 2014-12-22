using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data;
using FISCA.Data;

namespace Sunset.NewCourse
{
    public partial class TeacherAndClassCheck : BaseForm
    {
        BackgroundWorker Select_BGW = new BackgroundWorker();
        //BackgroundWorker Load_BGW = new BackgroundWorker();
        Dictionary<string, List<SchedulerCourseSection>> SchedulerDic = new Dictionary<string, List<SchedulerCourseSection>>();

        Dictionary<DataGridViewRow, bool> RowErrorDic = new Dictionary<DataGridViewRow, bool>();

        //string _SchoolYear { get; set; }
        //List<int> SchoolList { get; set; }
        //string _Semester { get; set; }
        //List<string> Semester { get; set; }

        TACC _tacc { get; set; }

        List<string> _CourseIDList { get; set; }

        /// <summary>
        /// 檢查班級與教師
        /// </summary>
        public TeacherAndClassCheck(List<string> CourseIDList)
        {
            InitializeComponent();

            _CourseIDList = CourseIDList;
        }

        private void TeacherAndClassCheck_Load(object sender, EventArgs e)
        {
            CourseAdmin.Instance.TempSourceChanged += new EventHandler(Instance_TempSourceChanged);
            labelX2.Text = "目前待處理課程：" + CourseAdmin.Instance.TempSource.Count;

            //Load_BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Load_BGW_RunWorkerCompleted);
            //Load_BGW.DoWork += new DoWorkEventHandler(Load_BGW_DoWork);

            this.Text = "班級教師檢查(資料取得中...)";
            //Load_BGW.RunWorkerAsync();

            Select_BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Select_BGW_RunWorkerCompleted);
            Select_BGW.DoWork += new DoWorkEventHandler(Select_BGW_DoWork);
            Select_BGW.RunWorkerAsync();
            //取得課程
        }

        void Load_BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            //DataTable dt = tool._Q.Select("select school_year,semester from $scheduler.scheduler_course_extension group by school_year,semester");
            //SchoolList = new List<int>();
            //Semester = new List<string>();

            //foreach (DataRow row in dt.Rows)
            //{
            //    int school1 = int.Parse("" + row[0]);
            //    string semester1 = "" + row[1];

            //    if (!SchoolList.Contains(school1))
            //        SchoolList.Add(school1);

            //    if (!Semester.Contains(semester1))
            //        Semester.Add(semester1);
            //}
            //SchoolList.Sort();
            //Semester.Sort();

        }

        void Load_BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //foreach (int s in SchoolList)
            //    comboBoxEx1.Items.Add(s.ToString());

            //comboBoxEx2.Items.AddRange(Semester.ToArray());

            //this.Text = "班級教師檢查";
            //buttonX1.Enabled = true;
            //buttonX1.Pulse(5);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(comboBoxEx1.Text) && !string.IsNullOrEmpty(comboBoxEx2.Text))
            //{
            //    if (!Select_BGW.IsBusy)
            //    {
            //        _SchoolYear = "" + comboBoxEx1.Text;
            //        _Semester = "" + comboBoxEx2.Text;

            //        buttonX1.Enabled = false;
            //        this.Text = "班級教師檢查(資料取得中...)";
            //        Select_BGW.RunWorkerAsync();
            //    }
            //    else
            //    {
            //        MsgBox.Show("系統忙碌中...");
            //    }
            //}
            //else
            //{
            //    MsgBox.Show("無法查詢資料...");
            //}
        }

        void Select_BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            _tacc = new TACC();

            //取得本學期所有課程物件
            string CourseString = string.Join("','", _CourseIDList);
            _tacc.SchList = tool._A.Select<SchedulerCourseExtension>(string.Format("uid in ('{0}')", CourseString));
            _tacc.SchList.Sort(_tacc.SortCourseName);
            foreach (SchedulerCourseExtension each in _tacc.SchList)
            {
                if (!_tacc.CourseList.Contains(each.UID))
                {
                    _tacc.CourseList.Add(each.UID);
                }
            }

            #region 取得班級物件: id : Name

            _tacc._ClassNameDic.Clear();
            DataTable dt1 = tool._Q.Select("select id,class_name from class order by grade_year,display_order,class_name");
            foreach (DataRow row in dt1.Rows)
            {
                string id = "" + row["id"];
                string class_name = "" + row["class_name"];

                if (!_tacc._ClassNameDic.ContainsKey(class_name))
                    _tacc._ClassNameDic.Add(class_name, id);

                if (!_tacc._ClassIDDic.ContainsKey(id))
                    _tacc._ClassIDDic.Add(id, class_name);
            }

            #endregion

            #region 取得教師物件: id : TeacherName : NickName

            DataTable dt2 = tool._Q.Select("select id,teacher_name,nickname from teacher where status=1 order by teacher_name");
            foreach (DataRow row in dt2.Rows)
            {
                string id = "" + row["id"];
                string teacher_name = "" + row["teacher_name"];
                string nickname = "" + row["nickname"];

                if (!string.IsNullOrEmpty(nickname))
                    teacher_name = teacher_name + "(" + nickname + ")";

                if (!_tacc._TeacherNameDic.ContainsKey(teacher_name))
                    _tacc._TeacherNameDic.Add(teacher_name, id);

                if (!_tacc._TeacherIDDic.ContainsKey(id))
                    _tacc._TeacherIDDic.Add(id, teacher_name);
            }

            #endregion

            #region 取得課程分段: 用來填入教師ID使用
            //課程分段的部份,需要詢問使用者
            //是否要同步修改課程分段上的授課教師
            //與(開放/不開放)查詢,功能相同

            if (_tacc.CourseList.Count != 0)
            {
                string qu = string.Join("','", _tacc.CourseList);
                //依課程系統編號取得課程分段
                SchedulerDic.Clear();
                List<SchedulerCourseSection> mCourseSections = tool._A.Select<SchedulerCourseSection>(string.Format("ref_course_id in ('{0}')", qu));
                foreach (SchedulerCourseSection each in mCourseSections)
                {
                    if (!SchedulerDic.ContainsKey(each.CourseID.ToString()))
                    {
                        SchedulerDic.Add(each.CourseID.ToString(), new List<SchedulerCourseSection>());
                    }
                    SchedulerDic[each.CourseID.ToString()].Add(each);
                }
            }


            #endregion
        }

        void Select_BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = "班級教師檢查";
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    dataGridViewX1.Rows.Clear();
                    RowErrorDic.Clear();
                    foreach (SchedulerCourseExtension each in _tacc.SchList)
                    {
                        List<SchedulerCourseSection> SectionList = new List<SchedulerCourseSection>();
                        if (SchedulerDic.ContainsKey(each.UID))
                        {
                            SectionList = SchedulerDic[each.UID];
                        }

                        DataGridViewRow row = new DataGridViewRow();

                        //如何算是錯誤資料
                        bool rowErrorClass = true;

                        row.CreateCells(dataGridViewX1);
                        row.Tag = each;
                        row.Cells[colSchoolYear.Index].Value = each.SchoolYear.ToString(); //學年度
                        row.Cells[colSemester.Index].Value = each.Semester; //學期
                        row.Cells[colCourseName.Index].Value = each.CourseName; //課程名稱

                        #region 班級名稱

                        if (!string.IsNullOrEmpty(each.ClassName))
                        {
                            if (_tacc._ClassNameDic.ContainsKey(each.ClassName))
                            {
                                if (each.ClassID.HasValue)
                                {
                                    if (_tacc._ClassIDDic.ContainsKey(each.ClassID.Value.ToString()))
                                    {
                                        if (_tacc._ClassIDDic[each.ClassID.Value.ToString()] != each.ClassName)
                                        {
                                            //查無班級
                                            rowErrorClass = false;
                                            row.Cells[colClass.Index].Value = each.ClassName + "(班級名稱與班級ID不符)";
                                            SetColor(row.Cells[colClass.Index], false);
                                        }
                                        else
                                        {
                                            //班級名稱
                                            rowErrorClass = true;
                                            row.Cells[colClass.Index].Value = each.ClassName;
                                            SetColor(row.Cells[colClass.Index], true);
                                        }
                                    }
                                    else
                                    {
                                        //查無班級
                                        rowErrorClass = false;
                                        row.Cells[colClass.Index].Value = each.ClassName + "(班級ID查無ischool班級)";
                                        SetColor(row.Cells[colClass.Index], false);
                                    }
                                }
                                else
                                {
                                    //班級名稱
                                    rowErrorClass = true;
                                    row.Cells[colClass.Index].Value = each.ClassName;
                                    SetColor(row.Cells[colClass.Index], true);
                                }
                            }
                            else
                            {
                                //查無班級
                                rowErrorClass = false;
                                row.Cells[colClass.Index].Value = each.ClassName + "(查無班級)";
                                SetColor(row.Cells[colClass.Index], false);
                            }
                        }
                        else
                        {
                            //沒有班級也算錯誤資料
                            rowErrorClass = false;
                        }

                        #endregion

                        #region 教師名稱

                        bool rowError1 = TeacherName(row, each.TeacherName1, colTeacher1.Index);
                        bool rowError2 = TeacherName(row, each.TeacherName2, colTeacher2.Index);
                        bool rowError3 = TeacherName(row, each.TeacherName3, colTeacher3.Index);
                        bool rowErrorTeacherName1 = rowError1 & rowError2 & rowError3; //其中有一個錯誤就為錯誤資料

                        #endregion

                        //檢查課程分段
                        bool rowError4 = CheckSchedulerList(row, SectionList, colTeacher1.Index);
                        bool rowError5 = CheckSchedulerList(row, SectionList, colTeacher2.Index);
                        bool rowError6 = CheckSchedulerList(row, SectionList, colTeacher3.Index);
                        bool rowErrorTeacherID = rowError4 & rowError5 & rowError6; //其中有一個錯誤就為錯誤資料

                        dataGridViewX1.Rows.Add(row);

                        bool allCheck = rowErrorClass & rowErrorTeacherName1 & rowErrorTeacherID; //其中有一個錯誤就為錯誤資料
                        //看資料是否正確
                        RowErrorDic.Add(row, allCheck);

                        if (!_tacc.SchRowIndexDic.ContainsKey(each.UID))
                            _tacc.SchRowIndexDic.Add(each.UID, row);
                    }

                    foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
                    {
                        row.Selected = false;
                    }
                    ifCheckBox();
                }
                else
                {
                    MsgBox.Show("背景作業發生錯誤:\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("背景作業已中止...");
            }
        }

        /// <summary>
        /// 檢查課程分段
        /// </summary>
        private bool CheckSchedulerList(DataGridViewRow row, List<SchedulerCourseSection> SectionList, int index)
        {
            #region 教師1
            bool t1 = true;
            int section = 1;
            foreach (SchedulerCourseSection each in SectionList)
            {
                if (index == colTeacher1.Index)
                {
                    if (!string.IsNullOrEmpty(each.TeacherName1))
                    {
                        if (!_tacc._TeacherNameDic.ContainsKey(each.TeacherName1))
                        {
                            row.Cells[index].ErrorText = "(課程分段" + section + ")ischool系統內查無教師1名稱";
                            t1 = false;
                        }

                        //沒有值
                        if (each.TeacherID1.HasValue)
                        {
                            //教師ID是否存在ischool系統
                            if (!_tacc._TeacherIDDic.ContainsKey(each.TeacherID1.Value.ToString()))
                            {
                                //不存在
                                row.Cells[index].ErrorText += "(課程分段" + section + ")教師ID不存在ischool系統";
                                t1 = false;
                            }
                            else
                            {
                                //存在系統...
                                if (_tacc._TeacherIDDic[each.TeacherID1.Value.ToString()] != each.TeacherName1)
                                {
                                    //教師ID與教師姓名不符
                                    row.Cells[index].ErrorText += "(課程分段" + section + ")教師ID與教師姓名不同步";
                                    t1 = false;
                                }
                            }
                        }

                    }
                }
                else if (index == colTeacher2.Index)
                {
                    if (!string.IsNullOrEmpty(each.TeacherName2))
                    {
                        if (!_tacc._TeacherNameDic.ContainsKey(each.TeacherName2))
                        {
                            row.Cells[index].ErrorText = "(課程分段" + section + ")ischool系統內查無教師2名稱";
                            t1 = false;
                        }

                        //沒有值
                        if (each.TeacherID2.HasValue)
                        {
                            //教師ID是否存在ischool系統
                            if (!_tacc._TeacherIDDic.ContainsKey(each.TeacherID2.Value.ToString()))
                            {
                                //不存在
                                row.Cells[index].ErrorText += "(課程分段" + section + ")教師ID不存在ischool系統";
                                t1 = false;
                            }
                            else
                            {
                                //存在系統...
                                if (_tacc._TeacherIDDic[each.TeacherID2.Value.ToString()] != each.TeacherName2)
                                {
                                    //教師ID與教師姓名不符
                                    row.Cells[index].ErrorText += "(課程分段" + section + ")教師ID與教師姓名不同步";
                                    t1 = false;
                                }
                            }
                        }
                    }
                }
                else if (index == colTeacher3.Index)
                {
                    if (!string.IsNullOrEmpty(each.TeacherName3))
                    {
                        if (!_tacc._TeacherNameDic.ContainsKey(each.TeacherName3))
                        {
                            row.Cells[index].ErrorText = "(課程分段" + section + ")ischool系統內查無教師3名稱";
                            t1 = false;
                        }

                        //沒有值
                        if (each.TeacherID3.HasValue)
                        {
                            //教師ID是否存在ischool系統
                            if (!_tacc._TeacherIDDic.ContainsKey(each.TeacherID3.Value.ToString()))
                            {
                                //不存在
                                row.Cells[index].ErrorText += "(課程分段" + section + ")教師ID不存在ischool系統";
                                t1 = false;
                            }
                            else
                            {
                                //存在系統...
                                if (_tacc._TeacherIDDic[each.TeacherID3.Value.ToString()] != each.TeacherName3)
                                {
                                    //教師ID與教師姓名不符
                                    row.Cells[index].ErrorText += "(課程分段" + section + ")教師ID與教師姓名不同步";
                                    t1 = false;
                                }
                            }
                        }
                    }
                }

                section++;
            }
            if (t1)
            {
                row.Cells[index].ErrorText = "";
            }

            return t1;
            #endregion
        }

        /// <summary>
        /// 檢查教師名稱
        /// </summary>
        private bool TeacherName(DataGridViewRow row, string TeacehrName, int CellIndex)
        {
            if (!string.IsNullOrEmpty(TeacehrName))
            {
                if (_tacc._TeacherNameDic.ContainsKey(TeacehrName))
                {
                    //教師1名稱
                    row.Cells[CellIndex].Value = TeacehrName;
                    SetColor(row.Cells[CellIndex], true);
                    return true;
                }
                else
                {
                    //查無教師1
                    row.Cells[CellIndex].Value = TeacehrName + "(查無教師)";
                    SetColor(row.Cells[CellIndex], false);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void SetColor(DataGridViewCell dataGridViewCell, bool p)
        {
            if (p)
            {
                dataGridViewCell.Style.BackColor = Color.White;
                dataGridViewCell.Style.ForeColor = Color.Black;
            }
            else
            {
                dataGridViewCell.Style.BackColor = Color.Red;
                dataGridViewCell.Style.ForeColor = Color.White;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CourseSectionEvents.RaiseChanged();
            CourseEvents.RaiseChanged();
            this.Close();
        }

        private void 修改課程所屬班級預設教師ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //呼叫修改畫面
            if (dataGridViewX1.SelectedRows.Count == 1)
                DataCheckIn(dataGridViewX1.SelectedRows[0]);
            else if (dataGridViewX1.SelectedRows.Count > 1)
                MsgBox.Show("只能選擇一個課程!!");
            else
                MsgBox.Show("必須選擇一個課程!!");
        }

        private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //呼叫修改畫面
            if (e.RowIndex > -1)
                DataCheckIn(dataGridViewX1.Rows[e.RowIndex]);
            else
                MsgBox.Show("滑鼠左鍵,必須雙擊於正確的位置!!");
        }

        private void DataCheckIn(DataGridViewRow row)
        {
            SchedulerCourseExtension sce = (SchedulerCourseExtension)row.Tag;
            DialogResult dr = System.Windows.Forms.DialogResult.No;
            if (SchedulerDic.ContainsKey(sce.UID)) //是否有課程分段資料
            {
                if (SchedulerDic[sce.UID].Count > 0)
                {
                    修改課程所屬班級預設教師 change = new 修改課程所屬班級預設教師(sce, _tacc, SchedulerDic[sce.UID]);
                    dr = change.ShowDialog();
                }
                else
                {
                    修改課程所屬班級預設教師 change = new 修改課程所屬班級預設教師(sce, _tacc, new List<SchedulerCourseSection>());
                    dr = change.ShowDialog();
                }
            }
            else
            {
                修改課程所屬班級預設教師 change = new 修改課程所屬班級預設教師(sce, _tacc, new List<SchedulerCourseSection>());
                dr = change.ShowDialog();
            }

            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                this.Text = "班級教師檢查(資料取得中...)";
                Select_BGW.RunWorkerAsync();
            }

        }

        private void 加入待處理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                SchedulerCourseExtension sce = (SchedulerCourseExtension)row.Tag;
                if (!list.Contains(sce.UID))
                {
                    list.Add(sce.UID);
                }
            }
            CourseAdmin.Instance.AddToTemp(list);
        }

        private void 清空待處理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CourseAdmin.Instance.RemoveFromTemp(CourseAdmin.Instance.TempSource);
        }

        void Instance_TempSourceChanged(object sender, EventArgs e)
        {
            labelX2.Text = "目前待處理課程：" + CourseAdmin.Instance.TempSource.Count;
        }

        private void dataGridViewX1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count == 1)
                修改課程所屬班級預設教師ToolStripMenuItem.Enabled = true;
            else
                修改課程所屬班級預設教師ToolStripMenuItem.Enabled = false;
        }

        //顯示清單中,有錯誤之行
        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            ifCheckBox();
        }

        private void ifCheckBox()
        {
            if (checkBoxX1.Checked)
            {
                //依據資料錯誤狀態而隱藏
                foreach (DataGridViewRow row in RowErrorDic.Keys)
                    row.Visible = !RowErrorDic[row];
            }
            else
            {
                //全部開啟
                foreach (DataGridViewRow row in RowErrorDic.Keys)
                    row.Visible = true;
            }
        }
    }
}
