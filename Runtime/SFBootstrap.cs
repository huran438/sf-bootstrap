using System;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SFramework.Bootstrap.Runtime
{
    public abstract class SFBootstrap : MonoBehaviour
    {
        protected event Action<float, long> Progress = (_, _) => { };
        protected event Action<float, string> StepProgress = (_, _) => { };

        [SerializeField]
        private SFBootstrapStep[] _initializationSteps = Array.Empty<SFBootstrapStep>();

        private CancellationTokenSource _cancellationTokenSource;

        protected virtual async UniTaskVoid Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            await PreInit(_cancellationTokenSource.Token);

            for (var index = 0; index < _initializationSteps.Length; index++)
            {
                var initializationStep = _initializationSteps[index];
                if (!initializationStep.Enabled) continue;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                await initializationStep.Data.Run(StepProgress, _cancellationTokenSource.Token);
                stopwatch.Stop();
                var elapsedTime = stopwatch.ElapsedMilliseconds;
                var progress = Mathf.InverseLerp(0, _initializationSteps.Length, index);
                Progress.Invoke(progress, elapsedTime);
                if (Debug.isDebugBuild)
                {
                    Debug.LogFormat("Bootstrap - {0:D8} - {1}", elapsedTime, initializationStep.Name);
                }
            }

            await Init(_cancellationTokenSource.Token);
        }

        protected abstract UniTask PreInit(CancellationToken cancellationToken);
        protected abstract UniTask Init(CancellationToken cancellationToken);
        protected abstract void Dispose();

        protected virtual void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            Dispose();
        }
    }
}