using System.Collections.Generic;
using System.Data;
using System.Text;
using Campus;
using FISCA.Data;
using SunsetCore;

namespace Sunset.NewCourse
{
    public partial class Course_TeacherNameView : TreeNavViewBase
    {
        private static Course_TeacherNameView _instance;

        /// <summary>
        /// 
        /// </summary>
        public static Course_TeacherNameView Instnace
        {
            get
            {
                if (_instance == null)
                    _instance = new Course_TeacherNameView();
                return _instance;
            }
        }

        public Course_TeacherNameView()
            : base()
        {
            InitializeComponent();
            NavText = "教師檢視";
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

            string cmd = string.Format(@"select uid,teacher_name_1 from $scheduler.scheduler_course_extension where uid in({0})", primarykeys);
            DataTable result = query.Select(cmd);
            root.Clear();

            List<string> InsertKey = new List<string>();
            foreach (DataRow row in result.Rows)
            {                
                string courseID = row["uid"].ToString();
                //授課教師名稱
                string teacherName = row["teacher_name_1"].ToString();

                if (string.IsNullOrWhiteSpace(teacherName))
                {
                    teacherName = "無課程評分導師";
                    root["無課程評分導師"].Tag = int.MaxValue;
                }

                root[teacherName].AddKey(courseID);
                InsertKey.Add(courseID);
            }

            foreach (string each in Source)
            {
                if (!InsertKey.Contains(each))
                {
                    root["無課程評分導師"].AddKey(each);
                    root["無課程評分導師"].Tag = int.MaxValue;
                }
            }
        }
    }
}
