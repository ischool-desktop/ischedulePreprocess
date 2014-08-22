using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Windows;

namespace Sunset
{
    class GetischoolTeacherList : ICommand
    {
        public string Text
        {
            get { return "從ischool複製教師清單"; }
        }

        public string Name
        {
            get { return "Get_ischool_TeacherList"; }
        }

        public bool IsChangeData
        {
            get { return true; }
        }

        public string Execute(object Context)
        {
            GetTeacherListForm gclf = new GetTeacherListForm();
            gclf.ShowDialog();

            return string.Empty;
        }
    }
}
