using UnityEngine;

namespace Users.FateX.Scripts.CollectableItem
{
    public class MagnetItem : MonoBehaviour, ICollectable, IMagnet
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
    }

    public interface IMagnet
    {
    }
}