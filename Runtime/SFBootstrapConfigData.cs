using System;
using UnityEngine;

namespace SFramework.Bootstrap.Runtime
{
    [CreateAssetMenu(menuName = "SFramework/Bootstrap/Config", fileName = "bsc_")]
    public class SFBootstrapConfigData : ScriptableObject
    {
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    return name;
                }

                return _name;
            }
        }

        public SFBootstrapStep[] Steps => _steps;

        [SerializeField]
        private string _name;

        [SerializeField]
        private SFBootstrapStep[] _steps = Array.Empty<SFBootstrapStep>();
    }
}