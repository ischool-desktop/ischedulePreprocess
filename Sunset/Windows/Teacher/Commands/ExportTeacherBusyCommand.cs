using Sunset.Windows;

namespace Sunset
{
    /// <summary>
    /// 匯出所有教師不排課時段
    /// </summary>
    public class ExportTeacherBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯出教師不排課時段(全部)"; }
        }

        public string Name
        {
            get { return "ExportAllTeacherBusy"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            ExportSunset.ExportTeacherExBusy_New();

            return string.Empty;
        }

        #endregion
    }
}