using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Presentation.Controls;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Sunset.NewCourse
{
    class PartitioningSetup
    {
        //檢查:
        //1.所選的課程,是否有設定節次
        //2.所選的課程,在輸入分割設定時,是否是合理切割

        List<string> _CourseIDList { get; set; }

        BackgroundWorker BGW_Check = new BackgroundWorker();

        BackgroundWorker BGW = new BackgroundWorker();

        BackgroundWorker BGW_Sch = new BackgroundWorker();

        frmAssignSplitSpec SplitSpec { get; set; }

        List<string> NewCourseSectionIDs = new List<string>();

        /// <summary>
        /// 準備資料
        /// 根據選取的課程系統編號
        /// 取得課程排課資料
        /// </summary>
        public PartitioningSetup(List<string> CourseIDList)
        {
            _CourseIDList = CourseIDList;

            BGW_Check.DoWork += new DoWorkEventHandler(BGW_Check_DoWork);
            BGW_Check.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_Check_RunWorkerCompleted);

            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            BGW_Sch.DoWork += new DoWorkEventHandler(BGW_Sch_DoWork);
            BGW_Sch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_Sch_RunWorkerCompleted);
        }

        /// <summary>
        /// 開始執行 分割設定
        /// </summary>
        public void Excute()
        {
            if (_CourseIDList.Count == 0)
            {
                MsgBox.Show("請選擇課程!!");
                return;
            }

            //取得節次內容,判斷一下是否為0或空白
            BGW_Check.RunWorkerAsync();
        }

        void BGW_Check_DoWork(object sender, DoWorkEventArgs e)
        {
            string selectRQ = string.Join("','", _CourseIDList);

            string strSQL_t = string.Format("select period from $scheduler.scheduler_course_extension where uid in ('{0}') group by period", selectRQ);
            DataTable dt1 = tool._Q.Select(strSQL_t);

            List<string> list = new List<string>();
            foreach (DataRow row in dt1.Rows)
            {
                string period = "" + row["period"];

                //如果為空或是內容為0,就錯誤
                if (string.IsNullOrEmpty(period) || period == "0")
                {
                    e.Cancel = true;
                    MsgBox.Show("部份課程節數為「0」或「未設定」無法批次指定「分割設定」");
                    return;
                }

                if (!list.Contains(period))
                {
                    list.Add(period);
                }
            }

            e.Result = list;
        }

        void BGW_Check_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    List<string> list = (List<string>)e.Result;

                    //節數類型大於1,就錯誤
                    if (list.Count != 1)
                    {
                        MsgBox.Show("課程「節數」不一致,無法批次指定「分割設定」");
                        return;
                    }

                    //理論上list應該只有1種節次
                    int CheckString = int.Parse(list[0]);

                    //設定畫面
                    SplitSpec = new frmAssignSplitSpec(CheckString);

                    if (SplitSpec.ShowDialog() == DialogResult.OK)
                    {
                        BGW.RunWorkerAsync();
                    }
                }
                else
                {
                    MsgBox.Show("檢查作業錯誤:\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("作業已取消!\n請重新選擇課程!!");
            }

        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            StringBuilder sbString = new StringBuilder();
            sbString.AppendLine("已成功指定" + _CourseIDList.Count + "筆課程分割設定：");
            sbString.AppendLine("課程分割設定指定為「" + SplitSpec.SplitSpec + "」");
            DataTable dt = tool._Q.Select(string.Format("select school_year,semester,course_name from $scheduler.scheduler_course_extension where uid in('{0}')", string.Join("','", _CourseIDList)));
            foreach (DataRow row in dt.Rows)
            {
                string school_year = "" + row["school_year"];
                string semester = "" + row["semester"];
                string course_name = "" + row["course_name"];

                sbString.AppendLine(string.Format("學年度「{0}」學期「{1}」課程「{2}」", school_year, semester, course_name));
            }

            string strCondition = string.Join(",", _CourseIDList);
            string strSQL = "update $scheduler.scheduler_course_extension SET split_spec='" + SplitSpec.SplitSpec + "' where uid in (" + strCondition + ")";

            tool._Update.Execute(strSQL);

            FISCA.LogAgent.ApplicationLog.Log("排課", "指定", sbString.ToString());

        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    MsgBox.Show("已成功指定" + _CourseIDList.Count + "筆課程分割設定!");

                    //是否同步增加課程分段
                    if (SplitSpec.IsCreateCourseSection)
                    {
                        FISCA.Presentation.MotherForm.SetStatusBarMessage("已開始處理 課程分段新增作業...");
                        BGW_Sch.RunWorkerAsync();

                        //建立Spliter(原寫法
                        //FunctionSpliter<string, string> Spliter = new FunctionSpliter<string, string>(300, 3);
                        //Spliter.Function = (x) => SunsetBL.CreateSchedulerCourseSectionByCourseIDs(x);
                        //Spliter.ProgressChange = x => MotherForm.SetStatusBarMessage(string.Empty, x);
                        //List<string> NewCourseSectionIDs = Spliter.Execute(CourseIDList);
                        //FISCA.LogAgent.ApplicationLog.Log("排課", "新增課程分段", "已成功新增「" + NewCourseSectionIDs.Count + "」筆課程分段!");
                        //MotherForm.SetStatusBarMessage("已成功新增「" + NewCourseSectionIDs.Count + "」筆課程分段!");
                    }

                    //排課課程 進行更新事件
                    CourseEvents.RaiseChanged();
                }
                else
                {
                    MsgBox.Show("指定課程分割設定錯誤:\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("已取消指定課程分割設定");
            }
        }

        void BGW_Sch_DoWork(object sender, DoWorkEventArgs e)
        {
            FunctionSpliter<string, string> Spliter = new FunctionSpliter<string, string>(300, 3);
            Spliter.Function = (x) => SunsetBL.CreateSchedulerCourseSectionByCourseIDs(x);
            //Spliter.ProgressChange = x => MotherForm.SetStatusBarMessage(string.Empty, x);
            NewCourseSectionIDs = Spliter.Execute(_CourseIDList);

            //製作詳細的課程分段Log
            List<SchedulerCourseSection> sch_list = tool._A.Select<SchedulerCourseSection>(NewCourseSectionIDs);
            Dictionary<string, List<SchedulerCourseSection>> sch_dic = new Dictionary<string, List<SchedulerCourseSection>>();
            foreach (SchedulerCourseSection each in sch_list)
            {
                if (!sch_dic.ContainsKey(each.CourseID.ToString()))
                {
                    sch_dic.Add(each.CourseID.ToString(), new List<SchedulerCourseSection>());
                }
                sch_dic[each.CourseID.ToString()].Add(each);
            }

            List<SchedulerCourseExtension> courseList = tool._A.Select<SchedulerCourseExtension>(sch_dic.Keys.ToList());
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("已成功新增課程分段:");
            foreach (SchedulerCourseExtension each in courseList)
            {
                if (sch_dic.ContainsKey(each.UID))
                {
                    sb.AppendLine(string.Format("學年度「{0}」學期「{1}」課程名稱「{2}」課程分段共「{3}」筆", each.SchoolYear.ToString(), each.Semester, each.CourseName, sch_dic[each.UID].Count));
                }
            }

            FISCA.LogAgent.ApplicationLog.Log("排課", "新增課程分段", sb.ToString());
        }

        void BGW_Sch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    FISCA.Presentation.MotherForm.SetStatusBarMessage("新增課程分段已完成!");
                    MsgBox.Show("新增課程分段已完成!\n(詳細請檢視系統歷程記錄!)");

                    //課程分段相關,進行更新事件
                    CourseSectionEvents.RaiseChanged();
                }
                else
                {
                    MsgBox.Show("新增課程分段錯誤:\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("已取消新增課程分段");
            }
        }

    }
}
