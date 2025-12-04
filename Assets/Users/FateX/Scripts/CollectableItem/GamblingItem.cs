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
        private Vector3 initialLocalPosition;

        private void Awake()
        {
            initialLocalPosition = body.localPosition;
            StartAnimation();
        }

        public GameObject Collect()
        {
            alreadyCollect = true;
            animSequence?.Complete();
            return gameObject;
        }

        public bool CanCollect()
        {
            return !alreadyCollect;
        }

        public void OnSpawn()
        {
            alreadyCollect = false;
            body.localPosition = initialLocalPosition; // Сброс позиции
            body.localRotation = Quaternion.identity;   // Сброс поворота
            body.localScale = Vector3.one;              // Сброс масштаба
            StartAnimation();
        }

        public void OnDespawn()
        {
            alreadyCollect = false;
            animSequence?.Kill(); // Kill() вместо Complete()
        }

        private void StartAnimation()
        {
            animSequence?.Kill();
            
            float startY = initialLocalPosition.y;
            
            animSequence = DOTween.Sequence();
            
            // Подпрыгивание (относительно начальной позиции)
            animSequence.Append(body.DOLocalMoveY(startY + 0.3f, 0.5f).SetEase(Ease.InOutSine));
            animSequence.Append(body.DOLocalMoveY(startY, 0.5f).SetEase(Ease.InOutSine));
            
            // Покачивание
            animSequence.Join(body.DOLocalRotate(new Vector3(0, 0, 15f), 0.4f).SetEase(Ease.InOutSine));
            animSequence.Append(body.DOLocalRotate(new Vector3(0, 0, -15f), 0.8f).SetEase(Ease.InOutSine));
            animSequence.Append(body.DOLocalRotate(Vector3.zero, 0.4f).SetEase(Ease.InOutSine));
            
            // Масштабирование
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