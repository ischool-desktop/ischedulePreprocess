//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sunset
//{
//    /// <summary>
//    /// 代表資料變化的收聽者。
//    /// </summary>
//    public interface IChangeSource
//    {
//        /// <summary>
//        /// 當值改變時發生。
//        /// </summary>
//        event EventHandler<ChangeEventArgs> StatusChanged;

//        /// <summary>
//        /// 是否停止觸發 StatusChanged 事件。
//        /// </summary>
//        bool Suspend { get; set; }

//        /// <summary>
//        /// 重設值的狀態為 Clean。
//        /// </summary>
//        void Reset();
//    }
//}