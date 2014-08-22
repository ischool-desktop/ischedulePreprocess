using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Windows;

namespace Sunset
{
    public class ImportTimeTableCommand : ICommand
    {

        #region ICommand 成員

        public string Text
        {
            get { return "匯入時間表清單"; }
        }

        public string Name
        {
            get { return "ImportTimeTable"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            (new ImportTimeTable()).Execute();

            return string.Empty;
        }

        #endregion
    }
}
