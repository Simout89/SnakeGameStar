using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Data;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.Shop
{
    public class ShopController : IInitializable
    {
        [Inject] private GameConfig gameConfig;
        [Inject] private ShopView _shopView;
        [Inject] private ICurrencyService _currencyService;
        [Inject] private PlayerStats _playerStats;

        private ShopModel _shopModel;

        public void Initialize()
        {
            _shopModel = new ShopModel(gameConfig.GameConfigData.StatsShopProducts);

            var allProducts = _shopModel.GetAllProducts();

            for (int i = 0; i < allProducts.Length; i++)
            {
                _shopView.AddProduct(allProducts[i], out ShopSlotEntry shopSlotEntry);
                _shopModel.ShopModelPositions[i].ShopSlotEntry = shopSlotEntry;
                int index = i;
                shopSlotEntry.Button.onClick.AddListener(() => OnBuyButtonClicked(index));
            }
        }

        public void OnBuyButtonClicked(int index)
        {

            var product = _shopModel.ShopModelPositions[index];
            var progress = product.ShopProductProgress;

            if (product.StatsShopProduct.StatsUpgradeLevels.Length <= progress.CurrentLevel)
            {
                Debug.Log("Макс лвл");
                return;
            }
            
            Debug.Log(product.StatsShopProduct.Name);

            var cost = product.StatsShopProduct.StatsUpgradeLevels[progress.CurrentLevel].Cost;
            if (_currencyService.TrySpendCoins((int)cost))
            {
                Debug.Log("Денег хватает");
                _playerStats.CoinDropChance += product.StatsShopProduct.StatsUpgradeLevels[progress.CurrentLevel].GoldDropChance;
                product.ShopSlotEntry.AddLight();
                
                progress.CurrentLevel++;
            }
            else
            {
                Debug.Log("Денег не хватает");
            }


            //bool success = _shopModel.Buy(shopProductData);
//
            //if (success)
            //{
            //    if (shopProductData is StatsShopProduct statsShopProduct)
            //    {
            //        statsShopProduct.GetStatsUpgradeByLevel(_shopModel.GetProductProgress(shopProductData)
            //            .CurrentLevel);
            //    }
            //}
        }
    }
}