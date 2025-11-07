using System;
using UnityEngine;

namespace Users.FateX.Scripts.Entity
{
    public class Entity: MonoBehaviour, ICollectable, IEntity
    {
        public virtual void Collect()
        {
            OnCollect?.Invoke();
        }

        public event Action OnCollect;
    }
}