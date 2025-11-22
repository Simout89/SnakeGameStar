using System;
using Lean.Pool;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.CollectableItem
{
    public class CollectableHandler: IDisposable
    {
        [Inject] private ExperienceSystem _experienceSystem;
        
        private SnakeInteraction _snakeInteraction;
        
        public void SetSnakeInteraction(SnakeInteraction snakeInteraction)
        {
            _snakeInteraction = snakeInteraction;
            
            snakeInteraction.OnCollect += HandleCollect;
        }

        public void Dispose()
        {
            _snakeInteraction.OnCollect -= HandleCollect;
        }

        private void HandleCollect(GameObject obj)
        {
            if (obj.TryGetComponent(out IExperiencePoints experiencePoints))
            {
                _experienceSystem.AddExperiencePoints(experiencePoints);
            }
            
            LeanPool.Despawn(obj);
        }
        
    }
}