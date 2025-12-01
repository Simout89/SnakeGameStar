using DG.Tweening;
using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.CollectableItem
{
    public class HealableItem: MonoBehaviour, ICollectable, IHealableItem, IPoolable
    {
        private bool alreadyCollect;
        
        public GameObject Collect()
        {
            transform.DOKill();
            
            alreadyCollect = true;
            
            return gameObject;
        }

        public bool CanCollect()
        {
            return !alreadyCollect;
        }

        public void SetValue(float value)
        {
            Value = value;
        }


        public float Value { get; private set; } = 2f;
        public void OnSpawn()
        {
            alreadyCollect = false;
        }

        public void OnDespawn()
        {
            alreadyCollect = false;
        }
    }

    public interface IHealableItem
    {
        public float Value { get; }
    }
}