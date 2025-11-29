using System;
using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/Shop/ShopProductData")]
    public class ShopProductData: ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
    }

    [Serializable]
    public class ShopProductProgress
    {
        public string Name;
        public int CurrentLevel;
        // public bool IsPurchased;

        public ShopProductProgress(ShopProductData shopProductData)
        {
            Name = shopProductData.Name;
        }
    }
}