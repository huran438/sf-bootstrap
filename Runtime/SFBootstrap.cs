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
        private SFBootstrapConfigData _defaultConfig;

        [SerializeField]
        private SFBootstrapConfig[] _configs = Array.Empty<SFBootstrapConfig>();
        
        private CancellationTokenSource _cancellationTokenSource;
        private Stopwatch _stopwatch;

        protected virtual async UniTaskVoid Awake()
        {
            var bootstrapConfigData = _defaultConfig;

            foreach (var bootstrapConfig in _configs)
            {
                if (bootstrapConfig.BundleId == Application.identifier)
                {
                    bootstrapConfigData = bootstrapConfig.Config;
                    break;
                }
            }
            
            if (bootstrapConfigData == null)
            {
                Debug.LogError("[BOOT] - Bootstrap Config not found");
                return;
            }
            
            _cancellationTokenSource = new CancellationTokenSource();
            _stopwatch = new Stopwatch();
            _stopwatch.Reset();
            _stopwatch.Start();

          
            await PreInit(_cancellationTokenSource.Token, _stopwatch.ElapsedMilliseconds);

            var stopwatch = new Stopwatch();
            
            for (var index = 0; index < bootstrapConfigData.Steps.Length; index++)
            {
                var initializationStep = bootstrapConfigData.Steps[index];
                if (initializationStep.Data == null) continue;
                if (initializationStep.Enabled == false) continue;
                stopwatch.Restart();
                await initializationStep.Data.Run(StepProgress, _cancellationTokenSource.Token, _stopwatch.ElapsedMilliseconds);
                stopwatch.Stop();
                var elapsedTime = stopwatch.ElapsedMilliseconds;
                var progress = Mathf.InverseLerp(0, bootstrapConfigData.Steps.Length, index);
                Progress.Invoke(progress, elapsedTime);
                Debug.LogFormat("[BOOT] - {0:D8} - {1}", elapsedTime, initializationStep.Name);
            }

            _stopwatch.Stop();

            await Init(_cancellationTokenSource.Token, _stopwatch.ElapsedMilliseconds);
        }

        protected abstract UniTask PreInit(CancellationToken cancellationToken, long elapsedMilliseconds);
        protected abstract UniTask Init(CancellationToken cancellationToken, long elapsedMilliseconds);
        protected abstract void Dispose();

        protected virtual void OnDestroy()
        {
            _stopwatch.Stop();
            _stopwatch = null;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            Dispose();
        }
    }
}