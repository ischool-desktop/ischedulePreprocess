using System.Collections.Generic;

namespace SunsetCore
{
    public class KeyCatalog : IEnumerable<string>
    {
        private HashSet<string> _keys;

        internal KeyCatalog(string name, KeyCatalogFactory factory)
        {
            Tag = null;
            Name = name;
            Factory = factory == null ? new KeyCatalogFactory() : factory;
            Subcatalogs = new KeyCatalogCollection(factory);
            _keys = new HashSet<string>();
        }

        public object Tag { get; set; }

        public string Name { get; private set; }

        public KeyCatalog this[string name]
        {
            get { return Subcatalogs[name]; }
        }

        public void AddKey(string key)
        {
            _keys.Add(key);
        }

        public IEnumerable<string> AllKey
        {
            get
            {
                if (IsLeaf)
                    return _keys;
                else
                {
                    List<string> result = new List<string>();
                    foreach (KeyCatalog catalog in Subcatalogs.SortedValues)
                        result.AddRange(catalog.AllKey);

                    return result;
                }
            }
        }

        public KeyCatalogCollection Subcatalogs { get; private set; }

        public bool ContainsKey(string key)
        {
            if (IsLeaf)
                return _keys.Contains(key);
            else
                return false;
        }

        public bool ContainsKeyRecursive(string key)
        {
            if (IsLeaf)
                return _keys.Contains(key);
            else
            {
                foreach (KeyCatalog catalog in Subcatalogs.Values)
                {
                    if (catalog.ContainsKeyRecursive(key))
                        return true;
                }
                return false;
            }
        }

        public void RemoveKeyRecursive(params string[] keys)
        {
            if (IsLeaf)
                _keys.ExceptWith(keys);
            else
            {
                foreach (KeyCatalog catalog in Subcatalogs.Values)
                    catalog.RemoveKeyRecursive(keys);
            }
        }

        public void Clear()
        {
            _keys.Clear();
            Subcatalogs.Clear();
        }

        public bool IsLeaf
        {
            get { return _keys.Count > 0 || Subcatalogs.Count <= 0; }
        }

        /// <summary>
        /// 這裡計算的是次數，並非唯一值的總數。
        /// </summary>
        public int TotalKeyCount
        {
            get
            {
                int total = _keys.Count;
                foreach (KeyCatalog catalog in Subcatalogs.Values)
                    total += catalog.TotalKeyCount;
                return total;
            }
        }

        public override string ToString()
        {
            return Factory.ToStringFormatter(this);
        }

        public KeyCatalogFactory Factory { get; private set; }

        #region IEnumerable<KeyCatalog> 成員
        public IEnumerator<string> GetEnumerator()
        {
            return _keys.GetEnumerator();
        }
        #endregion

        #region IEnumerable 成員

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
