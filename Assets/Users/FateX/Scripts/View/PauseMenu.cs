using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.View
{
    public class PauseMenu: MonoBehaviour
    {
        [SerializeField] private GameObject pauseBody;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private IInputService _inputService;
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

        private void OnDisable()
        {
            _inputService.InputSystemActions.Player.Escape.performed -= HandlePerformed;
        }
        
        public void OnPauseClick()
        {
            pauseBody.SetActive(true);
            _gameStateManager.ChangeState(GameStates.Pause);
        }

        public void OnResumeClick()
        {
            pauseBody.SetActive(false);
            _gameStateManager.ChangeState(_gameStateManager.PreviousState);
        }

        public void OnMainMenuClick()
        {
            _gameStateManager.ChangeState(GameStates.Play);
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
    }
}