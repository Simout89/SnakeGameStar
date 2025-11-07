using Lean.Pool;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.Entity
{
    public class EntitySpawner
    {
        [Inject] private ActiveEntities _activeEntities;
        public void Spawn(Entity entity, Vector2 pos)
        {
            Entity newEntity = LeanPool.Spawn(entity);

            newEntity.transform.position = pos;
            
            _activeEntities.AddEntity(newEntity);
        }
    }
}