using System.Windows.Forms;

namespace Sunset.Windows
{
    /// <summary>
    /// 內容檢視器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContentViewer<T>
    {
        /// <summary>
        /// 
        /// </summary>
        void Prepare();

        /// <summary>
        /// 
        /// </summary>
        T Content { set; }

        /// <summary>
        /// 顯示的UserControl
        /// </summary>
        UserControl Control { get; }
    }
}