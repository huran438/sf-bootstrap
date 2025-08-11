using System;
using UnityEngine;

namespace SFramework.Bootstrap.Runtime
{
    [CreateAssetMenu(menuName = "SFramework/Bootstrap/Config", fileName = "bsc_")]
    public class SFBootstrapConfigData : ScriptableObject
    {
        public SFBootstrapStep[] Steps => _steps;

        [SerializeField]
        private SFBootstrapStep[] _steps = Array.Empty<SFBootstrapStep>();
    }
}