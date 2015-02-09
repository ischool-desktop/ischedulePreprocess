using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.NewCourse
{
    class TypeSyneData
    {

        public SchedulerCourseExtension _low_Sce { get; set; }
        public SchedulerCourseExtension _new_Sce { get; set; }

        private int? 場地 { get; set; }
        private string 星期 { get; set; }
        private int 單雙週 { get; set; }
        private string 節次 { get; set; }
        private bool 跨中午 { get; set; }
        private string 教師1 { get; set; }
        private string 教師2 { get; set; }
        private string 教師3 { get; set; }

        public TypeSyneData(SchedulerCourseExtension Sce)
        {
            _low_Sce = Sce;

            場地 = _low_Sce.ClassroomID;
            星期 = _low_Sce.WeekDayCond;
            單雙週 = _low_Sce.WeekFlag;
            節次 = _low_Sce.PeriodCond;
            跨中午 = _low_Sce.LongBreak;
            教師1 = _low_Sce.TeacherName1;
            教師2 = _low_Sce.TeacherName2;
            教師3 = _low_Sce.TeacherName3;

        }

        /// <summary>
        /// 檢查預設值欄位是否有被更動過?
        /// </summary>
        public bool ChangeDataSnyc(SchedulerCourseExtension Sce)
        {
            _new_Sce = Sce;

            bool IsChange = false;
            IsChange = IsChange | 場地是否更動過();
            IsChange = IsChange | 星期是否更動過();
            IsChange = IsChange | 單雙週是否更動過();
            IsChange = IsChange | 節次是否更動過();
            IsChange = IsChange | 跨中午是否更動過();
            IsChange = IsChange | 教師1是否更動過();
            IsChange = IsChange | 教師2是否更動過();
            IsChange = IsChange | 教師3是否更動過();
            return IsChange;
        }

        public bool 場地是否更動過()
        {
            if (場地 != _new_Sce.ClassroomID)
                return true;
            else
                return false;
        }

        public bool 星期是否更動過()
        {
            if (星期 != _new_Sce.WeekDayCond)
                return true;
            else
                return false;
        }

        public bool 單雙週是否更動過()
        {
            if (單雙週 != _new_Sce.WeekFlag)
                return true;
            else
                return false;
        }

        public bool 節次是否更動過()
        {
            if (節次 != _new_Sce.PeriodCond)
                return true;
            else
                return false;
        }

        public bool 跨中午是否更動過()
        {
            if (跨中午 != _new_Sce.LongBreak)
                return true;
            else
                return false;
        }

        public bool 教師1是否更動過()
        {
            if (教師1 != _new_Sce.TeacherName1)
                return true;
            else
                return false;
        }

        public bool 教師2是否更動過()
        {
            if (教師2 != _new_Sce.TeacherName2)
                return true;
            else
                return false;
        }

        public bool 教師3是否更動過()
        {
            if (教師3 != _new_Sce.TeacherName3)
                return true;
            else
                return false;
        }
    }
}
