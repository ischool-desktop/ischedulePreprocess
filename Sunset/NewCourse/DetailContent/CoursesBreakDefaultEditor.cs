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
    /// 課程分段預設值
    /// </summary>
    [FCode("19e95cbd-b591-4825-8616-07ce9e463d0b", "課程分段預設值")]
    public partial class CoursesBreakDefaultEditor : FISCA.Presentation.DetailContent, IContentEditor<SchedulerCourseExtension>
    {
        //權限
        private FeatureAce UserPermission;
        private SchedulerCourseExtension mCourseExtension;
        //狀態變更
        private ChangeListener DataListener;

        public CoursesBreakDefaultEditor()
        {
            InitializeComponent();

            Group = "課程分段預設值";
        }

        private void CoursesBreakDefaultEditor_Load(object sender, EventArgs e)
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            TextInputeForm htf = new TextInputeForm(sb);
            htf.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            TextInputeForm htf = new TextInputeForm(sb);
            htf.ShowDialog();
        }
    }
}
