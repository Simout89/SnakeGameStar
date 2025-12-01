using UnityEngine;
using UnityEngine.SceneManagement;
using Users.FateX.Scripts.Data;
using Zenject;

namespace Users.FateX.Scripts
{
    public class Bootstrap: IInitializable
    {
        [Inject] private GameConfig _gameConfig;
        
        public void Initialize()
        {
            _gameConfig.SetConfig(Resources.LoadAll<GameConfigData>("Data")[0]);
            
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
    }
}