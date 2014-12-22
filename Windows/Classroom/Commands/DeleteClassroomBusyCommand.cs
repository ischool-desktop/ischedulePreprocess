using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.LogAgent;
using Sunset.Windows;

namespace Sunset
{
    public class DeleteClassroomBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "刪除場地不排課時段"; }
        }

        public string Name
        {
            get { return "DeleteClassroomBusy"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            string result = string.Empty;

            List<string> Names = Context as List<string>;

            List<string> Conditions = new List<string>();

            try
            {
                foreach (string Name in Names)
                {
                    Conditions.Add("(name='" + Name +"')");
                }

                string strSQL = string.Join(" or ", Conditions.ToArray());

                //取得教師
                List<Classroom> vClassrooms = Utility.AccessHelper.Select<Classroom>(strSQL);

                if (vClassrooms.Count == 0)
                {
                    result = "找不到對應的場地，無法刪除!";
                }
                else
                {
                    #region 取得場地不排課時段
                    List<ClassroomBusy> vClassroomBusys = Utility.AccessHelper.Select<ClassroomBusy>("ref_classroom_id in (" + string.Join(",", vClassrooms.Select(x => x.UID).ToArray()) + ")");
                    #endregion

                    //場地姓名
                    //星期
                    //開始時間
                    //結束時間
                    //不排課描述

                    #region 刪除不排課時段
                    if (vClassroomBusys.Count > 0)
                    {
                        string DeleteMsg = +vClassrooms.Count + "個場地「" + string.Join(",", vClassrooms.Select(x => x.ClassroomName).ToArray()) + "」不排課時段共" + vClassroomBusys.Count + "筆？";

                        if (MessageBox.Show("您是否要刪除" + DeleteMsg, "確認刪除不排課時段", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            StringBuilder strBuilder = new StringBuilder();

                            strBuilder.AppendLine("刪除不排課時段共" + vClassroomBusys.Count + "筆");
                            strBuilder.AppendLine("場地姓名,星期,開始時間,結束時間,不排課描述");

                            foreach (ClassroomBusy ClassroomBusy in vClassroomBusys)
                            {
                                Classroom Classroom = vClassrooms.Find(x => x.UID.Equals("" + ClassroomBusy.ClassroomID));

                                if (Classroom != null)
                                {
                                    strBuilder.AppendLine(Classroom.ClassroomName + "," + ClassroomBusy.WeekDay + "," + ClassroomBusy.BeginTime.ToString("HH:mm") + "," + ClassroomBusy.BeginTime.AddMinutes(ClassroomBusy.Duration).ToString("HH:mm")+","+ClassroomBusy.BusyDesc);
                                }
                            }

                            Utility.AccessHelper.DeletedValues(vClassroomBusys);

                            ApplicationLog.Log("排課", "刪除場地不排課時段", strBuilder.ToString());

                            result = "已刪除「" + vClassroomBusys.Count + "」筆場地不排課時段!";
                        }
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return result;
        }

        #endregion
    }
}