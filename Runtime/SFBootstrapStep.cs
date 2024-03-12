using System;
using UnityEngine;

namespace SFramework.Bootstrap.Runtime
{
    [Serializable]
    public struct SFBootstrapStep
    {
        public bool Enabled => _enabled && _data != null;
        public SFBootstrapStepData Data => _data;
        public string Name => string.IsNullOrEmpty(_name) ? (_data != null ? _data.name : "NULL") : _name; 

        [SerializeField] private string _name;
        [SerializeField] private bool _enabled;
        [SerializeField] private SFBootstrapStepData _data;
    }
}