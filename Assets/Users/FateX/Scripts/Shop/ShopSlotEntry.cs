using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts.Shop
{
    public class ShopSlotEntry: MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private SlotForUpgrade[] slotsForUpgrade;
        [field: SerializeField] public Button Button { get; private set; }
        private int buyCount = 0;
        private StatsShopProduct _statsShopProduct;

        public void Init(StatsShopProduct statsShopProduct)
        {
            _tmpText.text = statsShopProduct.LocalizedName.GetLocalizedString();
            image.sprite = statsShopProduct.Icon;

            for (int i = 0; i < statsShopProduct.StatsUpgradeLevels.Length; i++)
            {
                slotsForUpgrade[i].Slot.SetActive(true);
            }

            _statsShopProduct = statsShopProduct;
            _statsShopProduct.LocalizedName.StringChanged += Changed;
        }

        private void OnDestroy()
        {
            if(_statsShopProduct != null)
                _statsShopProduct.LocalizedName.StringChanged -= Changed;
        }

        private void Changed(string value)
        {
            _tmpText.text = value;
        }

        public void AddLight()
        {
            slotsForUpgrade[buyCount].Light.SetActive(true);
            buyCount++;
        }
    }
    
    [Serializable]
    public class SlotForUpgrade
    {
        public GameObject Slot;
        public GameObject Light;
    }
}