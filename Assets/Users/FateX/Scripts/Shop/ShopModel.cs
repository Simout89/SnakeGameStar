using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts.Shop
{
    public class ShopModel
    {
        private StatsShopProduct[] _shopProductDatas;
        private ShopProductProgress[] _shopProductProgress;
        
        public ShopModelPosition[] ShopModelPositions { get; private set; }
        public ShopModel(StatsShopProduct[] shopProductDatas)
        {
            _shopProductDatas = shopProductDatas;

            _shopProductProgress = new ShopProductProgress[_shopProductDatas.Length];

            ShopModelPositions = new ShopModelPosition[shopProductDatas.Length];

            for (int i = 0; i < _shopProductDatas.Length; i++)
            {
                _shopProductProgress[i] = new ShopProductProgress(_shopProductDatas[i]);
                ShopModelPositions[i] = new ShopModelPosition(i, new ShopProductProgress(_shopProductDatas[i]), shopProductDatas[i]);
            }
        }

        public bool Buy(ShopProductData shopProductData)
        {
            foreach (var shopProduct in _shopProductProgress)
            {
                if (shopProduct.Name == shopProductData.Name)
                {
                    shopProduct.CurrentLevel++;

                    return true;
                }
            }

            return false;
        }

        public ShopProductProgress GetProductProgress(ShopProductData shopProductData)
        {
            foreach (var shopProduct in _shopProductProgress)
            {
                if (shopProduct.Name == shopProductData.Name)
                {
                    shopProduct.CurrentLevel++;

                    return shopProduct;
                }
            }

            return null;
        }

        public StatsShopProduct[] GetAllProducts()
        {
            return _shopProductDatas;
        }
    }
    
    public class ShopModelPosition
    {
        public int Index;
        public ShopProductProgress ShopProductProgress;
        public StatsShopProduct StatsShopProduct;
        public ShopSlotEntry ShopSlotEntry;

        public ShopModelPosition(int index, ShopProductProgress shopProductProgress, StatsShopProduct statsShopProduct)
        {
            Index = index;
            ShopProductProgress = shopProductProgress;
            StatsShopProduct = statsShopProduct;
        }
    }
}