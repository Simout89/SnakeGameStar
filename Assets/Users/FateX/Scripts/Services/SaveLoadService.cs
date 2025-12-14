using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Achievements;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Shop;
using Zenject;

namespace Скриптерсы.Services
{
    public class SaveLoadService : ISaveLoadService
    {
        [Inject] private SnakeSegmentsRepository _snakeSegmentsRepository;
        [Inject] private GameConfig _gameConfig;
        private string CurrencySavePath => Path.Combine(Application.persistentDataPath, "currency_save.json");
        private string ShopSavePath => Path.Combine(Application.persistentDataPath, "shop_save.json");
        private string AchievementSavePath => Path.Combine(Application.persistentDataPath, "achievement_save.json");
        private string SegmentsSavePath => Path.Combine(Application.persistentDataPath, "segments_save.json");
        private string SettingsSavePath => Path.Combine(Application.persistentDataPath, "settings_save.json");
        private Dictionary<string, AchievementEntry> achievementEntries;
        private List<SnakeSegmentEntry> _snakeSegmentEntries;


        // Методы для валюты
        public PlayerCurrencyData LoadCurrencyData()
        {
            try
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                if (PlayerPrefs.HasKey(CurrencySavePath))
                {
                    string json = PlayerPrefs.GetString(CurrencySavePath);
                    return JsonUtility.FromJson<PlayerCurrencyData>(json);
                }
#else
                if (File.Exists(CurrencySavePath))
                {
                    string json = File.ReadAllText(CurrencySavePath);
                    return JsonUtility.FromJson<PlayerCurrencyData>(json);
                }
#endif
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
#if UNITY_WEBGL && !UNITY_EDITOR
                PlayerPrefs.SetString(CurrencySavePath, json);
                PlayerPrefs.Save();
#else
                File.WriteAllText(CurrencySavePath, json);
#endif
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
#if UNITY_WEBGL && !UNITY_EDITOR
                PlayerPrefs.DeleteKey(CurrencySavePath);
                PlayerPrefs.Save();
#else
                if (File.Exists(CurrencySavePath))
                {
                    File.Delete(CurrencySavePath);
                }
#endif
                Debug.Log("Данные валюты очищены");
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
#if UNITY_WEBGL && !UNITY_EDITOR
                if (PlayerPrefs.HasKey(ShopSavePath))
                {
                    string json = PlayerPrefs.GetString(ShopSavePath);
                    return JsonUtility.FromJson<ShopSaveData>(json);
                }
#else
                if (File.Exists(ShopSavePath))
                {
                    string json = File.ReadAllText(ShopSavePath);
                    return JsonUtility.FromJson<ShopSaveData>(json);
                }
#endif
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
#if UNITY_WEBGL && !UNITY_EDITOR
                PlayerPrefs.SetString(ShopSavePath, json);
                PlayerPrefs.Save();
#else
                File.WriteAllText(ShopSavePath, json);
#endif
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
#if UNITY_WEBGL && !UNITY_EDITOR
                PlayerPrefs.DeleteKey(ShopSavePath);
                PlayerPrefs.Save();
#else
                if (File.Exists(ShopSavePath))
                {
                    File.Delete(ShopSavePath);
                }
#endif
                Debug.Log("Данные магазина очищены");
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

#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.DeleteKey(SegmentsSavePath);
            PlayerPrefs.Save();
#else
            File.Delete(SegmentsSavePath);
#endif
            LoadSegments();

#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.DeleteKey(AchievementSavePath);
            PlayerPrefs.Save();
#else
            File.Delete(AchievementSavePath);
#endif
            LoadAchievements();

            _snakeSegmentsRepository.ClearData();

            Debug.Log("Все данные игры очищены");
        }

        public void LoadAchievements()
        {
            Dictionary<string, AchievementEntry> achievementEntries = new();
            
            // Сначала создаем записи для всех достижений из конфига
            foreach (var achievementData in _gameConfig.GameConfigData.AchievementDatas)
            {
                achievementEntries.Add(achievementData.Id,
                    new AchievementEntry(achievementData, new AchievementSaveData(achievementData)));
            }
            
            // Затем перезаписываем данными из сохранения (если есть)
#if UNITY_WEBGL && !UNITY_EDITOR
            if (PlayerPrefs.HasKey(AchievementSavePath))
            {
                string json = PlayerPrefs.GetString(AchievementSavePath);
#else
            if (File.Exists(AchievementSavePath))
            {
                string json = File.ReadAllText(AchievementSavePath);
#endif
                try
                {
                    List<AchievementSaveData> achievementSaveDatas =
                        JsonConvert.DeserializeObject<List<AchievementSaveData>>(json);

                    foreach (var saveData in achievementSaveDatas)
                    {
                        if (achievementEntries.ContainsKey(saveData.Id))
                        {
                            var achievementData = _gameConfig.GameConfigData.AchievementDatas
                                .FirstOrDefault(a => a.Id == saveData.Id);
                            if (achievementData != null)
                            {
                                achievementEntries[saveData.Id] = new AchievementEntry(achievementData, saveData);
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Ошибка при загрузке сохранений достижений: {e.Message}. Используются значения по умолчанию.");
                }
            }
            
            this.achievementEntries = achievementEntries;
        }

        public void SaveAchievements(Dictionary<string, AchievementEntry> achievementEntries)
        {
            List<AchievementSaveData> achievementSaveDatas = new List<AchievementSaveData>();

            foreach (var achievement in achievementEntries)
            {
                achievementSaveDatas.Add(achievement.Value.AchievementSaveData);
            }

            string json = JsonConvert.SerializeObject(achievementSaveDatas, Formatting.Indented);
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.SetString(AchievementSavePath, json);
            PlayerPrefs.Save();
#else
            File.WriteAllText(AchievementSavePath, json);
#endif
        }

        public Dictionary<string, AchievementEntry> GetAchievements()
        {
            if (achievementEntries == null)
            {
                LoadAchievements();
            }

            return achievementEntries;
        }

        public void LoadSegments()
        {
            List<SnakeSegmentEntry> snakeSegmentEntries = new();
            
            // Сначала создаем записи для всех сегментов из конфига
            foreach (var cardData in _gameConfig.GameConfigData.CardDatas)
            {
                if (cardData.CardType != CardType.Segment)
                    continue;

                snakeSegmentEntries.Add(new SnakeSegmentEntry(cardData, new SnakeSegmentSaveData(cardData)));
            }
            
            // Затем перезаписываем данными из сохранения (если есть)
#if UNITY_WEBGL && !UNITY_EDITOR
            if (PlayerPrefs.HasKey(SegmentsSavePath))
            {
                string json = PlayerPrefs.GetString(SegmentsSavePath);
#else
            if (File.Exists(SegmentsSavePath))
            {
                string json = File.ReadAllText(SegmentsSavePath);
#endif
                try
                {
                    Debug.Log(json);

                    List<SnakeSegmentSaveData> snakeSegmentSaveData =
                        JsonConvert.DeserializeObject<List<SnakeSegmentSaveData>>(json);

                    Debug.Log(snakeSegmentSaveData.Count);

                    foreach (var saveData in snakeSegmentSaveData)
                    {
                        // Ищем соответствующий сегмент в уже созданном списке
                        var existingEntry = snakeSegmentEntries.FirstOrDefault(e => e.CardData.Id.SequenceEqual(saveData.Id));
                        
                        if (existingEntry != null)
                        {
                            // Заменяем дефолтную запись на сохраненную
                            int index = snakeSegmentEntries.IndexOf(existingEntry);
                            var segmentData = _gameConfig.GameConfigData.CardDatas
                                .FirstOrDefault(a => a.Id.SequenceEqual(saveData.Id));
                            
                            if (segmentData != null)
                            {
                                snakeSegmentEntries[index] = new SnakeSegmentEntry(segmentData, saveData);
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Ошибка при загрузке сохранений сегментов: {e.Message}. Используются значения по умолчанию.");
                }
            }
            
            this._snakeSegmentEntries = snakeSegmentEntries;
        }

        public void SaveSegments(List<SnakeSegmentEntry> snakeSegmentEntries)
        {
            List<SnakeSegmentSaveData> segmentSaveDatas = new List<SnakeSegmentSaveData>();

            foreach (var segment in snakeSegmentEntries)
            {
                segmentSaveDatas.Add(segment.SnakeSegmentSaveData);
            }

            string json = JsonConvert.SerializeObject(segmentSaveDatas, Formatting.Indented);
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.SetString(SegmentsSavePath, json);
            PlayerPrefs.Save();
#else
            File.WriteAllText(SegmentsSavePath, json);
#endif

            Debug.Log("Сегменты сохранены");
        }

        public List<SnakeSegmentEntry> GetSegmentEntries()
        {
            if (_snakeSegmentEntries == null)
            {
                LoadSegments();
            }

            return _snakeSegmentEntries;
        }

        public SettingsSaveData LoadSettings()
        {
            
#if UNITY_WEBGL && !UNITY_EDITOR
            if (PlayerPrefs.HasKey(SettingsSavePath))
            {
                string json = PlayerPrefs.GetString(SettingsSavePath);
                SettingsSaveData settingsSaveData = JsonConvert.DeserializeObject<SettingsSaveData>(json);
                return settingsSaveData;
            }
            else
            {
                SettingsSaveData settingsSaveData = new();
                return settingsSaveData;
            }
#else
            if (File.Exists(SettingsSavePath))
            {
                string json = File.ReadAllText(SettingsSavePath);
                SettingsSaveData settingsSaveData = JsonConvert.DeserializeObject<SettingsSaveData>(json);
                return settingsSaveData;
            }
            else
            {
                SettingsSaveData settingsSaveData = new();
                return settingsSaveData;
            }
#endif
        }

        public void SaveSettings(SettingsSaveData settingsSaveData)
        {
            string json = JsonConvert.SerializeObject(settingsSaveData);


#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.SetString(SettingsSavePath, json);
            PlayerPrefs.Save();
#else
            File.WriteAllText(SettingsSavePath, json);
#endif
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


        public void LoadAchievements();
        public void SaveAchievements(Dictionary<string, AchievementEntry> achievementEntries);
        public Dictionary<string, AchievementEntry> GetAchievements();

        public void LoadSegments();
        public void SaveSegments(List<SnakeSegmentEntry> snakeSegmentEntries);
        public List<SnakeSegmentEntry> GetSegmentEntries();

        public SettingsSaveData LoadSettings();
        public void SaveSettings(SettingsSaveData settingsSaveData);
    }
}