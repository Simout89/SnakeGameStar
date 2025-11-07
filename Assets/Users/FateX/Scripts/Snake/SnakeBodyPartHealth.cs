using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeBodyPartHealth: MonoBehaviour, IDamageable
    {
        public float CurrentHealth { get; }
        
        public void TakeDamage(DamageInfo damageInfo)
        {
            OnHealthChanged?.Invoke(damageInfo.Amount);
        }

        public event Action<float> OnHealthChanged;
        public event Action OnDie;
    }
}