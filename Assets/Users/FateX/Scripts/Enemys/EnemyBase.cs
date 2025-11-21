using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Combat;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts
{
    public class EnemyBase: MonoBehaviour, IEnemy, IDamageable, IPoolable
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _shadow;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        
        public float CurrentHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action<EnemyBase> OnDie;
        
        public bool Visible = true;
        public bool AlreadyDie = false;
        private Vector3 startShadowScale;

        private void Awake()
        {
            startShadowScale = _shadow.localScale;
        }

        public void Move(Vector3 direction)
        {
            // transform.position = transform.position + direction;
            if(AlreadyDie)
                return;
            
            _rigidbody2D.linearVelocity = direction;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if(AlreadyDie)
                return;
            
            CurrentHealth -= damageInfo.Amount;

            DamageEffect();

            if (CurrentHealth <= 0)
            {
                OnDie?.Invoke(this);
                
                AlreadyDie = true;

                _shadow.DOScale(Vector3.zero, 0.5f);
                
                _spriteRenderer.material.DOFloat(1f, "_DissolveAmount", 0.5f).OnComplete((() =>
                {
                    _spriteRenderer.DOKill();
                    
                    LeanPool.Despawn(gameObject);
                }));
            }
        }

        private void DamageEffect()
        {
            _spriteRenderer.material.DOComplete();
            
            _spriteRenderer.material.SetFloat("_FlashAmount", 1);
            _spriteRenderer.material.DOFloat(0f, "_FlashAmount", 0.3f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
            {
                DamageInfo damageInfo = new DamageInfo(2);
                
                snakeBodyPartHealth.TakeDamage(damageInfo);
            }
        }

        public void OnSpawn()
        {
            _spriteRenderer.material.SetFloat("_FlashAmount", 0);
            _spriteRenderer.material.SetFloat("_DissolveAmount", 0);
            _shadow.localScale = startShadowScale;
            
            CurrentHealth = _enemyData.Health;

            AlreadyDie = false;
        }

        public void OnDespawn()
        {
            DamageOverTime.StopAllDots(this);
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
    }

    public interface IDamageDealer
    {
        
    }

    public interface IEnemy
    {
        public void Move(Vector3 direction);
    }
}