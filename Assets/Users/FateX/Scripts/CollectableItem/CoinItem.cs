using UnityEngine;

namespace Users.FateX.Scripts.CollectableItem
{
    public class CoinItem : MonoBehaviour, ICollectable, ICoin
    {
        private bool alreadyCollect;

        public int CoinAmount { get; private set; } = 1;

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

    public interface ICoin
    {
        public int CoinAmount { get; }
    }
}