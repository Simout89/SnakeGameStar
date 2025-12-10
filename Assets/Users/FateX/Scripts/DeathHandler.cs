using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine.SceneManagement;
using Users.FateX.Scripts.Tutorial;
using Users.FateX.Scripts.View;
using Zenject;

namespace Users.FateX.Scripts
{
    public class DeathHandler: IDisposable
    {
        [Inject] private DeathView _deathView;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        [Inject] private TutorialController _tutorialController;
        [Inject] private SettingsController _settingsController;

        private SnakeHealth _snakeHealth;
        
        public void SetSnakeHealth(SnakeHealth snakeHealth)
        {
            _snakeHealth = snakeHealth;
            _snakeHealth.OnDie += HandleDie;
            _deathView.OnDeathAnimationEnd += HandleDeathAnimationEnd;
        }
        
        public void Dispose()
        {
            _snakeHealth.OnDie -= HandleDie;
            _deathView.OnDeathAnimationEnd -= HandleDeathAnimationEnd;
            
            DOTween.KillAll();
            
            LeanPool.DespawnAll();
        }

        private void HandleDeathAnimationEnd()
        {
            DOTween.KillAll();
            
            LeanPool.DespawnAll();

            if (!_settingsController.SettingsSaveData.DeathTutorial)
            {
                _tutorialController.ShowTutorial(TutorialWindowType.DeathMenu);
                _settingsController.SettingsSaveData.DeathTutorial = true;
            }
        }

        private void HandleDie()
        {
            _deathView.Show();
            
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.SnakeDie);
            
            _gameStateManager.PushState(GameStates.Death);
            
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}