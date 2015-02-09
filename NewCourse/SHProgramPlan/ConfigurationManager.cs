using System;
using System.Collections.Generic;
using System.Text;
using SmartSchool.Customization.PlugIn.Configure;
using FISCA.Presentation;

namespace SmartSchool.Configure
{
    public class ConfigurationManager : IConfigurationManager
    {
        private static ConfigurationManager _Instance = null;

        public static ConfigurationManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ConfigurationManager();
                return _Instance;
            }
        }

        private Dictionary<string, IConfigurationManager> _Configurations = new Dictionary<string, IConfigurationManager>();

        private ConfigurationManager() { }

        #region IConfigurationManager 成員

        /// <summary>
        /// 註冊教務作業、學務作業
        /// </summary>
        public void InitializePanels()
        {
            //_Configurations.Add("教務作業", SHSchool.Affair.EduAdmin.Instance);
            //_Configurations.Add("學務作業", SHSchool.Affair.StuAdmin.Instance);

            //MotherForm.AddPanel(SHSchool.Affair.EduAdmin.Instance);
            //MotherForm.AddPanel(SHSchool.Affair.StuAdmin.Instance);
        }

        public void AddConfigurationItem(IConfigurationItem configurationItem)
        {
            string groupName = configurationItem.TabGroup == "" ? "設定" : configurationItem.TabGroup;
            if (!_Configurations.ContainsKey(groupName))
            {
                IConfigurationManager newConf = new Configuration(groupName);

                _Configurations.Add(groupName, newConf);
                MotherForm.AddPanel(newConf as INCPanel);
            }
            _Configurations[groupName].AddConfigurationItem(configurationItem);
        }

        #endregion
    }
}
