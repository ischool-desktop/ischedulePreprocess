using System.Drawing;
using System.Windows.Forms;

namespace Sunset.Windows
{
    /// <summary>
    /// 系統設定項目
    /// </summary>
    public interface IConfigurationItem
    {
        /// <summary>
        /// 所屬的設定群組
        /// </summary>
        string TabGroup { get;}
        /// <summary>
        /// 是否有控制頁面
        /// </summary>
        bool HasControlPanel { get;}
        /// <summary>
        /// 控制頁面(當HasControlPanel為true時)
        /// </summary>
        Panel ControlPanel { get;}
        /// <summary>
        /// 內容頁面
        /// </summary>
        Panel ContentPanel { get;}
        /// <summary>
        /// 項目名稱
        /// </summary>
        string Caption { get;}
        /// <summary>
        /// 項目類別
        /// </summary>
        string Category { get;}
        /// <summary>
        /// 照片
        /// </summary>
        Image Image { get;}
        /// <summary>
        /// 當此項目第一次被開啟時
        /// </summary>
        void Active();
    }
}