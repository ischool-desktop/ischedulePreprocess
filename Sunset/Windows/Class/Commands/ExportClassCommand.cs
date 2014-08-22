using Sunset.Windows;

namespace Sunset
{
    /// <summary>
    /// 匯出所有班級
    /// </summary>
    public class ExportClassCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯出班級清單"; }
        }

        public string Name
        {
            get { return "ExportAllClass"; }
        }

        public bool IsChangeData
        {
            get { return false; }
        }

        public string Execute(object Context)
        {
            ExportSunset.ExportClass_New();

            return string.Empty;
        }

        #endregion
    }
}