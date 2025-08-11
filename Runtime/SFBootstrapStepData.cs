using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SFramework.Bootstrap.Runtime
{
    public abstract class SFBootstrapStepData : ScriptableObject
    {
        public abstract UniTask Run(Action<float, string> stepProgress, CancellationToken cancellationToken, long elapsedMilliseconds);
    }
}