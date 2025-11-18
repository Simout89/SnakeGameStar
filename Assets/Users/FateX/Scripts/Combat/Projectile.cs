using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Combat
{
    public class Projectile: MonoBehaviour, IPoolable
    {
        [SerializeField] private Transform projectileTransform;
        [SerializeField] private float arcHeight = 4f;
        
        private Transform target;
        private CancellationTokenSource cts;
    
        public void Launch(Transform targetEnemy, float speed)
        {
            target = targetEnemy;
            cts = new CancellationTokenSource();
            FollowTarget(cts.Token, speed).Forget();
        }
    
        private async UniTask FollowTarget(CancellationToken token, float speed)
        {
            Vector2 startPos = transform.position;
            float totalDistance = 0f;
            
            while (target != null && !token.IsCancellationRequested)
            {
                Vector2 targetPos = target.position;
                Vector2 currentPos = transform.position;
                
                float step = speed * Time.deltaTime;
                
                transform.position = Vector2.MoveTowards(currentPos, targetPos, step);
                
                totalDistance += step;
                float totalDistanceToTarget = Vector2.Distance(startPos, targetPos);
                float progress = Mathf.Clamp01(totalDistance / totalDistanceToTarget);
                
                float height = arcHeight * Mathf.Sin(progress * Mathf.PI);
                
                Vector3 projectilePos = transform.position;
                projectilePos.y += height;
                projectileTransform.position = projectilePos;
                
                
                if (Vector2.Distance(transform.position, targetPos) < 0.1f)
                {
                    OnReachTarget();
                    break;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
    
        private void OnReachTarget()
        {
            LeanPool.Despawn(gameObject);
        }
        

        public void OnSpawn()
        {
            
        }

        public void OnDespawn()
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}