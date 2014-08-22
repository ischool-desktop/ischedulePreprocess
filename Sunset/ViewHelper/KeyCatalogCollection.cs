using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunsetCore
{
    public class KeyCatalogCollection
    {
        private Dictionary<string, KeyCatalog> _catalogs = new Dictionary<string, KeyCatalog>();

        /// <summary>
        /// 
        /// </summary>
        public KeyCatalogCollection(KeyCatalogFactory factory)
        {
            Factory = factory == null ? new KeyCatalogFactory() : factory;
        }

        public KeyCatalogFactory Factory { get; private set; }

        public KeyCatalog this[string name]
        {
            get
            {
                if (!_catalogs.ContainsKey(name))
                {
                    KeyCatalog kc = null;

                    kc = Factory.Create(name);

                    _catalogs.Add(name, kc);
                }
                return _catalogs[name];
            }
        }

        public void Add(KeyCatalog catalog)
        {
            _catalogs.Add(catalog.Name, catalog);
        }

        public void Add(string name)
        {
            _catalogs.Add(name, new KeyCatalog(name, Factory));
        }

        public void Remove(string name)
        {
            _catalogs.Remove(name);
        }

        public void Clear()
        {
            foreach (KeyCatalog catalog in _catalogs.Values)
                catalog.Clear();

            _catalogs.Clear();
        }

        public bool Contains(string name)
        {
            return _catalogs.ContainsKey(name);
        }

        public int Count
        {
            get { return _catalogs.Count; }
        }

        public IEnumerable<string> Names
        {
            get { return _catalogs.Keys; }
        }

        public IEnumerable<KeyCatalog> Values
        {
            get { return _catalogs.Values; }
        }

        public IEnumerable<KeyCatalog> SortedValues
        {
            get
            {
                List<KeyCatalog> catalogs = new List<KeyCatalog>(_catalogs.Values);
                catalogs.Sort(Factory.NameSorter);

                return catalogs;
            }
        }

        ~KeyCatalogCollection()
        {
            _catalogs.Clear();
        }
    }
}
