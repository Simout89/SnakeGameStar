using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class GameTimer: IDisposable
    {
        private float duration = 0;
        private bool _isRunning;
        public int CurrentTime { get; private set; }

        private CancellationTokenSource cancellationTokenSource;
        
        public event Action<int> OnSecondChanged;
        public event Action OnTimerEnd;

        public void StartTimer()
        {
            if (_isRunning) return;
            
            Debug.Log("Таймер запущен");

            _isRunning = true;
            
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            
            RunTimerAsync(cancellationTokenSource.Token).Forget();
        }
        
        public void StartTimer(float duration)
        {
            this.duration = duration;
            
            if (_isRunning) return;
            
            Debug.Log("Таймер запущен");

            _isRunning = true;

            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            
            RunTimerAsync(cancellationTokenSource.Token).Forget();
        }

        private async UniTask RunTimerAsync(CancellationToken cancellationToken)
        {
            try
            {
                float elapsed = 0f;
                int lastSecond = -1;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    
                    int currentSecond = Mathf.FloorToInt(elapsed);
                    if (currentSecond != lastSecond)
                    {
                        lastSecond = currentSecond;
                        CurrentTime = currentSecond;
                        OnSecondChanged?.Invoke(currentSecond);
                    }
                    
                    await UniTask.Yield(cancellationToken);
                }

                _isRunning = false;
                OnTimerEnd?.Invoke();
                
                Debug.Log("Таймер закончился");
            }
            catch (OperationCanceledException)
            {
                // Таймер был отменён - это нормально
                Debug.Log("Таймер был отменён");
                _isRunning = false;
            }
        }

        public void StopTimer()
        {
            if (!_isRunning) return;
            
            cancellationTokenSource?.Cancel();
            _isRunning = false;
        }

        public void Dispose()
        {
            StopTimer();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }
    }
}