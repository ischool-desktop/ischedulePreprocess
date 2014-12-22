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
    /// 課程班級檢視
    /// </summary>
    public partial class Course_ClassNameView : TreeNavViewBase
    {
        private static Course_ClassNameView _instance;

        /// <summary>
        /// 班級檢視
        /// </summary>
        public static Course_ClassNameView Instnace
        {
            get
            {
                if (_instance == null)
                    _instance = new Course_ClassNameView();
                return _instance;
            }
        }

        public Course_ClassNameView()
            : base()
        {
            InitializeComponent();
            NavText = "班級檢視";
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

            string cmd = string.Format(@"select uid,class_name from $scheduler.scheduler_course_extension where uid in({0})", primarykeys);
            DataTable result = query.Select(cmd);
            root.Clear();
            List<string> InsertKey = new List<string>();
            foreach (DataRow row in result.Rows)
            {
                string courseID = row["uid"].ToString();
                string className = row["class_name"].ToString();

                if (string.IsNullOrWhiteSpace(className))
                {
                    className = "未與班級關連";
                    root["未與班級關連"].Tag = int.MaxValue;
                }

                root[className].AddKey(courseID);
                InsertKey.Add(courseID);
            }
            foreach (string each in Source)
            {
                if (!InsertKey.Contains(each))
                {
                    root["未與班級關連"].AddKey(each);
                    root["未與班級關連"].Tag = int.MaxValue;
                }
            }
        }
    }
}
