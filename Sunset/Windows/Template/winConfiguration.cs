
namespace Sunset.Windows
{
    /// <summary>
    /// 新增、修改、刪除及匯入匯出表單
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class winConfiguration<T>
    {
        private IConfigurationDataAccess<T> mDataAccess;
        private IContentEditor<T> mEditor;

        /// <summary>
        /// 建構式，傳入資料存取及資料編輯介面即可運作
        /// </summary>
        /// <param name="vDataAccess"></param>
        /// <param name="vEditor"></param>
        public winConfiguration(IConfigurationDataAccess<T> vDataAccess,IContentEditor<T> vEditor)
        {
            mDataAccess = vDataAccess;
            mEditor = vEditor;    
        }

        /// <summary>
        /// 顯示表單
        /// </summary>
        public void ShowDialog()
        {
            mEditor.IsDirty = false;

            CampusConfiguration<T> vConfiguration = new CampusConfiguration<T>(mDataAccess,mEditor);

            ConfigurationForm vForm = (new ConfigurationForm(vConfiguration));

            vForm.ShowDialog();
        }
    }
}