//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using DevComponents.DotNetBar.Controls;

//namespace Sunset
//{
//    public class CheckBoxSource : ChangeSource
//    {
//        public CheckBoxSource(params CheckBoxX[] controls)
//        {
//            Controls = new Dictionary<CheckBoxX, CheckState>();
//            foreach (CheckBoxX control in controls)
//            {
//                Controls.Add(control, control.CheckState);
//                control.CheckedChanged += new EventHandler(Control_CheckStateChanged);
//            }
//        }

//        private void Control_CheckStateChanged(object sender, EventArgs e)
//        {
//            bool changed = false;
//            foreach (CheckBoxX control in Controls.Keys)
//            {
//                if (control.CheckState != Controls[control])
//                {
//                    changed = true;
//                    break;
//                }
//            }
//            if (changed)
//                RaiseStatusChanged(ValueStatus.Dirty);
//            else
//                RaiseStatusChanged(ValueStatus.Clean);
//        }

//        protected Dictionary<CheckBoxX, CheckState> Controls { get; private set; }

//        /// <summary>
//        /// 重設狀態為「Clean」。
//        /// </summary>
//        public override void Reset()
//        {
//            foreach (CheckBoxX control in new Dictionary<CheckBoxX, CheckState>(Controls).Keys)
//                Controls[control] = control.CheckState;
//        }
//    }
//}