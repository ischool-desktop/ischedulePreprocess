using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using FISCA.Data;
using K12.Data;

namespace Sunset
{
    /// <summary>
    /// 俊威建立的一個常用副程式或功能
    /// </summary>
    static class tool
    {
        static public AccessHelper _A = new AccessHelper();
        static public QueryHelper _Q = new QueryHelper();
        static public UpdateHelper _Update = new UpdateHelper();

        static public int SortTimeTables(TimeTable dt1, TimeTable dt2)
        {
            return dt1.TimeTableName.CompareTo(dt2.TimeTableName);
        }
    }
}
