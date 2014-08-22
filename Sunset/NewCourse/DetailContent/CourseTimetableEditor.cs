using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Campus.Windows;
using FISCA.Data;
using FISCA.Permission;
using FISCA.UDT;
using FCode = FISCA.Permission.FeatureCodeAttribute;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 排課
    /// </summary>
    [FCode("e562e1a4-8c28-45b5-ae54-aed3acba7ade", "排課")]
    public partial class CourseTimetableEditor : FISCA.Presentation.DetailContent, IContentEditor<SchedulerCourseExtension>
    {
        //權限
        private FeatureAce UserPermission;
        private SchedulerCourseExtension mCourseExtension;
        //狀態變更
        private ChangeListener DataListener;

        public CourseTimetableEditor()
        {
            InitializeComponent();

            Group = "排課";
        }

        private void TimetableEditor_Load(object sender, EventArgs e)
        {
            //設定權限
            UserPermission = FISCA.Permission.UserAcl.Current[FCode.GetCode(GetType())];
            this.Enabled = UserPermission.Editable;



        }

        /// <summary>
        /// 當控制項狀態變更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataListener_StatusChanged(object sender, ChangeEventArgs e)
        {
            SaveButtonVisible = (e.Status == ValueStatus.Dirty);
            CancelButtonVisible = (e.Status == ValueStatus.Dirty);
        }

        public void Prepare()
        {
        }

        /// <summary>
        /// 取得或設定內容
        /// </summary>
        public SchedulerCourseExtension Content
        {
            get
            {
                return mCourseExtension;
            }
            set
            {

            }
        }

        /// <summary>
        /// 傳回本身
        /// </summary>
        public UserControl Control
        {
            get { return this; }
        }
    }
}
