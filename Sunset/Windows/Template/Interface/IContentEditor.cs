
namespace Sunset.Windows
{
    /// <summary>
    /// 內容編輯器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContentEditor<T> : IContentViewer<T>
    {
        /// <summary>
        /// 取得或設定內容
        /// </summary>
        new T Content { get; set; }

        /// <summary>
        /// 是否被修改
        /// </summary>
        bool IsDirty { get; set; }

        /// <summary>
        /// 驗證內容是否合法
        /// </summary>
        /// <returns></returns>
        bool Validate();

        void SetTitle(string name);
    }
}