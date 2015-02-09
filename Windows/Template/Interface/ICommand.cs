using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.Windows
{
    /// <summary>
    /// 代表UI的某個動作
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 命令代碼
        /// </summary>
        string Text { get; }
        
        /// <summary>
        /// 命令名稱
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否有異動資料
        /// </summary>
        bool IsChangeData { get; }

        /// <summary>
        /// 執行此命令
        /// </summary>
        /// <returns>回傳執行結果</returns>
        string Execute(object Context);
    }
}