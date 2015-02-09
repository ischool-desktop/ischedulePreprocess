using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Windows;


namespace Sunset
{
    public class ImportTimeTableSecCommand : ICommand
    {

        #region ICommand 成員

        public string Text
        {
            get { return "匯入時間表分段"; }
        }

        public string Name
        {
            get { return "ImportTimeTableSec"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            (new ImportTimeTableSec()).Execute();

            return string.Empty;
        }

        #endregion
    }
}