using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts.Shop
{
    public class ShopView: MonoBehaviour
    {
        [SerializeField] private ShopSlotEntry[] shopSlotEntries;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text price;
        [field: SerializeField] public Button BuyButton { get; private set; }
        private int count = 0;
        
        public void AddProduct(StatsShopProduct statsShopProduct, out ShopSlotEntry shopSlotEntry)
        {
            shopSlotEntries[count].Init(statsShopProduct);
            shopSlotEntry = shopSlotEntries[count];
            count++;
        }

        public void SetDescription(string title, string description, string price)
        {
            BuyButton.gameObject.SetActive(true);
            this.title.text = title;
            this.description.text = description;
            this.price.text = price + "$";
        }
    }
}