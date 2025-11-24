using System;
using DG.Tweening;
using TheBlackCat.TrailEffect2D;
using UnityEngine;

namespace Users.FateX.Scripts.Upgrade
{
    public class SwordSegment: CombatSnakeSegment
    {
        [SerializeField] private Transform[] swords;
        [SerializeField] private TriggerDetector[] swordTriggers;
        [SerializeField] private SpriteRenderer[] swordSprites;

        private Vector3 originScale;
        private Sequence currentSequence;

        private void OnEnable()
        {
            foreach (var swordTrigger in swordTriggers)
            {
                swordTrigger.onTriggerEntered += HandleEntered;
            }
        }

        private void OnDisable()
        {
            foreach (var swordTrigger in swordTriggers)
            {
                swordTrigger.onTriggerEntered -= HandleEntered;
            }
            
            currentSequence?.Kill();
        }

        private void HandleEntered(Collider2D obj)
        {
            if (obj.TryGetComponent(out IDamageable damageable))
            {
                var damageInfo = new DamageInfo(CurrentStats.Damage, upgradeLevelsData.SegmentName);
                
                damageable.TakeDamage(damageInfo);
                
                DealDamage(damageInfo);
            }
        }

        public override void Attack()
        {
            base.Attack();
            
            currentSequence?.Kill();
            
            currentSequence = DOTween.Sequence();

            foreach (var sword in swords)
            {
                sword.gameObject.SetActive(true);
                sword.localScale = Vector3.zero;
                sword.localRotation = Quaternion.Euler(0, 0, 45);
            }

            foreach (var swordSprite in swordSprites)
            {
                TrailManager.Instance.StartTrail(swordSprite.gameObject);
            }

            foreach (var sword in swords)
            {
                currentSequence.Append(sword.DOScale(originScale, 0.1f));
            }
            
            foreach (var sword in swords)
            {
                currentSequence.Append(sword.DOLocalRotate(new Vector3(0,0, -45), 0.2f));
            }

            foreach (var swordSprite in swordSprites)
            {
                currentSequence.Append(swordSprite.DOFade(0f, 0.05f));
            }

            currentSequence.OnComplete(() =>
            {
                foreach (var sword in swords)
                {
                    sword.gameObject.SetActive(false);
                    sword.localScale = Vector3.zero;
                }

                foreach (var swordSprite in swordSprites)
                {
                    TrailManager.Instance.StopTrail(swordSprite.gameObject);
                    var c = swordSprite.color;
                    c.a = 1f;
                    swordSprite.color = c;

                }
                
                currentSequence = null;
            });
        }

        public override void Init(SnakeController snakeController)
        {
            base.Init(snakeController);

            originScale = swords[0].localScale;
            
            foreach (var sword in swords)
            {
                sword.localScale = Vector3.zero;
                sword.gameObject.SetActive(false);
            }
        }
    }
}