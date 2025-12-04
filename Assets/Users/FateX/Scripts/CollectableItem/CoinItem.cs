using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.CollectableItem
{
    public class CoinItem : MonoBehaviour, ICollectable, ICoin, IPoolable
    {
        [HideInInspector] public bool alreadyCollect;

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

        public void OnSpawn()
        {
            alreadyCollect = false;
        }

        public void OnDespawn()
        {
            alreadyCollect = false;
        }
    }

    public interface ICoin
    {
        public int CoinAmount { get; }
    }
}