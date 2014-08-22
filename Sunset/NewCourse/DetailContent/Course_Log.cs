using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.NewCourse
{
    static class Course_Log
    {

        static public string SetBool(string x1, string name1, string name2, bool j1)
        {
            StringBuilder sb = new StringBuilder();

            if (j1)
                sb.Append(SetUpdataValue(x1, name1, name2));
            else
                sb.Append(SetUpdataValue(x1, name2, name1));
            return sb.ToString();
        }

        static public bool SetPriValueBool(int? name1, int? name2)
        {
            string x1 = "";
            if (name1.HasValue)
            {
                x1 = name1.Value.ToString();
            }
            string x2 = "";
            if (name2.HasValue)
            {
                x2 = name2.Value.ToString();
            }

            if (x1 != x2)
                return true;
            else
                return false;
        }

        static public string SetPerValue(string Title, int? name1, int? name2)
        {
            string x1 = "";
            if (name1.HasValue)
            {
                x1 = name1.Value.ToString();
            }
            string x2 = "";
            if (name2.HasValue)
            {
                x2 = name2.Value.ToString();
            }
            if (x1 != x2)
                return SetUpdataValue(Title, x1, x2);
            else
                return "";
        }


        static public string SetUpdataValue(string Title, string name1, string name2)
        {
            return string.Format("「{0}」由「{1}」修改為「{2}」", Title, name1, name2);
        }

        static public string SetNewValue(string Title, string name1)
        {
            return string.Format("已新增「{0}」為「{1}」", Title, name1);
        }

        static public string GetWeekFlagName(int x)
        {
            switch (x)
            {
                case 1:
                    return "單";
                case 2:
                    return "雙";
                default:
                    return "單雙";
            }
        }

        static public string GetLongBreak(bool x)
        {
            if (x)
                return "是";
            else
                return "否";
        }

        static public string GetLogString(string a, string b, string c)
        {
            return string.Format("{0}由「{1}」修改為「{2}」", a, b, c);
        }
    }
}
