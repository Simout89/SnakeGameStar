using System;
using UnityEngine;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts
{
    public class EnemyBase: MonoBehaviour, IEnemy, IDamageable
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        
        public float CurrentHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action OnDie;
        
        public bool Visible = true;
        public bool AlreadyDie = false;
        
        public void Move(Vector3 direction)
        {
            // transform.position = transform.position + direction;
            _rigidbody2D.linearVelocity = direction;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
            {
                DamageInfo damageInfo = new DamageInfo(2);
                
                snakeBodyPartHealth.TakeDamage(damageInfo);
            }
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