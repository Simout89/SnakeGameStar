using Users.FateX.Scripts.Data;
using Zenject;

namespace Users.FateX.Scripts.Shop
{
    public class ShopController: IInitializable
    {
        [Inject] private GameConfig gameConfig; 
        
        private ShopModel _shopModel;
        
        public void Initialize()
        {
            _shopModel = new ShopModel(gameConfig.GameConfigData.ShopProductDatas);
        }

        public void OnBuyButtonClicked(ShopProductData shopProductData)
        {
            bool success = _shopModel.Buy(shopProductData);

            if (success)
            {
                if (shopProductData is StatsShopProduct statsShopProduct)
                {
                    statsShopProduct.GetStatsUpgradeByLevel(_shopModel.GetProductProgress(shopProductData).CurrentLevel);
                    
                }
            }
        }
    }
}