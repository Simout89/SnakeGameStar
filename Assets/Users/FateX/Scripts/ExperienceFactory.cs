using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.CollectableItem;
using Zenject;

namespace Users.FateX.Scripts
{
    public class ExperienceFactory
    {
        [Inject] private ItemManager _itemManager;
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
    }
}