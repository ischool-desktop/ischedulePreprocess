using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Windows;

namespace Sunset
{
    public class ExportTimeTableCommand : ICommand
    {
        public ExportTimeTableCommand()
        {
        }

        #region ICommand 成員

        public string Text
        {
            get { return "匯出時間表清單"; }
        }

        public string Name
        {
            get { return "ExportAllTimeTable"; }
        }

        public bool IsChangeData
        {
            get { return false ; }
        }

        public string Execute(object Context)
        {
            ExportSunset.ExportTimeTable();

            return string.Empty;
        }

        #endregion
    }
}
