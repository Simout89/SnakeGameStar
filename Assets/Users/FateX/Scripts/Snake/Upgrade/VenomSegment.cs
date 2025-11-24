using System;
using UnityEngine;
using Users.FateX.Scripts.Combat;

namespace Users.FateX.Scripts.Upgrade
{
    public class VenomSegment: CombatSnakeSegment
    {
        [SerializeField] private TriggerDetector _triggerDetector;

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

        private void HandleEntered(Collider2D obj)
        {
            if (obj.TryGetComponent(out IDamageable damageable))
            {
                DamageOverTime.StartDot(damageable, this, CurrentStats.DelayBetweenShots, new DamageInfo(CurrentStats.Damage, upgradeLevelsData.SegmentName));
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