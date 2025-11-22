using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class SnakeBodyPartHealth: MonoBehaviour, IDamageable
    {
        public float CurrentHealth { get; }
        
        public void TakeDamage(DamageInfo damageInfo)
        {
            OnTakeDamage?.Invoke(damageInfo);
        }

        public event Action<DamageInfo> OnTakeDamage;
    }
}