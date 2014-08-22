using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace SmartSchool
{
    public interface IEntity
    {
        string Title { get;}
        NavigationPanePanel NavPanPanel{get;}
        Panel ContentPanel { get;}
        Image Picture { get;}
        void Actived();
    }
}
