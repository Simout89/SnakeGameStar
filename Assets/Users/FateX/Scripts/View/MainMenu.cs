using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.View
{
    public class MainMenu: MonoBehaviour
    {
        [Inject] private CurrencyService _currencyService;
        [Inject] private ISaveLoadService _saveLoadService;
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        public void OnPlayClick()
        {
            SceneManager.LoadScene((int)Scenes.Gameplay);
        }

        public void ResetSave()
        {
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Denied);

            _saveLoadService.ClearAllData();
            _currencyService.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void PlaySelectSound()
        {
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Select);
        }
    }
}