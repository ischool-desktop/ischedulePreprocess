using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Campus;
using FISCA.Data;
using SunsetCore;

namespace Sunset.NewCourse
{
    /// <summary>
    /// 課程科目檢視
    /// </summary>
    public partial class Course_SubjectView : TreeNavViewBase
    {
        private static Course_SubjectView _instance;

        /// <summary>
        /// 
        /// </summary>
        public static Course_SubjectView Instnace
        {
            get
            {
                if (_instance == null)
                    _instance = new Course_SubjectView();
                return _instance;
            }
        }

        public Course_SubjectView()
            : base()
        {
            InitializeComponent();
            NavText = "科目檢視";
        }

        private CustomStringComparer NameComparer = new CustomStringComparer();

        protected override int KeyCatalogComparer(KeyCatalog x, KeyCatalog y)
        {
            int xtag = 1;
            int.TryParse("" + x.Tag, out xtag);

            int ytag = 1;
            int.TryParse("" + y.Tag, out ytag);

            string xnametag = xtag + x.Name;
            string ynametag = ytag + y.Name;
            return NameComparer.Compare(xnametag, ynametag);
        }

        protected override void GenerateTreeStruct(KeyCatalog root)
        {
            StringBuilder primarykeys = new StringBuilder();
            primarykeys.AppendFormat("{0}", "-1"); //如果沒有資料也不會爆掉。
            foreach (string key in Source)
                primarykeys.AppendFormat(",{0}", key);

            QueryHelper query = new QueryHelper();

            string cmd = string.Format(@"select uid,subject from $scheduler.scheduler_course_extension where uid in({0})", primarykeys);
            DataTable result = query.Select(cmd);
            root.Clear();
            foreach (DataRow row in result.Rows)
            {
                //科目名稱
                string subjectName = row["subject"].ToString();
                string courseID = row["uid"].ToString();

                if (string.IsNullOrWhiteSpace(subjectName))
                {
                    subjectName = "無科目名稱";
                    root["無科目名稱"].Tag = int.MaxValue;
                }

                root[subjectName].AddKey(courseID);
            }
        }
    }
}
