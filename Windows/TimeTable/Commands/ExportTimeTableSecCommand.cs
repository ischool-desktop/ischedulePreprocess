using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Windows;


namespace Sunset
{
    public class ExportTimeTableSecCommand : ICommand
    {

        #region ICommand 成員

        public string Text
        {
            get { return "匯出時間表分段(全部)"; }
        }

        public string Name
        {
            get { return "ExportAllTimeTableSec"; }
        }

        public bool IsChangeData
        {
            get { return false; }
        }

        public string Execute(object Context)
        {
            ExportSunset.ExportTimeTableSec2();

            return string.Empty;
        }

        #endregion
    }
}
