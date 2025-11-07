using System;

namespace Users.FateX.Scripts.Entity
{
    public interface IEntity
    {
        public event Action OnCollect;
    }
}