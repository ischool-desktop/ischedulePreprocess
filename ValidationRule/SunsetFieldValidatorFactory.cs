using Campus.DocumentValidator;

namespace Sunset
{
    /// <summary>
    /// 用來產生排課系統所需的自訂驗證規則
    /// </summary>
    public class SunsetFieldValidatorFactory : IFieldValidatorFactory
    {
        #region IFieldValidatorFactory 成員

        /// <summary>
        /// 根據typeName建立對應的FieldValidator
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="validatorDescription"></param>
        /// <returns></returns>
        public IFieldValidator CreateFieldValidator(string typeName, System.Xml.XmlElement validatorDescription)
        {
            switch (typeName.ToUpper())
            {
                case "CLASSINISCHOOLCHECK":
                    return new ClassInischoolCheck(); //取得ischool內的班級清單
                case "CLASSINCHECK":
                    return new ClassInCheck(); //取得排課班級清單
                case "CLASSNAMECHECK":
                    return new ClassInCheck();
                case "TEACHERINISCHOOLCHECK":
                    return new TeacherInischoolCheck(); //取得ischool系統內的所有老師
                case "TEACHERINCHECK":
                    return new TeacherInCheck(); //取得排課教師清單
                case "CLASSROOMNAMECHECK":
                    return new ClassroomNameCheck();
                case "LOCATIONNAMECHECK":
                    return new LocationNameCheck();
                case "TIMETABLENAMECHECK":
                    return new TimeTableNameCheck();
                case "TIMEFORMATCHECK":
                    return new TimeFormatCheck();
                default:
                    return null;
            }
        }

        #endregion
    }
}