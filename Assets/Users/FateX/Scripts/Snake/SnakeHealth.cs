using System;
using System.Collections.Generic;
using UnityEngine;
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

        public event Action OnHealthChanged;
        public event Action OnDie;

        private void Awake()
        {
            CurrentHealth = _snakeController.SnakeData.BaseHealth;
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
            foreach (var snakeBodyPart in snakeBodyParts)
            {
                snakeBodyPart.SnakeBodyPartHealth.OnTakeDamage -= HandleTakeDamage;
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += amount;

            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
            
            OnHealthChanged?.Invoke();
        }

        private void HandleTakeDamage(DamageInfo damageInfo)
        {
            if(Time.time < timeToNextHit)
                return;

            timeToNextHit = Time.time + 0.16f;
            
            Debug.Log("УРон");
            
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