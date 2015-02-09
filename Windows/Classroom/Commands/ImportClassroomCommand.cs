using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Windows;

namespace Sunset
{
    public class ImportClassroomCommand : ICommand
    {

        #region ICommand 成員

        public string Text
        {
            get { return "匯入場地清單"; }
        }

        public string Name
        {
            get { return "ImportClassroom"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            (new ImportClassroom()).Execute();

            return string.Empty;
        }

        #endregion
    }
}
