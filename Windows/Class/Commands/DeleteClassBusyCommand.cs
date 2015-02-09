using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.LogAgent;
using Sunset.Windows;

namespace Sunset
{
    public class DeleteClassBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "刪除班級不排課時段"; }
        }

        public string Name
        {
            get { return "DeleteClassBusy"; }
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
                    Conditions.Add("(class_name='" + Name + "')");
                }

                string strSQL = string.Join(" or ", Conditions.ToArray());

                List<ClassEx> vClasses = Utility.AccessHelper.Select<ClassEx>(strSQL);

                if (vClasses.Count == 0)
                {
                    result = "找不到對應的班級，無法刪除!";
                }
                else
                {
                    #region 取得班級不排課時段
                    List<ClassExBusy> vClassusys = Utility.AccessHelper.Select<ClassExBusy>("ref_class_id in (" + string.Join(",", vClasses.Select(x => x.UID).ToArray()) + ")");
                    #endregion

                    //班級 星期 開始時間 結束時間 不排課描述

                    #region 刪除不排課時段
                    if (vClasses.Count > 0)
                    {
                        string DeleteMsg = +vClasses.Count + "個班級「" + string.Join(",", vClasses.Select(x => x.ClassName).ToArray()) + "」不排課時段共" + vClassusys.Count + "筆？";

                        if (MessageBox.Show("您是否要刪除" + DeleteMsg, "確認刪除不排課時段", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            StringBuilder strBuilder = new StringBuilder();

                            strBuilder.AppendLine("刪除不排課時段共" + vClasses.Count + "筆");
                            strBuilder.AppendLine("班級名稱,星期,開始時間,結束時間,不排課描述");

                            foreach (ClassExBusy ClassBusy in vClassusys)
                            {
                                ClassEx Class = vClasses.Find(x => x.UID.Equals("" + ClassBusy.ClassID));

                                if (Class!=null)
                                {
                                    strBuilder.AppendLine(Class.ClassName + "," + ClassBusy.WeekDay + "," + ClassBusy.BeginTime.ToString("HH:mm") + "," + ClassBusy.BeginTime.AddMinutes(ClassBusy.Duration).ToString("HH:mm") + "," + ClassBusy.BusyDesc);
                                }
                            }

                            Utility.AccessHelper.DeletedValues(vClassusys);

                            ApplicationLog.Log("排課", "刪除班級不排課時段", strBuilder.ToString());

                            result = "已刪除「" + vClassusys.Count + "」筆班級不排課時段!";
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