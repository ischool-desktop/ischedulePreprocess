using Sunset.Windows;

namespace Sunset
{
    public class ImportClassBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯入班級不排課時段"; }
        }

        public string Name
        {
            get { return "ImportClassExBusy"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            (new ImportClassExBusy()).Execute();

            return string.Empty;
        }

        #endregion
    }
}