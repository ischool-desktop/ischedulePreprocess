//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sunset
//{
//    public class ChangeListener
//    {
//        public ChangeListener()
//        {
//            Sources = new Dictionary<IChangeSource, ValueStatus>();
//        }

//        private Dictionary<IChangeSource, ValueStatus> Sources { get; set; }

//        private ValueStatus CurrentStatus { get; set; }

//        /// <summary>
//        /// 當值的狀態改變時發生。
//        /// </summary>
//        public event EventHandler<ChangeEventArgs> StatusChanged;

//        /// <summary>
//        /// 新增「狀態收聽者」。
//        /// </summary>
//        /// <param name="listener"></param>
//        public void Add(IChangeSource source)
//        {
//            Sources.Add(source, ValueStatus.Clean);
//            source.StatusChanged += new EventHandler<ChangeEventArgs>(Source_StatusChanged);
//        }

//        private void Source_StatusChanged(object sender, ChangeEventArgs e)
//        {
//            if (StatusChanged != null && Listen)
//            {
//                Sources[sender as IChangeSource] = e.Status;
//                ValueStatus status = ValueStatus.Clean;
//                foreach (ValueStatus each in Sources.Values)
//                {
//                    if (each == ValueStatus.Dirty)
//                    {
//                        status = ValueStatus.Dirty;
//                        break;
//                    }
//                }

//                if (CurrentStatus != status)
//                    StatusChanged(this, new ChangeEventArgs(status));

//                CurrentStatus = status;
//            }
//        }

//        private bool Listen { get; set; }

//        public void SuspendListen()
//        {
//            Listen = false;

//            foreach (IChangeSource each in Sources.Keys)
//                each.Suspend = true;
//        }

//        public void ResumeListen()
//        {
//            Listen = true;

//            foreach (IChangeSource each in Sources.Keys)
//                each.Suspend = false;
//        }

//        /// <summary>
//        /// 將目前值的狀態設成「Clean」。
//        /// </summary>
//        public void Reset()
//        {
//            foreach (IChangeSource each in new List<IChangeSource>(Sources.Keys))
//            {
//                each.Reset();
//                Sources[each] = ValueStatus.Clean;
//            }

//            CurrentStatus = ValueStatus.Clean;
//        }
//    }
//}