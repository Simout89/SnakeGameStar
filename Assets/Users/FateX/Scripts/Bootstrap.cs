using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using Users.FateX.Scripts.Data;
using Zenject;
using Скриптерсы.Services;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine.UnityConsent;

namespace Users.FateX.Scripts
{
    public class Bootstrap : IInitializable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private ISaveLoadService _saveLoadService;
        
        public async void Initialize()
        {
            // 1. Ждём загрузки локализации
            await InitializeLocalization();

            await UnityServices.InitializeAsync();
            
            EndUserConsent.SetConsentState(new ConsentState 
            {
                AnalyticsIntent = ConsentStatus.Granted,
                AdsIntent = ConsentStatus.Denied
            });            
            // 2. Загружаем конфиг
            _gameConfig.SetConfig(Resources.LoadAll<GameConfigData>("Data")[0]);
            
            // 3. Загружаем сохранения
            _saveLoadService.LoadAchievements();
            _saveLoadService.LoadSegments();
            
            // 4. Загружаем звуковой банк
            _gameConfig.GameConfigData.SoundsData.Bank.Load();
            
            // 5. Переходим в главное меню только после полной инициализации
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }

        private async Task InitializeLocalization()
        {
            try
            {
                // Ждём, пока система локализации полностью инициализируется
                await LocalizationSettings.InitializationOperation.Task;
                
                Debug.Log("Localization loaded successfully");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Localization loading failed: {ex.Message}");
                // Можно установить язык по умолчанию или показать ошибку
            }
        }
    }
}