//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace Sunset
//{
//    public class ComboBoxSource : ChangeSource
//    {
//        public enum ListenAttribute
//        {
//            SelectedIndex, Text
//        }

//        public ComboBoxSource(ComboBox control, ListenAttribute target)
//        {
//            ListenOnValidated = false;
//            ListenTarget = target;
//            Control = control;
//            SubscribeControlEvents();
//            Reset();
//        }

//        public ComboBoxSource(ComboBox control, ListenAttribute target, bool listenOnValidated)
//        {
//            ListenOnValidated = listenOnValidated;
//            ListenTarget = target;
//            Control = control;
//            SubscribeControlEvents();
//            Reset();
//        }

//        private void SubscribeControlEvents()
//        {
//            Control.TextChanged += new EventHandler(Control_TextChanged);
//            Control.SelectedIndexChanged += new EventHandler(Control_SelectedIndexChanged);
//            Control.Validated += new EventHandler(Control_Validated);
//        }

//        protected ComboBox Control { get; set; }

//        protected string OriginText { get; set; }

//        protected int OriginSelectedIndex { get; set; }

//        public ListenAttribute ListenTarget { get; set; }

//        public bool ListenOnValidated { get; set; }

//        public override void Reset()
//        {
//            OriginText = Control.Text;
//            OriginSelectedIndex = Control.SelectedIndex;
//        }

//        private void Control_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (ListenTarget != ListenAttribute.SelectedIndex) return;
//            RaiseSelectedIndexChanged();
//        }

//        private void Control_TextChanged(object sender, EventArgs e)
//        {
//            if (ListenTarget != ListenAttribute.Text) return;
//            RaiseTextChanged();
//        }

//        private void Control_Validated(object sender, EventArgs e)
//        {
//            if (ListenTarget == ListenAttribute.SelectedIndex)
//                RaiseSelectedIndexChanged();

//            if (ListenTarget == ListenAttribute.Text)
//                RaiseTextChanged();
//        }

//        private void RaiseSelectedIndexChanged()
//        {
//            if (Control.SelectedIndex != OriginSelectedIndex)
//                RaiseStatusChanged(ValueStatus.Dirty);
//            else
//                RaiseStatusChanged(ValueStatus.Clean);
//        }

//        private void RaiseTextChanged()
//        {
//            if (Control.Text != OriginText)
//                RaiseStatusChanged(ValueStatus.Dirty);
//            else
//                RaiseStatusChanged(ValueStatus.Clean);
//        }
//    }
//}