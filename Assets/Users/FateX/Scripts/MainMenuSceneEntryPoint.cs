using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace Users.FateX.Scripts
{
    public class MainMenuSceneEntryPoint: IInitializable
    {
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        [Inject] private GameConfig _gameConfig;
        public void Initialize()
        {
            _gameConfig.GameConfigData.SoundsData.Bank.Load();

            
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.Music.StopGameMusic);
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.Music.PlayMainMusic);


            
            Debug.Log(321);
            
            Time.timeScale = 1;
        }
    }
}