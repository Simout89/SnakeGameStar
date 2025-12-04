using System;
using UnityEngine;
using Users.FateX.Scripts.Combat;

namespace Users.FateX.Scripts.Upgrade
{
    public class VenomSegment : CombatSnakeSegment
    {
        [SerializeField] private TriggerDetector _triggerDetector;
        private Vector3 originScale;

        private void Awake()
        {
            originScale = _triggerDetector.transform.localScale;
        }

        private void OnEnable()
        {
            _triggerDetector.onTriggerEntered += HandleEntered;
            _triggerDetector.onTriggerExited += HandleExited;
        }

        private void OnDisable()
        {
            _triggerDetector.onTriggerEntered -= HandleEntered;
            _triggerDetector.onTriggerExited -= HandleExited;
        }

        public override void Upgrade()
        {
            base.Upgrade();

            _triggerDetector.transform.localScale = originScale * CurrentStats.AttackRange;
        }

        private void HandleEntered(Collider2D obj)
        {
            if (obj.TryGetComponent(out IDamageable damageable))
            {
                DamageOverTime.StartDot(damageable, this, CurrentStats.DelayBetweenShots,
                    new DamageInfo(CurrentStats.Damage + SnakeController.PlayerStats.Damage.Sum,
                        upgradeLevelsData.SegmentName, Body.transform.position));
            }
        }

        private void HandleExited(Collider2D obj)
        {
            if (obj.TryGetComponent(out IDamageable damageable))
            {
                DamageOverTime.StopDot(damageable, this);
            }
        }
    }
}