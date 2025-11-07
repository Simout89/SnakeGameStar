using System;
using UnityEngine;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts
{
    public class Enemy: MonoBehaviour, IEnemy, IDamageable
    {
        [SerializeField] private EnemyData _enemyData;
        
        public float CurrentHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action OnDie;
        public void Move(Vector3 direction)
        {
            transform.position = transform.position + direction;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            
        }

    }

    public struct DamageInfo
    {
        public float Amount;
        public DamageInfo(float amount)
        {
            this.Amount = amount;
        }
    }

    public interface IDamageable
    {
        public float CurrentHealth { get; }
        public void TakeDamage(DamageInfo damageInfo);
        public event Action<float> OnHealthChanged;
        public event Action OnDie;
    }

    public interface IEnemy
    {
        public void Move(Vector3 direction);
    }
}