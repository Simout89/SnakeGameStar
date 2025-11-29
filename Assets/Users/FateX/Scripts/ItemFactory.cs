using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.CollectableItem;
using Zenject;

namespace Users.FateX.Scripts
{
    public class ItemFactory
    {
        [Inject] private ItemManager _itemManager;

        [Inject] private GameConfig _gameConfig;
        private XpItem xpItemPrefab;

        public void SetPrefab(XpItem xpItem)
        {
            xpItemPrefab = xpItem;
        }
        
        public void SpawnXp(Vector3 position)
        {
            var newXp = LeanPool.Spawn(xpItemPrefab);
            newXp.transform.position = position;
            _itemManager.AddXpItem(newXp);
        }

        public void SpawnCoin(Vector3 position)
        {
            var newCoin = LeanPool.Spawn(_gameConfig.GameConfigData.CoinItemPrefab);
            newCoin.transform.position = position;
        }
    }
}