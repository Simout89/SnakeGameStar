using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Users.FateX.Scripts.Combat
{
    public class Projectile: MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 200f;
    
        private Transform target;
        private CancellationTokenSource cts;
    
        public void Launch(Transform targetEnemy)
        {
            target = targetEnemy;
            cts = new CancellationTokenSource();
            FollowTarget(cts.Token).Forget();
        }
    
        private async UniTaskVoid FollowTarget(CancellationToken token)
        {
            while (target != null && !token.IsCancellationRequested)
            {
                // Направление к цели
                Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            
                // Движение к цели
                transform.position = Vector2.MoveTowards(
                    transform.position, 
                    target.position, 
                    speed * Time.deltaTime
                );
            
                // Поворот снаряда (опционально)
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, 
                    targetRotation, 
                    rotationSpeed * Time.deltaTime
                );
            
                // Проверка достижения цели
                if (Vector2.Distance(transform.position, target.position) < 0.1f)
                {
                    OnReachTarget();
                    break;
                }

                await UniTask.Yield();
            }
        }
    
        private void OnReachTarget()
        {
            // Нанести урон, эффект и т.д.
            Destroy(gameObject);
        }
    
        private void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}