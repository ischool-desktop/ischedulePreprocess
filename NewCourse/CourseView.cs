using System;
using System.Windows.Forms;
using FISCA.Presentation;

namespace Sunset.NewCourse
{
    public partial class CourseView : NavView
    {
        public CourseView()
        {
            InitializeComponent();

            NavText = "排課課程檢視";
 
            SourceChanged += new EventHandler(ExtracurricularActivitiesView_SourceChanged);
        }

        void ExtracurricularActivitiesView_SourceChanged(object sender, EventArgs e)
        {                        
            advTree1.Nodes.Clear();

            DevComponents.AdvTree.Node Node1 = new DevComponents.AdvTree.Node();
            Node1.Text = "所有課程(" + Source.Count.ToString() + ")";
           
            advTree1.Nodes.Add(Node1);
            advTree1.SelectedNode = Node1;

            SetListPaneSource(Source, false, false);
        }

        private void advTree1_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            //判斷是否有按Control,Shift
            bool SelectedAll = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            bool AddToTemp = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            //傳入ID
            SetListPaneSource(Source, SelectedAll, AddToTemp);            
        }
    }
}
