using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.CollectableItem;

namespace Users.FateX.Scripts
{
    public class ExperienceFactory
    {
        private XpItem xpItemPrefab;

        public void SetPrefab(XpItem xpItem)
        {
            xpItemPrefab = xpItem;
        }
        
        public void SpawnXp(Vector3 position)
        {
            var newXp = LeanPool.Spawn(xpItemPrefab);
            newXp.transform.position = position;
        }
    }
}