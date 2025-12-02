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
        public void OnPlayClick()
        {
            SceneManager.LoadScene((int)Scenes.Gameplay);
        }

        public void ResetSave()
        {
            _saveLoadService.ClearAllData();
            _currencyService.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}