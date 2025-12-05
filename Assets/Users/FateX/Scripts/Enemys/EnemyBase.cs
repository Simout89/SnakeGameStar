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
    public class EnemyBase : MonoBehaviour, IEnemy, IDamageable, IPoolable, IDamageDealer
    {
        [Header("References")] [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        [SerializeField] private Transform _shadow;
        [field: SerializeField] public Transform Body { get; private set; }
        [SerializeField] private EnemyData _enemyData;
        public EnemyData EnemyData => _enemyData;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        private Tween swayTween;

        public float CurrentHealth { get; private set; }
        public event Action<float> OnHealthChanged;
        public event Action<EnemyBase> OnDie;
        public event Action<EnemyBase> OnEnemyTakeDamage;
        private float knockbackEndTime;


        public bool Visible = true;
        public bool AlreadyDie = false;
        private Vector3 startShadowScale;
        private MaterialPropertyBlock materialPropertyBlock;
        private Vector3 originScale;
        private Collider2D _collider2D;
        public int CoinDropCount;

        public DamageInfo lastDamageInfo { get; private set; }
        private float statsMultiplayer = 1;

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

        public void MultiplyStats(float value)
        {
            CurrentHealth *= value;
            statsMultiplayer = value;
        }

        public void Move(Vector2 direction)
        {
            // transform.position = transform.position + direction;
            if (AlreadyDie || Time.time < knockbackEndTime)
                    return;

            // Debug.Log($"Enemy {name}: direction={direction}, magnitude={direction.magnitude}, velocity will be={direction * _enemyData.MoveSpeed}");


            _rigidbody2D.linearVelocity = direction * (_enemyData.MoveSpeed * Time.fixedDeltaTime);
            
            
            //_rigidbody2D.position += direction * _enemyData.MoveSpeed  * Time.fixedDeltaTime;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (AlreadyDie)
                return;

            lastDamageInfo = damageInfo;

            CurrentHealth -= damageInfo.Amount;

            OnEnemyTakeDamage?.Invoke(this);

            DamageEffect();
            
            KnockBack(damageInfo);

            if (CurrentHealth <= 0)
            {
                OnDie?.Invoke(this);

                AlreadyDie = true;
                
                KnockBack(damageInfo);

                _rigidbody2D.linearVelocity = Vector2.zero;

                _shadow.DOScale(Vector3.zero, 0.5f);

                DamageOverTime.StopAllDots((IDamageDealer)this);
                DamageOverTime.StopAllDots((IDamageable)this);

                _collider2D.enabled = false;

                _spriteRenderer.DOKill();
                DOTween.To(
                    () => GetFloat("_FadeAmount"),
                    v => SetFloat("_FadeAmount", v),
                    0.5f,
                    0.5f
                ).OnComplete(() => { LeanPool.Despawn(gameObject); });
            }
        }

        private void KnockBack(DamageInfo damageInfo)
        {
            knockbackEndTime = Time.time + 0.1f;

            _rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.AddForce(((Vector2)transform.position - damageInfo.DamageDealerPos).normalized * 2, ForceMode2D.Impulse);
        }

        private void DamageEffect()
        {
            DOTween.Kill(_spriteRenderer);

            SetFloat("_HitEffectBlend", 1f);

            DOTween.To(
                () => GetFloat("_HitEffectBlend"),
                v => SetFloat("_HitEffectBlend", v),
                0,
                0.3f
            ).SetTarget(_spriteRenderer);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
            {
                DamageInfo damageInfo = new DamageInfo(_enemyData.Damage * statsMultiplayer, _enemyData.EnemyName, transform.position);

                DamageOverTime.StartDot(snakeBodyPartHealth, this, 0.3f, damageInfo);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out SnakeBodyPartHealth snakeBodyPartHealth))
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

            SetFloat("_HitEffectBlend", 0f);
            SetFloat("_FadeAmount", 0f);

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
        public Vector2 DamageDealerPos;

        public DamageInfo(float amount, string damageDealerName, Vector2 damageDealerPos)
        {
            DamageDealerName = damageDealerName;
            Amount = amount;
            DamageDealerPos = damageDealerPos;
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
        public void Move(Vector2 direction);
    }
}