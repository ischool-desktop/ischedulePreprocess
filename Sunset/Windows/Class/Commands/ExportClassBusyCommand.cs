using Sunset.Windows;

namespace Sunset
{
    /// <summary>
    /// 匯出所有班級不排課時段
    /// </summary>
    public class ExportClassBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯出班級不排課時段(全部)"; }
        }

        public string Name
        {
            get { return "ExportAllClassBusy"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            ExportSunset.ExportClassBusy_New();

            return string.Empty;
        }

        #endregion
    }
}