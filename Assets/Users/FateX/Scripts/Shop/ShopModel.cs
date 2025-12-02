using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Shop;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.Shop
{
    public class ShopModel
    {
        private StatsShopProduct[] _shopProductDatas;
        private ShopProductProgress[] _shopProductProgress;
        private ISaveLoadService _saveService;

        public ShopModelPosition[] ShopModelPositions { get; private set; }

        public ShopModel(StatsShopProduct[] shopProductDatas, ISaveLoadService saveService)
        {
            _shopProductDatas = shopProductDatas;
            _saveService = saveService;

            _shopProductProgress = new ShopProductProgress[_shopProductDatas.Length];
            ShopModelPositions = new ShopModelPosition[shopProductDatas.Length];

            LoadProgress();

            for (int i = 0; i < _shopProductDatas.Length; i++)
            {
                ShopModelPositions[i] = new ShopModelPosition(i, _shopProductProgress[i], shopProductDatas[i]);
            }
        }

        private void LoadProgress()
        {
            ShopSaveData saveData = _saveService.LoadShopData();

            for (int i = 0; i < _shopProductDatas.Length; i++)
            {
                _shopProductProgress[i] = new ShopProductProgress(_shopProductDatas[i]);

                // Ищем сохраненный прогресс
                var savedProgress = saveData.ProductsProgress.Find(p => p.Name == _shopProductDatas[i].Name);
                if (savedProgress != null)
                {
                    _shopProductProgress[i].CurrentLevel = savedProgress.CurrentLevel;
                }
            }
        }

        public void SaveProgress()
        {
            ShopSaveData saveData = new ShopSaveData();
            saveData.ProductsProgress.AddRange(_shopProductProgress);
            _saveService.SaveShopData(saveData);
        }

        public bool Buy(ShopProductData shopProductData)
        {
            foreach (var shopProduct in _shopProductProgress)
            {
                if (shopProduct.Name == shopProductData.Name)
                {
                    shopProduct.CurrentLevel++;
                    SaveProgress();
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