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

        private Transform target;
        private CancellationTokenSource cts;

        public void Launch(Transform targetEnemy, float flightTime)
        {
            target = targetEnemy;
            cts = new CancellationTokenSource();
            
            Vector2 targetPos = PredictTargetPosition(targetEnemy, flightTime);
    
            FollowTarget(cts.Token, flightTime, targetPos).Forget();
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
            LeanPool.Despawn(gameObject);
        }

        public void OnSpawn() { }
    }
}