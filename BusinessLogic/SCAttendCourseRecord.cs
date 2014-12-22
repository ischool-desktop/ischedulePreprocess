
namespace Sunset
{
    /// <summary>
    /// 新增學生修課所需要的物件
    /// </summary>
    public class SCAttendCourseRecord : K12.Data.CourseRecord
    {
        /// <summary>
        /// 校部訂
        /// </summary>
        public new string RequiredBy
        {
            get { return base.RequiredBy; }
        }

        /// <summary>
        /// 必選修
        /// </summary>
        public new bool Required
        {
            get { return base.Required; }
        }
    }
}