//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sunset
//{
//    public abstract class ChangeSource : IChangeSource
//    {
//        #region IChangeSource 成員

//        public event EventHandler<ChangeEventArgs> StatusChanged;

//        public bool Suspend { get; set; }

//        protected void RaiseStatusChanged(ValueStatus status)
//        {
//            if (StatusChanged != null)
//                StatusChanged(this, new ChangeEventArgs(status));
//        }

//        public abstract void Reset();
//        #endregion
//    }
//}