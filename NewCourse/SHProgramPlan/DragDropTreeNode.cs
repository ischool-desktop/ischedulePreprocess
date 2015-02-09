using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SmartSchool.Common
{
    //enum KeyStatus:int{滑鼠左鍵=1,滑鼠右鍵=2,SHIFT鍵=4,CTRL鍵=8,滑鼠中鍵=16,ALT鍵=32}
    public class DragDropTreeNode:TreeNode
    {
        public virtual DragDropEffects CheckDragDropEffects(IDataObject Data, int keyStatus)
        {
            return DragDropEffects.None;
        }
        public virtual void DragDrop(IDataObject Data, int keyStatus)
        { }
        public virtual string DragOverMessage
        {
            get { return ""; }
        }
    }
}
