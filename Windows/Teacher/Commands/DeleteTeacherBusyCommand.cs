using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.LogAgent;
using Sunset.Windows;

namespace Sunset
{
    public class DeleteTeacherBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "刪除教師不排課時段"; }
        }

        public string Name
        {
            get { return "DeleteTeacherBusy"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }


        private Tuple<string, string> GetTeacherDetailName(string Name)
        {
            if (Name.Contains("(") && Name.EndsWith(")"))
            {
                int LeftIndex = Name.IndexOf("(");
                int RightIndex = Name.IndexOf(")");

                if (RightIndex > LeftIndex)
                {
                    string TeacherName = Name.Substring(0, LeftIndex);
                    string NickName = Name.Substring(LeftIndex + 1, RightIndex - LeftIndex - 1);

                    return new Tuple<string, string>(TeacherName, NickName);
                }
                else
                    return new Tuple<string, string>(Name, string.Empty);
            }
            else
                return new Tuple<string, string>(Name, string.Empty);
        }

        private string GetNicknameCondition(string Nickname)
        {
            if (string.IsNullOrEmpty(Nickname))
                return "nickname is null";
            else
                return "nickname='" + Nickname + "'";
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
                    Tuple<string, string> KeyDetail = GetTeacherDetailName(Name);

                    Conditions.Add("(teacher_name='" + KeyDetail.Item1 + "' and " + GetNicknameCondition(KeyDetail.Item2) + ")");
                }

                string strSQL = string.Join(" or ", Conditions.ToArray());

                //取得教師
                List<TeacherEx> vTeachers = Utility.AccessHelper.Select<TeacherEx>(strSQL);

                if (vTeachers.Count == 0)
                {
                    result = "找不到對應的教師，無法刪除!";
                }
                else
                {
                    #region 取得教師不排課時段
                    List<TeacherExBusy> vTeacherBusys = Utility.AccessHelper.Select<TeacherExBusy>("ref_teacher_id in (" + string.Join(",", vTeachers.Select(x => x.UID).ToArray()) + ")");
                    #endregion

                    //教師姓名
                    //教師暱稱
                    //星期
                    //開始時間
                    //結束時間
                    //不排課描述

                    #region 刪除不排課時段
                    if (vTeacherBusys.Count > 0)
                    {
                        string DeleteMsg = +vTeachers.Count + "位教師「" + string.Join(",", vTeachers.Select(x => x.FullTeacherName).ToArray()) + "」不排課時段共" + vTeacherBusys.Count + "筆？";

                        if (MessageBox.Show("您是否要刪除"+ DeleteMsg, "確認刪除不排課時段", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            StringBuilder strBuilder = new StringBuilder();

                            strBuilder.AppendLine("刪除不排課時段共" + vTeacherBusys.Count + "筆");
                            strBuilder.AppendLine("教師姓名,教師暱稱,星期,開始時間,結束時間,不排課描述");

                            foreach(TeacherExBusy TeacherBusy in vTeacherBusys)
                            {
                                TeacherEx Teacher = vTeachers.Find(x => x.UID.Equals(""+TeacherBusy.TeacherID));

                                if (Teacher != null)
                                {
                                    strBuilder.AppendLine(Teacher.TeacherName + "," + Teacher.NickName + "," + TeacherBusy.WeekDay + "," + TeacherBusy.BeginTime.ToString("HH:mm") + "," + TeacherBusy.BeginTime.AddMinutes(TeacherBusy.Duration).ToString("HH:mm")+","+TeacherBusy.BusyDesc);
                                }
                            }

                            Utility.AccessHelper.DeletedValues(vTeacherBusys);

                            ApplicationLog.Log("排課", "刪除教師不排課時段", strBuilder.ToString());

                            result = "已刪除「" + vTeacherBusys.Count + "」筆教師不排課時段!"; 
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
