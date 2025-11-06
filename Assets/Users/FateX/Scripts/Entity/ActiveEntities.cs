using System.Collections.Generic;
using Users.FateX.Scripts.Entity;

namespace Users.FateX.Scripts
{
    public class ActiveEntities
    {
        private List<IEntity> _entities = new List<IEntity>();

        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            _entities.Remove(entity);
        }
    }
}
