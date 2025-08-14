using System;
using UnityEngine;

namespace SFramework.Bootstrap.Runtime
{
    [Serializable]
    public class SFBootstrapConfig
    {
        public string BundleId => _bundleId;

        public SFBootstrapConfigData Data
        {
            get
            {
                foreach (var config in _configs)
                {
                    if(config.Platform == Application.platform) return config.Data;
                }

                return _data;
            }
        }

        [SerializeField]
        private string _bundleId;

        [SerializeField]
        private SFBootstrapConfigData _data;

        [SerializeField]
        private SFBootstrapConfigByPlatform[] _configs;
    }
    
    [Serializable]
    public class SFBootstrapConfigByPlatform
    {
        public RuntimePlatform Platform => _platform;
        public SFBootstrapConfigData Data => _data;
        
        [SerializeField]
        private RuntimePlatform _platform;
        
        [SerializeField]
        private SFBootstrapConfigData _data;
    }
}