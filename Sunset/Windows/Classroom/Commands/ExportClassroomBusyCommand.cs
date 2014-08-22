using Sunset.Windows;

namespace Sunset
{
    /// <summary>
    /// 匯出所有場地不排課時段
    /// </summary>
    public class ExportClassroomBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯出場地不排課時段(全部)"; }
        }

        public string Name
        {
            get { return "ExportAllClassroomBusy"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            ExportSunset.ExportClassroomBusy2();

            return string.Empty;
        }

        #endregion
    }
}