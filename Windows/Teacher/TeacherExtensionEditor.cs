using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Campus.Windows;
using DevComponents.DotNetBar.Controls;
using FISCA.Permission;
using FISCA.UDT;
using FCode = FISCA.Permission.FeatureCodeAttribute;

namespace Sunset
{
    /// <summary>
    /// 排課課程資料
    /// </summary>
    [FCode("Sunset.Detail0040", "教師排課資料")]
    public class TeacherExtensionEditor : FISCA.Presentation.DetailContent, IContentEditor<TeacherExtension>
    {
        private FeatureAce UserPermission;
        private BackgroundWorker mbkwTeacherExtension = new BackgroundWorker();
        private AccessHelper mAccessHelper = new AccessHelper();
        private bool IsBusy = false;
        private ChangeListener DataListener;
        private TeacherExtension mTeacherExtension;

        #region Components

        private ErrorProvider errExtraLength;
        private IContainer components;
        private TextBoxX txtComment;
        private DevComponents.DotNetBar.LabelX labelX7;
        private ErrorProvider errBasicLength;
        private ErrorProvider errCounselingLength;
        private TextBoxX txtCounselingLength;
        private DevComponents.DotNetBar.LabelX labelX4;
        private TextBoxX txtExtraLength;
        private DevComponents.DotNetBar.LabelX labelX3;
        private TextBoxX txtBasicLength;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX lblGroupName;
        #endregion

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public TeacherExtensionEditor()
        {
            InitializeComponent();

            //設定資料項目名稱
            Group = "教師排課資料";
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

        #region BackgroundWorker
        private void mbkwCourseExtension_DoWork(object sender, DoWorkEventArgs e)
        {
            if (string.IsNullOrEmpty(this.PrimaryKey)) return;

            mTeacherExtension = null;

            string strTeacherID = this.PrimaryKey;

            string strCondition = string.Format("ref_teacher_id={0}", strTeacherID);

            #region 依課程系統編號取得課程排課資料，若不存在則新增
            List<TeacherExtension> mTeacherExtensions = mAccessHelper.Select<TeacherExtension>(strCondition);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(mTeacherExtensions))
                mTeacherExtension = mTeacherExtensions[0];
            else
            {
                #region 若課程排課資料不存在則新增後再取得
                mTeacherExtension = new TeacherExtension();
                mTeacherExtension.TeacherID = K12.Data.Int.Parse(this.PrimaryKey);
                mTeacherExtension.BasicLength = null;
                mTeacherExtension.ExtraLength = null;
                mTeacherExtension.CounselingLength = null;

                List<TeacherExtension> InsertRecords = new List<TeacherExtension>();
                InsertRecords.Add(mTeacherExtension);

                List<string> NewIDs = mAccessHelper
                    .InsertValues(InsertRecords);

                if (NewIDs.Count > 0)
                {
                    List<TeacherExtension> NewTeacherExtensions = mAccessHelper
                        .Select<TeacherExtension>("UID="+NewIDs[0]);
                    if (NewTeacherExtensions.Count > 0)
                        mTeacherExtension = NewTeacherExtensions[0];
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 取得資料完成時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbkwCourseExtension_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //若為忙碌則重新執行
            if (IsBusy)
            {
                IsBusy = false;
                mbkwTeacherExtension.RunWorkerAsync();
                return;
            }

            //準備畫面上的資料
            Prepare();

            //設定排課課程資料
            Content = mTeacherExtension;

            this.Loading = false;
        }
        #endregion


        #region DetailContent相關程式碼
        /// <summary>
        /// 當選取教師改變時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            //若教師系統編號不空白才執行
            if (!string.IsNullOrEmpty(this.PrimaryKey))
            {
                this.Loading = true;

                if (mbkwTeacherExtension.IsBusy) //如果是忙碌的
                    IsBusy = true; //為True
                else
                    mbkwTeacherExtension.RunWorkerAsync(); //否則直接執行
            }
        }

        /// <summary>
        /// 當按下儲存按鈕時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSaveButtonClick(EventArgs e)
        {
            if (!string.IsNullOrEmpty(errExtraLength.GetError(txtExtraLength)) || 
                !string.IsNullOrEmpty(errBasicLength.GetError(txtBasicLength)) ||
                !string.IsNullOrEmpty(errCounselingLength.GetError(txtCounselingLength)))
            {
                MessageBox.Show("輸入資料有誤，無法儲存!");

                return;
            }

            StringBuilder strBuilder = new StringBuilder();

            TeacherExtension TeacherExtension = Content;

            if (TeacherExtension!=null)
            {
                List<TeacherExtension> TeacherExtensions = new List<TeacherExtension>();
                TeacherExtensions.Add(TeacherExtension);

                mAccessHelper.SaveAll(TeacherExtensions);
                strBuilder.AppendLine("已儲存教師排課資料");
            }

            SaveButtonVisible = false;
            CancelButtonVisible = false;

            this.Loading = true;

            DataListener.SuspendListen();         //終止變更判斷
            mbkwTeacherExtension.RunWorkerAsync(); //背景作業,取得並重新填入原資料

            FISCA.Presentation.MotherForm.SetStatusBarMessage(strBuilder.ToString());
        }

        /// <summary>
        /// 當按下取消按鈕時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCancelButtonClick(EventArgs e)
        {
            SaveButtonVisible = false;
            CancelButtonVisible = false;

            this.Loading = true;

            DataListener.SuspendListen(); //終止變更判斷

            if (!mbkwTeacherExtension.IsBusy)
                mbkwTeacherExtension.RunWorkerAsync();
        }

        /// <summary>
        /// 儲存按鈕是否可見
        /// </summary>
        public new bool SaveButtonVisible
        {
            get { return base.SaveButtonVisible; }
            set
            {
                //判斷權限
                if (Attribute.IsDefined(GetType(), typeof(FeatureCodeAttribute)))
                {
                    FeatureCodeAttribute fca = Attribute.GetCustomAttribute(GetType(), typeof(FeatureCodeAttribute)) as FeatureCodeAttribute;
                    if (fca != null)
                        if (FISCA.Permission.UserAcl.Current[fca.Code].Editable)
                            base.SaveButtonVisible = value;
                }
                else //沒有定義權限就按照平常方法處理。
                    base.SaveButtonVisible = value;
            }
        }

        /// <summary>
        /// 取消按鈕是否可見
        /// </summary>
        public new bool CancelButtonVisible
        {
            get { return base.CancelButtonVisible; }
            set
            {
                //判斷權限
                if (Attribute.IsDefined(GetType(), typeof(FeatureCodeAttribute)))
                {
                    FeatureCodeAttribute fca = Attribute.GetCustomAttribute(GetType(), typeof(FeatureCodeAttribute)) as FeatureCodeAttribute;

                    if (fca != null)
                        if (FISCA.Permission.UserAcl.Current[fca.Code].Editable)
                            base.CancelButtonVisible = value;
                }
                else //沒有定義權限就按照平常方法處理。
                    base.CancelButtonVisible = value;
            }
        }
        #endregion

        #region IContentEditor<CourseExtension> 成員
        /// <summary>
        /// 準備資料
        /// </summary>
        public void Prepare()
        {

        }

        /// <summary>
        /// 取得或設定內容
        /// </summary>
        public TeacherExtension Content
        {
            get
            {
                #region Step2:指定課程相關屬性
                mTeacherExtension.Comment = txtComment.Text;
                mTeacherExtension.BasicLength = K12.Data.Int.ParseAllowNull(txtBasicLength.Text);
                mTeacherExtension.ExtraLength = K12.Data.Int.ParseAllowNull(txtExtraLength.Text);
                mTeacherExtension.CounselingLength = K12.Data.Int.ParseAllowNull(txtCounselingLength.Text);
                #endregion

                return mTeacherExtension;
            }
            set
            {
                txtComment.Text = value.Comment;
                txtBasicLength.Text = K12.Data.Int.GetString(value.BasicLength);
                txtExtraLength.Text = K12.Data.Int.GetString(value.ExtraLength);
                txtCounselingLength.Text = K12.Data.Int.GetString(value.CounselingLength);

                #region Step4:設定介面
                SaveButtonVisible = false;
                CancelButtonVisible = false;

                DataListener.Reset();
                DataListener.ResumeListen();
                #endregion
            }
        }

        /// <summary>
        /// 傳回本身
        /// </summary>
        public UserControl Control
        {
            get { return this; }
        }
        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errExtraLength = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtComment = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.errBasicLength = new System.Windows.Forms.ErrorProvider(this.components);
            this.errCounselingLength = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblGroupName = new DevComponents.DotNetBar.LabelX();
            this.txtBasicLength = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtExtraLength = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtCounselingLength = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.errExtraLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errBasicLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errCounselingLength)).BeginInit();
            this.SuspendLayout();
            // 
            // errExtraLength
            // 
            this.errExtraLength.ContainerControl = this;
            // 
            // txtComment
            // 
            // 
            // 
            // 
            this.txtComment.Border.Class = "TextBoxBorder";
            this.txtComment.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtComment.Location = new System.Drawing.Point(87, 47);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(436, 25);
            this.txtComment.TabIndex = 8;
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(11, 51);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(74, 21);
            this.labelX7.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX7.TabIndex = 7;
            this.labelX7.Text = "教師註記：";
            // 
            // errBasicLength
            // 
            this.errBasicLength.ContainerControl = this;
            // 
            // errCounselingLength
            // 
            this.errCounselingLength.ContainerControl = this;
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            // 
            // 
            // 
            this.lblGroupName.BackgroundStyle.Class = "";
            this.lblGroupName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblGroupName.Location = new System.Drawing.Point(306, 54);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(0, 0);
            this.lblGroupName.TabIndex = 14;
            // 
            // txtBasicLength
            // 
            // 
            // 
            // 
            this.txtBasicLength.Border.Class = "TextBoxBorder";
            this.txtBasicLength.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtBasicLength.Location = new System.Drawing.Point(87, 8);
            this.txtBasicLength.Name = "txtBasicLength";
            this.txtBasicLength.Size = new System.Drawing.Size(80, 25);
            this.txtBasicLength.TabIndex = 16;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(11, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX1.TabIndex = 15;
            this.labelX1.Text = "基本時數：";
            // 
            // txtExtraLength
            // 
            // 
            // 
            // 
            this.txtExtraLength.Border.Class = "TextBoxBorder";
            this.txtExtraLength.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtExtraLength.Location = new System.Drawing.Point(265, 8);
            this.txtExtraLength.Name = "txtExtraLength";
            this.txtExtraLength.Size = new System.Drawing.Size(80, 25);
            this.txtExtraLength.TabIndex = 18;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(197, 11);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(74, 21);
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX3.TabIndex = 17;
            this.labelX3.Text = "兼課時數：";
            // 
            // txtCounselingLength
            // 
            // 
            // 
            // 
            this.txtCounselingLength.Border.Class = "TextBoxBorder";
            this.txtCounselingLength.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCounselingLength.Location = new System.Drawing.Point(443, 8);
            this.txtCounselingLength.Name = "txtCounselingLength";
            this.txtCounselingLength.Size = new System.Drawing.Size(80, 25);
            this.txtCounselingLength.TabIndex = 20;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(374, 9);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(74, 21);
            this.labelX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.labelX4.TabIndex = 19;
            this.labelX4.Text = "輔導時數：";
            // 
            // TeacherExtensionEditor
            // 
            this.Controls.Add(this.txtCounselingLength);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.txtExtraLength);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.txtBasicLength);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.lblGroupName);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.labelX7);
            this.Name = "TeacherExtensionEditor";
            this.Size = new System.Drawing.Size(550, 80);
            this.Load += new System.EventHandler(this.CourseExtensionEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errExtraLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errBasicLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errCounselingLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// 資料項目載入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourseExtensionEditor_Load(object sender, EventArgs e)
        {
            //設定權限
            UserPermission = FISCA.Permission.UserAcl.Current[FCode.GetCode(GetType())];
            this.Enabled = UserPermission.Editable;

            int a;

            txtBasicLength.TextChanged += (vsender, ve) =>
            {
                if ((!string.IsNullOrEmpty(txtBasicLength.Text)) && (!int.TryParse(txtBasicLength.Text, out a)))
                    errBasicLength.SetError(txtBasicLength, "只能輸入數字！");
                else
                    errBasicLength.Clear();
            };

            txtCounselingLength.TextChanged += (vsender,ve)=>
            {
                if ((!string.IsNullOrEmpty(txtBasicLength.Text)) && (!int.TryParse(txtCounselingLength.Text, out a)))
                    errCounselingLength.SetError(txtCounselingLength, "只能輸入數字！");
                else
                    errCounselingLength.Clear();
            };

            txtExtraLength.TextChanged += (vsender, ve) =>
            {
                if ((!string.IsNullOrEmpty(txtBasicLength.Text)) && (!int.TryParse(txtExtraLength.Text, out a)))
                    errExtraLength.SetError(txtExtraLength, "只能輸入數字！");
                else
                    errExtraLength.Clear();
            };

            //取得課程排課資料
            mbkwTeacherExtension.DoWork += new DoWorkEventHandler(mbkwCourseExtension_DoWork);

            //完成取得課程排課資料
            mbkwTeacherExtension.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mbkwCourseExtension_RunWorkerCompleted);

            //判斷DataGridView狀態變更
            DataListener = new ChangeListener();

            //加入TextBox狀態改變通知
            DataListener.Add(new TextBoxSource(txtBasicLength));
            DataListener.Add(new TextBoxSource(txtExtraLength));
            DataListener.Add(new TextBoxSource(txtCounselingLength));
            DataListener.Add(new TextBoxSource(txtComment));

            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(DataListener_StatusChanged);

            //終止變更判斷
            DataListener.SuspendListen();
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            this.SaveButtonVisible = true;
            this.CancelButtonVisible = true;
        }
    }
}