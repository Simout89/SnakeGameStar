using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.CollectableItem
{
    public class MagnetItem : MonoBehaviour, ICollectable, IMagnet, IPoolable
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

        public void OnSpawn()
        {
            alreadyCollect = false;
        }

        public void OnDespawn()
        {
            alreadyCollect = false;
        }
    }

    public interface IMagnet
    {
    }
}