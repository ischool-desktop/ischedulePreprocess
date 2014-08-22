using Sunset.Windows;

namespace Sunset
{
    public class ImportTeacherBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯入教師不排課時段"; }
        }

        public string Name
        {
            get { return "ImportTeacherExBusy"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            (new ImportTeacherExBusy()).Execute();

            return string.Empty;
        }

        #endregion
    }
}