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
        
        public void SpawnXp(Vector3 position)
        {
            var newXp = LeanPool.Spawn(_gameConfig.GameConfigData.XpPrefab);
            newXp.transform.position = position;
            _itemManager.AddXpItem(newXp);
        }
        
        public void SpawnXp(Vector3 position, XpItem xpItem)
        {
            var newXp = LeanPool.Spawn(xpItem);
            newXp.transform.position = position;
            _itemManager.AddXpItem(newXp);
        }

        public void SpawnCoin(Vector3 position)
        {
            var newCoin = LeanPool.Spawn(_gameConfig.GameConfigData.CoinItemPrefab);
            newCoin.transform.position = position;
        }
        
        public void SpawnMagnet(Vector3 position)
        {
            var newMagnet = LeanPool.Spawn(_gameConfig.GameConfigData.MagnetPrefab);
            newMagnet.transform.position = position;
        }
    }
}