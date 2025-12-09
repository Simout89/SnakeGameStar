using UnityEngine;
using Users.FateX.Scripts.Data;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.Shop
{
    public class ShopController : IInitializable
    {
        [Inject] private GameConfig gameConfig;
        [InjectOptional] private ShopView _shopView;
        [Inject] private ICurrencyService _currencyService;
        [Inject] private PlayerStats _playerStats;
        [Inject] private ISaveLoadService _saveService;
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;


        private ShopModel _shopModel;
        private int selectedProductIndex;

        public void Initialize()
        {
            _shopModel = new ShopModel(gameConfig.GameConfigData.StatsShopProducts, _saveService);
            var allProducts = _shopModel.GetAllProducts();

            // Применяем эффекты всех купленных улучшений
            for (int i = 0; i < allProducts.Length; i++)
            {
                var progress = _shopModel.ShopModelPositions[i].ShopProductProgress;
                var product = _shopModel.ShopModelPositions[i].StatsShopProduct;

                // Применяем эффекты для всех купленных уровней
                for (int level = 0; level < progress.CurrentLevel; level++)
                {
                    ApplyUpgradeEffect(product, level);
                }
            }

            // UI-часть только если ShopView есть
            if (_shopView != null)
            {
                InitializeUI(allProducts);
            }
        }

        private void InitializeUI(StatsShopProduct[] allProducts)
        {
            for (int i = 0; i < allProducts.Length; i++)
            {
                _shopView.AddProduct(allProducts[i], out ShopSlotEntry shopSlotEntry);
                _shopModel.ShopModelPositions[i].ShopSlotEntry = shopSlotEntry;

                var progress = _shopModel.ShopModelPositions[i].ShopProductProgress;

                // Добавляем визуальные индикаторы
                for (int level = 0; level < progress.CurrentLevel; level++)
                {
                    shopSlotEntry.AddLight();
                }

                int index = i;
                shopSlotEntry.Button.onClick.AddListener(() => OnProductClicked(index));
            }

            _shopView.BuyButton.onClick.AddListener(HandleBuyButtonClicked);
        }

        private void OnProductClicked(int index)
        {
            if (_shopView == null) return;
            
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Select);


            selectedProductIndex = index;
            var product = _shopModel.ShopModelPositions[index];
            int currentLevel = product.ShopProductProgress.CurrentLevel;
            int maxLevel = product.StatsShopProduct.StatsUpgradeLevels.Length;
            
            if (currentLevel < 0 || currentLevel >= maxLevel)
            {
                currentLevel = Mathf.Clamp(currentLevel, 0, maxLevel - 1);
            }

            _shopView.SetDescription(
                product.StatsShopProduct.Name, 
                product.StatsShopProduct.Desription,
                product.StatsShopProduct.StatsUpgradeLevels[currentLevel].Cost.ToString()
            );
        }

        private void ApplyUpgradeEffect(StatsShopProduct product, int level)
        {
            if (level < product.StatsUpgradeLevels.Length)
            {
                _playerStats.CoinDropChance.AddAdditional(product.Name,
                    product.StatsUpgradeLevels[level].GoldDropChance);
                _playerStats.Rerolls.AddAdditional(product.Name, product.StatsUpgradeLevels[level].Reroll);
                _playerStats.Exile.AddAdditional(product.Name, product.StatsUpgradeLevels[level].Exile);
                _playerStats.Health.AddAdditional(product.Name, product.StatsUpgradeLevels[level].Health);
                _playerStats.Damage.AddAdditional(product.Name, product.StatsUpgradeLevels[level].Damage);
                _playerStats.ProjectileCount.AddAdditional(product.Name,
                    product.StatsUpgradeLevels[level].ProjectileCount);
                _playerStats.PickUpRange.AddAdditional(product.Name, product.StatsUpgradeLevels[level].PickUpRange);
                _playerStats.MoveSpeed.AddAdditional(product.Name, product.StatsUpgradeLevels[level].MoveSpeed);
            }
        }

        public void HandleBuyButtonClicked()
        {
            if (_shopView == null) return;

            var product = _shopModel.ShopModelPositions[selectedProductIndex];
            var progress = product.ShopProductProgress;

            if (product.StatsShopProduct.StatsUpgradeLevels.Length <= progress.CurrentLevel)
            {
                Debug.Log("Макс лвл");
                _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Denied);

                
                return;
            }

            var cost = product.StatsShopProduct.StatsUpgradeLevels[progress.CurrentLevel].Cost;
            if (_currencyService.TrySpendCoins((int)cost))
            {
                Debug.Log("Денег хватает");
                _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Buy);

                ApplyUpgradeEffect(product.StatsShopProduct, progress.CurrentLevel);

                product.ShopSlotEntry.AddLight();
                progress.CurrentLevel++;
                OnProductClicked(selectedProductIndex);
                _shopModel.SaveProgress();
            }
            else
            {
                Debug.Log("Денег не хватает");
                
                _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Denied);

            }
        }
    }
}