using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Users.FateX.Scripts.Services;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.View
{
    public class PauseMenu: MonoBehaviour
    {
        [SerializeField] private GameObject pauseBody;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private IInputService _inputService;
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        private void OnEnable()
        {
            _inputService.InputSystemActions.Player.Escape.performed += HandlePerformed;
        }

        private void HandlePerformed(InputAction.CallbackContext obj)
        {
            if (_gameStateManager.CurrentState != GameStates.Pause)
            {
                OnPauseClick();
            }
            
        }
        
        public void PlaySelectSound()
        {
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.UiSound.Select);
        }

        private void OnDisable()
        {
            _inputService.InputSystemActions.Player.Escape.performed -= HandlePerformed;
        }
        
        public void OnPauseClick()
        {
            pauseBody.SetActive(true);
            _gameStateManager.PushState(GameStates.Pause);
        }

        public void OnResumeClick()
        {
            pauseBody.SetActive(false);
            _gameStateManager.PopState();
        }

        public void OnMainMenuClick()
        {
            _gameStateManager.PopState();
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
    }
}