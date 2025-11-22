using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Users.FateX.Scripts.Combat;

namespace Users.FateX.Scripts.Upgrade
{
    public class FireTrailSegment: CombatSnakeSegment
    {
        [Header("References")]
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private EdgeCollider2D _edgeCollider2D;
        [SerializeField] private TriggerDetector _edgeColliderTriggerDetector;
        
        public override void Attack()
        {
            base.Attack();
            
            
        }

        public override void Tick()
        {
            base.Tick();

            var lastTransform = GetLastSegmentTransform();
            if(_trailRenderer.transform.parent != lastTransform)
            {
                _trailRenderer.transform.parent = lastTransform;
                _trailRenderer.transform.position = lastTransform.position;
            }

            Vector3[] positions = new Vector3[_trailRenderer.positionCount];
            _trailRenderer.GetPositions(positions);

            List<Vector2> points2D = new List<Vector2>();
    
            for (int i = 0; i < positions.Length; i += 7)
            {
                Vector3 localPos = transform.InverseTransformPoint(positions[i]);
                points2D.Add(new Vector2(localPos.x, localPos.y));
            }
            
            _edgeCollider2D.points = points2D.ToArray();
        }

        private void OnEnable()
        {
            _edgeColliderTriggerDetector.onTriggerEntered += HandleEntered;
            _edgeColliderTriggerDetector.onTriggerExited += HandleExited;
        }
        
        private void OnDisable()
        {
            _edgeColliderTriggerDetector.onTriggerEntered -= HandleEntered;
            _edgeColliderTriggerDetector.onTriggerExited -= HandleExited;
        }

        private void HandleEntered(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                DamageOverTime.StartDot(damageable,this, CurrentStats.DelayBetweenShots, new DamageInfo(CurrentStats.Damage));
            }
        }
        
        private void HandleExited(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                DamageOverTime.StopDot(damageable, this);
            }
        }

        private Transform GetLastSegmentTransform()
        {
            var snakeSegmentBase = SnakeController.SegmentsBase.Last();
            return snakeSegmentBase.Body;
        }
    }
}