using UnityEngine;
using System.IO;
using Users.FateX.Scripts.Shop;
using Zenject;

namespace Скриптерсы.Services
{
    public class SaveLoadService: ISaveLoadService
    {
        private string CurrencySavePath => Path.Combine(Application.persistentDataPath, "currency_save.json");
        private string ShopSavePath => Path.Combine(Application.persistentDataPath, "shop_save.json");

        // Методы для валюты
        public PlayerCurrencyData LoadCurrencyData()
        {
            try
            {
                if (File.Exists(CurrencySavePath))
                {
                    string json = File.ReadAllText(CurrencySavePath);
                    return JsonUtility.FromJson<PlayerCurrencyData>(json);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки валюты: {e.Message}");
            }

            return new PlayerCurrencyData(0);
        }

        public void SaveCurrencyData(PlayerCurrencyData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(CurrencySavePath, json);
                Debug.Log($"Валюта сохранена в: {CurrencySavePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка сохранения валюты: {e.Message}");
            }
        }

        public void ClearCurrencyData()
        {
            try
            {
                if (File.Exists(CurrencySavePath))
                {
                    File.Delete(CurrencySavePath);
                    Debug.Log("Данные валюты очищены");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка очистки валюты: {e.Message}");
            }
        }

        // Методы для магазина
        public ShopSaveData LoadShopData()
        {
            try
            {
                if (File.Exists(ShopSavePath))
                {
                    string json = File.ReadAllText(ShopSavePath);
                    return JsonUtility.FromJson<ShopSaveData>(json);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки магазина: {e.Message}");
            }

            return new ShopSaveData();
        }

        public void SaveShopData(ShopSaveData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(ShopSavePath, json);
                Debug.Log($"Магазин сохранен в: {ShopSavePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка сохранения магазина: {e.Message}");
            }
        }

        public void ClearShopData()
        {
            try
            {
                if (File.Exists(ShopSavePath))
                {
                    File.Delete(ShopSavePath);
                    Debug.Log("Данные магазина очищены");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка очистки магазина: {e.Message}");
            }
        }

        public void ClearAllData()
        {
            ClearCurrencyData();
            ClearShopData();
            Debug.Log("Все данные игры очищены");
        }
    }

    public interface ISaveLoadService
    {
        public PlayerCurrencyData LoadCurrencyData();
        public void SaveCurrencyData(PlayerCurrencyData data);
        public void ClearCurrencyData();
        
        public ShopSaveData LoadShopData();
        public void SaveShopData(ShopSaveData data);
        public void ClearShopData();
        
        public void ClearAllData();
    }
}