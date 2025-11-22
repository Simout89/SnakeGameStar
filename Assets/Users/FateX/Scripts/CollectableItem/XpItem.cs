using UnityEngine;

namespace Users.FateX.Scripts.CollectableItem
{
    public class XpItem: MonoBehaviour, ICollectable, IExperiencePoints
    {
        public GameObject Collect()
        {
            return gameObject;
        }

        public float Value { get; private set; } = 1;
    }

    public interface IExperiencePoints
    {
        public float Value { get; }
    }
    
    public interface ICollectable
    {
        public GameObject Collect();
    }
}