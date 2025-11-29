using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts.Shop
{
    public class ShopModel
    {
        private ShopProductData[] _shopProductDatas;
        private ShopProductProgress[] _shopProductProgress;
        public ShopModel(ShopProductData[] shopProductDatas)
        {
            _shopProductDatas = shopProductDatas;

            _shopProductProgress = new ShopProductProgress[_shopProductDatas.Length];

            for (int i = 0; i < _shopProductDatas.Length; i++)
            {
                _shopProductProgress[i] = new ShopProductProgress(_shopProductDatas[i]);
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

        public ShopProductData[] GetAllProducts()
        {
            return _shopProductDatas;
        }
    }
}