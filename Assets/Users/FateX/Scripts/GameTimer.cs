using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class GameTimer
    {
        private float duration = 0;
        private bool _isRunning;
        public int CurrentTime { get; private set; }
        
        public event Action<int> OnSecondChanged;
        public event Action OnTimerEnd;

        public async void StartTimer()
        {
            if (_isRunning) return;
            
            Debug.Log("Таймер запущен");

            _isRunning = true;
            await RunTimerAsync();
        }
        
        public async void StartTimer(float duration)
        {
            this.duration = duration;
            
            if (_isRunning) return;
            
            Debug.Log("Таймер запущен");

            _isRunning = true;
            await RunTimerAsync();
        }

        private async UniTask RunTimerAsync()
        {
            float elapsed = 0f;
            int lastSecond = -1;

            while (elapsed < duration && this != null)
            {
                elapsed += Time.deltaTime;
                
                int currentSecond = Mathf.FloorToInt(elapsed);
                if (currentSecond != lastSecond)
                {
                    lastSecond = currentSecond;
                    CurrentTime = currentSecond;
                    OnSecondChanged?.Invoke(currentSecond);
                }
                
                await UniTask.Yield();
            }

            _isRunning = false;
            OnTimerEnd?.Invoke();
            
            Debug.Log("Таймер закончился");
        }
    }
}