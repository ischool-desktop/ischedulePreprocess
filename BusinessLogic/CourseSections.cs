using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    /// <summary>
    /// 課程分段集合
    /// </summary>
    public class CourseSections : IEnumerable<CourseSection>
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        public CourseSections()
        {
            Source = new List<CourseSection>();
        }

        /// <summary>
        /// 建構式，傳入課程分段列表
        /// </summary>
        /// <param name="Sections"></param>
        public CourseSections(List<CourseSection> Sections)
        {
            if (K12.Data.Utility.Utility.IsNullOrEmpty(Sections))
                throw new NullReferenceException("課程分段為空集合！");

            Source = Sections
                .OrderBy(x => x.Length)
                .ToList();
        }

        /// <summary>
        /// 取得課程分段
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CourseSection this[int index]
        {
            get
            {
                return index >= 0 && index < Count ? Source[index] : null;
            }
        }

        /// <summary>
        /// 增加課程分段
        /// </summary>
        /// <param name="Section"></param>
        /// <returns></returns>
        public void Add(CourseSection Section)
        {
            Source.Add(Section); 
        }

        /// <summary>
        /// 課程分段來源
        /// </summary>
        public List<CourseSection> Source { get; private set; }

        /// <summary>
        /// 課程分段數目
        /// </summary>
        public int Count { get { return Source.Count; } }

        /// <summary>
        /// 課程分段長度鍵值
        /// </summary>
        public string LensKey { get { return string.Join(",", Source.Select(x => x.Length).ToArray()); } }

        #region IEnumerable<CourseSection> 成員

        public IEnumerator<CourseSection> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成員

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        #endregion

        #region IComparer<CourseSection> 成員

        /// <summary>
        /// 比對課程分段群組是否相同
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            CourseSections TargetSections = obj as CourseSections;

            if (TargetSections == null)
                return false;

            if (this.Count != TargetSections.Count)
                return false;

            if (this.LensKey != TargetSections.LensKey)
                return false;

            for (int i = 0; i < Count; i++)
                if (!this[i].Equals(TargetSections[i]))
                    return false;

            return true;
        }

        /// <summary>
        /// 取得雜湊值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
           return base.GetHashCode();
        }

        #endregion
    }
}