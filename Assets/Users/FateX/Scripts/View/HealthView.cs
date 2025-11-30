using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class HealthView: MonoBehaviour
    {
        [SerializeField] private Image _image;

        private SnakeHealth _snakeHealth;
        
        private void OnDestroy()
        {
            _snakeHealth.OnHealthChanged -= HandleHealthChanged;
        }

        public void SetSnakeHealth(SnakeHealth snakeHealth)
        {
            _snakeHealth = snakeHealth;
            _snakeHealth.OnHealthChanged += HandleHealthChanged;

        }
        
        private void HandleHealthChanged()
        {
            _image.DOComplete();
            _image.DOFillAmount(_snakeHealth.CurrentHealth / _snakeHealth.MaxHealth, 0.1f);
        }
    }
}