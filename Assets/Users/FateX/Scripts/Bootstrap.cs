using UnityEngine;
using UnityEngine.SceneManagement;
using Users.FateX.Scripts.Data;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class Bootstrap: IInitializable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private ISaveLoadService _saveLoadService;
        
        public void Initialize()
        {
            _gameConfig.SetConfig(Resources.LoadAll<GameConfigData>("Data")[0]);
            
            _saveLoadService.LoadAchievements();
            _saveLoadService.LoadSegments();
            
            _gameConfig.GameConfigData.SoundsData.Bank.Load();
            
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
    }
}