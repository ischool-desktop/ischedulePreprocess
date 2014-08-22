using System.Collections.Generic;
using K12.Data;

namespace Sunset
{
    /// <summary>
    /// 預開課程記錄物件
    /// </summary>
    public class PreOpenCourseRecord
    {
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="Subject">課程規劃科目</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="ClassID">班級系統編號</param>
        /// <param name="ClassName">班級名稱</param>
        public PreOpenCourseRecord(ProgramSubject Subject,string SchoolYear,string ClassID,string ClassName)
        {
            this.Subject = Subject;
            this.ClassName = ClassName;
            this.ClassID = ClassID;
            this.SchoolYear = SchoolYear;
        }

        /// <summary>
        /// 課程規劃科目
        /// </summary>
        public ProgramSubject Subject { get; private set; }

        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName { get; private set;}

        /// <summary>
        /// 班級系統編號
        /// </summary>
        public string ClassID { get; private set;}

        /// <summary>
        /// 科目名稱
        /// </summary>
        public string SubjectName { get { return Subject.SubjectName; } }

        /// <summary>
        /// 級別
        /// </summary>
        public string Level { get { return K12.Data.Int.GetString(Subject.Level); } }

        /// <summary>
        /// 課程名稱，根據班級名稱、科目名稱及級別組成
        /// </summary>
        public string CourseName 
        {
            get { return (ClassName.Trim() + " " + SubjectName.Trim() + Utility.GetNumberString(Level)).Trim(); }
        }

        /// <summary>
        /// 課程鍵值，由課程名稱、學年度及學期所組成
        /// </summary>
        public string CourseKey
        {
            get { return CourseName + "," + SchoolYear.Trim() + "," + Semester.Trim(); }
        }

        /// <summary>
        /// 課程鍵值條件，內部使用，用來下SQL指令
        /// </summary>
        internal string CourseKeyCondition
        {
            get { return ToString(); }
        }

        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear { get; private set;}

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester { get { return "" + Subject.Semester;} }

        /// <summary>
        /// 課程系統編號
        /// </summary>
        public string CourseID { get; internal set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {         
            return "(course_name='"+ CourseName +"' and school_year="+ SchoolYear +" and semester=" + Semester + ")";
        }
    }

    /// <summary>
    /// 比較預開課程是否相同
    /// </summary>
    public class PreOpenCourseRecordComparer : IEqualityComparer<PreOpenCourseRecord>
    {
        /// <summary>
        /// 比較兩個預開課程是否相同
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(PreOpenCourseRecord x, PreOpenCourseRecord y)
        {
            return x.CourseKey == y.CourseKey;
        }

        /// <summary>
        /// 取得HashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(PreOpenCourseRecord obj)
        {
            return obj.GetHashCode();
        }
    }
}