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
        [Inject] private GameConfig _gameConfig;
        private string CurrencySavePath => Path.Combine(Application.persistentDataPath, "currency_save.json");
        private string ShopSavePath => Path.Combine(Application.persistentDataPath, "shop_save.json");
        private string AchievementSavePath => Path.Combine(Application.persistentDataPath, "achievement_save.json");
        private string SegmentsSavePath => Path.Combine(Application.persistentDataPath, "segments_save.json");
        private Dictionary<string, AchievementEntry> achievementEntries;
        private List<SnakeSegmentEntry> _snakeSegmentEntries;


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

        public void LoadAchievements()
        {
            if (File.Exists(AchievementSavePath))
            {
                string json = File.ReadAllText(AchievementSavePath);

                List<AchievementSaveData> achievementSaveDatas =
                    JsonConvert.DeserializeObject<List<AchievementSaveData>>(json);

                Dictionary<string, AchievementEntry> achievementEntries = new();
                foreach (var achievementSaveData in achievementSaveDatas)
                {
                    var achievementData =
                        _gameConfig.GameConfigData.AchievementDatas.FirstOrDefault(a => a.Id == achievementSaveData.Id);

                    achievementEntries.Add(achievementSaveData.Id,
                        new AchievementEntry(achievementData, achievementSaveData));
                }

                this.achievementEntries = achievementEntries;
            }
            else
            {
                Dictionary<string, AchievementEntry> achievementEntries = new();
                foreach (var achievementSaveData in _gameConfig.GameConfigData.AchievementDatas)
                {
                    achievementEntries.Add(achievementSaveData.Id,
                        new AchievementEntry(achievementSaveData, new AchievementSaveData(achievementSaveData)));
                }

                this.achievementEntries = achievementEntries;
            }
        }

        public void SaveAchievements(Dictionary<string, AchievementEntry> achievementEntries)
        {
            List<AchievementSaveData> achievementSaveDatas = new List<AchievementSaveData>();

            foreach (var achievement in achievementEntries)
            {
                achievementSaveDatas.Add(achievement.Value.AchievementSaveData);
            }

            string json = JsonConvert.SerializeObject(achievementSaveDatas, Formatting.Indented);
            File.WriteAllText(AchievementSavePath, json);

            Debug.Log("Достижения сохранены");
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
            if (File.Exists(SegmentsSavePath))
            {
                string json = File.ReadAllText(SegmentsSavePath);

                Debug.Log(json);

                List<SnakeSegmentSaveData> snakeSegmentSaveData =
                    JsonConvert.DeserializeObject<List<SnakeSegmentSaveData>>(json);

                Debug.Log(snakeSegmentSaveData.Count);

                List<SnakeSegmentEntry> snakeSegmentEntries = new();

                foreach (var VARIABLE in snakeSegmentSaveData)
                {
                    Debug.LogError(VARIABLE.Id);
                }

                foreach (var segment in snakeSegmentSaveData)
                {
                    Debug.Log(segment.Id);

                    //CardData segmentData = new();
                    //
                    //foreach (var VARIABLE in _gameConfig.GameConfigData.CardDatas)
                    //{
                    //    if (VARIABLE.Id == segment.Id)
                    //    {
                    //        segmentData = VARIABLE;
                    //    }
                    //}

                    var segmentData =
                        _gameConfig.GameConfigData.CardDatas.FirstOrDefault(a => a.Id.SequenceEqual(segment.Id));

                    snakeSegmentEntries.Add(new SnakeSegmentEntry(segmentData, segment));
                }

                this._snakeSegmentEntries = snakeSegmentEntries;
            }
            else
            {
                List<SnakeSegmentEntry> snakeSegmentEntries = new();
                foreach (var cardData in _gameConfig.GameConfigData.CardDatas)
                {
                    if (cardData.CardType != CardType.Segment)
                        continue;

                    snakeSegmentEntries.Add(new SnakeSegmentEntry(cardData, new SnakeSegmentSaveData(cardData)));
                }

                this._snakeSegmentEntries = snakeSegmentEntries;
            }
        }

        public void SaveSegments(List<SnakeSegmentEntry> snakeSegmentEntries)
        {
            List<SnakeSegmentSaveData> segmentSaveDatas = new List<SnakeSegmentSaveData>();

            foreach (var segment in snakeSegmentEntries)
            {
                segmentSaveDatas.Add(segment.SnakeSegmentSaveData);
            }

            string json = JsonConvert.SerializeObject(segmentSaveDatas, Formatting.Indented);
            File.WriteAllText(SegmentsSavePath, json);

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
    }
}