using Campus.DocumentValidator;

namespace Sunset
{
    /// <summary>
    /// 用來產生排課系統所需的自訂驗證規則
    /// </summary>
    public class SunsetRowValidatorFactory : IRowValidatorFactory
    {
        #region IRowValidatorFactory 成員

        /// <summary>
        /// 根據typeName建立對應的RowValidator
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="validatorDescription"></param>
        /// <returns></returns>
        public IRowVaildator CreateRowValidator(string typeName, System.Xml.XmlElement validatorDescription)
        {
            switch (typeName.ToUpper())
            {
                case "COURSENAMECHECK":
                    return new CourseNameCheck();
                case "SCHEDULERCOURSENAMECHECK":
                    return new SchedulerCourseNameCheck();
                case "TEACHERNAMECHECK":
                    return new TeacherNameCheck();
                case "TIMERANGECHECK":
                    return new TimeRangeCheck();
                case "TEACHERNAMEREPEATCHECK":
                    return new TeacherNameRepeatCheck();
                case "TEACHERNAMECHECK_NEW": //排課教師專用檢查
                    return new TeacherNameCheck_New();
                default:
                    return null;
            }
        }

        #endregion
    }
}