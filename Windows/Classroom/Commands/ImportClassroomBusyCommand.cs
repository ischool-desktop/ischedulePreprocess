using Sunset.Windows;

namespace Sunset
{
    public class ImportClassroomBusyCommand : ICommand
    {
        #region ICommand 成員

        public string Text
        {
            get { return "匯入場地不排課時段"; }
        }

        public string Name
        {
            get { return "ImportClassroomBusy"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            (new ImportClassroomBusy()).Execute();

            return string.Empty;
        }

        #endregion
    }
}