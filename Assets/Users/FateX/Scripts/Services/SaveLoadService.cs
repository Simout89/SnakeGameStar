using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Users.FateX.Scripts.Achievements;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Shop;
using Users.FateX.Scripts.Utils;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.Services
{
    public class SaveLoadService : ISaveLoadService
    {
        [Inject] private SnakeSegmentsRepository _snakeSegmentsRepository;
        [Inject] private GameConfig _gameConfig;

        // ===== FILE PATHS (NON-WEBGL) =====
        private string CurrencySavePath => Path.Combine(Application.persistentDataPath, "currency_save.json");
        private string ShopSavePath => Path.Combine(Application.persistentDataPath, "shop_save.json");
        private string AchievementSavePath => Path.Combine(Application.persistentDataPath, "achievement_save.json");
        private string SegmentsSavePath => Path.Combine(Application.persistentDataPath, "segments_save.json");
        private string SettingsSavePath => Path.Combine(Application.persistentDataPath, "settings_save.json");

        // ===== WEBGL KEYS =====
        private const string CurrencyKey = "currency_save";
        private const string ShopKey = "shop_save";
        private const string AchievementKey = "achievement_save";
        private const string SegmentsKey = "segments_save";
        private const string SettingsKey = "settings_save";

        private Dictionary<string, AchievementEntry> achievementEntries;
        private List<SnakeSegmentEntry> _snakeSegmentEntries;

        // ================= CURRENCY =================

        public PlayerCurrencyData LoadCurrencyData()
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            string json = WebLocalStorage.Get(CurrencyKey);
            if (string.IsNullOrEmpty(json))
                return new PlayerCurrencyData(0);

            return JsonUtility.FromJson<PlayerCurrencyData>(json);
#else
            if (!File.Exists(CurrencySavePath))
                return new PlayerCurrencyData(0);

            return JsonUtility.FromJson<PlayerCurrencyData>(File.ReadAllText(CurrencySavePath));
#endif
        }

        public void SaveCurrencyData(PlayerCurrencyData data)
        {
            string json = JsonUtility.ToJson(data, true);

#if UNITY_WEBGL && !UNITY_EDITOR
            WebLocalStorage.Set(CurrencyKey, json);
#else
            File.WriteAllText(CurrencySavePath, json);
#endif
        }

        public void ClearCurrencyData()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WebLocalStorage.Delete(CurrencyKey);
#else
            if (File.Exists(CurrencySavePath))
                File.Delete(CurrencySavePath);
#endif
        }

        // ================= SHOP =================

        public ShopSaveData LoadShopData()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string json = WebLocalStorage.Get(ShopKey);
            return string.IsNullOrEmpty(json) ? new ShopSaveData() : JsonUtility.FromJson<ShopSaveData>(json);
#else
            if (!File.Exists(ShopSavePath))
                return new ShopSaveData();

            return JsonUtility.FromJson<ShopSaveData>(File.ReadAllText(ShopSavePath));
#endif
        }

        public void SaveShopData(ShopSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);

#if UNITY_WEBGL && !UNITY_EDITOR
            WebLocalStorage.Set(ShopKey, json);
#else
            File.WriteAllText(ShopSavePath, json);
#endif
        }

        public void ClearShopData()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WebLocalStorage.Delete(ShopKey);
#else
            if (File.Exists(ShopSavePath))
                File.Delete(ShopSavePath);
#endif
        }

        // ================= ACHIEVEMENTS =================

        public void LoadAchievements()
        {
            achievementEntries = new();

            foreach (var data in _gameConfig.GameConfigData.AchievementDatas)
            {
                achievementEntries.Add(
                    data.Id,
                    new AchievementEntry(data, new AchievementSaveData(data))
                );
            }

#if UNITY_WEBGL && !UNITY_EDITOR
            string json = WebLocalStorage.Get(AchievementKey);
#else
            string json = File.Exists(AchievementSavePath) ? File.ReadAllText(AchievementSavePath) : null;
#endif
            if (string.IsNullOrEmpty(json))
                return;

            try
            {
                var saved = JsonConvert.DeserializeObject<List<AchievementSaveData>>(json);

                foreach (var save in saved)
                {
                    if (!achievementEntries.ContainsKey(save.Id))
                        continue;

                    var cfg = _gameConfig.GameConfigData.AchievementDatas.First(a => a.Id == save.Id);
                    achievementEntries[save.Id] = new AchievementEntry(cfg, save);
                }
            }
            catch
            {
                Debug.LogWarning("Achievements save corrupted, defaults used");
            }
        }

        public void SaveAchievements(Dictionary<string, AchievementEntry> entries)
        {
            var list = entries.Values.Select(e => e.AchievementSaveData).ToList();
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);

#if UNITY_WEBGL && !UNITY_EDITOR
            WebLocalStorage.Set(AchievementKey, json);
#else
            File.WriteAllText(AchievementSavePath, json);
#endif
        }

        public Dictionary<string, AchievementEntry> GetAchievements()
        {
            if (achievementEntries == null)
                LoadAchievements();

            return achievementEntries;
        }

        // ================= SEGMENTS =================

        public void LoadSegments()
        {
            _snakeSegmentEntries = new();

            foreach (var card in _gameConfig.GameConfigData.CardDatas)
            {
                if (card.CardType != CardType.Segment)
                    continue;

                _snakeSegmentEntries.Add(
                    new SnakeSegmentEntry(card, new SnakeSegmentSaveData(card))
                );
            }

#if UNITY_WEBGL && !UNITY_EDITOR
            string json = WebLocalStorage.Get(SegmentsKey);
#else
            string json = File.Exists(SegmentsSavePath) ? File.ReadAllText(SegmentsSavePath) : null;
#endif
            if (string.IsNullOrEmpty(json))
                return;

            try
            {
                var saved = JsonConvert.DeserializeObject<List<SnakeSegmentSaveData>>(json);

                foreach (var save in saved)
                {
                    var entry = _snakeSegmentEntries.FirstOrDefault(e => e.CardData.Id.SequenceEqual(save.Id));
                    if (entry == null) continue;

                    int index = _snakeSegmentEntries.IndexOf(entry);
                    var cfg = _gameConfig.GameConfigData.CardDatas.First(c => c.Id.SequenceEqual(save.Id));
                    _snakeSegmentEntries[index] = new SnakeSegmentEntry(cfg, save);
                }
            }
            catch
            {
                Debug.LogWarning("Segments save corrupted, defaults used");
            }
        }

        public void SaveSegments(List<SnakeSegmentEntry> entries)
        {
            var list = entries.Select(e => e.SnakeSegmentSaveData).ToList();
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);

#if UNITY_WEBGL && !UNITY_EDITOR
            WebLocalStorage.Set(SegmentsKey, json);
#else
            File.WriteAllText(SegmentsSavePath, json);
#endif
        }

        public List<SnakeSegmentEntry> GetSegmentEntries()
        {
            if (_snakeSegmentEntries == null)
                LoadSegments();

            return _snakeSegmentEntries;
        }

        // ================= SETTINGS =================

        public SettingsSaveData LoadSettings()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string json = WebLocalStorage.Get(SettingsKey);
            return string.IsNullOrEmpty(json)
                ? new SettingsSaveData()
                : JsonConvert.DeserializeObject<SettingsSaveData>(json);
#else
            if (!File.Exists(SettingsSavePath))
                return new SettingsSaveData();

            return JsonConvert.DeserializeObject<SettingsSaveData>(File.ReadAllText(SettingsSavePath));
#endif
        }

        public void SaveSettings(SettingsSaveData data)
        {
            string json = JsonConvert.SerializeObject(data);

#if UNITY_WEBGL && !UNITY_EDITOR
            WebLocalStorage.Set(SettingsKey, json);
#else
            File.WriteAllText(SettingsSavePath, json);
#endif
        }

        // ================= CLEAR ALL =================

        public void ClearAllData()
        {
            ClearCurrencyData();
            ClearShopData();

#if UNITY_WEBGL && !UNITY_EDITOR
            WebLocalStorage.Delete(AchievementKey);
            WebLocalStorage.Delete(SegmentsKey);
            WebLocalStorage.Delete(SettingsKey);
#else
            if (File.Exists(AchievementSavePath)) File.Delete(AchievementSavePath);
            if (File.Exists(SegmentsSavePath)) File.Delete(SegmentsSavePath);
            if (File.Exists(SettingsSavePath)) File.Delete(SettingsSavePath);
#endif

            achievementEntries = null;
            _snakeSegmentEntries = null;
            _snakeSegmentsRepository.ClearData();
        }
    }

    public interface ISaveLoadService
    {
        PlayerCurrencyData LoadCurrencyData();
        void SaveCurrencyData(PlayerCurrencyData data);
        void ClearCurrencyData();

        ShopSaveData LoadShopData();
        void SaveShopData(ShopSaveData data);
        void ClearShopData();

        void ClearAllData();

        void LoadAchievements();
        void SaveAchievements(Dictionary<string, AchievementEntry> achievementEntries);
        Dictionary<string, AchievementEntry> GetAchievements();

        void LoadSegments();
        void SaveSegments(List<SnakeSegmentEntry> snakeSegmentEntries);
        List<SnakeSegmentEntry> GetSegmentEntries();

        SettingsSaveData LoadSettings();
        void SaveSettings(SettingsSaveData settingsSaveData);
    }
}
