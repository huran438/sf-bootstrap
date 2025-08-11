using System;
using UnityEngine;

namespace SFramework.Bootstrap.Runtime
{
    [Serializable]
    public class SFBootstrapConfig
    {
        public string BundleId => _bundleId;
        public SFBootstrapConfigData Config => _config;

        [SerializeField]
        private string _bundleId;

        [SerializeField]
        private SFBootstrapConfigData _config;
    }
}