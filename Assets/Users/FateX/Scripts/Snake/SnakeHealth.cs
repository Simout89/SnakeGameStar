using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Users.FateX.Scripts.Upgrade;

namespace Users.FateX.Scripts
{
    public class SnakeHealth: MonoBehaviour
    {
        [SerializeField] private SnakeController _snakeController;
        private List<SnakeSegmentBase> snakeBodyParts = new List<SnakeSegmentBase>();

        public float CurrentHealth { get; private set; }
        public float MaxHealth => _snakeController.SnakeData.BaseHealth;
        
        private float timeToNextHit = 0;
        private bool isInvincible = false;
        private CancellationTokenSource cts;

        public event Action OnHealthChanged;
        public event Action OnDie;

        private void Start()
        {
            CurrentHealth = _snakeController.SnakeData.BaseHealth + _snakeController.PlayerStats.Health.Sum;
        }

        public void Add(SnakeSegmentBase snakeSegmentBase)
        {
            snakeBodyParts.Add(snakeSegmentBase);

            snakeSegmentBase.SnakeBodyPartHealth.OnTakeDamage += HandleTakeDamage;
        }
        
        public void Remove(SnakeSegmentBase snakeSegmentBase)
        {
            snakeSegmentBase.SnakeBodyPartHealth.OnTakeDamage -= HandleTakeDamage;
            
            snakeBodyParts.Remove(snakeSegmentBase);
        }

        private void OnDisable()
        {
            cts?.Cancel();
            cts?.Dispose();
            
            foreach (var snakeBodyPart in snakeBodyParts)
            {
                snakeBodyPart.SnakeBodyPartHealth.OnTakeDamage -= HandleTakeDamage;
            }
        }

        private void OnDestroy()
        {
            cts?.Dispose();
        }

        public void Heal(float amount)
        {
            CurrentHealth += amount;

            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
            
            OnHealthChanged?.Invoke();
        }

        public void SetInvincible(float duration)
        {
            SetInvincibleAsync(duration, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid SetInvincibleAsync(float duration, CancellationToken token)
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            isInvincible = true;

            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: cts.Token);
            
            isInvincible = false;
        }

        private void HandleTakeDamage(DamageInfo damageInfo)
        {
            if (isInvincible)
                return;

            if(Time.time < timeToNextHit)
                return;

            timeToNextHit = Time.time + 0.16f;
            
            Debug.Log("Урон");
            
            foreach (var snakeBodyPart in snakeBodyParts)
            {
                snakeBodyPart.DamageEffect();
            }

            CurrentHealth -= damageInfo.Amount;

            OnHealthChanged?.Invoke();
            
            if (CurrentHealth <= 0)
            {
                OnDie?.Invoke();
            }
        }
    }
}