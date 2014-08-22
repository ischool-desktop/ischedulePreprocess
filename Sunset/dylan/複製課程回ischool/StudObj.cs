using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sunset.NewCourse
{
    public class StudObj
    {
        public string StudentID { get; set; }
        public string ClassID { get; set; }
        public string 班級 { get; set; }
        public string 座號 { get; set; }
        public string 學號 { get; set; }
        public string 姓名 { get; set; }

        public StudObj(DataRow row)
        {
            StudentID = "" + row["id"];
            ClassID = "" + row["classID"];
            班級 = "" + row["class_name"];
            座號 = "" + row["seat_no"];
            學號 = "" + row["student_number"];
            姓名 = "" + row["name"];
        }
    }
}
