using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace Sunset.NewCourse
{
    public partial class 修改課程所屬班級預設教師 : BaseForm
    {
        private SchedulerCourseExtension _sce { get; set; }

        private TACC _tacc { get; set; }

        private List<SchedulerCourseSection> _SchList { get; set; }

        public 修改課程所屬班級預設教師(SchedulerCourseExtension sce, TACC tacc, List<SchedulerCourseSection> list)
        {
            InitializeComponent();
            _sce = sce;
            labelX1.Text = string.Format("學年度：{0}　學期：{1}\n課程名稱：{2}", _sce.SchoolYear.ToString(), _sce.Semester, _sce.CourseName);
            _tacc = tacc;
            _SchList = list;

            //本功能定義,使用者必須選取系統內教師與班級
        }

        private void 修改課程所屬班級預設教師_Load(object sender, EventArgs e)
        {
            comboBoxEx1.Items.AddRange(_tacc._ClassNameDic.Keys.ToArray());
            comboBoxEx2.Items.AddRange(_tacc._TeacherNameDic.Keys.ToArray());
            comboBoxEx3.Items.AddRange(_tacc._TeacherNameDic.Keys.ToArray());
            comboBoxEx4.Items.AddRange(_tacc._TeacherNameDic.Keys.ToArray());

            comboBoxEx1.Text = _sce.ClassName;
            comboBoxEx2.Text = _sce.TeacherName1;
            comboBoxEx3.Text = _sce.TeacherName2;
            comboBoxEx4.Text = _sce.TeacherName3;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!檢查班級名稱是否存在() || !檢查教師名稱是否存在(comboBoxEx2.Text) || !檢查教師名稱是否存在(comboBoxEx3.Text) || !檢查教師名稱是否存在(comboBoxEx4.Text))
            {
                DialogResult dr = MsgBox.Show("輸入的資料未正確對應\n繼續儲存動作?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dr == System.Windows.Forms.DialogResult.No)
                    return;
            }

            if (資料是否修改())
            {
                #region 修改資料
                bool checkTeacher = false;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("已修改排課課程資料\n學年度「{0}」學期「{1}」課程「{2}」：", _sce.SchoolYear.ToString(), _sce.Semester, _sce.CourseName));
                if (_sce.ClassName != comboBoxEx1.Text)
                {
                    sb.AppendLine(string.Format("所屬班級由「{0}」修改為「{1}」", _sce.ClassName, comboBoxEx1.Text));
                    _sce.ClassName = comboBoxEx1.Text;
                }
                if (_sce.TeacherName1 != comboBoxEx2.Text)
                {
                    sb.AppendLine(string.Format("預設教師1由「{0}」修改為「{1}」", _sce.TeacherName1, comboBoxEx2.Text));
                    _sce.TeacherName1 = comboBoxEx2.Text;
                    checkTeacher = true;
                }
                if (_sce.TeacherName2 != comboBoxEx3.Text)
                {
                    sb.AppendLine(string.Format("預設教師2由「{0}」修改為「{1}」", _sce.TeacherName2, comboBoxEx3.Text));
                    _sce.TeacherName2 = comboBoxEx3.Text;
                    checkTeacher = true;
                }
                if (_sce.TeacherName3 != comboBoxEx4.Text)
                {
                    sb.AppendLine(string.Format("預設教師3由「{0}」修改為「{1}」", _sce.TeacherName3, comboBoxEx4.Text));
                    _sce.TeacherName3 = comboBoxEx4.Text;
                    checkTeacher = true;
                }

                //是否為現存班級
                if (檢查班級名稱是否存在())
                {
                    int classID;
                    int.TryParse(_tacc._ClassNameDic[comboBoxEx1.Text], out classID);
                    _sce.ClassID = classID;
                }
                else
                {
                    _sce.ClassID = null; //清空
                }

                #region 除了名稱,也要同步課程分段的教師資料
                if (checkTeacher)
                {
                    DialogResult dr = MsgBox.Show("調整預設教師,是否要同步課程分段的授課教師資料?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        sb.AppendLine("");
                        sb.AppendLine("同步課程分段資料：");

                        #region 調整預設教師資料
                        if (檢查教師名稱是否存在(comboBoxEx2.Text))
                        {
                            string TeacherID = _tacc._TeacherNameDic[comboBoxEx2.Text];
                            string TeacherName = comboBoxEx2.Text;
                            sb.AppendLine(string.Format("授課教師1修改為「{0}」", TeacherName));
                            foreach (SchedulerCourseSection each in _SchList)
                            {
                                each.TeacherName1 = TeacherName;
                                each.TeacherID1 = int.Parse(TeacherID);
                            }
                        }
                        else
                        {
                            sb.AppendLine("授課教師1修改為「」");
                            foreach (SchedulerCourseSection each in _SchList)
                            {
                                each.TeacherName1 = "";
                                each.TeacherID1 = null;
                            }
                        }

                        if (檢查教師名稱是否存在(comboBoxEx3.Text))
                        {
                            string TeacherID = _tacc._TeacherNameDic[comboBoxEx3.Text];
                            string TeacherName = comboBoxEx3.Text;
                            sb.AppendLine(string.Format("授課教師2修改為「{0}」", TeacherName));
                            foreach (SchedulerCourseSection each in _SchList)
                            {
                                each.TeacherName2 = TeacherName;
                                each.TeacherID2 = int.Parse(TeacherID);
                            }
                        }
                        else
                        {
                            sb.AppendLine("授課教師2修改為「」");
                            foreach (SchedulerCourseSection each in _SchList)
                            {
                                each.TeacherName2 = "";
                                each.TeacherID2 = null;
                            }
                        }

                        if (檢查教師名稱是否存在(comboBoxEx4.Text))
                        {
                            string TeacherID = _tacc._TeacherNameDic[comboBoxEx4.Text];
                            string TeacherName = comboBoxEx4.Text;
                            sb.AppendLine(string.Format("授課教師3修改為「{0}」", TeacherName));
                            foreach (SchedulerCourseSection each in _SchList)
                            {
                                each.TeacherName3 = TeacherName;
                                each.TeacherID3 = int.Parse(TeacherID);
                            }
                        }
                        else
                        {
                            sb.AppendLine("授課教師3修改為「」");
                            foreach (SchedulerCourseSection each in _SchList)
                            {
                                each.TeacherName3 = "";
                                each.TeacherID3 = null;
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                List<SchedulerCourseExtension> list = new List<SchedulerCourseExtension>();
                list.Add(_sce);
                tool._A.UpdateValues(list); //課程基本資料

                tool._A.UpdateValues(_SchList); //課程分段

                FISCA.LogAgent.ApplicationLog.Log("排課", "修改", sb.ToString());

                #endregion
            }
            else
            {
                MsgBox.Show("資料未修改...");
            }
        }

        private bool 資料是否修改()
        {
            bool check = false;

            if (_sce.ClassName != comboBoxEx1.Text)
                check = true;

            check = check | 是否修改預設教師資料();

            return check;
        }

        private bool 是否修改預設教師資料()
        {
            bool checkTeacher = false;

            if (_sce.TeacherName1 != comboBoxEx2.Text)
                checkTeacher = checkTeacher | true;

            if (_sce.TeacherName2 != comboBoxEx3.Text)
                checkTeacher = checkTeacher | true;

            if (_sce.TeacherName3 != comboBoxEx4.Text)
                checkTeacher = checkTeacher | true;

            return checkTeacher;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        #region 資料檢查

        /// <summary>
        /// 檢查班級名稱
        /// </summary>
        private bool 檢查班級名稱是否存在()
        {
            if (_tacc._ClassNameDic.ContainsKey(comboBoxEx1.Text))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 檢查教師名稱
        /// </summary>
        private bool 檢查教師名稱是否存在(string TeacherName)
        {
            if (_tacc._TeacherNameDic.ContainsKey(TeacherName))
                return true;
            else
                return false;
        }

        #endregion

        private void comboBoxEx1_TextChanged(object sender, EventArgs e)
        {
            if (!檢查班級名稱是否存在())
                errorProvider1.SetError(comboBoxEx1, "不存在於ischool班級清單內!!");
            else
                errorProvider1.Clear();
        }

        private void comboBoxEx2_TextChanged(object sender, EventArgs e)
        {
            if (!檢查教師名稱是否存在(comboBoxEx2.Text))
                errorProvider2.SetError(comboBoxEx2, "不存在於ischool教師清單內!!");
            else
                errorProvider2.Clear();
        }

        private void comboBoxEx3_TextChanged(object sender, EventArgs e)
        {
            if (!檢查教師名稱是否存在(comboBoxEx3.Text))
                errorProvider3.SetError(comboBoxEx3, "不存在於ischool教師清單內!!");
            else
                errorProvider3.Clear();
        }

        private void comboBoxEx4_TextChanged(object sender, EventArgs e)
        {
            if (!檢查教師名稱是否存在(comboBoxEx4.Text))
                errorProvider4.SetError(comboBoxEx4, "不存在於ischool教師清單內!!");
            else
                errorProvider4.Clear();
        }
    }
}
