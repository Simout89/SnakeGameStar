using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Combat
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private Transform projectileTransform;
        [SerializeField] private float arcHeight = 4f;
        [SerializeField] private GameObject explosionVfx;
        [SerializeField] private LayerMask _snakeLayerMask;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private GlobalSoundPlayer _globalSoundPlayer;
        private Transform target;
        private CancellationTokenSource cts;
        private float areaOfEffectRaidus;
        private DamageInfo damageInfo;
        private bool piercing;

        public void InitSoundPlayer(GlobalSoundPlayer globalSoundPlayer)
        {
            _globalSoundPlayer = globalSoundPlayer;
        }

        public void LaunchArc(Transform targetEnemy, float flightTime, DamageInfo damageInfo, float AOERadius = 0)
        {
            target = targetEnemy;
            areaOfEffectRaidus = AOERadius;
            this.damageInfo = damageInfo;
            
            cts = new CancellationTokenSource();
            
            Vector2 targetPos = PredictTargetPosition(targetEnemy, flightTime);
    
            FollowTarget(cts.Token, flightTime, targetPos).Forget();
        }

        public void SimpleLaunch(Vector3 direction, DamageInfo damageInfo, bool piercing = false)
        {
            this.damageInfo = damageInfo;

            this.piercing = piercing;

            _rigidbody2D.AddForce(direction, ForceMode2D.Impulse);
            
            LeanPool.Despawn(gameObject, 10f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!piercing)
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    GameEvents.DamageDealt(damageInfo);
                    damageable.TakeDamage(damageInfo);
                }
            
                LeanPool.Despawn(gameObject);
            }
            else
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    GameEvents.DamageDealt(damageInfo);
                    damageable.TakeDamage(damageInfo);
                }
                else
                {
                    LeanPool.Despawn(gameObject);
                }
            }
        }

        private Vector2 PredictTargetPosition(Transform targetEnemy, float flightTime)
        {
            Rigidbody2D rb = targetEnemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                return (Vector2)targetEnemy.position + rb.linearVelocity * flightTime;
            }
            return targetEnemy.position;
        }

        private async UniTask FollowTarget(CancellationToken token, float flightTime, Vector2 predictedPos)
        {
            Vector2 startPos = transform.position;
            float elapsedTime = 0f;

            while (!token.IsCancellationRequested)
            {
                if (this == null || projectileTransform == null)
                    return;

                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / flightTime);

                Vector2 currentPos = Vector2.Lerp(startPos, predictedPos, progress);

                float height = arcHeight * Mathf.Sin(progress * Mathf.PI);
                Vector3 projectilePos = new Vector3(currentPos.x, currentPos.y + height, transform.position.z);
                projectileTransform.position = projectilePos;
                transform.position = currentPos;

                if (progress >= 1f)
                {
                    OnReachTarget();
                    break;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        public void OnDespawn()
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = null;
        }

        private void OnReachTarget()
        {
            AreaOfEffectDamage();

            LeanPool.Despawn(LeanPool.Spawn(explosionVfx, projectileTransform.position, Quaternion.identity), 1f);
            
            LeanPool.Despawn(gameObject);
        }

        private void AreaOfEffectDamage()
        {
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.WeaponSoundsData.Explosion);
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(projectileTransform.position, areaOfEffectRaidus, ~_snakeLayerMask);
            foreach (var collider in colliders)
            {
                if(collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damageInfo);
                    
                    GameEvents.DamageDealt(damageInfo);
                }
            }
        }   

        public void OnSpawn() { }
    }
}