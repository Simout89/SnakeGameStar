using System;
using Lean.Pool;
using UnityEngine;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.CollectableItem
{
    public class CollectableHandler: IDisposable
    {
        [Inject] private ExperienceSystem _experienceSystem;
        [Inject] private ItemManager _itemManager;
        [Inject] private ICurrencyService _currencyService;
        [Inject] private GameContext _gameContext;
        
        private SnakeInteraction _snakeInteraction;
        
        public void SetSnakeInteraction(SnakeInteraction snakeInteraction)
        {
            _snakeInteraction = snakeInteraction;
            
            snakeInteraction.OnCollect += HandleCollect;
        }

        public void Dispose()
        {
            _snakeInteraction.OnCollect -= HandleCollect;
        }

        private void HandleCollect(GameObject obj)
        {
            if (obj.TryGetComponent(out IExperiencePoints experiencePoints))
            {
                _experienceSystem.AddExperiencePoints(experiencePoints);
                
                _itemManager.RemoveXpItem(obj.GetComponent<XpItem>());
            }
            
            if (obj.TryGetComponent(out IMagnet magnet ))
            {
                foreach (var xpItem in _itemManager.GetXpItemsArray())
                {
                    HomingMover.StartMove(xpItem.transform, _snakeInteraction.transform, () =>
                    {
                        LeanPool.Despawn(obj);
                    });
                    
                    _itemManager.RemoveXpItem(xpItem.GetComponent<XpItem>());
                }
            }

            if (obj.TryGetComponent(out ICoin coin))
            {
                _currencyService.AddCoins(coin.CoinAmount);
            }
            
            if (obj.TryGetComponent(out IHealableItem healable))
            {
                _gameContext.SnakeHealth.Heal(healable.Value);
            }
            
            HomingMover.StartMove(obj.transform, _snakeInteraction.transform, () =>
            {
                LeanPool.Despawn(obj);
            });
            
            // LeanPool.Despawn(obj);
        }
        
    }
}