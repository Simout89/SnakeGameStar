using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Upgrade;

namespace Users.FateX.Scripts
{
    public class SnakeInteraction: MonoBehaviour
    {
        private List<SnakeSegmentBase> snakeSegmentBases = new List<SnakeSegmentBase>();
        public event Action<GameObject> OnCollect;
        
        public void Add(SnakeSegmentBase snakeSegmentBase)
        {
            snakeSegmentBases.Add(snakeSegmentBase);
            snakeSegmentBase.CollectableTrigger.onTriggerEntered += HandleEntered;
        }

        private void OnDisable()
        {
            foreach (var snakeSegmentBase in snakeSegmentBases)
            {
                snakeSegmentBase.CollectableTrigger.onTriggerEntered -= HandleEntered;
            }
        }

        private void HandleEntered(Collider2D obj)
        {
            if (obj.TryGetComponent(out ICollectable collectable))
            {
                if (collectable.CanCollect())
                {
                    OnCollect?.Invoke(collectable.Collect());
                }
            }
        }
    }
}