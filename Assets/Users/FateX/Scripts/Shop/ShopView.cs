using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts.Shop
{
    public class ShopView: MonoBehaviour
    {
        [SerializeField] private ShopSlotEntry[] shopSlotEntries;
        private int count = 0;
        
        public void AddProduct(StatsShopProduct statsShopProduct, out ShopSlotEntry shopSlotEntry)
        {
            shopSlotEntries[count].Init(statsShopProduct);
            shopSlotEntry = shopSlotEntries[count];
            count++;
        }
    }
}