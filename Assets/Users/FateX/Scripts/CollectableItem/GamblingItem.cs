using System;
using Lean.Pool;
using UnityEngine;
using DG.Tweening;

namespace Users.FateX.Scripts.CollectableItem
{
    public class GamblingItem: MonoBehaviour, ICollectable, IPoolable, IGamblingItem
    {
        [SerializeField] private Transform body;
        private bool alreadyCollect;
        private Sequence animSequence;

        private void Awake()
        {
            StartAnimation();
        }

        public GameObject Collect()
        {
            alreadyCollect = true;
            animSequence?.Kill();
            return gameObject;
        }

        public bool CanCollect()
        {
            return !alreadyCollect;
        }

        public void OnSpawn()
        {
            alreadyCollect = false;
            StartAnimation();
        }

        public void OnDespawn()
        {
            alreadyCollect = false;
            animSequence?.Kill();
        }

        private void StartAnimation()
        {
            animSequence?.Kill();
            
            animSequence = DOTween.Sequence();
            
            // Подпрыгивание вверх-вниз
            animSequence.Append(body.DOMoveY(body.position.y + 0.3f, 0.5f).SetEase(Ease.InOutSine));
            animSequence.Append(body.DOMoveY(body.position.y, 0.5f).SetEase(Ease.InOutSine));
            
            // Покачивание (поворот влево-вправо)
            animSequence.Join(body.DORotate(new Vector3(0, 0, 15f), 0.4f).SetEase(Ease.InOutSine));
            animSequence.Append(body.DORotate(new Vector3(0, 0, -15f), 0.8f).SetEase(Ease.InOutSine));
            animSequence.Append(body.DORotate(Vector3.zero, 0.4f).SetEase(Ease.InOutSine));
            
            // Масштабирование (пульсация)
            animSequence.Join(body.DOScale(1.1f, 0.6f).SetEase(Ease.InOutSine));
            animSequence.Append(body.DOScale(1f, 0.6f).SetEase(Ease.InOutSine));
            
            animSequence.SetLoops(-1, LoopType.Restart);
        }

        private void OnDestroy()
        {
            animSequence?.Kill();
        }
    }

    public interface IGamblingItem
    {
    }
}