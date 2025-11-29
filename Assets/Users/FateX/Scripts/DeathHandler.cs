using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine.SceneManagement;

namespace Users.FateX.Scripts
{
    public class DeathHandler: IDisposable
    {
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
            DOTween.KillAll();
            
            LeanPool.DespawnAll();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}