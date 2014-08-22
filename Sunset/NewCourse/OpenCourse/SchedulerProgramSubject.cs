//using System;
//using System.Collections.Generic;
//using System.Xml;
//using K12.Data;

//namespace Sunset
//{
//    /// <summary>
//    /// 課程規劃資訊
//    /// </summary>
//    public class SchedulerProgramPlanRecord
//    {
//        /// <summary>
//        /// 系統編號
//        /// </summary>
//        public string ID { get; set; }
//        /// <summary>
//        /// 名稱，必填
//        /// </summary>
//        public string Name { get; set; }
//        /// <summary>
//        /// 課程規劃科目列表
//        /// </summary>
//        public List<SchedulerProgramSubject> Subjects { get; set; }

//        /// <summary>
//        /// 預設建構式，將ID及Name設為空字串，並且初始化Subjects。
//        /// </summary>
//        public SchedulerProgramPlanRecord()
//        {
//            ID = string.Empty;
//            Name = string.Empty;
//            Subjects = new List<SchedulerProgramSubject>();
//        }

//        /// <summary>
//        /// 新增課程規劃記錄建構式，參數為新增記錄的必填欄位
//        /// </summary>
//        /// <param name="Name">名稱</param>
//        public SchedulerProgramPlanRecord(string Name)
//            : this()
//        {
//            this.Name = Name;
//        }

//        /// <summary>
//        /// XML參數建構式
//        /// <![CDATA[
//        /// ]]>
//        /// </summary>
//        /// <param name="data"></param>
//        public SchedulerProgramPlanRecord(XmlElement data)
//        {
//            Load(data);
//        }

//        /// <summary>
//        /// 從XML載入設定值
//        /// <![CDATA[
//        /// ]]>
//        /// </summary>
//        /// <param name="data"></param>
//        public void Load(XmlElement data)
//        {
//            XmlHelper helper = new XmlHelper(data);
//            ID = helper.GetString("@ID");
//            Name = helper.GetString("Name");

//            List<SchedulerProgramSubject> list = new List<SchedulerProgramSubject>();
//            foreach (var subject in helper.GetElements("Content/GraduationPlan/Subject"))
//            {
//                subject.SetAttribute("ID", ID);
//                subject.SetAttribute("Name", Name);
//                list.Add(new SchedulerProgramSubject(subject));
//            }
//            Subjects = list;
//        }
//    }

//    /// <summary>
//    /// 課程規劃科目記錄
//    /// </summary>
//    public class SchedulerProgramSubject : ICloneable
//    {
//        /// <summary>
//        /// 系統編號，唯讀屬性，若需設定請使用ProgramPlan的ID屬性。
//        /// </summary>
//        [Field(Caption = "編號", EntityName = "ProgramPlan", EntityCaption = "課程規劃", IsEntityPrimaryKey = true)]
//        public string ID { get; private set; }
//        /// <summary>
//        /// 名稱，必填；在此為唯讀屬性，若需設定請使用ProgramPlan的Name屬性。
//        /// </summary>
//        [Field(Caption = "名稱", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
//        public string Name { get; private set; }
//        /// <summary>
//        /// 年級
//        /// </summary>
//        [Field(Caption = "年級", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
//        public int GradeYear { get; set; }
//        /// <summary>
//        /// 學期
//        /// </summary>
//        [Field(Caption = "學期", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
//        public int Semester { get; set; }
//        /// <summary>
//        /// 學分數
//        /// </summary>
//        [Field(Caption = "學分數", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
//        public decimal? Credit { get; set; }
//        /// <summary>
//        /// 完整名稱
//        /// </summary>
//        [Field(Caption = "完整名稱", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
//        public string FullName { get; set; }
//        /// <summary>
//        /// 科目名稱
//        /// </summary>
//        [Field(Caption = "名稱", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
//        public string SubjectName { get; set; }
//        /// <summary>
//        /// 介面索引
//        /// </summary>
//        [Field(Caption = "介面索引", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
//        public int RowIndex { get; set; } //這是…
//        //國中專用屬性
//        /// <summary>
//        /// 是否計算成績，國中專屬
//        /// </summary>
//        [Field(Caption = "是否計算成績", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "國中專屬")]
//        public bool CalcFlag { get; set; }
//        /// <summary>
//        /// 上課時段，國中專屬
//        /// </summary>
//        [Field(Caption = "上課時段", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "國中專屬")]
//        public decimal? Period { get; set; }
//        /// <summary>
//        /// 所屬領域，國中專屬
//        /// </summary>
//        [Field(Caption = "所屬領域", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "國中專屬")]
//        public string Domain { get; set; }
//        //高中專用屬性
//        /// <summary>
//        /// 分類，高中專屬
//        /// </summary>
//        [Field(Caption = "分類", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
//        public string Category { get; set; }
//        /// <summary>
//        /// 分項，高中專屬
//        /// </summary>
//        [Field(Caption = "分項", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
//        public string Entry { get; set; }
//        /// <summary>
//        /// 級別，高中專屬
//        /// </summary>
//        [Field(Caption = "級別", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
//        public int? Level { get; set; }
//        /// <summary>
//        /// 不需評分，若資料庫值為空白，預設為false，亦即計入，高中專屬
//        /// </summary>
//        [Field(Caption = "不需評分", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
//        public bool NotIncludedInCalc { get; set; }
//        /// <summary>
//        /// 不計學分，若資料庫值為空白，預設為false，亦即計入學分，高中專屬
//        /// </summary>
//        [Field(Caption = "不計學分", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
//        public bool NotIncludedInCredit { get; set; }
//        /// <summary>
//        /// 校部訂，高中專屬
//        /// </summary>
//        [Field(Caption = "校部訂", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
//        public string RequiredBy { get; set; }
//        /// <summary>
//        /// 必選修，高中專屬
//        /// </summary>
//        [Field(Caption = "必選修", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
//        public bool Required { get; set; }
//        /// <summary>
//        /// 開始級別，高中專屬
//        /// </summary>
//        [Field(Caption = "開始級別", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
//        public int? StartLevel { get; set; }

//        /// <summary>
//        /// XML參數建構式
//        /// <![CDATA[
//        /// ]]>
//        /// </summary>
//        /// <param name="element"></param>
//        public SchedulerProgramSubject(XmlElement element)
//        {
//            ID = element.GetAttribute("ID");
//            Name = element.GetAttribute("Name");
//            GradeYear = K12.Data.Int.Parse(element.GetAttribute("GradeYear"));
//            Semester = K12.Data.Int.Parse(element.GetAttribute("Semester"));
//            Credit = K12.Data.Decimal.ParseAllowNull(element.GetAttribute("Credit"));
//            Period = K12.Data.Decimal.ParseAllowNull(element.GetAttribute("Period"));
//            Domain = element.GetAttribute("Domain");
//            FullName = element.GetAttribute("FullName");
//            SubjectName = element.GetAttribute("SubjectName");
//            bool b;
//            CalcFlag = bool.TryParse(element.GetAttribute("CalcFlag"), out b) ? b : true;

//            //高中專屬屬性：
//            XmlElement GroupElement = element.SelectSingleNode("Grouping") as XmlElement;

//            if (GroupElement != null)
//            {
//                RowIndex = K12.Data.Int.Parse(GroupElement.GetAttribute("RowIndex"));
//                StartLevel = K12.Data.Int.ParseAllowNull(GroupElement.GetAttribute("startLevel"));
//            }

//            NotIncludedInCalc = bool.TryParse(element.GetAttribute("NotIncludedInCalc"), out b) ? b : false;
//            NotIncludedInCredit = bool.TryParse(element.GetAttribute("NotIncludedInCredit"), out b) ? b : false;
//            RequiredBy = element.GetAttribute("RequiredBy");
//            Required = element.GetAttribute("Required").StartsWith("必");
//            Category = element.GetAttribute("Category");
//            Entry = element.GetAttribute("Entry");
//            Level = K12.Data.Int.ParseAllowNull(element.GetAttribute("Level"));

//            #region XML規格
//            //<Subject 
//            //  Category="一般科目" 
//            //  Credit="1" 
//            //  Domain="外國語文"
//            //  Entry=""
//            //  FullName="生活美語Ⅰ"
//            //  GradeYear="1"
//            //  Level="1" 
//            //  NotIncludedInCalc="false"
//            //  NotIncludedInCredit="false" 
//            //  Required="選修"
//            //  RequiredBy="校訂"
//            //  Semester="1"
//            //  SubjectName="生活美語">
//            //<Grouping RowIndex="1" startLevel="1"/>
//            //</Subject>
//            #endregion
//        }

//        /// <summary>
//        /// 預設建構式
//        /// </summary>
//        public SchedulerProgramSubject()
//        {
//            NotIncludedInCalc = false;
//            NotIncludedInCredit = false;
//            CalcFlag = true;
//        }

//        #region ICloneable 成員

//        /// <summary>
//        /// 複製課程規劃科目物件
//        /// </summary>
//        /// <returns></returns>
//        public object Clone()
//        {
//            SchedulerProgramSubject newSubject = new SchedulerProgramSubject();
//            newSubject.ID = this.ID;
//            newSubject.Name = this.Name;
//            newSubject.GradeYear = this.GradeYear;
//            newSubject.Semester = this.Semester;
//            newSubject.Credit = this.Credit;
//            newSubject.Period = this.Period;
//            newSubject.Domain = this.Domain;
//            newSubject.FullName = this.FullName;
//            newSubject.Level = this.Level;
//            newSubject.SubjectName = this.SubjectName;
//            newSubject.CalcFlag = this.CalcFlag;
//            newSubject.RowIndex = this.RowIndex;
//            return newSubject;
//        }
//        #endregion
//    }
//}