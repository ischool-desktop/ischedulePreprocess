using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus;

namespace SunsetCore
{
    public class KeyCatalogFactory
    {
        private CustomStringComparer comparer = new CustomStringComparer();

        public KeyCatalogFactory()
        {
            ToStringFormatter = x => string.Format("{0}({1})", x.Name, x.TotalKeyCount);
            NameSorter = (x, y) => comparer.Compare(x.Name, y.Name);
        }

        public KeyCatalogFactory(Func<KeyCatalog, string> formatter,
            Comparison<KeyCatalog> nameSorter)
            : this()
        {
            ToStringFormatter = formatter;
            NameSorter = nameSorter;
        }

        /// <summary>
        /// 提供 ToString 方法呼叫。
        /// </summary>
        public Func<KeyCatalog, string> ToStringFormatter { get; set; }

        /// <summary>
        /// 取得或設定名稱的排序物件。
        /// </summary>
        public Comparison<KeyCatalog> NameSorter { get; set; }

        public virtual KeyCatalog Create(string name)
        {
            return new KeyCatalog(name, this);
        }
    }

}
