using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Windows;

namespace Sunset
{
    public class ImportClassCommand : ICommand
    {

        #region ICommand 成員

        public string Text
        {
            get { return "匯入班級清單" ; }
        }

        public string Name
        {
            get { return "ImportClass"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            (new ImportClassEx()).Execute();

            return string.Empty;
        }

        #endregion
    }
}
