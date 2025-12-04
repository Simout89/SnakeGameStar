using DG.Tweening;
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
        
        public void SpawnXpWithArc(Vector3 startPos, Vector3 endPos)
        {
            var newXp = LeanPool.Spawn(_gameConfig.GameConfigData.XpPrefab, startPos, Quaternion.identity);
            newXp.alreadyCollect = true;
            newXp.transform.DOJump(endPos, 1 + Random.Range(-0.1f, 0.1f), 1, 1 + Random.Range(-0.1f, 0.1f)).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                newXp.alreadyCollect = false;
            });
        }

        public void SpawnCoin(Vector3 position)
        {
            var newCoin = LeanPool.Spawn(_gameConfig.GameConfigData.CoinItemPrefab);
            newCoin.transform.position = position;
        }
        
        public void SpawnCoinWithArc(Vector3 startPos, Vector3 endPos)
        {
            var newCoin = LeanPool.Spawn(_gameConfig.GameConfigData.CoinItemPrefab, startPos, Quaternion.identity);
            newCoin.alreadyCollect = true;
            newCoin.transform.DOJump(endPos, 1 + Random.Range(-0.1f, 0.1f), 1, 1 + Random.Range(-0.1f, 0.1f)).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                newCoin.alreadyCollect = false;
            });
        }
        
        public void SpawnGamblingItem(Vector3 position)
        {
            var newCoin = LeanPool.Spawn(_gameConfig.GameConfigData.GamblingItemPrefab);
            newCoin.transform.position = position;
        }
        
        public void SpawnMagnet(Vector3 position)
        {
            var newMagnet = LeanPool.Spawn(_gameConfig.GameConfigData.MagnetPrefab);
            newMagnet.transform.position = position;
        }
        
        public void SpawnMagnetWitchArc(Vector3 startPos, Vector3 endPos)
        {
            var newMagnet = LeanPool.Spawn(_gameConfig.GameConfigData.MagnetPrefab, startPos, Quaternion.identity);
            newMagnet.alreadyCollect = true;
            newMagnet.transform.DOJump(endPos, 1 + Random.Range(-0.1f, 0.1f), 1, 1 + Random.Range(-0.1f, 0.1f)).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                newMagnet.alreadyCollect = false;
            });
        }
        
        public void SpawnAppleWitchArc(Vector3 startPos, Vector3 endPos)
        {
            var newApple = LeanPool.Spawn(_gameConfig.GameConfigData.HealableItemPrefab, startPos, Quaternion.identity);
            newApple.alreadyCollect = true;
            newApple.transform.DOJump(endPos, 1 + Random.Range(-0.1f, 0.1f), 1, 1 + Random.Range(-0.1f, 0.1f)).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                newApple.alreadyCollect = false;
            });
        }
    }
}