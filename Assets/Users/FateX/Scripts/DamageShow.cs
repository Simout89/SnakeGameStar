using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts
{
    public class DamageShow: IInitializable, IDisposable
    {
        [Inject] private EnemyManager enemyManager;
        [Inject] private GameConfig _gameConfig;

        private void HandleEnemyDie(EnemyBase obj)
        {
            var newDamageView = LeanPool.Spawn(_gameConfig.GameConfigData.DamageViewPrefab, obj.transform.position + Vector3.up / 2 + (Vector3)Random.insideUnitCircle / 2, Quaternion.identity);
            newDamageView.TMPText.text = (((int)(obj.lastDamageInfo.Amount * GameConstant.VisualDamageMultiplayer + Random.Range(-2, 3))).ToString());

            var _baseScale = newDamageView.transform.localScale;
            
            newDamageView.CanvasGroup.alpha = 0f;
            newDamageView.transform.localScale = Vector3.zero;
            
            DOTween.Sequence()
                .Append(newDamageView.transform.DOScale(_baseScale * 1.3f, 0.15f).SetEase(Ease.OutBack))
                .Append(newDamageView.transform.DOScale(_baseScale, 0.1f).SetEase(Ease.InOutSine))
                .Join(newDamageView.CanvasGroup.DOFade(1f, 0.05f))
                .AppendInterval(0.4f)
                .Append(newDamageView.transform.DOMoveY(newDamageView.transform.position.y + 1f, 0.6f).SetEase(Ease.OutCubic))
                .Join(newDamageView.CanvasGroup.DOFade(0f, 0.6f))
                .OnComplete(() =>
                {
                    LeanPool.Despawn(newDamageView);
                });
        }

        public void Initialize()
        {
            enemyManager.OnEnemyTakeDamage += HandleEnemyDie;
        }

        public void Dispose()
        {
            enemyManager.OnEnemyTakeDamage -= HandleEnemyDie;
        }
    }
}