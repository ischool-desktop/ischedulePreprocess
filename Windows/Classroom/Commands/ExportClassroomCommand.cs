using Sunset.Windows;

namespace Sunset
{
    /// <summary>
    /// 匯出所有場地
    /// </summary>
    public class ExportClassroomCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯出場地清單"; }
        }

        public string Name
        {
            get { return "ExportAllClassroom"; }
        }

        public bool IsChangeData
        {
            get { return false; }
        }

        public string Execute(object Context)
        {
            ExportSunset.ExportClassroom();

            return string.Empty;
        }

        #endregion
    }
}