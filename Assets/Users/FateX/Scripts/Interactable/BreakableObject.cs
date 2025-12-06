using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Combat;
using Users.FateX.Scripts.Data;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.Interactable
{
    public class BreakableObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BreakableObjectData _breakableObjectData;
        [SerializeField] private Transform shadow;
        private MaterialPropertyBlock materialPropertyBlock;
        private bool alreadyDie = false;


        private void Awake()
        {
            CurrentHealth = _breakableObjectData.Health;
            materialPropertyBlock = new MaterialPropertyBlock();

        }

        public float CurrentHealth { get; private set; }
        
        public async void Respawn(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: this.GetCancellationTokenOnDestroy());
    
            // Сбрасываем флаг смерти
            alreadyDie = false;
    
            // Восстанавливаем здоровье
            CurrentHealth = _breakableObjectData.Health;
    
            // Останавливаем все анимации
            DOTween.Kill(_spriteRenderer);
            shadow.DOKill();
    
            // Сбрасываем shader свойства
            SetFloat("_FadeAmount", 0f);
            SetFloat("_HitEffectBlend", 0f);
    
            // Восстанавливаем масштаб тени
            shadow.localScale = Vector3.one;
    
            // Активируем объект
            gameObject.SetActive(true);
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if(alreadyDie)
                return;
            
            CurrentHealth -= damageInfo.Amount;

            DamageEffect();

            if (CurrentHealth <= 0)
            {
                alreadyDie = true;
                DamageOverTime.StopAllDots((IDamageable)this);
                
                foreach (var loot in _breakableObjectData.loot)
                {
                    var newObject = LeanPool.Spawn(loot, transform.position, Quaternion.identity);

                    // Случайная конечная точка вокруг объекта (в радиусе 3)
                    Vector3 endPos = transform.position + (Vector3)Random.insideUnitCircle * 3f;

                    // Высота дуги
                    float jumpPower = 2f;

                    // Длительность анимации
                    float duration = 0.6f;

                    // Один "прыжок" — создаёт дугу
                    newObject.transform.DOJump(endPos, jumpPower, 1, duration)
                        .SetEase(Ease.OutQuad);
                }

                shadow.DOScale(Vector3.zero, 0.5f);
                
                _spriteRenderer.DOKill();
                DOTween.To(
                    () => GetFloat("_FadeAmount"),
                    v => SetFloat("_FadeAmount", v),
                    1f,
                    0.5f
                ).OnComplete(() =>
                {

                    
                    gameObject.SetActive(false);
                    Respawn(180f);
                });
            }
        }
        
        private void DamageEffect()
        {
            DOTween.Kill(_spriteRenderer);
            
            DOTween.To(
                () => GetFloat("_HitEffectBlend"),
                v => SetFloat("_HitEffectBlend", v),
                0f,
                0.3f
            ).SetTarget(_spriteRenderer);

            SetFloat("_HitEffectBlend", 1f);
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
    }
}