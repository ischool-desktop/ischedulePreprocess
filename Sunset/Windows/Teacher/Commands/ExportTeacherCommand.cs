using Sunset.Windows;

namespace Sunset
{
    /// <summary>
    /// 匯出所有教師
    /// </summary>
    public class ExportTeacherCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯出教師清單"; }
        }

        public string Name
        {
            get { return "ExportAllTeacher"; }
        }

        public bool IsChangeData
        {
            get { return false; }
        }

        public string Execute(object Context)
        {
            ExportSunset.ExportTeacherEx_New();

            return string.Empty;
        }

        #endregion
    }
}