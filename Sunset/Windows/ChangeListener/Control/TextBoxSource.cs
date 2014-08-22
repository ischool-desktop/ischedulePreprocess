//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace Sunset
//{
//    public class TextBoxSource : ChangeSource
//    {
//        public TextBoxSource(TextBox control)
//        {
//            Control = control;
//            OriginValue = Control.Text;
//            Control.TextChanged += new EventHandler(Control_TextChanged);
//        }

//        private void Control_TextChanged(object sender, EventArgs e)
//        {
//            if (OriginValue != Control.Text)
//                RaiseStatusChanged(ValueStatus.Dirty);
//            else
//                RaiseStatusChanged(ValueStatus.Clean);
//        }

//        protected TextBox Control { get; private set; }

//        /// <summary>
//        /// 原始的資料(執行過 Reset 之後的資料)。
//        /// </summary>
//        protected string OriginValue { get; private set; }

//        /// <summary>
//        /// 重設狀態為「Clean」。
//        /// </summary>
//        public override void Reset()
//        {
//            OriginValue = Control.Text;
//        }
//    }
//}