using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.CollectableItem
{
    public class XpItem: MonoBehaviour, ICollectable, IExperiencePoints, IPoolable
    {
        private bool alreadyCollect;
        
        public GameObject Collect()
        {
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


        public float Value { get; private set; } = 0.1f;
        public void OnSpawn()
        {
            alreadyCollect = false;
        }

        public void OnDespawn()
        {
            alreadyCollect = false;
        }
    }

    public interface IExperiencePoints
    {
        public float Value { get; }
    }
    
    public interface ICollectable
    {
        public GameObject Collect();
        public bool CanCollect();
    }
}