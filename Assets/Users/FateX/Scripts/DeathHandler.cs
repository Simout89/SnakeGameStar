using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine.SceneManagement;
using Users.FateX.Scripts.View;
using Zenject;

namespace Users.FateX.Scripts
{
    public class DeathHandler: IDisposable
    {
        [Inject] private DeathView _deathView;
        [Inject] private GameStateManager _gameStateManager;
        private SnakeHealth _snakeHealth;
        
        public void SetSnakeHealth(SnakeHealth snakeHealth)
        {
            _snakeHealth = snakeHealth;
            _snakeHealth.OnDie += HandleDie;
        }
        
        public void Dispose()
        {
            _snakeHealth.OnDie -= HandleDie;
        }

        private void HandleDie()
        {
            _deathView.Show();
            
            _gameStateManager.ChangeState(GameStates.Death);
            
            DOTween.KillAll();
            
            LeanPool.DespawnAll();
            
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}