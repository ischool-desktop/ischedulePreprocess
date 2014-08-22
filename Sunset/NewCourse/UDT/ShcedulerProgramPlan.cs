using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data;

namespace Sunset.NewCourse
{

    /// <summary>
    /// 排課專用課程規劃
    /// </summary>
    [FISCA.UDT.TableName("scheduler.scheduler_program_plan")]
    public class SchedulerProgramPlan : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [FISCA.UDT.Field(Field = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 節數
        /// </summary>
        [FISCA.UDT.Field(Field = "content")]
        public string Content 
        {
            get 
            {
                return ConvertSubjectsToContent();
            }
            set
            {
                ConvertContentToSubjects(value);
            }
        }

        /// <summary>
        /// 課程規劃科目列表
        /// </summary>
        public List<ProgramSubject> Subjects { get; set; }

        private void ConvertContentToSubjects(string value)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(value);

            Subjects.Clear();

            foreach (XmlNode Node in xmldoc.DocumentElement.SelectNodes("Subject"))
                Subjects.Add(new ProgramSubject(Node as XmlElement));
        }

        private string ConvertSubjectsToContent()
        {
            DSXmlHelper helper = new DSXmlHelper("GraduationPlan");

            foreach (var subject in Subjects)
            {
                string strXML = subject.ToXml().OuterXml;

                helper.AddXmlString(".", strXML);

                //XmlElement element = null;

                //element = helper.AddElement("Subject");

                //element.SetAttribute("GradeYear", "" + subject.GradeYear);
                //element.SetAttribute("Semester", "" + subject.Semester);
                //element.SetAttribute("Credit", K12.Data.Decimal.GetString(subject.Credit));
                //element.SetAttribute("Period", K12.Data.Decimal.GetString(subject.Period));
                //element.SetAttribute("Domain", subject.Domain);
                //element.SetAttribute("FullName", subject.FullName);
                //element.SetAttribute("Level", K12.Data.Int.GetString(subject.Level));
                //element.SetAttribute("CalcFlag", "" + subject.CalcFlag);
                //element.SetAttribute("SubjectName", subject.SubjectName);

                //element = helper.AddElement("Subject", "Grouping");
                //element.SetAttribute("RowIndex", "" + subject.RowIndex);
            }

            string result = helper.BaseElement.OuterXml;

            return result;
        }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public SchedulerProgramPlan()
        {
            Subjects = new List<ProgramSubject>();
        }    
    }

    /// <summary>
    /// 課程規劃科目記錄
    /// </summary>
    public class ProgramSubject : ICloneable
    {
        /// <summary>
        /// 系統編號，唯讀屬性，若需設定請使用ProgramPlan的ID屬性。
        /// </summary>
        [Field(Caption = "編號", EntityName = "ProgramPlan", EntityCaption = "課程規劃", IsEntityPrimaryKey = true)]
        public string ID { get; private set; }
        /// <summary>
        /// 名稱，必填；在此為唯讀屬性，若需設定請使用ProgramPlan的Name屬性。
        /// </summary>
        [Field(Caption = "名稱", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
        public string Name { get; private set; }
        /// <summary>
        /// 年級
        /// </summary>
        [Field(Caption = "年級", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
        public int GradeYear { get; set; }
        /// <summary>
        /// 學期
        /// </summary>
        [Field(Caption = "學期", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
        public int Semester { get; set; }
        /// <summary>
        /// 學分數
        /// </summary>
        [Field(Caption = "學分數", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
        public decimal? Credit { get; set; }
        /// <summary>
        /// 完整名稱
        /// </summary>
        [Field(Caption = "完整名稱", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
        public string FullName { get; set; }
        /// <summary>
        /// 科目名稱
        /// </summary>
        [Field(Caption = "名稱", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
        public string SubjectName { get; set; }
        /// <summary>
        /// 介面索引
        /// </summary>
        [Field(Caption = "介面索引", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
        public int RowIndex { get; set; } //這是…
        //國中專用屬性
        /// <summary>
        /// 是否計算成績，國中專屬
        /// </summary>
        [Field(Caption = "是否計算成績", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "國中專屬")]
        public bool CalcFlag { get; set; }
        /// <summary>
        /// 上課時段，國中專屬
        /// </summary>
        [Field(Caption = "上課時段", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "國中專屬")]
        public decimal? Period { get; set; }
        /// <summary>
        /// 所屬領域，國中專屬
        /// </summary>
        [Field(Caption = "所屬領域", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "國中專屬")]
        public string Domain { get; set; }
        //高中專用屬性
        /// <summary>
        /// 分類，高中專屬
        /// </summary>
        [Field(Caption = "分類", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目")]
        public string Category { get; set; }
        /// <summary>
        /// 分項，高中專屬
        /// </summary>
        [Field(Caption = "分項", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
        public string Entry { get; set; }
        /// <summary>
        /// 級別，高中專屬
        /// </summary>
        [Field(Caption = "級別", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
        public int? Level { get; set; }
        /// <summary>
        /// 不需評分，若資料庫值為空白，預設為false，亦即計入，高中專屬
        /// </summary>
        [Field(Caption = "不需評分", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
        public bool NotIncludedInCalc { get; set; }
        /// <summary>
        /// 不計學分，若資料庫值為空白，預設為false，亦即計入學分，高中專屬
        /// </summary>
        [Field(Caption = "不計學分", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
        public bool NotIncludedInCredit { get; set; }
        /// <summary>
        /// 校部訂，高中專屬
        /// </summary>
        [Field(Caption = "校部訂", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
        public string RequiredBy { get; set; }
        /// <summary>
        /// 必選修，高中專屬
        /// </summary>
        [Field(Caption = "必選修", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
        public bool Required { get; set; }
        /// <summary>
        /// 開始級別，高中專屬
        /// </summary>
        [Field(Caption = "開始級別", EntityName = "ProgramSubject", EntityCaption = "課程規劃科目", Remark = "高中專屬")]
        public int? StartLevel { get; set; }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public ProgramSubject(XmlElement element)
        {
            Name = element.GetAttribute("Name");
            GradeYear = K12.Data.Int.Parse(element.GetAttribute("GradeYear"));
            Semester = K12.Data.Int.Parse(element.GetAttribute("Semester"));
            Credit = K12.Data.Decimal.ParseAllowNull(element.GetAttribute("Credit"));
            Period = K12.Data.Decimal.ParseAllowNull(element.GetAttribute("Period"));
            Domain = element.GetAttribute("Domain");
            FullName = element.GetAttribute("FullName");
            SubjectName = element.GetAttribute("SubjectName");
            bool b;
            CalcFlag = bool.TryParse(element.GetAttribute("CalcFlag"), out b) ? b : true;

            //高中專屬屬性：
            XmlElement elmGroup = element.SelectSingleNode("Grouping") as XmlElement;

            if (elmGroup != null)
            {
                RowIndex = K12.Data.Int.Parse(elmGroup.GetAttribute("RowIndex"));
                StartLevel = K12.Data.Int.ParseAllowNull(elmGroup.GetAttribute("startLevel"));
            }

            NotIncludedInCalc = bool.TryParse(element.GetAttribute("NotIncludedInCalc"), out b) ? b : false;
            NotIncludedInCredit = bool.TryParse(element.GetAttribute("NotIncludedInCredit"), out b) ? b : false;
            RequiredBy = element.GetAttribute("RequiredBy");
            Required = element.GetAttribute("Required").StartsWith("必");
            Category = element.GetAttribute("Category");
            Entry = element.GetAttribute("Entry");
            Level = K12.Data.Int.ParseAllowNull(element.GetAttribute("Level"));

            #region XML規格
            //<Subject 
            //  Category="一般科目" 
            //  Credit="1" 
            //  Domain="外國語文"
            //  Entry=""
            //  FullName="生活美語Ⅰ"
            //  GradeYear="1"
            //  Level="1" 
            //  NotIncludedInCalc="false"
            //  NotIncludedInCredit="false" 
            //  Required="選修"
            //  RequiredBy="校訂"
            //  Semester="1"
            //  SubjectName="生活美語">
            //<Grouping RowIndex="1" startLevel="1"/>
            //</Subject>
            #endregion
        }

        /// <summary>
        /// 輸出成XML格式
        /// </summary>
        /// <returns></returns>
        public XmlElement ToXml()
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlElement element = xmldoc.CreateElement("Subject");

            element.SetAttribute("ID",  ID);
            element.SetAttribute("Name", Name);
            element.SetAttribute("GradeYear", K12.Data.Int.GetString(GradeYear));
            element.SetAttribute("Semester", K12.Data.Int.GetString(Semester));
            element.SetAttribute("Credit", K12.Data.Decimal.GetString(Credit));
            element.SetAttribute("Period", K12.Data.Decimal.GetString(Period));
            element.SetAttribute("Domain", Domain);
            element.SetAttribute("FullName", FullName);
            element.SetAttribute("CalcFlag", "" + CalcFlag);
            element.SetAttribute("SubjectName", SubjectName);

            XmlElement elmGrouping = xmldoc.CreateElement("Grouping");
            elmGrouping.SetAttribute("RowIndex", K12.Data.Int.GetString(RowIndex));
            elmGrouping.SetAttribute("startLevel", K12.Data.Int.GetString(StartLevel));

            element.AppendChild(elmGrouping);
            element.SetAttribute("NotIncludedInCalc", "" + NotIncludedInCalc);
            element.SetAttribute("NotIncludedInCredit", "" + NotIncludedInCredit);
            element.SetAttribute("RequiredBy", RequiredBy);
            element.SetAttribute("Required", Required ? "必修" : "選修");
            element.SetAttribute("Category", Category);
            element.SetAttribute("Entry", Entry);
            element.SetAttribute("Level",K12.Data.Int.GetString(Level));

            return element;
        }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public ProgramSubject()
        {
            NotIncludedInCalc = false;
            NotIncludedInCredit = false;
            CalcFlag = true;
        }

        #region ICloneable 成員

        /// <summary>
        /// 複製課程規劃科目物件
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            ProgramSubject newSubject = new ProgramSubject();
            newSubject.ID = this.ID;
            newSubject.Name = this.Name;
            newSubject.GradeYear = this.GradeYear;
            newSubject.Semester = this.Semester;
            newSubject.Credit = this.Credit;
            newSubject.Period = this.Period;
            newSubject.Domain = this.Domain;
            newSubject.FullName = this.FullName;
            newSubject.Level = this.Level;
            newSubject.SubjectName = this.SubjectName;
            newSubject.CalcFlag = this.CalcFlag;
            newSubject.RowIndex = this.RowIndex;
            return newSubject;
        }
        #endregion
    }
}