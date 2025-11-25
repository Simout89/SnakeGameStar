using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Combat;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Upgrade;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts
{
    public class EnemyBase: MonoBehaviour, IEnemy, IDamageable, IPoolable, IDamageDealer
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _shadow;
        [field: SerializeField] public Transform Body { get; private set; }
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        private Tween swayTween;
        
        public float CurrentHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action<EnemyBase> OnDie;
        
        public bool Visible = true;
        public bool AlreadyDie = false;
        private Vector3 startShadowScale;
        private MaterialPropertyBlock materialPropertyBlock;
        private Vector3 originScale;
        private Collider2D _collider2D;
        
        public DamageInfo lastDamageInfo { get; private set; }

        private void Awake()
        {
            startShadowScale = _shadow.localScale;
            originScale = Body.localScale;

            _collider2D = GetComponent<Collider2D>();
            
            materialPropertyBlock = new MaterialPropertyBlock();
            
            StartSwaying();
        }

        public EnemyData GetData()
        {
            return _enemyData;
        }

        public void StartSwaying()
        {
            swayTween?.Kill();

            swayTween = Body.DOLocalRotate(new Vector3(0, 0, 10), Random.Range(0.4f, 0.5f))
                .From(new Vector3(0, 0, -10))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        public void StopSwaying()
        {
            swayTween?.Kill();
            Body.localRotation = Quaternion.identity;
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

            lastDamageInfo = damageInfo;
            
            CurrentHealth -= damageInfo.Amount;

            DamageEffect();

            if (CurrentHealth <= 0)
            {
                OnDie?.Invoke(this);
                
                AlreadyDie = true;
                
                _rigidbody2D.linearVelocity = Vector2.zero;

                _shadow.DOScale(Vector3.zero, 0.5f);
                
                DamageOverTime.StopAllDots((IDamageDealer)this);
                DamageOverTime.StopAllDots((IDamageable)this);
                
                _collider2D.enabled = false;
                
                _spriteRenderer.DOKill();
                DOTween.To(
                    () => GetFloat("_DissolveAmount"),
                    v => SetFloat("_DissolveAmount", v),
                    1f,
                    0.5f
                ).OnComplete(() =>
                {
                    LeanPool.Despawn(gameObject);
                });
            }
        }

        private void DamageEffect()
        {
            DOTween.Kill(_spriteRenderer);
            
            DOTween.To(
                () => GetFloat("_FlashAmount"),
                v => SetFloat("_FlashAmount", v),
                0f,
                0.3f
            ).SetTarget(_spriteRenderer);

            SetFloat("_FlashAmount", 1f);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
            {
                DamageInfo damageInfo = new DamageInfo(_enemyData.Damage, _enemyData.EnemyName);
                
                DamageOverTime.StartDot(snakeBodyPartHealth, this, 0.3f, damageInfo);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if(other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
            {
                DamageOverTime.StopDot(snakeBodyPartHealth, this);
            }
        }

        private void SetFloat(string property, float value)
        {
            _spriteRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetFloat(property, value);
            _spriteRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        private float GetFloat(string property)
        {
            _spriteRenderer.GetPropertyBlock(materialPropertyBlock);
            return materialPropertyBlock.GetFloat(property);
        }


        public void OnSpawn()
        {
            Visible = false;

            _collider2D.enabled = true;

            
            Body.localScale = originScale;

            StartSwaying();
            
            SetFloat("_FlashAmount", 0f);
            SetFloat("_DissolveAmount", 0f);

            _shadow.localScale = startShadowScale;
            CurrentHealth = _enemyData.Health;
            AlreadyDie = false;
        }

        public void OnDespawn()
        {
            StopSwaying();
            
            Body.localScale = originScale;
        }
        
        private void OnBecameVisible()
        {
            Visible = true;
            StartSwaying();
        }

        private void OnBecameInvisible()
        {
            Visible = false;
            StopSwaying();
        }
    }

    public struct DamageInfo
    {
        public string DamageDealerName;
        public float Amount;
        public DamageInfo(float amount, string damageDealerName)
        {
            this.Amount = amount;
            DamageDealerName = damageDealerName;
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